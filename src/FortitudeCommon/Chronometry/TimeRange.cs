// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

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
    public DateTime? ToTime { get; }

    public static CustomTypeStyler<UnboundedTimeRange> Styler { get; } =
        (utr, stsa) =>
            stsa.StartComplexType(utr, nameof(utr))
                .Field.WhenNonDefaultAdd(nameof(utr.FromTime), utr.FromTime, DateTime.MinValue, "{0:O}")
                .Field.WhenNonDefaultAdd(nameof(utr.ToTime), utr.ToTime, DateTime.MinValue, "{0:O}")
                .Complete();

    public static explicit operator BoundedTimeRange(UnboundedTimeRange toConvert) => new(toConvert.FromTime!.Value, toConvert.ToTime!.Value);
}

public struct UnboundedTimeSpanRange
{
    public UnboundedTimeSpanRange(TimeSpan? lowerLimit, TimeSpan? upperLimit)
    {
        LowerLimit = lowerLimit;
        UpperLimit = upperLimit;
    }

    public TimeSpan? LowerLimit { get; }
    public TimeSpan? UpperLimit { get; }
    
    public static CustomTypeStyler<UnboundedTimeSpanRange> Styler { get; } =
        (utsr, stsa) =>
            stsa.StartComplexType(utsr, nameof(utsr))
                .Field.WhenNonDefaultAdd(nameof(utsr.LowerLimit), utsr.LowerLimit, TimeSpan.Zero)
                .Field.WhenNonDefaultAdd(nameof(utsr.UpperLimit), utsr.UpperLimit, TimeSpan.Zero)
                .Complete();


    public static explicit operator BoundedTimeSpanRange
        (UnboundedTimeSpanRange toConvert) =>
        new(toConvert.LowerLimit!.Value, toConvert.UpperLimit!.Value);
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
    public DateTime ToTime { get; }
    
    public static CustomTypeStyler<BoundedTimeRange> Styler { get; } =
        (btr, stsa) =>
            stsa.StartComplexType(btr, nameof(btr))
                .Field.WhenNonDefaultAdd(nameof(btr.FromTime), btr.FromTime, DateTime.MinValue, "{0:O}")
                .Field.WhenNonDefaultAdd(nameof(btr.ToTime), btr.ToTime, DateTime.MinValue, "{0:O}")
                .Complete();
    
    public static implicit operator UnboundedTimeRange(BoundedTimeRange toConvert) => new(toConvert.FromTime, toConvert.ToTime);
}

public struct BoundedTimeSpanRange
{
    public BoundedTimeSpanRange(TimeSpan lowerLimit, TimeSpan upperLimit)
    {
        if (lowerLimit == TimeSpan.MinValue || upperLimit == TimeSpan.MaxValue)
            throw new Exception("Expected lowerLimit and upperLimit to have set values");
        if (lowerLimit > upperLimit) throw new Exception("Expected lowerLimit to be less than upperLimit ");
        LowerLimit = lowerLimit;
        UpperLimit = upperLimit;
    }

    public TimeSpan LowerLimit { get; }
    public TimeSpan UpperLimit { get; }

    
    public static CustomTypeStyler<BoundedTimeSpanRange> Styler { get; } =
        (btsr, stsa) =>
            stsa.StartComplexType(btsr, nameof(btsr))
                .Field.WhenNonDefaultAdd(nameof(btsr.LowerLimit), btsr.LowerLimit, TimeSpan.Zero)
                .Field.WhenNonDefaultAdd(nameof(btsr.UpperLimit), btsr.UpperLimit, TimeSpan.Zero)
                .Complete();

    public static implicit operator UnboundedTimeSpanRange(BoundedTimeSpanRange toConvert) => new(toConvert.LowerLimit, toConvert.UpperLimit);
}

public struct SubPeriodOffset
{
    public SubPeriodOffset(TimeSpan startOffset, TimeSpan periodLength)
    {
        StartOffset     = startOffset;
        SubPeriodLength = periodLength;
    }

    public TimeSpan StartOffset { get; }
    public TimeSpan SubPeriodLength { get; }
}

public static class TimeRangeExtensions
{
    public static CustomTypeStyler<UnboundedTimeRange> UnboundedTimeRangeFormatter
        = FormatUnboundedTimeRangeAppender;

    public static StyledTypeBuildResult FormatUnboundedTimeRangeAppender(this UnboundedTimeRange timeRange, IStyledTypeStringAppender sbc)
    {
        return sbc.StartComplexType( timeRange)
           .Field.AlwaysAdd(nameof(timeRange.FromTime), timeRange.FromTime, "{0:yyyy-MM-dd HH:mm:ss}")
           .Field.AlwaysAdd(nameof(timeRange.ToTime), timeRange.ToTime, "{0:yyyy-MM-dd HH:mm:ss}").Complete();
    }
    
    public static CustomTypeStyler<UnboundedTimeSpanRange> UnboundedTimeSpanRangeFormatter
        = FormatUnboundedTimeSpanRangeAppender;

    public static StyledTypeBuildResult FormatUnboundedTimeSpanRangeAppender(this UnboundedTimeSpanRange timeRange, IStyledTypeStringAppender sbc)
    {
        return sbc.StartComplexType(timeRange)
           .Field.AlwaysAdd(nameof(timeRange.LowerLimit), timeRange.LowerLimit, "{0:dd HH:mm:ss}")
           .Field.AlwaysAdd(nameof(timeRange.UpperLimit), timeRange.UpperLimit, "{0:dd HH:mm:ss}").Complete();
    }
    
    public static CustomTypeStyler<BoundedTimeRange> BoundedTimeRangeFormatter
        = FormatBoundedTimeRangeAppender;

    public static StyledTypeBuildResult FormatBoundedTimeRangeAppender(this BoundedTimeRange timeRange, IStyledTypeStringAppender sbc)
    {
        return sbc.StartComplexType(timeRange)
           .Field.AlwaysAdd(nameof(timeRange.FromTime), timeRange.FromTime, "{0:yyyy-MM-dd HH:mm:ss}")
           .Field.AlwaysAdd(nameof(timeRange.ToTime), timeRange.ToTime, "{0:yyyy-MM-dd HH:mm:ss}").Complete();
    }

    
    public static CustomTypeStyler<BoundedTimeSpanRange> BoundedTimeSpanRangeFormatter
        = FormatBoundedTimeSpanRangeAppender;

    public static StyledTypeBuildResult FormatBoundedTimeSpanRangeAppender(this BoundedTimeSpanRange timeRange, IStyledTypeStringAppender sbc)
    {
        return sbc.StartComplexType(timeRange)
           .Field.AlwaysAdd(nameof(timeRange.LowerLimit), timeRange.LowerLimit, "{0:dd HH:mm:ss}")
           .Field.AlwaysAdd(nameof(timeRange.UpperLimit), timeRange.UpperLimit, "{0:dd HH:mm:ss}").Complete();
    }

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

    public static bool IsWhollyBoundedBy(this BoundedTimeRange check, BoundedTimeRange boundedBy) =>
        check.FromTime >= boundedBy.FromTime && check.ToTime <= boundedBy.ToTime;

    public static bool Contains(this BoundedTimeRange boundingPeriod, DateTime checkTime) =>
        boundingPeriod.FromTime <= checkTime && boundingPeriod.ToTime >= checkTime;

    public static bool IsBounded(this UnboundedTimeRange periodRange) => periodRange is { FromTime: not null, ToTime: not null };

    public static TimeSpan TimeSpan(this BoundedTimeRange periodRange) => periodRange.ToTime - periodRange.FromTime;

    public static TimeSpan BoundedTimeSpan(this UnboundedTimeRange periodRange, BoundedTimeRange boundedRange)
    {
        var boundedTImeRange = periodRange.Intersection(boundedRange);
        if (boundedTImeRange == null || boundedTImeRange?.IsBounded() == false) throw new Exception("Unexpected range to be bounded");
        var bounded = boundedTImeRange!.Value;
        return bounded.FromTime!.Value - bounded.ToTime!.Value;
    }

    public static UnboundedTimeRange CapUpperTime(this UnboundedTimeRange toCap, DateTime maxUpperBound) =>
        new(toCap.FromTime, toCap.ToTime?.Min(maxUpperBound) ?? maxUpperBound);

    public static BoundedTimeRange TrimFromTime(this BoundedTimeRange toTrim, DateTime newFromTime) =>
        new(toTrim.FromTime.Max(newFromTime), toTrim.ToTime);

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
