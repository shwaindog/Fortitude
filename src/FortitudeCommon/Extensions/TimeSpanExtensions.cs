// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Text;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Extensions;

public static class TimeSpanExtensions
{
    public static Action<TimeSpan, IStyledTypeStringAppender> TimeSpanStructFormatter = FormatTimeSpanAsStructAppender;

    public static Action<TimeSpan, IStyledTypeStringAppender> TimeSpanFormatter = FormatTimeSpanAsStringAppender;

    public static void FormatTimeSpanAsStringAppender(this TimeSpan timeSpan, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.WriteBuffer;

        StringBuilder? setSb = null;

        if (timeSpan.Days != 0) setSb = sb.AppendFormat("{0:N0}", timeSpan.Days).Append(":");

        if (timeSpan.Hours != 0 || setSb != null) setSb   = sb.AppendFormat("{0:00}", timeSpan.Hours).Append(":");
        if (timeSpan.Minutes != 0 || setSb != null) setSb = sb.AppendFormat("{0:00}", timeSpan.Minutes).Append(":");
        if (timeSpan.Seconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:00}", timeSpan.Seconds).Append(".");

        if (timeSpan.Milliseconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:000}", timeSpan.Milliseconds);
        if (timeSpan.Milliseconds != 0 || setSb != null) setSb = sb.AppendFormat("{0:000}", timeSpan.Microseconds).Append(":");
    }

    public static void FormatTimeSpanAsStructAppender(this TimeSpan timeSpan, IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(TimeSpan))
           .AddTypeStart()
           .AddNonDefaultField(nameof(TimeSpan.Days), timeSpan.Days)
           .AddNonDefaultField(nameof(TimeSpan.Hours), timeSpan.Hours)
           .AddNonDefaultField(nameof(TimeSpan.Minutes), timeSpan.Minutes)
           .AddNonDefaultField(nameof(TimeSpan.Seconds), timeSpan.Seconds)
           .AddNonDefaultField(nameof(TimeSpan.Milliseconds), timeSpan.Milliseconds)
           .AddNonDefaultField(nameof(TimeSpan.Microseconds), timeSpan.Microseconds)
           .AddTypeEnd();
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
