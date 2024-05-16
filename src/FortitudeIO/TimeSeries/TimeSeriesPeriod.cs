#region

using FortitudeCommon.Extensions;
using static FortitudeIO.TimeSeries.TimeSeriesPeriod;

#endregion

namespace FortitudeIO.TimeSeries;

[Flags]
public enum TimeSeriesPeriod : ushort
{
    None = 0
    , Tick = 0x00_01
    , OneSecond = 0x00_02
    , OneMinute = 0x00_04
    , FiveMinutes = 0x00_08
    , TenMinutes = 0x00_10
    , FifteenMinutes = 0x00_20
    , ThirtyMinutes = 0x00_40
    , OneHour = 0x00_80
    , FourHours = 0x01_00
    , OneDay = 0x02_00
    , OneWeek = 0x04_00
    , OneMonth = 0x08_00
    , OneYear = 0x10_00
    , ConsumerConflated = 0x80_00
}

public static class TimeSeriesPeriodExtensions
{
    public static DateTime ContainingPeriodBoundaryStart(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
        {
            OneSecond => startTime.TruncToSecondBoundary(), OneMinute => startTime.TruncToMinuteBoundary()
            , FiveMinutes => startTime.TruncTo5MinuteBoundary()
            , TenMinutes => startTime.TruncTo10MinuteBoundary(), FifteenMinutes => startTime.TruncTo15MinuteBoundary()
            , ThirtyMinutes => startTime.TruncTo30MinuteBoundary()
            , OneHour => startTime.TruncTo1HourBoundary(), FourHours => startTime.TruncTo4HourBoundary(), OneDay => startTime.TruncToDayBoundary()
            , OneWeek => startTime.TruncToWeekBoundary()
            , OneMonth => startTime.TruncToMonthBoundary(), OneYear => startTime.TruncToYearBoundary(), _ => startTime
        };
    }

    public static DateTime PeriodEnd(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
        {
            OneSecond => startTime.AddSeconds(1), OneMinute => startTime.AddMinutes(1), FiveMinutes => startTime.AddMinutes(5)
            , TenMinutes => startTime.AddMinutes(10), FifteenMinutes => startTime.AddMinutes(15), ThirtyMinutes => startTime.AddMinutes(30)
            , OneHour => startTime.AddHours(1), FourHours => startTime.AddHours(4), OneDay => startTime.AddDays(1), OneWeek => startTime.AddDays(7)
            , OneMonth => startTime.AddMonths(1), OneYear => startTime.AddYears(1), _ => startTime
        };
    }

    public static DateTime PreviousPeriodStart(this TimeSeriesPeriod period, DateTime startTime)
    {
        return period switch
        {
            OneSecond => startTime.AddSeconds(-1), OneMinute => startTime.AddMinutes(-1), FiveMinutes => startTime.AddMinutes(-5)
            , TenMinutes => startTime.AddMinutes(-10), FifteenMinutes => startTime.AddMinutes(-15), ThirtyMinutes => startTime.AddMinutes(-30)
            , OneHour => startTime.AddHours(-1), FourHours => startTime.AddHours(-4), OneDay => startTime.AddDays(-1)
            , OneWeek => startTime.AddDays(-7)
            , OneMonth => startTime.AddMonths(-1), OneYear => startTime.AddYears(-1), _ => startTime
        };
    }
}
