// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower.Forge;

#endregion

namespace FortitudeMarkets.Trading.Orders.Venues;

public class VenueOrder : ReusableObject<IVenueOrder>, IVenueOrder
{
    public VenueOrder() { }

    public VenueOrder(IVenueOrder toClone)
    {
        VenueOrderId = toClone.VenueOrderId?.Clone();

        OrderId = toClone.OrderId?.Clone();

        Status = toClone.Status;
        Venue  = toClone.Venue;

        SubmitTime   = toClone.SubmitTime;
        VenueAckTime = toClone.VenueAckTime;

        Ticker = toClone.Ticker;
        Price  = toClone.Price;

        Quantity = toClone.Quantity;
    }

    public VenueOrder
    (IVenueOrderId venueId, IOrderId orderId, OrderStatus status, IVenue venue,
        DateTime submitTime, DateTime venueAckTime, string ticker, decimal price, decimal quantity)
        : this(venueId, orderId, status, venue, submitTime, venueAckTime, (MutableString)ticker, price, quantity) { }

    public VenueOrder
    (IVenueOrderId venueId, IOrderId orderId, OrderStatus status, IVenue venue,
        DateTime submitTime, DateTime venueAckTime, IMutableString ticker, decimal price, decimal quantity)
    {
        VenueOrderId = venueId;

        OrderId = orderId;

        Status = status;
        Venue  = venue;

        SubmitTime   = submitTime;
        VenueAckTime = venueAckTime;

        Ticker = ticker;
        Price  = price;

        Quantity = quantity;
    }

    public IVenueOrderId? VenueOrderId { get; set; }

    public IOrderId? OrderId { get; set; }

    public OrderStatus Status { get; set; }

    public IVenue?  Venue        { get; set; }
    public DateTime SubmitTime   { get; set; }
    public DateTime VenueAckTime { get; set; }

    public IMutableString? Ticker { get; set; }

    public decimal Price    { get; set; }
    public decimal Quantity { get; set; }

    public override IVenueOrder Clone() => Recycler?.Borrow<VenueOrder>().CopyFrom(this) ?? new VenueOrder(this);

    public override IVenueOrder CopyFrom(IVenueOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueOrderId = source.VenueOrderId;

        OrderId = source.OrderId;
        Status  = source.Status;
        Venue   = source.Venue;

        SubmitTime = source.SubmitTime;

        VenueAckTime = source.VenueAckTime;

        Ticker = source.Ticker;
        Price  = source.Price;

        Quantity = source.Quantity;
        return this;
    }
}
