using System;
using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Orders
{
    public sealed class Order : IOrder
    {
        public Order(IOrder toClone) 
        {
            OrderId = toClone.OrderId.Clone();
            TimeInForce = toClone.TimeInForce;
            CreationTime = toClone.CreationTime;
            SubmitTime = toClone.SubmitTime;
            DoneTime = toClone.DoneTime;
            Parties = toClone.Parties.Clone();
            Status = toClone.Status;
            OrderPublisher = toClone.OrderPublisher;
            Product = toClone.Product.Clone();
            Product.Order = this;
            VenueSelectionCriteria = toClone.VenueSelectionCriteria?.Clone();
            VenueOrders = toClone.VenueOrders?.Clone();
            Executions = toClone.Executions?.Clone();
            Message = toClone.Message;
        }

        public Order(IOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status, 
            IProductOrder product, DateTime submitTime, IParties parties, DateTime doneTime,
            IVenueCriteria venueSelectionCriteria, IVenueOrders venueOrders, IExecutions executions,
            string message, IOrderPublisher orderPublisher)
        : this(orderId, timeInForce, creationTime, status, product, submitTime, parties, doneTime, 
            venueSelectionCriteria, venueOrders, executions, (MutableString)message, orderPublisher)
        { }

        public Order(IOrderId orderId, TimeInForce timeInForce, DateTime creationTime, OrderStatus status, 
            IProductOrder product, DateTime submitTime, IParties parties, DateTime doneTime,
            IVenueCriteria venueSelectionCriteria, IVenueOrders venueOrders, IExecutions executions,
            IMutableString message, IOrderPublisher orderPublisher) 
        {
            OrderId = orderId;
            TimeInForce = timeInForce;
            CreationTime = creationTime;
            Parties = parties;
            DoneTime = doneTime;
            Status = status;
            OrderPublisher = orderPublisher;
            Product = product;
            SubmitTime = submitTime;
            VenueSelectionCriteria = venueSelectionCriteria;
            VenueOrders = venueOrders;
            Executions = executions;
            Message = message;
        }
        public IOrderId OrderId { get; set; }

        public TimeInForce TimeInForce { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime SubmitTime { get; set; }
        public DateTime DoneTime { get; set; }
        public IParties Parties { get; set; }
        private OrderStatus status;
        private IProductOrder product;

        public OrderStatus Status
        {
            get => status;
            set
            {
                if (value == OrderStatus.PendingNew && status != OrderStatus.PendingNew)
                {
                    SubmitTime = TimeContext.LocalTimeNow;
                }
                else if (value == OrderStatus.Dead)
                {
                    DoneTime = TimeContext.LocalTimeNow;
                }
                status = value;
            }
        }
        public IOrderPublisher OrderPublisher { get; set; }

        public IProductOrder Product
        {
            get => product;
            set
            {
                product = value;
                if(product != null) product.Order = this;
            }
        }

        public IVenueCriteria VenueSelectionCriteria { get; set; }
        public IVenueOrders VenueOrders { get; set; }
        public IExecutions Executions { get; set; }
        public IMutableString Message { get; set; }

        public override string ToString()
        {
            return new StringBuilder(256)
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
                .Append(Executions).Append(',')
                .Append(Message)
                .ToString();
        }
        
        public decimal PendingExecutedSize { get; set; }
        public bool HasPendingExecutions => PendingExecutedSize > 0;
        
        public IOrder Clone()
        {
            return new Order(this);
        }

        public override bool Equals(object obj)
        {
            return obj is IOrder order && OrderId.Equals(order.OrderId);
        }

        public override int GetHashCode()
        {
            return OrderId.GetHashCode();
        }
    }
}
