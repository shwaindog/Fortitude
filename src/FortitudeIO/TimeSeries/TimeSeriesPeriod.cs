﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeIO.TimeSeries;

[Flags]
public enum TimeSeriesPeriod : uint
{
    None           = 0
  , Tick           = 0x00_00_01
  , OneSecond      = 0x00_00_02
  , FiveSeconds    = 0x00_00_04
  , TenSeconds     = 0x00_00_08
  , FifteenSeconds = 0x00_00_10
  , ThirtySeconds  = 0x00_00_20
  , OneMinute      = 0x00_00_40
  , FiveMinutes    = 0x00_00_80
  , TenMinutes     = 0x00_01_00
  , FifteenMinutes = 0x00_02_00
  , ThirtyMinutes  = 0x00_04_00
  , OneHour        = 0x00_08_00
  , FourHours      = 0x00_10_00
  , TwelveHours    = 0x00_20_00
  , OneDay         = 0x00_40_00
  , OneWeek        = 0x00_80_00
  , OneMonth       = 0x01_00_00
  , OneQuarter     = 0x02_00_00
  , OneYear        = 0x04_00_00
  , FiveYears      = 0x08_00_00
  , OneDecade      = 0x10_00_00
  , OneCentury     = 0x20_00_00
  , OneMillennium  = 0x40_00_00
  , Eternity       = 0x80_00_00
}

public static class TimeSeriesPeriodExtensions
{
    public static DateTime ContainingPeriodBoundaryStart(this TimeSeriesPeriod period, DateTime anyPeriodTime)
    {
        return period switch
               {
                   OneSecond      => anyPeriodTime.TruncToSecondBoundary()
                 , FiveSeconds    => anyPeriodTime.TruncTo5SecondBoundary()
                 , TenSeconds     => anyPeriodTime.TruncTo10SecondBoundary()
                 , FifteenSeconds => anyPeriodTime.TruncTo15SecondBoundary()
                 , ThirtySeconds  => anyPeriodTime.TruncTo30SecondBoundary()
                 , OneMinute      => anyPeriodTime.TruncToMinuteBoundary()
                 , FiveMinutes    => anyPeriodTime.TruncTo5MinuteBoundary()
                 , TenMinutes     => anyPeriodTime.TruncTo10MinuteBoundary()
                 , FifteenMinutes => anyPeriodTime.TruncTo15MinuteBoundary()
                 , ThirtyMinutes  => anyPeriodTime.TruncTo30MinuteBoundary()
                 , OneHour        => anyPeriodTime.TruncTo1HourBoundary()
                 , FourHours      => anyPeriodTime.TruncTo4HourBoundary()
                 , TwelveHours    => anyPeriodTime.TruncTo12HourBoundary()
                 , OneDay         => anyPeriodTime.TruncToDayBoundary()
                 , OneWeek        => anyPeriodTime.TruncToWeekBoundary()
                 , OneMonth       => anyPeriodTime.TruncToMonthBoundary()
                 , OneQuarter     => anyPeriodTime.TruncToQuarterBoundary()
                 , OneYear        => anyPeriodTime.TruncToYearBoundary()
                 , FiveYears      => anyPeriodTime.TruncTo5YearBoundary()
                 , OneDecade      => anyPeriodTime.TruncToDecadeBoundary()
                 , OneCentury     => anyPeriodTime.TruncToCenturyBoundary()
                 , OneMillennium  => anyPeriodTime.TruncToMillenniumBoundary()
                 , Eternity       => DateTime.MinValue
                 , _              => anyPeriodTime
               };
    }

    public static TimeSpan AveragePeriodTimeSpan(this TimeSeriesPeriod period)
    {
        return period switch
               {
                   OneSecond      => TimeSpan.FromSeconds(1)
                 , FiveSeconds    => TimeSpan.FromSeconds(5)
                 , TenSeconds     => TimeSpan.FromSeconds(10)
                 , FifteenSeconds => TimeSpan.FromSeconds(15)
                 , ThirtySeconds  => TimeSpan.FromSeconds(30)
                 , OneMinute      => TimeSpan.FromMinutes(1)
                 , FiveMinutes    => TimeSpan.FromMinutes(5)
                 , TenMinutes     => TimeSpan.FromMinutes(10)
                 , FifteenMinutes => TimeSpan.FromMinutes(15)
                 , ThirtyMinutes  => TimeSpan.FromMinutes(30)
                 , OneHour        => TimeSpan.FromHours(1)
                 , FourHours      => TimeSpan.FromHours(4)
                 , TwelveHours    => TimeSpan.FromHours(12)
                 , OneDay         => TimeSpan.FromDays(1)
                 , OneWeek        => TimeSpan.FromDays(7)
                 , OneMonth       => TimeSpan.FromDays(365.25) / 12
                 , OneQuarter     => TimeSpan.FromDays(365.25) / 4
                 , OneYear        => TimeSpan.FromDays(365.25)
                 , FiveYears      => TimeSpan.FromDays(365.25 * 5)
                 , OneDecade      => TimeSpan.FromDays(3652.5)
                 , OneCentury     => TimeSpan.FromDays(365250)
                 , OneMillennium  => TimeSpan.FromDays(3652500)
                 , Eternity       => TimeSpan.MaxValue
                 , _              => throw new Exception("Period has not fixed TimeSpan")
               };
    }

    public static DateTime PeriodEnd(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
               {
                   OneSecond      => startTime.AddSeconds(1)
                 , FiveSeconds    => startTime.AddSeconds(5)
                 , TenSeconds     => startTime.AddSeconds(10)
                 , FifteenSeconds => startTime.AddSeconds(15)
                 , ThirtySeconds  => startTime.AddSeconds(30)
                 , OneMinute      => startTime.AddMinutes(1)
                 , FiveMinutes    => startTime.AddMinutes(5)
                 , TenMinutes     => startTime.AddMinutes(10)
                 , FifteenMinutes => startTime.AddMinutes(15)
                 , ThirtyMinutes  => startTime.AddMinutes(30)
                 , OneHour        => startTime.AddHours(1)
                 , FourHours      => startTime.AddHours(4)
                 , TwelveHours    => startTime.AddHours(12)
                 , OneDay         => startTime.AddDays(1)
                 , OneWeek        => startTime.AddDays(7)
                 , OneMonth       => startTime.AddMonths(1)
                 , OneQuarter     => startTime.AddMonths(3)
                 , OneYear        => startTime.AddYears(1)
                 , FiveYears      => startTime.AddYears(5)
                 , OneDecade      => startTime.AddYears(10)
                 , OneCentury     => startTime.AddYears(100)
                 , OneMillennium  => startTime.AddYears(1000)
                 , Eternity       => DateTime.MaxValue
                 , _              => startTime
               };
    }

    public static DateTime PreviousPeriodStart(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
               {
                   OneSecond      => startTime.AddSeconds(-1)
                 , FiveSeconds    => startTime.AddSeconds(-5)
                 , TenSeconds     => startTime.AddSeconds(-10)
                 , FifteenSeconds => startTime.AddSeconds(-15)
                 , ThirtySeconds  => startTime.AddSeconds(-30)
                 , OneMinute      => startTime.AddMinutes(-1)
                 , FiveMinutes    => startTime.AddMinutes(-5)
                 , TenMinutes     => startTime.AddMinutes(-10)
                 , FifteenMinutes => startTime.AddMinutes(-15)
                 , ThirtyMinutes  => startTime.AddMinutes(-30)
                 , OneHour        => startTime.AddHours(-1)
                 , FourHours      => startTime.AddHours(-4)
                 , TwelveHours    => startTime.AddHours(-12)
                 , OneDay         => startTime.AddDays(-1)
                 , OneWeek        => startTime.AddDays(-7)
                 , OneMonth       => startTime.AddMonths(-1)
                 , OneQuarter     => startTime.AddMonths(-3)
                 , OneYear        => startTime.AddYears(-1)
                 , FiveYears      => startTime.AddYears(-5)
                 , OneDecade      => startTime.AddYears(-10)
                 , OneCentury     => startTime.AddYears(-100)
                 , OneMillennium  => startTime.AddYears(-1000)
                 , Eternity       => DateTime.MinValue
                 , _              => startTime
               };
    }

    public static TimeSeriesPeriod NextContainingPeriod(this TimeSeriesPeriod period)
    {
        return period switch
               {
                   Tick           => OneSecond
                 , OneSecond      => FiveSeconds
                 , FiveSeconds    => TenSeconds
                 , TenSeconds     => ThirtySeconds
                 , FifteenSeconds => ThirtySeconds
                 , ThirtySeconds  => OneMinute
                 , OneMinute      => FiveMinutes
                 , FiveMinutes    => TenMinutes
                 , TenMinutes     => ThirtyMinutes
                 , FifteenMinutes => ThirtyMinutes
                 , ThirtyMinutes  => OneHour
                 , OneHour        => FourHours
                 , FourHours      => TwelveHours
                 , TwelveHours    => OneDay
                 , OneDay         => OneMonth
                 , OneWeek        => None
                 , OneMonth       => OneQuarter
                 , OneQuarter     => OneYear
                 , OneYear        => FiveYears
                 , FiveYears      => OneDecade
                 , OneDecade      => OneCentury
                 , OneCentury     => OneMillennium
                 , OneMillennium  => Eternity
                 , _              => None
               };
    }

    public static TimeSeriesPeriod GranularDivisiblePeriod(this TimeSeriesPeriod period)
    {
        return period switch
               {
                   Tick           => None
                 , OneSecond      => Tick
                 , FiveSeconds    => OneSecond
                 , TenSeconds     => FiveSeconds
                 , FifteenSeconds => FiveSeconds
                 , ThirtySeconds  => FifteenSeconds
                 , OneMinute      => ThirtySeconds
                 , FiveMinutes    => OneMinute
                 , TenMinutes     => FiveMinutes
                 , FifteenMinutes => FiveMinutes
                 , ThirtyMinutes  => FifteenMinutes
                 , OneHour        => ThirtyMinutes
                 , FourHours      => OneHour
                 , TwelveHours    => FourHours
                 , OneDay         => TwelveHours
                 , OneWeek        => OneDay
                 , OneMonth       => OneDay
                 , OneQuarter     => OneMonth
                 , OneYear        => OneQuarter
                 , FiveYears      => OneYear
                 , OneDecade      => FiveYears
                 , OneCentury     => OneDecade
                 , OneMillennium  => OneCentury
                 , Eternity       => OneMillennium
                 , _              => None
               };
    }
    public static TimeSeriesPeriod[] ConstructingDivisiblePeriods(this TimeSeriesPeriod period)
    {
        return period switch
               {
                   Tick           => []
                 , OneSecond      => [Tick]
                 , FiveSeconds    => [OneSecond, Tick]
                 , TenSeconds     => [FiveSeconds, OneSecond, Tick]
                 , FifteenSeconds => [FiveSeconds, OneSecond, Tick]
                 , ThirtySeconds  => [FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneMinute      => [ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , FiveMinutes    => [OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , TenMinutes     => [FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , FifteenMinutes => [FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , ThirtyMinutes  => [FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneHour        => [ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , FourHours      => [OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , TwelveHours    => [FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneDay         => [TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneWeek        => [OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneMonth       => [OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneQuarter     => [OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneYear        => [OneQuarter, OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , FiveYears      => [OneYear, OneQuarter, OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneDecade      => [FiveYears, OneYear, OneQuarter, OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneCentury     => [OneDecade, FiveYears, OneYear, OneQuarter, OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , OneMillennium  => [OneCentury, OneDecade, FiveYears, OneYear, OneQuarter, OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , Eternity       => [OneMillennium, OneCentury, OneDecade, FiveYears, OneYear, OneQuarter, OneMonth, OneDay, TwelveHours, FourHours, OneHour, ThirtyMinutes, FifteenMinutes, FiveMinutes, OneMinute, ThirtySeconds, FifteenSeconds, FiveSeconds, OneSecond, Tick]
                 , _              => []
               };
    }
    
    public static List<TimeSeriesPeriodRange> ConstructingDivisiblePeriods(this TimeSeriesPeriod period, DateTime asAt)
    {
        var periodRanges     = new List<TimeSeriesPeriodRange>();
        var currentTime      = period.ContainingPeriodBoundaryStart(asAt);
        var candidatePeriods = period.ConstructingDivisiblePeriods();
        var i                = 0;
        var currentPeriod    = candidatePeriods.Length > 0 ? candidatePeriods[i++] : Tick;
        while (currentPeriod != Tick)
        {
            var checkTime = currentPeriod.PeriodEnd(currentTime);
            if (checkTime <= asAt)
            {
                periodRanges.Add(new TimeSeriesPeriodRange(currentTime, currentPeriod));
                currentTime = currentPeriod.PeriodEnd(currentTime);
            }
            else
            {
                currentPeriod = candidatePeriods[i++];
            }
        }
        periodRanges.Add(new TimeSeriesPeriodRange(currentTime, Tick));
        return periodRanges;
    }

    public static string ShortName(this TimeSeriesPeriod period)
    {
        return period switch
               {
                   Tick           => "tick"
                 , OneSecond      => "1s"
                 , FiveSeconds    => "5s"
                 , TenSeconds     => "10s"
                 , FifteenSeconds => "15s"
                 , ThirtySeconds  => "30s"
                 , OneMinute      => "1m"
                 , FiveMinutes    => "5m"
                 , TenMinutes     => "10m"
                 , FifteenMinutes => "15m"
                 , ThirtyMinutes  => "30m"
                 , OneHour        => "1h"
                 , FourHours      => "4h"
                 , TwelveHours    => "12h"
                 , OneDay         => "1d"
                 , OneWeek        => "1W"
                 , OneMonth       => "1M"
                 , OneQuarter     => "1Q"
                 , OneYear        => "1Y"
                 , FiveYears      => "5Y"
                 , OneDecade      => "10Y"
                 , OneCentury     => "100Y"
                 , OneMillennium  => "1000Y"
                 , Eternity       => "Eternity"
                 , _              => throw new Exception("Can not convert period to short name")
               };
    }

    public static TimeSeriesPeriod FromShortName(this string shortName)
    {
        return shortName switch
               {
                   "tick"    => Tick
                 , "1s"      => OneSecond
                 , "5s"      => FiveSeconds
                 , "10s"     => TenSeconds
                 , "15s"     => FifteenSeconds
                 , "30s"     => ThirtySeconds
                 , "1m"      => OneMinute
                 , "5m"      => FiveMinutes
                 , "10m"     => TenMinutes
                 , "15m"     => FifteenMinutes
                 , "30m"     => ThirtyMinutes
                 , "1h"      => OneHour
                 , "4h"      => FourHours
                 , "12h"     => TwelveHours
                 , "1d"      => OneDay
                 , "1W"      => OneWeek
                 , "1M"      => OneMonth
                 , "1Q"      => OneQuarter
                 , "1Y"      => OneYear
                 , "5Y"      => FiveYears
                 , "10Y"     => OneDecade
                 , "100Y"    => OneCentury
                 , "1000Y"   => OneMillennium
                 , "Eternity" => Eternity
                 , _         => throw new Exception("Can not convert period from short name")
               };
    }
}
