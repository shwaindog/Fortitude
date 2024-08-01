// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;

public class ResolverState(PricingInstrumentId pricingInstrumentId)
{
    public readonly IDoublyLinkedList<PricePeriodSummary> Cache = new DoublyLinkedList<PricePeriodSummary>();

    public readonly PricingInstrumentId PricingInstrumentId = pricingInstrumentId;

    public TimeSpan            CacheTimeSpan;
    public InstrumentFileInfo? ExistingRepoInfo;
    public BoundedTimeRange    ExistingRepoRange;
    public InstrumentFileInfo? QuotesRepoInfo;
    public BoundedTimeRange    QuotesRepoRange;
    public InstrumentFileInfo? SubPeriodRepoInfo;
    public BoundedTimeRange    SubPeriodsRepoRange;

    public string Ticker => PricingInstrumentId.Ticker;
    public string Source => PricingInstrumentId.Source;

    public TimeBoundaryPeriod Period => PricingInstrumentId.CoveringPeriod.Period;
}
