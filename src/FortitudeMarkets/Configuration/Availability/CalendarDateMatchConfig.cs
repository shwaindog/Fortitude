// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Configuration.Availability;


public interface ICalendarDateMatchConfig
{
    ushort?    Year  { get; set; }
    MonthFlags Month { get; set; }
    byte?      Day   { get; set; }

    WeekDayOccurenceInMonth? DayOccurenceInMonth { get; set; }

    bool DateMatches(DateTime check);

    bool DateMatches(DateTimeOffset check);
}

public readonly struct CalendarDateMatch
{
    public CalendarDateMatch(ICalendarDateMatchConfig toClone)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;

        DayOccurenceInMonth = toClone.DayOccurenceInMonth;
    }

    public CalendarDateMatch
        (ushort? year = null, MonthFlags month = MonthFlags.AllMonths, byte? dayOfMonth = null, WeekDayOccurenceInMonth? dayOccurenceInMonth = null)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;

        DayOccurenceInMonth = dayOccurenceInMonth;
    }

    public ushort?    Year  { get; }
    public MonthFlags Month { get; }
    public byte?      Day   { get; }


    public WeekDayOccurenceInMonth? DayOccurenceInMonth { get; }
}

public class CalendarDateMatchConfig : ConfigSection, ICalendarDateMatchConfig
{
    public CalendarDateMatchConfig(IConfigurationRoot root, string path) : base(root, path)
    {
        Year  = 0;
        Month = MonthFlags.January;
        Day   = 1;
    }

    public CalendarDateMatchConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDateMatchConfig(ushort year, MonthFlags month, byte dayOfMonth)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;
    }

    public CalendarDateMatchConfig(ICalendarDateMatchConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;
    }

    public CalendarDateMatchConfig(ICalendarDateMatchConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort? Year
    {
        get
        {
            var checkValue = this[nameof(Year)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue!) : null;
        }
        set => this[nameof(Year)] = value.ToString();
    }

    public MonthFlags Month
    {
        get
        {
            var checkValue = this[nameof(Year)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<MonthFlags>(checkValue!) : MonthFlags.None;
        }
        set => this[nameof(Month)] = value.ToString();
    }

    public byte? Day
    {
        get
        {
            var checkValue = this[nameof(Day)];
            return checkValue.IsNotNullOrEmpty() ? byte.Parse(checkValue!) : null;
        }
        set
        {
            if (value == 0 || value > 31) throw new ArgumentException("Expected day to be between 1 and 31");
            this[nameof(Day)] = value.ToString();
        }
    }

    public WeekDayOccurenceInMonth? DayOccurenceInMonth
    {
        get
        {
            var checkValue = this[nameof(DayOccurenceInMonth)];
            return checkValue.IsNotNullOrEmpty() ? checkValue.ToWeekDayOccurenceInMonth() : null;
        }
        set => this[nameof(DayOccurenceInMonth)] = value.ToConfigString();
    }


    public bool DateMatches(DateTimeOffset check) => this.DateTimeMatches(check.DateTime);

    public bool DateMatches(DateTime check) => this.DateTimeMatches(check);

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(Year)]                = null;
        root[path + ":" + nameof(Month)]               = null;
        root[path + ":" + nameof(Day)]                 = null;
        root[path + ":" + nameof(DayOccurenceInMonth)] = null;
    }

    public static implicit operator CalendarDateMatch(CalendarDateMatchConfig toConvert) => new(toConvert);
}

public static class CalendarDateExtensions
{
    public static bool DateTimeMatches
        (this CalendarDateMatchConfig yearMonthDay, DateTime check)
    {
        var yearMatches  = (yearMonthDay.Year == check.Year || yearMonthDay.Year == null);
        var monthMatches = yearMonthDay.Month.HasAnyOf(check.Month.ToMonthsFlags());
        var dayMatches   = check.Day == yearMonthDay.Day;
        if (yearMatches && monthMatches && !dayMatches && yearMonthDay.DayOccurenceInMonth != null)
        {
            var offsetDayInMonth = yearMonthDay.DayOccurenceInMonth.Value.DayInCurrentMonth(check);
            dayMatches = check.Day == offsetDayInMonth;
        }
        var allAreSame = yearMatches && monthMatches && dayMatches;
        return allAreSame;
    }

    public static bool DateTimeMatches
        (this CalendarDateMatch yearMonthDay, DateTime check)
    {
        var yearMatches  = (yearMonthDay.Year == check.Year || yearMonthDay.Year == null);
        var monthMatches = yearMonthDay.Month.HasAnyOf(check.Month.ToMonthsFlags());
        var dayMatches   = check.Day == yearMonthDay.Day;
        if (yearMatches && monthMatches && !dayMatches && yearMonthDay.DayOccurenceInMonth != null)
        {
            var offsetDayInMonth = yearMonthDay.DayOccurenceInMonth.Value.DayInCurrentMonth(check);
            dayMatches = check.Day == offsetDayInMonth;
        }
        var allAreSame = yearMatches && monthMatches && dayMatches;
        return allAreSame;
    }
}
