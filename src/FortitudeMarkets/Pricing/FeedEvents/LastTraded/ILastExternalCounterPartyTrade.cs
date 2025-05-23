// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface ILastExternalCounterPartyTrade : ILastPaidGivenTrade
{
    int     ExternalCounterPartyId   { get; }
    string? ExternalCounterPartyName { get; }
    int     ExternalTraderId         { get; }
    string? ExternalTraderName       { get; }

    new ILastExternalCounterPartyTrade Clone();
}

public interface IMutableLastExternalCounterPartyTrade : IMutableLastPaidGivenTrade, ILastExternalCounterPartyTrade
{
    new int     ExternalCounterPartyId   { get; set; }
    new string? ExternalCounterPartyName { get; set; }
    new int     ExternalTraderId         { get; set; }
    new string? ExternalTraderName       { get; set; }

    new IMutableLastExternalCounterPartyTrade Clone();
}
