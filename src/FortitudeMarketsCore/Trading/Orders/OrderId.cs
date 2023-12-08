#region

using System.Text;
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
        AdapterOrderId = 0;
        ClientOrderId = 0;
        TrackingId?.DecrementRefCount();
        TrackingId = null;
        VenueAdapterOrderId?.DecrementRefCount();
        VenueAdapterOrderId = null;
        VenueClientOrderId?.DecrementRefCount();
        VenueClientOrderId = null;
        ParentOrderId?.DecrementRefCount();
        ParentOrderId = null;
        base.Reset();
    }


    public override IOrderId CopyFrom(IOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        AdapterOrderId = source.AdapterOrderId;
        VenueAdapterOrderId = source.VenueAdapterOrderId?.SyncOrRecycle(VenueAdapterOrderId as MutableString);
        ClientOrderId = source.ClientOrderId;
        VenueClientOrderId = source.VenueClientOrderId?.SyncOrRecycle(VenueClientOrderId as MutableString);
        ParentOrderId = source.ParentOrderId?.SyncOrRecycle(ParentOrderId as OrderId);
        TrackingId = source.TrackingId?.SyncOrRecycle(VenueAdapterOrderId as MutableString);
        return this;
    }

    object ICloneable.Clone() => Clone();

    public override IOrderId Clone() => Recycler?.Borrow<OrderId>().CopyFrom(this) ?? new OrderId(this);

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("OrderId(");
        if (ClientOrderId != 0) sb.Append("ClientOrderId: ").Append(ClientOrderId).Append(", ");
        if (AdapterOrderId != 0) sb.Append("AdapterOrderId: ").Append(AdapterOrderId).Append(", ");
        if (TrackingId != null && TrackingId.Length > 0) sb.Append("TrackingId: ").Append(TrackingId).Append(", ");
        if (VenueClientOrderId != null && VenueClientOrderId.Length > 0)
            sb.Append("VenueClientOrderId: ").Append(VenueClientOrderId).Append(", ");
        if (VenueAdapterOrderId != null && VenueAdapterOrderId.Length > 0)
            sb.Append("VenueAdapterOrderId: ").Append(VenueAdapterOrderId).Append(", ");
        if (ParentOrderId != null) sb.Append("ParentOrderId: ").Append(ParentOrderId).Append(", ");
        if (sb[^2] == ',')
        {
            sb[^2] = ')';
            sb.Length -= 1;
        }

        return sb.ToString();
    }
}
