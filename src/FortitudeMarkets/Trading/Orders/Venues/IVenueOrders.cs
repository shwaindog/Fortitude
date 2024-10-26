#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenueOrders : IEnumerable<IVenueOrder>, IReusableObject<IVenueOrders>
{
    int Count { get; }
    IVenueOrder? this[int index] { get; set; }
}
