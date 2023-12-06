#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
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

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IOrderStatusUpdate orderStatusUpdate)
        {
            OrderId = orderStatusUpdate.OrderId.SyncOrRecycle(OrderId)!;
            NewOrderStatus = orderStatusUpdate.NewOrderStatus;
            EventType = orderStatusUpdate.EventType;
        }

        return this;
    }

    public override void Reset()
    {
        NewOrderStatus = OrderStatus.Unknown;
        EventType = OrderUpdateEventType.Unknown;
        base.Reset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxOrderStatusUpdate>().CopyFrom(this) ??
        new OrxOrderStatusUpdate(this);

    internal void Configure(IOrderId orderId, OrderStatus orderStatus, OrderUpdateEventType orderUpdateEventType,
        IRecycler orxRecyclerFactory)
    {
        Configure();
        if (orderId != null)
        {
            var orxOrderId = orxRecyclerFactory.Borrow<OrxOrderId>();
            orxOrderId.CopyFrom(orderId);
            OrderId = orxOrderId;
        }

        NewOrderStatus = orderStatus;
        EventType = orderUpdateEventType;
    }
}
