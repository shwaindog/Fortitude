// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;

#endregion

namespace FortitudeMarkets.Config.Availability;

public readonly struct WeekdayStartStopPair(TimeZonedWeekDayTime openTime, TimeZonedWeekDayTime closeTime)
{
    public TimeZonedWeekDayTime OpenTime  { get; } = openTime;
    public TimeZonedWeekDayTime CloseTime { get; } = closeTime;
}

public class WeekDayOpenClosePairComparer : IComparer<WeekdayStartStopPair>
{
    public int Compare
        (WeekdayStartStopPair x, WeekdayStartStopPair y) =>
        x.OpenTime.StartOfUtcWeekTimeSpan().CompareTo(y.OpenTime.StartOfUtcWeekTimeSpan());
}

public static class WeekDayOpenClosePairExtensions
{
    public static bool WithinOpeningHours(this WeekdayStartStopPair weekdayStartStopPair, DateTimeOffset dateTimeOffset)
    {
        var startOfWeekTimeSpan = dateTimeOffset.TimeSinceUtcStartOfWeek();
        return startOfWeekTimeSpan >= weekdayStartStopPair.OpenTime.StartOfUtcWeekTimeSpan() &&
               startOfWeekTimeSpan < weekdayStartStopPair.CloseTime.StartOfUtcWeekTimeSpan();
    }

    public static bool WithinOpeningHours(this WeekdayStartStopPair weekdayStartStopPair, DateTime dateTime)
    {
        var startOfWeekTimeSpan = dateTime.TimeSinceUtcStartOfWeek();
        return startOfWeekTimeSpan >= weekdayStartStopPair.OpenTime.StartOfUtcWeekTimeSpan() &&
               startOfWeekTimeSpan < weekdayStartStopPair.CloseTime.StartOfUtcWeekTimeSpan();
    }
}
