// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

public class LinearMidMidPriceGenerator : MidPriceGenerator
{
    private readonly decimal  deltaPrice;
    private readonly long     deltaRunTimeTicks;
    private readonly decimal  percentageRunTime;
    private readonly decimal  startPrice;
    private readonly DateTime startTime;
    private readonly long     totalRunPlusSleepTicks;

    public LinearMidMidPriceGenerator(decimal startPrice,
        DateTime startTime, decimal deltaPrice, TimeSpan deltaRunTimeSpan, int roundAtDecimalPlaces = 6, TimeSpan? deltaPostRunSleepTimeSpan = null)
    {
        this.startPrice      = startPrice;
        RoundAtDecimalPlaces = roundAtDecimalPlaces;
        this.startTime       = startTime;
        this.deltaPrice      = deltaPrice;
        deltaRunTimeTicks    = deltaRunTimeSpan.Ticks;
        var deltaPostRunSleepTimeTicks = deltaPostRunSleepTimeSpan?.Ticks ?? 0;
        totalRunPlusSleepTicks = deltaRunTimeTicks + deltaPostRunSleepTimeTicks;
        percentageRunTime      = (decimal)deltaRunTimeTicks / totalRunPlusSleepTicks;
    }


    public override MidPriceTime PriceAt(DateTime atTime, int sequenceNumber = 0)
    {
        var ticksSinceStart = atTime.Ticks - startTime.Ticks;
        if (ticksSinceStart < 0) return new MidPriceTime(startPrice, 0, atTime, sequenceNumber);
        var wholeRunPeriods  = ticksSinceStart / totalRunPlusSleepTicks;
        var wholeRunTime     = wholeRunPeriods * percentageRunTime;
        var remainderTicks   = ticksSinceStart % totalRunPlusSleepTicks;
        var remainderRunTime = remainderTicks < deltaRunTimeTicks ? remainderTicks : deltaRunTimeTicks;

        var totalRunTimeTicks = wholeRunTime + remainderRunTime;

        var deltaAmount = deltaPrice * totalRunTimeTicks / totalRunPlusSleepTicks;
        return new MidPriceTime(decimal.Round(startPrice + deltaAmount, RoundAtDecimalPlaces), deltaAmount, atTime, sequenceNumber);
    }
}
