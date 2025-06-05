// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public class OrdersCountPriceVolumeLayer : PriceVolumeLayer, IMutableOrdersCountPriceVolumeLayer
{
    public OrdersCountPriceVolumeLayer() { }

    public OrdersCountPriceVolumeLayer
        (decimal price = 0m, decimal volume = 0m, uint ordersCount = 0, decimal? internalVolume = null) : base(price, volume)
    {
        OrdersCount    = ordersCount;
        InternalVolume = internalVolume ?? 0m;
    }

    public OrdersCountPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IOrdersCountPriceVolumeLayer ordersCountPvl)
        {
            OrdersCount    = ordersCountPvl.OrdersCount;
            InternalVolume = ordersCountPvl.InternalVolume;
        }
    }

    protected string OrdersCountPriceVolumeLayerToStringMembers =>
        $"{PriceVolumeLayerToStringMembers}, {nameof(OrdersCount)}: {OrdersCount}, {nameof(InternalVolume)}: {InternalVolume:N2}";

    [JsonIgnore] public override LayerType LayerType => LayerType.OrdersCountPriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalOrdersCountFlags | base.SupportsLayerFlags;

    public virtual uint OrdersCount { get; set; }

    public virtual decimal InternalVolume { get; set; }

    public decimal ExternalVolume => Volume - InternalVolume;

    public override bool IsEmpty
    {
        get => OrdersCount == 0 && InternalVolume == 0m && base.IsEmpty;
        set
        {
            base.IsEmpty = value;
            if (!value) return;
            OrdersCount    = 0;
            InternalVolume = 0;
        }
    }

    public override OrdersCountPriceVolumeLayer ResetWithTracking()
    {
        OrdersCount    = 0;
        InternalVolume = 0;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        OrdersCount    = 0;
        InternalVolume = 0;
        base.StateReset();
    }

    IOrdersCountPriceVolumeLayer ICloneable<IOrdersCountPriceVolumeLayer>.Clone() => Clone();

    IOrdersCountPriceVolumeLayer IOrdersCountPriceVolumeLayer.Clone() => Clone();

    IMutableOrdersCountPriceVolumeLayer IMutableOrdersCountPriceVolumeLayer.Clone() => Clone();

    public override OrdersCountPriceVolumeLayer Clone() =>
        Recycler?.Borrow<OrdersCountPriceVolumeLayer>().CopyFrom(this, QuoteInstantBehaviorFlags.DisableUpgradeLayer, CopyMergeFlags.FullReplace) 
     ?? new OrdersCountPriceVolumeLayer(this);

    IMutableOrdersCountPriceVolumeLayer ITrackableReset<IMutableOrdersCountPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableOrdersCountPriceVolumeLayer IMutableOrdersCountPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override OrdersCountPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);

        if (source is IOrdersCountPriceVolumeLayer ordersCountPvl)
        {
            OrdersCount    = ordersCountPvl.OrdersCount;
            InternalVolume = ordersCountPvl.InternalVolume;
        }
        return this;
    }

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        var baseSame = base.AreEquivalent(other, exactTypes);

        if (other is not IOrdersCountPriceVolumeLayer ordersCountPvl) return baseSame && !exactTypes;

        var ordersCountSame    = OrdersCount == ordersCountPvl.OrdersCount;
        var internalVolumeSame = InternalVolume == ordersCountPvl.InternalVolume;

        return baseSame && ordersCountSame && internalVolumeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrdersCountPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = ((int)OrdersCount * 397) ^ hashCode;
            hashCode = (InternalVolume.GetHashCode() * 397) ^ hashCode;
            return hashCode;
        }
    }

    public override string ToString() => $"{nameof(OrdersCountPriceVolumeLayer)}{{{OrdersCountPriceVolumeLayerToStringMembers}}}";
}
