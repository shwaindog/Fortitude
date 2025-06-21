// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.CounterParties;
using FortitudeMarkets.Trading.ORX.Executions;
using FortitudeMarkets.Trading.ORX.Orders.SpotOrders;
using FortitudeMarkets.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders;

[OrxMapToDerivedClasses([
    (ushort)ProductType.Spot
    /*            (ushort)ProductType.Forward,
    (ushort)ProductType.Swap,
    (ushort)ProductType.Future,
    (ushort)ProductType.MultiLegForward,*/
], [
    typeof(OrxSpotOrder)
    /*            typeof(OrxSpotOrder), //todo OrxForwardOrder
    typeof(OrxSpotOrder), //todo OrxSwapOrder
    typeof(OrxSpotOrder), //todo OrxFutureOrder
    typeof(OrxSpotOrder)  //todo OrxMultiLegForwardOrder*/
])]
public abstract class OrxOrder : ReusableObject<IOrder>, IOrder, ITransferState<OrxOrder>, ICloneable<OrxOrder>
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(OrxOrder));

    private IOrderPublisher? orderPublisher;

    protected OrxOrder()
    {
        OrderId = new OrxOrderId();
    }

    protected OrxOrder(IOrder toClone)
    {
        OrderId      = new OrxOrderId(toClone.OrderId);
        TickerId     = toClone.TickerId;
        Ticker       = toClone.Ticker != null ? new MutableString(toClone.Ticker) : null;
        TimeInForce  = toClone.TimeInForce;
        CreationTime = toClone.CreationTime;
        Status       = toClone.Status;
        SubmitTime   = toClone.SubmitTime;
        DoneTime     = toClone.DoneTime;
        Parties      = toClone.Parties != null ? new OrxParties(toClone.Parties) : null;
        Message      = toClone.Message != null ? new MutableString(toClone.Message) : null;
        IsComplete   = toClone.IsComplete;
        IsError      = toClone.IsError;

        OrderPublisher = toClone.OrderPublisher;

        VenueSelectionCriteria = toClone.VenueSelectionCriteria != null ? new OrxVenueCriteria(toClone.VenueSelectionCriteria) : null;

        VenueOrders = toClone.VenueOrders != null ? new OrxVenueOrders(toClone.VenueOrders) : null;
        Executions  = toClone.Executions != null ? new OrxExecutions(toClone.Executions) : null;
    }

    protected OrxOrder
    (OrxOrderId orderId, ushort tickerId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status
      , DateTime submitTime, OrxParties parties, DateTime doneTime
      , OrxVenueCriteria venueSelectionCriteria, OrxVenueOrders venueOrders, OrxExecutions executions
      , IOrderPublisher orderPublisher, MutableString? message = null, bool isError = false
      , MutableString? tickerName = null, bool isComplete = false)
    {
        OrderId      = orderId;
        TickerId     = tickerId;
        Ticker       = tickerName;
        TimeInForce  = timeInForce;
        CreationTime = creationTime;
        Status       = status;
        SubmitTime   = submitTime;
        DoneTime     = doneTime;
        Parties      = parties;
        VenueOrders  = venueOrders;
        Executions   = executions;
        IsError      = isError;
        Message      = message;
        IsComplete   = isComplete;

        OrderPublisher = orderPublisher;

        VenueSelectionCriteria = venueSelectionCriteria;
    }

    [OrxMandatoryField(0)] public OrxOrderId OrderId { get; set; }

    [OrxMandatoryField(1)] public ushort TickerId { get; set; }

    IOrderId IOrder.OrderId
    {
        get => OrderId;
        set => OrderId = (OrxOrderId)value;
    }

    [OrxMandatoryField(2)] public TimeInForce TimeInForce { get; set; }

    [OrxMandatoryField(3)] public DateTime CreationTime { get; set; }

    [OrxMandatoryField(4)] public OrderStatus Status { get; set; }

    [OrxOptionalField(6)] public DateTime? SubmitTime { get; set; }

    [OrxOptionalField(7)] public DateTime? DoneTime { get; set; }

    [OrxOptionalField(8)] public OrxParties? Parties { get; set; }

    [OrxOptionalField(9)] public OrxVenueCriteria? VenueSelectionCriteria { get; set; }

    [OrxOptionalField(10)] public OrxVenueOrders? VenueOrders { get; set; }

    [OrxOptionalField(11)] public OrxExecutions? Executions { get; set; }

    [OrxOptionalField(12)] public MutableString? Message { get; set; } = new();

    IMutableString? IOrder.Ticker
    {
        get => Ticker;
        set => Ticker = value as MutableString;
    }

    [OrxOptionalField(13)] public MutableString? Ticker { get; set; }

    [OrxOptionalField(14)] public bool IsError { get; set; }

    [OrxOptionalField(15)] public bool IsComplete { get; set; }

    public abstract ProductType ProductType { get; }

    public bool AutoRecycledByProducer { get; set; }

    IParties? IOrder.Parties
    {
        get => Parties;
        set => Parties = value as OrxParties;
    }

    public IOrderPublisher? OrderPublisher
    {
        get => orderPublisher;
        set
        {
            if (orderPublisher == value) return;
            if (value != null) value.IncrementRefCount();
            orderPublisher = value;
        }
    }

    IVenueCriteria? IOrder.VenueSelectionCriteria
    {
        get => VenueSelectionCriteria;
        set => VenueSelectionCriteria = value as OrxVenueCriteria;
    }

    IVenueOrders? IOrder.VenueOrders
    {
        get => VenueOrders;
        set => VenueOrders = value as OrxVenueOrders;
    }

    IExecutions? IOrder.Executions
    {
        get => Executions;
        set => Executions = value as OrxExecutions;
    }

    IMutableString? IOrder.Message
    {
        get => Message;
        set => Message = value as MutableString ?? Recycler?.Borrow<MutableString>() ?? new MutableString();
    }

    public abstract void RegisterExecution(IExecution execution);

    public abstract void ApplyAmendment(IOrderAmend amendment);

    public abstract bool RequiresAmendment(IOrderAmend amendment);

    public override void StateReset()
    {
        TimeInForce  = TimeInForce.None;
        CreationTime = DateTimeConstants.UnixEpoch;
        Status       = OrderStatus.Unknown;
        SubmitTime   = DateTimeConstants.UnixEpoch;
        DoneTime     = DateTimeConstants.UnixEpoch;
        Parties?.DecrementRefCount();
        Parties = null;
        OrderPublisher?.DecrementRefCount();
        OrderPublisher = null;
        VenueSelectionCriteria?.DecrementRefCount();
        VenueSelectionCriteria = null;
        VenueOrders?.DecrementRefCount();
        VenueOrders = null;
        Executions?.DecrementRefCount();
        Executions = null;
        IsError    = false;
        IsComplete = false;
        base.StateReset();
    }

    public abstract IOrder AsDomainOrder();
    public abstract OrxOrder AsOrxOrder();

    public override OrxOrder Clone() => throw new NotImplementedException("Derived classes must override this");

    OrxOrder ITransferState<OrxOrder>.CopyFrom(OrxOrder source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override OrxOrder CopyFrom(IOrder order, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId      = order.OrderId.SyncOrRecycle(OrderId)!;
        TimeInForce  = order.TimeInForce;
        CreationTime = order.CreationTime;
        Status       = order.Status;
        SubmitTime   = order.SubmitTime;
        DoneTime     = order.DoneTime;
        Parties      = order.Parties.SyncOrRecycle(Parties);

        OrderPublisher = order.OrderPublisher;

        VenueSelectionCriteria = order.VenueSelectionCriteria.SyncOrRecycle(VenueSelectionCriteria);

        VenueOrders = order.VenueOrders.SyncOrRecycle(VenueOrders);
        Executions  = order.Executions.SyncOrRecycle(Executions);
        Message     = order.Message.SyncOrRecycle(Message);

        return this;
    }

    public bool AreEquivalent(IOrder? source, bool exactTypes = false)
    {
        if (source == null) return false;
        var orderIdsSame     = OrderId.AreEquivalent(source.OrderId, exactTypes);
        var tickerIdSame     = TickerId == source.TickerId;
        var timeInForceSame  = TimeInForce == source.TimeInForce;
        var creationTimeSame = CreationTime == source.CreationTime;
        var statusSame       = Status == source.Status;
        var submitTimeSame   = SubmitTime == source.SubmitTime;
        var doneTimeSame     = DoneTime == source.DoneTime;
        var partiesSame      = Equals(Parties, source.Parties);

        var venueCriteriaSame = Equals(VenueSelectionCriteria, source.VenueSelectionCriteria);

        var venueOrdersSame = Equals(VenueOrders, source.VenueOrders);
        var executionsSame  = Equals(Executions, source.Executions);
        var messageSame     = Equals(Message, source.Message);
        var tickerSame      = Equals(Ticker, source.Ticker);
        var isErrorSame     = IsError == source.IsError;
        var isCompleteSame  = IsComplete == source.IsComplete;

        return orderIdsSame && tickerIdSame && timeInForceSame && creationTimeSame && statusSame && submitTimeSame
            && doneTimeSame && partiesSame && venueCriteriaSame && venueOrdersSame && executionsSame && messageSame
            && tickerSame && isErrorSame && isCompleteSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrder, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId.GetHashCode();
            hashCode = (hashCode * 397) ^ TickerId;
            hashCode = (hashCode * 397) ^ (Ticker?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ TimeInForce.GetHashCode();
            hashCode = (hashCode * 397) ^ CreationTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Status;
            hashCode = (hashCode * 397) ^ SubmitTime.GetHashCode();
            hashCode = (hashCode * 397) ^ DoneTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (Parties != null ? Parties.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (OrderPublisher != null ? OrderPublisher.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (VenueSelectionCriteria != null ? VenueSelectionCriteria.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (VenueOrders != null ? VenueOrders.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Executions != null ? Executions.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Message?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ IsError.GetHashCode();
            hashCode = (hashCode * 397) ^ IsComplete.GetHashCode();
            return hashCode;
        }
    }
    
    protected string OrderToStringMembers 
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(nameof(InstanceNum)).Append(": ").Append(InstanceNum);
            sb.Append(", ").Append(nameof(OrderId)).Append(": ").Append(OrderId);
            sb.Append(", ").Append(nameof(TickerId)).Append(": ").Append(TickerId);
            sb.Append(", ").Append(nameof(ProductType)).Append(": ").Append(ProductType);
            if (Ticker != null) sb.Append(", ").Append(nameof(Ticker)).Append(": ").Append(Ticker);
            if (TimeInForce != TimeInForce.None) sb.Append(", ").Append(nameof(TimeInForce)).Append(": ").Append(TimeInForce);
            if (CreationTime.IsNotUnixEpochOrDefault()) sb.Append(", ").Append(nameof(CreationTime)).Append(": ").Append(CreationTime);
            if (Status != OrderStatus.Unknown) sb.Append(", ").Append(nameof(Status)).Append(": ").Append(Status);
            if (SubmitTime.IsNotNullOrUnixEpochOrDefault()) sb.Append(", ").Append(nameof(SubmitTime)).Append(": ").Append(SubmitTime);
            if (DoneTime.IsNotNullOrUnixEpochOrDefault()) sb.Append(", ").Append(nameof(DoneTime)).Append(": ").Append(DoneTime);
            if (Parties != null) sb.Append(", ").Append(nameof(Parties)).Append(", ").Append(": ").Append(Parties);
            if (OrderPublisher != null) sb.Append(", ").Append(nameof(OrderPublisher)).Append(": ").Append(OrderPublisher);
            if (VenueSelectionCriteria != null) sb.Append(", ").Append(nameof(VenueSelectionCriteria)).Append(": ").Append(VenueSelectionCriteria);
            if (VenueOrders != null) sb.Append(", ").Append(nameof(VenueOrders)).Append(": ").Append(VenueOrders);
            if (Executions != null) sb.Append(", ").Append(nameof(Executions)).Append(": ").Append(Executions);
            sb.Append(", ").Append(nameof(IsError)).Append(": ").Append(IsError);
            sb.Append(", ").Append(nameof(IsComplete)).Append(": ").Append(IsComplete);

            return sb.ToString();
        }
    }

    public override string ToString() => $"{nameof(OrxOrder)}{{{OrderToStringMembers}}}";
}
