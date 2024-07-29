// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public struct MovingAverageOffset
{
    public MovingAverageOffset(TimePeriod averagePeriod, TimePeriod? latestOffsetPeriod = null)
    {
        AveragePeriod      = averagePeriod;
        LatestOffsetPeriod = latestOffsetPeriod ?? new TimePeriod(TimeSpan.Zero);
    }

    public MovingAverageOffset(TimeSpan averagePeriod, TimePeriod? latestOffsetPeriod = null)
    {
        AveragePeriod      = new TimePeriod(averagePeriod);
        LatestOffsetPeriod = latestOffsetPeriod ?? new TimePeriod(TimeSpan.Zero);
    }

    public MovingAverageOffset(TimePeriod averagePeriod, TimeSpan latestOffsetPeriod)
    {
        AveragePeriod      = averagePeriod;
        LatestOffsetPeriod = new TimePeriod(latestOffsetPeriod);
    }

    public MovingAverageOffset(TimePeriod averagePeriod, TimePeriod latestPeriodOffset, int latestOffsetNumberOfPeriods = 1)
    {
        AveragePeriod               = averagePeriod;
        LatestOffsetPeriod          = latestPeriodOffset;
        LatestOffsetNumberOfPeriods = latestOffsetNumberOfPeriods;
    }

    public TimePeriod AveragePeriod               { get; }
    public TimePeriod LatestOffsetPeriod          { get; }
    public int        LatestOffsetNumberOfPeriods { get; }
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
        if (movingAvg.LatestOffsetPeriod.IsTimeSeriesPeriod())
        {
            calcStartTime
                = movingAvg.LatestOffsetPeriod.TimeSeriesPeriod.ContainingPeriodBoundaryStart(asOfTime -
                                                                                              movingAvg.AveragePeriod.AveragePeriodTimeSpan());
            for (var i = 0; i < movingAvg.LatestOffsetNumberOfPeriods; i++)
                calcStartTime = movingAvg.LatestOffsetPeriod.TimeSeriesPeriod.PreviousPeriodStart(calcStartTime);
            calcEndTime = movingAvg.LatestOffsetPeriod.TimeSeriesPeriod.PeriodEnd(calcStartTime);
        }
        else if (movingAvg.AveragePeriod.IsTimeSpan())
        {
            calcStartTime = asOfTime.Subtract(movingAvg.AveragePeriod.AveragePeriodTimeSpan());
            for (var i = 0; i < movingAvg.LatestOffsetNumberOfPeriods; i++)
                calcStartTime = calcStartTime.Subtract(movingAvg.LatestOffsetPeriod.AveragePeriodTimeSpan());
            calcEndTime = calcStartTime.Add(movingAvg.AveragePeriod.AveragePeriodTimeSpan());
        }
        return new BoundedTimeRange(calcStartTime, calcEndTime);
    }
}
