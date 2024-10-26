// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

public interface ILastTrade : IReusableObject<ILastTrade>, IInterfacesComparable<ILastTrade>
{
    LastTradeType   LastTradeType           { get; }
    LastTradedFlags SupportsLastTradedFlags { get; }

    DateTime TradeTime  { get; }
    decimal  TradePrice { get; }
    bool     IsEmpty    { get; }
}

public interface IMutableLastTrade : ILastTrade
{
    new DateTime TradeTime  { get; set; }
    new decimal  TradePrice { get; set; }
    new bool     IsEmpty    { get; set; }

    new IMutableLastTrade Clone();
}
