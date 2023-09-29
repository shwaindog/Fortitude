using System;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.Orders.Server
{
    public class OrderAmendResponse : OrderUpdate, IOrderAmendResponse
    {
        public override uint MessageId => (uint) TradingMessageIds.Amend;

        public OrderAmendResponse()
        {
        }

        public OrderAmendResponse(IOrderAmendResponse toClone) : base(toClone)
        {
            AmendType = toClone.AmendType;
            OldOrderId = toClone.OldOrderId != null ? new OrderId(toClone.OldOrderId) : null;
        }

        public OrderAmendResponse(IOrder order, OrderUpdateEventType orderUpdateType, DateTime adapterUpdateTime, 
            AmendType amendType, IOrderId newOrderId) : base(order, orderUpdateType, adapterUpdateTime)
        {
            AmendType = amendType;
            OldOrderId = newOrderId;
        }

        public AmendType AmendType { get; set; }
        public IOrderId OldOrderId { get; set; }
        
        public new IOrderAmendResponse Clone()
        {
            return new OrderAmendResponse(this);
        }
    }
}