#region

using FortitudeMarketsApi.Pricing.LastTraded;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded;

public interface IPQLastPaidGivenTrade : IPQLastTrade, IMutableLastPaidGivenTrade
{
    bool IsWasPaidUpdated { get; set; }
    bool IsWasGivenUpdated { get; set; }
    bool IsTradeVolumeUpdated { get; set; }
    new IPQLastPaidGivenTrade Clone();
}
