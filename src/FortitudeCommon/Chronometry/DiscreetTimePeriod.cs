// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Extensions;
using MathNet.Numerics;
using static FortitudeCommon.Chronometry.TimeBoundaryPeriod;

#endregion

namespace FortitudeCommon.Chronometry;

public readonly struct DiscreetTimePeriod
{
    public DiscreetTimePeriod()
    {
        NumberOfPeriods = 0;
        Period          = Tick;
    }

    public DiscreetTimePeriod(TimeSpan timeSpan)
    {
        var    preStandardizedTimeSeriesPeriod = Tick;
        ushort preStandardNumberOfPeriods;
        if (timeSpan < TimeSpan.Zero)
        {
            preStandardizedTimeSeriesPeriod = Tick;
            preStandardNumberOfPeriods      = 0;
        }
        else if (timeSpan == TimeSpan.MaxValue)
        {
            preStandardizedTimeSeriesPeriod = Eternity;
            preStandardNumberOfPeriods      = 1;
        }
        else if (timeSpan < TimeSpan.FromMilliseconds(1))
        {
            preStandardizedTimeSeriesPeriod = OneMicrosecond;
            preStandardNumberOfPeriods      = (ushort)timeSpan.TotalMicroseconds.Round(0);
        }
        else if (timeSpan < TimeSpan.FromSeconds(1))
        {
            preStandardizedTimeSeriesPeriod = OneMillisecond;
            preStandardNumberOfPeriods      = (ushort)timeSpan.TotalMilliseconds.Round(0);
        }
        else if (timeSpan < TimeSpan.FromHours(4))
        {
            preStandardizedTimeSeriesPeriod = OneSecond;
            preStandardNumberOfPeriods      = (ushort)timeSpan.TotalSeconds.Round(0);
        }
        else if (timeSpan < TimeSpan.FromDays(7))
        {
            preStandardizedTimeSeriesPeriod = OneMinute;
            preStandardNumberOfPeriods      = (ushort)timeSpan.TotalMinutes.Round(0);
        }
        else if (timeSpan < TimeSpan.FromDays(365.25))
        {
            preStandardizedTimeSeriesPeriod = OneHour;
            preStandardNumberOfPeriods      = (ushort)timeSpan.TotalHours.Round(0);
        }
        else if (timeSpan < TimeSpan.FromDays(365.25) * 10)
        {
            preStandardizedTimeSeriesPeriod = OneDay;
            preStandardNumberOfPeriods      = (ushort)timeSpan.TotalDays.Round(0);
        }
        else if (timeSpan < TimeSpan.FromDays(365.25) * 100)
        {
            preStandardizedTimeSeriesPeriod = OneMonth;
            preStandardNumberOfPeriods      = (ushort)(timeSpan.TotalHours / 30.437).Round(0);
        }
        else
        {
            preStandardizedTimeSeriesPeriod = OneYear;
            preStandardNumberOfPeriods      = (ushort)(timeSpan.TotalHours / 8760.0).Round(0);
        }
        (Period, NumberOfPeriods) = StandardizeRepresentation(preStandardizedTimeSeriesPeriod, preStandardNumberOfPeriods);
    }

    private Tuple<TimeBoundaryPeriod, ushort> StandardizeRepresentation(TimeBoundaryPeriod proposed, ushort numberOfProposed)
    {
        var convertedTimeSeriesPeriod = proposed;
        var convertedNumber           = numberOfProposed;

        (convertedTimeSeriesPeriod, convertedNumber) = StandardUnits(convertedTimeSeriesPeriod, convertedNumber);
        (convertedTimeSeriesPeriod, convertedNumber) = StandardizeMultiples(convertedTimeSeriesPeriod, convertedNumber);

        return new Tuple<TimeBoundaryPeriod, ushort>(convertedTimeSeriesPeriod, convertedNumber);
    }

    private (TimeBoundaryPeriod convertedTimeSeriesPeriod, ushort convertedNumber) StandardizeMultiples
        (TimeBoundaryPeriod convertedTimeBoundaryPeriod, ushort convertedNumber)
    {
        switch (convertedTimeBoundaryPeriod)
        {
            case OneSecond:
                (convertedTimeBoundaryPeriod, convertedNumber) = StandardizeOneSecondMultiples(convertedTimeBoundaryPeriod, convertedNumber);
                break;
            case OneMinute:
                (convertedTimeBoundaryPeriod, convertedNumber) = StandardizeOneMinuteMultiples(convertedTimeBoundaryPeriod, convertedNumber);
                break;
            case OneHour:
                (convertedTimeBoundaryPeriod, convertedNumber) = StandardizeOneHourMultiples(convertedTimeBoundaryPeriod, convertedNumber);
                break;
        }
        return (convertedTimeBoundaryPeriod, convertedNumber);
    }


    private (TimeBoundaryPeriod convertedTimeSeriesPeriod, ushort convertedNumber) StandardizeOneSecondMultiples
        (TimeBoundaryPeriod convertedTimeBoundaryPeriod, ushort convertedNumber)
    {
        switch (convertedNumber)
        {
            case 5:
                convertedTimeBoundaryPeriod = FiveSeconds;
                convertedNumber             = 1;
                break;
            case 10:
                convertedTimeBoundaryPeriod = TenSeconds;
                convertedNumber             = 1;
                break;
            case 15:
                convertedTimeBoundaryPeriod = FifteenSeconds;
                convertedNumber             = 1;
                break;
            case 30:
                convertedTimeBoundaryPeriod = ThirtySeconds;
                convertedNumber             = 1;
                break;
            case 60:
                convertedTimeBoundaryPeriod = OneMinute;
                convertedNumber             = 1;
                break;
            case 300:
                convertedTimeBoundaryPeriod = FiveMinutes;
                convertedNumber             = 1;
                break;
            case 600:
                convertedTimeBoundaryPeriod = TenMinutes;
                convertedNumber             = 1;
                break;
            case 900:
                convertedTimeBoundaryPeriod = FifteenMinutes;
                convertedNumber             = 1;
                break;
            case 1800:
                convertedTimeBoundaryPeriod = ThirtyMinutes;
                convertedNumber             = 1;
                break;
            case 3600:
                convertedTimeBoundaryPeriod = OneHour;
                convertedNumber             = 1;
                break;
            case 4 * 3600:
                convertedTimeBoundaryPeriod = FourHours;
                convertedNumber             = 1;
                break;
        }
        return (convertedTimeBoundaryPeriod, convertedNumber);
    }

    private (TimeBoundaryPeriod convertedTimeSeriesPeriod, ushort convertedNumber) StandardizeOneMinuteMultiples
        (TimeBoundaryPeriod convertedTimeBoundaryPeriod, ushort convertedNumber)
    {
        switch (convertedNumber)
        {
            case 5:
                convertedTimeBoundaryPeriod = FiveMinutes;
                convertedNumber             = 1;
                break;
            case 10:
                convertedTimeBoundaryPeriod = TenMinutes;
                convertedNumber             = 1;
                break;
            case 15:
                convertedTimeBoundaryPeriod = FifteenMinutes;
                convertedNumber             = 1;
                break;
            case 30:
                convertedTimeBoundaryPeriod = FifteenMinutes;
                convertedNumber             = 1;
                break;
            case 60:
                convertedTimeBoundaryPeriod = OneHour;
                convertedNumber             = 1;
                break;
            case 240:
                convertedTimeBoundaryPeriod = FourHours;
                convertedNumber             = 1;
                break;
            case 1440:
                convertedTimeBoundaryPeriod = OneDay;
                convertedNumber             = 1;
                break;
            case 10_080:
                convertedTimeBoundaryPeriod = OneWeek;
                convertedNumber             = 1;
                break;
        }
        return (convertedTimeBoundaryPeriod, convertedNumber);
    }

    private (TimeBoundaryPeriod convertedTimeSeriesPeriod, ushort convertedNumber) StandardizeOneHourMultiples
        (TimeBoundaryPeriod convertedTimeBoundaryPeriod, ushort convertedNumber)
    {
        switch (convertedNumber)
        {
            case 24:
                convertedTimeBoundaryPeriod = OneDay;
                convertedNumber             = 1;
                break;
            case 24 * 7:
                convertedTimeBoundaryPeriod = OneWeek;
                convertedNumber             = 1;
                break;
            case 61_362:
                convertedTimeBoundaryPeriod = OneYear;
                convertedNumber             = 1;
                break;
        }
        return (convertedTimeBoundaryPeriod, convertedNumber);
    }

    private (TimeBoundaryPeriod, ushort) StandardUnits(TimeBoundaryPeriod convertedTimeBoundaryPeriod, ushort convertedNumber)
    {
        switch (convertedTimeBoundaryPeriod)
        {
            case OneMicrosecond when convertedNumber > 1000:
                convertedTimeBoundaryPeriod = OneMillisecond;
                convertedNumber             = (ushort)(convertedNumber / 1000);
                break;
            case OneMillisecond when convertedNumber > 1000:
                convertedTimeBoundaryPeriod = OneSecond;
                convertedNumber             = (ushort)(convertedNumber / 1000);
                break;
            case OneSecond when convertedNumber > 4 * 3_600:
                convertedTimeBoundaryPeriod =  OneMinute;
                convertedNumber             /= 60;
                break;
            case FiveSeconds when convertedNumber > 4 * 720:
                convertedTimeBoundaryPeriod =  OneMinute;
                convertedNumber             /= 12;
                break;
            case TenSeconds when convertedNumber > 4 * 360:
                convertedTimeBoundaryPeriod =  OneMinute;
                convertedNumber             /= 6;
                break;
            case FifteenSeconds when convertedNumber > 4 * 240:
                convertedTimeBoundaryPeriod =  OneMinute;
                convertedNumber             /= 4;
                break;
            case ThirtySeconds when convertedNumber > 4 * 120:
                convertedTimeBoundaryPeriod =  OneMinute;
                convertedNumber             /= 2;
                break;
            case OneMinute when convertedNumber > 60 * 24 * 7:
                convertedTimeBoundaryPeriod =  OneHour;
                convertedNumber             /= 60;
                break;
            case FiveMinutes when convertedNumber > 12 * 24 * 7:
                convertedTimeBoundaryPeriod =  OneHour;
                convertedNumber             /= 12;
                break;
            case TenMinutes when convertedNumber > 6 * 24 * 7:
                convertedTimeBoundaryPeriod =  OneHour;
                convertedNumber             /= 6;
                break;
            case FifteenMinutes when convertedNumber > 4 * 24 * 7:
                convertedTimeBoundaryPeriod =  OneHour;
                convertedNumber             /= 4;
                break;
            case ThirtyMinutes when convertedNumber > 2 * 24 * 7:
                convertedTimeBoundaryPeriod =  OneHour;
                convertedNumber             /= 2;
                break;
            case OneHour when convertedNumber == 4:
                convertedTimeBoundaryPeriod = FourHours;
                convertedNumber             = 1;
                break;
            case OneHour when convertedNumber == 12:
                convertedTimeBoundaryPeriod = TwelveHours;
                convertedNumber             = 1;
                break;
            case OneHour when convertedNumber == 24 * 7:
                convertedTimeBoundaryPeriod = OneWeek;
                convertedNumber             = 1;
                break;
            case OneHour when convertedNumber > 365.25 * 24:
                convertedTimeBoundaryPeriod = OneDay;
                convertedNumber             = (ushort)(convertedNumber / 24);
                break;
            case OneDay when convertedNumber is > 3_653 and < 36_525:
                convertedTimeBoundaryPeriod = OneYear;
                convertedNumber             = (ushort)(convertedNumber / 365.25).Round(0);
                break;
            case OneDay when convertedNumber == 7:
                convertedTimeBoundaryPeriod = OneWeek;
                convertedNumber             = 1;
                break;
            case OneDay when convertedNumber == 1_826:
                convertedTimeBoundaryPeriod = FiveYears;
                convertedNumber             = 1;
                break;
            case OneDay when convertedNumber == 3_653:
                convertedTimeBoundaryPeriod = OneDecade;
                convertedNumber             = 1;
                break;
            case OneDay when convertedNumber == 36_525:
                convertedTimeBoundaryPeriod = OneCentury;
                convertedNumber             = 1;
                break;
            case OneMonth when convertedNumber == 12:
                convertedTimeBoundaryPeriod = OneYear;
                convertedNumber             = 1;
                break;
            case OneMonth when convertedNumber == 60:
                convertedTimeBoundaryPeriod = FiveYears;
                convertedNumber             = 1;
                break;
            case OneMonth when convertedNumber == 120:
                convertedTimeBoundaryPeriod = OneDecade;
                convertedNumber             = 1;
                break;
            case OneMonth when convertedNumber == 1_200:
                convertedTimeBoundaryPeriod = OneCentury;
                convertedNumber             = 1;
                break;
            case OneMonth when convertedNumber == 12_000:
                convertedTimeBoundaryPeriod = OneMillennium;
                convertedNumber             = 1;
                break;
            case OneYear when convertedNumber == 5:
                convertedTimeBoundaryPeriod = FiveYears;
                convertedNumber             = 1;
                break;
            case OneYear when convertedNumber == 10:
                convertedTimeBoundaryPeriod = OneDecade;
                convertedNumber             = 1;
                break;
            case OneYear when convertedNumber == 100:
                convertedTimeBoundaryPeriod = OneCentury;
                convertedNumber             = 1;
                break;
            case OneYear when convertedNumber == 1000:
                convertedTimeBoundaryPeriod = OneMillennium;
                convertedNumber             = 1;
                break;
        }
        return (convertedTimeBoundaryPeriod, convertedNumber);
    }

    public DiscreetTimePeriod(TimeBoundaryPeriod period, ushort numOfPeriods = 1)
    {
        Period          = period;
        NumberOfPeriods = numOfPeriods;
        if (period > Tick && numOfPeriods > 0) (Period, NumberOfPeriods) = StandardizeRepresentation(period, numOfPeriods);
    }

    public TimeSpan TimeSpan => Period.AveragePeriodTimeSpan() * NumberOfPeriods;

    public TimeBoundaryPeriod Period { get; }

    public ushort NumberOfPeriods { get; }

    public bool Equals(DiscreetTimePeriod other) => TimeSpan == other.TimeSpan;

    public override bool Equals(object? obj) => obj is DiscreetTimePeriod other && Equals(other);

    public override int GetHashCode() => TimeSpan.GetHashCode();

    public override string ToString() => $"{nameof(DiscreetTimePeriod)}({nameof(Period)}: {Period}, {nameof(NumberOfPeriods)}: {NumberOfPeriods})";

    public static bool operator ==(DiscreetTimePeriod lhs, DiscreetTimePeriod rhs) => lhs.TimeSpan == rhs.TimeSpan;
    public static bool operator !=(DiscreetTimePeriod lhs, DiscreetTimePeriod rhs) => lhs.TimeSpan != rhs.TimeSpan;

    public static bool operator >(DiscreetTimePeriod lhs, DiscreetTimePeriod rhs) => lhs.TimeSpan > rhs.TimeSpan;
    public static bool operator <(DiscreetTimePeriod lhs, DiscreetTimePeriod rhs) => lhs.TimeSpan < rhs.TimeSpan;

    public static bool operator ==(DiscreetTimePeriod lhs, TimeSpan rhs) => lhs.TimeSpan == rhs;
    public static bool operator !=(DiscreetTimePeriod lhs, TimeSpan rhs) => lhs.TimeSpan != rhs;

    public static bool operator >(DiscreetTimePeriod lhs, TimeSpan rhs) => lhs.TimeSpan > rhs;
    public static bool operator <(DiscreetTimePeriod lhs, TimeSpan rhs) => lhs.TimeSpan < rhs;

    public static bool operator ==(TimeSpan lhs, DiscreetTimePeriod rhs) => rhs.TimeSpan == lhs;
    public static bool operator !=(TimeSpan lhs, DiscreetTimePeriod rhs) => rhs.TimeSpan != lhs;

    public static bool operator >(TimeSpan lhs, DiscreetTimePeriod rhs) => lhs > rhs.TimeSpan;
    public static bool operator <(TimeSpan lhs, DiscreetTimePeriod rhs) => lhs < rhs.TimeSpan;

    public static bool operator ==(DiscreetTimePeriod lhs, TimeBoundaryPeriod rhs) => lhs.NumberOfPeriods == 1 && lhs.Period == rhs;
    public static bool operator !=(DiscreetTimePeriod lhs, TimeBoundaryPeriod rhs) => lhs.NumberOfPeriods != 1 || lhs.Period != rhs;

    public static bool operator >(DiscreetTimePeriod lhs, TimeBoundaryPeriod rhs) => lhs.TimeSpan > rhs.AveragePeriodTimeSpan();
    public static bool operator <(DiscreetTimePeriod lhs, TimeBoundaryPeriod rhs) => lhs.TimeSpan > rhs.AveragePeriodTimeSpan();

    public static bool operator ==(TimeBoundaryPeriod lhs, DiscreetTimePeriod rhs) => rhs.NumberOfPeriods == 1 && rhs.Period == lhs;
    public static bool operator !=(TimeBoundaryPeriod lhs, DiscreetTimePeriod rhs) => rhs.NumberOfPeriods != 1 || rhs.Period != lhs;

    public static bool operator >(TimeBoundaryPeriod lhs, DiscreetTimePeriod rhs) => lhs.AveragePeriodTimeSpan() > rhs.TimeSpan;
    public static bool operator <(TimeBoundaryPeriod lhs, DiscreetTimePeriod rhs) => lhs.AveragePeriodTimeSpan() < rhs.TimeSpan;

    public static implicit operator TimeSpan(DiscreetTimePeriod toConvert) => toConvert.TimeSpan;

    public static implicit operator DiscreetTimePeriod(TimeBoundaryPeriod toConvert) => new(toConvert);

    public static explicit operator TimeBoundaryPeriod(DiscreetTimePeriod toConvert)
    {
        if (toConvert.NumberOfPeriods > 1) throw new ArgumentException("Can only convert a single period to TimeBoundaryPeriods");
        return toConvert.Period;
    }
}

public static class DiscreetTimePeriodExtensions
{
    public static TimeBoundaryPeriod ToTimeBoundaryPeriod(this DiscreetTimePeriod timePeriod)
    {
        if (timePeriod.NumberOfPeriods > 1) throw new ArgumentException("Can only convert a single period to TimeBoundaryPeriods");
        return timePeriod.Period;
    }

    public static bool IsUncommonTimeSpan(this DiscreetTimePeriod spanOrPeriod) => spanOrPeriod.NumberOfPeriods != 1;

    public static bool IsTimeBoundaryPeriod(this DiscreetTimePeriod spanOrPeriod) => spanOrPeriod.NumberOfPeriods == 1;

    public static bool IsTickPeriod(this DiscreetTimePeriod spanOrPeriod) => spanOrPeriod.IsTimeBoundaryPeriod() && spanOrPeriod.Period == Tick;

    public static TimeSpan AveragePeriodTimeSpan(this DiscreetTimePeriod discreetPeriod) =>
        discreetPeriod.IsUncommonTimeSpan()
            ? discreetPeriod.TimeSpan
            : discreetPeriod.Period.AveragePeriodTimeSpan();

    public static string ShortName(this DiscreetTimePeriod discreetTimePeriod)
    {
        if (discreetTimePeriod.IsTimeBoundaryPeriod()) return discreetTimePeriod.Period.ShortName();
        var ts = discreetTimePeriod.TimeSpan;
        if (ts == TimeSpan.Zero) return "0s";
        var totalMs = ts.TotalMilliseconds;
        switch (totalMs)
        {
            case < 1000.0:                                                 return $"{(int)totalMs}ms";
            case >= 1000.0 and < 60_000.0:                                 return $"{(int)ts.TotalSeconds}s";
            case >= 60_000.0 and < 3_600_000.0:                            return $"{(int)ts.TotalMinutes}m";
            case >= 3_600_000.0 and < 24 * 3_600_000.0:                    return $"{(int)ts.TotalHours}h";
            case >= 24 * 3_600_000.0 and < 10 * 365.25 * 24 * 3_600_000.0: return $"{(int)ts.TotalDays}d";
            case >= 10 * 365.25 * 24 * 3_600_000.0:                        return $"{(int)ts.TotalDays / 365}y";
        }
        return ts.ToString();
    }

    public static DiscreetTimePeriod ShortNameToDiscreetTimePeriod(this string shortName)
    {
        var intComponentPeriods = shortName.SafeExtractInt();
        if (intComponentPeriods == null) return new DiscreetTimePeriod();
        var numberOfPeriods = (ushort)intComponentPeriods;
        var sbUnits         = new StringBuilder();
        foreach (var aChar in shortName)
            if (aChar is < '0' or > '9')
                sbUnits.Append(aChar);
        var unitsString = sbUnits.ToString();

        return unitsString switch
               {
                   "tick"     => new DiscreetTimePeriod(Tick)
                 , "us"       => new DiscreetTimePeriod(OneMicrosecond, numberOfPeriods)
                 , "ms"       => new DiscreetTimePeriod(OneMillisecond, numberOfPeriods)
                 , "s"        => new DiscreetTimePeriod(OneSecond, numberOfPeriods)
                 , "m"        => new DiscreetTimePeriod(OneMinute, numberOfPeriods)
                 , "h"        => new DiscreetTimePeriod(OneHour, numberOfPeriods)
                 , "d"        => new DiscreetTimePeriod(OneDay, numberOfPeriods)
                 , "W"        => new DiscreetTimePeriod(OneWeek, numberOfPeriods)
                 , "M"        => new DiscreetTimePeriod(OneMonth, numberOfPeriods)
                 , "Q"        => new DiscreetTimePeriod(OneMonth, (ushort)(3 * numberOfPeriods))
                 , "Y"        => new DiscreetTimePeriod(OneYear, numberOfPeriods)
                 , "Eternity" => new DiscreetTimePeriod(Eternity, 1)
                 , _          => new DiscreetTimePeriod()
               };
    }
}
