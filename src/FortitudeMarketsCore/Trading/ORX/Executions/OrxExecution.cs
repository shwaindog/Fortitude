using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.CounterParties;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Executions
{
    public class OrxExecution : IExecution
    {
        public OrxExecution()
        {
        }

        public OrxExecution(IExecution toClone)
        {
            ExecutionId = new OrxExecutionId(toClone.ExecutionId);
            Venue = new OrxVenue(toClone.Venue);
            VenueOrderId = new OrxVenueOrderId(toClone.VenueOrderId);
            OrderId = new OrxOrderId(toClone.OrderId);
            ExecutionTime = toClone.ExecutionTime;
            Price = toClone.Price;
            Quantity = toClone.Quantity;
            CounterParty = new OrxParty(toClone.CounterParty);
            ValueDate = toClone.ValueDate;
            Type = toClone.Type;
            ExecutionStageType = toClone.ExecutionStageType;
        }


        public OrxExecution(OrxExecutionId executionId, OrxVenue venue, OrxVenueOrderId venueOrderId, 
            OrxOrderId orderId, DateTime executionTime, decimal price, decimal quantity, OrxParty counterParty,
            DateTime valueDate, ExecutionType type, ExecutionStageType executionStageType)
        {
            ExecutionId = executionId;
            Venue = venue;
            VenueOrderId = venueOrderId;
            OrderId = orderId;
            ExecutionTime = executionTime;
            Price = price;
            Quantity = quantity;
            CounterParty = counterParty;
            ValueDate = valueDate;
            Type = type;
            ExecutionStageType = executionStageType;
        }

        [OrxMandatoryField(0)]
        public OrxExecutionId ExecutionId { get; set; }

        IExecutionId IExecution.ExecutionId
        {
            get => ExecutionId;
            set => ExecutionId = (OrxExecutionId)value;
        }

        [OrxMandatoryField(1)]
        public OrxVenue Venue { get; set; }

        IVenue IExecution.Venue
        {
            get => Venue;
            set => Venue = (OrxVenue)value;
        }

        [OrxMandatoryField(2)]
        public OrxVenueOrderId VenueOrderId { get; set; }

        [OrxMandatoryField(3)]
        public OrxOrderId OrderId { get; set; }

        IOrderId IExecution.OrderId
        {
            get => OrderId;
            set => OrderId = (OrxOrderId)value;
        }

        IVenueOrderId IExecution.VenueOrderId
        {
            get => VenueOrderId;
            set => VenueOrderId = (OrxVenueOrderId)value;
        }

        [OrxMandatoryField(4)]
        public DateTime ExecutionTime { get; set; } = DateTimeConstants.UnixEpoch;

        [OrxMandatoryField(5)]
        public decimal Price { get; set; }

        [OrxMandatoryField(6)]
        public decimal Quantity { get; set; }

        [OrxMandatoryField(7)]
        public decimal CumlativeQuantity { get; set; }

        [OrxMandatoryField(8)]
        public decimal CumlativeVwapPrice { get; set; }

        [OrxOptionalField(9)]
        public OrxParty CounterParty { get; set; }

        IParty IExecution.CounterParty
        {
            get => CounterParty;
            set => CounterParty = (OrxParty)value;
        }

        [OrxOptionalField(10)]
        public DateTime ValueDate { get; set; } = DateTimeConstants.UnixEpoch;

        [OrxOptionalField(11)]
        public ExecutionType Type { get; set; }

        [OrxOptionalField(12)]
        public ExecutionStageType ExecutionStageType { get; set; }

        public void CopyFrom(IExecution execution, IRecycler recycler)
        {
            if (execution.ExecutionId != null)
            {
                var orxExecutionId = recycler.Borrow<OrxExecutionId>();
                orxExecutionId.CopyFrom(execution.ExecutionId, recycler);
                ExecutionId = orxExecutionId;
            }
            if (execution.Venue != null)
            {
                var orxVenue = recycler.Borrow<OrxVenue>();
                orxVenue.CopyFrom(execution.Venue, recycler);
                Venue = orxVenue;
            }
            if (execution.VenueOrderId != null)
            {
                var orxVenueOrderId = recycler.Borrow<OrxVenueOrderId>();
                orxVenueOrderId.CopyFrom(execution.VenueOrderId, recycler);
                VenueOrderId = orxVenueOrderId;
            }
            if (execution.OrderId != null)
            {
                var orxOrderId = recycler.Borrow<OrxOrderId>();
                orxOrderId.CopyFrom(execution.OrderId, recycler);
                OrderId = orxOrderId;
            }
            Price = execution.Price;
            Quantity = execution.Quantity;
            if (execution.CounterParty != null)
            {
                var orxCounterParty = recycler.Borrow<OrxParty>();
                orxCounterParty.CopyFrom(execution.CounterParty, recycler);
                CounterParty = orxCounterParty;
            }
            ValueDate = execution.ValueDate;
            Type = execution.Type;
            ExecutionStageType = execution.ExecutionStageType;
        }

        public IExecution Clone()
        {
            return new OrxExecution(this);
        }

        protected bool Equals(OrxExecution other)
        {
            var executionIdSame = Equals(ExecutionId, other.ExecutionId);
            var venueSame = Equals(Venue, other.Venue);
            var venueOrderIdSame = Equals(VenueOrderId, other.VenueOrderId);
            var orderIdSame = Equals(OrderId, other.OrderId);
            var executionTimeSame = Equals(ExecutionTime, other.ExecutionTime);
            var priceSame = Price == other.Price;
            var quantitySame = Quantity == other.Quantity;
            var cumlativeQuantitySame = CumlativeQuantity == other.CumlativeQuantity;
            var cumlativeVwapPriceSame = CumlativeVwapPrice == other.CumlativeVwapPrice;
            var counterPartySame = Equals(CounterParty, other.CounterParty);
            var valueDateSame = ValueDate.Equals(other.ValueDate);
            var typeSame = Type == other.Type;
            var executionStageTypeSame = ExecutionStageType == other.ExecutionStageType;

            return executionIdSame && venueSame && venueOrderIdSame && orderIdSame && executionTimeSame && 
                   priceSame && quantitySame && cumlativeQuantitySame && cumlativeVwapPriceSame && 
                   counterPartySame && valueDateSame && typeSame && executionStageTypeSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxExecution) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (ExecutionId != null ? ExecutionId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Venue != null ? Venue.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (VenueOrderId != null ? VenueOrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OrderId != null ? OrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExecutionTime != null ? ExecutionTime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ Quantity.GetHashCode();
                hashCode = (hashCode * 397) ^ CumlativeQuantity.GetHashCode();
                hashCode = (hashCode * 397) ^ CumlativeVwapPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ (CounterParty != null ? CounterParty.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Type;
                hashCode = (hashCode * 397) ^ (int)ExecutionStageType;
                return hashCode;
            }
        }
    }
}