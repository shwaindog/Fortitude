#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenue : IReusableObject<IVenue>
{
    ushort VenueId { get; set; }
    IMutableString Name { get; set; }
}
