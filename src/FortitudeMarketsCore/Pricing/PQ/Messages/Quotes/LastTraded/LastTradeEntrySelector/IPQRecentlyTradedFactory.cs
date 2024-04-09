namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public interface IPQRecentlyTradedFactory
{
    Type EntryCreationType { get; }
    IPQLastTrade CreateNewLastTradeEntry();
    IPQLastTrade UpgradeLayer(IPQLastTrade original);
}
