using System;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Client;
using FortitudeMarketsCore.Trading.Orders.Client;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Client
{
    public class OrxOrderSubmitRequest : OrxTradingMessage, IOrderSubmitRequest
    {
        public override uint MessageId => (uint) TradingMessageIds.SubmitRequest;

        public OrxOrderSubmitRequest()
        {
        }

        public OrxOrderSubmitRequest(IOrderSubmitRequest toClone)
        {
            OrderDetails = toClone.OrderDetails != null ? new OrxOrder(toClone.OrderDetails) : null;
            AttemptNumber = toClone.AttemptNumber;
            CurrentAttemptTime = toClone.CurrentAttemptTime;
            OriginalAttemptTime = toClone.OriginalAttemptTime;
            Tag = toClone.Tag != null ? new MutableString(toClone.Tag) : null;
        }

        public OrxOrderSubmitRequest(OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
            DateTime originalAttemptTime, string tag)
        : this(orderDetails, attemptNumber, currentAttemptTime, originalAttemptTime, (MutableString)tag)
        { }

        public OrxOrderSubmitRequest(OrxOrder orderDetails, int attemptNumber, DateTime currentAttemptTime,
            DateTime originalAttemptTime, MutableString tag)
        {
            OrderDetails = orderDetails;
            AttemptNumber = attemptNumber;
            CurrentAttemptTime = currentAttemptTime;
            OriginalAttemptTime = originalAttemptTime;
            Tag = tag;
        }

        [OrxMandatoryField(10)]
        public OrxOrder OrderDetails { get; set; }

        IOrder IOrderSubmitRequest.OrderDetails
        {
            get => OrderDetails;
            set => OrderDetails = (OrxOrder)value;
        }
        [OrxMandatoryField(11)]
        public int AttemptNumber { get; set; }

        [OrxMandatoryField(12)]
        public DateTime CurrentAttemptTime { get; set; }
        
        [OrxOptionalField(13)]
        public DateTime OriginalAttemptTime { get; set; }
        
        [OrxOptionalField(14)]
        public MutableString Tag { get; set; }

        IMutableString IOrderSubmitRequest.Tag
        {
            get => Tag;
            set => Tag = (MutableString)value;
        }

        public virtual IOrderSubmitRequest Clone()
        {
            return new OrderSubmitRequest(this);
        }

        protected bool Equals(OrxOrderSubmitRequest other)
        {
            var orderDetailsSame = Equals(OrderDetails, other.OrderDetails);
            var attemptNumberSame = AttemptNumber == other.AttemptNumber;
            var currentAttemptTimeSame = Equals(CurrentAttemptTime, other.CurrentAttemptTime);
            var originalAttemptTimeSame = Equals(OriginalAttemptTime, other.OriginalAttemptTime);
            var tagSame = string.Equals(Tag, other.Tag);
            return orderDetailsSame && attemptNumberSame && currentAttemptTimeSame && originalAttemptTimeSame
                && tagSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrxOrderSubmitRequest) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (OrderDetails != null ? OrderDetails.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AttemptNumber;
                hashCode = (hashCode * 397) ^ (CurrentAttemptTime != null ? CurrentAttemptTime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (OriginalAttemptTime != null ? OriginalAttemptTime.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Tag != null ? Tag.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
