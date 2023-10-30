#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server;

public class OrxOrderStatusUpdate : OrxTradingMessage, IOrderStatusUpdate
{
    public OrxOrderStatusUpdate() => OrderId = new OrxOrderId();

    public OrxOrderStatusUpdate(IOrderStatusUpdate toClone)
    {
        OrderId = new OrxOrderId(toClone.OrderId);
        NewOrderStatus = toClone.NewOrderStatus;
        EventType = toClone.EventType;
    }

    public OrxOrderStatusUpdate(OrxOrderId orderId, OrderStatus newOrderStatus, OrderUpdateEventType eventType)
    {
        OrderId = orderId;
        NewOrderStatus = newOrderStatus;
        EventType = eventType;
    }

    public OrxOrderId OrderId { get; set; }
    public override uint MessageId => (uint)TradingMessageIds.StatusUpdate;

    IOrderId IOrderStatusUpdate.OrderId
    {
        get => OrderId;
        set => OrderId = (OrxOrderId)value;
    }

    public OrderStatus NewOrderStatus { get; set; }
    public OrderUpdateEventType EventType { get; set; }

    internal void Configure(IOrderId orderId, OrderStatus orderStatus, OrderUpdateEventType orderUpdateEventType,
        IRecycler orxRecyclerFactory)
    {
        Configure();
        if (orderId != null)
        {
            var orxOrderId = orxRecyclerFactory.Borrow<OrxOrderId>();
            orxOrderId.CopyFrom(orderId, orxRecyclerFactory);
            OrderId = orxOrderId;
        }

        NewOrderStatus = orderStatus;
        EventType = orderUpdateEventType;
    }
}
