// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.TimeSeries;
using FortitudeMarketsCore.Pricing.Generators.MidPrice;
using FortitudeMarketsCore.Pricing.Summaries;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarketsCore.Pricing.Generators.Summaries;

public struct GeneratePriceSummariesInfo
{
    public GeneratePriceSummariesInfo
    (ISourceTickerQuoteInfo sourceTickerQuoteInfo,
        IMidPriceGenerator? midPriceGenerator = null)
    {
        SourceTickerQuoteInfo = sourceTickerQuoteInfo;
        MidPriceGenerator     = midPriceGenerator ?? new SyntheticRepeatableMidPriceGenerator(1.0m, DateTime.Now);
    }

    public DateTime SinglePriceSummaryStartTime = DateTime.Now;

    public TimeSeriesPeriod SummaryPeriod = TimeSeriesPeriod.OneSecond;

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

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo;

    public IMidPriceGenerator MidPriceGenerator;
}

public interface IPricePeriodSummaryGenerator<out TPriceSummary> where TPriceSummary : IMutablePricePeriodSummary
{
    TPriceSummary Next { get; }

    IEnumerable<TPriceSummary> PriceSummaries(DateTime startDateTime, TimeSeriesPeriod priceSummaryPeriod, int numToGenerate);
}

public abstract class PricePeriodSummaryGenerator<TPriceSummary> : IPricePeriodSummaryGenerator<TPriceSummary>
    where TPriceSummary : IMutablePricePeriodSummary
{
    private readonly   TimeSeriesPeriod           defaultSummaryPeriod;
    protected readonly GeneratePriceSummariesInfo GeneratePriceSummaryInfo;

    private DateTime nextSinglePriceSummaryStartTime;

    protected Normal         NormalDist = null!;
    protected TPriceSummary? PreviousPeriodSummary;
    protected Random         PseudoRandom = null!;

    protected PricePeriodSummaryGenerator(GeneratePriceSummariesInfo generatePriceSummaryInfo)
    {
        GeneratePriceSummaryInfo        = generatePriceSummaryInfo;
        nextSinglePriceSummaryStartTime = generatePriceSummaryInfo.SinglePriceSummaryStartTime;
        defaultSummaryPeriod            = generatePriceSummaryInfo.SummaryPeriod;
    }

    public TPriceSummary Next
    {
        get
        {
            var prevCurrMid = GeneratePriceSummaryInfo.MidPriceGenerator
                                                      .PreviousCurrentPrices(nextSinglePriceSummaryStartTime, defaultSummaryPeriod, 1)
                                                      .First();
            PseudoRandom = new Random(prevCurrMid.PreviousMid.Mid.GetHashCode() ^ prevCurrMid.CurrentMid.Mid.GetHashCode());
            NormalDist   = new Normal(0, 1, PseudoRandom);

            nextSinglePriceSummaryStartTime = defaultSummaryPeriod.PeriodEnd(nextSinglePriceSummaryStartTime);

            PreviousPeriodSummary = PriceSummaries(prevCurrMid);
            return PreviousPeriodSummary;
        }
    }

    public IEnumerable<TPriceSummary> PriceSummaries
        (DateTime startingFromTime, TimeSeriesPeriod priceSummaryPeriod, int numToGenerate)
    {
        foreach (var prevCurrMids in GeneratePriceSummaryInfo.MidPriceGenerator
                                                             .PreviousCurrentPrices(startingFromTime, priceSummaryPeriod, numToGenerate * 2)
                                                             .Where((_, index) => index % 2 == 0))
        {
            PseudoRandom = new Random(prevCurrMids.PreviousMid.Mid.GetHashCode() ^ prevCurrMids.CurrentMid.Mid.GetHashCode());
            NormalDist   = new Normal(0, 1, PseudoRandom);

            PreviousPeriodSummary = PriceSummaries(prevCurrMids);
            yield return PreviousPeriodSummary;
        }
    }

    public abstract TPriceSummary CreatePricePeriodSummary
        (ISourceTickerQuoteInfo sourceTickerQuoteInfo, PreviousCurrentMidPriceTime previousCurrentMidPriceTime);

    public TPriceSummary PriceSummaries(PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var     currMid = previousCurrentMidPriceTime.CurrentMid;
        var     prevMid = previousCurrentMidPriceTime.PreviousMid;
        decimal startMid;
        decimal startSpread;
        if (PreviousPeriodSummary == null || PseudoRandom.NextDouble() > GeneratePriceSummaryInfo.ProbabilityNextStartPriceSameAsLastEndPrice)
        {
            startMid = prevMid.Mid;
            startSpread = Math.Max(GeneratePriceSummaryInfo.MinimumSpreadPips
                                 , GeneratePriceSummaryInfo.AverageSpreadPips * GeneratePriceSummaryInfo.Pip + (decimal)NormalDist.Sample() *
                                   GeneratePriceSummaryInfo.SpreadStandardDeviationPips * GeneratePriceSummaryInfo.Pip);
        }
        else
        {
            startMid    = (PreviousPeriodSummary.EndAskPrice + PreviousPeriodSummary.EndBidPrice) / 2;
            startSpread = PreviousPeriodSummary.EndAskPrice - PreviousPeriodSummary.EndBidPrice;
        }

        var halfSpread    = startSpread / 2;
        var startBidPrice = startMid - halfSpread;
        var startAskPrice = startMid + halfSpread;

        halfSpread = Math.Max(GeneratePriceSummaryInfo.MinimumSpreadPips
                            , GeneratePriceSummaryInfo.AverageSpreadPips * GeneratePriceSummaryInfo.Pip + (decimal)NormalDist.Sample() *
                              GeneratePriceSummaryInfo.SpreadStandardDeviationPips * GeneratePriceSummaryInfo.Pip) / 2;

        var highLowMid = prevMid.Mid;
        var highLowSpread = Math.Max(GeneratePriceSummaryInfo.MinimumHighestLowestSpreadPips
                                   , GeneratePriceSummaryInfo.AverageHighestLowestSpreadPips * GeneratePriceSummaryInfo.Pip +
                                     (decimal)NormalDist.Sample() *
                                     GeneratePriceSummaryInfo.HighestLowestStandardDeviationPips * GeneratePriceSummaryInfo.Pip);
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

        var volume = Math.Max(GeneratePriceSummaryInfo.MinimumVolume
                            , (long)(GeneratePriceSummaryInfo.AverageVolume +
                                     NormalDist.Sample() * GeneratePriceSummaryInfo.VolumeStandardDeviation));

        var tickCount = Math.Max(GeneratePriceSummaryInfo.MinimumTickCount
                               , (uint)(GeneratePriceSummaryInfo.AverageTickCount +
                                        NormalDist.Sample() * GeneratePriceSummaryInfo.TickCountStandardDeviation));

        var priceSummary = CreatePricePeriodSummary(GeneratePriceSummaryInfo.SourceTickerQuoteInfo, previousCurrentMidPriceTime);

        priceSummary.StartBidPrice   = startBidPrice;
        priceSummary.StartAskPrice   = startAskPrice;
        priceSummary.HighestBidPrice = highestBidPrice;
        priceSummary.HighestAskPrice = highestAskPrice;
        priceSummary.LowestBidPrice  = lowestBidPrice;
        priceSummary.LowestAskPrice  = lowestAskPrice;
        priceSummary.EndBidPrice     = endBidPrice;
        priceSummary.EndAskPrice     = endAskPrice;
        priceSummary.AverageBidPrice = averageBidPrice;
        priceSummary.AverageAskPrice = averageAskPrice;
        priceSummary.PeriodVolume    = volume;
        priceSummary.TickCount       = tickCount;

        return priceSummary;
    }
}

public class PricePeriodSummaryGenerator : PricePeriodSummaryGenerator<PricePeriodSummary>
{
    public PricePeriodSummaryGenerator(GeneratePriceSummariesInfo generatePriceSummaryInfo) : base(generatePriceSummaryInfo) { }

    public override PricePeriodSummary CreatePricePeriodSummary
        (ISourceTickerQuoteInfo sourceTickerQuoteInfo, PreviousCurrentMidPriceTime previousCurrentMidPriceTime)
    {
        var currMid = previousCurrentMidPriceTime.CurrentMid;
        return new PricePeriodSummary(GeneratePriceSummaryInfo.SummaryPeriod, currMid.Time
                                    , GeneratePriceSummaryInfo.SummaryPeriod.PeriodEnd(currMid.Time));
    }
}
