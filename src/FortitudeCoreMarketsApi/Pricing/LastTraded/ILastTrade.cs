#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.LastTraded;

public interface ILastTrade : ICloneable<ILastTrade>, IInterfacesComparable<ILastTrade>
{
    DateTime TradeTime { get; }
    decimal TradePrice { get; }
    bool IsEmpty { get; }
}
