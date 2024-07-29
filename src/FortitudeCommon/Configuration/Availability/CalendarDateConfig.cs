// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Configuration.Availability;

public interface ICalendarDateConfig
{
    ushort Year  { get; set; }
    Month  Month { get; set; }
    byte   Day   { get; set; }

    bool DateMatches(DateTime check);
    bool DateMatches(DateTimeOffset check);
}

public readonly struct CalendarDate
{
    public CalendarDate(ICalendarDateConfig toClone)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;
    }

    public CalendarDate(ushort year, Month month, byte dayOfMonth)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;
    }

    public ushort Year  { get; }
    public Month  Month { get; }
    public byte   Day   { get; }
}

public class CalendarDateConfig : ConfigSection, ICalendarDateConfig
{
    public CalendarDateConfig(IConfigurationRoot root, string path) : base(root, path)
    {
        Year  = 0;
        Month = Month.January;
        Day   = 1;
    }

    public CalendarDateConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDateConfig(ushort year, Month month, byte dayOfMonth)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;
    }

    public CalendarDateConfig(ICalendarDateConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;
    }

    public CalendarDateConfig(ICalendarDateConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort Year
    {
        get
        {
            var checkValue = this[nameof(Year)];
            return checkValue != null ? ushort.Parse(checkValue) : (ushort)0;
        }
        set => this[nameof(Year)] = value.ToString();
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
            return checkValue != null ? byte.Parse(checkValue) : (byte)0;
        }
        set
        {
            if (value == 0 || value > 31) throw new ArgumentException("Expected day to be between 1 and 31");
            this[nameof(Day)] = value.ToString();
        }
    }

    public bool DateMatches(DateTimeOffset check) => Year == (ushort)check.Year && check.Month == (int)Month && check.Day == Day;
    public bool DateMatches(DateTime check)       => Year == (ushort)check.Year && check.Month == (int)Month && check.Day == Day;

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(Year)]  = null;
        root[path + ":" + nameof(Month)] = null;
        root[path + ":" + nameof(Day)]   = null;
    }

    public static implicit operator CalendarDate(CalendarDateConfig toConvert) => new(toConvert);
}

public static class CalendarDateExtensions
{
    public static bool DateTimeMatches
        (this CalendarDate yearMonthDay, DateTime check) =>
        yearMonthDay.Year == (ushort)check.Year && check.Month == (int)yearMonthDay.Month && check.Day == yearMonthDay.Day;
}
