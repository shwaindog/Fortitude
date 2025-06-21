// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Server;

public class OrxOrderUpdate : OrxTradingMessage, IOrderUpdate, ITransferState<OrxOrderUpdate>
{
    private OrxOrder? order;
    public OrxOrderUpdate() { }

    public OrxOrderUpdate(IOrderUpdate toClone) : base(toClone)
    {
        CopyFrom(toClone);
    }

    public OrxOrderUpdate(IOrder order, OrderUpdateEventType reason, DateTime adapterUpdateTime)
    {
        Order             = order.AsOrxOrder();
        OrderUpdateType   = reason;
        AdapterUpdateTime = adapterUpdateTime;
    }

    [OrxMandatoryField(9)]
    public OrxOrder? Order
    {
        get => order;
        set
        {
            if (ReferenceEquals(value, order)) return;
            value?.IncrementRefCount();
            order?.DecrementRefCount();
            order = value;
        }
    }

    public override uint MessageId => (ushort)TradingMessageIds.OrderUpdate;

    IOrder? IOrderUpdate.Order
    {
        get => Order;
        set => Order = value as OrxOrder;
    }

    [OrxMandatoryField(10)] public OrderUpdateEventType OrderUpdateType { get; set; }

    [OrxOptionalField(11)] public DateTime AdapterUpdateTime { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    public override void StateReset()
    {
        OrderUpdateType   = OrderUpdateEventType.Unknown;
        AdapterUpdateTime = DateTimeConstants.UnixEpoch;
        Order             = null; // decremented in setter
        base.StateReset();
    }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage versionedMessage
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is IOrderUpdate orderUpdate)
        {
            if (Order == null)
            {
                Order = orderUpdate.Order?.AsOrxOrder().Clone();
            } else if (orderUpdate.Order != null)
            {
                Order.CopyFrom(orderUpdate.Order, copyMergeFlags);
            }
            AdapterUpdateTime = orderUpdate.AdapterUpdateTime;
            OrderUpdateType   = orderUpdate.OrderUpdateType;
        }

        return this;
    }

    public IOrderUpdate CopyFrom(IOrderUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (Order == null)
        {
            Order = source.Order?.AsOrxOrder().Clone();
        } else if (source.Order != null)
        {
            Order.CopyFrom(source.Order, copyMergeFlags);
        }
        OrderUpdateType   = source.OrderUpdateType;
        AdapterUpdateTime = source.AdapterUpdateTime;
        return this;
    }


    public override IOrderUpdate Clone() => Recycler?.Borrow<OrxOrderUpdate>().CopyFrom(this) ?? new OrxOrderUpdate(this);

    public OrxOrderUpdate CopyFrom(OrxOrderUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (Order == null)
        {
            Order = source.Order?.AsOrxOrder().Clone();
        } else if (source.Order != null)
        {
            Order.CopyFrom(source.Order, copyMergeFlags);
        }
        OrderUpdateType   = source.OrderUpdateType;
        AdapterUpdateTime = source.AdapterUpdateTime;
        return this;
    }


    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("OrxOrderUpdate(");
        if (Order != null) sb.Append("VenueOrder: ").Append(Order).Append(", ");
        if (OrderUpdateType != OrderUpdateEventType.Unknown) sb.Append("OrderUpdateType: ").Append(OrderUpdateType).Append(", ");
        if (AdapterUpdateTime != DateTimeConstants.UnixEpoch) sb.Append("AdapterUpdateTime: ").Append(AdapterUpdateTime).Append(", ");
        if (ClientReceivedTime != DateTimeConstants.UnixEpoch) sb.Append("ClientReceivedTime: ").Append(ClientReceivedTime).Append(", ");
        if (sb[^2] == ',')
        {
            sb[^2]    =  ')';
            sb.Length -= 1;
        }

        return sb.ToString();
    }
}
