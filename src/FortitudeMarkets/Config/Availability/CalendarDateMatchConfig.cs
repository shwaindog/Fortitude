// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Configuration;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.Availability;

// ReSharper disable UnusedMember.Global
[JsonConverter(typeof(JsonStringEnumConverter<NamedHoliday>))]
public enum NamedHoliday
{
    Unknown
  , SpecialEvent
  , SecuritySituation
  , NationalDayOfMourning
  , Olympics
  , EasterGoodFriday
  , EasterMonday
  , NewYearsDay
  , ChristmasDay
  , LabourDay
  , LaborDay
  , MayDay
  , VersakDay
  , EidalFitr                // Islamic End of Ramadan -  It falls on the first day of Shawwal, the tenth month of the Islamic calendar.
  , EidalAdha                // Islamic - It Falls on the 10th of Dhu al-Hijja, the twelfth and final month of the Islamic calendar.
  , ChineseNewYear           // Chinese Spring Festival 1st day of 1st Luna-solar month
  , ChineseTombSweeping      // aka Chingming 4~6 April
  , ChineseMidAutumnFestival // 15th day of 8th Luna-solar month
  , NationalDay              // Depending on country can be 26th Jan, 1st October...
  , PresidentsDay
  , MemorialDay
  , AnzacDay
  , Thanksgiving
  , IndependenceDay
  , KingsBirthday // Monarch or Martin Luther
  , VictoryDay
  , VictoriaDay
  , CivicDay
  , Armistice
  , BoxingDay
  , BankHoliday
  , AssumptionDay
  , AllSaintsDay
  , GermanEpiphany
  , WomensDay
  , ChildrensDay
  , WhitMonday
  , CorpusChristi
  , AugsburgerPeaceFestival
  , ReformationDay
  , RepentenceAndPrayerDay
  , TruthAndReconciliationDay
}
// ReSharper restore UnusedMember.Global

public interface ICalendarDateMatchConfig : IInterfacesComparable<ICalendarDateMatchConfig>
{
    public const short DefaultFirstNonWeekdayCarry  = 2;
    public const short DefaultSecondNonWeekdayCarry = 1;

    NamedHoliday HolidayName { get; set; }

    ushort?    Year  { get; set; }
    MonthFlags Month { get; set; }
    byte?      Day   { get; set; }

    short FirstNonWeekdayCarry  { get; set; }
    short SecondNonWeekdayCarry { get; set; }

    MonthFloatingWeekday? FloatingWeekday { get; set; }

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

        FloatingWeekday = toClone.FloatingWeekday;

        HolidayName = toClone.HolidayName;
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public CalendarDateMatch
    (ushort? year = null, MonthFlags month = MonthFlags.AllMonths, byte? dayOfMonth = null
      , MonthFloatingWeekday? floatingWeekday = null, NamedHoliday holidayName = NamedHoliday.Unknown)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;

        FloatingWeekday = floatingWeekday;

        HolidayName = holidayName;
    }

    public NamedHoliday HolidayName { get; }

    public ushort?    Year  { get; }
    public MonthFlags Month { get; }
    public byte?      Day   { get; }


    public MonthFloatingWeekday? FloatingWeekday { get; }
}

public class CalendarDateMatchConfig : ConfigSection, ICalendarDateMatchConfig
{
    public CalendarDateMatchConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public CalendarDateMatchConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDateMatchConfig
    (ushort year, MonthFlags month, byte dayOfMonth,
        MonthFloatingWeekday? dayOccurenceInMonth = null, NamedHoliday holidayName = NamedHoliday.Unknown)
    {
        Year  = year;
        Month = month;
        Day   = dayOfMonth;

        FloatingWeekday = dayOccurenceInMonth;

        HolidayName = holidayName;
    }

    public CalendarDateMatchConfig(ICalendarDateMatchConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;

        FloatingWeekday = toClone.FloatingWeekday;

        HolidayName = toClone.HolidayName;
    }

    public CalendarDateMatchConfig(ICalendarDateMatchConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    public NamedHoliday HolidayName
    {
        get
        {
            var checkValue = this[nameof(HolidayName)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<NamedHoliday>(checkValue!) : NamedHoliday.Unknown;
        }
        set => this[nameof(HolidayName)] = value.ToString();
    }

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

    public short FirstNonWeekdayCarry
    {
        get
        {
            var checkValue = this[nameof(FirstNonWeekdayCarry)];
            return checkValue.IsNotNullOrEmpty() ? short.Parse(checkValue!) : ICalendarDateMatchConfig.DefaultFirstNonWeekdayCarry;
        }
        set => this[nameof(FirstNonWeekdayCarry)] = value.ToString();
    }

    public short SecondNonWeekdayCarry
    {
        get
        {
            var checkValue = this[nameof(SecondNonWeekdayCarry)];
            return checkValue.IsNotNullOrEmpty() ? short.Parse(checkValue!) : ICalendarDateMatchConfig.DefaultSecondNonWeekdayCarry;
        }
        set => this[nameof(SecondNonWeekdayCarry)] = value.ToString();
    }

    public bool DateMatches(DateTimeOffset check) => this.DateTimeMatches(check.DateTime);

    public bool DateMatches(DateTime check) => this.DateTimeMatches(check);

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[path + ":" + nameof(Year)]            = null;
        root[path + ":" + nameof(Month)]           = null;
        root[path + ":" + nameof(Day)]             = null;
        root[path + ":" + nameof(FloatingWeekday)] = null;
        root[path + ":" + nameof(HolidayName)]     = null;
    }

    public bool AreEquivalent(ICalendarDateMatchConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var yearSame         = Year == other.Year;
        var monthSame        = Month == other.Month;
        var daySame          = Day == other.Day;
        var dayOccurenceSame = Equals(FloatingWeekday, other.FloatingWeekday);
        var holidayNameSame  = HolidayName == other.HolidayName;

        var allAreSame = yearSame && monthSame && daySame && dayOccurenceSame && holidayNameSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ICalendarDateMatchConfig, true);


    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = HolidayName.GetHashCode();
            hashCode = (hashCode * 397) ^ Year.GetHashCode();
            hashCode = (hashCode * 397) ^ Month.GetHashCode();
            hashCode = (hashCode * 397) ^ (Day != null ? Day.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (FloatingWeekday != null ? FloatingWeekday.GetHashCode() : 0);
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(CalendarDateMatchConfig)}{{{nameof(HolidayName)}: {HolidayName}, {nameof(Year)}: {Year}, {nameof(Month)}: {Month}, " +
        $"{nameof(Day)}: {Day}, {nameof(FloatingWeekday)}: {FloatingWeekday}}}";

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
        if (yearMatches && monthMatches && !dayMatches && yearMonthDay.FloatingWeekday != null)
        {
            var offsetDayInMonth = yearMonthDay.FloatingWeekday.Value.DayInCurrentMonth(check);
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
            var offsetDayInMonth = yearMonthDay.FloatingWeekday.Value.DayInCurrentMonth(check);
            dayMatches = check.Day == offsetDayInMonth;
        }
        var allAreSame = yearMatches && monthMatches && dayMatches;
        return allAreSame;
    }
}
