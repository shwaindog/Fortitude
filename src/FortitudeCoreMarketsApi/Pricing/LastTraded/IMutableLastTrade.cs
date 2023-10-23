#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IMutableLastTrade : ILastTrade, IStoreState<ILastTrade>
{
    new DateTime TradeTime { get; set; }
    new decimal TradePrice { get; set; }
    void Reset();
    new IMutableLastTrade Clone();
}
