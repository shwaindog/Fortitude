#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Server;

public class OrderUpdate : TradingMessage, IOrderUpdate
{
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
        OrderUpdateType = orderUpdateType;
        AdapterUpdateTime = adapterUpdateTime;
    }

    public override uint MessageId => (uint)TradingMessageIds.OrderUpdate;

    public IOrder? Order { get; set; }
    public OrderUpdateEventType OrderUpdateType { get; set; }
    public DateTime AdapterUpdateTime { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public override void Reset()
    {
        Order?.DecrementRefCount();
        Order = null;
        OrderUpdateType = OrderUpdateEventType.Unknown;
        AdapterUpdateTime = DateTimeConstants.UnixEpoch;
        ClientReceivedTime = DateTimeConstants.UnixEpoch;
        base.Reset();
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

    public override IOrderUpdate Clone() =>
        (IOrderUpdate?)Recycler?.Borrow<OrderUpdate>().CopyFrom(this) ?? new OrderUpdate(this);
}
