#region

using System.Collections;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class VenueOrders : IVenueOrders
{
    private readonly IList<IVenueOrder?> venueOrders;

    public VenueOrders() => venueOrders = new List<IVenueOrder?>();

    public VenueOrders(IVenueOrders toClone)
    {
        venueOrders = toClone.Select(vo => vo?.Clone()).ToList();
    }

    public VenueOrders(IList<IVenueOrder?> venueOrders) => this.venueOrders = venueOrders;

    public int Count => venueOrders.Count;

    public IVenueOrder? this[int index]
    {
        get => venueOrders[index];
        set => venueOrders[index] = value;
    }

    public IVenueOrders Clone() => new VenueOrders(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IVenueOrder> GetEnumerator() => venueOrders.GetEnumerator()!;
}
