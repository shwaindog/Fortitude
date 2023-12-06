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

public class OrxAmendReject : OrxTradingMessage
{
    public OrxAmendReject() { }

    private OrxAmendReject(OrxAmendReject toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.AmendReject;

    [OrxMandatoryField(10)] public MutableString? Reason { get; set; }

    [OrxMandatoryField(11)] public OrxOrderId? OrderId { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is OrxAmendReject amendReject)
        {
            Reason = amendReject.Reason.SyncOrRecycle(Reason);
            OrderId = amendReject.OrderId.SyncOrRecycle(OrderId);
        }

        return this;
    }

    public override void Reset()
    {
        Reason?.DecrementRefCount();
        Reason = null;
        OrderId?.DecrementRefCount();
        OrderId = null;
        base.Reset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxAmendReject>().CopyFrom(this) ?? new OrxAmendReject(this);
}
