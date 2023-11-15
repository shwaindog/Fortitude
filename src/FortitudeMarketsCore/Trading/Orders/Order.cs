#region

using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.Counterparties;
using FortitudeMarketsCore.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders;

public sealed class Order : IOrder
{
    private IProductOrder? product;
    private int refCount = 0;
    private OrderStatus status;

    public Order(IOrderId orderId, TimeInForce timeInForce, DateTime creationTime, IProductOrder product)
    {
        OrderId = orderId;
        TimeInForce = timeInForce;
        CreationTime = creationTime;
        this.product = product;
        Parties = new Parties(null, null);
        OrderPublisher = null;
        VenueOrders = new VenueOrders();
    }

    public Order(IOrder toClone)
    {
        OrderId = toClone.OrderId.Clone();
        TimeInForce = toClone.TimeInForce;
        CreationTime = toClone.CreationTime;
        SubmitTime = toClone.SubmitTime;
        DoneTime = toClone.DoneTime;
        Parties = toClone.Parties?.Clone();
        Status = toClone.Status;
        OrderPublisher = toClone.OrderPublisher;
        product = toClone.Product?.Clone();
        if (product != null) product.Order = this;

        VenueSelectionCriteria = toClone.VenueSelectionCriteria?.Clone();
        VenueOrders = toClone.VenueOrders?.Clone();
        Executions = toClone.Executions?.Clone();
    }

    public Order(IOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status,
        IProductOrder product, DateTime submitTime, IParties parties, DateTime doneTime,
        IVenueCriteria venueSelectionCriteria, IVenueOrders? venueOrders, IExecutions? executions,
        string message, IOrderPublisher? orderPublisher)
        : this(orderId, timeInForce, creationTime, status, product, submitTime, parties, doneTime,
            venueSelectionCriteria, venueOrders, executions, orderPublisher) { }

    public Order(IOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status,
        IProductOrder product, DateTime submitTime, IParties parties, DateTime doneTime,
        IVenueCriteria venueSelectionCriteria, IVenueOrders? venueOrders, IExecutions? executions,
        IOrderPublisher? orderPublisher)
    {
        OrderId = orderId;
        TimeInForce = timeInForce;
        CreationTime = creationTime;
        Parties = parties;
        DoneTime = doneTime;
        Status = status;
        OrderPublisher = orderPublisher;
        this.product = product;
        SubmitTime = submitTime;
        VenueSelectionCriteria = venueSelectionCriteria;
        VenueOrders = venueOrders;
        Executions = executions;
    }

    public decimal PendingExecutedSize { get; set; }
    public bool HasPendingExecutions => PendingExecutedSize > 0;
    public IOrderId OrderId { get; set; }

    public TimeInForce TimeInForce { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime SubmitTime { get; set; }
    public DateTime DoneTime { get; set; }
    public IParties? Parties { get; set; }

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

    public IOrderPublisher? OrderPublisher { get; set; }

    public IProductOrder? Product
    {
        get => product;
        set
        {
            product = value;
            if (product != null) product.Order = this;
        }
    }

    public IVenueCriteria? VenueSelectionCriteria { get; set; }
    public IVenueOrders? VenueOrders { get; set; }
    public IExecutions? Executions { get; set; }
    public IMutableString Message { get; set; } = new MutableString();

    public void CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId = source.OrderId;
        TimeInForce = source.TimeInForce;
        CreationTime = source.CreationTime;
        SubmitTime = source.SubmitTime;
        DoneTime = source.DoneTime;
        Parties = source.Parties;
        Status = source.Status;
        OrderPublisher = source.OrderPublisher;
        Product = source.Product;
        VenueOrders = source.VenueOrders;
        Executions = source.Executions;
        Message = source.Message;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IOrder)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }

    public IOrder Clone() => new Order(this);

    public override string ToString() =>
        new StringBuilder(256)
            .Append(OrderId).Append(',')
            .Append(TimeInForce).Append(',')
            .Append(CreationTime.ToString("O")).Append(',')
            .Append(SubmitTime.ToString("O")).Append(',')
            .Append(DoneTime.ToString("O")).Append(',')
            .Append(Parties).Append(',')
            .Append(Status).Append(',')
            .Append(Product).Append(',')
            .Append(VenueSelectionCriteria).Append(',')
            .Append(VenueOrders).Append(',')
            .Append(Executions)
            .ToString();

    public override bool Equals(object? obj) => obj is IOrder order && OrderId.Equals(order.OrderId);

    public override int GetHashCode() => OrderId.GetHashCode();
}
