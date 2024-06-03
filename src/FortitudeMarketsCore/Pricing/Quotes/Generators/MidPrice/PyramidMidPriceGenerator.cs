// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.Quotes.Generators.MidPrice;

public class PyramidMidPriceGenerator : CompositeMidMidPriceGenerator
{
    public PyramidMidPriceGenerator(decimal startPrice,
        DateTime startTime, decimal deltaPrice, TimeSpan riseTimeSpan, TimeSpan fallTimeSpan, TimeSpan runTimeSpan,
        int roundAtDecimalPlaces = 6, TimeSpan? deltaPostRunSleepTimeSpan = null)
        : base(startPrice, startTime, runTimeSpan, roundAtDecimalPlaces, deltaPostRunSleepTimeSpan)
    {
        var adjustRiseTime = TimeSpan.FromTicks((long)(riseTimeSpan.Ticks * PercentageRunTime));
        var adjustFallTime = TimeSpan.FromTicks((long)(fallTimeSpan.Ticks * PercentageRunTime));

        var risingLineGenerator = new LinearMidMidPriceGenerator(startPrice, startTime, deltaPrice, adjustRiseTime,
                                                                 roundAtDecimalPlaces, adjustFallTime);
        var fallingLineGenerator = new LinearMidMidPriceGenerator(startPrice, startTime + adjustRiseTime, deltaPrice, adjustFallTime,
                                                                  roundAtDecimalPlaces, adjustRiseTime);
        ComposedGenerators.Add(risingLineGenerator);
        ComposedGenerators.Add(fallingLineGenerator);
    }
}
