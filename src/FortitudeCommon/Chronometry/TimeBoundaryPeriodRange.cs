// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeCommon.Chronometry;

public interface ITimeBoundaryPeriodRange
{
    DateTime PeriodStartTime { get; }

    TimeBoundaryPeriod TimeBoundaryPeriod { get; }
    BoundedTimeRange   ToBoundedTimeRange(DateTime? maxDateTime = null);
}

public struct TimeBoundaryPeriodRange // not inheriting from ITimeSeriesPeriodRange to prevent accidental boxing unboxing
{
    public TimeBoundaryPeriodRange() { }

    public TimeBoundaryPeriodRange(ITimeBoundaryPeriodRange timeBoundaryPeriodRange)
    {
        PeriodStartTime    = timeBoundaryPeriodRange.PeriodStartTime;
        TimeBoundaryPeriod = timeBoundaryPeriodRange.TimeBoundaryPeriod;
    }

    public TimeBoundaryPeriodRange(DateTime periodStartTime, TimeBoundaryPeriod timeBoundaryPeriod)
    {
        PeriodStartTime    = periodStartTime;
        TimeBoundaryPeriod = timeBoundaryPeriod;
    }

    public DateTime PeriodStartTime { get; set; }

    public TimeBoundaryPeriod TimeBoundaryPeriod { get; set; }

    public static implicit operator BoundedTimeRange(TimeBoundaryPeriodRange toConvert) =>
        new(toConvert.PeriodStartTime, toConvert.TimeBoundaryPeriod.PeriodEnd(toConvert.PeriodStartTime));
}

public static class TimeBoundaryPeriodRangeExtensions
{
    public static DateTime PeriodEnd(this TimeBoundaryPeriodRange range)      => range.TimeBoundaryPeriod.PeriodEnd(range.PeriodStartTime);
    public static TimeSpan PeriodTimeSpan(this TimeBoundaryPeriodRange range) => range.PeriodEnd() - range.PeriodStartTime;

    public static bool ContainsTime(this TimeBoundaryPeriodRange range, DateTime checkTime) =>
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

    public static IEnumerable<BoundedTimeRange> To16SubTimeRanges(this TimeBoundaryPeriodRange timeRange, IRecycler recycler)
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

    public static IEnumerable<BoundedTimeRange> Reverse16SubTimeRanges(this TimeBoundaryPeriodRange timeRange, IRecycler recycler)
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

    public static bool IsContainedBy(this TimeBoundaryPeriodRange range, BoundedTimeRange timeRange) =>
        range.PeriodStartTime <= timeRange.FromTime && range.PeriodEnd() > timeRange.ToTime;

    public static bool CompletelyContains(this BoundedTimeRange timeRange, TimeBoundaryPeriodRange range) => range.IsContainedBy(timeRange);

    public static bool IsContainedBy(this TimeBoundaryPeriodRange range, UnboundedTimeRange timeRange) =>
        range.PeriodStartTime <= (timeRange.FromTime ?? DateTime.MaxValue) && range.PeriodEnd() > (timeRange.ToTime ?? DateTime.MinValue);

    public static bool CompletelyContains(this UnboundedTimeRange timeRange, TimeBoundaryPeriodRange range) => range.IsContainedBy(timeRange);

    public static bool Intersects(this TimeBoundaryPeriodRange periodRange, UnboundedTimeRange? timeRange = null) =>
        timeRange == null || (periodRange.PeriodStartTime < (timeRange.Value.ToTime ?? DateTime.MaxValue) &&
                              periodRange.PeriodEnd() > (timeRange.Value.FromTime ?? DateTime.MinValue));

    public static bool Intersects(this TimeBoundaryPeriodRange periodRange, BoundedTimeRange timeRange) =>
        periodRange.PeriodStartTime < timeRange.ToTime && periodRange.PeriodEnd() > timeRange.FromTime;


    public static TimeBoundaryPeriodRange AsTimeSeriesPeriodRange(this ITimeBoundaryPeriodRange range) => new(range);

    public static DateTime PeriodEnd(this ITimeBoundaryPeriodRange range)      => range.TimeBoundaryPeriod.PeriodEnd(range.PeriodStartTime);
    public static TimeSpan PeriodTimeSpan(this ITimeBoundaryPeriodRange range) => range.PeriodEnd() - range.PeriodStartTime;

    public static bool ContainsTime(this ITimeBoundaryPeriodRange range, DateTime checkTime) =>
        range.PeriodStartTime <= checkTime && range.PeriodEnd() > checkTime;

    public static IEnumerable<BoundedTimeRange> To16SubTimeRanges(this ITimeBoundaryPeriodRange timeRange, IRecycler recycler)
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

    public static IEnumerable<BoundedTimeRange> Reverse16SubTimeRanges(this ITimeBoundaryPeriodRange timeRange, IRecycler recycler)
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

    public static bool IsContainedBy(this ITimeBoundaryPeriodRange range, BoundedTimeRange timeRange) =>
        range.PeriodStartTime <= timeRange.FromTime && range.PeriodEnd() > timeRange.ToTime;

    public static bool CompletelyContains(this BoundedTimeRange timeRange, ITimeBoundaryPeriodRange range) => range.IsContainedBy(timeRange);

    public static bool IsContainedBy(this ITimeBoundaryPeriodRange range, UnboundedTimeRange timeRange) =>
        range.PeriodStartTime >= (timeRange.FromTime ?? DateTime.MinValue) && range.PeriodEnd() <= (timeRange.ToTime ?? DateTime.MaxValue);

    public static bool CompletelyContains(this UnboundedTimeRange timeRange, ITimeBoundaryPeriodRange range) => range.IsContainedBy(timeRange);

    public static bool Intersects(this ITimeBoundaryPeriodRange periodRange, UnboundedTimeRange? timeRange = null) =>
        timeRange == null || (periodRange.PeriodStartTime < (timeRange.Value.ToTime ?? DateTime.MaxValue) &&
                              periodRange.PeriodEnd() > (timeRange.Value.FromTime ?? DateTime.MinValue));
}
