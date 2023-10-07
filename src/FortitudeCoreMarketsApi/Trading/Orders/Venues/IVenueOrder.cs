using System;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders.Products;

namespace FortitudeMarketsApi.Trading.Orders.Venues
{
    public interface IVenueOrder
    {
        IVenueOrderId VenueOrderId { get; set; }
        IOrderId OrderId { get; set; }
        OrderStatus Status { get; set; }
        DateTime SubmitTime { get; set; }
        DateTime VenueAckTime { get; set; }
        IVenue Venue { get; set; }
        IMutableString Ticker { get; set; }
        decimal Price { get; set; }
        decimal Quantity { get; set; }
        IVenueOrder Clone();
    }
}
