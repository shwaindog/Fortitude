#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Conflation;

#endregion

namespace FortitudeMarketsCore.Pricing.Conflation;

public static class PeriodSummaryHelpers
{
    public static TimeSeriesPeriod CalcTimeFrame(this IPeriodSummary periodSumary)
    {
        if (periodSumary.StartTime.Equals(DateTimeConstants.UnixEpoch)
            || periodSumary.EndTime < periodSumary.StartTime) return TimeSeriesPeriod.None;
        if (periodSumary.EndTime.Equals(DateTimeConstants.UnixEpoch)) return TimeSeriesPeriod.None;
        if (periodSumary.StartTime.Equals(periodSumary.EndTime)) return TimeSeriesPeriod.Tick;
        var diffTimeSpan = periodSumary.EndTime - periodSumary.StartTime;
        var totalSeconds = (int)diffTimeSpan.TotalSeconds;
        if (totalSeconds == 1) return TimeSeriesPeriod.OneSecond;
        if (totalSeconds == 60) return TimeSeriesPeriod.OneMinute;
        if (totalSeconds == 300) return TimeSeriesPeriod.FiveMinutes;
        if (totalSeconds == 600) return TimeSeriesPeriod.TenMinutes;
        if (totalSeconds == 900) return TimeSeriesPeriod.FifteenMinutes;
        if (totalSeconds == 1800) return TimeSeriesPeriod.ThirtyMinutes;
        if (totalSeconds == 3600) return TimeSeriesPeriod.OneHour;
        if (totalSeconds == 14400) return TimeSeriesPeriod.FourHours;
        if (totalSeconds == 3600 * 24) return TimeSeriesPeriod.OneDay;
        if (totalSeconds >= 3600 * 24 * 5
            && totalSeconds <= 7 * 24 * 3600) return TimeSeriesPeriod.OneWeek;
        if (totalSeconds >= 28 * 24 * 3600
            && totalSeconds <= 31 * 24 * 3600) return TimeSeriesPeriod.OneMonth;
        if (totalSeconds >= 365 * 24 * 3600
            && totalSeconds <= 366 * 24 * 3600) return TimeSeriesPeriod.OneYear;

        return TimeSeriesPeriod.ConsumerConflated;
    }
}
