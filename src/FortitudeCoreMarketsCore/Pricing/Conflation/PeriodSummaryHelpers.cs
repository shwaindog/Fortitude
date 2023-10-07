using System;
using FortitudeCommon.Chronometry;
using FortitudeMarketsApi.Pricing.Conflation;

namespace FortitudeMarketsCore.Pricing.Conflation
{
    public static class PeriodSummaryHelpers
    {
        public static TimeFrame CalcTimeFrame(this IPeriodSummary periodSumary)
        {
            if (periodSumary.StartTime.Equals(DateTimeConstants.UnixEpoch)
                || periodSumary.EndTime < periodSumary.StartTime) return TimeFrame.Unknown;
            if (periodSumary.EndTime.Equals(DateTimeConstants.UnixEpoch)) return TimeFrame.Unknown;
            if (periodSumary.StartTime.Equals(periodSumary.EndTime)) return TimeFrame.Tick;
            TimeSpan diffTimeSpan = periodSumary.EndTime - periodSumary.StartTime;
            var totalSeconds = (int)diffTimeSpan.TotalSeconds;
            if (totalSeconds == (int)TimeFrame.OneSecond) return TimeFrame.OneSecond;
            if (totalSeconds == (int)TimeFrame.OneMinute) return TimeFrame.OneMinute;
            if (totalSeconds == (int)TimeFrame.FiveMinutes) return TimeFrame.FiveMinutes;
            if (totalSeconds == (int)TimeFrame.TenMinutes) return TimeFrame.TenMinutes;
            if (totalSeconds == (int)TimeFrame.FifteenMinutes) return TimeFrame.FifteenMinutes;
            if (totalSeconds == (int)TimeFrame.ThirtyMinutes) return TimeFrame.ThirtyMinutes;
            if (totalSeconds == (int)TimeFrame.OneHour) return TimeFrame.OneHour;
            if (totalSeconds == (int)TimeFrame.FourHours) return TimeFrame.FourHours;
            if (totalSeconds == (int)TimeFrame.OneDay) return TimeFrame.OneDay;
            if (totalSeconds >= (int)TimeFrame.OneWeek
                && totalSeconds <= 7 * 24 * 3600) return TimeFrame.OneWeek;
            if (totalSeconds >= 28 * 24 * 3600
                && totalSeconds <= 31 * 24 * 3600) return TimeFrame.OneMonth;

            return TimeFrame.Conflation;
        }
    }
}
