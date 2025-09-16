// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Extensions;

public static class TimeSpanExtensions
{
    public static CustomTypeStyler<TimeSpan> TimeSpanStyler = FormatTimeSpanAsStructAppender;

    public static CustomTypeStyler<TimeSpan> StylerComplexType(this TimeSpan _) => TimeSpanStyler;

    public static StyledTypeBuildResult FormatTimeSpanAsStructAppender(this TimeSpan timeSpan, IStyledTypeStringAppender sbc)
    {
        return
        sbc.StartComplexType(timeSpan)
           .Field.WhenNonDefaultAdd(nameof(TimeSpan.Days), timeSpan.Days)
           .Field.WhenNonDefaultAdd(nameof(TimeSpan.Hours), timeSpan.Hours)
           .Field.WhenNonDefaultAdd(nameof(TimeSpan.Minutes), timeSpan.Minutes)
           .Field.WhenNonDefaultAdd(nameof(TimeSpan.Seconds), timeSpan.Seconds)
           .Field.WhenNonDefaultAdd(nameof(TimeSpan.Milliseconds), timeSpan.Milliseconds)
           .Field.WhenNonDefaultAdd(nameof(TimeSpan.Microseconds), timeSpan.Microseconds)
           .Complete();
    }

    public static TimeSpan Min(this TimeSpan first, TimeSpan? optionalSecond)
    {
        if (optionalSecond == null) return first;
        return first.Min(optionalSecond.Value);
    }

    public static TimeSpan Min(this TimeSpan first, TimeSpan second)
    {
        var minTicks = Math.Min(first.Ticks, second.Ticks);
        return TimeSpan.FromTicks(minTicks);
    }

    public static TimeSpan Min(this TimeSpan first, TimeSpan second, TimeSpan third)
    {
        var minTicks = Math.Min(Math.Min(first.Ticks, second.Ticks), third.Ticks);
        return TimeSpan.FromTicks(minTicks);
    }

    public static TimeSpan Max(this TimeSpan first, TimeSpan? optionalSecond)
    {
        if (optionalSecond == null) return first;
        return first.Max(optionalSecond.Value);
    }

    public static TimeSpan Max(this TimeSpan first, TimeSpan second)
    {
        var maxTicks = Math.Max(first.Ticks, second.Ticks);
        return TimeSpan.FromTicks(maxTicks);
    }

    public static TimeSpan Max(this TimeSpan first, TimeSpan second, TimeSpan third)
    {
        var maxTicks = Math.Max(Math.Max(first.Ticks, second.Ticks), third.Ticks);
        return TimeSpan.FromTicks(maxTicks);
    }
}
