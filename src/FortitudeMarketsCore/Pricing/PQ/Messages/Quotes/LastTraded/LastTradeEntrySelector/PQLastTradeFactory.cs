#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public class PQLastTradeFactory : IPQRecentlyTradedFactory
{
    public virtual Type EntryCreationType => typeof(PQLastTrade);

    public IPQLastTrade CreateNewLastTradeEntry() => new PQLastTrade(0m, DateTimeConstants.UnixEpoch);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastTrade(original);
}
