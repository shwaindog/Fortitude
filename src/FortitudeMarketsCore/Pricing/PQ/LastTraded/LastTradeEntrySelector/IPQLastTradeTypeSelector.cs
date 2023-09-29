using System;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.LastTraded.EntrySelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector
{
    public interface IPQLastTradeTypeSelector : 
        ILastTradeEntryFlagsSelector<IPQRecentlyTradedFactory, IPQSourceTickerQuoteInfo>
    {
        bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType);
        IPQLastTrade SelectLastTradeEntry(IPQLastTrade original, ILastTrade desired);
    }
}