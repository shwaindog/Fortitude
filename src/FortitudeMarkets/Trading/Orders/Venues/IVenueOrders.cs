#region

using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenueOrders : IList<IVenueOrder>, IReusableObject<IVenueOrders>
{
}
