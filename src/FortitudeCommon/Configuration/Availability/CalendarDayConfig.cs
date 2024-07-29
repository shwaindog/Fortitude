// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Configuration.Availability;

public interface ICalendarDayConfig
{
    Month Month { get; }
    byte  Day   { get; }

    bool DateMatches(DateTime check);
    bool DateMatches(DateTimeOffset check);
}

public struct CalendarDay
{
    public CalendarDay(ICalendarDayConfig toClone)
    {
        Month = toClone.Month;
        Day   = toClone.Day;
    }

    public CalendarDay(Month month, byte dayOfMonth)
    {
        Month = month;
        Day   = dayOfMonth;
    }

    public Month Month { get; }
    public byte  Day   { get; }
}

public class CalendarDayConfig : ConfigSection, ICalendarDayConfig
{
    public CalendarDayConfig(IConfigurationRoot root, string path) : base(root, path)
    {
        Month = Month.January;
        Day   = 1;
    }

    public CalendarDayConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDayConfig(ushort year, Month month, byte dayOfMonth)
    {
        Month = month;
        Day   = dayOfMonth;
    }

    public CalendarDayConfig(ICalendarDayConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Month = toClone.Month;
        Day   = toClone.Day;
    }

    public CalendarDayConfig(ICalendarDayConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDayConfig(Month month, byte dayOfMonth)
    {
        Month = month;
        Day   = dayOfMonth;
    }

    public Month Month
    {
        get => Enum.TryParse<Month>(this[nameof(Month)]!, out var month) ? month : Month.January;
        set => this[nameof(Month)] = value.ToString();
    }

    public byte Day
    {
        get
        {
            var checkValue = this[nameof(Day)];
            return checkValue != null ? byte.Parse(checkValue) : (byte)1;
        }
        set
        {
            if (value == 0 || value > 31) throw new ArgumentException("Expected day to be between 1 and 31");
            this[nameof(Day)] = value.ToString();
        }
    }

    public bool DateMatches(DateTimeOffset check) => check.Month == (int)Month && check.Day == Day;

    public bool DateMatches(DateTime check) => check.Month == (int)Month && check.Day == Day;

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(Month)] = null;
        root[path + ":" + nameof(Day)]   = null;
    }

    public static implicit operator CalendarDay(CalendarDayConfig toConvert) => new(toConvert);
}

public static class CalendarDayExtensions
{
    public static bool DateTimeMatches(this CalendarDay monthDay, DateTime check) => check.Month == (int)monthDay.Month && check.Day == monthDay.Day;
}
