// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.Generators.MidPrice;

public class SyntheticRepeatableMidPriceGenerator : CompositeMidMidPriceGenerator
{
    public SyntheticRepeatableMidPriceGenerator
    (decimal startPrice, DateTime startTime, decimal maxPercentagePriceMove = 0.1m,
        long runTimeTicks = 100_000, int roundAtDecimalPlaces = 6
      , long deltaPostRunSleepTimeTicks = 200_000)
        : base(startPrice, startTime, TimeSpan.FromTicks(runTimeTicks), roundAtDecimalPlaces,
               TimeSpan.FromTicks(deltaPostRunSleepTimeTicks))
    {
        var largestDelta = startPrice * maxPercentagePriceMove;

        var fastPeriod          = TimeSpan.FromTicks(runTimeTicks / 10);
        var remainingFastPeriod = TimeSpan.FromTicks(runTimeTicks - fastPeriod.Ticks * 2);
        var smallestDelta       = largestDelta * 0.001m;

        var smallestRisingPyramid = new PyramidMidPriceGenerator(startPrice, startTime + remainingFastPeriod, smallestDelta,
                                                                 fastPeriod, fastPeriod,
                                                                 fastPeriod * 2, roundAtDecimalPlaces, remainingFastPeriod);
        ComposedGenerators.Add(smallestRisingPyramid);
        var smallestFallingPyramid = new PyramidMidPriceGenerator(startPrice, startTime + fastPeriod, -smallestDelta,
                                                                  fastPeriod, fastPeriod,
                                                                  fastPeriod * 2, roundAtDecimalPlaces, fastPeriod);
        ComposedGenerators.Add(smallestFallingPyramid);

        var midSpeedPeriod = TimeSpan.FromTicks(runTimeTicks / 2);
        var midDelta       = largestDelta * 0.01m;

        var midRisingPyramid = new PyramidMidPriceGenerator(startPrice, startTime + midSpeedPeriod, midDelta,
                                                            midSpeedPeriod, midSpeedPeriod, midSpeedPeriod,
                                                            roundAtDecimalPlaces, midSpeedPeriod);
        ComposedGenerators.Add(midRisingPyramid);

        var midFallingPyramid = new PyramidMidPriceGenerator(startPrice, startTime + midSpeedPeriod, -midDelta,
                                                             midSpeedPeriod, midSpeedPeriod, fastPeriod,
                                                             roundAtDecimalPlaces, midSpeedPeriod);
        ComposedGenerators.Add(midFallingPyramid);

        var longSpeedPeriod = TimeSpan.FromTicks(runTimeTicks);
        var largeDelta      = largestDelta * 0.1m;

        var largeRisingPyramid = new PyramidMidPriceGenerator(startPrice, startTime + longSpeedPeriod, largeDelta,
                                                              longSpeedPeriod * 20, longSpeedPeriod * 40, fastPeriod,
                                                              roundAtDecimalPlaces, longSpeedPeriod);
        ComposedGenerators.Add(largeRisingPyramid);

        var largeFallingPyramid = new PyramidMidPriceGenerator(startPrice, startTime, -largeDelta,
                                                               longSpeedPeriod * 10, longSpeedPeriod, midSpeedPeriod,
                                                               roundAtDecimalPlaces, longSpeedPeriod);
        ComposedGenerators.Add(largeFallingPyramid);

        var sineWaveGenerator = new SineWaveMidPriceGenerator(startPrice, startTime + longSpeedPeriod,
                                                              largeDelta, TimeSpan.FromTicks(runTimeTicks * 40),
                                                              midSpeedPeriod, roundAtDecimalPlaces, midSpeedPeriod);

        ComposedGenerators.Add(sineWaveGenerator);
    }
}
