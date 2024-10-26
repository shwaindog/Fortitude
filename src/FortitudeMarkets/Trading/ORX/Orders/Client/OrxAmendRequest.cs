#region

using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Client;

public class OrxAmendRequest : OrxOrderSubmitRequest, IOrderAmendRequest
{
    private OrxOrderAmend? amendment;
    public OrxAmendRequest() { }

    public OrxAmendRequest(IOrderAmendRequest toClone) : base(toClone) =>
        Amendment = toClone.Amendment != null ? new OrxOrderAmend(toClone.Amendment) : null;

    public OrxAmendRequest(OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
        DateTime originalAttemptTime, string? tag, OrxOrderAmend amendment)
        : base(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, tag) =>
        Amendment = amendment;

    [OrxMandatoryField(20)]
    public OrxOrderAmend? Amendment
    {
        get => amendment;
        set
        {
            value?.IncrementRefCount();
            amendment = value;
        }
    }

    public override void StateReset()
    {
        Amendment?.DecrementRefCount();
        Amendment = null;
        base.StateReset();
    }

    public override uint MessageId => (uint)TradingMessageIds.Amend;

    IOrderAmend? IOrderAmendRequest.Amendment
    {
        get => Amendment;
        set => Amendment = value as OrxOrderAmend;
    }
}
