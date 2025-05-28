// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[JsonDerivedType(typeof(LastTrade))]
[JsonDerivedType(typeof(LastPaidGivenTrade))]
[JsonDerivedType(typeof(LastExternalCounterPartyTrade))]
[JsonDerivedType(typeof(PQLastTrade))]
[JsonDerivedType(typeof(PQLastPaidGivenTrade))]
[JsonDerivedType(typeof(PQLastExternalCounterPartyTrade))]
public interface ILastTrade : IReusableObject<ILastTrade>, IInterfacesComparable<ILastTrade>, IShowsEmpty
{
    [JsonIgnore] LastTradeType   LastTradeType           { get; }
    [JsonIgnore] LastTradedFlags SupportsLastTradedFlags { get; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    uint TradeId { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    uint BatchId { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    LastTradedTypeFlags TradeTypeFlags { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    LastTradedLifeCycleFlags TradeLifeCycleStatus { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    DateTime FirstNotifiedTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    DateTime AdapterReceivedTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    DateTime UpdateTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    DateTime TradeTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    decimal TradePrice { get; }
}

public interface IMutableLastTrade : IReusableObject<IMutableLastTrade>, ILastTrade, ITrackableReset<IMutableLastTrade>, IEmptyable
{
    new uint     TradeId             { get; set; }
    new uint     BatchId             { get; set; }
    new DateTime FirstNotifiedTime   { get; set; }
    new DateTime AdapterReceivedTime { get; set; }
    new DateTime TradeTime           { get; set; }
    new decimal  TradePrice          { get; set; }
    new DateTime UpdateTime          { get; set; }

    new LastTradedTypeFlags      TradeTypeFlags       { get; set; }
    new LastTradedLifeCycleFlags TradeLifeCycleStatus { get; set; }

    new IMutableLastTrade Clone();
}
