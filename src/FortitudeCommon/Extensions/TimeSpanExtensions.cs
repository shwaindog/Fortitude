// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Extensions;

public static class TimeSpanExtensions
{
    public static CustomTypeStyler<TimeSpan> TimeSpanStyler = FormatTimeSpanAsStructAppender;

    public static CustomTypeStyler<TimeSpan> StylerComplexType(this TimeSpan _) => TimeSpanStyler;
    

    public static CustomTypeStyler<TimeSpan> TimeSpanFormatter = FormatTimeSpanAsStringAppender;
    
    public static CustomTypeStyler<TimeSpan> StylerAsStringFormatter(this TimeSpan _) => TimeSpanFormatter;

    public static StyledTypeBuildResult FormatTimeSpanAsStringAppender(this TimeSpan timeSpan, IStyledTypeStringAppender sbc)
    {
        var tb = sbc.StartSimpleValueType(timeSpan);
        using(var sb = tb.StartDelimitedStringBuilder())
        {
            IStringBuilder? setSb = null;
            if (timeSpan.Days != 0) setSb = sb.AppendFormat("{0:N0}", timeSpan.Days).Append(":");

            if (timeSpan.Hours != 0 || setSb != null) setSb   = sb.AppendFormat("{0:00}", timeSpan.Hours).Append(":");
            if (timeSpan.Minutes != 0 || setSb != null) setSb = sb.AppendFormat("{0:00}", timeSpan.Minutes).Append(":");
            if (timeSpan.Seconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:00}", timeSpan.Seconds).Append(".");

            if (timeSpan.Milliseconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:000}", timeSpan.Milliseconds);
            if (timeSpan.Microseconds != 0 || setSb != null) sb.AppendFormat("{0:000}", timeSpan.Microseconds);
        }
        
        return tb.Complete();
    }

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
