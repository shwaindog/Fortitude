// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public class PriceVolumeLayer : ReusableQuoteElement<IPriceVolumeLayer>, IMutablePriceVolumeLayer
{
    public PriceVolumeLayer() { }

    public PriceVolumeLayer(decimal price = 0m, decimal volume = 0m)
    {
        Price  = price;
        Volume = volume;
    }

    public PriceVolumeLayer(IPriceVolumeLayer toClone)
    {
        Price  = toClone.Price;
        Volume = toClone.Volume;
    }

    protected string PriceVolumeLayerToStringMembers => $"{nameof(Price)}: {Price:N5}, {nameof(Volume)}: {Volume:N2}";

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Price { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public decimal Volume { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual bool IsEmpty
    {
        get => Price == 0m && Volume == 0m;
        set
        {
            if (!value) return;
            Price = Volume = 0m;
        }
    }

    [JsonIgnore] public virtual LayerType LayerType => LayerType.PriceVolume;

    [JsonIgnore] public virtual LayerFlags SupportsLayerFlags => LayerFlagsExtensions.PriceVolumeLayerFlags;

    IMutablePriceVolumeLayer ITrackableReset<IMutablePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    public virtual PriceVolumeLayer ResetWithTracking()
    {
        Price = Volume = 0m;
        return this;
    }

    public override void StateReset()
    {
        Price = Volume = 0m;
        base.StateReset();
    }

    IMutablePriceVolumeLayer ITransferQuoteState<IMutablePriceVolumeLayer>.CopyFrom
        (IMutablePriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) => 
        CopyFrom(source, behaviorFlags, copyMergeFlags);

    IReusableQuoteElement<IMutablePriceVolumeLayer> ITransferQuoteState<IReusableQuoteElement<IMutablePriceVolumeLayer>>.CopyFrom
        (IReusableQuoteElement<IMutablePriceVolumeLayer> source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPriceVolumeLayer)source, behaviorFlags, copyMergeFlags);

    public override PriceVolumeLayer CopyFrom (IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Price  = source.Price;
        Volume = source.Volume;
        return this;
    }

    IMutablePriceVolumeLayer ICloneable<IMutablePriceVolumeLayer>.Clone() => Clone();

    IMutablePriceVolumeLayer IMutablePriceVolumeLayer.Clone() => Clone();

    public override PriceVolumeLayer Clone() => 
        Recycler?.Borrow<PriceVolumeLayer>().CopyFrom(this, QuoteInstantBehaviorFlags.DisableUpgradeLayer) ?? new PriceVolumeLayer(this);

    bool IInterfacesComparable<IMutablePriceVolumeLayer>.AreEquivalent(IMutablePriceVolumeLayer? other, bool exactTypes) => 
        AreEquivalent(other, exactTypes);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var priceSame  = Price == other.Price;
        var volumeSame = Volume == other.Volume;

        return priceSame && volumeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (Price.GetHashCode() * 397) ^ Volume.GetHashCode();
        }
    }

    public override string ToString() => $"{nameof(PriceVolumeLayer)}{{{PriceVolumeLayerToStringMembers}}}";
}
