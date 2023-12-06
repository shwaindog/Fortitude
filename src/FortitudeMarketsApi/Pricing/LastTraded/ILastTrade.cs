#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface ILastTrade : IReusableObject<ILastTrade>, IInterfacesComparable<ILastTrade>
{
    DateTime TradeTime { get; }
    decimal TradePrice { get; }
    bool IsEmpty { get; }
}
