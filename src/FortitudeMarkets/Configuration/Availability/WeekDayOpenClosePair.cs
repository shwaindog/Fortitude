// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;

#endregion

namespace FortitudeMarkets.Configuration.Availability;

public struct WeekDayOpenClosePair
{
    public WeekDayOpenClosePair(TimeZonedWeekDayTime openTime, TimeZonedWeekDayTime closeTime)
    {
        CloseTime = closeTime;
        OpenTime  = openTime;
    }

    public TimeZonedWeekDayTime OpenTime  { get; }
    public TimeZonedWeekDayTime CloseTime { get; }
}

public class WeekDayOpenClosePairComparer : IComparer<WeekDayOpenClosePair>
{
    public int Compare
        (WeekDayOpenClosePair x, WeekDayOpenClosePair y) =>
        x.OpenTime.StartOfUtcWeekTimeSpan().CompareTo(y.OpenTime.StartOfUtcWeekTimeSpan());
}

public static class WeekDayOpenClosePairExtensions
{
    public static bool WithinOpeningHours(this WeekDayOpenClosePair weeklyDayOpenClosePair, DateTimeOffset dateTimeOffset)
    {
        var startOfWeekTimeSpan = dateTimeOffset.TimeSinceUtcStartOfWeek();
        return startOfWeekTimeSpan >= weeklyDayOpenClosePair.OpenTime.StartOfUtcWeekTimeSpan() &&
               startOfWeekTimeSpan < weeklyDayOpenClosePair.CloseTime.StartOfUtcWeekTimeSpan();
    }

    public static bool WithinOpeningHours(this WeekDayOpenClosePair weeklyDayOpenClosePair, DateTime dateTime)
    {
        var startOfWeekTimeSpan = dateTime.TimeSinceUtcStartOfWeek();
        return startOfWeekTimeSpan >= weeklyDayOpenClosePair.OpenTime.StartOfUtcWeekTimeSpan() &&
               startOfWeekTimeSpan < weeklyDayOpenClosePair.CloseTime.StartOfUtcWeekTimeSpan();
    }
}
