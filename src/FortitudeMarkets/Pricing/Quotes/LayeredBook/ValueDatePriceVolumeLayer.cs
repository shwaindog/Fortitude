// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class ValueDatePriceVolumeLayer : PriceVolumeLayer, IMutableValueDatePriceVolumeLayer
{
    public ValueDatePriceVolumeLayer() => ValueDate = DateTimeConstants.UnixEpoch;

    public ValueDatePriceVolumeLayer
    (decimal price = 0m, decimal volume = 0m,
        DateTime? valueDate = null) : base(price, volume) =>
        ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;

    public ValueDatePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IValueDatePriceVolumeLayer valueDatePriceVolumeLayer)
            ValueDate = valueDatePriceVolumeLayer.ValueDate;
        else
            ValueDate = DateTimeConstants.UnixEpoch;
    }

    protected string ValueDatePriceVolumeLayerToStringMembers => $"{PriceVolumeLayerToStringMembers}, {nameof(ValueDate)}: {ValueDate}";

    [JsonIgnore] public override LayerType LayerType => LayerType.ValueDatePriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalValueDateFlags | base.SupportsLayerFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get => base.IsEmpty && ValueDate == DateTimeConstants.UnixEpoch;
        set
        {
            if (!value) return;
            ValueDate = DateTimeConstants.UnixEpoch;

            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        base.StateReset();
        ValueDate = DateTimeConstants.UnixEpoch;
    }

    public override IPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IValueDatePriceVolumeLayer sourceSourcePriceVolumeLayer) ValueDate = sourceSourcePriceVolumeLayer.ValueDate;
        return this;
    }

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => ((IValueDatePriceVolumeLayer)this).Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => (IValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    public override IPriceVolumeLayer Clone() => Recycler?.Borrow<ValueDatePriceVolumeLayer>().CopyFrom(this) ?? new ValueDatePriceVolumeLayer(this);

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
