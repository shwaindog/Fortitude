// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public abstract class TransmittableOrder : ReusableObject<ITransmittableOrder>, ITransmittableOrder
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TransmittableOrder));

    protected readonly IMutableOrder WrappedOrder;

    private IOrderPublisher? orderPublisher;
    private IMutableString?  mutableMessage;
    private IMutableString?  mutableTicker;

    protected TransmittableOrder()
    {
        WrappedOrder = null!;
    }

    protected TransmittableOrder(ITransmittableOrder toClone)
    {
        WrappedOrder = toClone.AsOrder.Clone();
    }

    protected TransmittableOrder
    (IMutableOrder toWrap, IOrderId orderId, ushort tickerId, uint accountId, DateTime creationTime, OrderStatus status = OrderStatus.New,
        TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, bool isComplete = false
      , string? tickerName = null, string? message = null, DateTime? lastUpdateTime = null)
        : this(toWrap, orderId, tickerId, new Parties(accountId), creationTime, status, timeInForce, venueSelectionCriteria, submitTime, doneTime
             , venueOrders, executions, isComplete, (MutableString?)tickerName, (MutableString?)message, lastUpdateTime) { }

    protected TransmittableOrder
    (IMutableOrder toWrap, IOrderId orderId, ushort tickerId, IParties parties, DateTime? creationTime = null, OrderStatus status = OrderStatus.New
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, bool isComplete = false
      , IMutableString? tickerName = null, IMutableString? message = null, DateTime? lastUpdateTime = null)
    {
        WrappedOrder = toWrap;
        if (WrappedOrder is Order order)
        {
            order.CopyStrings             = CopyStrings;
            order.OrderStringMemberFields = OrderStringMemberFields;
        }

        WrappedOrder.VenueSelectionCriteria = venueSelectionCriteria;

        WrappedOrder.OrderId      = orderId;
        WrappedOrder.TickerId     = tickerId;
        WrappedOrder.TimeInForce  = timeInForce;
        WrappedOrder.CreationTime = creationTime ?? TimeContext.UtcNow;
        LastUpdateTime            = lastUpdateTime ?? CreationTime;
        WrappedOrder.Parties      = parties;
        WrappedOrder.DoneTime     = doneTime;
        WrappedOrder.Status       = status;
        WrappedOrder.SubmitTime   = submitTime;
        WrappedOrder.VenueOrders  = venueOrders;
        WrappedOrder.Executions   = executions;
        WrappedOrder.IsComplete   = isComplete;

        MutableTicker  = tickerName;
        MutableMessage = message;
    }

    public virtual ProductType ProductType => WrappedOrder.ProductType;

    public abstract void ApplyAmendment(IOrderAmend amendment);
    public abstract bool RequiresAmendment(IOrderAmend amendment);
    public abstract void RegisterExecution(IExecution execution);

    public IMutableOrder AsOrder => WrappedOrder;

    public IOrderId OrderId
    {
        get => WrappedOrder.OrderId;
        set => WrappedOrder.OrderId = value;
    }

    public ushort TickerId
    {
        get => WrappedOrder.TickerId;
        set => WrappedOrder.TickerId = value;
    }

    public IParties Parties
    {
        get => WrappedOrder.Parties;
        set => WrappedOrder.Parties = value;
    }

    public TimeInForce TimeInForce
    {
        get => WrappedOrder.TimeInForce;
        set => WrappedOrder.TimeInForce = value;
    }

    public DateTime CreationTime
    {
        get => WrappedOrder.CreationTime;
        set => WrappedOrder.CreationTime = value;
    }

    public DateTime LastUpdateTime
    {
        get => WrappedOrder.CreationTime;
        set => WrappedOrder.CreationTime = value;
    }

    public DateTime? DoneTime
    {
        get => WrappedOrder.DoneTime;
        set => WrappedOrder.DoneTime = value;
    }

    public IExecutions? Executions
    {
        get => WrappedOrder.Executions;
        set => WrappedOrder.Executions = value;
    }

    public OrderStatus Status
    {
        get => WrappedOrder.Status;
        set => WrappedOrder.Status = value;
    }

    public DateTime? SubmitTime
    {
        get => WrappedOrder.SubmitTime;
        set => WrappedOrder.SubmitTime = value;
    }

    public IVenueOrders? VenueOrders
    {
        get => WrappedOrder.VenueOrders;
        set => WrappedOrder.VenueOrders = value;
    }

    public IVenueCriteria? VenueSelectionCriteria
    {
        get => WrappedOrder.VenueSelectionCriteria;
        set => WrappedOrder.VenueSelectionCriteria = value;
    }

    public bool IsComplete
    {
        get => WrappedOrder.IsComplete;
        set => WrappedOrder.IsComplete = value;
    }

    public bool IsError => WrappedOrder.IsError;

    public IOrderPublisher? OrderPublisher
    {
        get => orderPublisher;
        set
        {
            if (value == orderPublisher) return;
            if (value != null) value.IncrementRefCount();
            orderPublisher = value;
        }
    }

    public string? Ticker
    {
        get => MutableTicker?.ToString();
        set => MutableTicker = MutableTicker.TransferOrCreate(value);
    }

    public IMutableString? MutableTicker
    {
        get => mutableTicker;
        set => mutableTicker = mutableTicker.TransferOrReplace(value);
    }

    public string? Message
    {
        get => MutableMessage?.ToString();
        set => MutableMessage = MutableMessage.TransferOrCreate(value);
    }

    public IMutableString? MutableMessage
    {
        get => mutableMessage;
        set => mutableMessage = mutableMessage.TransferOrReplace(value);
    }

    public abstract ITransmittableOrder AsTransmittableOrder { get; }

    public abstract OrxOrder AsOrxOrder { get; }

    public override void StateReset()
    {
        MutableTicker?.DecrementRefCount();
        MutableTicker = null;
        MutableMessage?.DecrementRefCount();
        MutableMessage = null;

        OrderPublisher = null;
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IOrder ICloneable<IOrder>.Clone() => Clone();

    IMutableOrder ICloneable<IMutableOrder>.Clone() => Clone();

    IMutableOrder IMutableOrder.            Clone() => Clone();

    public abstract override ITransmittableOrder Clone();

    IReusableObject<IOrder> ITransferState<IReusableObject<IOrder>>.CopyFrom
        (IReusableObject<IOrder> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IOrder)source, copyMergeFlags);

    IOrder ITransferState<IOrder>.CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);


    public TransmittableOrder CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ITransmittableOrder transmittableOrder)
        {
            var sourceWrappedOrder = transmittableOrder.AsOrder;
            WrappedOrder.CopyFrom(sourceWrappedOrder, copyMergeFlags);
            MutableTicker  = transmittableOrder.MutableTicker;
            MutableMessage = transmittableOrder.MutableMessage;
            OrderPublisher = transmittableOrder.OrderPublisher;
        }
        else
        {
            WrappedOrder.CopyFrom(source, copyMergeFlags);
        }

        return this;
    }

    public bool AreEquivalent(IOrder? other, bool exactTypes = false)
    {
        if (other is not ITransmittableOrder transmittableOrder) return false;

        var wrappedSame        = WrappedOrder.AreEquivalent(transmittableOrder.AsOrder, exactTypes);
        var mutableTickerSame  = Equals(MutableTicker, transmittableOrder.MutableTicker);
        var mutableMessageSame = Equals(MutableMessage, transmittableOrder.MutableMessage);
        var orderPublisherSame = Equals(OrderPublisher, transmittableOrder.OrderPublisher);

        var allAreSame = wrappedSame && mutableTickerSame && mutableMessageSame && orderPublisherSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrder, true);

    public override int GetHashCode() => OrderId.GetHashCode();

    protected void CopyStrings(IMutableOrder destination, IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ITransmittableOrder transmittable)
        {
            MutableTicker  = transmittable.MutableTicker;
            MutableMessage = transmittable.MutableMessage;
        }
        else
        {
            Order.DefaultCopyStrings(WrappedOrder, source, copyMergeFlags);
        }
    }

    protected StringBuilder OrderStringMemberFields(IOrder order, StringBuilder? sb)
    {
        sb ??= new StringBuilder();
        if (MutableTicker != null) sb.Append(", ").Append(nameof(Ticker)).Append(": ").Append(MutableTicker);
        if (MutableMessage != null) sb.Append(", ").Append(nameof(Message)).Append(": ").Append(MutableMessage);
        if (OrderPublisher != null) sb.Append(", ").Append(nameof(OrderPublisher)).Append(": ").Append(OrderPublisher);
        return sb;
    }

    public virtual string OrderToStringMembers => WrappedOrder.OrderToStringMembers;

    public override string ToString() => $"{nameof(TransmittableOrder)}{{{OrderToStringMembers}}}";
}
