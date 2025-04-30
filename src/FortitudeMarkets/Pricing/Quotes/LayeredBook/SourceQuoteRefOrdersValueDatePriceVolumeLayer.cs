// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class SourceQuoteRefOrdersValueDatePriceVolumeLayer : OrdersPriceVolumeLayer,
    IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer
{
    public SourceQuoteRefOrdersValueDatePriceVolumeLayer() : base(LayerType.OrdersFullPriceVolume) => ValueDate = DateTimeConstants.UnixEpoch;

    public SourceQuoteRefOrdersValueDatePriceVolumeLayer
    (decimal price = 0m, decimal volume = 0m,
        DateTime? valueDate = null, string? sourceName = null, bool executable = false,
        uint quoteRef = 0u, uint ordersCount = 0, decimal internalVolume = 0) : base(LayerType.OrdersFullPriceVolume, price, volume, ordersCount
                                                                                   , internalVolume)
    {
        SourceName = sourceName;
        Executable = executable;

        SourceQuoteReference = quoteRef;

        ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;
    }

    public SourceQuoteRefOrdersValueDatePriceVolumeLayer(IPriceVolumeLayer toClone)
        : base(toClone, LayerType.SourceQuoteRefOrdersValueDatePriceVolume)
    {
        if (toClone is ISourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer)
        {
            SourceName = srcQtRefTrdrVlDtPriceVolumeLayer.SourceName;
            Executable = srcQtRefTrdrVlDtPriceVolumeLayer.Executable;

            SourceQuoteReference = srcQtRefTrdrVlDtPriceVolumeLayer.SourceQuoteReference;

            ValueDate = srcQtRefTrdrVlDtPriceVolumeLayer.ValueDate;
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


    protected string SourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers =>
        $"{OrdersCountPriceVolumeLayerToStringMembers}, {nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: {ValueDate}, {JustOrdersToString}";

    [JsonIgnore] public override LayerType LayerType => LayerType.SourceQuoteRefOrdersValueDatePriceVolume;

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
         && ValueDate == DateTimeConstants.UnixEpoch && SourceQuoteReference == 0;
        set
        {
            if (!value) return;

            SourceName = null;
            Executable = false;
            ValueDate  = DateTimeConstants.UnixEpoch;

            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        base.StateReset();
        SourceName           = null;
        Executable           = false;
        ValueDate            = DateTimeConstants.UnixEpoch;
        SourceQuoteReference = 0;
    }

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => Clone();

    ISourceQuoteRefOrdersValueDatePriceVolumeLayer ICloneable<ISourceQuoteRefOrdersValueDatePriceVolumeLayer>.Clone() => Clone();

    ISourceQuoteRefOrdersValueDatePriceVolumeLayer ISourceQuoteRefOrdersValueDatePriceVolumeLayer.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() => Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => Clone();

    IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer
        ICloneable<IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer>.Clone() =>
        Clone();

    IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer IMutableSourceQuoteRefOrdersValueDatePriceVolumeLayer.
        Clone() =>
        Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => Clone();

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvLayer)) return false;

        var baseSame                 = base.AreEquivalent(other, exactTypes);
        var sourceNameSame           = SourceName == srcQtRefTrdrVlDtPvLayer.SourceName;
        var executableSame           = Executable == srcQtRefTrdrVlDtPvLayer.Executable;
        var sourceQuoteReferenceSame = SourceQuoteReference == srcQtRefTrdrVlDtPvLayer.SourceQuoteReference;
        var valueDateSame            = ValueDate == srcQtRefTrdrVlDtPvLayer.ValueDate;

        var allAreSame = baseSame && sourceNameSame && executableSame && sourceQuoteReferenceSame && valueDateSame;
        return allAreSame;
    }

    public override SourceQuoteRefOrdersValueDatePriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISourceQuoteRefOrdersValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer)
        {
            SourceName = srcQtRefTrdrVlDtPriceVolumeLayer.SourceName;
            Executable = srcQtRefTrdrVlDtPriceVolumeLayer.Executable;

            SourceQuoteReference = srcQtRefTrdrVlDtPriceVolumeLayer.SourceQuoteReference;

            ValueDate = srcQtRefTrdrVlDtPriceVolumeLayer.ValueDate;
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

    public override SourceQuoteRefOrdersValueDatePriceVolumeLayer Clone() =>
        Recycler?.Borrow<SourceQuoteRefOrdersValueDatePriceVolumeLayer>().CopyFrom(this) ??
        new SourceQuoteRefOrdersValueDatePriceVolumeLayer(this);

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

    public override string ToString() =>
        $"{nameof(SourceQuoteRefOrdersValueDatePriceVolumeLayer)}{{{SourceQuoteRefTraderValueDatePriceVolumeLayerToStringMembers}}}";
}
