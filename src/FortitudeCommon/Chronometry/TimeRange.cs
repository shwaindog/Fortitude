// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;

#endregion

namespace FortitudeCommon.Chronometry;

public struct UnboundedTimeRange
{
    public UnboundedTimeRange(DateTime? fromTime, DateTime? toTime)
    {
        FromTime = fromTime;
        ToTime   = toTime;
    }

    public DateTime? FromTime { get; }
    public DateTime? ToTime   { get; }


    public static explicit operator BoundedTimeRange(UnboundedTimeRange toConvert) => new(toConvert.FromTime!.Value, toConvert.ToTime!.Value);
}

public struct BoundedTimeRange
{
    public BoundedTimeRange(DateTime fromTime, DateTime toTime)
    {
        if (fromTime == DateTime.MinValue || toTime == DateTime.MaxValue) throw new Exception("Expected from and toTime to have set values");
        if (fromTime > toTime) throw new Exception("Expected fromTime to be less than toTime ");
        FromTime = fromTime;
        ToTime   = toTime;
    }

    public DateTime FromTime { get; }
    public DateTime ToTime   { get; }


    public static implicit operator UnboundedTimeRange(BoundedTimeRange toConvert) => new(toConvert.FromTime, toConvert.ToTime);
}

public struct SubPeriodOffset
{
    public SubPeriodOffset(TimeSpan startOffset, TimeSpan periodLength)
    {
        StartOffset     = startOffset;
        SubPeriodLength = periodLength;
    }

    public TimeSpan StartOffset     { get; }
    public TimeSpan SubPeriodLength { get; }
}

public static class TimeRangeExtensions
{
    public static bool IntersectsWith(this UnboundedTimeRange lhs, UnboundedTimeRange? rhsOptional)
    {
        if (rhsOptional == null) return true;
        var rhs = rhsOptional.Value;
        return (lhs.FromTime < rhs.ToTime || (lhs.FromTime == null && lhs.ToTime != null) || (rhs.ToTime == null && rhs.FromTime != null))
            && (lhs.ToTime > rhs.FromTime || (lhs.ToTime == null && lhs.FromTime != null) || (rhs.FromTime == null && rhs.ToTime != null));
    }

    public static bool IntersectsWith(this BoundedTimeRange lhs, BoundedTimeRange? rhsOptional)
    {
        if (rhsOptional == null) return true;
        var rhs = rhsOptional.Value;
        return lhs.FromTime < rhs.ToTime && lhs.ToTime > rhs.FromTime;
    }

    public static bool IsBounded(this UnboundedTimeRange periodRange) => periodRange is { FromTime: not null, ToTime: not null };

    public static TimeSpan TimeSpan(this BoundedTimeRange periodRange) => periodRange.FromTime - periodRange.ToTime;

    public static TimeSpan BoundedTimeSpan(this UnboundedTimeRange periodRange, BoundedTimeRange boundedRange)
    {
        var boundedTImeRange = periodRange.Intersection(boundedRange);
        if (boundedTImeRange == null || boundedTImeRange?.IsBounded() == false) throw new Exception("Unexpected range to be bounded");
        var bounded = boundedTImeRange!.Value;
        return bounded.FromTime!.Value - bounded.ToTime!.Value;
    }

    public static UnboundedTimeRange CapUpperTime
        (this UnboundedTimeRange toCap, DateTime maxUpperBound) =>
        new(toCap.FromTime, toCap.ToTime?.Min(maxUpperBound) ?? maxUpperBound);

    public static UnboundedTimeRange? Intersection(this UnboundedTimeRange lhs, UnboundedTimeRange rhs)
    {
        var minTicks = DateTimeExtensions.MinTicks;
        var maxTicks = DateTimeExtensions.MaxTicks;

        var maxStartTime = Math.Max(lhs.FromTime?.Ticks ?? minTicks, rhs.FromTime?.Ticks ?? minTicks);
        var minEndTime   = Math.Min(lhs.ToTime?.Ticks ?? maxTicks, rhs.ToTime?.Ticks ?? maxTicks);

        if (maxStartTime > minEndTime) return null;
        DateTime? fromTime = maxStartTime != minTicks ? DateTime.FromBinary(maxStartTime) : null;
        DateTime? toTime   = minEndTime != maxTicks ? DateTime.FromBinary(minEndTime) : null;
        return new UnboundedTimeRange(fromTime, toTime);
    }

    public static BoundedTimeRange? Intersection(this BoundedTimeRange lhs, BoundedTimeRange rhs)
    {
        var maxStartTime = Math.Max(lhs.FromTime.Ticks, rhs.FromTime.Ticks);
        var minEndTime   = Math.Min(lhs.ToTime.Ticks, rhs.ToTime.Ticks);

        if (maxStartTime > minEndTime) return null;
        var fromTime = DateTime.FromBinary(maxStartTime);
        var toTime   = DateTime.FromBinary(minEndTime);
        return new BoundedTimeRange(fromTime, toTime);
    }
}
