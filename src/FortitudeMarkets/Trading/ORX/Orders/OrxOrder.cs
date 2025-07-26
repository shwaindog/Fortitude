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
using FortitudeCommon.Types.Mutable.Strings;
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
public abstract class OrxOrder : ReusableObject<ITransmittableOrder>, ITransmittableOrder, ITransferState<OrxOrder>, ICloneable<OrxOrder>
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(OrxOrder));

    private IOrderPublisher? orderPublisher;

    protected OrxOrder()
    {
        OrderId = null!;
        Parties = null!;
    }

    protected OrxOrder(ITransmittableOrder toClone)
    {
        OrderId        = new OrxOrderId(toClone.OrderId);
        TickerId       = toClone.TickerId;
        Parties        = new OrxParties(toClone.Parties);
        TimeInForce    = toClone.TimeInForce;
        CreationTime   = toClone.CreationTime;
        LastUpdateTime = toClone.LastUpdateTime;
        Status         = toClone.Status;
        SubmitTime     = toClone.SubmitTime;
        DoneTime       = toClone.DoneTime;
        IsComplete     = toClone.IsComplete;
        IsError        = toClone.IsError;

        OrderPublisher = toClone.OrderPublisher;

        VenueSelectionCriteria = toClone.VenueSelectionCriteria != null ? new OrxVenueCriteria(toClone.VenueSelectionCriteria) : null;

        VenueOrders = toClone.VenueOrders != null ? new OrxVenueOrders(toClone.VenueOrders) : null;
        Executions  = toClone.Executions != null ? new OrxExecutions(toClone.Executions) : null;

        MutableTicker  = (MutableString?)toClone.MutableTicker;
        MutableMessage = (MutableString?)toClone.MutableMessage;
    }

    protected OrxOrder
    (OrxOrderId orderId, ushort tickerId, OrxParties parties, DateTime? creationTime = null, OrderStatus status = OrderStatus.New
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , OrxVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , OrxVenueOrders? venueOrders = null, OrxExecutions? executions = null, bool isComplete = false
      , MutableString? tickerName = null, MutableString? message = null, DateTime? lastUpdateTime = null)
    {
        OrderId        = orderId;
        TickerId       = tickerId;
        MutableTicker  = tickerName;
        TimeInForce    = timeInForce;
        CreationTime   = creationTime ?? TimeContext.UtcNow;
        LastUpdateTime = lastUpdateTime ?? CreationTime;
        Status         = status;
        SubmitTime     = submitTime;
        DoneTime       = doneTime;
        Parties        = parties;
        VenueOrders    = venueOrders;
        Executions     = executions;
        MutableMessage = message;
        IsComplete     = isComplete;

        OrderPublisher = orderPublisher;

        VenueSelectionCriteria = venueSelectionCriteria;
    }

    public abstract ProductType ProductType { get; }

    public abstract IMutableOrder AsOrder { get; }

    [OrxMandatoryField(0)] public OrxOrderId OrderId { get; set; }

    IOrderId IOrder.OrderId => OrderId;

    IOrderId IMutableOrder.OrderId
    {
        get => OrderId;
        set => OrderId = (OrxOrderId)value;
    }

    [OrxMandatoryField(1)] public ushort TickerId { get; set; }

    [OrxMandatoryField(2)] public OrxParties Parties { get; set; }

    IParties IOrder.Parties => Parties;

    IParties IMutableOrder.Parties
    {
        get => Parties;
        set => Parties = (OrxParties)value;
    }

    [OrxMandatoryField(3)] public TimeInForce TimeInForce { get; set; }

    [OrxMandatoryField(4)] public DateTime CreationTime   { get; set; }

    [OrxMandatoryField(5)] public DateTime LastUpdateTime { get; set; }

    [OrxMandatoryField(6)] public OrderStatus Status { get; set; }

    [OrxOptionalField(6)] public DateTime? SubmitTime { get; set; }

    [OrxOptionalField(7)] public DateTime? DoneTime { get; set; }

    [OrxOptionalField(8)] public OrxVenueCriteria? VenueSelectionCriteria { get; set; }

    IVenueCriteria? IOrder.VenueSelectionCriteria => VenueSelectionCriteria;

    IVenueCriteria? IMutableOrder.VenueSelectionCriteria
    {
        get => VenueSelectionCriteria;
        set => VenueSelectionCriteria = value as OrxVenueCriteria;
    }

    [OrxOptionalField(9)] public OrxVenueOrders? VenueOrders { get; set; }

    IVenueOrders? IOrder.VenueOrders => VenueOrders;

    IVenueOrders? IMutableOrder.VenueOrders
    {
        get => VenueOrders;
        set => VenueOrders = value as OrxVenueOrders;
    }

    [OrxOptionalField(10)] public OrxExecutions? Executions { get; set; }

    IExecutions? IOrder.Executions => Executions;  

    IExecutions? IMutableOrder.Executions
    {
        get => Executions;
        set => Executions = value as OrxExecutions;
    }

    [OrxOptionalField(11)] public bool IsError { get; set; }

    [OrxOptionalField(12)] public bool IsComplete { get; set; }

    [OrxOptionalField(13)] public MutableString? MutableMessage { get; set; }

    public string? Message => MutableMessage?.ToString();

    string? IMutableOrder.Message
    {
        get => MutableMessage?.ToString();
        set => MutableMessage = MutableMessage.TransferOrCreate(value);
    }

    IMutableString? ITransmittableOrder.MutableMessage
    {
        get => MutableMessage;
        set => MutableMessage = MutableMessage.TransferOrReplace(value);
    }

    [OrxOptionalField(14)] public MutableString? MutableTicker { get; set; }

    public string? Ticker => MutableTicker?.ToString();

    string? IMutableOrder.Ticker
    {
        get => MutableTicker?.ToString();
        set => MutableTicker = value;
    }

    IMutableString? ITransmittableOrder.MutableTicker
    {
        get => MutableTicker;
        set => MutableTicker = MutableTicker.TransferOrReplace(value);
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

    public abstract void RegisterExecution(IExecution execution);

    public abstract void ApplyAmendment(IOrderAmend amendment);

    public abstract bool RequiresAmendment(IOrderAmend amendment);

    public override void StateReset()
    {
        TimeInForce  = TimeInForce.None;
        CreationTime = default;
        LastUpdateTime = default;
        Status       = OrderStatus.Unknown;
        SubmitTime   = null;
        DoneTime     = null;
        Parties.DecrementRefCount();
        Parties = null!;
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

    public abstract ITransmittableOrder AsTransmittableOrder { get; }

    public abstract OrxOrder AsOrxOrder { get; }

    IMutableOrder ICloneable<IMutableOrder>.Clone() => Clone();

    IOrder ICloneable<IOrder>.  Clone() => Clone();

    IMutableOrder IMutableOrder.Clone() => Clone();


    public abstract override OrxOrder Clone();

    IReusableObject<IOrder> ITransferState<IReusableObject<IOrder>>.CopyFrom
        (IReusableObject<IOrder> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IOrder)source, copyMergeFlags);


    OrxOrder ITransferState<OrxOrder>.CopyFrom(OrxOrder source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    IOrder ITransferState<IOrder>.CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override OrxOrder CopyFrom(ITransmittableOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom(source, copyMergeFlags);

    public virtual OrxOrder CopyFrom(IOrder order, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId        = order.OrderId.SyncOrRecycle(OrderId)!;
        TimeInForce    = order.TimeInForce;
        CreationTime   = order.CreationTime;
        LastUpdateTime = order.LastUpdateTime;
        Status         = order.Status;
        SubmitTime     = order.SubmitTime;
        DoneTime       = order.DoneTime;

        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        Parties ??= new OrxParties();
        Parties.CopyFrom(order.Parties, copyMergeFlags);


        VenueSelectionCriteria = order.VenueSelectionCriteria.SyncOrRecycle(VenueSelectionCriteria);

        VenueOrders = order.VenueOrders.SyncOrRecycle(VenueOrders);
        Executions  = order.Executions.SyncOrRecycle(Executions);

        if (order is ITransmittableOrder transmittableOrder)
        {
            MutableMessage = transmittableOrder.MutableMessage.SyncOrRecycle(MutableMessage);
            MutableTicker  = transmittableOrder.MutableMessage.SyncOrRecycle(MutableMessage);
            OrderPublisher = transmittableOrder.OrderPublisher;
        }
        else
        {
            ((IMutableOrder)this).Ticker  = order.Ticker;
            ((IMutableOrder)this).Message = order.Message;

            OrderPublisher = null;
        }

        return this;
    }

    public bool AreEquivalent(IOrder? other, bool exactTypes = false) => AreEquivalent(other as ITransmittableOrder, exactTypes);

    public virtual bool AreEquivalent(ITransmittableOrder? source, bool exactTypes = false)
    {
        if (source == null) return false;
        var orderIdsSame     = OrderId.AreEquivalent(source.OrderId, exactTypes);
        var tickerIdSame     = TickerId == source.TickerId;
        var timeInForceSame  = TimeInForce == source.TimeInForce;
        var creationTimeSame = CreationTime == source.CreationTime;
        var updateTimeSame   = LastUpdateTime == source.LastUpdateTime;
        var statusSame       = Status == source.Status;
        var submitTimeSame   = SubmitTime == source.SubmitTime;
        var doneTimeSame     = DoneTime == source.DoneTime;
        var partiesSame      = Equals(Parties, source.Parties);

        var venueCriteriaSame = Equals(VenueSelectionCriteria, source.VenueSelectionCriteria);

        var venueOrdersSame = Equals(VenueOrders, source.VenueOrders);
        var executionsSame  = Equals(Executions, source.Executions);
        var messageSame     = Equals(MutableMessage, source.MutableMessage);
        var tickerSame      = Equals(MutableTicker, source.MutableTicker);
        var isErrorSame     = IsError == source.IsError;
        var isCompleteSame  = IsComplete == source.IsComplete;

        return orderIdsSame && tickerIdSame && timeInForceSame && creationTimeSame && updateTimeSame && statusSame && submitTimeSame
            && doneTimeSame && partiesSame && venueCriteriaSame && venueOrdersSame && executionsSame && messageSame
            && tickerSame && isErrorSame && isCompleteSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITransmittableOrder, true);

    public override int GetHashCode() => OrderId.GetHashCode();

    public virtual string OrderToStringMembers
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(nameof(InstanceNum)).Append(": ").Append(InstanceNum);
            sb.Append(", ").Append(nameof(OrderId)).Append(": ").Append(OrderId);
            sb.Append(", ").Append(nameof(TickerId)).Append(": ").Append(TickerId);
            sb.Append(", ").Append(nameof(Parties)).Append(", ").Append(": ").Append(Parties);
            sb.Append(", ").Append(nameof(ProductType)).Append(": ").Append(ProductType);
            sb.Append(", ").Append(nameof(CreationTime)).Append(": ").Append(CreationTime);
            sb.Append(", ").Append(nameof(LastUpdateTime)).Append(": ").Append(LastUpdateTime);
            if (MutableTicker != null) sb.Append(", ").Append(nameof(IMutableOrder.Ticker)).Append(": ").Append(MutableTicker);
            if (MutableMessage != null) sb.Append(", ").Append(nameof(IMutableOrder.Message)).Append(": ").Append(MutableMessage);
            if (TimeInForce != TimeInForce.None) sb.Append(", ").Append(nameof(TimeInForce)).Append(": ").Append(TimeInForce);
            if (Status != OrderStatus.Unknown) sb.Append(", ").Append(nameof(Status)).Append(": ").Append(Status);
            if (SubmitTime.IsNotNullOrUnixEpochOrDefault()) sb.Append(", ").Append(nameof(SubmitTime)).Append(": ").Append(SubmitTime);
            if (DoneTime.IsNotNullOrUnixEpochOrDefault()) sb.Append(", ").Append(nameof(DoneTime)).Append(": ").Append(DoneTime);
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
