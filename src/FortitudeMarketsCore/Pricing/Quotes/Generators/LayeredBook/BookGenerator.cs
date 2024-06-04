// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.Generators.LayeredBook;

public interface IBookGenerator
{
    void PopulateBidAskBooks(IMutableLevel2Quote level2Quote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime);
}

public struct BookGenerationInfo
{
    public BookGenerationInfo() { }

    public LayerType LayerType = LayerType.PriceVolume;

    public decimal Pip = 0.0001m;

    public int VolumeRounding = 10_000;

    public int NumberOfBookLayers = 20;

    public decimal AverageSpreadPips = 0.0001m;

    public decimal AverageLayerPips = 0.1m;

    public int HighestVolumeLayer = 4;

    public double ChangeOnSameProbability = 0.3;

    public long AverageTopOfBookVolume = 300_000;

    public long HighestLayerAverageVolume = 5_000_000;

    public long AverageDeltaVolumePerLayer = 500_000;

    public long MaxVolumeVariance = 4_000_000;

    public decimal MaxPriceLayerPips = 2m;

    public decimal SmallestPriceLayerPips = 0.05m;

    public decimal SpreadStandardDeviation = 0.0005m;

    public decimal TightestSpreadPips = 0.1m;

    public GenerateBookLayerInfo GenerateBookLayerInfo = new();
}

public class BookGenerator : IBookGenerator
{
    private readonly BookGenerationInfo        bookGenerationInfo;
    private readonly PriceVolumeLayerGenerator priceVolumeLayerGenerator;

    private Normal normalDist   = null!;
    private Random pseudoRandom = null!;

    public BookGenerator(BookGenerationInfo bookGenerationInfo)
    {
        this.bookGenerationInfo   = bookGenerationInfo;
        priceVolumeLayerGenerator = new PriceVolumeLayerGenerator(bookGenerationInfo.GenerateBookLayerInfo);
    }

    public void PopulateBidAskBooks(IMutableLevel2Quote level2Quote, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var prev                  = previousCurrentMidPriceTime.PreviousMid;
        var curr                  = previousCurrentMidPriceTime.CurrentMid;
        var timeBasedPseudoRandom = new Random(prev.Time.GetHashCode() ^ curr.Time.GetHashCode());
        if (prev.Mid == curr.Mid && timeBasedPseudoRandom.NextDouble() < bookGenerationInfo.ChangeOnSameProbability)
            pseudoRandom = timeBasedPseudoRandom;
        else
            pseudoRandom = new Random(prev.Mid.GetHashCode() ^ curr.Mid.GetHashCode());

        normalDist                             = new Normal(0, 1, pseudoRandom);
        priceVolumeLayerGenerator.PseudoRandom = pseudoRandom;
        priceVolumeLayerGenerator.NormalDist   = normalDist;
        var topBidAskSpread = Math.Max(bookGenerationInfo.TightestSpreadPips,
                                       bookGenerationInfo.AverageSpreadPips +
                                       (decimal)normalDist.Sample() * bookGenerationInfo.SpreadStandardDeviation);
        var mid           = curr.Mid;
        var roundedTopBid = decimal.Round(mid - topBidAskSpread / 2);
        var roundedTopAsk = decimal.Round(mid + topBidAskSpread / 2);

        while (roundedTopAsk - roundedTopBid > bookGenerationInfo.TightestSpreadPips)
            roundedTopBid -= bookGenerationInfo.SmallestPriceLayerPips;

        PopulateBook(level2Quote.BidBook, roundedTopBid, previousCurrentMidPriceTime);
        PopulateBook(level2Quote.AskBook, roundedTopAsk, previousCurrentMidPriceTime);
    }

    public void PopulateBook(IMutableOrderBook newBook, decimal startingPrice, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var incrementVolToMaxMid = (bookGenerationInfo.HighestLayerAverageVolume - bookGenerationInfo.AverageTopOfBookVolume) /
                                   Math.Max(1, bookGenerationInfo.HighestVolumeLayer);
        var volumeMid          = bookGenerationInfo.AverageTopOfBookVolume;
        var volumeStdDeviation = bookGenerationInfo.MaxVolumeVariance / 3;
        var currLayerPrice     = startingPrice;
        var missedCount        = 0;
        for (var i = 0; i < bookGenerationInfo.NumberOfBookLayers; i++)
        {
            var layerVol = (long)Math.Ceiling((volumeMid + (decimal)normalDist.Sample() * volumeStdDeviation) / bookGenerationInfo.VolumeRounding) *
                           bookGenerationInfo.VolumeRounding;
            if (layerVol > 0)
            {
                var index = newBook.AppendEntryAtEnd();
                var entry = newBook[index]!;
                entry.Price  = currLayerPrice;
                entry.Volume = layerVol;
                SetOtherLayerValues(entry, i, previousCurrentMidPriceTime);
                var priceDelta =
                    Math.Min(bookGenerationInfo.MaxPriceLayerPips,
                             Math.Ceiling(Math.Max(bookGenerationInfo.SmallestPriceLayerPips,
                                                   bookGenerationInfo.AverageLayerPips +
                                                   (decimal)normalDist.Sample() * bookGenerationInfo.AverageLayerPips) /
                                          bookGenerationInfo.SmallestPriceLayerPips) * bookGenerationInfo.SmallestPriceLayerPips) *
                    bookGenerationInfo.Pip;

                currLayerPrice += newBook.BookSide == BookSide.BidBook ? -priceDelta : priceDelta;
            }
            else
            {
                if (++missedCount > 3) break;
            }
            if (i < bookGenerationInfo.HighestVolumeLayer)
                volumeMid += incrementVolToMaxMid;
            else if (i > bookGenerationInfo.HighestVolumeLayer)
                volumeMid -= bookGenerationInfo.AverageDeltaVolumePerLayer;
            else
                volumeMid = bookGenerationInfo.HighestLayerAverageVolume;
        }
    }

    public void SetOtherLayerValues(IPriceVolumeLayer bookLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        priceVolumeLayerGenerator.PopulatePriceVolumeLayer(bookLayer, depth, previousCurrentMidPriceTime);
    }
}
