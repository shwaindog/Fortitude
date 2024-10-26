#region

using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Server;

public sealed class OrxCancelReject : OrxTradingMessage
{
    public OrxCancelReject() { }

    private OrxCancelReject(OrxCancelReject toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.CancelRejectResponse;

    [OrxMandatoryField(10)] public string? Reason { get; set; }

    [OrxOptionalField(11)] public OrxOrderId? OrderId { get; set; }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxCancelReject>().CopyFrom(this) ?? new OrxCancelReject(this);
}
