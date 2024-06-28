// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeIO.TimeSeries;

public interface ITimeSeriesPeriodRange
{
    DateTime         PeriodStartTime  { get; }
    TimeSeriesPeriod TimeSeriesPeriod { get; }
    BoundedTimeRange ToBoundedTimeRange(DateTime? maxDateTime = null);
}

public struct TimeSeriesPeriodRange // not inheriting from ITimeSeriesPeriodRange to prevent accidental boxing unboxing
{
    public TimeSeriesPeriodRange() { }

    public TimeSeriesPeriodRange(ITimeSeriesPeriodRange timeSeriesPeriodRange)
    {
        PeriodStartTime  = timeSeriesPeriodRange.PeriodStartTime;
        TimeSeriesPeriod = timeSeriesPeriodRange.TimeSeriesPeriod;
    }

    public TimeSeriesPeriodRange(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public DateTime         PeriodStartTime  { get; set; }
    public TimeSeriesPeriod TimeSeriesPeriod { get; set; }

    public static implicit operator BoundedTimeRange(TimeSeriesPeriodRange toConvert) =>
        new(toConvert.PeriodStartTime, toConvert.TimeSeriesPeriod.PeriodEnd(toConvert.PeriodStartTime));
}

public static class TimeSeriesPeriodRangeExtensions
{
    public static DateTime PeriodEnd(this TimeSeriesPeriodRange range)      => range.TimeSeriesPeriod.PeriodEnd(range.PeriodStartTime);
    public static TimeSpan PeriodTimeSpan(this TimeSeriesPeriodRange range) => range.PeriodEnd() - range.PeriodStartTime;

    public static bool ContainsTime(this TimeSeriesPeriodRange range, DateTime checkTime) =>
        range.PeriodStartTime <= checkTime && range.PeriodEnd() > checkTime;

    public static double ContributingPercentageOfTimeRange(this BoundedTimeRange subPeriod, BoundedTimeRange totalBoundedTime)
    {
        if (!subPeriod.IntersectsWith(totalBoundedTime)) return 0;
        var totalTimeSpan        = totalBoundedTime.TimeSpan();
        var intersection         = subPeriod.Intersection(totalBoundedTime)!.Value;
        var intersectionTimeSpan = intersection.TimeSpan();

        var percentageOfTotal = intersectionTimeSpan / totalTimeSpan;
        return percentageOfTotal;
    }

    public static IEnumerable<BoundedTimeRange> To16SubTimeRanges(this TimeSeriesPeriodRange timeRange, IRecycler recycler)
    {
        var autoRecycleEnumerable = recycler.Borrow<AutoRecycledEnumerable<BoundedTimeRange>>();
        var timeSpan              = timeRange.PeriodTimeSpan();
        var sixteenthTimeSpan     = timeSpan / 16;
        var currentStartTime      = timeRange.PeriodStartTime;
        for (var i = 0; i < 16; i++)
        {
            var endTime = currentStartTime + sixteenthTimeSpan;
            autoRecycleEnumerable.Add(new BoundedTimeRange(currentStartTime, endTime));
            currentStartTime = endTime;
        }
        return autoRecycleEnumerable;
    }

    public static IEnumerable<BoundedTimeRange> Reverse16SubTimeRanges(this TimeSeriesPeriodRange timeRange, IRecycler recycler)
    {
        var autoRecycleEnumerable = recycler.Borrow<AutoRecycledEnumerable<BoundedTimeRange>>();
        var timeSpan              = timeRange.PeriodTimeSpan();
        var sixteenthTimeSpan     = timeSpan / 16;
        var currentEndTime        = timeRange.PeriodEnd();
        for (var i = 0; i < 16; i++)
        {
            var currentStartTime = currentEndTime - sixteenthTimeSpan;
            autoRecycleEnumerable.Add(new BoundedTimeRange(currentStartTime, currentEndTime));
            currentEndTime = currentStartTime;
        }
        return autoRecycleEnumerable;
    }

    public static bool Intersects(this TimeSeriesPeriodRange periodRange, UnboundedTimeRange? timeRange = null) =>
        timeRange == null || (periodRange.PeriodStartTime < (timeRange.Value.ToTime ?? DateTime.MaxValue) &&
                              periodRange.PeriodEnd() > (timeRange.Value.FromTime ?? DateTime.MinValue));

    public static bool Intersects(this TimeSeriesPeriodRange periodRange, BoundedTimeRange timeRange) =>
        periodRange.PeriodStartTime < timeRange.ToTime && periodRange.PeriodEnd() > timeRange.FromTime;


    public static TimeSeriesPeriodRange AsTimeSeriesPeriodRange(this ITimeSeriesPeriodRange range) => new(range);
    public static DateTime              PeriodEnd(this ITimeSeriesPeriodRange range) => range.TimeSeriesPeriod.PeriodEnd(range.PeriodStartTime);
    public static TimeSpan              PeriodTimeSpan(this ITimeSeriesPeriodRange range) => range.PeriodEnd() - range.PeriodStartTime;

    public static bool ContainsTime(this ITimeSeriesPeriodRange range, DateTime checkTime) =>
        range.PeriodStartTime <= checkTime && range.PeriodEnd() > checkTime;

    public static IEnumerable<BoundedTimeRange> To16SubTimeRanges(this ITimeSeriesPeriodRange timeRange, IRecycler recycler)
    {
        var autoRecycleEnumerable = recycler.Borrow<AutoRecycledEnumerable<BoundedTimeRange>>();
        var timeSpan              = timeRange.PeriodTimeSpan();
        var sixteenthTimeSpan     = timeSpan / 16;
        var currentStartTime      = timeRange.PeriodStartTime;
        for (var i = 0; i < 16; i++)
        {
            var endTime = currentStartTime + sixteenthTimeSpan;
            autoRecycleEnumerable.Add(new BoundedTimeRange(currentStartTime, endTime));
            currentStartTime = endTime;
        }
        return autoRecycleEnumerable;
    }

    public static IEnumerable<BoundedTimeRange> Reverse16SubTimeRanges(this ITimeSeriesPeriodRange timeRange, IRecycler recycler)
    {
        var autoRecycleEnumerable = recycler.Borrow<AutoRecycledEnumerable<BoundedTimeRange>>();
        var timeSpan              = timeRange.PeriodTimeSpan();
        var sixteenthTimeSpan     = timeSpan / 16;
        var currentEndTime        = timeRange.PeriodEnd();
        for (var i = 0; i < 16; i++)
        {
            var currentStartTime = currentEndTime - sixteenthTimeSpan;
            autoRecycleEnumerable.Add(new BoundedTimeRange(currentStartTime, currentEndTime));
            currentEndTime = currentStartTime;
        }
        return autoRecycleEnumerable;
    }

    public static bool Intersects(this ITimeSeriesPeriodRange periodRange, UnboundedTimeRange? timeRange = null) =>
        timeRange == null || (periodRange.PeriodStartTime < (timeRange.Value.ToTime ?? DateTime.MaxValue) &&
                              periodRange.PeriodEnd() > (timeRange.Value.FromTime ?? DateTime.MinValue));
}
