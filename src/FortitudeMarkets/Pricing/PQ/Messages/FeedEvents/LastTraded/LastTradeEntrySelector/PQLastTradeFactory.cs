
namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public class PQLastTradeFactory : IPQRecentlyTradedFactory
{
    public virtual Type EntryCreationType => typeof(PQLastTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastTrade(0m, DateTime.MinValue);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastTrade(original);
}
