#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenue : IReusableObject<IVenue>
{
    ushort VenueId { get; set; }
    IMutableString Name { get; set; }
}
