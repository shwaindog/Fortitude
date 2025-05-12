// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Generators.Candles;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.Generators.MidPrice;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Candles;

public class PQStorageCandleGenerator : CandleGenerator<PQStorageCandle>
{
    public PQStorageCandleGenerator(GenerateCandlesInfo generateCandleInfo) : base(generateCandleInfo) { }

    public override PQStorageCandle CreateCandle
        (ISourceTickerInfo sourceTickerInfo, MidPriceTimePair midPriceTimePair)
    {
        var currMid = midPriceTimePair.CurrentMid;
        return new PQStorageCandle
        {
            TimeBoundaryPeriod = GenerateCandleInfo.CandlePeriod
          , PeriodStartTime    = currMid.Time
        };
    }
}
