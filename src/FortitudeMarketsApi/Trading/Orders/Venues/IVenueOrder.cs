#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Venues;

public interface IVenueOrder : IRecyclableObject<IVenueOrder>
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
    IVenueOrder Clone();
}
