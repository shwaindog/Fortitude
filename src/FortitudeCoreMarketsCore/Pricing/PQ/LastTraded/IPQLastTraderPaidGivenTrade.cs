using System;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded
{
    public interface IPQLastTraderPaidGivenTrade : IPQLastPaidGivenTrade, IMutableLastTraderPaidGivenTrade,
        IPQSupportsStringUpdates<ILastTrade>
    {
        int TraderId { get; set; }
        bool IsTraderNameUpdated { get; set; }
        IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
        new IPQLastTraderPaidGivenTrade Clone();
    }
}