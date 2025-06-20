// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public class ValueDatePriceVolumeLayer : PriceVolumeLayer, IMutableValueDatePriceVolumeLayer
{
    public ValueDatePriceVolumeLayer() => ValueDate = DateTime.MinValue;

    public ValueDatePriceVolumeLayer
    (decimal price = 0m, decimal volume = 0m,
        DateTime? valueDate = null) : base(price, volume) =>
        ValueDate = valueDate ?? DateTime.MinValue;

    public ValueDatePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IValueDatePriceVolumeLayer valueDatePriceVolumeLayer)
            ValueDate = valueDatePriceVolumeLayer.ValueDate;
        else
            ValueDate = DateTime.MinValue;
    }

    protected string ValueDatePriceVolumeLayerToStringMembers => $"{PriceVolumeLayerToStringMembers}, {nameof(ValueDate)}: {ValueDate}";

    [JsonIgnore] public override LayerType LayerType => LayerType.ValueDatePriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalValueDateFlags | base.SupportsLayerFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && ValueDate == DateTime.MinValue;
        set
        {
            if (!value) return;
            ValueDate = DateTime.MinValue;

            base.IsEmpty = true;
        }
    }

    IMutableValueDatePriceVolumeLayer ITrackableReset<IMutableValueDatePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.                 ResetWithTracking() => ResetWithTracking();

    public override ValueDatePriceVolumeLayer ResetWithTracking()
    {
        ValueDate = DateTime.MinValue;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        base.StateReset();
        ValueDate = DateTime.MinValue;
    }

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => ((IValueDatePriceVolumeLayer)this).Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => Clone();

    public override ValueDatePriceVolumeLayer Clone() => 
        Recycler?.Borrow<ValueDatePriceVolumeLayer>().CopyFrom(this, QuoteInstantBehaviorFlags.DisableUpgradeLayer) 
     ?? new ValueDatePriceVolumeLayer(this);

    public override ValueDatePriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);
        if (source is IValueDatePriceVolumeLayer sourceSourcePriceVolumeLayer) ValueDate = sourceSourcePriceVolumeLayer.ValueDate;
        return this;
    }

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IValueDatePriceVolumeLayer otherValueDatePvLayer)) return false;
        var baseSame      = base.AreEquivalent(otherValueDatePvLayer, exactTypes);
        var valueDateSame = Equals(ValueDate, otherValueDatePvLayer.ValueDate);

        return baseSame && valueDateSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ ValueDate.GetHashCode();
        }
    }

    public override string ToString() => $"{nameof(ValueDatePriceVolumeLayer)}{{{ValueDatePriceVolumeLayerToStringMembers}}}";
}
