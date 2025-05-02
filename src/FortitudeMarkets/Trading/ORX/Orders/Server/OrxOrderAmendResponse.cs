// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Server;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Server;

public class OrxOrderAmendResponse : OrxOrderUpdate, IOrderAmendResponse
{
    public OrxOrderAmendResponse() { }

    public OrxOrderAmendResponse(IOrderAmendResponse toClone) : base(toClone)
    {
        AmendType  = toClone.AmendType;
        OldOrderId = toClone.OldOrderId != null ? new OrxOrderId(toClone.OldOrderId) : null;
    }

    public OrxOrderAmendResponse
    (OrxOrder order, OrderUpdateEventType reason, DateTime adapterUpdateTime,
        AmendType amendType, OrxOrderId? oldOrderId) : base(order, reason, adapterUpdateTime)
    {
        AmendType  = amendType;
        OldOrderId = oldOrderId;
    }

    [OrxOptionalField(21)] public OrxOrderId? OldOrderId { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.Amend;

    [OrxMandatoryField(20)] public AmendType AmendType { get; set; }

    IOrderId? IOrderAmendResponse.OldOrderId
    {
        get => OldOrderId;
        set => OldOrderId = value as OrxOrderId;
    }

    public override void StateReset()
    {
        OldOrderId?.DecrementRefCount();
        OldOrderId = null;
        AmendType  = default;
        base.StateReset();
    }

    public override IOrderAmendResponse Clone() =>
        (IOrderAmendResponse?)Recycler?.Borrow<OrxOrderAmendResponse>().CopyFrom(this) ??
        new OrxOrderAmendResponse(this);

    public override IVersionedMessage CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IOrderAmendResponse amendResponse)
        {
            AmendType = amendResponse.AmendType;
            if (amendResponse.OldOrderId != null)
            {
                var newOrderId = Recycler!.Borrow<OrxOrderId>();
                newOrderId.CopyFrom(amendResponse.OldOrderId, copyMergeFlags);
                OldOrderId = newOrderId;
            }
        }

        return this;
    }
}
