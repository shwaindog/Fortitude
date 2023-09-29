using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading.ORX.Serialization;

namespace FortitudeMarketsCore.Trading.ORX.Orders.Venues
{
    public class OrxVenue : IVenue
    {
        public OrxVenue()
        {
        }

        public OrxVenue(IVenue toClone)
        {
            VenueId = toClone.VenueId;
            Name = toClone.Name != null ? new MutableString(toClone.Name) : null;
        }

        public OrxVenue(ushort venueId, string name)
        : this(venueId, (MutableString)name)
        { }

        public OrxVenue(ushort venueId, MutableString name)
        {
            VenueId = venueId;
            Name = name;
        }

        [OrxMandatoryField(0)]
        public ushort VenueId { get; set; }

        [OrxOptionalField(1)]
        public MutableString Name { get; set; }

        IMutableString IVenue.Name
        {
            get => Name;
            set => Name = (MutableString)value;
        }

        public void CopyFrom(IVenue venue, IRecycler recycler)
        {
            VenueId = venue.VenueId;
            Name = venue.Name != null
                ? recycler.Borrow<MutableString>().Clear().Append(venue.Name) as MutableString
                : null;
        }

        public IVenue Clone()
        {
            return new OrxVenue(this);
        }

        protected bool Equals(OrxVenue other)
        {
            var venueIdSame = VenueId == other.VenueId;
            var nameSame = string.Equals(Name, other.Name);
            return venueIdSame && nameSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((OrxVenue) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (VenueId.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}