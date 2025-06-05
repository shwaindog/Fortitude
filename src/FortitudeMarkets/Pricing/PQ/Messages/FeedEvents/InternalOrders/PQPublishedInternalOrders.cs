// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

public interface IPQPublishedInternalOrders : IMutablePublishedInternalOrders, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{

}
