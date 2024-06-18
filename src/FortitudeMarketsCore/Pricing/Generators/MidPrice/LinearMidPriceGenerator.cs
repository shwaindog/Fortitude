// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.Generators.MidPrice;

public class LinearMidMidPriceGenerator : MidPriceGenerator
{
    private readonly decimal deltaPrice;
    private readonly long    deltaRunTimeTicks;
    private readonly decimal percentageRunTime;
    private readonly long    totalRunPlusSleepTicks;

    public LinearMidMidPriceGenerator
    (decimal startPrice, DateTime startTime, decimal deltaPrice, TimeSpan deltaRunTimeSpan, int roundAtDecimalPlaces = 6
      , TimeSpan? deltaPostRunSleepTimeSpan = null)
    {
        StartPrice           = startPrice;
        RoundAtDecimalPlaces = roundAtDecimalPlaces;
        StartTime            = startTime;
        this.deltaPrice      = deltaPrice;
        deltaRunTimeTicks    = deltaRunTimeSpan.Ticks;
        var deltaPostRunSleepTimeTicks = deltaPostRunSleepTimeSpan?.Ticks ?? 0;
        totalRunPlusSleepTicks = deltaRunTimeTicks + deltaPostRunSleepTimeTicks;
        percentageRunTime      = (decimal)deltaRunTimeTicks / totalRunPlusSleepTicks;
    }


    public override MidPriceTime PriceAt(DateTime atTime, int sequenceNumber = 0)
    {
        var ticksSinceStart = atTime.Ticks - StartTime.Ticks;
        if (ticksSinceStart < 0) return new MidPriceTime(StartPrice, 0, atTime, sequenceNumber);
        var wholeRunPeriods  = ticksSinceStart / totalRunPlusSleepTicks;
        var wholeRunTime     = wholeRunPeriods * percentageRunTime;
        var remainderTicks   = ticksSinceStart % totalRunPlusSleepTicks;
        var remainderRunTime = remainderTicks < deltaRunTimeTicks ? remainderTicks : deltaRunTimeTicks;

        var totalRunTimeTicks = wholeRunTime + remainderRunTime;

        var deltaAmount = deltaPrice * totalRunTimeTicks / totalRunPlusSleepTicks;
        return new MidPriceTime(decimal.Round(StartPrice + deltaAmount, RoundAtDecimalPlaces), deltaAmount, atTime, sequenceNumber);
    }
}
