// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface ILastTraderPaidGivenTrade : ILastPaidGivenTrade
{
    string? TraderName { get; }

    new ILastTraderPaidGivenTrade Clone();
}

public interface IMutableLastTraderPaidGivenTrade : IMutableLastPaidGivenTrade, ILastTraderPaidGivenTrade
{
    new string? TraderName { get; set; }

    new IMutableLastTraderPaidGivenTrade Clone();
}
