// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Extensions;

public static class TimeSpanExtensions
{
    public static StructStyler<TimeSpan> TimeSpanStructFormatter = FormatTimeSpanAsStructAppender;

    public static StructStyler<TimeSpan> TimeSpanFormatter = FormatTimeSpanAsStringAppender;

    public static void FormatTimeSpanAsStringAppender(this TimeSpan timeSpan, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        IStringBuilder? setSb = null;
        sb.Append("\"");
        if (timeSpan.Days != 0) setSb = sb.AppendFormat("{0:N0}", timeSpan.Days).Append(":");

        if (timeSpan.Hours != 0 || setSb != null) setSb   = sb.AppendFormat("{0:00}", timeSpan.Hours).Append(":");
        if (timeSpan.Minutes != 0 || setSb != null) setSb = sb.AppendFormat("{0:00}", timeSpan.Minutes).Append(":");
        if (timeSpan.Seconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:00}", timeSpan.Seconds).Append(".");

        if (timeSpan.Milliseconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:000}", timeSpan.Milliseconds);
        if (timeSpan.Milliseconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:000}", timeSpan.Microseconds).Append(":");
        sb.Append("\"");
    }

    public static void FormatTimeSpanAsStructAppender(this TimeSpan timeSpan, IStyledTypeStringAppender sbc)
    {
        sbc.StartComplexType(nameof(TimeSpan))
           .Field.AddWhenNonDefault(nameof(TimeSpan.Days), timeSpan.Days)
           .Field.AddWhenNonDefault(nameof(TimeSpan.Hours), timeSpan.Hours)
           .Field.AddWhenNonDefault(nameof(TimeSpan.Minutes), timeSpan.Minutes)
           .Field.AddWhenNonDefault(nameof(TimeSpan.Seconds), timeSpan.Seconds)
           .Field.AddWhenNonDefault(nameof(TimeSpan.Milliseconds), timeSpan.Milliseconds)
           .Field.AddWhenNonDefault(nameof(TimeSpan.Microseconds), timeSpan.Microseconds)
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
