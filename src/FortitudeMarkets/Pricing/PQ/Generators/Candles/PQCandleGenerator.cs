// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.FeedEvents.Generators.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Candles;

public class PQCandleGenerator : CandleGenerator<PQCandle>
{
    public PQCandleGenerator(GenerateCandlesInfo generateCandleInfo) : base(generateCandleInfo) { }

    public override PQCandle CreateCandle
        (ISourceTickerInfo sourceTickerInfo, MidPriceTimePair midPriceTimePair)
    {
        var currMid = midPriceTimePair.CurrentMid;
        return new PQCandle
        {
            TimeBoundaryPeriod = GenerateCandleInfo.CandlePeriod
          , PeriodEndTime      = GenerateCandleInfo.CandlePeriod.PeriodEnd(currMid.Time)
          , PeriodStartTime    = currMid.Time
        };
    }
}
