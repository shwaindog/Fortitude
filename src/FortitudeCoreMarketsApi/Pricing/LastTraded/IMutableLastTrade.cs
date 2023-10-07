using System;
using FortitudeCommon.Types;

namespace FortitudeMarketsApi.Pricing.LastTraded
{
    public interface IMutableLastTrade : ILastTrade, IStoreState<ILastTrade>
    {
        new DateTime TradeTime { get; set; }
        new decimal TradePrice { get; set; }
        bool IsEmpty { get; }
        void Reset();
        new IMutableLastTrade Clone();
    }
}