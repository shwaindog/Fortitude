// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public class MovingAverageCalculationState
{
    private readonly DateTime earliestQuoteTime;
    private readonly TimeSpan ignoreTickGapsGreaterThan;

    private readonly MovingAverageOffset movingAverageOffset;

    private IBidAskInstant? lastCalcEarliest;

    private DateTime? lastCalcTimeEarliest;
    private DateTime? lastCalcTimeLatest;

    public MovingAverageCalculationState(MovingAverageOffset movingAvg, DateTime earliestQuoteTime, TimeSpan ignoreTickGapsGreaterThan)
    {
        movingAverageOffset            = movingAvg;
        this.earliestQuoteTime         = earliestQuoteTime;
        this.ignoreTickGapsGreaterThan = ignoreTickGapsGreaterThan;
    }

    public TimePeriod AveragePeriod => movingAverageOffset.AveragePeriod;

    public DateTime EarliestTime(DateTime asOfTime)
    {
        var earliestTime = movingAverageOffset.PeriodTimeRange(asOfTime).FromTime;
        return earliestTime;
    }

    public BidAskInstantPair Calculate(IDoublyLinkedList<IBidAskInstant> timeOrderedPairs, DateTime atTime, uint batchNumber)
    {
        var movingAverageTimeRange = movingAverageOffset.PeriodTimeRange(atTime);
        var calcStartTime          = movingAverageTimeRange.FromTime.Max(earliestQuoteTime);
        var calcEndTime            = movingAverageTimeRange.ToTime;

        var startDeltaFrom = lastCalcTimeEarliest ?? calcStartTime;
        var endDeltaFrom   = lastCalcTimeLatest ?? calcEndTime;

        var justCalcDeltaPeriods            = true;
        var currentNode                     = lastCalcEarliest;
        var nextNode                        = currentNode?.Next;
        var runningTotalTimeWeightedBidInMs = 0m;
        var runningTotalTimeWeightedAskInMs = 0m;

        var cumulativeIgnoreWeightingPeriod = TimeSpan.Zero;
        if (currentNode == null)
        {
            justCalcDeltaPeriods = false;

            currentNode = timeOrderedPairs.Head;
            nextNode    = timeOrderedPairs.Head;
            while (nextNode != null && nextNode.AtTime < startDeltaFrom)
            {
                currentNode = nextNode;
                nextNode    = nextNode.Next;
            }
            nextNode = currentNode?.Next;
        }
        while (currentNode != null && currentNode.AtTime < calcEndTime)
        {
            if ((lastCalcEarliest?.AtTime ?? DateTime.MinValue) < currentNode.AtTime && currentNode.AtTime <= calcStartTime &&
                calcEndTime.Min(nextNode?.AtTime) > calcStartTime)
                lastCalcEarliest = currentNode;
            if (calcEndTime.Min(nextNode?.AtTime) - currentNode.AtTime > ignoreTickGapsGreaterThan)
            {
                cumulativeIgnoreWeightingPeriod += calcEndTime.Min(nextNode?.AtTime) - currentNode.AtTime;

                currentNode = nextNode;
                nextNode    = currentNode?.Next;
                continue;
            }
            if (justCalcDeltaPeriods)
            {
                // subtract previous weighted value that is no longer within range
                if ((currentNode.AtTime < startDeltaFrom && (nextNode?.AtTime ?? calcEndTime) > startDeltaFrom)
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
                // add new weighted value that is now within range
                else if ((currentNode.AtTime < endDeltaFrom && (nextNode?.AtTime ?? calcEndTime) > endDeltaFrom)
                      || (currentNode.AtTime >= endDeltaFrom && currentNode.AtTime < calcEndTime))
                {
                    var sliceStartTicks = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, endDeltaFrom.Ticks);
                    var sliceEndTicks   = Math.Max(sliceStartTicks, Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks));

                    var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                    var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                    var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                    runningTotalTimeWeightedBidInMs += bidTimeWeightedCurrentPrice;
                    runningTotalTimeWeightedAskInMs += askTimeWeightedCurrentPrice;
                }
            }
            else
            {
                if ((currentNode.AtTime < calcStartTime && (nextNode?.AtTime ?? calcEndTime) > calcStartTime)
                 || (currentNode.AtTime > calcStartTime && currentNode.AtTime < calcEndTime))
                {
                    var sliceStartTicks = Math.Max(currentNode.AtTime.Ticks, calcStartTime.Ticks);
                    var sliceEndTicks   = Math.Min(nextNode?.AtTime.Ticks ?? calcEndTime.Ticks, calcEndTime.Ticks);

                    var sliceMs = (decimal)TimeSpan.FromTicks(sliceEndTicks - sliceStartTicks).TotalMilliseconds;

                    var bidTimeWeightedCurrentPrice = currentNode.BidPrice * sliceMs;
                    var askTimeWeightedCurrentPrice = currentNode.AskPrice * sliceMs;

                    runningTotalTimeWeightedBidInMs += bidTimeWeightedCurrentPrice;
                    runningTotalTimeWeightedAskInMs += askTimeWeightedCurrentPrice;
                }
            }

            if (currentNode.AtTime > calcEndTime) break;

            currentNode = nextNode;
            nextNode    = currentNode?.Next;
        }
        var periodTotalMs = (decimal)(calcEndTime - calcStartTime - cumulativeIgnoreWeightingPeriod).TotalMilliseconds;
        lastCalcTimeEarliest = calcStartTime;
        lastCalcTimeLatest   = calcEndTime;
        return new BidAskInstantPair
            (runningTotalTimeWeightedBidInMs / periodTotalMs, runningTotalTimeWeightedAskInMs / periodTotalMs, calcEndTime);
    }
}
