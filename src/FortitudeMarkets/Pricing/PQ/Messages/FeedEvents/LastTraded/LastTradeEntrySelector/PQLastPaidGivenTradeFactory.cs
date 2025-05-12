namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public class PQLastPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public Type EntryCreationType => typeof(PQLastPaidGivenTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastPaidGivenTrade(0m, DateTime.MinValue, 0m, false, false);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastPaidGivenTrade(original);
}
