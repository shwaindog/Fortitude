// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Extensions;

public static class TimeSpanExtensions
{
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
