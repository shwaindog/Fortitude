// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

public class SourceQuoteRefTraderValueDatePriceVolumeLayer : TraderPriceVolumeLayer,
    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
{
    public SourceQuoteRefTraderValueDatePriceVolumeLayer() => ValueDate = DateTimeConstants.UnixEpoch;

    public SourceQuoteRefTraderValueDatePriceVolumeLayer(decimal price = 0m, decimal volume = 0m,
        DateTime? valueDate = null, string? sourceName = null, bool executable = false,
        uint quoteRef = 0u) : base(price, volume)
    {
        SourceName = sourceName;
        Executable = executable;

        SourceQuoteReference = quoteRef;

        ValueDate = valueDate ?? DateTimeConstants.UnixEpoch;
    }

    public SourceQuoteRefTraderValueDatePriceVolumeLayer(IPriceVolumeLayer toClone)
        : base(toClone)
    {
        if (toClone is ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer)
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

    public override LayerType LayerType => LayerType.SourceQuoteRefTraderValueDatePriceVolume;

    public override LayerFlags SupportsLayerFlags =>
        LayerFlags.SourceQuoteReference | LayerFlags.SourceName
                                        | LayerFlags.Executable | LayerFlags.ValueDate | base.SupportsLayerFlags;

    public string?  SourceName { get; set; }
    public bool     Executable { get; set; }
    public DateTime ValueDate  { get; set; }

    public uint SourceQuoteReference { get; set; }

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

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPriceVolumeLayer)
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

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => (IMutableSourcePriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer ICloneable<IValueDatePriceVolumeLayer>.Clone() => (IValueDatePriceVolumeLayer)Clone();

    IValueDatePriceVolumeLayer IValueDatePriceVolumeLayer.Clone() => (IValueDatePriceVolumeLayer)Clone();

    ISourceQuoteRefTraderValueDatePriceVolumeLayer ICloneable<ISourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone() =>
        (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    ISourceQuoteRefTraderValueDatePriceVolumeLayer ISourceQuoteRefTraderValueDatePriceVolumeLayer.Clone() =>
        (ISourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer ICloneable<IMutableValueDatePriceVolumeLayer>.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IMutableValueDatePriceVolumeLayer IMutableValueDatePriceVolumeLayer.Clone() => (IMutableValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer
        ICloneable<IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer.
        Clone() =>
        (IMutableSourceQuoteRefTraderValueDatePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => (ISourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => (ISourcePriceVolumeLayer)Clone();

    public override IPriceVolumeLayer Clone() =>
        Recycler?.Borrow<SourceQuoteRefTraderValueDatePriceVolumeLayer>().CopyFrom(this) ??
        new SourceQuoteRefTraderValueDatePriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourceQuoteRefTraderValueDatePriceVolumeLayer srcQtRefTrdrVlDtPvLayer)) return false;

        var baseSame                 = base.AreEquivalent(other, exactTypes);
        var sourceNameSame           = SourceName == srcQtRefTrdrVlDtPvLayer.SourceName;
        var executableSame           = Executable == srcQtRefTrdrVlDtPvLayer.Executable;
        var sourceQuoteReferenceSame = SourceQuoteReference == srcQtRefTrdrVlDtPvLayer.SourceQuoteReference;
        var valueDateSame            = ValueDate == srcQtRefTrdrVlDtPvLayer.ValueDate;

        var allAreSame = baseSame && sourceNameSame && executableSame && sourceQuoteReferenceSame && valueDateSame;
        // if (!allAreSame) Debugger.Break();
        return allAreSame;
    }

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
        $"SourceQuoteRefTraderValueDatePriceVolumeLayer {{{nameof(Price)}: {Price:N5}, " +
        $"{nameof(Volume)}: {Volume:N2}, {nameof(TraderDetails)}:[ " +
        $"{string.Join(",", TraderDetails)}], {nameof(Count)}: {Count}, " +
        $"{nameof(SourceName)}: {SourceName}, {nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}, {nameof(ValueDate)}: " +
        $"{ValueDate} }}";
}
