namespace FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;

public interface IPQRecentlyTradedFactory
{
    Type EntryCreationType { get; }
    IPQLastTrade CreateNewLastTradeEntry();
    IPQLastTrade UpgradeLayer(IPQLastTrade original);
}
