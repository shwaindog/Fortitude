// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public class SourcePriceVolumeLayer : PriceVolumeLayer, IMutableSourcePriceVolumeLayer
{
    protected LayerBooleanValues BooleanValues;
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

    protected string SourcePriceVolumeLayerToStringMembers =>
        $"{PriceVolumeLayerToStringMembers}, {nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}";

    [JsonIgnore] public override LayerType  LayerType          => LayerType.SourcePriceVolume;
    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionSourceLayerFlags | base.SupportsLayerFlags;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable
    {
        get => BooleanValues.HasExecutable();
        set
        {
            if (value)
                BooleanValues |= LayerBooleanValues.Executable;

            else if (Executable) BooleanValues ^= LayerBooleanValues.Executable;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
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

    IMutableSourcePriceVolumeLayer ITrackableReset<IMutableSourcePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override SourcePriceVolumeLayer ResetWithTracking()
    {
        SourceName = null;
        Executable = false;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        base.StateReset();
        SourceName = null;
        Executable = false;
    }

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => Clone();

    public override SourcePriceVolumeLayer Clone() =>
        Recycler?.Borrow<SourcePriceVolumeLayer>().CopyFrom(this, QuoteInstantBehaviorFlags.DisableUpgradeLayer) ?? new SourcePriceVolumeLayer(this);

    public override SourcePriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);
        if (source is ISourcePriceVolumeLayer sourceSourcePriceVolumeLayer)
        {
            SourceName = sourceSourcePriceVolumeLayer.SourceName;
            Executable = sourceSourcePriceVolumeLayer.Executable;
        }

        return this;
    }

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

    public override string ToString() => $"{nameof(SourcePriceVolumeLayer)}{{{SourcePriceVolumeLayerToStringMembers} }}";
}
