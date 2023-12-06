#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders;

#endregion

namespace FortitudeMarketsCore.Trading.Orders;

public class OrderId : ReusableObject<IOrderId>, IOrderId
{
    public OrderId() { }

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

    public override void Reset()
    {
        base.Reset();
    }


    public override IOrderId CopyFrom(IOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        AdapterOrderId = source.AdapterOrderId;
        VenueAdapterOrderId = source.VenueAdapterOrderId?.CopyOrClone(VenueAdapterOrderId as MutableString);
        ClientOrderId = source.ClientOrderId;
        VenueClientOrderId = source.VenueClientOrderId?.CopyOrClone(VenueClientOrderId as MutableString);
        ParentOrderId = source.ParentOrderId?.CopyOrClone(ParentOrderId as OrderId);
        TrackingId = source.TrackingId?.Clone();
        return this;
    }

    object ICloneable.Clone() => Clone();

    public override IOrderId Clone() => Recycler?.Borrow<OrderId>().CopyFrom(this) ?? new OrderId(this);
}
