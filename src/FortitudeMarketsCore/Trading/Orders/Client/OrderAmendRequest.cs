using System;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;

namespace FortitudeMarketsCore.Trading.Orders.Client
{
    public class OrderAmendRequest : OrderSubmitRequest, IOrderAmendRequest
    {
        public OrderAmendRequest(IOrderAmendRequest toClone) : base(toClone)
        {
            Amendment = toClone.Amendment;
        }

        public OrderAmendRequest(IOrder orderDetails, 
            DateTime originalAttemptNumber, DateTime currentAttempt, int attemptNumber, string tag,
            IOrderAmend orderAmendment) :
            base(orderDetails, attemptNumber, currentAttempt, originalAttemptNumber, tag)
        {
            Amendment = orderAmendment;
        }

        public IOrderAmend Amendment { get; set; }
    }
}