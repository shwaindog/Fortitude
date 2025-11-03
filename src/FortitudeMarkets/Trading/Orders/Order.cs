// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.Counterparties;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Products;
using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public class Order : ReusableObject<IOrder>, IMutableOrder
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(Order));

    private OrderStatus status;

    private string? ticker;
    private string? message;

    public Order()
    {
        status  = OrderStatus.New;
        OrderId = null!;
        Parties = null!;
    }

    protected Order(IMutableOrder toClone)
    {
        OrderId        = toClone.OrderId.Clone();
        TickerId       = toClone.TickerId;
        TimeInForce    = toClone.TimeInForce;
        CreationTime   = toClone.CreationTime;
        LastUpdateTime = toClone.LastUpdateTime;
        SubmitTime     = toClone.SubmitTime;
        DoneTime       = toClone.DoneTime;
        Parties        = toClone.Parties.Clone();
        Status         = toClone.Status;
        IsComplete     = toClone.IsComplete;
        IsError        = toClone.IsError;

        VenueSelectionCriteria = toClone.VenueSelectionCriteria?.Clone();

        VenueOrders = toClone.VenueOrders?.Clone();
        Executions  = toClone.Executions?.Clone();

        CopyStrings(this, toClone, CopyMergeFlags.Default);
    }

    protected Order
    (IOrderId orderId, ushort tickerId, uint accountId, DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew,
        TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, bool isError = false, bool isComplete = false
      , string? tickerName = null, string? message = null, DateTime? lastUpdateTime = null)
    {
        VenueSelectionCriteria = venueSelectionCriteria;

        OrderId      = orderId;
        TickerId     = tickerId;
        TimeInForce  = timeInForce;
        CreationTime = creationTime ?? TimeContext.UtcNow;
        LastUpdateTime = lastUpdateTime ?? CreationTime;
        Parties        = new Parties(accountId);
        DoneTime       = doneTime;
        Status         = status;
        SubmitTime     = submitTime;
        VenueOrders    = venueOrders;
        Executions     = executions;
        IsError        = isError;
        IsComplete     = isComplete;

        Ticker  = tickerName;
        Message = message;
    }

    protected Order
    (IOrderId orderId, ushort tickerId, IParties parties, DateTime? creationTime = null, OrderStatus status = OrderStatus.PendingNew
      , TimeInForce timeInForce = TimeInForce.ImmediateOrCancel
      , IVenueCriteria? venueSelectionCriteria = null, DateTime? submitTime = null, DateTime? doneTime = null
      , IVenueOrders? venueOrders = null, IExecutions? executions = null, bool isError = false, bool isComplete = false
      , string? tickerName = null, string? message = null, DateTime? lastUpdateTime = null)
    {
        VenueSelectionCriteria = venueSelectionCriteria;

        OrderId        = orderId;
        TickerId       = tickerId;
        TimeInForce    = timeInForce;
        CreationTime   = creationTime ?? TimeContext.UtcNow;
        LastUpdateTime = lastUpdateTime ?? CreationTime;
        Parties        = parties;
        DoneTime       = doneTime;
        Status         = status;
        SubmitTime     = submitTime;
        VenueOrders    = venueOrders;
        Executions     = executions;
        IsError        = isError;
        IsComplete     = isComplete;

        Ticker  = tickerName;
        Message = message;
    }

    public decimal PendingExecutedSize  { get; set; }
    public bool    HasPendingExecutions => PendingExecutedSize > 0;

    public IOrderId  OrderId        { get; set; }
    public ushort    TickerId       { get; set; }
    public DateTime  CreationTime   { get; set; }
    public DateTime  LastUpdateTime { get; set; }
    public DateTime? SubmitTime     { get; set; }
    public DateTime? DoneTime       { get; set; }
    public IParties  Parties        { get; set; }

    public bool      IsComplete     { get; set; }
    public bool      IsError        { get; set; }

    public virtual string? Ticker
    {
        get => ticker;
        set => ticker = value;
    }

    public TimeInForce TimeInForce { get; set; }

    public virtual ProductType ProductType { get; init; } = ProductType.Unknown;

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


    public IVenueCriteria? VenueSelectionCriteria { get; set; }

    public IVenueOrders? VenueOrders { get; set; }

    public IExecutions? Executions { get; set; }

    public virtual string? Message
    {
        get => message;
        set => message = value;
    }

    public override void StateReset()
    {
        PendingExecutedSize = 0;
        OrderId.DecrementRefCount();
        TickerId = 0;
        Parties.DecrementRefCount();
        Parties        = null!;
        CreationTime   = default;
        LastUpdateTime = default;
        SubmitTime     = null;
        DoneTime       = null;

        VenueSelectionCriteria?.DecrementRefCount();
        VenueSelectionCriteria = null;
        VenueOrders?.DecrementRefCount();
        VenueOrders = null;
        Executions?.DecrementRefCount();
        Executions = null;
        IsError    = false;
        IsComplete = false;

        status  = OrderStatus.New;
        ticker  = null;
        message = null;

        CopyStrings             = DefaultCopyStrings;
        OrderStringMemberFields = DefaultOrderStringMemberFields;

        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IMutableOrder ICloneable<IMutableOrder>.Clone() => Clone();

    IMutableOrder IMutableOrder.Clone() => Clone();

    public override Order Clone() => Recycler?.Borrow<Order>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new Order(this);

    public override Order CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId        = source.OrderId;
        TickerId       = source.TickerId;
        TimeInForce    = source.TimeInForce;
        CreationTime   = source.CreationTime;
        LastUpdateTime = source.LastUpdateTime;
        SubmitTime     = source.SubmitTime;
        DoneTime       = source.DoneTime;
        Status         = source.Status;
        VenueOrders    = source.VenueOrders;
        Executions     = source.Executions;
        IsError        = source.IsError;
        IsComplete     = source.IsComplete;

        Parties ??= new Parties();
        Parties.CopyFrom(source.Parties, copyMergeFlags);

        CopyStrings(this, source, copyMergeFlags);

        return this;
    }

    internal Action<IMutableOrder, IOrder, CopyMergeFlags> CopyStrings { get; set; } = DefaultCopyStrings;

    internal static void DefaultCopyStrings(IMutableOrder destination, IOrder source, CopyMergeFlags copyMergeFlags)
    {
        destination.Ticker  = source.Ticker;
        destination.Message = source.Message;
    }

    public bool AreEquivalent(IOrder? source, bool exactTypes = false)
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
        var messageSame     = Equals(Message, source.Message);
        var tickerSame      = Equals(Ticker, source.Ticker);
        var isErrorSame     = IsError == source.IsError;
        var isCompleteSame  = IsComplete == source.IsComplete;

        return orderIdsSame && tickerIdSame && timeInForceSame && creationTimeSame && updateTimeSame && statusSame && submitTimeSame
            && doneTimeSame && partiesSame && venueCriteriaSame && venueOrdersSame && executionsSame && messageSame
            && tickerSame && isErrorSame && isCompleteSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrder, true);

    public override int GetHashCode() => OrderId.GetHashCode();

    internal Func<IOrder, StringBuilder?, StringBuilder> OrderStringMemberFields { get; set; } = DefaultOrderStringMemberFields;

    internal static StringBuilder DefaultOrderStringMemberFields(IOrder order, StringBuilder? sb)
    {
        sb ??= new StringBuilder();
        if (order.Ticker != null) sb.Append(", ").Append(nameof(order.Ticker)).Append(": ").Append(order.Ticker);
        if (order.Message != null) sb.Append(", ").Append(nameof(order.Message)).Append(": ").Append(order.Message);
        return sb;
    }

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
            OrderStringMemberFields(this, sb);
            if (TimeInForce != TimeInForce.None) sb.Append(", ").Append(nameof(TimeInForce)).Append(": ").Append(TimeInForce);
            if (Status != OrderStatus.Unknown) sb.Append(", ").Append(nameof(Status)).Append(": ").Append(Status);
            if (SubmitTime.IsNotNullOrUnixEpochOrDefault()) sb.Append(", ").Append(nameof(SubmitTime)).Append(": ").Append(SubmitTime);
            if (DoneTime.IsNotNullOrUnixEpochOrDefault()) sb.Append(", ").Append(nameof(DoneTime)).Append(": ").Append(DoneTime);
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
