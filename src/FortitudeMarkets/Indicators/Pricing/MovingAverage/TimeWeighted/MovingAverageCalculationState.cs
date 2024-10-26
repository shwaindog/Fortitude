// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Indicators.Pricing.Parameters;
using MathNet.Numerics;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.MovingAverage.TimeWeighted;

public class MovingAverageCalculationState
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MovingAverageCalculationState));

    private readonly CalculateMovingAverageOptions calculateOptions;

    private readonly bool includeInvalidTime;

    private readonly MovingAverageOffset movingAverageOffset;

    private readonly bool useWallClock;

    private PreviousRunCalculation? previousRunCalculation;

    private RunningCalculation run;

    public MovingAverageCalculationState(MovingAverageOffset movingAvg, CalculateMovingAverageOptions calculateOptions)
    {
        movingAverageOffset   = movingAvg;
        this.calculateOptions = calculateOptions;
        var upToAtTime = movingAverageOffset.LatestOffsetPeriod > TimeSpan.Zero;
        useWallClock       = calculateOptions.TimeLengthFlags.IsUseWallTime() || upToAtTime;
        includeInvalidTime = calculateOptions.TimeLengthFlags.HasIncludeOpenMarketInvalidPeriodsFlag();
    }

    public DiscreetTimePeriod AveragePeriod => movingAverageOffset.AveragePeriod;

    public DateTime EarliestTime(DateTime asOfTime)
    {
        var earliestTime = movingAverageOffset.WallClockPeriodTimeRange(asOfTime).FromTime;
        return earliestTime;
    }

    public ValidRangeBidAskPeriodValue Calculate(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs, DateTime atTime, uint batchNumber)
    {
        var wallClockMovingAverageTimeRange = movingAverageOffset.WallClockPeriodTimeRange(atTime);

        run = new RunningCalculation
        {
            // go backwards from latest to earliest
            RemainingPeriod = movingAverageOffset.AveragePeriod.AveragePeriodTimeSpan()

          , IncludedTimeSpan = TimeSpan.Zero
          , CalcStartTime    = wallClockMovingAverageTimeRange.ToTime
          , StartDeltaFrom   = previousRunCalculation?.StartDeltaFrom
          , EndDeltaFrom     = previousRunCalculation?.EndDeltaFrom

          , TotalTimeWeightedBidInMs = previousRunCalculation?.TotalTimeWeightedBidInMs ?? 0m
          , TotalTimeWeightedAskInMs = previousRunCalculation?.TotalTimeWeightedAskInMs ?? 0m
        };

        run.JustCalcDeltaPeriods = run is { StartDeltaFrom: not null, EndDeltaFrom: not null };
        if (run.JustCalcDeltaPeriods) run.RemainingPeriod = (wallClockMovingAverageTimeRange.ToTime - run.StartDeltaFrom!.Value) * 2;

        StartFromLatestTick(timeOrderedPairs);

        while (run.PreviousTick != null && run.PreviousTick.AtTime > run.CalcStartTime)
        {
            run.CurrentTick  = run.PreviousTick;
            run.PreviousTick = run.PreviousTick.Previous;
        }

        Logger.Info("State Start Bid: {0}, Ask: {1} remaining {2}s.  For Start time {3}"
                  , run.TotalTimeWeightedBidInMs, run.TotalTimeWeightedAskInMs, run.RemainingPeriod, run.CalcStartTime);

        AddLatestTickWeightedBidAsk(wallClockMovingAverageTimeRange);

        var calcEndTime = (run.CurrentTick!.AtTime - run.RemainingPeriod).Min(run.EndDeltaFrom + run.RemainingPeriod);
        while (run is { PreviousTick: not null, CurrentTick: not null }
            && (run.RemainingPeriod > TimeSpan.Zero ||
                (run.JustCalcDeltaPeriods && run.CurrentTick.AtTime > calcEndTime.Min(previousRunCalculation?.EndDeltaFrom))))
        {
            calcEndTime = run.CurrentTick.AtTime - run.RemainingPeriod;

            var prevToCurrentTimeSpan = run.CurrentTick.AtTime - run.PreviousTick.AtTime;
            if (run.PreviousTick.IsMarketClosed() || run.PreviousTick is { ExcludeValues: true }
                                                  || (run.PreviousTick.SweepStartOfDataGap && !calculateOptions.IncludeOpenMarketDataGapsValues))
            {
                if (useWallClock || !run.PreviousTick.ExcludeTime) run.RemainingPeriod -= prevToCurrentTimeSpan;
                run.CurrentTick  = run.PreviousTick;
                run.PreviousTick = run.CurrentTick?.Previous;
                continue;
            }
            if (run.JustCalcDeltaPeriods)
                calcEndTime = AddOrSubtractDeltaFromPreviousValue();
            else
                AddFullCalculation();

            run.CurrentTick  = run.PreviousTick;
            run.PreviousTick = run.CurrentTick.Previous;
        }
        var periodTotalMs = (decimal)run.IncludedTimeSpan.TotalMilliseconds;
        SaveRunForNextDeltaRun(calcEndTime);
        var result = new ValidRangeBidAskPeriodValue
            (run.TotalTimeWeightedBidInMs / periodTotalMs, run.TotalTimeWeightedAskInMs / periodTotalMs
           , wallClockMovingAverageTimeRange.ToTime, wallClockMovingAverageTimeRange.ToTime
           , AveragePeriod, wallClockMovingAverageTimeRange.ToTime - run.RemainingPeriod);


        Logger.Info("State Final average Bid: {0}, Ask: {1} for remaining {2} and inc {3}.  For end time {4}"
                  , result.BidPrice, result.AskPrice, run.RemainingPeriod, run.IncludedTimeSpan, calcEndTime);
        return result;
    }

    private void SaveRunForNextDeltaRun(DateTime calcEndTime)
    {
        if (run.RemainingPeriod <= TimeSpan.Zero)
        {
            Logger.Info("State Final weighted Bid: {0}, Ask: {1} for remaining {2} and inc {3}.  For end time {4}"
                      , run.TotalTimeWeightedBidInMs, run.TotalTimeWeightedAskInMs, run.RemainingPeriod, run.IncludedTimeSpan, calcEndTime);
            previousRunCalculation =
                new PreviousRunCalculation(run.CalcStartTime, calcEndTime, run.TotalTimeWeightedBidInMs, run.TotalTimeWeightedAskInMs);
        }
        else
        {
            Logger.Info("State Clear weighted Bid: {0}, Ask: {1} for remaining {2} and inc {3}.  For end time {4}"
                      , run.TotalTimeWeightedBidInMs, run.TotalTimeWeightedAskInMs, run.RemainingPeriod, run.IncludedTimeSpan, calcEndTime);
            previousRunCalculation = null;
        }
    }

    private void AddFullCalculation()
    {
        if ((run.CurrentTick!.AtTime > run.CalcStartTime && run.PreviousTick!.AtTime < run.CalcStartTime)
         || run.CurrentTick.AtTime <= run.CalcStartTime)
        {
            var sliceStart = run.CalcStartTime.Min(run.PreviousTick!.AtTime);
            var sliceEnd   = run.CurrentTick.AtTime;
            var validMs    = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            if (validMs > (decimal)run.RemainingPeriod.TotalMilliseconds)
            {
                sliceStart = sliceEnd - run.RemainingPeriod;
                validMs    = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            }

            var weightedBidAsk = SliceWeightedBidAskPair(run.PreviousTick!, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs += weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs += weightedBidAsk.AskPrice;

            Logger.Info("State Full Add Bid: {0}, Ask: {1} for {2}ms. remaining {3} and inc {4}"
                      , run.PreviousTick.BidPrice, run.PreviousTick.AskPrice, validMs.Round(0), run.RemainingPeriod, run.IncludedTimeSpan);
        }
    }

    private DateTime AddOrSubtractDeltaFromPreviousValue()
    {
        var prevToCurrentTimeSpan       = run.CurrentTick!.AtTime - run.PreviousTick!.AtTime;
        var fromCurrentProjectedEndTime = run.CurrentTick!.AtTime - run.RemainingPeriod;
        var calcEndTime = prevToCurrentTimeSpan > run.RemainingPeriod
            ? fromCurrentProjectedEndTime.Max(run.EndDeltaFrom!.Value + run.RemainingPeriod)
            : fromCurrentProjectedEndTime.Min(run.EndDeltaFrom!.Value + run.RemainingPeriod);
        // add new weighted value that is now within range
        if ((run.CurrentTick.AtTime > run.CalcStartTime && run.PreviousTick.AtTime < run.CalcStartTime)
         || (run.CurrentTick.AtTime <= run.CalcStartTime && run.CurrentTick.AtTime > run.StartDeltaFrom))
        {
            var sliceStart = run.PreviousTick.AtTime.Max(run.StartDeltaFrom);
            var sliceEnd   = run.CurrentTick.AtTime.Min(run.CalcStartTime);

            var validMs = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            if (validMs > (decimal)run.RemainingPeriod.TotalMilliseconds)
            {
                sliceStart = sliceEnd - run.RemainingPeriod;
                validMs    = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            }

            var weightedBidAsk = SliceWeightedBidAskPair(run.PreviousTick, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs += weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs += weightedBidAsk.AskPrice;

            Logger.Info("State Delta Add Bid: {0}, Ask: {1} for {2}ms. remaining {3} and inc {4}"
                      , run.PreviousTick.BidPrice, run.PreviousTick.AskPrice, validMs.Round(0), run.RemainingPeriod, run.IncludedTimeSpan);
            if (sliceStart > run.PreviousTick.AtTime) run.IncludedTimeSpan += sliceStart - run.PreviousTick.AtTime;
        }
        // subtract old weighted values that are no longer within range
        else if ((run.CurrentTick.AtTime > run.EndDeltaFrom && run.PreviousTick.AtTime < calcEndTime)
              || (run.PreviousTick.AtTime < calcEndTime && run.CurrentTick.AtTime > run.EndDeltaFrom))
        {
            var sliceStart = run.PreviousTick.AtTime.Max(run.EndDeltaFrom);
            var sliceEnd   = run.CurrentTick.AtTime.Min(sliceStart + run.RemainingPeriod);

            var validMs = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            if (validMs > (decimal)run.RemainingPeriod.TotalMilliseconds)
            {
                sliceStart = sliceEnd - run.RemainingPeriod;
                validMs    = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            }

            var weightedBidAsk = SliceWeightedBidAskPair(run.PreviousTick, sliceStart, sliceEnd);

            run.IncludedTimeSpan         -= sliceEnd - sliceStart; // subtract these as this is now outside the moving average and is being deducted
            run.TotalTimeWeightedBidInMs -= weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs -= weightedBidAsk.AskPrice;

            if (run.RemainingPeriod <= TimeSpan.Zero)
            {
                if (sliceEnd < run.CurrentTick.AtTime) run.IncludedTimeSpan += run.CurrentTick.AtTime - sliceEnd;
                Logger.Info("State Finished Delta Sub Bid: {0}, Ask: {1} for {2}ms. remaining {3} and inc {4}"
                          , run.PreviousTick.BidPrice, run.PreviousTick.AskPrice, validMs.Round(0), run.RemainingPeriod, run.IncludedTimeSpan);
                return sliceEnd;
            }
            Logger.Info("State Delta Sub Bid: {0}, Ask: {1} for {2}ms. remaining {3} and inc {4}"
                      , run.PreviousTick.BidPrice, run.PreviousTick.AskPrice, validMs.Round(0), run.RemainingPeriod, run.IncludedTimeSpan);
        }
        else
        {
            run.IncludedTimeSpan += prevToCurrentTimeSpan;
        }
        return calcEndTime;
    }

    private void AddLatestTickWeightedBidAsk(BoundedTimeRange wallClockMovingAverageTimeRange)
    {
        if (run.JustCalcDeltaPeriods)
        {
            // subtract previous weighted value that is no longer within range
            if (run.StartDeltaFrom >= run.CurrentTick!.AtTime
             && run.StartDeltaFrom < run.CalcStartTime
             && run.CurrentTick.AtTime < run.CalcStartTime)
            {
                var sliceStart = run.CurrentTick.AtTime.Max(run.StartDeltaFrom.Value);
                var sliceEnd   = run.CalcStartTime;

                var validMs = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
                if (validMs > (decimal)run.RemainingPeriod.TotalMilliseconds)
                {
                    sliceStart = sliceEnd - run.RemainingPeriod;
                    validMs    = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
                }

                var weightedBidAsk = SliceWeightedBidAskPair(run.CurrentTick, sliceStart, sliceEnd);

                run.TotalTimeWeightedBidInMs += weightedBidAsk.BidPrice;
                run.TotalTimeWeightedAskInMs += weightedBidAsk.AskPrice;

                if (sliceStart > run.CurrentTick.AtTime) run.IncludedTimeSpan += sliceStart - run.CurrentTick.AtTime;

                Logger.Info("State First Tick Delta Add Bid: {0}, Ask: {1} for {2}ms. remaining {3} and inc {4}"
                          , run.CurrentTick.BidPrice, run.CurrentTick.AskPrice, validMs.Round(0), run.RemainingPeriod, run.IncludedTimeSpan);
            }
        }
        else
        {
            run.JustCalcDeltaPeriods     = false;
            run.TotalTimeWeightedBidInMs = 0m;
            run.TotalTimeWeightedAskInMs = 0m;

            var sliceStart = run.CurrentTick!.AtTime;
            var sliceEnd   = run.CalcStartTime;

            var validMs = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            if (validMs > (decimal)run.RemainingPeriod.TotalMilliseconds)
            {
                sliceStart = sliceEnd - run.RemainingPeriod;
                validMs    = (decimal)(sliceEnd - sliceStart).TotalMilliseconds;
            }

            var weightedBidAsk = SliceWeightedBidAskPair(run.CurrentTick, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs = weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs = weightedBidAsk.AskPrice;

            Logger.Info("State First Full Tick Add Bid: {0}, Ask: {1} for {2}ms. remaining {3} and inc {4}"
                      , run.CurrentTick.BidPrice, run.CurrentTick.AskPrice, validMs.Round(0), run.RemainingPeriod, run.IncludedTimeSpan);
        }
    }

    private void StartFromLatestTick(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs)
    {
        run.CurrentTick  = timeOrderedPairs.Tail;
        run.PreviousTick = timeOrderedPairs.Tail?.Previous;
    }

    private BidAskPair SliceWeightedBidAskPair(IValidRangeBidAskPeriod tick, DateTime sliceStart, DateTime sliceEnd)
    {
        var sliceTotalPeriod = sliceEnd - sliceStart;

        if (sliceTotalPeriod > run.RemainingPeriod)
        {
            sliceStart       = sliceEnd - run.RemainingPeriod;
            sliceTotalPeriod = sliceEnd - sliceStart;
        }

        var bidPrice = tick.BidPrice;
        var askPrice = tick.AskPrice;
        if (tick.UsePreviousValues)
        {
            bidPrice = tick.Previous?.BidPrice ?? bidPrice;
            askPrice = tick.Previous?.AskPrice ?? askPrice;
        }
        else if (tick.UseNextValues)
        {
            bidPrice = tick.Next?.BidPrice ?? bidPrice;
            askPrice = tick.Next?.AskPrice ?? askPrice;
        }

        var sliceMs = (decimal)sliceTotalPeriod.TotalMilliseconds;

        var bidTimeWeightedCurrentPrice = bidPrice * sliceMs;
        var askTimeWeightedCurrentPrice = askPrice * sliceMs;
        if (!useWallClock && !includeInvalidTime && tick.ValidTo < sliceEnd && !calculateOptions.IncludeOpenMarketInvalidValues)
        {
            var validPeriod = sliceStart < tick.ValidTo ? tick.ValidTo - sliceStart : TimeSpan.Zero;
            var validMs     = (decimal)validPeriod.TotalMilliseconds;
            bidTimeWeightedCurrentPrice = bidPrice * validMs;
            askTimeWeightedCurrentPrice = askPrice * validMs;

            run.RemainingPeriod  -= validPeriod;
            run.IncludedTimeSpan += validPeriod;
        }
        else
        {
            run.IncludedTimeSpan += sliceTotalPeriod;
            run.RemainingPeriod  -= sliceTotalPeriod;
        }
        return new BidAskPair(bidTimeWeightedCurrentPrice, askTimeWeightedCurrentPrice);
    }

    private struct RunningCalculation
    {
        public TimeSpan  RemainingPeriod;
        public decimal   TotalTimeWeightedBidInMs;
        public decimal   TotalTimeWeightedAskInMs;
        public DateTime? StartDeltaFrom;
        public DateTime? EndDeltaFrom;
        public DateTime  CalcStartTime;
        public bool      JustCalcDeltaPeriods;
        public TimeSpan  IncludedTimeSpan;

        public IValidRangeBidAskPeriod? CurrentTick;
        public IValidRangeBidAskPeriod? PreviousTick;
    }

    private readonly struct PreviousRunCalculation
    {
        public PreviousRunCalculation
            (DateTime startDeltaFrom, DateTime endDeltaFrom, decimal totalTimeWeightedBidInMs, decimal totalTimeWeightedAskInMs)
        {
            EndDeltaFrom             = endDeltaFrom;
            StartDeltaFrom           = startDeltaFrom;
            TotalTimeWeightedAskInMs = totalTimeWeightedAskInMs;
            TotalTimeWeightedBidInMs = totalTimeWeightedBidInMs;
        }

        public readonly decimal  TotalTimeWeightedBidInMs;
        public readonly decimal  TotalTimeWeightedAskInMs;
        public readonly DateTime StartDeltaFrom;
        public readonly DateTime EndDeltaFrom;
    }
}
