// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Extensions;
using FortitudeIO.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Indicators;

public struct TimeSeriesSpan
{
    public TimeSeriesSpan(TimeSeriesPeriod period, int numberOfPeriods = 10)
    {
        NumberOfPeriods = numberOfPeriods;
        Period          = period;
    }

    public int              NumberOfPeriods { get; set; }
    public TimeSeriesPeriod Period          { get; set; }
}

public enum TimeLengthType
{
    TimeLength
  , TimeSeriesLength
}

public struct TimeLength
{
    public TimeLength(TimeSpan timeSpanLength)
    {
        Type           = TimeLengthType.TimeLength;
        TimeSpanLength = timeSpanLength;
    }

    public TimeLength(TimeSeriesSpan timeSeriesLength)
    {
        Type = TimeLengthType.TimeSeriesLength;

        TimeSeriesLength = timeSeriesLength;
    }

    public TimeLengthType  Type             { get; set; }
    public TimeSpan?       TimeSpanLength   { get; set; }
    public TimeSeriesSpan? TimeSeriesLength { get; set; }
}

public static class TimeLengthExtensions
{
    public static TimeSpan LargerPeriodOf(this TimeLength timeLength, TimeSeriesPeriod period, int numberOfPeriods)
    {
        var averagePeriod = period.AveragePeriodTimeSpan() * numberOfPeriods;
        return timeLength.ToTimeSpan().Max(averagePeriod);
    }

    public static TimeSpan ToTimeSpan(this TimeLength timeLength)
    {
        if (timeLength.Type == TimeLengthType.TimeLength) return timeLength.TimeSpanLength!.Value;
        var calcTimeSpan = timeLength.TimeSeriesLength!.Value.Period.AveragePeriodTimeSpan() * timeLength.TimeSeriesLength!.Value.NumberOfPeriods;
        return calcTimeSpan;
    }
}
