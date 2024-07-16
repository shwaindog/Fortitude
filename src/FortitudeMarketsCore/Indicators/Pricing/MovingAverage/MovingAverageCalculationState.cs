// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Indicators.Pricing;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public class MovingAverageCalculationState
{
    private readonly MovingAverageOffset firstMovingAverageOffset;
    private readonly long                indicatorSourceTickerId;

    private readonly MovingAverageOffset[] movingAvgs;

    private readonly ushort publishAsSource;

    private IBidAskInstant? lastCalcEarliest;
    private IBidAskInstant? lastCalcLatest;

    private DateTime? lastCalcTimeEarliest;
    private DateTime? lastCalcTimeLatest;

    public MovingAverageCalculationState(long indicatorSourceTickerId, MovingAverageOffset[] movingAvg, ushort publishAsSource)
    {
        this.indicatorSourceTickerId = indicatorSourceTickerId;
        this.movingAvgs              = movingAvg;
        firstMovingAverageOffset     = movingAvg[0];
        this.publishAsSource         = publishAsSource;
    }

    public MovingAverageCalculationState(long indicatorSourceTickerId, MovingAverageOffset movingAvg, ushort publishAsSource)
    {
        this.indicatorSourceTickerId = indicatorSourceTickerId;
        this.movingAvgs              = new[] { movingAvg };
        firstMovingAverageOffset     = movingAvg;
        this.publishAsSource         = publishAsSource;
    }

    public bool IsBatchRequest => movingAvgs.Length > 1;

    public DateTime EarliestTime(DateTime asOfTime)
    {
        var earliestTime = movingAvgs.Select(ma => ma.PeriodTimeRange(asOfTime).FromTime).Min();
        return earliestTime;
    }

    public SameIndicatorBidAskInstantChain CalculateBatch(IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime, IRecycler recycler)
    {
        var resultChain = recycler.Borrow<SameIndicatorBidAskInstantChain>();
        resultChain.Configure(indicatorSourceTickerId, firstMovingAverageOffset.AveragePeriod);
        var batchCount = 0u;
        foreach (var movingAverageOffset in movingAvgs)
        {
            var movingAverageInstant = Calculate(movingAverageOffset, timeOrderedPairs, atTime, batchCount++);
            resultChain.AddLast(movingAverageInstant.ToBidAskInstant(recycler));
        }
        return resultChain;
    }

    public IndicatorBidAskInstant CalculateSingle(IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime, IRecycler recycler)
    {
        var movingAverageInstantPair = Calculate(firstMovingAverageOffset, timeOrderedPairs, atTime, 0);
        var movingAverageInstant
            = movingAverageInstantPair.ToIndicatorBidAskInstant(indicatorSourceTickerId, firstMovingAverageOffset.AveragePeriod, recycler);
        return movingAverageInstant;
    }

    public BidAskInstantPair Calculate
        (MovingAverageOffset movingAvg, IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime, uint batchNumber)
    {
        var movingAverageTimeRange = movingAvg.PeriodTimeRange(atTime);
        var calcStartTime          = movingAverageTimeRange.FromTime;
        var calcEndTime            = movingAverageTimeRange.ToTime;

        var startDeltaFrom = lastCalcTimeEarliest ?? calcStartTime;
        var endDeltaFrom   = lastCalcTimeLatest ?? calcEndTime;

        var justCalcDeltaPeriods            = true;
        var currentNode                     = lastCalcEarliest;
        var nextNode                        = currentNode?.Next;
        var runningTotalTimeWeightedBidInMs = 0m;
        var runningTotalTimeWeightedAskInMs = 0m;
        if (currentNode == null)
        {
            justCalcDeltaPeriods = false;

            currentNode = timeOrderedPairs.Head;
            nextNode    = currentNode?.Next;
            while (nextNode != null && nextNode.AtTime < calcStartTime)
            {
                currentNode = nextNode;
                nextNode    = nextNode.Next;
            }
            nextNode = currentNode?.Next;
        }
        while (currentNode != null && currentNode.AtTime < calcEndTime)
        {
            if (currentNode.AtTime <= calcStartTime && (nextNode?.AtTime ?? calcEndTime) > calcStartTime)
            {
                lastCalcEarliest = currentNode;
            }
            if (justCalcDeltaPeriods
             && (currentNode.AtTime < startDeltaFrom && (nextNode?.AtTime ?? calcEndTime) > startDeltaFrom)
             || (currentNode.AtTime >= startDeltaFrom && currentNode.AtTime <= calcStartTime))
            {
                var sliceStartTicks = Math.Max(currentNode.AtTime.Ticks, startDeltaFrom.Ticks);
                var sliceEndTicks   = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks);

                var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                runningTotalTimeWeightedBidInMs -= bidTimeWeightedCurrentPrice;
                runningTotalTimeWeightedAskInMs -= askTimeWeightedCurrentPrice;
            }
            else if (justCalcDeltaPeriods
                  && (currentNode.AtTime < endDeltaFrom && (nextNode?.AtTime ?? calcEndTime) > endDeltaFrom)
                  || (currentNode.AtTime > endDeltaFrom && currentNode.AtTime < calcEndTime))
            {
                var sliceStartTicks = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, endDeltaFrom.Ticks);
                var sliceEndTicks   = Math.Max(sliceStartTicks, Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks));

                var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                runningTotalTimeWeightedBidInMs += bidTimeWeightedCurrentPrice;
                runningTotalTimeWeightedAskInMs += askTimeWeightedCurrentPrice;
            }
            else if (!justCalcDeltaPeriods
                  && ((currentNode.AtTime < calcStartTime && (nextNode?.AtTime ?? calcEndTime) > calcStartTime)
                   || (currentNode.AtTime > calcStartTime && currentNode.AtTime < calcEndTime)))
            {
                var sliceStartTicks = Math.Max(currentNode.AtTime.Ticks, calcStartTime.Ticks);
                var sliceEndTicks   = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks);

                var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                runningTotalTimeWeightedBidInMs += bidTimeWeightedCurrentPrice;
                runningTotalTimeWeightedAskInMs += askTimeWeightedCurrentPrice;
            }
            else if (justCalcDeltaPeriods && currentNode.AtTime > calcStartTime && currentNode.AtTime < (lastCalcLatest?.AtTime ?? calcStartTime))
            {
                currentNode = lastCalcLatest!;
                nextNode    = currentNode.Next;
                continue;
            }
            else if (currentNode.AtTime <= calcEndTime && (nextNode?.AtTime ?? calcEndTime) >= calcEndTime)
            {
                lastCalcLatest = currentNode;
            }

            if (currentNode.AtTime > calcEndTime) break;

            currentNode = nextNode;
            nextNode    = currentNode?.Next;
        }
        var periodTotalMs = (decimal)(calcEndTime - calcStartTime).TotalMilliseconds;
        lastCalcTimeEarliest = calcStartTime;
        lastCalcTimeLatest   = calcEndTime;
        return new BidAskInstantPair
            (runningTotalTimeWeightedBidInMs / periodTotalMs, runningTotalTimeWeightedAskInMs / periodTotalMs, calcEndTime);
    }
}
