// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;

#endregion

namespace FortitudeMarkets.Indicators;

public struct TimeSeriesSpan
{
    public TimeSeriesSpan(TimeBoundaryPeriod period, int numberOfPeriods = 10)
    {
        NumberOfPeriods = numberOfPeriods;
        Period          = period;
    }

    public int NumberOfPeriods { get; set; }
    public TimeBoundaryPeriod Period { get; set; }

    public static PalantírReveal<TimeSeriesSpan> Styler { get; } =
        (tss, stsa) =>
            stsa.StartComplexType(tss)
                .Field.AlwaysAdd(nameof(tss.Period), tss.Period)
                .Field.AlwaysAdd(nameof(tss.NumberOfPeriods), tss.NumberOfPeriods)
                .Complete();
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

    public TimeLengthType Type { get; set; }
    public TimeSpan? TimeSpanLength { get; set; }
    public TimeSeriesSpan? TimeSeriesLength { get; set; }


    public static PalantírReveal<TimeLength> Styler { get; } =
        (tl, stsa) =>
            stsa.StartComplexType(tl)
                .Field.AlwaysAdd(nameof(tl.Type), tl.Type)
                .Field.AlwaysAdd(nameof(tl.TimeSpanLength), tl.TimeSpanLength)
                .Field.AlwaysReveal(nameof(tl.TimeSeriesLength), tl.TimeSeriesLength, TimeSeriesSpan.Styler)
                .Complete();
}

public static class TimeLengthExtensions
{
    public static TimeSpan LargerPeriodOf(this TimeLength timeLength, TimeBoundaryPeriod period, int numberOfPeriods)
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
