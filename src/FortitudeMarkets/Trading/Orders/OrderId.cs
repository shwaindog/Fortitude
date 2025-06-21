// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Trading.ORX.Orders;

#endregion

namespace FortitudeMarkets.Trading.Orders;

public class OrderId : ReusableObject<IOrderId>, IOrderId
{
    public OrderId() { }

    public OrderId(IOrderId toClone)
    {
        ClientOrderId  = toClone.ClientOrderId;
        TrackingId     = toClone.TrackingId;
        AdapterOrderId = toClone.AdapterOrderId;
        ParentOrderId  = toClone.ParentOrderId;
        OrderBookingId = toClone.OrderBookingId;
    }

    public OrderId(uint clientOrderId, uint? trackingId = null, uint? adapterOrderId = null
     , long? orderBookingId = null, IOrderId? parentOrderId = null)
    {
        ClientOrderId  = clientOrderId;
        TrackingId     = trackingId;
        AdapterOrderId = adapterOrderId;
        OrderBookingId      = orderBookingId;
        ParentOrderId = parentOrderId ?? new OrxOrderId();
    }

    public uint ClientOrderId { get; set; }

    public uint? TrackingId { get; set; }

    public uint? AdapterOrderId { get; set; }

    public long?     OrderBookingId { get; set; }

    public IOrderId? ParentOrderId  { get; set; }


    public override void StateReset()
    {
        ClientOrderId  = 0;
        TrackingId     = null;
        AdapterOrderId = null;
        OrderBookingId = null;
        ParentOrderId?.DecrementRefCount();
        ParentOrderId = null;
        base.StateReset();
    }
    

    object ICloneable.Clone() => Clone();

    public override IOrderId Clone() => Recycler?.Borrow<OrderId>().CopyFrom(this) ?? new OrderId(this);


    public override IOrderId CopyFrom(IOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClientOrderId  = source.ClientOrderId;
        TrackingId     = source.TrackingId;
        AdapterOrderId = source.AdapterOrderId;
        OrderBookingId = source.OrderBookingId;
        ParentOrderId  = source.ParentOrderId?.SyncOrRecycle(ParentOrderId as OrderId);
        return this;
    }

    public bool AreEquivalent(IOrderId? other, bool exactTypes = false)
    {
        if(other == null) return false;
        var clientOrderIdSame = ClientOrderId == other.ClientOrderId;
        var parentIdSame      = ParentOrderId?.AreEquivalent(other.ParentOrderId, exactTypes) ?? other.ParentOrderId == null;

        var trackingIdSame = true;
        var adapterIdSame = true;
        var orderBookingIdSame = true;
        if (exactTypes)
        {
            trackingIdSame = TrackingId == other.TrackingId;
            adapterIdSame  = AdapterOrderId == other.AdapterOrderId;
            trackingIdSame = OrderBookingId == other.OrderBookingId;
        }

        var allAreSame = clientOrderIdSame && parentIdSame && trackingIdSame && adapterIdSame && orderBookingIdSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderId, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)ClientOrderId;
            hashCode = (hashCode * 397) ^ TrackingId.GetHashCode();
            hashCode = (hashCode * 397) ^ AdapterOrderId.GetHashCode();
            hashCode = (hashCode * 397) ^ OrderBookingId.GetHashCode();
            hashCode = (hashCode * 397) ^ (ParentOrderId != null ? ParentOrderId.GetHashCode() : 0);
            return hashCode;
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(nameof(OrderId)).Append("{");
        if (ClientOrderId != 0) sb.Append(nameof(ClientOrderId)).Append(": ").Append(ClientOrderId).Append(", ");
        if (ClientOrderId != 0) sb.Append(nameof(TrackingId)).Append(": ").Append(TrackingId).Append(", ");
        if (AdapterOrderId != 0) sb.Append(nameof(AdapterOrderId)).Append(": ").Append(AdapterOrderId).Append(", ");
        if (AdapterOrderId != 0) sb.Append(nameof(OrderBookingId)).Append(": ").Append(OrderBookingId).Append(", ");
        if (ParentOrderId != null) sb.Append(nameof(ParentOrderId)).Append(": ").Append(ParentOrderId).Append(", ");
        if (sb[^2] == ',')
        {
            sb.Length -= 2;
        }
        sb.Append("}");

        return sb.ToString();
    }
}
