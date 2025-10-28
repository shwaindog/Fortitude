#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public interface IVenueOrder : IReusableObject<IVenueOrder>
{
    IVenueOrderId? VenueOrderId { get; set; }
    IOrderId? OrderId { get; set; }
    OrderStatus Status { get; set; }
    DateTime SubmitTime { get; set; }
    DateTime VenueAckTime { get; set; }
    IVenue? Venue { get; set; }
    IMutableString? Ticker { get; set; }
    decimal Price { get; set; }
    decimal Quantity { get; set; }
}
