using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;
using MathNet.Numerics.Distributions;

namespace FortitudeMarketsCore.Pricing.Quotes.Generators.LayeredBook;

public struct GenerateBookLayerInfo
{
    public GenerateBookLayerInfo() { }

    public int MaxNumberOfSourceNames = 12;

    public int[] CandidateValueDateAddDays = [0, 1, 2, 7, 14, 21, 28, 30, 60, 90];

    public bool IsTraderCountOnly = false;

    public int MaxNumberOfUniqueTraderName = 100;

    public int AverageTradersPerLayer = 3;

    public int TradersPerLayerStandardDeviation = 15;

    public double ExecutableProbability = 0.99;
}

public class PriceVolumeLayerGenerator
{
    private readonly GenerateBookLayerInfo generateLayerInfo;

    internal Normal NormalDist   { get; set; } = null!;
    internal Random PseudoRandom { get; set; } = null!;

    public PriceVolumeLayerGenerator(GenerateBookLayerInfo generateLayerInfo)
    {
        this.generateLayerInfo = generateLayerInfo;
    }

    public void PopulatePriceVolumeLayer(IPriceVolumeLayer bookLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var layerType = bookLayer.LayerType;

        switch (layerType)
        {
            case LayerType.None :
            case LayerType.PriceVolume : // already done in BookGenerator
                return;
            case LayerType.ValueDatePriceVolume :
                PopulateValueDate((IMutableValueDatePriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.SourcePriceVolume :
                PopulateSourceName((IMutableSourcePriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.SourceQuoteRefPriceVolume :
                PopulateSourceQuoteRef((IMutableSourceQuoteRefPriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.TraderPriceVolume :
                PopulateTraderPriceVolume((IMutableTraderPriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.SourceQuoteRefTraderValueDatePriceVolume :
                PopulateSourceQuoteRefTraderPriceVolume((IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
        }
    }

    protected virtual void PopulateValueDate(IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, int depth, 
        PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var index = PseudoRandom.Next(0, generateLayerInfo.CandidateValueDateAddDays.Length);
        var date = previousCurrentMidPriceTime.CurrentMid.Time.AddDays(generateLayerInfo.CandidateValueDateAddDays[index]);
        valueDatePriceVolumeLayer.ValueDate = date;
    }

    protected virtual void PopulateSourceName(IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, int depth, 
        PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        sourcePriceVolumeLayer.Executable = PseudoRandom.NextDouble() < generateLayerInfo.ExecutableProbability;
        var sourceNumber = PseudoRandom.Next(1, generateLayerInfo.MaxNumberOfSourceNames + 1);
        sourcePriceVolumeLayer.SourceName = $"SourceName_{sourceNumber}";
    }

    protected virtual void PopulateSourceQuoteRef(IMutableSourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer, int depth, 
        PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        sourceQuoteRefPriceVolumeLayer.SourceQuoteReference = (uint)previousCurrentMidPriceTime.CurrentMid.Time.Ticks & 0xFFFF_FFFF;
        PopulateSourceName(sourceQuoteRefPriceVolumeLayer, depth, previousCurrentMidPriceTime);
    }

    protected virtual void PopulateTraderPriceVolume(IMutableTraderPriceVolumeLayer traderPriceVolumeLayer, int depth, 
        PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var numberOfTradersOnLayer = Math.Min(generateLayerInfo.MaxNumberOfUniqueTraderName,  Math.Max(0, generateLayerInfo.AverageTradersPerLayer + 
                          (int)(NormalDist.Sample() * generateLayerInfo.TradersPerLayerStandardDeviation)));

        if (generateLayerInfo.IsTraderCountOnly)
        {
            traderPriceVolumeLayer.SetTradersCountOnly(numberOfTradersOnLayer);
            return;
        }
        var traderVolume = traderPriceVolumeLayer.Volume / numberOfTradersOnLayer;
        for (int i = 0; i < numberOfTradersOnLayer; i++)
        {
            var traderNum = PseudoRandom.Next(0, generateLayerInfo.MaxNumberOfUniqueTraderName);
            traderPriceVolumeLayer[i]!.TraderName = $"TraderName_{traderNum}";
            traderPriceVolumeLayer[i]!.TraderVolume = traderVolume;
        }
    }

    protected virtual void PopulateSourceQuoteRefTraderPriceVolume(IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer, 
        int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        PopulateTraderPriceVolume(srcQtRefTrdrVlDtPriceVolumeLayer, depth, previousCurrentMidPriceTime);
        PopulateSourceQuoteRef(srcQtRefTrdrVlDtPriceVolumeLayer, depth, previousCurrentMidPriceTime);
        PopulateValueDate(srcQtRefTrdrVlDtPriceVolumeLayer, depth, previousCurrentMidPriceTime);
    }
}