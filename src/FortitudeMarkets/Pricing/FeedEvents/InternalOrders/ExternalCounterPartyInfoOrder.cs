// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public class ExternalCounterPartyInfoOrder : PublishedOrder, IMutableExternalCounterPartyInfoOrder
{
    public int     ExternalCounterPartyId   { get; set; }
    public string? ExternalCounterPartyName { get; set; }

    public int     ExternalTraderId   { get; set; }
    public string? ExternalTraderName { get; set; }
}
