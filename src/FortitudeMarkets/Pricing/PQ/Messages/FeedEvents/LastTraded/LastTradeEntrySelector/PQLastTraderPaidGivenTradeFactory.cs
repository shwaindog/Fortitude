using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public class PQLastTraderPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public PQLastTraderPaidGivenTradeFactory(IPQNameIdLookupGenerator nameIdLookup) => TraderNameIdLookup = nameIdLookup;

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    public Type EntryCreationType => typeof(PQLastExternalCounterPartyTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastExternalCounterPartyTrade(TraderNameIdLookup);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastExternalCounterPartyTrade(original, TraderNameIdLookup);
}
