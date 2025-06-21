// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Client;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public abstract class Order : ReusableObject<IOrder>, IOrder
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(Order));

    private IOrderPublisher? orderPublisher;
    private OrderStatus      status;

    protected Order()
    {
        status  = OrderStatus.New;
        OrderId = null!;
    }

    protected Order(IOrderId orderId, ushort tickerId, uint accountId, TimeInForce timeInForce, DateTime creationTime)
    {
        OrderId        = orderId;
        TickerId       = tickerId;
        TimeInForce    = timeInForce;
        CreationTime   = creationTime;
        Parties        = new Parties(accountId);
        OrderPublisher = null;
        VenueOrders    = new VenueOrders();
    }

    protected Order(IOrderId orderId, ushort tickerId, IParties parties, TimeInForce timeInForce, DateTime creationTime)
    {
        OrderId        = orderId;
        TickerId       = tickerId;
        TimeInForce    = timeInForce;
        CreationTime   = creationTime;
        Parties        = new Parties(parties);
        OrderPublisher = null;
        VenueOrders    = new VenueOrders();
    }

    protected Order(IOrder toClone)
    {
        OrderId      = toClone.OrderId.Clone();
        TickerId     = toClone.TickerId;
        Ticker       = toClone.Ticker?.Clone();
        TimeInForce  = toClone.TimeInForce;
        CreationTime = toClone.CreationTime;
        SubmitTime   = toClone.SubmitTime;
        DoneTime     = toClone.DoneTime;
        Parties      = toClone.Parties?.Clone();
        Status       = toClone.Status;
        Message      = toClone.Message;
        IsComplete   = toClone.IsComplete;
        IsError      = toClone.IsError;

        OrderPublisher = toClone.OrderPublisher;

        VenueSelectionCriteria = toClone.VenueSelectionCriteria?.Clone();

        VenueOrders = toClone.VenueOrders?.Clone();
        Executions  = toClone.Executions?.Clone();
    }

    protected Order
    (IOrderId orderId, ushort tickerId, uint accountId, DateTime creationTime, OrderStatus status = OrderStatus.PendingNew, 
        TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, IMutableString? tickerName = null, IMutableString? message = null
      , bool isError = false, bool isComplete = false)
    {
        OrderId        = orderId;
        TickerId       = tickerId;
        TimeInForce    = timeInForce;
        CreationTime   = creationTime;
        Parties        = new Parties(accountId);
        DoneTime       = doneTime;
        Status         = status;
        Ticker         = tickerName;
        SubmitTime     = submitTime;
        VenueOrders    = venueOrders;
        Executions     = executions;
        IsError        = isError;
        Message        = message;
        IsComplete     = isComplete;

        VenueSelectionCriteria = venueSelectionCriteria;
    }

    protected Order
    (IOrderId orderId, ushort tickerId, IParties parties, DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, IMutableString? tickerName = null, IMutableString? message = null
      , bool isError = false, bool isComplete = false)
    {
        OrderId        = orderId;
        TickerId       = tickerId;
        TimeInForce    = timeInForce;
        CreationTime   = creationTime ?? TimeContext.UtcNow;
        Parties        = parties;
        DoneTime       = doneTime;
        Status         = status;
        Ticker         = tickerName;
        SubmitTime     = submitTime;
        VenueOrders    = venueOrders;
        Executions     = executions;
        IsError        = isError;
        Message        = message;
        IsComplete     = isComplete;

        VenueSelectionCriteria = venueSelectionCriteria;
    }

    public decimal PendingExecutedSize  { get; set; }
    public bool    HasPendingExecutions => PendingExecutedSize > 0;

    public IOrderId  OrderId      { get; set; }
    public ushort    TickerId     { get; set; }
    public DateTime  CreationTime { get; set; }
    public DateTime?  SubmitTime   { get; set; }
    public DateTime?  DoneTime     { get; set; }
    public IParties? Parties      { get; set; }
    public bool      IsComplete   { get; set; }
    public bool      IsError      { get; set; }

    public IMutableString? Ticker      { get; set; }
    public TimeInForce     TimeInForce { get; set; }

    public abstract ProductType ProductType { get; }

    public abstract void ApplyAmendment(IOrderAmend amendment);
    public abstract bool RequiresAmendment(IOrderAmend amendment);

    public abstract void RegisterExecution(IExecution execution);

    public OrderStatus Status
    {
        get => status;
        set
        {
            if (value == OrderStatus.PendingNew && status != OrderStatus.PendingNew)
                SubmitTime = TimeContext.LocalTimeNow;

            else if (value == OrderStatus.Dead) DoneTime = TimeContext.LocalTimeNow;
            status = value;
        }
    }

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

    public IVenueCriteria? VenueSelectionCriteria { get; set; }
    public IVenueOrders?   VenueOrders { get; set; }
    public IExecutions?    Executions  { get; set; }
    public IMutableString? Message     { get; set; }

    public abstract IOrder AsDomainOrder();
    public abstract OrxOrder AsOrxOrder();

    public override void StateReset()
    {
        PendingExecutedSize = 0;
        OrderId.DecrementRefCount();
        TickerId = 0;
        Ticker?.DecrementRefCount();
        Ticker = null;
        Parties?.DecrementRefCount();
        Parties        = null;
        OrderPublisher = null;
        VenueSelectionCriteria?.DecrementRefCount();
        VenueSelectionCriteria = null;
        VenueOrders?.DecrementRefCount();
        VenueOrders = null;
        Executions?.DecrementRefCount();
        Executions = null;
        Message?.Clear();
        IsError = false;
        IsComplete = false;

        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    public override IOrder CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId        = source.OrderId;
        TickerId       = source.TickerId;
        Ticker         = source.Ticker;
        TimeInForce    = source.TimeInForce;
        CreationTime   = source.CreationTime;
        SubmitTime     = source.SubmitTime;
        DoneTime       = source.DoneTime;
        Parties        = source.Parties;
        Status         = source.Status;
        OrderPublisher = source.OrderPublisher;
        VenueOrders    = source.VenueOrders;
        Executions     = source.Executions;
        Message        = source.Message;
        IsError        = source.IsError;
        IsComplete     = source.IsComplete;
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

    public override int GetHashCode() => OrderId.GetHashCode();

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

    public override string ToString() => $"{nameof(Order)}{{{OrderToStringMembers}}}";
}
