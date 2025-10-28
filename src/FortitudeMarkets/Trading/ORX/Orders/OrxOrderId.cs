// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Orders;

#endregion

namespace FortitudeMarkets.Trading.ORX.Orders;

public class OrxOrderId : ReusableObject<IOrderId>, IOrderId, ITransferState<OrxOrderId>
{
    public OrxOrderId() { }

    public OrxOrderId(IOrderId toClone)
    {
        ClientOrderId  = toClone.ClientOrderId;
        TrackingId     = toClone.TrackingId;
        AdapterOrderId = toClone.AdapterOrderId;
        OrderBookingId = toClone.OrderBookingId;
        ParentOrderId  = toClone.ParentOrderId != null ? new OrxOrderId(toClone.ParentOrderId) : null;
    }

    public OrxOrderId(uint clientOrderId, uint? trackingId = null, uint? adapterOrderId = null
      , long? orderBookingId = null, IOrderId? parentOrderId = null)
    {
        ClientOrderId  = clientOrderId;
        TrackingId     = trackingId;
        AdapterOrderId = adapterOrderId;
        OrderBookingId = orderBookingId;
        ParentOrderId  = parentOrderId != null ? new OrxOrderId() : null;
    }

    [OrxMandatoryField(1)] public uint ClientOrderId { get; set; }

    [OrxOptionalField(2)] public uint? TrackingId { get; set; }

    [OrxOptionalField(3)] public uint? AdapterOrderId { get; set; }

    [OrxOptionalField(4)] public long? OrderBookingId { get; set; }

    [OrxOptionalField(5)] public OrxOrderId? ParentOrderId { get; set; }

    public bool AutoRecycledByProducer { get; set; } = false;

    IOrderId? IOrderId.ParentOrderId
    {
        get => ParentOrderId;
        set => ParentOrderId = value as OrxOrderId;
    }

    public override IOrderId Clone() => Recycler?.Borrow<OrxOrderId>().CopyFrom(this) ?? new OrxOrderId(this);

    public override IOrderId CopyFrom(IOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClientOrderId  = source.ClientOrderId;
        TrackingId     = source.TrackingId;
        AdapterOrderId = source.AdapterOrderId;
        OrderBookingId = source.OrderBookingId;
        ParentOrderId  = source.ParentOrderId?.SyncOrRecycle(ParentOrderId);
        return this;
    }

    public OrxOrderId CopyFrom(OrxOrderId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (OrxOrderId)CopyFrom((IOrderId)source, copyMergeFlags);

    public bool AreEquivalent(IOrderId? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var clientOrderIdSame = ClientOrderId == other.ClientOrderId;

        var adapterIdSame      = AdapterOrderId == 0 || other.AdapterOrderId == 0 || AdapterOrderId == other.AdapterOrderId;
        var orderBookingIdSame = OrderBookingId == 0 || other.OrderBookingId == 0 || OrderBookingId == other.OrderBookingId ;

        var allAreSame = clientOrderIdSame && adapterIdSame && orderBookingIdSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => (ReferenceEquals(this, obj)) || AreEquivalent(obj as IOrderId, true);
    
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)ClientOrderId;
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
