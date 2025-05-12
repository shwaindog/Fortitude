// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles.Construction;

public interface ICandleRequestResponseAttendant
{
    ValueTask<List<Candle>> BuildFromParts(BoundedTimeRange requestRange);
}

public interface ICandleStreamRequestAttendant
{
    bool            HasCompleted      { get; }
    DateTime        ReadCacheFromTime { get; set; }
    ValueTask<bool> BuildFromParts();
    ValueTask<bool> PublishFromCache(BoundedTimeRange requestRange);
    ValueTask<bool> RetrieveFromRepository(BoundedTimeRange requestRange);
}

public class CandleAttendantBase
{
    protected IHistoricalCandleResolverRule ConstructingRule;
    protected ResolverState                             State;

    public CandleAttendantBase(IHistoricalCandleResolverRule constructingRule)
    {
        ConstructingRule = constructingRule;
        State            = constructingRule.State;
    }
}
