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

public sealed class Order : ReusableObject<IOrder>, IOrder
{
    private IProductOrder? product;
    private OrderStatus status;

    public Order()
    {
        product = null;
        status = OrderStatus.New;
        OrderId = null!;
    }

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

    public override void Reset()
    {
        base.Reset();
        PendingExecutedSize = 0;
        OrderId.DecrementRefCount();
        Parties?.DecrementRefCount();
        Parties = null;
        OrderPublisher = null;
        Product?.DecrementRefCount();
        Product = null;
        VenueSelectionCriteria?.DecrementRefCount();
        VenueSelectionCriteria = null;
        VenueOrders?.DecrementRefCount();
        VenueOrders = null;
        Executions?.DecrementRefCount();
        Executions = null;
        Message?.Clear();
    }

    object ICloneable.Clone() => Clone();

    public override IOrder Clone() => Recycler?.Borrow<Order>().CopyFrom(this) ?? new Order(this);

    public override IOrder CopyFrom(IOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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
        return this;
    }

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
