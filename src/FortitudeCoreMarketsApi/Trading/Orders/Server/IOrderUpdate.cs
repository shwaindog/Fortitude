using System;

namespace FortitudeMarketsApi.Trading.Orders.Server
{
    public interface IOrderUpdate : ITradingMessage
    {
        IOrder Order { get; set; }
        OrderUpdateEventType OrderUpdateType { get; set; }
        DateTime AdapterUpdateTime { get; set; }
        DateTime ClientReceivedTime { get; set; }
        IOrderUpdate Clone();
    }
}
