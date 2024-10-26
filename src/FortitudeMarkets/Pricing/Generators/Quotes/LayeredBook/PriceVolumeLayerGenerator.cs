using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using MathNet.Numerics.Distributions;

namespace FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;

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

public interface IPriceVolumeLayerGenerator
{
    void PopulatePriceVolumeLayer(IPriceVolumeLayer bookLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime);
    Normal NormalDist { get; set; }
    Random PseudoRandom { get; set; }

    DateTime GenerateValueDate(PreviousCurrentMidPriceTime previousCurrentMidPriceTime);
    uint GenerateQuoteRef(PreviousCurrentMidPriceTime previousCurrentMidPriceTime);
}


public class PriceVolumeLayerGenerator : IPriceVolumeLayerGenerator
{
    private readonly GenerateBookLayerInfo generateLayerInfo;

    public Normal NormalDist { get; set; } = null!;
    public Random PseudoRandom { get; set; } = null!;

    public PriceVolumeLayerGenerator(GenerateBookLayerInfo generateLayerInfo)
    {
        this.generateLayerInfo = generateLayerInfo;
    }

    public void PopulatePriceVolumeLayer(IPriceVolumeLayer bookLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var layerType = bookLayer.LayerType;

        switch (layerType)
        {
            case LayerType.None:
            case LayerType.PriceVolume: // already done in BookGenerator
                return;
            case LayerType.ValueDatePriceVolume:
                PopulateValueDate((IMutableValueDatePriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.SourcePriceVolume:
                PopulateSourceName((IMutableSourcePriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.SourceQuoteRefPriceVolume:
                PopulateSourceQuoteRef((IMutableSourceQuoteRefPriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.TraderPriceVolume:
                PopulateTraderPriceVolume((IMutableTraderPriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
            case LayerType.SourceQuoteRefTraderValueDatePriceVolume:
                PopulateSourceQuoteRefTraderPriceVolume((IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)bookLayer, depth, previousCurrentMidPriceTime);
                return;
        }
    }

    protected virtual void PopulateValueDate
    (IMutableValueDatePriceVolumeLayer valueDatePriceVolumeLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var date = GenerateValueDate(previousCurrentMidPriceTime);
        valueDatePriceVolumeLayer.ValueDate = date;
    }

    public DateTime GenerateValueDate(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var index = PseudoRandom.Next(0, generateLayerInfo.CandidateValueDateAddDays.Length);
        var date = previousCurrentMidPriceTime.CurrentMid.Time.AddDays(generateLayerInfo.CandidateValueDateAddDays[index]).Date;
        return date;
    }

    protected virtual void PopulateSourceName(IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, int depth,
        PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        SetExecutable(sourcePriceVolumeLayer, PseudoRandom.NextDouble() < generateLayerInfo.ExecutableProbability);
        var sourceNumber = PseudoRandom.Next(1, generateLayerInfo.MaxNumberOfSourceNames + 1);
        SetSourceName(sourcePriceVolumeLayer, $"SourceName_{sourceNumber}");
    }

    protected virtual void SetExecutable(IMutableSourcePriceVolumeLayer sourcePvL, bool executable)
    {
        sourcePvL.Executable = executable;
    }

    protected virtual void SetSourceName(IMutableSourcePriceVolumeLayer sourcePvL, string name)
    {
        sourcePvL.SourceName = name;
    }

    protected virtual void PopulateSourceQuoteRef
    (IMutableSourceQuoteRefPriceVolumeLayer sourceQuoteRefPriceVolumeLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        sourceQuoteRefPriceVolumeLayer.SourceQuoteReference = GenerateQuoteRef(previousCurrentMidPriceTime);
        PopulateSourceName(sourceQuoteRefPriceVolumeLayer, depth, previousCurrentMidPriceTime);
    }

    public uint GenerateQuoteRef(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        return (uint)(previousCurrentMidPriceTime.CurrentMid.Time.Ticks / TimeSpan.TicksPerMillisecond & 0xFFFF_FFFF);
    }

    protected virtual void PopulateTraderPriceVolume
    (IMutableTraderPriceVolumeLayer traderPriceVolumeLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var numberOfTradersOnLayer = Math.Min(generateLayerInfo.MaxNumberOfUniqueTraderName, Math.Max(0, generateLayerInfo.AverageTradersPerLayer +
                          (int)(NormalDist.Sample() * generateLayerInfo.TradersPerLayerStandardDeviation)));
        if (generateLayerInfo.IsTraderCountOnly)
        {
            traderPriceVolumeLayer.SetTradersCountOnly(numberOfTradersOnLayer);
            return;
        }
        if (numberOfTradersOnLayer == 0) return;
        var traderVolume = traderPriceVolumeLayer.Volume / numberOfTradersOnLayer;
        for (int i = 0; i < numberOfTradersOnLayer; i++)
        {
            var traderNum = PseudoRandom.Next(0, generateLayerInfo.MaxNumberOfUniqueTraderName);
            SetTraderName(traderPriceVolumeLayer[i]!, $"TraderName_{traderNum}");
            traderPriceVolumeLayer[i]!.TraderVolume = traderVolume;
        }
    }

    protected virtual void SetTraderName(IMutableTraderLayerInfo traderLayerInfo, string name)
    {
        traderLayerInfo.TraderName = name;
    }

    protected virtual void PopulateSourceQuoteRefTraderPriceVolume
    (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer, int depth, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        PopulateTraderPriceVolume(srcQtRefTrdrVlDtPriceVolumeLayer, depth, previousCurrentMidPriceTime);
        PopulateSourceQuoteRef(srcQtRefTrdrVlDtPriceVolumeLayer, depth, previousCurrentMidPriceTime);
        PopulateValueDate(srcQtRefTrdrVlDtPriceVolumeLayer, depth, previousCurrentMidPriceTime);
    }
}