// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.Extensions;

namespace FortitudeMarkets.Config.Availability;

public readonly struct TimeZonedWeekDayTime
{
    public TimeZonedWeekDayTime(TimeZoneInfo timeZone, DayOfWeekFlags dayOfWeekFlags, TimeSpan timeOfDay)
    {
        DayOfWeek = dayOfWeekFlags.ToSystemDayOfWeek();
        TimeOfDay = timeOfDay;
        TimeZone  = timeZone;
    }

    public TimeZonedWeekDayTime(TimeZoneInfo timeZone, DayOfWeek dayOfWeek, TimeSpan timeOfDay)
    {
        DayOfWeek = dayOfWeek;
        TimeOfDay = timeOfDay;
        TimeZone  = timeZone;
    }

    public TimeZoneInfo TimeZone { get; }

    public DayOfWeek DayOfWeek { get; }

    public TimeSpan  TimeOfDay { get; }

    public static bool operator <(TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) => lhs.FromMondayWeekTimeSpan() < rhs.FromMondayWeekTimeSpan();

    public static bool operator >(TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) => lhs.FromMondayWeekTimeSpan() > rhs.FromMondayWeekTimeSpan();

    public static bool operator == (TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) =>
        lhs.FromMondayWeekTimeSpan() == rhs.FromMondayWeekTimeSpan();

    public static bool operator != (TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) =>
        lhs.FromMondayWeekTimeSpan() != rhs.FromMondayWeekTimeSpan();

    public bool Equals(TimeZonedWeekDayTime other) => this.FromMondayWeekTimeSpan() == other.FromMondayWeekTimeSpan();

    public override bool Equals(object? obj) => obj is TimeZonedWeekDayTime other && Equals(other);

    public override int GetHashCode() => this.FromMondayWeekTimeSpan().GetHashCode();

    public override string ToString() =>
        $"{nameof(TimeZonedWeekDayTime)}({nameof(TimeZone)}: {TimeZone}, {nameof(DayOfWeek)}: {DayOfWeek}, {nameof(TimeOfDay)}: {TimeOfDay})";
}

public static class TimeZonedWeekDayTimeExtensions
{
    // Since sunday nights 1-2am is when daylight savings clocks are alter use Monday as the anchor for relative timespans
    public static TimeSpan FromMondayWeekTimeSpan(this TimeZonedWeekDayTime timeZonedWeekDay)
    {
        var sinceSundayTimeSpan = TimeSpan.FromDays((timeZonedWeekDay.DayOfWeek - DayOfWeek.Monday));
        return timeZonedWeekDay.TimeOfDay + sinceSundayTimeSpan;
    }

    public static DateTimeOffset ToTimeInWeek(this TimeZonedWeekDayTime timeZonedWeekDay, DateTimeOffset forTimeInWeek)
    {
        var convertedDate = new DateTimeOffset(TimeZoneInfo.ConvertTime(forTimeInWeek, timeZonedWeekDay.TimeZone).Date
                                             , timeZonedWeekDay.TimeZone!.GetUtcOffset(forTimeInWeek));
        var mondayOfWeek   = convertedDate.TruncToWeekBoundary().AddDays(1); // will add 23 or 25 hours if DST switch
        var convertedTime = mondayOfWeek + timeZonedWeekDay.FromMondayWeekTimeSpan();
        return convertedTime;
    }
}
