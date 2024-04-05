#region

using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client;

public class OrxAmendRequest : OrxOrderSubmitRequest, IOrderAmendRequest
{
    public OrxAmendRequest() { }

    public OrxAmendRequest(IOrderAmendRequest toClone) : base(toClone) =>
        Amendment = toClone.Amendment != null ? new OrxOrderAmend(toClone.Amendment) : null;

    public OrxAmendRequest(OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
        DateTime originalAttemptTime, string? tag, OrxOrderAmend amendment)
        : base(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, tag) =>
        Amendment = amendment;

    [OrxMandatoryField(20)] public OrxOrderAmend? Amendment { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.Amend;

    IOrderAmend? IOrderAmendRequest.Amendment
    {
        get => Amendment;
        set => Amendment = value as OrxOrderAmend;
    }
}
