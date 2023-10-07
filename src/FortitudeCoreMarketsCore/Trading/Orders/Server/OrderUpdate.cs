#region

using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Server;

public class OrderUpdate : TradingMessage, IOrderUpdate
{
    public OrderUpdate(IOrderUpdate toClone) : base(toClone)
    {
        Order = toClone.Order!.Clone();
        OrderUpdateType = toClone.OrderUpdateType;
        AdapterUpdateTime = toClone.AdapterUpdateTime;
        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public OrderUpdate(IOrder order, OrderUpdateEventType orderUpdateType, DateTime adapterUpdateTime)
    {
        Order = order;
        OrderUpdateType = orderUpdateType;
        AdapterUpdateTime = adapterUpdateTime;
    }

    public override uint MessageId => (uint)TradingMessageIds.OrderUpdate;

    public IOrder? Order { get; set; }
    public OrderUpdateEventType OrderUpdateType { get; set; }
    public DateTime AdapterUpdateTime { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public IOrderUpdate Clone() => new OrderUpdate(this);
}
