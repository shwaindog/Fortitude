// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Text.Json.Serialization;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface ILastPaidGivenTrade : ILastTrade
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    uint OrderId { get; }

    bool    WasPaid     { get; }
    bool    WasGiven    { get; }
    decimal TradeVolume { get; }

    new ILastPaidGivenTrade Clone();
}

public interface IMutableLastPaidGivenTrade : IMutableLastTrade, ILastPaidGivenTrade
{
    new uint    OrderId     { get; set; }
    new bool    WasPaid     { get; set; }
    new bool    WasGiven    { get; set; }
    new decimal TradeVolume { get; set; }

    new IMutableLastPaidGivenTrade Clone();
}
