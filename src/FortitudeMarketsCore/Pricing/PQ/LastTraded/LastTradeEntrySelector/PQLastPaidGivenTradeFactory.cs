#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;

public class PQLastPaidGivenTradeFactory : IPQRecentlyTradedFactory
{
    public Type EntryCreationType => typeof(PQLastPaidGivenTrade);

    public IPQLastTrade CreateNewLastTradeEntry() =>
        new PQLastPaidGivenTrade(0m, DateTimeConstants.UnixEpoch, 0m, false, false);

    public IPQLastTrade UpgradeLayer(IPQLastTrade original) => new PQLastPaidGivenTrade(original);
}
