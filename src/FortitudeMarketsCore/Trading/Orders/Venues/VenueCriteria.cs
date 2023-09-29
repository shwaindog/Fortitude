using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Orders.Venues
{
    public class VenueCriteria : IVenueCriteria
    {
        private readonly IList<IVenue> venues;

        public VenueCriteria(IVenueCriteria toClone)
        {
            venues = toClone.Select(v => v.Clone()).ToList();
            VenueSelectionMethod = toClone.VenueSelectionMethod;
        }

        public VenueCriteria(IList<IVenue> venues, VenueSelectionMethod venueSelectionMethod)
        {
            this.venues = venues;
            VenueSelectionMethod = venueSelectionMethod;
        }

        public IVenue this[int index]
        {
            get => venues[index];
            set => venues[index] = value;
        }

        public int Count => venues.Count;
        public VenueSelectionMethod VenueSelectionMethod { get; set; }
        public IVenueCriteria Clone()
        {
            return new VenueCriteria(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IVenue> GetEnumerator()
        {
            return venues.GetEnumerator();
        }
    }
}