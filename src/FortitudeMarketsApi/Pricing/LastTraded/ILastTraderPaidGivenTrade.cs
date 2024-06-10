// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.LastTraded;

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
