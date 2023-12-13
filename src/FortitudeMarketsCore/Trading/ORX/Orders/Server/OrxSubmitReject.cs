#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders.Server;

public sealed class OrxSubmitReject : OrxTradingMessage
{
    public OrxSubmitReject() { }

    private OrxSubmitReject(OrxSubmitReject toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.SubmitRejectResponse;

    [OrxMandatoryField(10)] public MutableString? Reason { get; set; }

    [OrxMandatoryField(11)] public OrxOrderId? OrderId { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxSubmitReject submitReject)
        {
            Reason = submitReject.Reason.SyncOrRecycle(Reason);
            OrderId = submitReject.OrderId.SyncOrRecycle(OrderId);
        }

        return this;
    }

    public override void StateReset()
    {
        Reason?.DecrementRefCount();
        Reason = null;
        OrderId?.DecrementRefCount();
        OrderId = null;
        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxSubmitReject>().CopyFrom(this) ?? new OrxSubmitReject(this);
}
