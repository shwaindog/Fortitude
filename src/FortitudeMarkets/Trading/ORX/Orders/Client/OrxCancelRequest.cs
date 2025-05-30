﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Client;

public sealed class OrxCancelRequest : OrxTradingMessage
{
    public OrxCancelRequest(OrxOrderId adapterOrderId) => OrderId = adapterOrderId;

    public OrxCancelRequest() { }

    private OrxCancelRequest(OrxCancelRequest toClone)
    {
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)TradingMessageIds.CancelRequest;

    [OrxMandatoryField(10)] public OrxOrderId? OrderId { get; set; }

    public override IVersionedMessage CopyFrom
        (IVersionedMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is OrxCancelRequest cancelRequest) OrderId = cancelRequest.OrderId.SyncOrRecycle(OrderId);

        return this;
    }

    public override void StateReset()
    {
        OrderId?.DecrementRefCount();
        OrderId = null;
        base.StateReset();
    }

    public override IAuthenticatedMessage Clone() =>
        (IAuthenticatedMessage?)Recycler?.Borrow<OrxCancelRequest>().CopyFrom(this) ?? new OrxCancelRequest(this);
}
