// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Configuration.Availability;

public enum MonthlyWeekDayOccurence : short
{
    None              = 0
  , SecondLastInMonth = -2
  , LastInMonth       = -1
  , FirstInMonth      = 1
  , SecondInMonth     = 2
  , ThirdInMonth      = 3
}

public readonly struct WeekDayOccurenceInMonth(MonthlyWeekDayOccurence weekDayOccurenceInMonth, DayOfWeek systemDayOfWeek)
{
    public readonly MonthlyWeekDayOccurence OccurenceInMonth = weekDayOccurenceInMonth;

    public readonly DayOfWeek WeekDay = systemDayOfWeek;
}

public static class WeekDayOccurenceInMonthExtensions
{
    public static WeekDayOccurenceInMonth? ToWeekDayOccurenceInMonth(this string? csvValues)
    {
        if (csvValues == null) return null;
        var split = csvValues.Split(',');
        if (csvValues.Length != 2) return null;
        if (!Enum.TryParse<MonthlyWeekDayOccurence>(split[1], out var monthlyWeekDayOccurence))
        {
            return null;
        }
        if (!Enum.TryParse<DayOfWeek>(split[1], out var dayOfWeek))
        {
            return null;
        }
        return new WeekDayOccurenceInMonth(monthlyWeekDayOccurence, dayOfWeek);
    }

    public static string? ToConfigString(this WeekDayOccurenceInMonth? weekDayOccurenceInMonth)
    {
        if (weekDayOccurenceInMonth == null) return null;
        return $"{weekDayOccurenceInMonth.Value.OccurenceInMonth},{weekDayOccurenceInMonth.Value.WeekDay}";
    }

    public static int DayInCurrentMonth(this WeekDayOccurenceInMonth monthOffset, DateTime forThisYearAndMonth)
    {
        var fromYear          = forThisYearAndMonth.Year;
        var fromMonth         = forThisYearAndMonth.Month;
        var fromNextMonth     = fromMonth + 1 % 12;
        var fromNextMonthYear = fromNextMonth == 1 ? fromYear + 1 : fromYear;
        var fromDate =
            monthOffset.OccurenceInMonth switch
            {
                MonthlyWeekDayOccurence.LastInMonth or
                    MonthlyWeekDayOccurence.SecondLastInMonth => new DateTime(fromNextMonthYear, fromNextMonth, 1).AddDays(-1)
              , _ => new DateTime(fromYear, fromMonth, 1)
            };
        DayOfWeek searchDay = monthOffset.WeekDay;
        switch (monthOffset.OccurenceInMonth)
        {
            case MonthlyWeekDayOccurence.SecondLastInMonth:
                int skip = 1;
                while (skip > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) skip--;
                    fromDate = fromDate.AddDays(-1);
                }
                return fromDate.Day;
            case MonthlyWeekDayOccurence.LastInMonth:
                while (fromDate.DayOfWeek != searchDay)
                {
                    fromDate = fromDate.AddDays(-1);
                }
                return fromDate.Day;
            case MonthlyWeekDayOccurence.FirstInMonth:
                while (fromDate.DayOfWeek != searchDay)
                {
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
            case MonthlyWeekDayOccurence.SecondInMonth:
                int skipNext = 1;
                while (skipNext > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) skipNext--;
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
            case MonthlyWeekDayOccurence.ThirdInMonth:
                int jump = 2;
                while (jump > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) jump--;
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
        }
        return -1;
    }
}
