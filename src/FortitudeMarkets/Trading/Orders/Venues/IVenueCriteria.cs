#region

using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenueCriteria : IEnumerable<IVenue>, IReusableObject<IVenueCriteria>
{
    IVenue this[int index] { get; set; }
    int Count { get; }
    VenueSelectionMethod VenueSelectionMethod { get; set; }
}
