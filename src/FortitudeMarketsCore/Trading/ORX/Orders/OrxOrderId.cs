using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders;

namespace FortitudeMarketsCore.Trading.ORX.Orders
{
    public class OrxOrderId : IOrderId
    {
        public OrxOrderId()
        {
        }

        public OrxOrderId(IOrderId toClone)
        {
            ClientOrderId = toClone.ClientOrderId;
            AdapterOrderId = toClone.AdapterOrderId;
            VenueClientOrderId = toClone.VenueClientOrderId != null ? new MutableString(toClone.VenueClientOrderId) : null;
            VenueAdapterOrderId = toClone.VenueAdapterOrderId != null ? new MutableString(toClone.VenueAdapterOrderId) : null;
            ParentOrderId = toClone.ParentOrderId != null ? new OrxOrderId(toClone.ParentOrderId) : null; 
            TrackingId = toClone.TrackingId != null ? new MutableString(toClone.TrackingId) : null;
        }
        
        public OrxOrderId(long clientOrderId, MutableString venueClientOrderId, long adapterOrderId = 0, 
            MutableString venueAdapterOrderId = null, OrxOrderId parentOrderId = null, MutableString trackingId = null)
        {
            ClientOrderId = clientOrderId;
            AdapterOrderId = adapterOrderId;
            VenueClientOrderId = venueClientOrderId;
            VenueAdapterOrderId = venueAdapterOrderId;
            ParentOrderId = parentOrderId;
            TrackingId = trackingId;
        }

        [OrxMandatoryField(0)]
        public long ClientOrderId { get; set; }

        [OrxMandatoryField(1)]
        public MutableString VenueClientOrderId { get; set; }

        IMutableString IOrderId.VenueClientOrderId
        {
            get => VenueClientOrderId;
            set => VenueClientOrderId = (MutableString)value;
        }

        [OrxOptionalField(2)]
        public long AdapterOrderId { get; set; }

        [OrxOptionalField(3)]
        public MutableString VenueAdapterOrderId { get; set; }

        IMutableString IOrderId.VenueAdapterOrderId
        {
            get => VenueAdapterOrderId;
            set => VenueAdapterOrderId = (MutableString)value;
        }

        [OrxOptionalField(4)]
        public MutableString TrackingId { get; set; }

        IMutableString IOrderId.TrackingId
        {
            get => TrackingId;
            set => TrackingId = (MutableString)value;
        }

        [OrxOptionalField(5)]
        public OrxOrderId ParentOrderId { get; set; }

        IOrderId IOrderId.ParentOrderId
        {
            get => ParentOrderId;
            set => ParentOrderId = (OrxOrderId)value;
        }

        public void CopyFrom(IOrderId orderId, IRecycler orxRecyclingFactory)
        {
            ClientOrderId = orderId.ClientOrderId;
            VenueClientOrderId = orderId.VenueClientOrderId != null
                ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(orderId.VenueClientOrderId)
                : null;
            AdapterOrderId = orderId.AdapterOrderId;
            VenueAdapterOrderId = orderId.VenueAdapterOrderId != null
                ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(orderId.VenueAdapterOrderId)
                : null;
            ParentOrderId = orderId.ParentOrderId != null ? new OrxOrderId(orderId.ParentOrderId) : null; 
            TrackingId = orderId.TrackingId != null
                ? orxRecyclingFactory.Borrow<MutableString>().Clear().Append(orderId.TrackingId)
                : null;
        }

        public IOrderId Clone()
        {
            return new OrxOrderId(this);
        }

        protected bool Equals(OrxOrderId other)
        {
            var clientOrderIdSame = Equals(ClientOrderId, other.ClientOrderId);
            var venueClientOrderIdSame = Equals(VenueClientOrderId, other.VenueClientOrderId);
            var adapterOrderId = Equals(AdapterOrderId, other.AdapterOrderId);
            var venueAdapterOrderId = Equals(VenueAdapterOrderId, other.VenueAdapterOrderId);
            var parentOrderIdSame = Equals(ParentOrderId, other.ParentOrderId);
            var trackingIdSame = Equals(TrackingId, other.TrackingId);
            return clientOrderIdSame && venueClientOrderIdSame && adapterOrderId && venueAdapterOrderId && 
                   parentOrderIdSame && trackingIdSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxOrderId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = TrackingId.GetHashCode();
                hashCode = (hashCode * 397) ^ ClientOrderId.GetHashCode();
                hashCode = (hashCode * 397) ^ (VenueClientOrderId != null ? VenueClientOrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AdapterOrderId.GetHashCode();
                hashCode = (hashCode * 397) ^ (VenueAdapterOrderId != null ? AdapterOrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParentOrderId != null ? ParentOrderId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TrackingId != null ? TrackingId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}