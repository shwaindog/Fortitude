#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.LastTraded.EntrySelector;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public interface IPQLastTradeTypeSelector :
    ILastTradeEntryFlagsSelector<IPQRecentlyTradedFactory, IPQSourceTickerQuoteInfo>
{
    bool TypeCanWholeyContain(Type copySourceType, Type copyDestinationType);
    IPQLastTrade? SelectLastTradeEntry(IPQLastTrade? original, ILastTrade? desired);
}
