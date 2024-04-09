#region

using FortitudeCommon.Chronometry;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public class PQLastTraderPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public PQLastTraderPaidGivenTradeFactory(IPQNameIdLookupGenerator nameIdLookup) => TraderNameIdLookup = nameIdLookup;

    public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
    public Type EntryCreationType => typeof(PQLastTraderPaidGivenTrade);

    public IPQLastTrade CreateNewLastTradeEntry() =>
        new PQLastTraderPaidGivenTrade(0m, DateTimeConstants.UnixEpoch, 0m, false, false,
            TraderNameIdLookup);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastTraderPaidGivenTrade(original);
}
