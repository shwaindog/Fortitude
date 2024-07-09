// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsCore.Pricing.Summaries;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing.PeriodSummaries.Construction;

public class ResolverState
{
    public readonly IDoublyLinkedList<PricePeriodSummary> cacheLatest = new DoublyLinkedList<PricePeriodSummary>();

    public readonly TimeSeriesPeriod period;
    public readonly ISourceTickerId  tickerId;

    public TimeSpan            cacheTimeSpan;
    public BoundedTimeRange    existingSummariesHistoricalRange;
    public InstrumentFileInfo? existingSummariesInfo;
    public InstrumentFileInfo? quotesFileInfo;
    public BoundedTimeRange    quotesHistoricalRange;
    public InstrumentFileInfo? subPeriodFileInfo;
    public BoundedTimeRange    subPeriodsHistoricalRange;

    public ResolverState(ISourceTickerId tickerId, TimeSeriesPeriod period)
    {
        this.period   = period;
        this.tickerId = tickerId;
    }
}
