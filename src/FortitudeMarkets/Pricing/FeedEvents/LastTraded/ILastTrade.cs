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
[JsonDerivedType(typeof(LastTraderPaidGivenTrade))]
[JsonDerivedType(typeof(PQLastTrade))]
[JsonDerivedType(typeof(PQLastPaidGivenTrade))]
[JsonDerivedType(typeof(PQLastTraderPaidGivenTrade))]
public interface ILastTrade : IReusableObject<ILastTrade>, IInterfacesComparable<ILastTrade>, IShowsEmpty
{
    [JsonIgnore] LastTradeType   LastTradeType           { get; }
    [JsonIgnore] LastTradedFlags SupportsLastTradedFlags { get; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    DateTime TradeTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    decimal TradePrice { get; }
}

public interface IMutableLastTrade : ILastTrade, ITrackableReset<IMutableLastTrade>, IEmptyable
{
    new DateTime TradeTime  { get; set; }
    new decimal  TradePrice { get; set; }

    new IMutableLastTrade Clone();
}
