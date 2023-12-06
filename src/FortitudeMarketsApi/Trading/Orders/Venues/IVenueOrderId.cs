#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenueOrderId : IReusableObject<IVenueOrderId>
{
    IMutableString VenueClientOrderId { get; set; }
    IMutableString VenueOrderIdentifier { get; set; }
}
