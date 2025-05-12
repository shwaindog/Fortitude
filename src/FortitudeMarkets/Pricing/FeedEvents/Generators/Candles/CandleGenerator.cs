// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Generators.Candles;

public struct GenerateCandlesInfo
{
    public GenerateCandlesInfo
    (ISourceTickerInfo sourceTickerInfo,
        IMidPriceGenerator? midPriceGenerator = null)
    {
        SourceTickerInfo  = sourceTickerInfo;
        MidPriceGenerator = midPriceGenerator ?? new SyntheticRepeatableMidPriceGenerator(1.0m, DateTime.Now);
    }

    public DateTime SingleCandleStartTime = DateTime.Now;

    public TimeBoundaryPeriod CandlePeriod = TimeBoundaryPeriod.OneSecond;

    public double ProbabilityNextStartPriceSameAsLastEndPrice = 0.95;

    public decimal Pip = 0.0001m;

    public decimal AverageSpreadPips = 2.0m;

    public decimal MinimumSpreadPips = 0.2m;

    public decimal SpreadStandardDeviationPips = 1.0m;

    public decimal AverageHighestLowestSpreadPips = 30m;

    public decimal MinimumHighestLowestSpreadPips = 3m;

    public decimal HighestLowestStandardDeviationPips = 50m;

    public long AverageVolume = 5_000_000;

    public long MinimumVolume = 10_000;

    public long VolumeStandardDeviation = 2_000_000;

    public uint AverageTickCount = 3_000;

    public uint TickCountStandardDeviation = 1_000;

    public uint MinimumTickCount = 1;

    public ISourceTickerInfo SourceTickerInfo;

    public IMidPriceGenerator MidPriceGenerator;
}

public interface ICandleGenerator<out TCandle> where TCandle : IMutableCandle
{
    TCandle Next { get; }

    IEnumerable<TCandle> Candles(DateTime startDateTime, TimeBoundaryPeriod candlePeriod, int numToGenerate);
}

public abstract class CandleGenerator<TCandle> : ICandleGenerator<TCandle>
    where TCandle : IMutableCandle
{
    private readonly   TimeBoundaryPeriod         defaultCandlePeriod;
    protected readonly GenerateCandlesInfo GenerateCandleInfo;

    private DateTime nextSingleCandleStartTime;

    protected Normal         NormalDist = null!;
    protected TCandle? PreviousCandle;
    protected Random         PseudoRandom = null!;

    protected CandleGenerator(GenerateCandlesInfo generateCandleInfo)
    {
        GenerateCandleInfo        = generateCandleInfo;
        nextSingleCandleStartTime = generateCandleInfo.SingleCandleStartTime;
        defaultCandlePeriod            = generateCandleInfo.CandlePeriod;
    }

    public TCandle Next
    {
        get
        {
            var prevCurrMid = GenerateCandleInfo.MidPriceGenerator
                                                      .PreviousCurrentPrices(nextSingleCandleStartTime, defaultCandlePeriod, 1)
                                                      .First();
            PseudoRandom = new Random(prevCurrMid.PreviousMid.Mid.GetHashCode() ^ prevCurrMid.CurrentMid.Mid.GetHashCode());
            NormalDist   = new Normal(0, 1, PseudoRandom);

            nextSingleCandleStartTime = defaultCandlePeriod.PeriodEnd(nextSingleCandleStartTime);

            PreviousCandle = Candles(prevCurrMid);
            return PreviousCandle;
        }
    }

    public IEnumerable<TCandle> Candles
        (DateTime startingFromTime, TimeBoundaryPeriod candlePeriod, int numToGenerate)
    {
        foreach (var prevCurrMids in GenerateCandleInfo.MidPriceGenerator
                                                             .PreviousCurrentPrices(startingFromTime, candlePeriod, numToGenerate * 2)
                                                             .Where((_, index) => index % 2 == 0))
        {
            PseudoRandom = new Random(prevCurrMids.PreviousMid.Mid.GetHashCode() ^ prevCurrMids.CurrentMid.Mid.GetHashCode());
            NormalDist   = new Normal(0, 1, PseudoRandom);

            PreviousCandle = Candles(prevCurrMids);
            yield return PreviousCandle;
        }
    }

    public abstract TCandle CreateCandle
        (ISourceTickerInfo sourceTickerInfo, MidPriceTimePair midPriceTimePair);

    public TCandle Candles(MidPriceTimePair midPriceTimePair)
    {
        var     currMid = midPriceTimePair.CurrentMid;
        var     prevMid = midPriceTimePair.PreviousMid;
        decimal startMid;
        decimal startSpread;
        if (PreviousCandle == null || PseudoRandom.NextDouble() > GenerateCandleInfo.ProbabilityNextStartPriceSameAsLastEndPrice)
        {
            startMid = prevMid.Mid;
            startSpread = Math.Max(GenerateCandleInfo.MinimumSpreadPips
                                 , GenerateCandleInfo.AverageSpreadPips * GenerateCandleInfo.Pip + (decimal)NormalDist.Sample() *
                                   GenerateCandleInfo.SpreadStandardDeviationPips * GenerateCandleInfo.Pip);
        }
        else
        {
            startMid    = (PreviousCandle.EndAskPrice + PreviousCandle.EndBidPrice) / 2;
            startSpread = PreviousCandle.EndAskPrice - PreviousCandle.EndBidPrice;
        }

        var halfSpread    = startSpread / 2;
        var startBidPrice = startMid - halfSpread;
        var startAskPrice = startMid + halfSpread;

        halfSpread = Math.Max(GenerateCandleInfo.MinimumSpreadPips
                            , GenerateCandleInfo.AverageSpreadPips * GenerateCandleInfo.Pip + (decimal)NormalDist.Sample() *
                              GenerateCandleInfo.SpreadStandardDeviationPips * GenerateCandleInfo.Pip) / 2;

        var highLowMid = prevMid.Mid;
        var highLowSpread = Math.Max(GenerateCandleInfo.MinimumHighestLowestSpreadPips
                                   , GenerateCandleInfo.AverageHighestLowestSpreadPips * GenerateCandleInfo.Pip +
                                     (decimal)NormalDist.Sample() *
                                     GenerateCandleInfo.HighestLowestStandardDeviationPips * GenerateCandleInfo.Pip);
        var halfHighLowSpread = highLowSpread / 2;
        var highestBidPrice   = highLowMid + halfHighLowSpread - halfSpread;
        var highestAskPrice   = highLowMid + halfHighLowSpread + halfSpread;
        var lowestBidPrice    = highLowMid - halfHighLowSpread - halfSpread;
        var lowestAskPrice    = highLowMid - halfHighLowSpread + halfSpread;

        var endMid      = currMid.Mid;
        var endBidPrice = endMid - halfSpread;
        var endAskPrice = endMid + halfSpread;

        var averageMid      = (prevMid.Mid + currMid.Mid) / 2;
        var averageBidPrice = averageMid - halfSpread;
        var averageAskPrice = averageMid + halfSpread;

        var volume = Math.Max(GenerateCandleInfo.MinimumVolume
                            , (long)(GenerateCandleInfo.AverageVolume +
                                     NormalDist.Sample() * GenerateCandleInfo.VolumeStandardDeviation));

        volume = RoundVolumeToScaleValue(volume);

        var tickCount = Math.Max(GenerateCandleInfo.MinimumTickCount
                               , (uint)(GenerateCandleInfo.AverageTickCount +
                                        NormalDist.Sample() * GenerateCandleInfo.TickCountStandardDeviation));

        var candle = CreateCandle(GenerateCandleInfo.SourceTickerInfo, midPriceTimePair);

        candle.StartBidPrice   = startBidPrice;
        candle.StartAskPrice   = startAskPrice;
        candle.HighestBidPrice = highestBidPrice;
        candle.HighestAskPrice = highestAskPrice;
        candle.LowestBidPrice  = lowestBidPrice;
        candle.LowestAskPrice  = lowestAskPrice;
        candle.EndBidPrice     = endBidPrice;
        candle.EndAskPrice     = endAskPrice;
        candle.AverageBidPrice = averageBidPrice;
        candle.AverageAskPrice = averageAskPrice;
        candle.PeriodVolume    = volume;
        candle.TickCount       = tickCount;

        return candle;
    }

    private long RoundVolumeToScaleValue(long volume)
    {
        int volumeRound;
        var minVolAmounts = Math.Min(GenerateCandleInfo.SourceTickerInfo.IncrementSize
                                   , GenerateCandleInfo.SourceTickerInfo.MinSubmitSize);
        if (minVolAmounts < 1 && minVolAmounts.Scale > 0)
        {
            volumeRound = minVolAmounts.Scale;
        }
        else if (minVolAmounts > 10)
        {
            var count = 1;
            while (minVolAmounts > 10)
            {
                count++;
                minVolAmounts /= 10;
            }
            volumeRound = -count;
        }
        else
        {
            volumeRound = 0;
        }
        volume = volume.Round(volumeRound);
        return volume;
    }
}

public class CandleGenerator : CandleGenerator<Candle>
{
    public CandleGenerator(GenerateCandlesInfo generateCandleInfo) : base(generateCandleInfo) { }

    public override Candle CreateCandle
        (ISourceTickerInfo sourceTickerInfo, MidPriceTimePair midPriceTimePair)
    {
        var currMid = midPriceTimePair.CurrentMid;
        return new Candle(GenerateCandleInfo.CandlePeriod, currMid.Time
                        , GenerateCandleInfo.CandlePeriod.PeriodEnd(currMid.Time));
    }
}
