using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public class PQLastTraderPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public PQLastTraderPaidGivenTradeFactory(IPQNameIdLookupGenerator nameIdLookup) => TraderNameIdLookup = nameIdLookup;

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    public Type EntryCreationType => typeof(PQLastTraderPaidGivenTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastTraderPaidGivenTrade(TraderNameIdLookup, 0m, DateTime.MinValue);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastTraderPaidGivenTrade(original, TraderNameIdLookup);
}
