using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public interface IVenue
    {
        ushort VenueId { get; set; }
        IMutableString Name { get; set; }
        IVenue Clone();
    }
}