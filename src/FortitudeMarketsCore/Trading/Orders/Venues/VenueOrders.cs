using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeMarketsApi.Trading.Orders.Venues;

namespace FortitudeMarketsCore.Trading.Orders.Venues
{
    public class VenueOrders : IVenueOrders
    {
        private readonly IList<IVenueOrder> venueOrders;

        public VenueOrders(IVenueOrders toClone)
        {
            venueOrders = toClone.Select(vo => vo.Clone()).ToList();
        }

        public VenueOrders(IList<IVenueOrder> venueOrders)
        {
            this.venueOrders = venueOrders;
        }

        public int Count => venueOrders.Count;

        public IVenueOrder this[int index]
        {
            get => venueOrders[index];
            set => venueOrders[index] = value;
        }

        public IVenueOrders Clone()
        {
            return new VenueOrders(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IVenueOrder> GetEnumerator()
        {
            return venueOrders.GetEnumerator();
        }
    }
}