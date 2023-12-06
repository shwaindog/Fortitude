#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server;

public class OrxOrderUpdate : OrxTradingMessage, IOrderUpdate
{
    public OrxOrderUpdate() { }

    public OrxOrderUpdate(IOrderUpdate toClone) : base(toClone)
    {
        Order = new OrxOrder(toClone.Order!);
        AdapterUpdateTime = toClone.AdapterUpdateTime;
        OrderUpdateType = toClone.OrderUpdateType;
    }

    public OrxOrderUpdate(IOrder order, OrderUpdateEventType reason, DateTime adapterUpdateTime)
    {
        Order = order is OrxOrder orxOrder ? orxOrder : new OrxOrder(order);
        OrderUpdateType = reason;
        AdapterUpdateTime = adapterUpdateTime;
    }

    [OrxMandatoryField(9)] public OrxOrder? Order { get; set; }

    public override uint MessageId => (ushort)TradingMessageIds.OrderUpdate;

    IOrder? IOrderUpdate.Order
    {
        get => Order;
        set => Order = value as OrxOrder;
    }

    [OrxMandatoryField(10)] public OrderUpdateEventType OrderUpdateType { get; set; }

    [OrxOptionalField(11)] public DateTime AdapterUpdateTime { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    public override void Reset()
    {
        OrderUpdateType = OrderUpdateEventType.Unknown;
        AdapterUpdateTime = DateTimeConstants.UnixEpoch;
        Order?.DecrementRefCount();
        Order = null;
        base.Reset();
    }

    public override IVersionedMessage CopyFrom(IVersionedMessage versionedMessage
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(versionedMessage, copyMergeFlags);
        if (versionedMessage is IOrderUpdate orderUpdate)
        {
            Order = orderUpdate.Order.SyncOrRecycle(Order);
            AdapterUpdateTime = orderUpdate.AdapterUpdateTime;
            OrderUpdateType = orderUpdate.OrderUpdateType;
        }

        return this;
    }

    public override IOrderUpdate Clone() =>
        (IOrderUpdate?)Recycler?.Borrow<OrxOrderUpdate>().CopyFrom(this) ?? new OrxOrderUpdate(this);
}
