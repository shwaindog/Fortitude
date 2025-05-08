#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.Orders.Server;

public class OrderUpdate : TradingMessage, IOrderUpdate
{
    private IOrder? order;
    public OrderUpdate() { }

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
        Order.IncrementRefCount();
        OrderUpdateType = orderUpdateType;
        AdapterUpdateTime = adapterUpdateTime;
    }

    public override uint MessageId => (uint)TradingMessageIds.OrderUpdate;

    public IOrder? Order
    {
        get => order;
        set
        {
            if (value == order) return;
            value?.IncrementRefCount();
            order?.DecrementRefCount();
            order = value;
        }
    }

    public OrderUpdateEventType OrderUpdateType { get; set; }
    public DateTime AdapterUpdateTime { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public override void StateReset()
    {
        Order?.DecrementRefCount();
        Order = null;
        OrderUpdateType = OrderUpdateEventType.Unknown;
        AdapterUpdateTime = DateTimeConstants.UnixEpoch;
        ClientReceivedTime = DateTimeConstants.UnixEpoch;
        base.StateReset();
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IOrderUpdate orderUpdate)
        {
            Order = orderUpdate.Order.SyncOrRecycle(Order as Order);
            OrderUpdateType = orderUpdate.OrderUpdateType;
            AdapterUpdateTime = orderUpdate.AdapterUpdateTime;
            ClientReceivedTime = orderUpdate.ClientReceivedTime;
        }

        return this;
    }

    public IOrderUpdate CopyFrom(IOrderUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IOrderUpdate)CopyFrom((IVersionedMessage)source, copyMergeFlags);

    public override IOrderUpdate Clone() => Recycler?.Borrow<OrderUpdate>().CopyFrom(this) ?? new OrderUpdate(this);

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("OrderUpdate(");
        if (Order != null) sb.Append("VenueOrder: ").Append(Order).Append(", ");
        if (OrderUpdateType != OrderUpdateEventType.Unknown)
            sb.Append("OrderUpdateType: ").Append(OrderUpdateType).Append(", ");
        if (AdapterUpdateTime != DateTimeConstants.UnixEpoch)
            sb.Append("AdapterUpdateTime: ").Append(AdapterUpdateTime).Append(", ");
        if (ClientReceivedTime != DateTimeConstants.UnixEpoch)
            sb.Append("ClientReceivedTime: ").Append(ClientReceivedTime).Append(", ");
        if (sb[^2] == ',')
        {
            sb[^2] = ')';
            sb.Length -= 1;
        }

        return sb.ToString();
    }
}
