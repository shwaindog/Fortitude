using System;
using FortitudeCommon.Chronometry;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector
{
    public class PQLastTraderPaidGivenTradeFactory : IPQRecentlyTradedFactory
    {
        public PQLastTraderPaidGivenTradeFactory(IPQNameIdLookupGenerator nameIdLookup)
        {
            TraderNameIdLookup = nameIdLookup;
        }

        public IPQNameIdLookupGenerator TraderNameIdLookup { get; set; }
        public Type EntryCreationType => typeof(PQLastTraderPaidGivenTrade);

        public IPQLastTrade CreateNewLastTradeEntry()
        {
            return new PQLastTraderPaidGivenTrade(0m, DateTimeConstants.UnixEpoch, 0m, false, false,
                TraderNameIdLookup);
        }

        public IPQLastTrade UpgradeLayer(IPQLastTrade original)
        {
            return new PQLastTraderPaidGivenTrade(original);
        }
    }
}