#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenueCriteria : IEnumerable<IVenue>, IRecyclableObject<IVenueCriteria>
{
    IVenue this[int index] { get; set; }
    int Count { get; }
    VenueSelectionMethod VenueSelectionMethod { get; set; }
    IVenueCriteria Clone();
}
