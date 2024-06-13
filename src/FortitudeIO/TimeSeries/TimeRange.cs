// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries;

public struct TimeRange
{
    public TimeRange(DateTime? fromTime, DateTime? toTime)
    {
        FromTime = fromTime;
        ToTime   = toTime;
    }

    public DateTime? FromTime { get; }
    public DateTime? ToTime   { get; }
}

public static class TimeRangeExtensions
{
    public static bool IntersectsWith(this TimeRange? periodRange, TimeSeriesPeriodRange timeSeriesRange)
    {
        if (periodRange == null) return true;
        var range = periodRange.Value;
        return (range.FromTime < timeSeriesRange.TimeSeriesPeriod.PeriodEnd(timeSeriesRange.PeriodStartTime)
             || (range.FromTime == null && range.ToTime != null))
            && (range.ToTime > timeSeriesRange.PeriodStartTime || (range.ToTime == null && range.FromTime != null));
    }
}
