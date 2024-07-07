// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;

public interface ISummaryRequestResponseAttendant
{
    ValueTask<List<PricePeriodSummary>> BuildFromParts(BoundedTimeRange requestRange);
}

public interface ISummaryStreamRequestAttendant
{
    bool            HasCompleted      { get; }
    DateTime        ReadCacheFromTime { get; set; }
    ValueTask<bool> BuildFromParts();
    ValueTask<bool> PublishFromCache(BoundedTimeRange requestRange);
    ValueTask<bool> RetrieveFromRepository(BoundedTimeRange requestRange);
}

public class SummaryAttendantBase
{
    protected IHistoricalPricePeriodSummaryResolverRule ConstructingRule;
    protected ResolverState                             State;

    public SummaryAttendantBase(IHistoricalPricePeriodSummaryResolverRule constructingRule)
    {
        ConstructingRule = constructingRule;
        State            = constructingRule.State;
    }
}
