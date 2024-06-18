// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.Generators.MidPrice;

public class SineWaveMidPriceGenerator : MidPriceGenerator
{
    private readonly long    adjustedFullRevolutionTicks;
    private readonly decimal magnitude;
    private readonly decimal percentageRunTime;
    private readonly long    runPeriodTicks;
    private readonly long    totalRunDisableTicks;

    public SineWaveMidPriceGenerator
    (decimal startPrice, DateTime startTime, decimal magnitude, TimeSpan fullRevolutionTimeSpan
      , TimeSpan runPeriodTimeSpan, int roundAtDecimalPlaces = 6, TimeSpan? postRunSleepTimeSpan = null)
    {
        StartPrice           = startPrice;
        RoundAtDecimalPlaces = roundAtDecimalPlaces;
        StartTime            = startTime;
        this.magnitude       = magnitude;
        runPeriodTicks       = runPeriodTimeSpan.Ticks;
        var deltaPostRunSleepTimeTicks = postRunSleepTimeSpan?.Ticks ?? 0;
        totalRunDisableTicks        = runPeriodTicks + deltaPostRunSleepTimeTicks;
        percentageRunTime           = (decimal)runPeriodTicks / totalRunDisableTicks;
        adjustedFullRevolutionTicks = (long)(fullRevolutionTimeSpan.Ticks * percentageRunTime);
    }

    public override MidPriceTime PriceAt(DateTime atTime, int sequenceNumber = 0)
    {
        var ticksSinceStart = atTime.Ticks - StartTime.Ticks;
        if (ticksSinceStart < 0) return new MidPriceTime(StartPrice, 0, atTime, sequenceNumber);
        var wholeRunPeriods  = ticksSinceStart / totalRunDisableTicks;
        var wholeRunTime     = wholeRunPeriods * percentageRunTime;
        var remainderTicks   = ticksSinceStart % totalRunDisableTicks;
        var remainderRunTime = remainderTicks < runPeriodTicks ? (decimal)remainderTicks : runPeriodTicks;

        var totalRunTimeTicks   = wholeRunTime + remainderRunTime;
        var remainderRevolution = totalRunTimeTicks % adjustedFullRevolutionTicks;
        var radians             = (double)remainderRevolution / adjustedFullRevolutionTicks * (Math.PI * 2);

        var sineValue = Math.Sin(radians);
        var delta     = magnitude * (decimal)sineValue;
        return new MidPriceTime(decimal.Round(StartPrice + delta, RoundAtDecimalPlaces), delta, atTime, sequenceNumber);
    }
}
