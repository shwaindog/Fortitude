using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Orders.Venues
{
    public class VenueOrderId : IVenueOrderId
    {
        public VenueOrderId(IVenueOrderId toClone)
        {
            VenueClientOrderId = toClone.VenueClientOrderId;
            VenueOrderIdentifier = toClone.VenueOrderIdentifier;
        }

        public VenueOrderId(string marketClientOrderId, string marketOrderIdentifier)
        : this((MutableString)marketClientOrderId, (MutableString)marketClientOrderId)
        {

        }
        public VenueOrderId(IMutableString marketClientOrderId, IMutableString marketOrderIdentifier)
        {
            VenueClientOrderId = marketClientOrderId;
            VenueOrderIdentifier = marketOrderIdentifier;
        }

        public IMutableString VenueClientOrderId { get; set; }
        public IMutableString VenueOrderIdentifier { get; set; }
        public IVenueOrderId Clone()
        {
            return new VenueOrderId(this);
        }
    }
}