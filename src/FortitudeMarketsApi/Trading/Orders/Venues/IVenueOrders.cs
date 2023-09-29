using System.Collections.Generic;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public interface IVenueOrders : IEnumerable<IVenueOrder>
    {
        int Count { get; }
        IVenueOrder this[int index] { get; set; }
        IVenueOrders Clone();
    }
}
