#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.CounterParties;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders.Products;
using FortitudeMarketsCore.Trading.ORX.Orders.Products.General;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Orders;

public class OrxOrder : ReusableObject<IOrder>, IOrder
{
    private OrxProductOrder? product;

    public OrxOrder()
    {
        OrderId = new OrxOrderId();
        Parties = new OrxParties();
        VenueOrders = new OrxVenueOrders();
        Executions = new OrxExecutions();
    }

    public OrxOrder(IOrder toClone)
    {
        OrderId = new OrxOrderId(toClone.OrderId);
        TimeInForce = toClone.TimeInForce;
        CreationTime = toClone.CreationTime;
        Status = toClone.Status;
        product = toClone.Product?.CreateNewOrxProductOrder();
        SubmitTime = toClone.SubmitTime;
        DoneTime = toClone.DoneTime;
        Parties = toClone.Parties != null ? new OrxParties(toClone.Parties) : null;
        OrderPublisher = toClone.OrderPublisher;
        VenueSelectionCriteria = toClone.VenueSelectionCriteria != null ?
            new OrxVenueCriteria(toClone.VenueSelectionCriteria) :
            null;
        VenueOrders = toClone.VenueOrders != null ? new OrxVenueOrders(toClone.VenueOrders) : null;
        Executions = toClone.Executions != null ? new OrxExecutions(toClone.Executions) : null;
    }

    public OrxOrder(OrxOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status
        , OrxProductOrder product,
        DateTime submitTime, OrxParties parties, DateTime doneTime,
        OrxVenueCriteria venueSelectionCriteria, OrxVenueOrders venueOrders, OrxExecutions executions,
        string message, IOrderPublisher orderPublisher)
        : this(orderId, timeInForce, creationTime, status, product, submitTime, parties, doneTime
            , venueSelectionCriteria,
            venueOrders, executions, (MutableString)message, orderPublisher) { }

    public OrxOrder(OrxOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status
        , OrxProductOrder product,
        DateTime submitTime, OrxParties parties, DateTime doneTime,
        OrxVenueCriteria venueSelectionCriteria, OrxVenueOrders venueOrders, OrxExecutions executions,
        MutableString message, IOrderPublisher orderPublisher)
    {
        OrderId = orderId;
        TimeInForce = timeInForce;
        CreationTime = creationTime;
        Status = status;
        this.product = product;
        SubmitTime = submitTime;
        DoneTime = doneTime;
        Parties = parties;
        OrderPublisher = orderPublisher;
        VenueSelectionCriteria = venueSelectionCriteria;
        VenueOrders = venueOrders;
        Executions = executions;
    }

    [OrxMandatoryField(0)] public OrxOrderId OrderId { get; set; }

    [OrxMandatoryField(4, new[]
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
    })]
    public OrxProductOrder? Product
    {
        get => product;
        set
        {
            product = value;
            if (product != null) product.Order = this;
        }
    }

    [OrxOptionalField(7)] public OrxParties? Parties { get; set; }

    [OrxOptionalField(8)] public OrxVenueCriteria? VenueSelectionCriteria { get; set; }

    [OrxOptionalField(9)] public OrxVenueOrders? VenueOrders { get; set; }

    [OrxOptionalField(10)] public OrxExecutions? Executions { get; set; }

    [OrxOptionalField(11)] public MutableString Message { get; set; } = new();
    public bool AutoRecycledByProducer { get; set; }

    IOrderId IOrder.OrderId
    {
        get => OrderId;
        set => OrderId = (OrxOrderId)value;
    }

    [OrxMandatoryField(1)] public TimeInForce TimeInForce { get; set; }

    [OrxMandatoryField(2)] public DateTime CreationTime { get; set; }

    [OrxMandatoryField(3)] public OrderStatus Status { get; set; }

    IProductOrder? IOrder.Product
    {
        get => Product;
        set => Product = value as OrxProductOrder;
    }

    [OrxMandatoryField(5)] public DateTime SubmitTime { get; set; }

    [OrxOptionalField(6)] public DateTime DoneTime { get; set; }

    IParties? IOrder.Parties
    {
        get => Parties;
        set => Parties = value as OrxParties;
    }

    public IOrderPublisher? OrderPublisher { get; set; }

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

    IMutableString IOrder.Message
    {
        get => Message;
        set => Message = value as MutableString ?? Recycler?.Borrow<MutableString>() ?? new MutableString();
    }

    public override void Reset()
    {
        TimeInForce = TimeInForce.None;
        CreationTime = DateTimeConstants.UnixEpoch;
        Status = OrderStatus.Unknown;
        Product?.DecrementRefCount();
        Product = null;
        SubmitTime = DateTimeConstants.UnixEpoch;
        DoneTime = DateTimeConstants.UnixEpoch;
        Parties?.DecrementRefCount();
        Parties = null;
        OrderPublisher = null;
        VenueSelectionCriteria?.DecrementRefCount();
        VenueSelectionCriteria = null;
        VenueOrders?.DecrementRefCount();
        VenueOrders = null;
        Executions?.DecrementRefCount();
        Executions = null;
        base.Reset();
    }

    public override IOrder Clone() => Recycler?.Borrow<OrxOrder>().CopyFrom(this) ?? new OrxOrder(this);


    public override IOrder CopyFrom(IOrder order, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId = order.OrderId.SyncOrRecycle(OrderId)!;
        TimeInForce = order.TimeInForce;
        CreationTime = order.CreationTime;
        Status = order.Status;
        Product = order.Product.CloneOrRecycle(Product);
        SubmitTime = order.SubmitTime;
        DoneTime = order.DoneTime;
        Parties = order.Parties.SyncOrRecycle(Parties);
        OrderPublisher = order.OrderPublisher;
        VenueSelectionCriteria = order.VenueSelectionCriteria.SyncOrRecycle(VenueSelectionCriteria);
        VenueOrders = order.VenueOrders.SyncOrRecycle(VenueOrders);
        Executions = order.Executions.SyncOrRecycle(Executions);
        Message = order.Message.SyncOrRecycle(Message)!;
        return this;
    }

    protected bool Equals(OrxOrder other)
    {
        var orderIdsSame = Equals(OrderId, other.OrderId);
        var timeInForceSame = TimeInForce == other.TimeInForce;
        var creationTimeSame = Equals(CreationTime, other.CreationTime);
        var statusSame = Status == other.Status;
        var productSame = Equals(Product, other.Product);
        var submitTimeSame = Equals(SubmitTime, other.SubmitTime);
        var doneTimeSame = Equals(DoneTime, other.DoneTime);
        var partiesSame = Equals(Parties, other.Parties);
        var venueCriteriaSame = Equals(VenueSelectionCriteria, other.VenueSelectionCriteria);
        var venueOrdersSame = Equals(VenueOrders, other.VenueOrders);
        var executionsSame = Equals(Executions, other.Executions);
        var messageSame = Equals(Message, other.Message);

        return orderIdsSame && timeInForceSame && creationTimeSame && statusSame && productSame && submitTimeSame
               && doneTimeSame && partiesSame && venueCriteriaSame && venueOrdersSame && executionsSame && messageSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxOrder)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId != null ? OrderId.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ TimeInForce.GetHashCode();
            hashCode = (hashCode * 397) ^ CreationTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Status;
            hashCode = (hashCode * 397) ^ (Product != null ? Product.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ SubmitTime.GetHashCode();
            hashCode = (hashCode * 397) ^ DoneTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (Parties != null ? Parties.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (OrderPublisher != null ? OrderPublisher.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (VenueSelectionCriteria != null ? VenueSelectionCriteria.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (VenueOrders != null ? VenueOrders.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Executions != null ? Executions.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ Message.GetHashCode();
            return hashCode;
        }
    }
}
