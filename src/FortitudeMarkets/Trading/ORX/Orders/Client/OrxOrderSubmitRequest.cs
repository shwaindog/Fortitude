// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Config;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.ORX.Orders.SpotOrders;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders.Client;

public class OrxOrderSubmitRequest : OrxTradingMessage, IOrderSubmitRequest
{
    public OrxOrderSubmitRequest() { }

    public OrxOrderSubmitRequest(IOrderSubmitRequest toClone)
    {
        OrderDetails        = toClone.OrderDetails?.AsOrxOrder();
        AttemptNumber       = toClone.AttemptNumber;
        CurrentAttemptTime  = toClone.CurrentAttemptTime;
        OriginalAttemptTime = toClone.OriginalAttemptTime;
        Tag                 = toClone.Tag != null ? new MutableString(toClone.Tag) : null;
    }

    public OrxOrderSubmitRequest
    (OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
        DateTime originalAttemptTime, string? tag)
        : this(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, (MutableString)tag) { }

    public OrxOrderSubmitRequest
    (OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
        DateTime originalAttemptTime, MutableString tag)
    {
        OrderDetails        = orderDetails;
        AttemptNumber       = attemptNumber;
        CurrentAttemptTime  = currentAttemptTime;
        OriginalAttemptTime = originalAttemptTime;
        Tag                 = tag;
    }

    [OrxMandatoryField(10, new[]
    {
        (ushort)ProductType.Spot
        /*            (ushort)ProductType.Forward,
        (ushort)ProductType.Swap,
        (ushort)ProductType.Future,
        (ushort)ProductType.MultiLegForward,*/
    }, new[]
    {
        typeof(OrxSpotOrder)
        /*            typeof(OrxSpotOrder), //todo OrxForwardOrder
        typeof(OrxSpotOrder), //todo OrxSwapOrder
        typeof(OrxSpotOrder), //todo OrxFutureOrder
        typeof(OrxSpotOrder)  //todo OrxMultiLegForwardOrder*/
    })] public OrxOrder? OrderDetails { get; set; }

    [OrxOptionalField(14)] public MutableString? Tag { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.SubmitRequest;

    IOrder? IOrderSubmitRequest.OrderDetails
    {
        get => OrderDetails;
        set => OrderDetails = value as OrxOrder;
    }

    [OrxMandatoryField(11)] public int AttemptNumber { get; set; }

    [OrxMandatoryField(12)] public DateTime CurrentAttemptTime { get; set; }

    [OrxOptionalField(13)] public DateTime OriginalAttemptTime { get; set; }

    IMutableString? IOrderSubmitRequest.Tag
    {
        get => Tag;
        set => Tag = value as MutableString;
    }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IOrderSubmitRequest orderSubmitRequest)
        {
            if (OrderDetails == null)
            {
                OrderDetails = orderSubmitRequest.OrderDetails?.AsOrxOrder();
            } else if (orderSubmitRequest.OrderDetails != null)
            {
                OrderDetails.CopyFrom(orderSubmitRequest.OrderDetails, copyMergeFlags);
            }
            Tag                 = orderSubmitRequest.Tag.SyncOrRecycle(Tag);
            AttemptNumber       = orderSubmitRequest.AttemptNumber;
            CurrentAttemptTime  = orderSubmitRequest.CurrentAttemptTime;
            OriginalAttemptTime = orderSubmitRequest.OriginalAttemptTime;
        }

        return this;
    }

    public override void StateReset()
    {
        OrderDetails?.DecrementRefCount();
        OrderDetails = null;
        Tag?.DecrementRefCount();
        Tag                 = null;
        AttemptNumber       = 0;
        CurrentAttemptTime  = DateTimeConstants.UnixEpoch;
        OriginalAttemptTime = DateTimeConstants.UnixEpoch;
        base.StateReset();
    }

    public override IOrderSubmitRequest Clone() =>
        (IOrderSubmitRequest?)Recycler?.Borrow<OrderSubmitRequest>().CopyFrom(this) ?? new OrderSubmitRequest(this);

    protected bool Equals(OrxOrderSubmitRequest other)
    {
        var orderDetailsSame        = Equals(OrderDetails, other.OrderDetails);
        var attemptNumberSame       = AttemptNumber == other.AttemptNumber;
        var currentAttemptTimeSame  = Equals(CurrentAttemptTime, other.CurrentAttemptTime);
        var originalAttemptTimeSame = Equals(OriginalAttemptTime, other.OriginalAttemptTime);
        var tagSame                 = Equals(Tag, other.Tag);
        return orderDetailsSame && attemptNumberSame && currentAttemptTimeSame && originalAttemptTimeSame
            && tagSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxOrderSubmitRequest)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderDetails != null ? OrderDetails.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ AttemptNumber;
            hashCode = (hashCode * 397) ^ CurrentAttemptTime.GetHashCode();
            hashCode = (hashCode * 397) ^ OriginalAttemptTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (Tag != null ? Tag.GetHashCode() : 0);
            return hashCode;
        }
    }
}
