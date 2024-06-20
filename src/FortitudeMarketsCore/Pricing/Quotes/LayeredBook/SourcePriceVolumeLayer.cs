// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

public class SourcePriceVolumeLayer : PriceVolumeLayer, IMutableSourcePriceVolumeLayer
{
    public SourcePriceVolumeLayer() { }

    public SourcePriceVolumeLayer
    (decimal price = 0m, decimal volume = 0m,
        string? sourceName = null, bool executable = false)
        : base(price, volume)
    {
        SourceName = sourceName;
        Executable = executable;
    }

    public SourcePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is ISourcePriceVolumeLayer srcPvLayer)
        {
            SourceName = srcPvLayer.SourceName;
            Executable = srcPvLayer.Executable;
        }
    }

    public override LayerType  LayerType          => LayerType.SourcePriceVolume;
    public override LayerFlags SupportsLayerFlags => LayerFlags.SourceName | LayerFlags.Executable | base.SupportsLayerFlags;

    public string? SourceName { get; set; }

    public bool Executable { get; set; }

    public override bool IsEmpty
    {
        get => base.IsEmpty && SourceName == null && !Executable;
        set
        {
            if (!value) return;
            Executable   = false;
            SourceName   = null;
            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        base.StateReset();
        SourceName = null;
        Executable = false;
    }

    public override IPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISourcePriceVolumeLayer sourceSourcePriceVolumeLayer)
        {
            SourceName = sourceSourcePriceVolumeLayer.SourceName;
            Executable = sourceSourcePriceVolumeLayer.Executable;
        }

        return this;
    }

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => (ISourcePriceVolumeLayer)Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => (IMutableSourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => (ISourcePriceVolumeLayer)Clone();

    public override IPriceVolumeLayer Clone() => Recycler?.Borrow<SourcePriceVolumeLayer>().CopyFrom(this) ?? new SourcePriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourcePriceVolumeLayer otherISourcePvLayer)) return false;

        var baseSame       = base.AreEquivalent(other, exactTypes);
        var executableSame = Executable == otherISourcePvLayer.Executable;
        var sourceNameSame = string.Equals(SourceName, otherISourcePvLayer.SourceName);

        return baseSame && executableSame && sourceNameSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        return AreEquivalent((IPriceVolumeLayer?)obj, true);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashcode = (base.GetHashCode() * 397) ^ (SourceName?.GetHashCode() ?? 0);
            hashcode = (hashcode * 397) ^ Executable.GetHashCode();
            return hashcode;
        }
    }

    public override string ToString() =>
        $"SourcePriceVolumeLayer{{{nameof(Price)}: {Price:N5}, {nameof(Volume)}: " +
        $"{Volume:N2}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable} }}";
}
