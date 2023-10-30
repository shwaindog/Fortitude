#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class VenueOrder : IVenueOrder
{
    public VenueOrder(IVenueOrder toClone)
    {
        VenueOrderId = toClone.VenueOrderId?.Clone();
        OrderId = toClone.OrderId?.Clone();
        Status = toClone.Status;
        Venue = toClone.Venue;
        SubmitTime = toClone.SubmitTime;
        VenueAckTime = toClone.VenueAckTime;
        Ticker = toClone.Ticker;
        Price = toClone.Price;
        Quantity = toClone.Quantity;
    }

    public VenueOrder(IVenueOrderId venueId, IOrderId orderId, OrderStatus status, IVenue venue,
        DateTime submitTime, DateTime venueAckTime, string ticker, decimal price, decimal quantity)
        : this(venueId, orderId, status, venue, submitTime, venueAckTime, (MutableString)ticker, price, quantity) { }

    public VenueOrder(IVenueOrderId venueId, IOrderId orderId, OrderStatus status, IVenue venue,
        DateTime submitTime, DateTime venueAckTime, IMutableString ticker, decimal price, decimal quantity)
    {
        VenueOrderId = venueId;
        OrderId = orderId;
        Status = status;
        Venue = venue;
        SubmitTime = submitTime;
        VenueAckTime = venueAckTime;
        Ticker = ticker;
        Price = price;
        Quantity = quantity;
    }

    public IVenueOrderId? VenueOrderId { get; set; }
    public IOrderId? OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public IVenue? Venue { get; set; }
    public DateTime SubmitTime { get; set; }
    public DateTime VenueAckTime { get; set; }
    public IMutableString? Ticker { get; set; }
    public decimal Price { get; set; }
    public decimal Quantity { get; set; }

    public IVenueOrder Clone() => new VenueOrder(this);
}
