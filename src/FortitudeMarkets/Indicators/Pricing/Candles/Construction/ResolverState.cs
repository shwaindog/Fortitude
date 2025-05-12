// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.TimeSeries.FileSystem;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.FeedEvents.Candles;

#endregion

namespace FortitudeMarkets.Indicators.Pricing.Candles.Construction;

public class ResolverState(PricingInstrumentId pricingInstrumentId)
{
    public readonly IDoublyLinkedList<Candle> Cache = new DoublyLinkedList<Candle>();

    public readonly PricingInstrumentId PricingInstrumentId = pricingInstrumentId;

    public TimeSpan            CacheTimeSpan;
    public InstrumentFileInfo? ExistingRepoInfo;
    public BoundedTimeRange    ExistingRepoRange;
    public InstrumentFileInfo? QuotesRepoInfo;
    public BoundedTimeRange    QuotesRepoRange;
    public InstrumentFileInfo? SubCandleRepoInfo;
    public BoundedTimeRange    SubCandleRepoRange;

    public string Ticker => PricingInstrumentId.Ticker;
    public string Source => PricingInstrumentId.Source;

    public TimeBoundaryPeriod CandlePeriod => PricingInstrumentId.CoveringPeriod.Period;
}
