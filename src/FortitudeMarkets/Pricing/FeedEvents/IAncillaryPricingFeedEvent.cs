// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Indicators;
using FortitudeMarkets.Pricing.FeedEvents.PriceAdjustments;
using FortitudeMarkets.Pricing.FeedEvents.Signals;
using FortitudeMarkets.Pricing.FeedEvents.Strategies;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.FeedEvents.TrackingTimes;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface IAncillaryPricingFeedEvent : ITradingStatusFeedEvent
{
    TickerQuoteDetailLevel TickerQuoteDetailLevel { get; }

    ICandle?                     ConflationSummaryCandle    { get; }
    IPublishedCandles?           PublishedCandles           { get; }
    IPublishedIndicators?        PublishedIndicators        { get; }
    IAdditionalTrackingFields?   AdditionalTrackingFields   { get; }
    IPublishedSignals?           PublishedSignals           { get; }
    IPublishedStrategyDecisions? PublishedStrategyDecisions { get; }

    IPublishedContinuousPriceAdjustments? ContinuousPriceAdjustments { get; }
}

public interface IMutableAncillaryPricingFeedEvent : IAncillaryPricingFeedEvent, IMutableTradingStatusFeedEvent
{
    new IMutableCandle?                     ConflationSummaryCandle    { get; set; }
    new IMutablePublishedCandles?           PublishedCandles           { get; set; }
    new IMutablePublishedIndicators?        PublishedIndicators        { get; set; }
    new IMutableAdditionalTrackingFields?   AdditionalTrackingFields   { get; set; }
    new IMutableTickerRegionInfo?           TickerRegionInfo           { get; set; }
    new IMutablePublishedSignals?           PublishedSignals           { get; set; }
    new IMutablePublishedStrategyDecisions? PublishedStrategyDecisions { get; set; }

    new IMutablePublishedContinuousPriceAdjustments? ContinuousPriceAdjustments { get; set; }
}