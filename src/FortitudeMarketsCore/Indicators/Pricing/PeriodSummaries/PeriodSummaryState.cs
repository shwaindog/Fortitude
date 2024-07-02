// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Summaries;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries;

public class PeriodSummaryState : ITimeSeriesPeriodRange
{
    public BidAskPair? NextPeriodBidAskStart;

    public BidAskPair? PreviousPeriodBidAskEnd;

    public IDoublyLinkedList<IPricePeriodSummary> SubSummaryPeriods = new DoublyLinkedList<IPricePeriodSummary>();

    public PeriodSummaryState(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public bool             HasPublishedComplete { get; set; }
    public DateTime         PeriodStartTime      { get; set; }
    public TimeSeriesPeriod TimeSeriesPeriod     { get; set; }

    public BoundedTimeRange ToBoundedTimeRange
        (DateTime? maxDateTime = null) =>
        new(PeriodStartTime, TimeSeriesPeriod.PeriodEnd(PeriodStartTime).Min(maxDateTime));

    public void Configure(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public IPricePeriodSummary BuildPeriodSummary(IRecycler recycler, DateTime? atTime = null)
    {
        var toPopulate = recycler.Borrow<PricePeriodSummary>();
        toPopulate.TimeSeriesPeriod = TimeSeriesPeriod;
        toPopulate.PeriodStartTime  = PeriodStartTime;
        toPopulate.PeriodEndTime    = this.PeriodEnd();

        var runningTimeWeightedBidAverage = 0m;
        var runningTimeWeightedAskAverage = 0m;
        if (this is { PreviousPeriodBidAskEnd: not null, SubSummaryPeriods.Head: QuoteWrappingPricePeriodSummary quoteSummary })
        {
            var periodToQuoteMs = (decimal)(quoteSummary.PeriodStartTime - PeriodStartTime).TotalMilliseconds;
            runningTimeWeightedBidAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.BidPrice;
            runningTimeWeightedAskAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.AskPrice;
        }
        var currentPeriodSummary = SubSummaryPeriods.Head;
        while (currentPeriodSummary != null)
        {
            var currentPeriodBoundedMs = (decimal)currentPeriodSummary.ToBoundedTimeRange(toPopulate.PeriodEndTime).TimeSpan().TotalMilliseconds;

            runningTimeWeightedBidAverage += currentPeriodBoundedMs * currentPeriodSummary.AverageBidAsk.BidPrice;
            runningTimeWeightedAskAverage += currentPeriodBoundedMs * currentPeriodSummary.AverageBidAsk.AskPrice;

            toPopulate.Merge(currentPeriodSummary);
            currentPeriodSummary = currentPeriodSummary.Next;
        }
        toPopulate.AverageBidPrice = runningTimeWeightedBidAverage;
        toPopulate.AverageAskPrice = runningTimeWeightedAskAverage;
        var now = atTime ?? DateTime.UtcNow;

        toPopulate.PeriodSummaryFlags = SummaryFlagsAsAt(recycler, now);

        return toPopulate;
    }

    public PricePeriodSummaryFlags SummaryFlagsAsAt(IRecycler recycler, DateTime? currentTime = null)
    {
        var now = currentTime ?? DateTime.UtcNow;

        var nowPeriodStart = TimeSeriesPeriod.ContainingPeriodBoundaryStart(now);
        var currentFlags   = nowPeriodStart == PeriodStartTime ? PricePeriodSummaryFlags.PeriodLatest : PricePeriodSummaryFlags.None;
        currentFlags |= PreviousPeriodBidAskEnd != null ? PricePeriodSummaryFlags.CreatedFromPreviousEnd : PricePeriodSummaryFlags.None;

        var boundedPeriodToNow = new BoundedTimeRange(PeriodStartTime, now);

        var    currentPeriodSummary = SubSummaryPeriods.Tail;
        ushort missingTickPeriods   = 0;

        foreach (var subRange in this.Reverse16SubTimeRanges(recycler))
        {
            if (subRange.IntersectsWith(boundedPeriodToNow))
            {
                currentPeriodSummary ??= SubSummaryPeriods.Tail;
                var previousPeriodSummary = currentPeriodSummary?.Next;

                var subRangeCurrentIntersection = subRange.Intersection(boundedPeriodToNow)!.Value;

                while (currentPeriodSummary != null && currentPeriodSummary.PeriodStartTime < subRangeCurrentIntersection.ToTime &&
                       (previousPeriodSummary == null || previousPeriodSummary.PeriodStartTime > subRangeCurrentIntersection.ToTime))
                    if (currentPeriodSummary.PeriodStartTime >= subRangeCurrentIntersection.ToTime)
                    {
                        previousPeriodSummary = currentPeriodSummary;
                        currentPeriodSummary  = currentPeriodSummary.Previous;
                    }
                    else if (previousPeriodSummary != null && previousPeriodSummary.PeriodStartTime < subRangeCurrentIntersection.ToTime)
                    {
                        currentPeriodSummary  = previousPeriodSummary;
                        previousPeriodSummary = previousPeriodSummary.Next;
                    }
                var percentageComplete = 0.0;
                while (currentPeriodSummary != null && currentPeriodSummary.PeriodEndTime > subRangeCurrentIntersection.FromTime)
                {
                    percentageComplete   += currentPeriodSummary.ContributingCompletePercentage(subRangeCurrentIntersection, recycler);
                    currentPeriodSummary =  currentPeriodSummary.Previous;
                }
                if (percentageComplete < 0.5) missingTickPeriods |= 1;
            }
            missingTickPeriods <<= 1;
        }
        currentFlags |= (PricePeriodSummaryFlags)((uint)missingTickPeriods << 16);

        if (missingTickPeriods == 0 && nowPeriodStart != PeriodStartTime && NextPeriodBidAskStart != null)
            currentFlags |= PricePeriodSummaryFlags.IsBestPossible;

        return currentFlags;
    }
}
