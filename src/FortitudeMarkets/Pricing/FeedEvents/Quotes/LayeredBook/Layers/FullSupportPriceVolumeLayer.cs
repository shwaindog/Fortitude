// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public class FullSupportPriceVolumeLayer : OrdersPriceVolumeLayer, IMutableFullSupportPriceVolumeLayer
{
    public FullSupportPriceVolumeLayer() : base(LayerType.OrdersFullPriceVolume) => ValueDate = DateTime.MinValue;

    public FullSupportPriceVolumeLayer
    (decimal price = 0m, decimal volume = 0m,
        DateTime? valueDate = null, string? sourceName = null, bool executable = false,
        uint quoteRef = 0u, uint ordersCount = 0, decimal internalVolume = 0)
        : base(LayerType.OrdersFullPriceVolume, price, volume, ordersCount, internalVolume)
    {
        SourceName = sourceName;
        Executable = executable;

        SourceQuoteReference = quoteRef;

        ValueDate = valueDate ?? DateTime.MinValue;
    }

    public FullSupportPriceVolumeLayer(IPriceVolumeLayer toClone)
        : base(toClone, LayerType.FullSupportPriceVolume)
    {
        if (toClone is IFullSupportPriceVolumeLayer fullSupportPriceVolumeLayer)
        {
            SourceName = fullSupportPriceVolumeLayer.SourceName;
            Executable = fullSupportPriceVolumeLayer.Executable;

            SourceQuoteReference = fullSupportPriceVolumeLayer.SourceQuoteReference;

            ValueDate = fullSupportPriceVolumeLayer.ValueDate;
        }
        else if (toClone is ISourceQuoteRefPriceVolumeLayer srcQtRefPriceVolumeLayer)
        {
            SourceName = srcQtRefPriceVolumeLayer.SourceName;
            Executable = srcQtRefPriceVolumeLayer.Executable;

            SourceQuoteReference = srcQtRefPriceVolumeLayer.SourceQuoteReference;
        }
        else if (toClone is ISourcePriceVolumeLayer srcPriceVolumeLayer)
        {
            SourceName = srcPriceVolumeLayer.SourceName;
            Executable = srcPriceVolumeLayer.Executable;
        }
        else if (toClone is IValueDatePriceVolumeLayer valueDatePvLayer)
        {
            ValueDate = valueDatePvLayer.ValueDate;
        }
    }


    protected string FullSupportPriceVolumeLayerToStringMembers =>
        $"{OrdersCountPriceVolumeLayerToStringMembers}, {nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: {ValueDate}, {JustOrdersToString}";

    [JsonIgnore] public override LayerType LayerType => LayerType.FullSupportPriceVolume;

    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionalFullSupportLayerFlags | base.SupportsLayerFlags;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SourceName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public override bool IsEmpty
    {
        get =>
            base.IsEmpty && SourceName == null && !Executable
         && ValueDate == DateTime.MinValue && SourceQuoteReference == 0;
        set
        {
            if (!value) return;

            SourceName = null;
            Executable = false;
            ValueDate  = DateTime.MinValue;

            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    IMutableFullSupportPriceVolumeLayer ITrackableReset<IMutableFullSupportPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableFullSupportPriceVolumeLayer IMutableFullSupportPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IMutableSourcePriceVolumeLayer ITrackableReset<IMutableSourcePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IMutableSourceQuoteRefPriceVolumeLayer ITrackableReset<IMutableSourceQuoteRefPriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    IMutableValueDatePriceVolumeLayer ITrackableReset<IMutableValueDatePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override FullSupportPriceVolumeLayer ResetWithTracking()
    {
        SourceName = null;
        Executable = false;
        ValueDate  = DateTime.MinValue;

        SourceQuoteReference = 0;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        base.StateReset();
        SourceName = null;
        Executable = false;
        ValueDate  = DateTime.MinValue;

        SourceQuoteReference = 0;
    }

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => Clone();

    IFullSupportPriceVolumeLayer ICloneable<IFullSupportPriceVolumeLayer>.Clone() => Clone();

    IFullSupportPriceVolumeLayer IFullSupportPriceVolumeLayer.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => Clone();

    IMutableFullSupportPriceVolumeLayer
        ICloneable<IMutableFullSupportPriceVolumeLayer>.Clone() =>
        Clone();

    IMutableFullSupportPriceVolumeLayer IMutableFullSupportPriceVolumeLayer.
        Clone() =>
        Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => Clone();

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is IFullSupportPriceVolumeLayer fullSupportPvLayer)) return false;

        var baseSame       = base.AreEquivalent(other, exactTypes);
        var sourceNameSame = SourceName == fullSupportPvLayer.SourceName;
        var executableSame = Executable == fullSupportPvLayer.Executable;
        var valueDateSame  = ValueDate == fullSupportPvLayer.ValueDate;

        var sourceQuoteReferenceSame = SourceQuoteReference == fullSupportPvLayer.SourceQuoteReference;

        var allAreSame = baseSame && sourceNameSame && executableSame && sourceQuoteReferenceSame && valueDateSame;
        return allAreSame;
    }

    public override FullSupportPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IFullSupportPriceVolumeLayer fullSupportPriceVolumeLayer)
        {
            SourceName = fullSupportPriceVolumeLayer.SourceName;
            Executable = fullSupportPriceVolumeLayer.Executable;

            SourceQuoteReference = fullSupportPriceVolumeLayer.SourceQuoteReference;

            ValueDate = fullSupportPriceVolumeLayer.ValueDate;
        }
        else if (source is ISourceQuoteRefPriceVolumeLayer srcQtRefPriceVolumeLayer)
        {
            SourceName = srcQtRefPriceVolumeLayer.SourceName;
            Executable = srcQtRefPriceVolumeLayer.Executable;

            SourceQuoteReference = srcQtRefPriceVolumeLayer.SourceQuoteReference;
        }
        else if (source is ISourcePriceVolumeLayer srcPriceVolumeLayer)
        {
            SourceName = srcPriceVolumeLayer.SourceName;
            Executable = srcPriceVolumeLayer.Executable;
        }
        else if (source is IValueDatePriceVolumeLayer valueDatePvLayer)
        {
            ValueDate = valueDatePvLayer.ValueDate;
        }

        return this;
    }

    public override FullSupportPriceVolumeLayer Clone() =>
        Recycler?.Borrow<FullSupportPriceVolumeLayer>().CopyFrom(this) ??
        new FullSupportPriceVolumeLayer(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (SourceName?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SourceQuoteReference;
            return hashCode;
        }
    }

    public override string ToString() => $"{nameof(FullSupportPriceVolumeLayer)}{{{FullSupportPriceVolumeLayerToStringMembers}}}";
}
