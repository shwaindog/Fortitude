namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public interface IPQRecentlyTradedFactory
{
    Type EntryCreationType { get; }
    IPQLastTrade CreateNewLastTradeEntry();
    IPQLastTrade UpgradeLayer(IPQLastTrade original);
}
