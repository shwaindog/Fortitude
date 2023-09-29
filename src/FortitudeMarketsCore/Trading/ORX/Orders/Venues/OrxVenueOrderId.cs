using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues
{
    public class OrxVenueOrderId : IVenueOrderId
    {
        public OrxVenueOrderId()
        {
        }
        public OrxVenueOrderId(IVenueOrderId toClone)
        {
            VenueClientOrderId = toClone.VenueClientOrderId != null 
                ? new MutableString(toClone.VenueClientOrderId) 
                : null;
            VenueOrderIdentifier = toClone.VenueOrderIdentifier != null 
                ? new MutableString(toClone.VenueOrderIdentifier) 
                : null;
        }

        public OrxVenueOrderId(string venueClientOrderId, string venueOrderIdentifier)
            : this((MutableString)venueClientOrderId, venueOrderIdentifier)
        { }

        public OrxVenueOrderId(MutableString venueClientOrderId, MutableString venueOrderIdentifier)
        {
            VenueClientOrderId = venueClientOrderId;
            VenueOrderIdentifier = venueOrderIdentifier;
        }

        [OrxMandatoryField(0)]
        public MutableString VenueClientOrderId { get; set; }

        IMutableString IVenueOrderId.VenueClientOrderId
        {
            get => VenueClientOrderId;
            set => VenueClientOrderId = (MutableString)value;
        }

        [OrxOptionalField(1)]
        public MutableString VenueOrderIdentifier { get; set; }

        IMutableString IVenueOrderId.VenueOrderIdentifier
        {
            get => VenueOrderIdentifier;
            set => VenueOrderIdentifier = (MutableString)value;
        }

        public void CopyFrom(IVenueOrderId venueOrderId, IRecycler recycler)
        {
            VenueClientOrderId = venueOrderId.VenueClientOrderId != null
                ? recycler.Borrow<MutableString>().Clear().Append(venueOrderId.VenueClientOrderId) as MutableString
                : null;
            VenueOrderIdentifier = venueOrderId.VenueOrderIdentifier != null
                ? recycler.Borrow<MutableString>().Clear().Append(venueOrderId.VenueOrderIdentifier) as MutableString
                : null;
        }

        public IVenueOrderId Clone()
        {
            return new OrxVenueOrderId(this);
        }

        protected bool Equals(OrxVenueOrderId other)
        {
            var clientIdSame = Equals(VenueClientOrderId, other.VenueClientOrderId);
            var venueIdSame = Equals(VenueOrderIdentifier, other.VenueOrderIdentifier);
            return clientIdSame && venueIdSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((OrxVenueOrderId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((VenueClientOrderId != null ? VenueClientOrderId.GetHashCode() : 0) * 397) ^ 
                       (VenueOrderIdentifier != null ? VenueOrderIdentifier.GetHashCode() : 0);
            }
        }
    }
}
