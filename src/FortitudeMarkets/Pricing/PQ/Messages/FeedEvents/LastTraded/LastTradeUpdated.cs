﻿namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[Flags]
public enum LastTradeUpdated : byte
{
    None = 0x00
    , TradePriceUpdated = 0x01
    , TradeTimeDateUpdated = 0x02
    , TradeTimeSub2MinUpdated = 0x04
    , TraderNameUpdated = 0x08
    , WasPaidUpdated = 0x10
    , WasGivenUpdated = 0x20
    , VolumeUpdated = 0x40
}
