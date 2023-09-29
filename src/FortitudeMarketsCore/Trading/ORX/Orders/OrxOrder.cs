using System;
using FortitudeCommon.DataStructures.Memory;
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
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Orders
{
    public class OrxOrder : IOrder
    {
        private OrxProductOrder product;

        public OrxOrder()
        {
        }

        public OrxOrder(IOrder toClone)
        {
            OrderId = new OrxOrderId(toClone.OrderId);
            TimeInForce = toClone.TimeInForce;
            CreationTime = toClone.CreationTime;
            Status = toClone.Status;
            Product =  toClone.Product.CreateNewOrxProductOrder();
            SubmitTime = toClone.SubmitTime;
            DoneTime = toClone.DoneTime;
            Parties = toClone.Parties != null ? new OrxParties(toClone.Parties) : null;
            OrderPublisher = toClone.OrderPublisher;
            VenueSelectionCriteria = toClone.VenueSelectionCriteria != null 
                ? new OrxVenueCriteria(toClone.VenueSelectionCriteria)
                : null;
            VenueOrders = toClone.VenueOrders != null ? new OrxVenueOrders(toClone.VenueOrders) : null;
            Executions = toClone.Executions != null ? new OrxExecutions(toClone.Executions) : null;
            Message = toClone.Message != null ? new MutableString(toClone.Message) : null ;
        }

        public OrxOrder(OrxOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status, OrxProductOrder product,
            DateTime submitTime, OrxParties parties, DateTime doneTime, 
            OrxVenueCriteria venueSelectionCriteria, OrxVenueOrders venueOrders, OrxExecutions executions, 
            string message, IOrderPublisher orderPublisher)
        : this(orderId, timeInForce, creationTime, status, product, submitTime, parties, doneTime, venueSelectionCriteria,
            venueOrders, executions, (MutableString)message, orderPublisher)
        { }

        public OrxOrder(OrxOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status, OrxProductOrder product,
            DateTime submitTime, OrxParties parties, DateTime doneTime, 
            OrxVenueCriteria venueSelectionCriteria, OrxVenueOrders venueOrders, OrxExecutions executions, 
            MutableString message, IOrderPublisher orderPublisher)
        {
            OrderId = orderId;
            TimeInForce = timeInForce;
            CreationTime = creationTime;
            Status = status;
            Product = product;
            SubmitTime = submitTime;
            DoneTime = doneTime;
            Parties = parties;
            OrderPublisher = orderPublisher;
            VenueSelectionCriteria = venueSelectionCriteria;
            VenueOrders = venueOrders;
            Executions = executions;
            Message = message;
        }

        [OrxMandatoryField(0)]
        public OrxOrderId OrderId { get; set; }

        IOrderId IOrder.OrderId
        {
            get => OrderId;
            set => OrderId = (OrxOrderId) value;
        }

        [OrxMandatoryField(1)]
        public TimeInForce TimeInForce { get; set; }

        [OrxMandatoryField(2)]
        public DateTime CreationTime { get; set; }

        [OrxMandatoryField(3)]
        public OrderStatus Status { get; set; }

        [OrxMandatoryField(4, new[]
        {
            (ushort) ProductType.Spot,
/*            (ushort)ProductType.Forward,
            (ushort)ProductType.Swap,
            (ushort)ProductType.Future,
            (ushort)ProductType.MultiLegForward,*/
        }, new[]
        {
            typeof(OrxSpotOrder),
/*            typeof(OrxSpotOrder), //todo OrxForwardOrder
            typeof(OrxSpotOrder), //todo OrxSwapOrder
            typeof(OrxSpotOrder), //todo OrxFutureOrder
            typeof(OrxSpotOrder)  //todo OrxMultiLegForwardOrder*/
        })]
        public OrxProductOrder Product
        {
            get => product;
            set
            {
                product = value;
                if(product != null) product.Order = this;
            }
        }

        IProductOrder IOrder.Product
        {
            get => Product;
            set => Product = (OrxProductOrder)value;
        }

        [OrxMandatoryField(5)]
        public DateTime SubmitTime { get; set; }

        [OrxOptionalField(6)]
        public DateTime DoneTime { get; set; }

        [OrxOptionalField(7)]
        public OrxParties Parties { get; set; }

        IParties IOrder.Parties
        {
            get => Parties;
            set => Parties = (OrxParties)value;
        }

        public IOrderPublisher OrderPublisher { get; set; }

        [OrxOptionalField(8)]
        public OrxVenueCriteria VenueSelectionCriteria { get; set; }

        IVenueCriteria IOrder.VenueSelectionCriteria
        {
            get => VenueSelectionCriteria;
            set => VenueSelectionCriteria = (OrxVenueCriteria)value;
        }

        [OrxOptionalField(9)]
        public OrxVenueOrders VenueOrders { get; set; }

        IVenueOrders IOrder.VenueOrders
        {
            get => VenueOrders;
            set => VenueOrders = (OrxVenueOrders)value;
        }

        [OrxOptionalField(10)]
        public OrxExecutions Executions { get; set; }

        IExecutions IOrder.Executions
        {
            get => Executions;
            set => Executions = (OrxExecutions)value;
        }

        [OrxOptionalField(11)]
        public MutableString Message { get; set; }

        IMutableString IOrder.Message
        {
            get => Message;
            set => Message = (MutableString)value;
        }

        public void CopyFrom(IOrder order, IRecycler recycler)
        {
            if (order.OrderId != null)
            {
                var orxOrderId = recycler.Borrow<OrxOrderId>();
                orxOrderId.CopyFrom(order.OrderId, recycler);
                OrderId = orxOrderId;
            }

            TimeInForce = order.TimeInForce;
            CreationTime = order.CreationTime;
            Status = order.Status;
            if (order.Product != null)
            {
                var orxProductOrder = ((OrxProductOrder) order.Product).GetPooledInstance(recycler);
                orxProductOrder.CopyFrom(order.Product, recycler);
                Product = orxProductOrder;
            }
            SubmitTime = order.SubmitTime;
            DoneTime = order.DoneTime;
            if (order.Parties != null)
            {
                var orxParties = recycler.Borrow<OrxParties>();
                orxParties.CopyFrom(order.Parties, recycler);
                Parties = orxParties;
            }
            OrderPublisher = order.OrderPublisher;
            if (order.VenueSelectionCriteria != null)
            {
                var orxVenueCriteria = recycler.Borrow<OrxVenueCriteria>();
                orxVenueCriteria.CopyFrom(order.VenueSelectionCriteria, recycler);
                VenueSelectionCriteria = orxVenueCriteria;
            }
            if (order.VenueOrders != null)
            {
                var orxVenueOrders = recycler.Borrow<OrxVenueOrders>();
                orxVenueOrders.CopyFrom(order.VenueOrders, recycler);
                VenueOrders = orxVenueOrders;
            }
            if (order.Executions != null)
            {
                var orxExecutions = recycler.Borrow<OrxExecutions>();
                orxExecutions.CopyFrom(order.Executions, recycler);
                Executions = orxExecutions;
            }

            Message = order.Message != null
                ? recycler.Borrow<MutableString>().Clear().Append(order.Message) as MutableString
                : null;
        }

        public IOrder Clone()
        {
            return new OrxOrder(this);
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
                   && doneTimeSame && partiesSame && venueCriteriaSame && venueOrdersSame && executionsSame &&
                   messageSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxOrder) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (OrderId != null ? OrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ TimeInForce.GetHashCode();
                hashCode = (hashCode * 397) ^ CreationTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Status;
                hashCode = (hashCode * 397) ^ (Product != null ? Product.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SubmitTime.GetHashCode();
                hashCode = (hashCode * 397) ^ DoneTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Parties != null ? Parties.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OrderPublisher != null ? OrderPublisher.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (VenueSelectionCriteria != null ? VenueSelectionCriteria.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (VenueOrders != null ? VenueOrders.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Executions != null ? Executions.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
