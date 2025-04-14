// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

public interface IBookGenerator
{
    QuoteBookValuesGenerator QuoteBookGenerator { get; }

    void PopulateBidAskBooks(IMutableLevel2Quote level2Quote, MidPriceTimePair midPriceTimePair);
}

public class BookGenerator : IBookGenerator
{
    private Normal normalDist   = null!;
    private Random pseudoRandom = null!;

    public BookGenerator(QuoteBookValuesGenerator quoteBookGenerator) => QuoteBookGenerator = quoteBookGenerator;

    public QuoteBookValuesGenerator QuoteBookGenerator { get; }

    public void PopulateBidAskBooks(IMutableLevel2Quote level2Quote, MidPriceTimePair midPriceTimePair)
    {
        var (roundedTopBid, roundedTopAsk) = QuoteBookGenerator.TopBidAskPrice;

        var maxLayersToGenerate = Math.Min(level2Quote.SourceTickerInfo!.MaximumPublishedLayers
                                         , QuoteBookGenerator.BookGenerationInfo.NumberOfBookLayers);
        PopulateBook(level2Quote.BidBook, roundedTopBid, maxLayersToGenerate, midPriceTimePair);
        PopulateBook(level2Quote.AskBook, roundedTopAsk, maxLayersToGenerate, midPriceTimePair);
    }

    public virtual void InitializeBook(IMutableOrderBook newBook) { }

    public void PopulateBook
        (IMutableOrderBook newBook, decimal startingPrice, int maxLayersToGenerate, MidPriceTimePair midPriceTimePair)
    {
        InitializeBook(newBook);
        var missedCount = 0;

        var layerIndex = 0;
        for (var i = 0; i < maxLayersToGenerate; i++)
        {
            decimal layerVol;
            if (newBook.BookSide == BookSide.BidBook)
            {
                layerVol = QuoteBookGenerator.BidVolumeAt(i);
                if (layerVol > 0)
                {
                    missedCount = 0;
                    while (newBook.Capacity <= i) newBook.AppendEntryAtEnd();
                    var entry = newBook[layerIndex++]!;
                    entry.Price  = QuoteBookGenerator.BidPriceAt(i);
                    entry.Volume = layerVol;
                    SetBidLayerValues(entry, i, midPriceTimePair);
                }
            }
            else
            {
                layerVol = QuoteBookGenerator.AskVolumeAt(i);
                if (layerVol > 0)
                {
                    missedCount = 0;
                    while (newBook.Capacity <= i) newBook.AppendEntryAtEnd();
                    var entry = newBook[layerIndex++]!;
                    entry.Price  = QuoteBookGenerator.AskPriceAt(i);
                    entry.Volume = layerVol;
                    SetAskLayerValues(entry, i, midPriceTimePair);
                }
            }
            if (layerVol == 0)
            {
                if (QuoteBookGenerator.BookGenerationInfo.AllowEmptySlotGaps) layerIndex++;
                if (++missedCount > 3) break;
            }
        }
    }

    public void SetBidLayerValues(IPriceVolumeLayer bookLayer, int depth, MidPriceTimePair midPriceTimePair)
    {
        PopulatePriceVolumeLayer(bookLayer, depth, BookSide.BidBook);
    }

    public void SetAskLayerValues(IPriceVolumeLayer bookLayer, int depth, MidPriceTimePair midPriceTimePair)
    {
        PopulatePriceVolumeLayer(bookLayer, depth, BookSide.AskBook);
    }

    protected virtual void PopulatePriceVolumeLayer(IPriceVolumeLayer bookLayer, int depth, BookSide side)
    {
        var layerType = bookLayer.LayerType;

        switch (layerType)
        {
            case LayerType.None:
            case LayerType.PriceVolume: // already done in BookGenerator
                return;
            case LayerType.ValueDatePriceVolume:
                PopulateValueDate((IMutableValueDatePriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.SourcePriceVolume:
                PopulateSourceName((IMutableSourcePriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.SourceQuoteRefPriceVolume:
                PopulateSourceQuoteRef((IMutableSourceQuoteRefPriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.TraderPriceVolume:
                PopulateTraderPriceVolume((IMutableTraderPriceVolumeLayer)bookLayer, depth, side);
                return;
            case LayerType.SourceQuoteRefTraderValueDatePriceVolume:
                PopulateSourceQuoteRefTraderPriceVolume((IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)bookLayer, depth, side);
                return;
        }
    }

    protected virtual void PopulateValueDate(IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, int depth, BookSide side)
    {
        var newValueDate = side switch
                           {
                               BookSide.AskBook => QuoteBookGenerator.AskValueDateAt(depth)
                             , BookSide.BidBook => QuoteBookGenerator.BidValueDateAt(depth)
                             , _                => valueDatePriceVolumeLayer.ValueDate
                           };
        var previousValueDate = side switch
                                {
                                    BookSide.AskBook => QuoteBookGenerator.PreviousAskValueDateAt(QuoteBookGenerator.AskPriceAt(depth), depth)
                                  , BookSide.BidBook => QuoteBookGenerator.PreviousBidValueDateAt(QuoteBookGenerator.BidPriceAt(depth), depth)
                                  , _                => null
                                };
        SetValueDate(side, valueDatePriceVolumeLayer, newValueDate, previousValueDate);
    }

    protected virtual void SetValueDate
        (BookSide side, IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, DateTime newValueDate, DateTime? prevValueDate)
    {
        valueDatePriceVolumeLayer.ValueDate = newValueDate;
    }

    protected virtual void PopulateSourceName(IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, int depth, BookSide side)
    {
        switch (side)
        {
            case BookSide.AskBook:
                SetExecutable(side, sourcePriceVolumeLayer, QuoteBookGenerator.AskExecutableAt(depth)
                            , QuoteBookGenerator.PreviousAskExecutableAt(QuoteBookGenerator.AskPriceAt(depth), depth));
                SetSourceName(side, sourcePriceVolumeLayer, QuoteBookGenerator.AskSourceNameAt(depth), QuoteBookGenerator.AskSourceIdAt(depth)
                            , QuoteBookGenerator.PreviousAskSourceIdAt(QuoteBookGenerator.AskPriceAt(depth), depth));
                break;
            case BookSide.BidBook:
                SetExecutable(side, sourcePriceVolumeLayer, QuoteBookGenerator.BidExecutableAt(depth)
                            , QuoteBookGenerator.PreviousBidExecutableAt(QuoteBookGenerator.BidPriceAt(depth), depth));
                SetSourceName(side, sourcePriceVolumeLayer, QuoteBookGenerator.BidSourceNameAt(depth), QuoteBookGenerator.BidSourceIdAt(depth)
                            , QuoteBookGenerator.PreviousBidSourceIdAt(QuoteBookGenerator.BidPriceAt(depth), depth));
                break;
        }
    }

    protected virtual void SetExecutable(BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, bool executable, bool? prevExecutable)
    {
        sourcePriceVolumeLayer.Executable = executable;
    }

    protected virtual void SetSourceName
        (BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, string sourceName, ushort sourceId, ushort? prevSourceId)
    {
        sourcePriceVolumeLayer.SourceName = sourceName;
    }

    protected virtual void PopulateSourceQuoteRef
        (IMutableSourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer, int depth, BookSide side)
    {
        var sourceQtRef =
            side switch
            {
                BookSide.AskBook => QuoteBookGenerator.AskQuoteRefAt(depth)
              , BookSide.BidBook => QuoteBookGenerator.BidQuoteRefAt(depth)
              , _                => sourceQuoteRefPriceVolumeLayer.SourceQuoteReference
            };
        var prevSourceQtRef = side switch
                              {
                                  BookSide.AskBook => QuoteBookGenerator.PreviousAskQuoteRefAt(QuoteBookGenerator.AskPriceAt(depth), depth)
                                , BookSide.BidBook => QuoteBookGenerator.PreviousAskQuoteRefAt(QuoteBookGenerator.BidPriceAt(depth), depth)
                                , _                => null
                              };
        SetSourceQuoteRef(side, sourceQuoteRefPriceVolumeLayer, sourceQtRef, prevSourceQtRef);
        PopulateSourceName(sourceQuoteRefPriceVolumeLayer, depth, side);
    }

    protected virtual void SetSourceQuoteRef
        (BookSide side, IMutableSourceQuoteRefPriceVolumeLayer sourcePriceVolumeLayer, uint sourceQuoteRef, uint? prevQuoteRef)
    {
        sourcePriceVolumeLayer.SourceQuoteReference = sourceQuoteRef;
    }


    protected virtual void PopulateTraderPriceVolume
        (IMutableTraderPriceVolumeLayer traderPriceVolumeLayer, int depth, BookSide side)
    {
        var numberOfTradersOnLayer = side switch
                                     {
                                         BookSide.AskBook => QuoteBookGenerator.AskNumOfTradersAt(depth)
                                       , BookSide.BidBook => QuoteBookGenerator.BidNumOfTradersAt(depth)
                                       , _                => 0
                                     };
        if (QuoteBookGenerator.BookGenerationInfo.GenerateBookLayerInfo.IsTraderCountOnly)
        {
            traderPriceVolumeLayer.SetTradersCountOnly(numberOfTradersOnLayer);
            return;
        }
        if (numberOfTradersOnLayer == 0) return;
        for (var i = 0; i < numberOfTradersOnLayer; i++)
        {
            var traderVolumeLayer = traderPriceVolumeLayer[i]!;
            switch (side)
            {
                case BookSide.AskBook:
                    var askPrice    = QuoteBookGenerator.AskPriceAt(depth);
                    var askTraderId = QuoteBookGenerator.AskTraderIdAt(depth, i, depth);
                    SetTraderName(side, traderVolumeLayer, i, QuoteBookGenerator.AskTraderNameAt(depth, i)
                                , QuoteBookGenerator.PreviousAskTraderNameAt(askPrice, askTraderId, depth));
                    SetTraderVolume(side, traderVolumeLayer, i, QuoteBookGenerator.AskTraderVolumeAt(depth, i)
                                  , QuoteBookGenerator.PreviousAskTraderVolumeAt(askPrice, askTraderId, depth));
                    break;
                case BookSide.BidBook:
                    var bidPrice    = QuoteBookGenerator.BidPriceAt(depth);
                    var bidTraderId = QuoteBookGenerator.BidTraderIdAt(depth, i, depth);
                    SetTraderName(side, traderVolumeLayer, i, QuoteBookGenerator.BidTraderNameAt(depth, i)
                                , QuoteBookGenerator.PreviousBidTraderNameAt(bidPrice, bidTraderId, depth));
                    SetTraderVolume(side, traderVolumeLayer, i, QuoteBookGenerator.BidTraderVolumeAt(depth, i)
                                  , QuoteBookGenerator.PreviousBidTraderVolumeAt(bidPrice, bidTraderId, depth));
                    break;
            }
        }
    }

    protected virtual void SetTraderVolume
        (BookSide side, IMutableTraderLayerInfo traderLayerInfo, int pos, decimal traderVolume, decimal? prevTraderVolume)
    {
        traderLayerInfo.TraderVolume = traderVolume;
    }

    protected virtual void SetTraderName(BookSide side, IMutableTraderLayerInfo traderLayerInfo, int pos, string traderName, string? prevTraderName)
    {
        traderLayerInfo.TraderName = traderName;
    }


    protected virtual void PopulateSourceQuoteRefTraderPriceVolume
    (
        IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer
      , int depth
      , BookSide side
    )
    {
        PopulateTraderPriceVolume(srcQtRefTrdrVlDtPriceVolumeLayer, depth, side);
        PopulateSourceQuoteRef(srcQtRefTrdrVlDtPriceVolumeLayer, depth, side);
        PopulateValueDate(srcQtRefTrdrVlDtPriceVolumeLayer, depth, side);
    }
}
