// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;

public class ResolverState
{
    public readonly IDoublyLinkedList<PricePeriodSummary> cacheLatest = new DoublyLinkedList<PricePeriodSummary>();

    public readonly TimeSeriesPeriod        period;
    public readonly ISourceTickerIdentifier tickerId;

    public TimeSpan            cacheTimeSpan;
    public BoundedTimeRange    existingSummariesHistoricalRange;
    public InstrumentFileInfo? existingSummariesInfo;
    public InstrumentFileInfo? quotesFileInfo;
    public BoundedTimeRange    quotesHistoricalRange;
    public InstrumentFileInfo? subPeriodFileInfo;
    public BoundedTimeRange    subPeriodsHistoricalRange;

    public ResolverState(ISourceTickerIdentifier tickerId, TimeSeriesPeriod period)
    {
        this.period   = period;
        this.tickerId = tickerId;
    }
}
