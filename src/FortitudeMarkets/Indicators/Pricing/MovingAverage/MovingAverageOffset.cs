// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.MovingAverage;

public struct MovingAverageOffset
{
    public MovingAverageOffset(DiscreetTimePeriod averagePeriod, DiscreetTimePeriod? latestOffsetPeriod = null)
    {
        AveragePeriod      = averagePeriod;
        LatestOffsetPeriod = latestOffsetPeriod ?? new DiscreetTimePeriod(TimeSpan.Zero);
    }

    public MovingAverageOffset(TimeSpan averagePeriod, DiscreetTimePeriod? latestOffsetPeriod = null)
    {
        AveragePeriod      = new DiscreetTimePeriod(averagePeriod);
        LatestOffsetPeriod = latestOffsetPeriod ?? new DiscreetTimePeriod(TimeSpan.Zero);
    }

    public MovingAverageOffset(DiscreetTimePeriod averagePeriod, TimeSpan latestOffsetPeriod)
    {
        AveragePeriod      = averagePeriod;
        LatestOffsetPeriod = new DiscreetTimePeriod(latestOffsetPeriod);
    }

    public MovingAverageOffset(DiscreetTimePeriod averagePeriod, DiscreetTimePeriod latestPeriodOffset, int latestOffsetNumberOfPeriods = 1)
    {
        AveragePeriod               = averagePeriod;
        LatestOffsetPeriod          = latestPeriodOffset;
        LatestOffsetNumberOfPeriods = latestOffsetNumberOfPeriods;
    }

    public DiscreetTimePeriod AveragePeriod      { get; }
    public DiscreetTimePeriod LatestOffsetPeriod { get; }

    public int LatestOffsetNumberOfPeriods { get; }
}

public static class MovingAverageParamsExtensions
{
    public static IEnumerable<MovingAverageOffset> Flatten(this BatchIndicatorPublishInterval batchInterval)
    {
        var offsetsUseTimeSpan = batchInterval.BatchEntryOffsetPeriod;
        for (var i = 0; i < batchInterval.BatchCount; i++)
        {
            var movingAvgParam = new MovingAverageOffset
                (batchInterval.CoveringPeriod, batchInterval.LatestOffset.AveragePeriodTimeSpan() + offsetsUseTimeSpan.AveragePeriodTimeSpan() * i);
            yield return movingAvgParam;
        }
    }

    public static BoundedTimeRange WallClockPeriodTimeRange(this MovingAverageOffset movingAvg, DateTime asOfTime)
    {
        var calcStartTime = asOfTime;
        var calcEndTime   = asOfTime;
        if (movingAvg.LatestOffsetPeriod.IsTimeBoundaryPeriod())
        {
            calcStartTime
                = movingAvg.LatestOffsetPeriod.Period.ContainingPeriodBoundaryStart(asOfTime -
                                                                                    movingAvg.AveragePeriod.AveragePeriodTimeSpan());
            for (var i = 0; i < movingAvg.LatestOffsetNumberOfPeriods; i++)
                calcStartTime = movingAvg.LatestOffsetPeriod.Period.PreviousPeriodStart(calcStartTime);
            calcEndTime = movingAvg.LatestOffsetPeriod.Period.PeriodEnd(calcStartTime);
        }
        else if (movingAvg.AveragePeriod.IsUncommonTimeSpan())
        {
            calcStartTime = asOfTime.Subtract(movingAvg.AveragePeriod.AveragePeriodTimeSpan());
            for (var i = 0; i < movingAvg.LatestOffsetNumberOfPeriods; i++)
                calcStartTime = calcStartTime.Subtract(movingAvg.LatestOffsetPeriod.AveragePeriodTimeSpan());
            calcEndTime = calcStartTime.Add(movingAvg.AveragePeriod.AveragePeriodTimeSpan());
        }
        return new BoundedTimeRange(calcStartTime, calcEndTime);
    }
}
