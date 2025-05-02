// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.Orders.Client;

public class OrderSubmitRequest : TradingMessage, IOrderSubmitRequest
{
    public OrderSubmitRequest() { }

    public OrderSubmitRequest(IOrderSubmitRequest toClone) : base(toClone)
    {
        OrderDetails        = toClone.OrderDetails?.Clone();
        OriginalAttemptTime = toClone.OriginalAttemptTime;
        CurrentAttemptTime  = toClone.CurrentAttemptTime;
        AttemptNumber       = toClone.AttemptNumber;
        Tag                 = toClone.Tag;
    }

    public OrderSubmitRequest
    (IOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
        DateTime originalAttemptTime, string tag)
        : this(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, (MutableString)tag) { }

    public OrderSubmitRequest
    (IOrder orderDetails,
        int attemptNumber, DateTime currentAttemptTime, DateTime originalAttemptTime, IMutableString tag)
    {
        OrderDetails        = orderDetails;
        OriginalAttemptTime = originalAttemptTime;
        CurrentAttemptTime  = currentAttemptTime;
        AttemptNumber       = attemptNumber;
        Tag                 = tag;
    }

    public override uint MessageId => (uint)TradingMessageIds.SubmitRequest;

    public IOrder?  OrderDetails        { get; set; }
    public DateTime OriginalAttemptTime { get; set; }
    public DateTime CurrentAttemptTime  { get; set; }
    public int      AttemptNumber       { get; set; }

    public IMutableString? Tag { get; set; }

    public override void StateReset()
    {
        OrderDetails?.DecrementRefCount();
        OrderDetails        = null;
        OriginalAttemptTime = DateTimeConstants.UnixEpoch;
        CurrentAttemptTime  = DateTimeConstants.UnixEpoch;
        AttemptNumber       = 0;
        Tag?.DecrementRefCount();
        Tag = null;
        base.StateReset();
    }

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IOrderSubmitRequest orderSubmitRequest)
        {
            OrderDetails        = orderSubmitRequest.OrderDetails.SyncOrRecycle(OrderDetails as Order);
            OriginalAttemptTime = orderSubmitRequest.OriginalAttemptTime;
            CurrentAttemptTime  = orderSubmitRequest.CurrentAttemptTime;
            AttemptNumber       = orderSubmitRequest.AttemptNumber;
            Tag                 = orderSubmitRequest.Tag.SyncOrRecycle(Tag as MutableString);
        }

        return this;
    }

    public override IOrderSubmitRequest Clone() =>
        (IOrderSubmitRequest?)Recycler?.Borrow<OrderSubmitRequest>().CopyFrom(this) ?? new OrderSubmitRequest(this);
}
