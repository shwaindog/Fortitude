using System;
using FortitudeCommon.Chronometry;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector
{
    public class PQLastTradeFactory : IPQRecentlyTradedFactory
    {
        public virtual Type EntryCreationType => typeof(PQLastTrade);

        public IPQLastTrade CreateNewLastTradeEntry()
        {
            return new PQLastTrade(0m, DateTimeConstants.UnixEpoch);
        }

        public IPQLastTrade UpgradeLayer(IPQLastTrade original)
        {
            return new PQLastTrade(original);
        }
    }
}