using System;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LastTraded
{
    public interface ILastTrade : ICloneable<ILastTrade>, IInterfacesComparable<ILastTrade>
    {
        DateTime TradeTime { get; }
        decimal TradePrice { get; }
    }
}