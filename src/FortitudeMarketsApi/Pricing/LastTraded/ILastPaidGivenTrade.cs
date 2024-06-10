// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface ILastPaidGivenTrade : ILastTrade
{
    bool    WasPaid     { get; }
    bool    WasGiven    { get; }
    decimal TradeVolume { get; }

    new ILastPaidGivenTrade Clone();
}

public interface IMutableLastPaidGivenTrade : IMutableLastTrade, ILastPaidGivenTrade
{
    new bool    WasPaid     { get; set; }
    new bool    WasGiven    { get; set; }
    new decimal TradeVolume { get; set; }

    new IMutableLastPaidGivenTrade Clone();
}
