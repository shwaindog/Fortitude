#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenue : IRecyclableObject<IVenue>
{
    ushort VenueId { get; set; }
    IMutableString Name { get; set; }
    IVenue Clone();
}
