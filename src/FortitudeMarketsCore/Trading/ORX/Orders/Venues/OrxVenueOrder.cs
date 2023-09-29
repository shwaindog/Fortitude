using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues
{
    public class OrxVenueOrder : IVenueOrder
    {
        public OrxVenueOrder()
        {
        }

        public OrxVenueOrder(IVenueOrder toClone)
        {
            VenueOrderId = new OrxVenueOrderId(toClone.VenueOrderId);
            OrderId = new OrxOrderId(toClone.OrderId);
            Status = toClone.Status;
            Venue = new OrxVenue(toClone.Venue);
            Ticker = toClone.Ticker != null ? new MutableString(toClone.Ticker) : null;
            Price = toClone.Price;
            Quantity = toClone.Quantity;
        }

        public OrxVenueOrder(OrxVenueOrderId venueOrderId, OrxOrderId orderId, OrderStatus status, OrxVenue venue, 
            string ticker, decimal price, decimal quantity)
        :this(venueOrderId, orderId, status, venue, (MutableString)ticker, price, quantity)
        { }

        public OrxVenueOrder(OrxVenueOrderId venueOrderId, OrxOrderId orderId, OrderStatus status, OrxVenue venue, 
            MutableString ticker, decimal price, decimal quantity)
        {
            VenueOrderId = venueOrderId;
            OrderId = orderId;
            Status = status;
            Venue = venue;
            Ticker = ticker;
            Price = price;
            Quantity = quantity;
        }

        [OrxMandatoryField(0)]
        public OrxVenueOrderId VenueOrderId { get; set; }

        IVenueOrderId IVenueOrder.VenueOrderId
        {
            get => VenueOrderId;
            set => VenueOrderId = (OrxVenueOrderId)value;
        }

        [OrxMandatoryField(1)]
        public OrxOrderId OrderId { get; set; }

        IOrderId IVenueOrder.OrderId
        {
            get => OrderId;
            set => OrderId = (OrxOrderId)value;
        }

        [OrxMandatoryField(2)]
        public OrderStatus Status { get; set; }

        [OrxMandatoryField(3)]
        public OrxVenue Venue { get; set; }

        IVenue IVenueOrder.Venue
        {
            get => Venue;
            set => Venue = (OrxVenue)value;
        }

        [OrxMandatoryField(4)]
        public DateTime SubmitTime { get; set; } = DateTimeConstants.UnixEpoch;

        [OrxMandatoryField(5)]
        public DateTime VenueAckTime { get; set; } = DateTimeConstants.UnixEpoch;

        [OrxMandatoryField(6)]
        public MutableString Ticker { get; set; }

        IMutableString IVenueOrder.Ticker
        {
            get => Ticker;
            set => Ticker = (MutableString)value;
        }

        [OrxMandatoryField(7)]
        public decimal Price { get; set; }

        [OrxMandatoryField(8)]
        public decimal Quantity { get; set; }

        public void CopyFrom(IVenueOrder venueOrder, IRecycler recycler)
        {
            if (venueOrder.VenueOrderId != null)
            {
                var orxVenueOrderId = recycler.Borrow<OrxVenueOrderId>();
                orxVenueOrderId.CopyFrom(venueOrder.VenueOrderId, recycler);
                VenueOrderId = orxVenueOrderId;
            }
            if (venueOrder.OrderId != null)
            {
                var orxOrderId = recycler.Borrow<OrxOrderId>();
                orxOrderId.CopyFrom(venueOrder.OrderId, recycler);
                OrderId = orxOrderId;
            }
            Status = venueOrder.Status;
            if (venueOrder.Venue != null)
            {
                var orxVenue = recycler.Borrow<OrxVenue>();
                orxVenue.CopyFrom(venueOrder.Venue, recycler);
                Venue = orxVenue;
            }

            Ticker = venueOrder.Ticker != null
                ? recycler.Borrow<MutableString>().Clear().Append(venueOrder.Ticker) as MutableString
                : null;
            Price = venueOrder.Price;
            Quantity = venueOrder.Quantity;
        }

        public IVenueOrder Clone()
        {
            return new OrxVenueOrder(this);
        }

        protected bool Equals(OrxVenueOrder other)
        {
            var venueIdSame = Equals(VenueOrderId, other.VenueOrderId);
            var orderIdSame = Equals(OrderId, other.OrderId);
            var statusSame = Status == other.Status;
            var venueSame = Equals(Venue, other.Venue);
            var submitTimeSame = Equals(SubmitTime, other.SubmitTime);
            var venueAckTimeSame = Equals(VenueAckTime, other.VenueAckTime);
            var tickerSame = Equals(Ticker, other.Ticker);
            var priceSame = Price == other.Price;
            var quantitySame = Quantity == other.Quantity;

            return venueIdSame && orderIdSame && statusSame && venueSame && submitTimeSame && 
                   venueAckTimeSame && tickerSame && priceSame && quantitySame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxVenueOrder) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (VenueOrderId != null ? VenueOrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ OrderId?.GetHashCode() ?? 0;
                hashCode = (hashCode * 397) ^ (int) Status;
                hashCode = (hashCode * 397) ^ (Venue != null ? Venue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ SubmitTime.GetHashCode();
                hashCode = (hashCode * 397) ^ VenueAckTime.GetHashCode();
                hashCode = (hashCode * 397) ^ (Ticker != null ? Ticker.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ Quantity.GetHashCode();
                return hashCode;
            }
        }
    }
}