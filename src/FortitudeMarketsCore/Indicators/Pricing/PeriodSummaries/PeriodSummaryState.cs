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

    public bool HasPublishedComplete { get; set; }

    public bool IsEmpty => SubSummaryPeriods.Head == null && PreviousPeriodBidAskEnd == null;

    public DateTime PeriodStartTime { get; set; }

    public TimeSeriesPeriod TimeSeriesPeriod { get; set; }

    public BoundedTimeRange ToBoundedTimeRange
        (DateTime? maxDateTime = null) =>
        new(PeriodStartTime, TimeSeriesPeriod.PeriodEnd(PeriodStartTime).Min(maxDateTime));

    public void Configure(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public PricePeriodSummary BuildPeriodSummary(IRecycler recycler, DateTime? atTime = null)
    {
        var toPopulate = recycler.Borrow<PricePeriodSummary>();
        toPopulate.TimeSeriesPeriod = TimeSeriesPeriod;
        toPopulate.PeriodStartTime  = PeriodStartTime;
        toPopulate.PeriodEndTime    = this.PeriodEnd();
        toPopulate.OpeningState(PreviousPeriodBidAskEnd);

        var periodLengthAt = (atTime ?? TimeContext.UtcNow).Min(toPopulate.PeriodEndTime);

        var runningTimeWeightedBidAverage = 0m;
        var runningTimeWeightedAskAverage = 0m;

        var currentPeriodSummary = SubSummaryPeriods.Head;

        var averageFrom = currentPeriodSummary?.PeriodStartTime ?? PeriodStartTime;

        if (this is { PreviousPeriodBidAskEnd: not null } && currentPeriodSummary?.PeriodStartTime > PeriodStartTime)
        {
            averageFrom = PeriodStartTime;
            var periodToQuoteMs = (decimal)(currentPeriodSummary.PeriodStartTime - PeriodStartTime).TotalMilliseconds;
            runningTimeWeightedBidAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.BidPrice;
            runningTimeWeightedAskAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.AskPrice;
        }
        else if (this is { PreviousPeriodBidAskEnd: not null } && currentPeriodSummary == null)
        {
            averageFrom = PeriodStartTime;
            var periodToQuoteMs = (decimal)(periodLengthAt - PeriodStartTime).TotalMilliseconds;
            runningTimeWeightedBidAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.BidPrice;
            runningTimeWeightedAskAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.AskPrice;
            toPopulate.UnchangedClosingState(PreviousPeriodBidAskEnd);
        }
        while (currentPeriodSummary != null && currentPeriodSummary.PeriodStartTime < periodLengthAt)
        {
            var currentPeriodBoundedMs = (decimal)currentPeriodSummary.ToBoundedTimeRange(periodLengthAt).TimeSpan().TotalMilliseconds;

            runningTimeWeightedBidAverage += currentPeriodBoundedMs * currentPeriodSummary.AverageBidAsk.BidPrice;
            runningTimeWeightedAskAverage += currentPeriodBoundedMs * currentPeriodSummary.AverageBidAsk.AskPrice;

            toPopulate.MergeBoundaries(currentPeriodSummary);
            currentPeriodSummary = currentPeriodSummary.Next;
        }
        var startNowMs = (decimal)(periodLengthAt - averageFrom).TotalMilliseconds;
        if (startNowMs > 0)
        {
            toPopulate.AverageBidPrice = runningTimeWeightedBidAverage / startNowMs;
            toPopulate.AverageAskPrice = runningTimeWeightedAskAverage / startNowMs;
        }
        else if (runningTimeWeightedBidAverage > 0 || runningTimeWeightedAskAverage > 0)
        {
            var currentPeriodBoundedMs = (decimal)(periodLengthAt - PeriodStartTime).TotalMilliseconds;
            toPopulate.AverageBidPrice = runningTimeWeightedBidAverage / currentPeriodBoundedMs;
            toPopulate.AverageAskPrice = runningTimeWeightedAskAverage / currentPeriodBoundedMs;
        }
        else
        {
            toPopulate.AverageBidPrice = toPopulate.EndBidPrice;
            toPopulate.AverageAskPrice = toPopulate.EndAskPrice;
        }

        toPopulate.PeriodSummaryFlags = SummaryFlagsAsAt(recycler, periodLengthAt);

        return toPopulate;
    }

    public PricePeriodSummaryFlags SummaryFlagsAsAt(IRecycler recycler, DateTime? currentTime = null)
    {
        var now = currentTime ?? DateTime.UtcNow;

        var nowPeriodStart = TimeSeriesPeriod.ContainingPeriodBoundaryStart(now);
        var currentFlags   = nowPeriodStart == PeriodStartTime ? PricePeriodSummaryFlags.PeriodLatest : PricePeriodSummaryFlags.None;
        currentFlags |= PreviousPeriodBidAskEnd != null ? PricePeriodSummaryFlags.CreatedFromPreviousEnd : PricePeriodSummaryFlags.None;

        var boundedPeriodToNow = new BoundedTimeRange(PeriodStartTime, now);

        var  currentPeriodSummary = SubSummaryPeriods.Tail;
        uint missingTickPeriods   = 0;

        foreach (var subRange in this.Reverse16SubTimeRanges(recycler))
        {
            missingTickPeriods <<= 1;
            if (subRange.IntersectsWith(boundedPeriodToNow) && subRange.ContributingPercentageOfTimeRange(boundedPeriodToNow) > 0.5 / 16)
            {
                currentPeriodSummary ??= SubSummaryPeriods.Tail;

                var subRangeCurrentIntersection = subRange.Intersection(boundedPeriodToNow)!.Value;

                while (currentPeriodSummary != null && currentPeriodSummary.PeriodStartTime > subRangeCurrentIntersection.ToTime)
                    currentPeriodSummary = currentPeriodSummary.Previous;

                var hasPreceedingQuote = PreviousPeriodBidAskEnd != null ||
                                         SubSummaryPeriods.Head?.PeriodStartTime < subRangeCurrentIntersection.FromTime;
                var percentageComplete = hasPreceedingQuote ? 1.0 : 0.0;
                if (!hasPreceedingQuote)
                {
                    while (currentPeriodSummary != null && currentPeriodSummary.PeriodEndTime > subRangeCurrentIntersection.FromTime)
                    {
                        percentageComplete   += currentPeriodSummary.ContributingCompletePercentage(subRangeCurrentIntersection, recycler);
                        currentPeriodSummary =  currentPeriodSummary.Previous;
                    }
                    if (currentPeriodSummary == null && PreviousPeriodBidAskEnd != null)
                    {
                        var openToFirstTick = new BoundedTimeRange(PeriodStartTime, SubSummaryPeriods.Head?.PeriodStartTime ?? PeriodStartTime);
                        percentageComplete += openToFirstTick.ContributingPercentageOfTimeRange(subRange);
                    }
                }
                if (percentageComplete < 0.5) missingTickPeriods |= 1;
                currentPeriodSummary = currentPeriodSummary?.Next;
            }
        }
        currentFlags |= (PricePeriodSummaryFlags)((uint)missingTickPeriods << 16);

        if (missingTickPeriods == 0 && nowPeriodStart != PeriodStartTime && NextPeriodBidAskStart != null)
            currentFlags |= PricePeriodSummaryFlags.IsBestPossible;

        return currentFlags;
    }

    public PeriodSummaryState Clear()
    {
        var currentSummary = SubSummaryPeriods.Head;
        while (currentSummary != null)
        {
            var removed = SubSummaryPeriods.Remove(currentSummary);
            removed.DecrementRefCount();

            currentSummary = currentSummary.Next;
        }
        SubSummaryPeriods.Clear();
        NextPeriodBidAskStart   = null;
        PreviousPeriodBidAskEnd = null;
        HasPublishedComplete    = false;
        return this;
    }
}
