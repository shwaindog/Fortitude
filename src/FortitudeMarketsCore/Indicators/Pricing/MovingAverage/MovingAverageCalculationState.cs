// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public class MovingAverageCalculationState
{
    private readonly MovingAverageParams movingAvg;
    private readonly ushort              publishAsSource;

    private IBidAskInstant? lastCalcEarliest;
    private IBidAskInstant? lastCalcLatest;

    private DateTime? lastCalcTimeEarliest;
    private DateTime? lastCalcTimeLatest;

    private BidAskInstantPair runningTotalTimeWeightedInMs;

    public MovingAverageCalculationState(MovingAverageParams movingAvg, ushort publishAsSource)
    {
        this.movingAvg       = movingAvg;
        this.publishAsSource = publishAsSource;
    }

    public DateTime EarliestTime(DateTime asOfTime)
    {
        var movingAverageTimeRange = movingAvg.PeriodTimeRange(asOfTime);
        return movingAverageTimeRange.Item1;
    }

    public BidAskInstantPair Calculate(IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime, uint batchNumber)
    {
        var movingAverageTimeRange = movingAvg.PeriodTimeRange(atTime);
        var calcStartTime          = movingAverageTimeRange.Item1;
        var calcEndTime            = movingAverageTimeRange.Item2;

        var startDeltaFrom = lastCalcTimeEarliest ?? calcStartTime;
        var endDeltaFrom   = lastCalcTimeLatest ?? calcEndTime;

        var justCalcDeltaPeriods = true;
        var currentNode          = lastCalcEarliest;
        var nextNode             = currentNode?.Next;
        if (currentNode == null)
        {
            runningTotalTimeWeightedInMs = new BidAskInstantPair();

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
             || (currentNode.AtTime > startDeltaFrom && currentNode.AtTime < calcStartTime))
            {
                var sliceStartTicks = Math.Max(currentNode.AtTime.Ticks, startDeltaFrom.Ticks);
                var sliceEndTicks   = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks);

                var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                runningTotalTimeWeightedInMs.BidPrice -= bidTimeWeightedCurrentPrice;
                runningTotalTimeWeightedInMs.AskPrice -= askTimeWeightedCurrentPrice;
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

                runningTotalTimeWeightedInMs.BidPrice += bidTimeWeightedCurrentPrice;
                runningTotalTimeWeightedInMs.AskPrice += askTimeWeightedCurrentPrice;
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

                runningTotalTimeWeightedInMs.BidPrice += bidTimeWeightedCurrentPrice;
                runningTotalTimeWeightedInMs.AskPrice += askTimeWeightedCurrentPrice;
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
            (runningTotalTimeWeightedInMs.BidPrice / periodTotalMs
           , runningTotalTimeWeightedInMs.AskPrice / periodTotalMs
           , calcEndTime, publishAsSource, batchNumber);
    }
}
