// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Extensions;

public static class DateTimeExtensions
{
    private const long FiveSecondsTicks    = TimeSpan.TicksPerSecond * 5;
    private const long TenSecondsTicks     = TimeSpan.TicksPerSecond * 10;
    private const long FifteenSecondsTicks = TimeSpan.TicksPerSecond * 15;
    private const long ThirtySecondsTicks  = TimeSpan.TicksPerSecond * 30;
    private const long FiveMinuteTicks     = TimeSpan.TicksPerMinute * 5;
    private const long TenMinuteTicks      = TimeSpan.TicksPerMinute * 10;
    private const long FifteenMinuteTicks  = TimeSpan.TicksPerMinute * 15;
    private const long ThirtyMinuteTicks   = TimeSpan.TicksPerMinute * 30;
    private const long FourHourTicks       = TimeSpan.TicksPerHour * 4;
    private const long TwelveHourTicks     = TimeSpan.TicksPerHour * 12;

    public static readonly long MinTicks = DateTime.MinValue.Ticks;
    public static readonly long MaxTicks = DateTime.MaxValue.Ticks;

    public static bool IsUnixEpochOrDefault(this DateTime checkDateTime) =>
        checkDateTime == DateTime.MinValue || checkDateTime == DateTime.UnixEpoch;

    public static bool IsNotUnixEpochOrDefault(this DateTime checkDateTime) => !checkDateTime.IsUnixEpochOrDefault();

    public static bool IsNullOrUnixEpochOrDefault(this DateTime? checkDateTime) =>
        checkDateTime == null || checkDateTime == DateTime.MinValue || checkDateTime == DateTime.UnixEpoch;

    public static bool IsNotNullOrUnixEpochOrDefault(this DateTime? checkDateTime) => !checkDateTime.IsNullOrUnixEpochOrDefault();

    public static DateTime TruncToMicrosecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMicrosecond));

    public static DateTimeOffset TruncToMicrosecondBoundary
        (this DateTimeOffset allTicks) =>
        allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMicrosecond));

    public static DateTime TruncToMillisecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMillisecond));

    public static DateTimeOffset TruncToMillisecondBoundary
        (this DateTimeOffset allTicks) =>
        allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMillisecond));

    public static DateTime TruncToSecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerSecond));

    public static DateTimeOffset TruncToSecondBoundary
        (this DateTimeOffset allTicks) =>
        allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerSecond));

    public static DateTime TruncTo5SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FiveSecondsTicks));

    public static DateTimeOffset TruncTo5SecondBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FiveSecondsTicks));

    public static DateTime TruncTo10SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TenSecondsTicks));

    public static DateTimeOffset TruncTo10SecondBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TenSecondsTicks));

    public static DateTime TruncTo15SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FifteenSecondsTicks));

    public static DateTimeOffset TruncTo15SecondBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FifteenSecondsTicks));

    public static DateTime TruncTo30SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % ThirtySecondsTicks));

    public static DateTimeOffset TruncTo30SecondBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % ThirtySecondsTicks));

    public static DateTime TruncToMinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMinute));

    public static DateTimeOffset TruncToMinuteBoundary
        (this DateTimeOffset allTicks) =>
        allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMinute));

    public static DateTime TruncTo5MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FiveMinuteTicks));

    public static DateTimeOffset TruncTo5MinuteBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FiveMinuteTicks));

    public static DateTime TruncTo10MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TenMinuteTicks));

    public static DateTimeOffset TruncTo10MinuteBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TenMinuteTicks));

    public static DateTime TruncTo15MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FifteenMinuteTicks));

    public static DateTimeOffset TruncTo15MinuteBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FifteenMinuteTicks));

    public static DateTime TruncTo30MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % ThirtyMinuteTicks));

    public static DateTimeOffset TruncTo30MinuteBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % ThirtyMinuteTicks));

    public static DateTime TruncTo1HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerHour));

    public static DateTimeOffset TruncTo1HourBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerHour));

    public static DateTime TruncTo4HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FourHourTicks));

    public static DateTimeOffset TruncTo4HourBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % FourHourTicks));

    public static DateTime TruncTo12HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TwelveHourTicks));

    public static DateTimeOffset TruncTo12HourBoundary(this DateTimeOffset allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TwelveHourTicks));

    public static DateTime TruncToDayBoundary(this DateTime allTicks) => allTicks.Date;

    public static DateTime TruncToWeekBoundary(this DateTime allTicks)
    {
        var currentDay = allTicks.Date;
        return currentDay.DayOfWeek switch
               {
                   DayOfWeek.Monday    => currentDay.AddDays(-1)
                 , DayOfWeek.Tuesday   => currentDay.AddDays(-2)
                 , DayOfWeek.Wednesday => currentDay.AddDays(-3)
                 , DayOfWeek.Thursday  => currentDay.AddDays(-4)
                 , DayOfWeek.Friday    => currentDay.AddDays(-5)
                 , DayOfWeek.Saturday  => currentDay.AddDays(-6)
                 , _                   => currentDay
               };
    }

    public static DateTimeOffset TruncToWeekBoundary(this DateTimeOffset allTicks)
    {
        var currentDay = allTicks.Date;
        return currentDay.DayOfWeek switch
               {
                   DayOfWeek.Monday    => new DateTimeOffset(currentDay.AddDays(-1), allTicks.Offset)
                 , DayOfWeek.Tuesday   => new DateTimeOffset(currentDay.AddDays(-2), allTicks.Offset)
                 , DayOfWeek.Wednesday => new DateTimeOffset(currentDay.AddDays(-3), allTicks.Offset)
                 , DayOfWeek.Thursday  => new DateTimeOffset(currentDay.AddDays(-4), allTicks.Offset)
                 , DayOfWeek.Friday    => new DateTimeOffset(currentDay.AddDays(-5), allTicks.Offset)
                 , DayOfWeek.Saturday  => new DateTimeOffset(currentDay.AddDays(-6), allTicks.Offset)
                 , _                   => new DateTimeOffset(currentDay, allTicks.Offset)
               };
    }

    public static DateTime TruncToMonthBoundary(this DateTime allTicks) => allTicks.Date.AddDays(-allTicks.Date.Day + 1);

    public static DateTimeOffset TruncToMonthBoundary(this DateTimeOffset allTicks) => allTicks.Date.AddDays(-allTicks.Date.Day + 1);

    public static DateTime TruncToQuarterBoundary(this DateTime allTicks)
    {
        var quarterMonth = allTicks.Month / 4 * 4;
        return new DateTime(allTicks.Year, quarterMonth, 1);
    }

    public static DateTimeOffset TruncToQuarterBoundary(this DateTimeOffset allTicks)
    {
        var quarterMonth = allTicks.Month / 4 * 4;
        return new DateTime(allTicks.Year, quarterMonth, 1);
    }

    public static DateTime       TruncToYearBoundary(this DateTime allTicks) => allTicks.Date.AddDays(-allTicks.Date.DayOfYear + 1);
    public static DateTimeOffset TruncToYearBoundary(this DateTimeOffset allTicks) => allTicks.Date.AddDays(-allTicks.Date.DayOfYear + 1);
    public static DateTime       TruncTo5YearBoundary(this DateTime allTicks) => allTicks.TruncToYearBoundary().AddYears(-(allTicks.Year % 5) + 1);

    public static DateTimeOffset TruncTo5YearBoundary
        (this DateTimeOffset allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.Year % 5) + 1);

    public static DateTime TruncToDecadeBoundary(this DateTime allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 10));

    public static DateTimeOffset TruncToDecadeBoundary(this DateTimeOffset allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 10));

    public static DateTime TruncToCenturyBoundary(this DateTime allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 100));

    public static DateTimeOffset TruncToCenturyBoundary(this DateTimeOffset allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 100));

    public static DateTime TruncToMillenniumBoundary(this DateTime allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 1000));

    public static DateTimeOffset TruncToMillenniumBoundary(this DateTimeOffset allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 1000));


    public static DateTime TruncToFirstSundayInMonth(this DateTime dateTime)
    {
        var firstDayOfMonth = dateTime.TruncToMonthBoundary();
        var dayOfWeek       = firstDayOfMonth.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday) return firstDayOfMonth;
        var daysToSunday = 7 - (int)dayOfWeek;
        return firstDayOfMonth.AddDays(daysToSunday);
    }

    public static DateTimeOffset TruncToFirstSundayInMonth(this DateTimeOffset dateTime)
    {
        var firstDayOfMonth = dateTime.TruncToMonthBoundary();
        var dayOfWeek       = firstDayOfMonth.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday) return firstDayOfMonth;
        var daysToSunday = 7 - (int)dayOfWeek;
        return firstDayOfMonth.AddDays(daysToSunday);
    }

    public static DateTime TruncToFirstSundayInYear(this DateTime dateTime)
    {
        var firstDayOfYear = dateTime.TruncToYearBoundary();
        var dayOfWeek      = firstDayOfYear.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday) return firstDayOfYear;
        var daysToSunday = 7 - (int)dayOfWeek;
        return firstDayOfYear.AddDays(daysToSunday);
    }

    public static DateTimeOffset TruncToFirstSundayInYear(this DateTimeOffset dateTime)
    {
        var firstDayOfYear = dateTime.TruncToYearBoundary();
        var dayOfWeek      = firstDayOfYear.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday) return firstDayOfYear;
        var daysToSunday = 7 - (int)dayOfWeek;
        return firstDayOfYear.AddDays(daysToSunday);
    }

    public static DateTime TruncToFirstSundayInDecade(this DateTime dateTime)
    {
        var firstDayOfDecade = dateTime.TruncToDecadeBoundary();
        var dayOfWeek        = firstDayOfDecade.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday) return firstDayOfDecade;
        var daysToSunday = 7 - (int)dayOfWeek;
        return firstDayOfDecade.AddDays(daysToSunday);
    }

    public static DateTimeOffset TruncToFirstSundayInDecade(this DateTimeOffset dateTime)
    {
        var firstDayOfDecade = dateTime.TruncToDecadeBoundary();
        var dayOfWeek        = firstDayOfDecade.DayOfWeek;
        if (dayOfWeek == DayOfWeek.Sunday) return firstDayOfDecade;
        var daysToSunday = 7 - (int)dayOfWeek;
        return firstDayOfDecade.AddDays(daysToSunday);
    }

    public static DateTime CappedTicksToDateTime(this long ticks)
    {
        var cappedTicks = Math.Min(MaxTicks, Math.Max(MinTicks, ticks));
        return DateTime.FromBinary(ticks);
    }

    public static DateTimeOffset CappedTicksToDateTimeOffset(this long ticks)
    {
        var cappedTicks = Math.Min(MaxTicks, Math.Max(MinTicks, ticks));
        return new DateTimeOffset(DateTime.FromBinary(cappedTicks));
    }

    public static DateTime Min(this DateTime first, DateTime? optionalSecond)
    {
        if (optionalSecond == null) return first;
        return first.Min(optionalSecond.Value);
    }

    public static DateTimeOffset Min(this DateTimeOffset first, DateTimeOffset? optionalSecond)
    {
        if (optionalSecond == null) return first;
        return first.Min(optionalSecond.Value);
    }

    public static DateTime Min(this DateTime first, DateTime second)
    {
        var minTicks = Math.Min(first.Ticks, second.Ticks);
        return DateTime.FromBinary(minTicks);
    }

    public static DateTimeOffset Min(this DateTimeOffset first, DateTimeOffset second)
    {
        var minTicks = Math.Min(first.Ticks, second.Ticks);
        return DateTime.FromBinary(minTicks);
    }

    public static DateTime Min(this DateTime first, DateTime second, DateTime third)
    {
        var minTicks = Math.Min(Math.Min(first.Ticks, second.Ticks), third.Ticks);
        return DateTime.FromBinary(minTicks);
    }

    public static DateTimeOffset Min(this DateTimeOffset first, DateTimeOffset second, DateTimeOffset third)
    {
        var minTicks = Math.Min(Math.Min(first.Ticks, second.Ticks), third.Ticks);
        return new DateTimeOffset(DateTime.FromBinary(minTicks));
    }

    public static DateTime Max(this DateTime first, DateTime? optionalSecond)
    {
        if (optionalSecond == null) return first;
        return first.Max(optionalSecond.Value);
    }

    public static DateTimeOffset Max(this DateTimeOffset first, DateTimeOffset? optionalSecond)
    {
        if (optionalSecond == null) return first;
        return first.Max(optionalSecond.Value);
    }

    public static DateTime Max(this DateTime first, DateTime second)
    {
        var maxTicks = Math.Max(first.Ticks, second.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTimeOffset Max(this DateTimeOffset first, DateTimeOffset second)
    {
        var maxTicks = Math.Max(first.Ticks, second.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTime Max(this DateTime first, DateTime second, DateTime third)
    {
        var maxTicks = Math.Max(Math.Max(first.Ticks, second.Ticks), third.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTimeOffset Max(this DateTimeOffset first, DateTimeOffset second, DateTimeOffset third)
    {
        var maxTicks = Math.Max(Math.Max(first.Ticks, second.Ticks), third.Ticks);
        return new DateTimeOffset(DateTime.FromBinary(maxTicks));
    }

    public static TimeSpan TimeSinceUtcStartOfWeek(this DateTime dateTime)
    {
        var utcTime             = dateTime.ToUniversalTime();
        var sinceSundayTimeSpan = TimeSpan.FromDays((int)utcTime.DayOfWeek);
        var sinceDayStart       = utcTime - utcTime.Date;
        return sinceDayStart + sinceSundayTimeSpan;
    }

    public static TimeSpan TimeSinceUtcStartOfWeek(this DateTimeOffset dateTimeOffset)
    {
        var utcTimeOffset       = dateTimeOffset.Offset;
        var sinceSundayTimeSpan = TimeSpan.FromDays((int)dateTimeOffset.DayOfWeek);
        var sinceDayStart       = dateTimeOffset - dateTimeOffset.Date;
        return sinceDayStart + utcTimeOffset + sinceSundayTimeSpan;
    }

    public static TimeSpan TimeToNextUtcStartOfWeek(this DateTime dateTime)
    {
        var utcTime                  = dateTime.ToUniversalTime();
        var timeSpanToSundayNextWeek = TimeSpan.FromDays(7 - (int)utcTime.DayOfWeek);
        var sinceDayStart            = utcTime - utcTime.Date;
        return timeSpanToSundayNextWeek - sinceDayStart;
    }

    public static TimeSpan TimeToNextUtcStartOfWeek(this DateTimeOffset dateTimeOffset)
    {
        var utcTimeOffset            = dateTimeOffset.Offset;
        var timeSpanToSundayNextWeek = TimeSpan.FromDays(7 - (int)dateTimeOffset.DayOfWeek);
        var sinceDayStart            = dateTimeOffset - dateTimeOffset.Date;
        return timeSpanToSundayNextWeek - (sinceDayStart + utcTimeOffset);
    }

    public static DateTime StartOfNextDay(this DateTime dateTime)
    {
        var sinceDayStart     = dateTime - dateTime.Date;
        var remainingTimeSpan = TimeSpan.FromDays(1) - sinceDayStart;
        return dateTime + remainingTimeSpan;
    }

    public static DateTimeOffset StartOfNextDay(this DateTimeOffset dateTimeOffset)
    {
        var sinceDayStart     = dateTimeOffset - dateTimeOffset.Date;
        var remainingTimeSpan = TimeSpan.FromDays(1) - sinceDayStart;
        return dateTimeOffset + remainingTimeSpan;
    }
}
