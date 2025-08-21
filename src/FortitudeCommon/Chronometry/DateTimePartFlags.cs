using FortitudeCommon.Extensions;

namespace FortitudeCommon.Chronometry;

[Flags]
public enum DateTimePartFlags : uint
{
    None           = 0x00_00_00
  , Micros         = 0x00_00_01
  , MicroTens      = 0x00_00_02
  , MicroHundreds  = 0x00_00_04
  , Millis         = 0x00_00_08
  , MillisTens     = 0x00_00_10
  , MillisHundreds = 0x00_00_20
  , Seconds        = 0x00_00_40
  , Minutes        = 0x00_00_80
  , Hours          = 0x00_01_00
  , Days           = 0x00_02_00
  , Weeks          = 0x00_04_00
  , Months         = 0x00_08_00
  , Years          = 0x00_10_00
  , Decades        = 0x00_20_00
  , Centuries      = 0x00_40_00
  , Millennia      = 0x00_80_00
}

public static class DateTimePartFlagsExtensions
{
    public static bool HasMicros(this DateTimePartFlags flags)         => (flags & DateTimePartFlags.Micros) > 0;
    public static bool HasMicroTens(this DateTimePartFlags flags)      => (flags & DateTimePartFlags.MicroTens) > 0;
    public static bool HasMicroHundreds(this DateTimePartFlags flags)  => (flags & DateTimePartFlags.MicroHundreds) > 0;
    public static bool HasMillis(this DateTimePartFlags flags)         => (flags & DateTimePartFlags.Millis) > 0;
    public static bool HasMillisTens(this DateTimePartFlags flags)     => (flags & DateTimePartFlags.MillisTens) > 0;
    public static bool HasMillisHundreds(this DateTimePartFlags flags) => (flags & DateTimePartFlags.MillisHundreds) > 0;
    public static bool HasSeconds(this DateTimePartFlags flags)        => (flags & DateTimePartFlags.Seconds) > 0;
    public static bool HasMinutes(this DateTimePartFlags flags)        => (flags & DateTimePartFlags.Minutes) > 0;
    public static bool HasHours(this DateTimePartFlags flags)          => (flags & DateTimePartFlags.Hours) > 0;
    public static bool HasDays(this DateTimePartFlags flags)           => (flags & DateTimePartFlags.Days) > 0;
    public static bool HasWeeks(this DateTimePartFlags flags)          => (flags & DateTimePartFlags.Weeks) > 0;
    public static bool HasMonths(this DateTimePartFlags flags)         => (flags & DateTimePartFlags.Months) > 0;
    public static bool HasYears(this DateTimePartFlags flags)          => (flags & DateTimePartFlags.Years) > 0;
    public static bool HasDecades(this DateTimePartFlags flags)        => (flags & DateTimePartFlags.Decades) > 0;
    public static bool HasCenturies(this DateTimePartFlags flags)      => (flags & DateTimePartFlags.Centuries) > 0;
    public static bool HasMillennia(this DateTimePartFlags flags)      => (flags & DateTimePartFlags.Millennia) > 0;

    public static DateTimePartFlags Unset(this DateTimePartFlags flags, DateTimePartFlags toUnset) => flags & ~toUnset;

    public static bool HasAllOf(this DateTimePartFlags flags, DateTimePartFlags checkAllFound)    => (flags & checkAllFound) == checkAllFound;
    
    public static bool HasNoneOf(this DateTimePartFlags flags, DateTimePartFlags checkNonAreSet)  => (flags & checkNonAreSet) == 0;
    
    public static bool HasAnyOf(this DateTimePartFlags flags, DateTimePartFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    
    public static bool IsExactly(this DateTimePartFlags flags, DateTimePartFlags checkAllFound)   => flags == checkAllFound;
    
    public static DateTime? NextDifferenceFlagTime(this DateTimePartFlags flags, DateTime fromTime)
    {
        if (flags.HasMicros()) return fromTime.AddTicks(TimeSpan.TicksPerMicrosecond - fromTime.Ticks % TimeSpan.TicksPerMicrosecond);
        if (flags.HasMicroTens()) return fromTime.AddTicks((10 * TimeSpan.TicksPerMicrosecond) - fromTime.Ticks %  (10 * TimeSpan.TicksPerMicrosecond));
        if (flags.HasMicroHundreds()) return fromTime.AddTicks((100 * TimeSpan.TicksPerMicrosecond) - fromTime.Ticks %  (100 * TimeSpan.TicksPerMicrosecond));
        if (flags.HasMillis()) return fromTime.AddTicks(TimeSpan.TicksPerMillisecond - fromTime.Ticks % TimeSpan.TicksPerMillisecond);
        if (flags.HasMillisTens()) return fromTime.AddTicks((10 * TimeSpan.TicksPerMillisecond) - fromTime.Ticks %  (10 * TimeSpan.TicksPerMillisecond));
        if (flags.HasMillisHundreds()) return fromTime.AddTicks((100 * TimeSpan.TicksPerMillisecond) - fromTime.Ticks %  (100 * TimeSpan.TicksPerMillisecond));
        if (flags.HasSeconds()) return fromTime.AddTicks(TimeSpan.TicksPerSecond - fromTime.Ticks % TimeSpan.TicksPerSecond);
        if (flags.HasMinutes()) return fromTime.AddTicks(TimeSpan.TicksPerMinute - fromTime.Ticks % TimeSpan.TicksPerMinute);
        if (flags.HasHours()) return fromTime.AddTicks(TimeSpan.TicksPerHour - fromTime.Ticks % TimeSpan.TicksPerHour);
        if (flags.HasDays()) return fromTime.AddTicks(TimeSpan.TicksPerDay - fromTime.Ticks % TimeSpan.TicksPerDay);
        if (flags.HasWeeks()) return fromTime.TruncToWeekBoundary().AddDays(7);
        if (flags.HasMonths()) return fromTime.TruncToMonthBoundary().AddMonths(1);
        if (flags.HasYears()) return fromTime.TruncToYearBoundary().AddYears(1);
        if (flags.HasDecades()) return fromTime.TruncToDecadeBoundary().AddYears(10);
        if (flags.HasCenturies()) return fromTime.TruncToCenturyBoundary().AddYears(100);
        if (flags.HasMillennia()) return fromTime.TruncToMillenniumBoundary().AddYears(1000);
        return null;
    }

    public static DateTimePartFlags TimePartFlagsFromDateTimeFormatString(this string dateTimeFormatString)
    {
        var flagsSoFar = DateTimePartFlags.None;
        if (dateTimeFormatString.Contains("YYYY"))
        {
            flagsSoFar |= DateTimePartFlags.Centuries;
        }
        if (dateTimeFormatString.Contains("YYY"))
        {
            flagsSoFar |= DateTimePartFlags.Centuries;
        }
        if (dateTimeFormatString.Contains("YY"))
        {
            flagsSoFar |= DateTimePartFlags.Decades;
        }
        if (dateTimeFormatString.Contains("Y"))
        {
            flagsSoFar |= DateTimePartFlags.Years;
        }
        if (dateTimeFormatString.Contains("M"))
        {
            flagsSoFar |= DateTimePartFlags.Months;
        }
        if (dateTimeFormatString.Contains("D") | dateTimeFormatString.Contains("d"))
        {
            flagsSoFar |= DateTimePartFlags.Days;
        }
        if (dateTimeFormatString.Contains("H") | dateTimeFormatString.Contains("h"))
        {
            flagsSoFar |= DateTimePartFlags.Hours;
        }
        if (dateTimeFormatString.Contains("m"))
        {
            flagsSoFar |= DateTimePartFlags.Minutes;
        }
        if (dateTimeFormatString.Contains("s"))
        {
            flagsSoFar |= DateTimePartFlags.Seconds;
        }
        if (dateTimeFormatString.Contains("f"))
        {
            flagsSoFar |= DateTimePartFlags.MillisHundreds;
        }
        if (dateTimeFormatString.Contains("ff"))
        {
            flagsSoFar |= DateTimePartFlags.MillisTens;
        }
        if (dateTimeFormatString.Contains("fff"))
        {
            flagsSoFar |= DateTimePartFlags.Millis;
        }
        return flagsSoFar;
    }
}
