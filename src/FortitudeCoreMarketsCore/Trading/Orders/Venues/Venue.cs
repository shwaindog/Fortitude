using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public class Venue : IVenue
    {
        public Venue(IVenue toClone)
        {
            VenueId = toClone.VenueId;
            Name = toClone.Name;
        }

        public Venue(ushort venueId, string name)
        : this(venueId, (MutableString)name)
        {
        }

        public Venue(ushort venueId, IMutableString name)
        {
            VenueId = venueId;
            Name = name;
        }

        public ushort VenueId { get; set; }
        public IMutableString Name { get; set; }
        public IVenue Clone()
        {
            return new Venue(this);
        }
    }
}