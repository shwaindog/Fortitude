#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenueOrderId : IReusableObject<IVenueOrderId>
{
    IMutableString VenueClientOrderId { get; set; }
    IMutableString VenueOrderIdentifier { get; set; }
}
