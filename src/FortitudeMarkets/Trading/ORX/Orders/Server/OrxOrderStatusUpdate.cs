// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Server;

public class OrxOrderStatusUpdate : OrxTradingMessage, IOrderStatusUpdate
{
    public OrxOrderStatusUpdate() => OrderId = new OrxOrderId();

    public OrxOrderStatusUpdate(IOrderStatusUpdate toClone)
    {
        OrderId   = new OrxOrderId(toClone.OrderId);
        EventType = toClone.EventType;

        NewOrderStatus = toClone.NewOrderStatus;
    }

    public OrxOrderStatusUpdate(OrxOrderId orderId, OrderStatus newOrderStatus, OrderUpdateEventType eventType)
    {
        OrderId   = orderId;
        EventType = eventType;

        NewOrderStatus = newOrderStatus;
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

    public override IVersionedMessage CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IOrderStatusUpdate orderStatusUpdate)
        {
            OrderId = orderStatusUpdate.OrderId.SyncOrRecycle(OrderId)!;

            EventType = orderStatusUpdate.EventType;

            NewOrderStatus = orderStatusUpdate.NewOrderStatus;
        }

        return this;
    }

    public override void StateReset()
    {
        NewOrderStatus = OrderStatus.Unknown;

        EventType = OrderUpdateEventType.Unknown;

        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxOrderStatusUpdate>().CopyFrom(this) ??
        new OrxOrderStatusUpdate(this);

    internal void Configure
    (IOrderId orderId, OrderStatus orderStatus, OrderUpdateEventType orderUpdateEventType,
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
