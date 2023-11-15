#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsCore.Trading.Orders.Venues;

public class VenueOrder : IVenueOrder
{
    private int refCount = 0;

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

    public void CopyFrom(IVenueOrder source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueOrderId = source.VenueOrderId;
        OrderId = source.OrderId;
        Status = source.Status;
        Venue = source.Venue;
        SubmitTime = source.SubmitTime;
        VenueAckTime = source.VenueAckTime;
        Ticker = source.Ticker;
        Price = source.Price;
        Quantity = source.Quantity;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IVenueOrder)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);
        return IsInRecycler;
    }


    public IVenueOrder Clone() => new VenueOrder(this);
}
