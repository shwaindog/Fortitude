// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class SourceQuoteRefPriceVolumeLayer : SourcePriceVolumeLayer, IMutableSourceQuoteRefPriceVolumeLayer
{
    public SourceQuoteRefPriceVolumeLayer() { }

    public SourceQuoteRefPriceVolumeLayer
    (decimal price = 0m, decimal volume = 0m,
        string? sourceName = null, bool executable = false, uint quoteReference = 0u)
        : base(price, volume, sourceName, executable) =>
        SourceQuoteReference = quoteReference;

    public SourceQuoteRefPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPvLayer)
            SourceQuoteReference = srcQtRefPvLayer.SourceQuoteReference;
    }

    public override LayerType  LayerType          => LayerType.SourceQuoteRefPriceVolume;
    public override LayerFlags SupportsLayerFlags => LayerFlags.SourceQuoteReference | base.SupportsLayerFlags;

    public uint SourceQuoteReference { get; set; }
    public override bool IsEmpty
    {
        get => base.IsEmpty && SourceQuoteReference == 0u;
        set
        {
            if (!value) return;
            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        base.StateReset();
        SourceQuoteReference = 0;
    }

    public override IPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISourceQuoteRefPriceVolumeLayer sourceSourcePriceVolumeLayer)
            SourceQuoteReference = sourceSourcePriceVolumeLayer.SourceQuoteReference;
        return this;
    }

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    public override IPriceVolumeLayer Clone() =>
        Recycler?.Borrow<SourceQuoteRefPriceVolumeLayer>().CopyFrom(this) ?? new SourceQuoteRefPriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourceQuoteRefPriceVolumeLayer otherISourcePvLayer)) return false;
        var baseSame     = base.AreEquivalent(otherISourcePvLayer, exactTypes);
        var quoteRefSame = SourceQuoteReference == otherISourcePvLayer.SourceQuoteReference;

        return baseSame && quoteRefSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ (int)SourceQuoteReference;
        }
    }

    public override string ToString() =>
        $"SourceQuoteRefPriceVolumeLayer{{{nameof(Price)}: {Price:N5}, {nameof(Volume)}: " +
        $"{Volume:N2} , {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0} }}";
}
