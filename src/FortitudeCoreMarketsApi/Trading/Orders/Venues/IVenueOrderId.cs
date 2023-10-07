using System.Net.Mail;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public interface IVenueOrderId
    {
        IMutableString VenueClientOrderId { get; set; }
        IMutableString VenueOrderIdentifier { get; set; }
        IVenueOrderId Clone();
    }
}