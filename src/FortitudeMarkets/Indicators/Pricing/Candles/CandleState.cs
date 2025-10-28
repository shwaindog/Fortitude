// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles;

public class CandleState : ITimeBoundaryPeriodRange
{
    public BidAskPair? NextPeriodBidAskStart;

    public BidAskPair? PreviousPeriodBidAskEnd;

    public IDoublyLinkedList<ICandle> SubCandlesPeriods = new DoublyLinkedList<ICandle>();

    public CandleState(DateTime periodStartTime, TimeBoundaryPeriod timeBoundaryPeriod)
    {
        PeriodStartTime    = periodStartTime;
        TimeBoundaryPeriod = timeBoundaryPeriod;
    }

    public bool HasPublishedComplete { get; set; }

    public bool IsEmpty => SubCandlesPeriods.Head == null && PreviousPeriodBidAskEnd == null;

    public DateTime PeriodStartTime { get; set; }

    public TimeBoundaryPeriod TimeBoundaryPeriod { get; set; }

    public BoundedTimeRange ToBoundedTimeRange
        (DateTime? maxDateTime = null) =>
        new(PeriodStartTime, TimeBoundaryPeriod.PeriodEnd(PeriodStartTime).Min(maxDateTime));

    public void Configure(DateTime periodStartTime, TimeBoundaryPeriod timeBoundaryPeriod)
    {
        PeriodStartTime    = periodStartTime;
        TimeBoundaryPeriod = timeBoundaryPeriod;
    }

    public Candle BuildCandle(IRecycler recycler, DateTime? atTime = null)
    {
        var toPopulate = recycler.Borrow<Candle>();
        toPopulate.TimeBoundaryPeriod = TimeBoundaryPeriod;
        toPopulate.PeriodStartTime    = PeriodStartTime;
        toPopulate.PeriodEndTime      = this.PeriodEnd();
        toPopulate.OpeningState(PreviousPeriodBidAskEnd);

        var periodLengthAt = (atTime ?? TimeContext.UtcNow).Min(toPopulate.PeriodEndTime);

        var runningTimeWeightedBidAverage = 0m;
        var runningTimeWeightedAskAverage = 0m;

        var currentCandle = SubCandlesPeriods.Head;

        var averageFrom = currentCandle?.PeriodStartTime ?? PeriodStartTime;

        if (this is { PreviousPeriodBidAskEnd: not null } && currentCandle?.PeriodStartTime > PeriodStartTime)
        {
            averageFrom = PeriodStartTime;
            var periodToQuoteMs = (decimal)(currentCandle.PeriodStartTime - PeriodStartTime).TotalMilliseconds;
            runningTimeWeightedBidAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.BidPrice;
            runningTimeWeightedAskAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.AskPrice;
        }
        else if (this is { PreviousPeriodBidAskEnd: not null } && currentCandle == null)
        {
            averageFrom = PeriodStartTime;
            var periodToQuoteMs = (decimal)(periodLengthAt - PeriodStartTime).TotalMilliseconds;
            runningTimeWeightedBidAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.BidPrice;
            runningTimeWeightedAskAverage = periodToQuoteMs * PreviousPeriodBidAskEnd!.Value.AskPrice;
            toPopulate.UnchangedClosingState(PreviousPeriodBidAskEnd);
        }
        while (currentCandle != null && currentCandle.PeriodStartTime < periodLengthAt)
        {
            var currentPeriodBoundedMs = (decimal)currentCandle.ToBoundedTimeRange(periodLengthAt).TimeSpan().TotalMilliseconds;

            runningTimeWeightedBidAverage += currentPeriodBoundedMs * currentCandle.AverageBidAsk.BidPrice;
            runningTimeWeightedAskAverage += currentPeriodBoundedMs * currentCandle.AverageBidAsk.AskPrice;

            toPopulate.MergeBoundaries(currentCandle);
            currentCandle = currentCandle.Next;
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

        toPopulate.CandleFlags = CandleFlagsAsAt(recycler, periodLengthAt);

        return toPopulate;
    }

    public CandleFlags CandleFlagsAsAt(IRecycler recycler, DateTime? currentTime = null)
    {
        var now = currentTime ?? DateTime.UtcNow;

        var nowPeriodStart = TimeBoundaryPeriod.ContainingPeriodBoundaryStart(now);
        var currentFlags   = nowPeriodStart == PeriodStartTime ? CandleFlags.PeriodLatest : CandleFlags.None;
        currentFlags |= PreviousPeriodBidAskEnd != null ? CandleFlags.CreatedFromPreviousEnd : CandleFlags.None;

        var boundedPeriodToNow = new BoundedTimeRange(PeriodStartTime, now);

        var  currentCandle = SubCandlesPeriods.Tail;
        uint missingTickPeriods   = 0;

        foreach (var subRange in this.Reverse16SubTimeRanges(recycler))
        {
            missingTickPeriods <<= 1;
            if (subRange.IntersectsWith(boundedPeriodToNow) && subRange.ContributingPercentageOfTimeRange(boundedPeriodToNow) > 0.5 / 16)
            {
                currentCandle ??= SubCandlesPeriods.Tail;

                var subRangeCurrentIntersection = subRange.Intersection(boundedPeriodToNow)!.Value;

                while (currentCandle != null && currentCandle.PeriodStartTime > subRangeCurrentIntersection.ToTime)
                    currentCandle = currentCandle.Previous;

                var hasPreceedingQuote = PreviousPeriodBidAskEnd != null ||
                                         SubCandlesPeriods.Head?.PeriodStartTime < subRangeCurrentIntersection.FromTime;
                var percentageComplete = hasPreceedingQuote ? 1.0 : 0.0;
                if (!hasPreceedingQuote)
                {
                    while (currentCandle != null && currentCandle.PeriodEndTime > subRangeCurrentIntersection.FromTime)
                    {
                        percentageComplete   += currentCandle.ContributingCompletePercentage(subRangeCurrentIntersection, recycler);
                        currentCandle =  currentCandle.Previous;
                    }
                    if (currentCandle == null && PreviousPeriodBidAskEnd != null)
                    {
                        var openToFirstTick = new BoundedTimeRange(PeriodStartTime, SubCandlesPeriods.Head?.PeriodStartTime ?? PeriodStartTime);
                        percentageComplete += openToFirstTick.ContributingPercentageOfTimeRange(subRange);
                    }
                }
                if (percentageComplete < 0.5) missingTickPeriods |= 1;
                currentCandle = currentCandle?.Next;
            }
        }
        currentFlags |= (CandleFlags)((uint)missingTickPeriods << 16);

        if (missingTickPeriods == 0 && nowPeriodStart != PeriodStartTime && NextPeriodBidAskStart != null)
            currentFlags |= CandleFlags.IsBestPossible;

        return currentFlags;
    }

    public CandleState Clear()
    {
        var currentSummary = SubCandlesPeriods.Head;
        while (currentSummary != null)
        {
            var removed = SubCandlesPeriods.Remove(currentSummary);
            removed.DecrementRefCount();

            currentSummary = currentSummary.Next;
        }
        SubCandlesPeriods.Clear();
        NextPeriodBidAskStart   = null;
        PreviousPeriodBidAskEnd = null;
        HasPublishedComplete    = false;
        return this;
    }
}
