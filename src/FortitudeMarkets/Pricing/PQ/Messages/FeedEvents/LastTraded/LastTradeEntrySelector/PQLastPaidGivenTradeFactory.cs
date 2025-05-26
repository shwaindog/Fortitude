namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public class PQLastPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public Type EntryCreationType => typeof(PQLastPaidGivenTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastPaidGivenTrade();

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastPaidGivenTrade(original);
}
