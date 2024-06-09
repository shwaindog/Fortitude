// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IMutableLastTrade : ILastTrade
{
    new DateTime TradeTime  { get; set; }
    new decimal  TradePrice { get; set; }
    new bool     IsEmpty    { get; set; }

    new IMutableLastTrade Clone();
}
