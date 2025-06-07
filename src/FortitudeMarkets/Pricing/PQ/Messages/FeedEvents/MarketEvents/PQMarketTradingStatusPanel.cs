// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.MarketEvents;

public interface IPQMarketTradingStatusPanel : IMutableMarketTradingStatusPanel, ITrackableReset<IPQMarketTradingStatusPanel>, ICloneable<IPQMarketTradingStatusPanel>
  , IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{
    new IPQMarketTradingStatusPanel ResetWithTracking();
    new IPQMarketTradingStatusPanel Clone();
}
