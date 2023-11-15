#region

using FortitudeCommon.Types;
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

    public IOrderUpdate Clone() => new OrxOrderUpdate(this);

    public void CopyFrom(IOrderUpdate orderUpdate, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(orderUpdate, copyMergeFlags);
        if (orderUpdate.Order != null)
        {
            Order ??= Recycler!.Borrow<OrxOrder>();
            Order.CopyFrom(orderUpdate.Order, copyMergeFlags);
        }
        else
        {
            if (Order != null)
            {
                Order.DecrementRefCount();
                Order = null;
            }
        }

        AdapterUpdateTime = orderUpdate.AdapterUpdateTime;
        OrderUpdateType = orderUpdate.OrderUpdateType;
    }
}
