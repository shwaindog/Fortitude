// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.MovingAverage;

public struct MovingAverageParams
{
    public MovingAverageParams(TimeSpan periodTimeSpan, TimeSpan? latestOffsetTimeSpan = null)
    {
        PeriodTimeSpan       = periodTimeSpan;
        LatestOffsetTimeSpan = latestOffsetTimeSpan ?? TimeSpan.Zero;
    }

    public MovingAverageParams(TimeSpan periodTimeSpan, TimeSeriesPeriod? latestPeriodOffset, int latestOffsetNumberOfPeriods = 1)
    {
        PeriodTimeSpan               = periodTimeSpan;
        LatestOffsetTimeSeriesPeriod = latestPeriodOffset ?? FortitudeIO.TimeSeries.TimeSeriesPeriod.None;
        LatestOffsetNumberOfPeriods  = latestOffsetNumberOfPeriods;
    }

    public MovingAverageParams(TimeSeriesPeriod timeSeriesPeriod, TimeSeriesPeriod? latestPeriodOffset, int latestOffsetNumberOfPeriods = 1)
    {
        TimeSeriesPeriod             = timeSeriesPeriod;
        LatestOffsetTimeSeriesPeriod = latestPeriodOffset ?? FortitudeIO.TimeSeries.TimeSeriesPeriod.None;
        LatestOffsetNumberOfPeriods  = latestOffsetNumberOfPeriods;
    }

    public MovingAverageParams(TimeSeriesPeriod timeSeriesPeriod, TimeSpan? latestOffsetTimeSpan = null)
    {
        TimeSeriesPeriod     = timeSeriesPeriod;
        LatestOffsetTimeSpan = latestOffsetTimeSpan ?? TimeSpan.Zero;
    }

    public TimeSpan?         PeriodTimeSpan               { get; }
    public TimeSeriesPeriod? TimeSeriesPeriod             { get; }
    public TimeSpan          LatestOffsetTimeSpan         { get; }
    public TimeSeriesPeriod  LatestOffsetTimeSeriesPeriod { get; }
    public int               LatestOffsetNumberOfPeriods  { get; }
}

public static class MovingAverageParamsExtensions
{
    public static IEnumerable<MovingAverageParams> Flatten(this BatchPricePublishInterval batchInterval)
    {
        var currentTimeSpan    = TimeSpan.Zero;
        var periodsOffset      = 0;
        var offsetsUseTimeSpan = batchInterval.BatchPublishEntrySeparation.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan;
        for (var i = 0; i < batchInterval.BatchCount; i++)
        {
            if (batchInterval.EntryRange.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
            {
                var movingAvgParam = offsetsUseTimeSpan
                    ? new MovingAverageParams(batchInterval.EntryRange.PublishTimeSpan!.Value, currentTimeSpan)
                    : new MovingAverageParams(batchInterval.EntryRange.PublishTimeSpan!.Value, batchInterval.BatchPublishEntrySeparation.PublishPeriod
                                            , periodsOffset++);
                yield return movingAvgParam;
            }
            if (batchInterval.EntryRange.PriceIndicatorPublishType == PriceIndicatorPublishType.TimeSeriesPeriod)
            {
                var movingAvgParam = offsetsUseTimeSpan
                    ? new MovingAverageParams(batchInterval.EntryRange.PublishPeriod!.Value, currentTimeSpan)
                    : new MovingAverageParams(batchInterval.EntryRange.PublishPeriod!.Value, batchInterval.BatchPublishEntrySeparation.PublishPeriod
                                            , periodsOffset++);
                yield return movingAvgParam;
            }
            if (batchInterval.BatchPublishEntrySeparation.PriceIndicatorPublishType == PriceIndicatorPublishType.SetTimeSpan)
                currentTimeSpan += batchInterval.BatchPublishEntrySeparation.PublishTimeSpan!.Value;
        }
    }

    public static Tuple<DateTime, DateTime> PeriodTimeRange(this MovingAverageParams movingAvg, DateTime asOfTime)
    {
        var calcStartTime = asOfTime;
        var calcEndTime   = asOfTime;
        if (movingAvg.TimeSeriesPeriod != null)
        {
            calcStartTime = movingAvg.TimeSeriesPeriod.Value.ContainingPeriodBoundaryStart(asOfTime);
            for (var i = 0; i <= movingAvg.LatestOffsetNumberOfPeriods; i++)
                calcStartTime = movingAvg.LatestOffsetTimeSeriesPeriod.PreviousPeriodStart(calcStartTime);
            calcEndTime = movingAvg.TimeSeriesPeriod.Value.PeriodEnd(calcStartTime);
        }
        else if (movingAvg.PeriodTimeSpan != null)
        {
            calcStartTime = asOfTime.Subtract(movingAvg.PeriodTimeSpan!.Value);
            for (var i = 0; i < movingAvg.LatestOffsetNumberOfPeriods; i++)
                calcStartTime = calcStartTime.Subtract(movingAvg.LatestOffsetTimeSpan);
            calcEndTime = calcStartTime.Add(movingAvg.PeriodTimeSpan!.Value);
        }
        return new Tuple<DateTime, DateTime>(calcStartTime, calcEndTime);
    }
}
