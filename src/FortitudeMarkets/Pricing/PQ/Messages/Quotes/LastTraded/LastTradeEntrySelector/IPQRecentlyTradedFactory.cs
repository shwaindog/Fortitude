namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public interface IPQRecentlyTradedFactory
{
    Type EntryCreationType { get; }
    IPQLastTrade CreateNewLastTradeEntry();
    IPQLastTrade UpgradeLayer(IPQLastTrade original);
}
