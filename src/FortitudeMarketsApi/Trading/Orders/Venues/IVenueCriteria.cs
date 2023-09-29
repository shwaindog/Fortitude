using System.Collections.Generic;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public interface IVenueCriteria : IEnumerable<IVenue>
    {
        IVenue this[int index] { get; set; }
        int Count { get; }
        VenueSelectionMethod VenueSelectionMethod { get; set; }
        IVenueCriteria Clone();
    }
}
