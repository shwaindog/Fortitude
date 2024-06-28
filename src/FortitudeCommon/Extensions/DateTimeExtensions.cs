// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Extensions;

public static class DateTimeExtensions
{
    public static  long MinTicks           = DateTime.MinValue.Ticks;
    public static  long MaxTicks           = DateTime.MaxValue.Ticks;
    private static long fiveMinuteTicks    = TimeSpan.TicksPerMinute * 5;
    private static long tenMinuteTicks     = TimeSpan.TicksPerMinute * 10;
    private static long fifteenMinuteTicks = TimeSpan.TicksPerMinute * 15;
    private static long thirtyMinuteTicks  = TimeSpan.TicksPerMinute * 30;
    private static long fourHourTicks      = TimeSpan.TicksPerHour * 4;
    private static long twelveHourTicks    = TimeSpan.TicksPerHour * 12;

    public static DateTime TruncToSecondBoundary(this DateTime allTicks)   => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerSecond));
    public static DateTime TruncTo5SecondBoundary(this DateTime allTicks)  => allTicks.AddTicks(-(allTicks.Ticks % 5 * TimeSpan.TicksPerSecond));
    public static DateTime TruncTo10SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % 10 * TimeSpan.TicksPerSecond));
    public static DateTime TruncTo15SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % 15 * TimeSpan.TicksPerSecond));
    public static DateTime TruncTo30SecondBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % 30 * TimeSpan.TicksPerSecond));

    public static DateTime TruncToMinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerMinute));

    public static DateTime TruncTo5MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % fiveMinuteTicks));

    public static DateTime TruncTo10MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % tenMinuteTicks));

    public static DateTime TruncTo15MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % fifteenMinuteTicks));

    public static DateTime TruncTo30MinuteBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % thirtyMinuteTicks));

    public static DateTime TruncTo1HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % TimeSpan.TicksPerHour));

    public static DateTime TruncTo4HourBoundary(this DateTime allTicks)  => allTicks.AddTicks(-(allTicks.Ticks % fourHourTicks));
    public static DateTime TruncTo12HourBoundary(this DateTime allTicks) => allTicks.AddTicks(-(allTicks.Ticks % twelveHourTicks));

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

    public static DateTime TruncToMonthBoundary(this DateTime allTicks) => allTicks.Date.AddDays(-allTicks.Date.Day + 1);

    public static DateTime TruncToQuarterBoundary(this DateTime allTicks)
    {
        var quarterMonth = allTicks.Month / 4 * 4;
        return new DateTime(allTicks.Year, quarterMonth, 1);
    }

    public static DateTime TruncToYearBoundary(this DateTime allTicks)  => allTicks.Date.AddDays(-allTicks.Date.DayOfYear + 1);
    public static DateTime TruncTo5YearBoundary(this DateTime allTicks) => allTicks.TruncToYearBoundary().AddYears(-(allTicks.Year % 5) + 1);

    public static DateTime TruncToDecadeBoundary(this DateTime allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 10));

    public static DateTime TruncToCenturyBoundary(this DateTime allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 100));

    public static DateTime TruncToMillenniumBoundary(this DateTime allTicks) =>
        allTicks.TruncToYearBoundary().AddYears(-(allTicks.TruncToYearBoundary().Year % 1000));


    public static DateTime TruncToFirstSundayInMonth(this DateTime dateTime)
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

    public static DateTime TruncToFirstSundayInDecade(this DateTime dateTime)
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
        return DateTime.FromBinary(cappedTicks);
    }

    public static DateTime Min(this DateTime first, DateTime? optionalSecond)
    {
        if (optionalSecond == null) return first;
        var maxTicks = Math.Min(first.Ticks, optionalSecond.Value.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTime Min(this DateTime first, DateTime second)
    {
        var maxTicks = Math.Min(first.Ticks, second.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTime Min(this DateTime first, DateTime second, DateTime third)
    {
        var maxTicks = Math.Min(Math.Min(first.Ticks, second.Ticks), third.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTime Max(this DateTime first, DateTime? optionalSecond)
    {
        if (optionalSecond == null) return first;
        var maxTicks = Math.Max(first.Ticks, optionalSecond.Value.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTime Max(this DateTime first, DateTime second)
    {
        var maxTicks = Math.Max(first.Ticks, second.Ticks);
        return DateTime.FromBinary(maxTicks);
    }

    public static DateTime Max(this DateTime first, DateTime second, DateTime third)
    {
        var maxTicks = Math.Max(Math.Max(first.Ticks, second.Ticks), third.Ticks);
        return DateTime.FromBinary(maxTicks);
    }
}
