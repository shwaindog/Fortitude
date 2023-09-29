using System;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.Orders.Client
{
    public class OrderSubmitRequest : TradingMessage, IOrderSubmitRequest
    {
        public override uint MessageId => (uint) TradingMessageIds.SubmitRequest;

        public OrderSubmitRequest(IOrderSubmitRequest toClone) : base(toClone)
        {
            OrderDetails = toClone.OrderDetails.Clone();
            OriginalAttemptTime = toClone.OriginalAttemptTime;
            CurrentAttemptTime = toClone.CurrentAttemptTime;
            AttemptNumber = toClone.AttemptNumber;
            Tag = toClone.Tag;
        }

        public OrderSubmitRequest(IOrder orderDetails, int attemptNumber, DateTime currentAttemptTime, 
            DateTime originalAttemptTime, string tag)
        : this(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, (MutableString)tag )
        {
        }

        public OrderSubmitRequest(IOrder orderDetails,
            int attemptNumber, DateTime currentAttemptTime, DateTime originalAttemptTime, IMutableString tag)
        {
            OrderDetails = orderDetails;
            OriginalAttemptTime = originalAttemptTime;
            CurrentAttemptTime = currentAttemptTime;
            AttemptNumber = attemptNumber;
            Tag = tag;
        }
        
        public IOrder OrderDetails { get; set; }
        public DateTime OriginalAttemptTime { get; set; }
        public DateTime CurrentAttemptTime { get; set; }
        public int AttemptNumber { get; set; }
        public IMutableString Tag { get; set; }
        public virtual IOrderSubmitRequest Clone()
        {
            return new OrderSubmitRequest(this);
        }
    }
}