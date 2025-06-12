// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.MarketEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.MarketEvents;

public interface IPQMarketCalendarPanel : IMutableMarketCalendarPanel, ITrackableReset<IPQMarketCalendarPanel>, ICloneable<IPQMarketCalendarPanel>
  , IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{
    new IPQMarketCalendarPanel ResetWithTracking();
    new IPQMarketCalendarPanel Clone();
}
