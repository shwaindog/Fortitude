#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenueOrders : IList<IVenueOrder>, IReusableObject<IVenueOrders>
{
}
