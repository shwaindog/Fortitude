#region

using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.Orders.Server;

public class OrderAmendResponse : OrderUpdate, IOrderAmendResponse
{
    public OrderAmendResponse(IOrderAmendResponse toClone) : base(toClone)
    {
        AmendType = toClone.AmendType;
        OldOrderId = new OrderId(toClone.OldOrderId!);
    }

    public OrderAmendResponse(IOrder order, OrderUpdateEventType orderUpdateType, DateTime adapterUpdateTime,
        AmendType amendType, IOrderId newOrderId) : base(order, orderUpdateType, adapterUpdateTime)
    {
        AmendType = amendType;
        OldOrderId = newOrderId;
    }

    public override uint MessageId => (uint)TradingMessageIds.Amend;

    public AmendType AmendType { get; set; }
    public IOrderId? OldOrderId { get; set; }

    public new IOrderAmendResponse Clone() => new OrderAmendResponse(this);
}
