using System.Text.Json.Serialization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

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

public interface ICalendarDateHolidayConfig : ICalendarDateMatchConfig, ICloneable<ICalendarDateHolidayConfig>, IStyledToStringObject
{
    public const short DefaultFirstNonWeekdayCarry  = 2;
    public const short DefaultSecondNonWeekdayCarry = 1;

    NamedHoliday HolidayName { get; set; }

    short CarryWeekendDirection  { get; set; }

    new ICalendarDateHolidayConfig Clone();
}


public readonly struct CalendarDateHoliday
{
    public CalendarDateHoliday(ICalendarDateHolidayConfig toClone)
    {
        Year  = toClone.Year;
        Month = toClone.Month;
        Day   = toClone.Day;

        FloatingWeekday = toClone.FloatingWeekday;

        HolidayName = toClone.HolidayName;
    }

    // ReSharper disable once ConvertToPrimaryConstructor
    public CalendarDateHoliday
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

    public override string ToString() => 
        $"{nameof(CalendarDateHoliday)}{{{nameof(HolidayName)}: {HolidayName}, {nameof(Year)}: {Year}, {nameof(Month)}: {Month}, " +
        $"{nameof(Day)}: {Day}, {nameof(FloatingWeekday)}: {FloatingWeekday}}}";
}


public class CalendarDateHolidayConfig : CalendarDateMatchConfig, ICalendarDateHolidayConfig
{
    public CalendarDateHolidayConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public CalendarDateHolidayConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public CalendarDateHolidayConfig
    (ushort year, MonthFlags month, byte dayOfMonth,
        MonthFloatingWeekday? dayOccurenceInMonth = null, NamedHoliday holidayName = NamedHoliday.Unknown) 
        : base(year, month, dayOfMonth, dayOccurenceInMonth)
    {
        HolidayName = holidayName;
    }

    public CalendarDateHolidayConfig(ICalendarDateHolidayConfig toClone, IConfigurationRoot root, string path) 
        : base(toClone, root, path)
    {
        HolidayName = toClone.HolidayName;
    }

    public CalendarDateHolidayConfig(ICalendarDateHolidayConfig toClone) 
        : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    public NamedHoliday HolidayName
    {
        get
        {
            var checkValue = this[nameof(HolidayName)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<NamedHoliday>(checkValue!) : NamedHoliday.Unknown;
        }
        set => this[nameof(HolidayName)] = value.ToString();
    }

    
    public short CarryWeekendDirection
    {
        get
        {
            var checkValue = this[nameof(CarryWeekendDirection)];
            return checkValue.IsNotNullOrEmpty() ? short.Parse(checkValue!) : ICalendarDateHolidayConfig.DefaultFirstNonWeekdayCarry;
        }
        set => this[nameof(CarryWeekendDirection)] = value.ToString();
    }

    public new static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(HolidayName)}"]     = null;
        CalendarDateMatchConfig.ClearValues(root, path);
    }

    ICalendarDateHolidayConfig ICloneable<ICalendarDateHolidayConfig>.Clone() => Clone();

    ICalendarDateHolidayConfig ICalendarDateHolidayConfig.Clone() => Clone();

    public override CalendarDateHolidayConfig Clone() => new (this);

    public override bool AreEquivalent(ICalendarDateMatchConfig? other, bool exactTypes = false)
    {
        if (other is not ICalendarDateHolidayConfig holidayConfig) return false;
        var baseSame        = base.AreEquivalent(holidayConfig, exactTypes);
        var holidayNameSame = HolidayName == holidayConfig.HolidayName;

        var allAreSame = baseSame && holidayNameSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ICalendarDateMatchConfig, true);


    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ HolidayName.GetHashCode();
            return hashCode;
        }
    }
    
    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
            .Field.AlwaysAdd(nameof(HolidayName), HolidayName)
            .Field.AlwaysAdd(nameof(Year), Year)
            .Field.AlwaysAdd(nameof(Month), Month)
            .Field.AlwaysAdd(nameof(Day), Day)
            .Field.AlwaysAdd(nameof(FloatingWeekday), FloatingWeekday, MonthFloatingWeekday.Styler)
            .Complete();
    

    public override string ToString() =>
        $"{nameof(CalendarDateHolidayConfig)}{{{nameof(HolidayName)}: {HolidayName}, {nameof(Year)}: {Year}, {nameof(Month)}: {Month}, " +
        $"{nameof(Day)}: {Day}, {nameof(FloatingWeekday)}: {FloatingWeekday}}}";

    public static implicit operator CalendarDateHoliday(CalendarDateHolidayConfig toConvert) => new(toConvert);
}