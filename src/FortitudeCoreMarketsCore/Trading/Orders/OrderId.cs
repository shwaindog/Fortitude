#region

using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders;

#endregion

namespace FortitudeMarketsCore.Trading.Orders;

public class OrderId : IOrderId
{
    public OrderId(IOrderId toClone)
    {
        ClientOrderId = toClone.ClientOrderId;
        VenueClientOrderId = toClone.VenueClientOrderId != null ? new MutableString(toClone.VenueClientOrderId) : null;
        AdapterOrderId = toClone.AdapterOrderId;
        VenueAdapterOrderId
            = toClone.VenueAdapterOrderId != null ? new MutableString(toClone.VenueAdapterOrderId) : null;
        ParentOrderId = toClone.ParentOrderId != null ? new OrxOrderId(toClone.ParentOrderId) : null;
        TrackingId = toClone.TrackingId;
    }

    public OrderId(long clientOrderId, string venueClientOrderId = "", long adapterOrderId = 0,
        string venueAdapterOrderId = "", IOrderId? parentOrderId = null, string trackingId = "")
        : this(clientOrderId, (MutableString)venueClientOrderId, (MutableString)venueAdapterOrderId
            , (MutableString)trackingId, adapterOrderId, parentOrderId) { }

    public OrderId(long clientOrderId, IMutableString venueClientOrderId,
        IMutableString venueAdapterOrderId, IMutableString trackingId, long adapterOrderId = 0,
        IOrderId? parentOrderId = null)
    {
        ClientOrderId = clientOrderId;
        AdapterOrderId = adapterOrderId;
        VenueClientOrderId = venueClientOrderId;
        VenueAdapterOrderId = venueAdapterOrderId;
        ParentOrderId = parentOrderId ?? new OrxOrderId();
        TrackingId = trackingId;
    }

    public long AdapterOrderId { get; set; }
    public IMutableString? VenueAdapterOrderId { get; set; }
    public long ClientOrderId { get; set; }
    public IMutableString? VenueClientOrderId { get; set; }
    public IOrderId? ParentOrderId { get; set; }
    public IMutableString? TrackingId { get; set; }

    public IOrderId Clone() => new OrderId(this);
}
