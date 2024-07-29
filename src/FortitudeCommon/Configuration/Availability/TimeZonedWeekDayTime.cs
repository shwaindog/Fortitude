// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Configuration.Availability;

public readonly struct TimeZonedWeekDayTime
{
    public TimeZonedWeekDayTime(TimeZoneInfo timeZone, DayOfWeek dayOfWeek, TimeSpan timeOfDay)
    {
        DayOfWeek = dayOfWeek;
        TimeOfDay = timeOfDay;
        TimeZone  = timeZone;
    }

    public TimeZoneInfo TimeZone { get; }

    public DayOfWeek DayOfWeek { get; }
    public TimeSpan  TimeOfDay { get; }

    public static bool operator <(TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) => lhs.StartOfUtcWeekTimeSpan() < rhs.StartOfUtcWeekTimeSpan();

    public static bool operator >(TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) => lhs.StartOfUtcWeekTimeSpan() > rhs.StartOfUtcWeekTimeSpan();

    public static bool operator ==
        (TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) =>
        lhs.StartOfUtcWeekTimeSpan() == rhs.StartOfUtcWeekTimeSpan();

    public static bool operator !=
        (TimeZonedWeekDayTime lhs, TimeZonedWeekDayTime rhs) =>
        lhs.StartOfUtcWeekTimeSpan() != rhs.StartOfUtcWeekTimeSpan();

    public bool Equals(TimeZonedWeekDayTime other) => this.StartOfUtcWeekTimeSpan() == other.StartOfUtcWeekTimeSpan();

    public override bool Equals(object? obj) => obj is TimeZonedWeekDayTime other && Equals(other);

    public override int GetHashCode() => this.StartOfUtcWeekTimeSpan().GetHashCode();

    public override string ToString() =>
        $"{nameof(TimeZonedWeekDayTime)}({nameof(TimeZone)}: {TimeZone}, {nameof(DayOfWeek)}: {DayOfWeek}, {nameof(TimeOfDay)}: {TimeOfDay})";
}

public static class TimeZonedWeekDayTimeExtensions
{
    public static TimeSpan StartOfUtcWeekTimeSpan(this TimeZonedWeekDayTime timeZonedWeekDay)
    {
        var utcTimeOffset       = timeZonedWeekDay.TimeZone.BaseUtcOffset;
        var sinceSundayTimeSpan = TimeSpan.FromDays((int)timeZonedWeekDay.DayOfWeek);
        return timeZonedWeekDay.TimeOfDay + utcTimeOffset + sinceSundayTimeSpan;
    }
}
