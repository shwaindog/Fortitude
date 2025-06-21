#region

using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;

#endregion

namespace FortitudeMarkets.Trading.Orders.Client;

public class OrderAmendRequest : OrderSubmitRequest, IOrderAmendRequest
{
    public OrderAmendRequest(IOrderAmendRequest toClone) : base(toClone) => Amendment = toClone.Amendment;

    public OrderAmendRequest(ITransmittableOrder orderDetails,
        DateTime originalAttemptNumber, DateTime currentAttempt, int attemptNumber, string tag,
        IOrderAmend orderAmendment) :
        base(orderDetails, attemptNumber, currentAttempt, originalAttemptNumber, tag) =>
        Amendment = orderAmendment;

    public IOrderAmend? Amendment { get; set; }
}
