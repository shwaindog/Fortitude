// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.TimeSeries;

public struct TimeSeriesPeriodRange
{
    public TimeSeriesPeriodRange() { }

    public TimeSeriesPeriodRange(DateTime periodStartTime, TimeSeriesPeriod timeSeriesPeriod)
    {
        PeriodStartTime  = periodStartTime;
        TimeSeriesPeriod = timeSeriesPeriod;
    }

    public DateTime         PeriodStartTime  { get; set; }
    public TimeSeriesPeriod TimeSeriesPeriod { get; set; }
}

public static class TimeSeriesPeriodRangeExtensions
{
    public static DateTime PeriodEnd(this TimeSeriesPeriodRange range) => range.TimeSeriesPeriod.PeriodEnd(range.PeriodStartTime);
}
