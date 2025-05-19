// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public enum OrderUpdateReason
{
    None
  , HasSourceStatusChange
  , HasChildOrderUpdate
  , HasOrderAmendUpdate
  , HasPositionFillChange
}
