// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Indicators.Pricing.Parameters;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage.TimeWeighted;

public class MovingAverageCalculationState
{
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

    public TimePeriod AveragePeriod => movingAverageOffset.AveragePeriod;

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
          , CurrentTick      = previousRunCalculation?.StartFromTick ?? timeOrderedPairs.Tail

          , TotalTimeWeightedBidInMs = previousRunCalculation?.TotalTimeWeightedBidInMs ?? 0m
          , TotalTimeWeightedAskInMs = previousRunCalculation?.TotalTimeWeightedAskInMs ?? 0m
        };

        run.JustCalcDeltaPeriods = run is { StartDeltaFrom: not null, EndDeltaFrom: not null };

        if (run.CurrentTick == null) FindLastCalculatedTick(timeOrderedPairs);
        run.PreviousTick = run.CurrentTick?.Previous;
        while (run.PreviousTick != null && run.PreviousTick.AtTime < (run.StartDeltaFrom ?? run.CalcStartTime))
        {
            run.CurrentTick  = run.PreviousTick;
            run.PreviousTick = run.PreviousTick.Previous;
        }
        run.PreviousTick = run.CurrentTick?.Previous;
        var startFromTick = run.CurrentTick!;

        AddLatestTickWeightedBidAsk(wallClockMovingAverageTimeRange);

        var calcEndTime = run.CurrentTick!.AtTime - run.RemainingPeriod;
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
                AddOrSubtractDeltaFromPreviousValue();
            else
                AddFullCalculation();

            run.CurrentTick  = run.PreviousTick;
            run.PreviousTick = run.CurrentTick.Previous;
        }
        var periodTotalMs = (decimal)run.IncludedTimeSpan.TotalMilliseconds;
        SaveRunForNextDeltaRun(startFromTick, calcEndTime);
        return new ValidRangeBidAskPeriodValue
            (run.TotalTimeWeightedBidInMs / periodTotalMs, run.TotalTimeWeightedAskInMs / periodTotalMs
           , wallClockMovingAverageTimeRange.ToTime, wallClockMovingAverageTimeRange.ToTime
           , wallClockMovingAverageTimeRange.ToTime - run.RemainingPeriod, AveragePeriod);
    }

    private void SaveRunForNextDeltaRun(IValidRangeBidAskPeriod startTick, DateTime calcEndTime)
    {
        if (run.RemainingPeriod <= TimeSpan.Zero)
            previousRunCalculation = new PreviousRunCalculation
                (startTick, run.CalcStartTime, calcEndTime, run.TotalTimeWeightedBidInMs, run.TotalTimeWeightedAskInMs);
        else
            previousRunCalculation = null;
    }

    private void AddFullCalculation()
    {
        var prevToCurrentTimeSpan = run.CurrentTick!.AtTime - run.PreviousTick!.AtTime;
        if ((run.CurrentTick.AtTime > run.CalcStartTime && run.PreviousTick.AtTime < run.CalcStartTime)
         || run.CurrentTick.AtTime <= run.CalcStartTime)
        {
            var sliceStart = run.CalcStartTime.Min(run.PreviousTick.AtTime);
            var sliceEnd   = run.CurrentTick.AtTime;

            var weightedBidAsk = SliceWeightedBidAskPair(run.PreviousTick!, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs += weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs += weightedBidAsk.AskPrice;
        }
        else
        {
            if (useWallClock || !run.PreviousTick.ExcludeTime) run.RemainingPeriod -= prevToCurrentTimeSpan;
        }
    }

    private void AddOrSubtractDeltaFromPreviousValue()
    {
        var calcEndTime = run.CurrentTick!.AtTime - run.RemainingPeriod;

        var prevToCurrentTimeSpan = run.CurrentTick.AtTime - run.PreviousTick!.AtTime;
        // add new weighted value that is now within range
        if ((run.CurrentTick.AtTime > run.StartDeltaFrom && run.PreviousTick.AtTime < run.StartDeltaFrom)
         || (run.CurrentTick.AtTime <= run.StartDeltaFrom && run.CurrentTick.AtTime >= run.CalcStartTime))
        {
            var sliceStart = run.PreviousTick.AtTime.Max(run.StartDeltaFrom);
            var sliceEnd   = run.CurrentTick.AtTime;

            var weightedBidAsk = SliceWeightedBidAskPair(run.PreviousTick, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs += weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs += weightedBidAsk.AskPrice;
        }
        // subtract old weighted values that are no longer within range
        else if ((run.CurrentTick.AtTime > run.EndDeltaFrom && run.PreviousTick.AtTime < run.EndDeltaFrom)
              || run.CurrentTick.AtTime <= run.EndDeltaFrom)
        {
            var sliceStart = calcEndTime.Max(run.PreviousTick.AtTime);
            var sliceEnd   = run.CurrentTick.AtTime;

            var weightedBidAsk = SliceWeightedBidAskPair(run.PreviousTick, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs -= weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs -= weightedBidAsk.AskPrice;
        }
        else
        {
            if (useWallClock || !run.PreviousTick.ExcludeTime) run.RemainingPeriod -= prevToCurrentTimeSpan;
        }
    }

    private void AddLatestTickWeightedBidAsk(BoundedTimeRange wallClockMovingAverageTimeRange)
    {
        if (run.JustCalcDeltaPeriods && run.StartDeltaFrom > run.CurrentTick!.AtTime && run.StartDeltaFrom < wallClockMovingAverageTimeRange.ToTime)
        {
            // subtract previous weighted value that is no longer within range
            if ((run.CurrentTick.AtTime.Max(wallClockMovingAverageTimeRange.ToTime) > run.StartDeltaFrom &&
                 run.CurrentTick.AtTime < run.StartDeltaFrom)
             || (run.CurrentTick.AtTime <= run.StartDeltaFrom && run.CurrentTick.AtTime >= run.CalcStartTime))
            {
                var sliceStart = run.CurrentTick.AtTime.Max(run.StartDeltaFrom.Value);
                var sliceEnd   = wallClockMovingAverageTimeRange.ToTime;

                var weightedBidAsk = SliceWeightedBidAskPair(run.CurrentTick, sliceStart, sliceEnd);

                run.TotalTimeWeightedBidInMs -= weightedBidAsk.BidPrice;
                run.TotalTimeWeightedAskInMs -= weightedBidAsk.AskPrice;
            }
        }
        else
        {
            var sliceStart = run.CurrentTick!.AtTime;
            var sliceEnd   = wallClockMovingAverageTimeRange.ToTime;

            var weightedBidAsk = SliceWeightedBidAskPair(run.CurrentTick, sliceStart, sliceEnd);

            run.TotalTimeWeightedBidInMs -= weightedBidAsk.BidPrice;
            run.TotalTimeWeightedAskInMs -= weightedBidAsk.AskPrice;
        }
    }

    private void FindLastCalculatedTick(IDoublyLinkedList<IValidRangeBidAskPeriod> timeOrderedPairs)
    {
        run.JustCalcDeltaPeriods     = false;
        run.TotalTimeWeightedBidInMs = 0m;
        run.TotalTimeWeightedAskInMs = 0m;

        run.CurrentTick  = timeOrderedPairs.Tail;
        run.PreviousTick = timeOrderedPairs.Tail;
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
        (IValidRangeBidAskPeriod startFromTick, DateTime startDeltaFrom, DateTime endDeltaFrom
          , decimal totalTimeWeightedBidInMs, decimal totalTimeWeightedAskInMs)
        {
            EndDeltaFrom             = endDeltaFrom;
            StartDeltaFrom           = startDeltaFrom;
            StartFromTick            = startFromTick;
            TotalTimeWeightedAskInMs = totalTimeWeightedAskInMs;
            TotalTimeWeightedBidInMs = totalTimeWeightedBidInMs;
        }

        public readonly decimal  TotalTimeWeightedBidInMs;
        public readonly decimal  TotalTimeWeightedAskInMs;
        public readonly DateTime StartDeltaFrom;
        public readonly DateTime EndDeltaFrom;

        public readonly IValidRangeBidAskPeriod StartFromTick;
    }
}
