// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.Availability;

public interface ICalendarDateMatchConfig : IInterfacesComparable<ICalendarDateMatchConfig>, ICloneable<ICalendarDateMatchConfig>
{
    ushort?    Year  { get; set; }
    MonthFlags Month { get; set; }
    byte?      Day   { get; set; }

    MonthFloatingWeekday? FloatingWeekday { get; set; }

    bool DateMatches(DateTime check);

    bool DateMatches(DateTimeOffset check);

    byte ResolveDateForYear(ushort year);
}

public readonly struct CalendarDateMatch
{
    public CalendarDateMatch(ICalendarDateMatchConfig toClone)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;

        FloatingWeekday = toClone.FloatingWeekday;
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public CalendarDateMatch
    (ushort? year = null, MonthFlags month = MonthFlags.AllMonths, byte? dayOfMonth = null
      , MonthFloatingWeekday? floatingWeekday = null)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;

        FloatingWeekday = floatingWeekday;
    }

    public ushort?    Year  { get; }

    public MonthFlags Month { get; }

    public byte?      Day   { get; }

    public MonthFloatingWeekday? FloatingWeekday { get; }
}

public class CalendarDateMatchConfig : ConfigSection, ICalendarDateMatchConfig
{
    public CalendarDateMatchConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public CalendarDateMatchConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDateMatchConfig(ushort year, MonthFlags month, byte dayOfMonth, MonthFloatingWeekday? dayOccurenceInMonth = null)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;

        FloatingWeekday = dayOccurenceInMonth;
    }

    public CalendarDateMatchConfig(ICalendarDateMatchConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;

        FloatingWeekday = toClone.FloatingWeekday;
    }

    public CalendarDateMatchConfig(ICalendarDateMatchConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ushort? Year
    {
        get
        {
            var checkValue = this[nameof(Year)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue!) : null;
        }
        set => this[nameof(Year)] = value?.ToString() ?? "";
    }

    public MonthFlags Month
    {
        get
        {
            var checkValue = this[nameof(Month)];
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

    public MonthFloatingWeekday? FloatingWeekday
    {
        get
        {
            if (GetSection(nameof(FloatingWeekday)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                DayOfWeekFlags                  weekday         = DayOfWeekFlags.None;
                MonthlyFloatingWeekDayOccurence floatingWeekday = MonthlyFloatingWeekDayOccurence.None;
                foreach (var cs in GetSection(nameof(FloatingWeekday)).GetChildren())
                {
                    switch (cs.Key)
                    {
                        case nameof(MonthFloatingWeekday.Weekday):
                            weekday = cs.Value.IsNotNullOrEmpty() ? Enum.Parse<DayOfWeekFlags>(cs.Value!) : DayOfWeekFlags.None;
                            break;
                        case nameof(MonthFloatingWeekday.OccurenceInMonth):
                            floatingWeekday = cs.Value.IsNotNullOrEmpty()
                                ? Enum.Parse<MonthlyFloatingWeekDayOccurence>(cs.Value!)
                                : MonthlyFloatingWeekDayOccurence.None;
                            break;
                    }
                }
                return new MonthFloatingWeekday(floatingWeekday, weekday.ToSystemDayOfWeek());
            }
            return null;
        }
        set
        {
            if (value == null)
            {
                this[nameof(FloatingWeekday) + ConfigurationPath.KeyDelimiter + nameof(MonthFloatingWeekday.Weekday)]          = "";
                this[nameof(FloatingWeekday) + ConfigurationPath.KeyDelimiter + nameof(MonthFloatingWeekday.OccurenceInMonth)] = "";
            }
            else
            {
                this[nameof(FloatingWeekday) + ConfigurationPath.KeyDelimiter + nameof(MonthFloatingWeekday.Weekday)]
                    = value.Value.Weekday.ToString();

                this[nameof(FloatingWeekday) + ConfigurationPath.KeyDelimiter + nameof(MonthFloatingWeekday.OccurenceInMonth)]
                    = value.Value.OccurenceInMonth.ToString();
            }
        }
    }

    public byte ResolveDateForYear(ushort year)
    {
        if (Day != null) return Day.Value;
        var floatingWeekday = FloatingWeekday;
        if (floatingWeekday == null) throw new ArgumentException("Expected Day or FloatingWeekday to be set");
        return (byte)floatingWeekday.Value.DayInCurrentMonth(year, Month.ToCalendarMonth());
    }

    public bool DateMatches(DateTimeOffset check) => this.DateTimeMatches(check.DateTime);

    public bool DateMatches(DateTime check) => this.DateTimeMatches(check);

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(Year)]            = null;
        root[path + ":" + nameof(Month)]           = null;
        root[path + ":" + nameof(Day)]             = null;
        root[path + ":" + nameof(FloatingWeekday)] = null;
    }

    object ICloneable.Clone() => Clone();

    ICalendarDateMatchConfig ICloneable<ICalendarDateMatchConfig>.Clone() => Clone();

    public virtual CalendarDateMatchConfig Clone() => new(this);

    public virtual bool AreEquivalent(ICalendarDateMatchConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var yearSame         = Year == other.Year;
        var monthSame        = Month == other.Month;
        var daySame          = Day == other.Day;
        var dayOccurenceSame = Equals(FloatingWeekday, other.FloatingWeekday);

        var allAreSame = yearSame && monthSame && daySame && dayOccurenceSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ICalendarDateMatchConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Month.GetHashCode();
            hashCode = (hashCode * 397) ^ Year.GetHashCode();
            hashCode = (hashCode * 397) ^ (Day != null ? Day.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (FloatingWeekday != null ? FloatingWeekday.GetHashCode() : 0);
            return hashCode;
        }
    }

    protected string CalendarDateMatchConfigToStringMembers =>
        $"{nameof(Year)}: {Year}, {nameof(Month)}: {Month}, {nameof(Day)}: {Day}, {nameof(FloatingWeekday)}: {FloatingWeekday}";


    public override string ToString() =>
        $"{nameof(CalendarDateMatchConfig)}{{{CalendarDateMatchConfigToStringMembers}}}";

    public static implicit operator CalendarDateMatch(CalendarDateMatchConfig toConvert) => new(toConvert);
}

public static class CalendarDateMatchConfigExtensions
{
    public static bool DateTimeMatches
        (this CalendarDateMatchConfig yearMonthDay, DateTime check)
    {
        var yearMatches  = (yearMonthDay.Year == check.Year || yearMonthDay.Year == null);
        var monthMatches = yearMonthDay.Month.HasAnyOf(check.Month.ToMonthsFlags());
        var dayMatches   = check.Day == yearMonthDay.Day;
        if (yearMatches && monthMatches && !dayMatches && yearMonthDay.FloatingWeekday != null)
        {
            var offsetDayInMonth = yearMonthDay.FloatingWeekday.Value.DayInCurrentMonth(check.Year, check.Month);
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
        if (yearMatches && monthMatches && !dayMatches && yearMonthDay.FloatingWeekday != null)
        {
            var offsetDayInMonth = yearMonthDay.FloatingWeekday.Value.DayInCurrentMonth(check.Year, check.Month);
            dayMatches = check.Day == offsetDayInMonth;
        }
        var allAreSame = yearMatches && monthMatches && dayMatches;
        return allAreSame;
    }
}
