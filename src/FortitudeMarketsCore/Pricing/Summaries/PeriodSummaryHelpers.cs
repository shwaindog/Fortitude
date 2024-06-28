// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Pricing.Summaries;

public static class PeriodSummaryHelpers
{
    public static TimeSeriesPeriod CalcTimeFrame(this IPricePeriodSummary pricePeriodSumary)
    {
        if (pricePeriodSumary.PeriodStartTime.Equals(DateTimeConstants.UnixEpoch)
         || pricePeriodSumary.PeriodEndTime < pricePeriodSumary.PeriodStartTime) return TimeSeriesPeriod.None;
        if (pricePeriodSumary.PeriodEndTime.Equals(DateTimeConstants.UnixEpoch)) return TimeSeriesPeriod.None;
        if (pricePeriodSumary.PeriodStartTime.Equals(pricePeriodSumary.PeriodEndTime)) return TimeSeriesPeriod.Tick;
        var diffTimeSpan = pricePeriodSumary.PeriodEndTime - pricePeriodSumary.PeriodStartTime;
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

        return TimeSeriesPeriod.None;
    }
}
