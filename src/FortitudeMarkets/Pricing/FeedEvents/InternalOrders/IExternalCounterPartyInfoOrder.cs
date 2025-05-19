// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IAdditionalExternalCounterPartyInfo
{
    int ExternalCounterPartyId { get; }

    string? ExternalCounterPartyName { get; }

    int ExternalTraderId { get;  }

    string? ExternalTraderName { get; }
}

public interface IMutableAdditionalExternalCounterPartyInfo : IAdditionalExternalCounterPartyInfo
{
    new int ExternalCounterPartyId { get; set; }

    new string? ExternalCounterPartyName { get; set; }

    new int ExternalTraderId { get; set; }

    new string? ExternalTraderName { get; set; }
}

public interface IExternalCounterPartyInfoOrder : IPublishedOrder, IAdditionalExternalCounterPartyInfo
{
}

public interface IMutableExternalCounterPartyInfoOrder : IExternalCounterPartyInfoOrder, IMutableAdditionalExternalCounterPartyInfo,  IMutablePublishedOrder
{
}