using System.Collections.Generic;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LastTraded
{
    public interface IRecentlyTraded : ICloneable<IRecentlyTraded>, IEnumerable<ILastTrade>,
        IInterfacesComparable<IRecentlyTraded>
    {
        bool HasLastTrades { get; }
        int Count { get; }
        int Capacity { get; }
        ILastTrade this[int i] { get; }
    }
}
