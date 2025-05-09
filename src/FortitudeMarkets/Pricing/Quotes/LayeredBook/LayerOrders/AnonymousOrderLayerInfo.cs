// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerOrders;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class AnonymousOrderLayerInfo : ReusableObject<IAnonymousOrderLayerInfo>, IMutableAnonymousOrderLayerInfo
{
    public AnonymousOrderLayerInfo() { }

    public AnonymousOrderLayerInfo
    (int orderId, LayerOrderFlags orderFlags, DateTime createdTime, decimal orderVolume, DateTime? updatedTime = null
      , decimal? remainingVolume = null)
    {
        OrderId              = orderId;
        OrderFlags           = orderFlags;
        CreatedTime          = createdTime;
        OrderVolume          = orderVolume;
        OrderRemainingVolume = remainingVolume ?? orderVolume;
        UpdatedTime          = updatedTime ?? createdTime;
    }

    public AnonymousOrderLayerInfo(IAnonymousOrderLayerInfo toClone)
    {
        OrderId              = toClone.OrderId;
        OrderFlags           = toClone.OrderFlags;
        CreatedTime          = toClone.CreatedTime;
        OrderVolume          = toClone.OrderVolume;
        OrderRemainingVolume = toClone.OrderRemainingVolume;
        UpdatedTime          = toClone.UpdatedTime;
    }

    public int             OrderId     { get; set; }
    public LayerOrderFlags OrderFlags  { get; set; }
    public DateTime        CreatedTime { get; set; }
    public DateTime        UpdatedTime { get; set; }

    public decimal OrderVolume { get; set; }

    public decimal OrderRemainingVolume { get; set; }

    public virtual bool IsEmpty
    {
        get =>
            OrderId == 0 && OrderFlags == LayerOrderFlags.None && CreatedTime == default && UpdatedTime == default && OrderVolume == 0m &&
            OrderRemainingVolume == 0m;
        set
        {
            if (!value) return;
            OrderId     = 0;
            OrderFlags  = LayerOrderFlags.None;
            CreatedTime = default;
            UpdatedTime = default;
            OrderVolume = 0m;

            OrderRemainingVolume = 0m;
        }
    }

    public override void StateReset()
    {
        OrderId     = 0;
        OrderFlags  = LayerOrderFlags.None;
        CreatedTime = default;
        UpdatedTime = default;
        OrderVolume = 0m;

        OrderRemainingVolume = 0m;
    }

    public override IAnonymousOrderLayerInfo CopyFrom
    (IAnonymousOrderLayerInfo source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        OrderId     = source.OrderId;
        OrderFlags  = source.OrderFlags;
        CreatedTime = source.CreatedTime;
        UpdatedTime = source.UpdatedTime;
        OrderVolume = source.OrderVolume;

        OrderRemainingVolume = source.OrderRemainingVolume;
        return this;
    }

    object ICloneable.Clone() => Clone();

    IMutableAnonymousOrderLayerInfo IMutableAnonymousOrderLayerInfo.Clone() => Clone();

    public bool AreEquivalent(IMutableAnonymousOrderLayerInfo? other, bool exactTypes = false) =>
        AreEquivalent((IAnonymousOrderLayerInfo?)other, exactTypes);

    public virtual bool AreEquivalent(IAnonymousOrderLayerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var orderIdsSame   = OrderId == other.OrderId;
        var orderFlagsSame = OrderFlags == other.OrderFlags;
        var createdSame    = CreatedTime == other.CreatedTime;
        var updatedSame    = UpdatedTime == other.UpdatedTime;
        var volumeSame     = OrderVolume == other.OrderVolume;

        var remainingVolumeSame = OrderRemainingVolume == other.OrderRemainingVolume;

        return orderIdsSame && orderFlagsSame && createdSame && updatedSame && volumeSame && remainingVolumeSame;
    }

    public override AnonymousOrderLayerInfo Clone() =>
        Recycler?.Borrow<AnonymousOrderLayerInfo>().CopyFrom(this) as AnonymousOrderLayerInfo ?? new AnonymousOrderLayerInfo(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IAnonymousOrderLayerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = OrderId;
            hashCode = ((int)OrderFlags * 397) ^ hashCode;
            hashCode = (CreatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (UpdatedTime.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderVolume.GetHashCode() * 397) ^ hashCode;
            hashCode = (OrderRemainingVolume.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(AnonymousOrderLayerInfo)}{{{nameof(OrderId)}: {OrderId}, {nameof(OrderFlags)}: {OrderFlags}, " +
        $"{nameof(CreatedTime)}: {CreatedTime}, {nameof(UpdatedTime)}: {UpdatedTime}, {nameof(OrderVolume)}: {OrderVolume:N2}, " +
        $"{nameof(OrderRemainingVolume)}: {OrderRemainingVolume:N2}}}";
}
