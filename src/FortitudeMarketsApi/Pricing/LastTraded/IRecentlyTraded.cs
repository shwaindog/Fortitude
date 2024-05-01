#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface IRecentlyTraded : IReusableObject<IRecentlyTraded>, IEnumerable<ILastTrade>,
    IInterfacesComparable<IRecentlyTraded>
{
    LastTradeType LastTradesOfType { get; }
    LastTradedFlags LastTradesSupportFlags { get; }
    bool HasLastTrades { get; }
    int Count { get; }
    int Capacity { get; }
    ILastTrade? this[int i] { get; }
}
