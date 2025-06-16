// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;

namespace FortitudeMarkets.Config.Availability;


[JsonConverter(typeof(JsonStringEnumConverter<MonthlyFloatingWeekDayOccurence>))]
public enum MonthlyFloatingWeekDayOccurence : short
{
    None              = 0
  , SecondLastInMonth = -2
  , LastInMonth       = -1
  , FirstInMonth      = 1
  , SecondInMonth     = 2
  , ThirdInMonth      = 3
  , FourthInMonth     = 4
}

public readonly struct MonthFloatingWeekday(MonthlyFloatingWeekDayOccurence floatingWeekDayOccurenceInMonth, DayOfWeek systemDayOfWeek) : IEquatable<MonthFloatingWeekday>
{
    public readonly MonthlyFloatingWeekDayOccurence OccurenceInMonth = floatingWeekDayOccurenceInMonth;

    public readonly DayOfWeek Weekday = systemDayOfWeek;

    public bool Equals(MonthFloatingWeekday other) => OccurenceInMonth == other.OccurenceInMonth && Weekday == other.Weekday;

    public override bool Equals(object? obj) => obj is MonthFloatingWeekday other && Equals(other);

    public override int GetHashCode()
    {
        unchecked
        {
            return ((int)OccurenceInMonth * 397) ^ (int)Weekday;
        }
    }

    public override string ToString() => $"{nameof(MonthFloatingWeekday)}{{{nameof(OccurenceInMonth)}: {OccurenceInMonth}, {nameof(Weekday)}: {Weekday}}}";
}

public static class MonthFloatingWeekdayExtensions
{
    public static MonthFloatingWeekday? ToWeekDayOccurenceInMonth(this string? csvValues)
    {
        if (csvValues == null) return null;
        var split = csvValues.Split(',');
        if (csvValues.Length != 2) return null;
        if (!Enum.TryParse<MonthlyFloatingWeekDayOccurence>(split[1], out var monthlyWeekDayOccurence))
        {
            return null;
        }
        if (!Enum.TryParse<DayOfWeek>(split[1], out var dayOfWeek))
        {
            return null;
        }
        return new MonthFloatingWeekday(monthlyWeekDayOccurence, dayOfWeek);
    }

    public static string? ToConfigString(this MonthFloatingWeekday? weekDayOccurenceInMonth)
    {
        if (weekDayOccurenceInMonth == null) return null;
        return $"{weekDayOccurenceInMonth.Value.OccurenceInMonth},{weekDayOccurenceInMonth.Value.Weekday}";
    }

    public static int DayInCurrentMonth(this MonthFloatingWeekday monthFloatingWeekdayOffset, int year, int month)
    {
        var fromYear          = year;
        var fromMonth         = month;
        var fromNextMonth     = fromMonth + 1 % 12;
        var fromNextMonthYear = fromNextMonth == 1 ? fromYear + 1 : fromYear;
        var fromDate =
            monthFloatingWeekdayOffset.OccurenceInMonth switch
            {
                MonthlyFloatingWeekDayOccurence.LastInMonth or
                    MonthlyFloatingWeekDayOccurence.SecondLastInMonth => new DateTime(fromNextMonthYear, fromNextMonth, 1).AddDays(-1)
              , _ => new DateTime(fromYear, fromMonth, 1)
            };
        DayOfWeek searchDay = monthFloatingWeekdayOffset.Weekday;
        switch (monthFloatingWeekdayOffset.OccurenceInMonth)
        {
            case MonthlyFloatingWeekDayOccurence.SecondLastInMonth:
                int skip = 1;
                while (skip > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) skip--;
                    fromDate = fromDate.AddDays(-1);
                }
                return fromDate.Day;
            case MonthlyFloatingWeekDayOccurence.LastInMonth:
                while (fromDate.DayOfWeek != searchDay)
                {
                    fromDate = fromDate.AddDays(-1);
                }
                return fromDate.Day;
            case MonthlyFloatingWeekDayOccurence.FirstInMonth:
                while (fromDate.DayOfWeek != searchDay)
                {
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
            case MonthlyFloatingWeekDayOccurence.SecondInMonth:
                int skipNext = 1;
                while (skipNext > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) skipNext--;
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
            case MonthlyFloatingWeekDayOccurence.ThirdInMonth:
                int jump = 2;
                while (jump > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) jump--;
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
            case MonthlyFloatingWeekDayOccurence.FourthInMonth:
                int ignore = 3;
                while (ignore > 0 || fromDate.DayOfWeek != searchDay)
                {
                    if (fromDate.DayOfWeek == searchDay) ignore--;
                    fromDate = fromDate.AddDays(1);
                }
                return fromDate.Day;
        }
        return -1;
    }


}
