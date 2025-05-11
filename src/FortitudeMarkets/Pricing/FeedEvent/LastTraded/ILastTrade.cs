// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

[JsonDerivedType(typeof(LastTrade))]
[JsonDerivedType(typeof(LastPaidGivenTrade))]
[JsonDerivedType(typeof(LastTraderPaidGivenTrade))]
[JsonDerivedType(typeof(PQLastTrade))]
[JsonDerivedType(typeof(PQLastPaidGivenTrade))]
[JsonDerivedType(typeof(PQLastTraderPaidGivenTrade))]
public interface ILastTrade : IReusableObject<ILastTrade>, IInterfacesComparable<ILastTrade>
{
    [JsonIgnore] LastTradeType   LastTradeType           { get; }
    [JsonIgnore] LastTradedFlags SupportsLastTradedFlags { get; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    DateTime TradeTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    decimal TradePrice { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsEmpty { get; }
}

public interface IMutableLastTrade : ILastTrade
{
    new DateTime TradeTime  { get; set; }
    new decimal  TradePrice { get; set; }
    new bool     IsEmpty    { get; set; }

    new IMutableLastTrade Clone();
}
