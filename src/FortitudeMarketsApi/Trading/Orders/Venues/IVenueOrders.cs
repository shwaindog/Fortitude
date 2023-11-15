#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenueOrders : IEnumerable<IVenueOrder>, IRecyclableObject<IVenueOrders>
{
    int Count { get; }
    IVenueOrder? this[int index] { get; set; }
    IVenueOrders Clone();
}
