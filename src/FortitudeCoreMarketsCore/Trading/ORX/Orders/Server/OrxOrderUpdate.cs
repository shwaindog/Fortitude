#region

using FortitudeCommon.DataStructures.Memory;
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

    [OrxMandatoryField(10)] public OrxOrder? Order { get; set; }

    public override uint MessageId => (ushort)TradingMessageIds.OrderUpdate;

    IOrder? IOrderUpdate.Order
    {
        get => Order;
        set => Order = value as OrxOrder;
    }

    [OrxMandatoryField(11)] public OrderUpdateEventType OrderUpdateType { get; set; }

    [OrxOptionalField(12)] public DateTime AdapterUpdateTime { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    public IOrderUpdate Clone() => new OrxOrderUpdate(this);

    public void CopyFrom(IOrderUpdate orderUpdate, IRecycler orxRecyclingFactory)
    {
        base.CopyFrom(orderUpdate, orxRecyclingFactory);
        if (orderUpdate.Order != null)
        {
            var orxOrder = orxRecyclingFactory.Borrow<OrxOrder>();
            orxOrder.CopyFrom(orderUpdate.Order, orxRecyclingFactory);
            Order = orxOrder;
        }

        AdapterUpdateTime = orderUpdate.AdapterUpdateTime;
        OrderUpdateType = orderUpdate.OrderUpdateType;
    }
}
