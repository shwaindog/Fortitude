#region

using FortitudeCommon.Chronometry;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public class PQLastTraderPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public PQLastTraderPaidGivenTradeFactory(IPQNameIdLookupGenerator nameIdLookup) => TraderNameIdLookup = nameIdLookup;

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    public Type EntryCreationType => typeof(PQLastTraderPaidGivenTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastTraderPaidGivenTrade(TraderNameIdLookup, 0m, DateTimeConstants.UnixEpoch);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastTraderPaidGivenTrade(original, TraderNameIdLookup);
}
