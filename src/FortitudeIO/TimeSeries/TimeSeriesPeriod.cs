// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeIO.TimeSeries;

[Flags]
public enum TimeSeriesPeriod : ushort
{
    None           = 0
  , Tick           = 0x00_01
  , OneSecond      = 0x00_02
  , FifteenSeconds = 0x00_04
  , ThirtySeconds  = 0x00_08
  , OneMinute      = 0x00_10
  , FiveMinutes    = 0x00_20
  , TenMinutes     = 0x00_40
  , FifteenMinutes = 0x00_80
  , ThirtyMinutes  = 0x01_00
  , OneHour        = 0x02_00
  , FourHours      = 0x04_00
  , OneDay         = 0x08_00
  , OneWeek        = 0x10_00
  , OneMonth       = 0x20_00
  , OneYear        = 0x40_00
  , OneDecade      = 0x80_00
}

public static class TimeSeriesPeriodExtensions
{
    public static DateTime ContainingPeriodBoundaryStart(this TimeSeriesPeriod period, DateTime anyPeriodTime)
    {
        return period switch
               {
                   OneSecond      => anyPeriodTime.TruncToSecondBoundary()
                 , FifteenSeconds => anyPeriodTime.TruncTo15SecondBoundary()
                 , ThirtySeconds  => anyPeriodTime.TruncTo30SecondBoundary()
                 , OneMinute      => anyPeriodTime.TruncToMinuteBoundary()
                 , FiveMinutes    => anyPeriodTime.TruncTo5MinuteBoundary()
                 , TenMinutes     => anyPeriodTime.TruncTo10MinuteBoundary()
                 , FifteenMinutes => anyPeriodTime.TruncTo15MinuteBoundary()
                 , ThirtyMinutes  => anyPeriodTime.TruncTo30MinuteBoundary()
                 , OneHour        => anyPeriodTime.TruncTo1HourBoundary()
                 , FourHours      => anyPeriodTime.TruncTo4HourBoundary()
                 , OneDay         => anyPeriodTime.TruncToDayBoundary()
                 , OneWeek        => anyPeriodTime.TruncToWeekBoundary()
                 , OneMonth       => anyPeriodTime.TruncToMonthBoundary()
                 , OneYear        => anyPeriodTime.TruncToYearBoundary()
                 , OneDecade      => anyPeriodTime.TruncToDecadeBoundary()
                 , _              => anyPeriodTime
               };
    }

    public static DateTime PeriodEnd(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
               {
                   OneSecond      => startTime.AddSeconds(1)
                 , FifteenSeconds => startTime.AddSeconds(15)
                 , ThirtySeconds  => startTime.AddSeconds(30)
                 , OneMinute      => startTime.AddMinutes(1)
                 , FiveMinutes    => startTime.AddMinutes(5)
                 , TenMinutes     => startTime.AddMinutes(10)
                 , FifteenMinutes => startTime.AddMinutes(15)
                 , ThirtyMinutes  => startTime.AddMinutes(30)
                 , OneHour        => startTime.AddHours(1)
                 , FourHours      => startTime.AddHours(4)
                 , OneDay         => startTime.AddDays(1)
                 , OneWeek        => startTime.AddDays(7)
                 , OneMonth       => startTime.AddMonths(1)
                 , OneYear        => startTime.AddYears(1)
                 , OneDecade      => startTime.AddYears(10)
                 , _              => startTime
               };
    }

    public static DateTime PreviousPeriodStart(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
               {
                   OneSecond      => startTime.AddSeconds(-1)
                 , FifteenSeconds => startTime.AddSeconds(-15)
                 , ThirtySeconds  => startTime.AddSeconds(-30)
                 , OneMinute      => startTime.AddMinutes(-1)
                 , FiveMinutes    => startTime.AddMinutes(-5)
                 , TenMinutes     => startTime.AddMinutes(-10)
                 , FifteenMinutes => startTime.AddMinutes(-15)
                 , ThirtyMinutes  => startTime.AddMinutes(-30)
                 , OneHour        => startTime.AddHours(-1)
                 , FourHours      => startTime.AddHours(-4)
                 , OneDay         => startTime.AddDays(-1)
                 , OneWeek        => startTime.AddDays(-7)
                 , OneMonth       => startTime.AddMonths(-1)
                 , OneYear        => startTime.AddYears(-1)
                 , OneDecade      => startTime.AddYears(-10)
                 , _              => startTime
               };
    }

    public static string ShortName(this TimeSeriesPeriod period)
    {
        return period switch
               {
                   Tick           => "tick"
                 , OneSecond      => "1s"
                 , FifteenSeconds => "15s"
                 , ThirtySeconds  => "30s"
                 , OneMinute      => "1m"
                 , FiveMinutes    => "5m"
                 , TenMinutes     => "10m"
                 , FifteenMinutes => "15m"
                 , ThirtyMinutes  => "30m"
                 , OneHour        => "1h"
                 , FourHours      => "4h"
                 , OneDay         => "1d"
                 , OneWeek        => "1W"
                 , OneMonth       => "1M"
                 , OneYear        => "1Y"
                 , OneDecade      => "1Decade"
                 , _              => throw new Exception("Can not convert period to short name")
               };
    }

    public static TimeSeriesPeriod FromShortName(this string shortName)
    {
        return shortName switch
               {
                   "tick"    => Tick
                 , "1s"      => OneSecond
                 , "15s"     => FifteenSeconds
                 , "30s"     => ThirtySeconds
                 , "1m"      => OneMinute
                 , "5m"      => FiveMinutes
                 , "10m"     => TenMinutes
                 , "15m"     => FifteenMinutes
                 , "30m"     => ThirtyMinutes
                 , "1h"      => OneHour
                 , "4h"      => FourHours
                 , "1d"      => OneDay
                 , "1W"      => OneWeek
                 , "1M"      => OneMonth
                 , "1Y"      => OneYear
                 , "1Decade" => OneDecade
                 , _         => throw new Exception("Can not convert period from short name")
               };
    }
}
