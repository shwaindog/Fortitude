﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

public class CompositeMidMidPriceGenerator : MidPriceGenerator
{
    protected readonly decimal                  PercentageRunTime;
    private readonly   long                     runTimeTicks;
    private readonly   decimal                  startPrice;
    private readonly   DateTime                 startTime;
    private readonly   long                     totalRunPlusSleepTicks;
    protected          List<IMidPriceGenerator> ComposedGenerators = new();

    public CompositeMidMidPriceGenerator(decimal startPrice, DateTime startTime, TimeSpan runTimeSpan,
        int roundAtDecimalPlaces = 6, TimeSpan? deltaPostRunSleepTimeSpan = null)
    {
        this.startPrice      = startPrice;
        RoundAtDecimalPlaces = roundAtDecimalPlaces;
        this.startTime       = startTime;
        runTimeTicks         = runTimeSpan.Ticks;
        var deltaPostRunSleepTimeTicks = deltaPostRunSleepTimeSpan?.Ticks ?? 0;
        totalRunPlusSleepTicks = runTimeTicks + deltaPostRunSleepTimeTicks;
        PercentageRunTime      = (decimal)runTimeTicks / totalRunPlusSleepTicks;
    }

    public CompositeMidMidPriceGenerator(decimal startPrice, DateTime startTime, TimeSpan runTimeSpan,
        int roundAtDecimalPlaces = 6, TimeSpan? deltaPostRunSleepTimeSpan = null, params IMidPriceGenerator[] compositeGenerators)
        : this(startPrice, startTime, runTimeSpan, roundAtDecimalPlaces, deltaPostRunSleepTimeSpan)
    {
        ComposedGenerators.AddRange(compositeGenerators);
    }

    public override MidPriceTime PriceAt(DateTime atTime, int sequenceNumber = 0)
    {
        var ticksSinceStart = atTime.Ticks - startTime.Ticks;
        if (ticksSinceStart < 0) return new MidPriceTime(startPrice, 0, atTime, sequenceNumber);
        var wholeRunPeriods  = ticksSinceStart / totalRunPlusSleepTicks;
        var wholeRunTime     = wholeRunPeriods * PercentageRunTime;
        var remainderTicks   = ticksSinceStart % totalRunPlusSleepTicks;
        var remainderRunTime = remainderTicks < runTimeTicks ? remainderTicks : runTimeTicks;

        var totalRunTimeTicks = (long)wholeRunTime + remainderRunTime;

        var adjustedDateTime = startTime + TimeSpan.FromTicks(totalRunTimeTicks);

        var sumDelta =
            ComposedGenerators.Aggregate(0m,
                                         (runningTotal, generator) => runningTotal + generator.PriceAt(adjustedDateTime).Delta);

        return new MidPriceTime(decimal.Round(startPrice + sumDelta, RoundAtDecimalPlaces), sumDelta, atTime, sequenceNumber);
    }
}