#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public class PQSourceQuoteRefPriceVolumeLayer : PQSourcePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer
{
    private uint sourceQuoteReference;

    public PQSourceQuoteRefPriceVolumeLayer(decimal price = 0m, decimal volume = 0m,
        IPQNameIdLookupGenerator? sourceIdToNameIdLookup = null, string? sourceName = null,
        bool executable = false, uint quoteRef = 0u)
        : base(price, volume, sourceIdToNameIdLookup, sourceName, executable) =>
        SourceQuoteReference = quoteRef;

    public PQSourceQuoteRefPriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is ISourceQuoteRefPriceVolumeLayer sourceQtRefPvLayer)
            SourceQuoteReference = sourceQtRefPvLayer.SourceQuoteReference;
        SetFlagsSame(toClone);
    }

    public uint SourceQuoteReference
    {
        get => sourceQuoteReference;
        set
        {
            if (sourceQuoteReference == value) return;
            IsSourceQuoteReferenceUpdated = true;
            sourceQuoteReference = value;
        }
    }

    public bool IsSourceQuoteReferenceUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.SourceQuoteRefUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.SourceQuoteRefUpdatedFlag;
            else if (IsSourceQuoteReferenceUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.SourceQuoteRefUpdatedFlag;
        }
    }

    public override bool IsEmpty => base.IsEmpty && SourceQuoteReference == 0;

    public override void Reset()
    {
        SourceQuoteReference = 0;
        base.Reset();
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id < PQFieldKeys.LayerSourceQuoteRefOffset || pqFieldUpdate.Id >=
            PQFieldKeys.LayerSourceQuoteRefOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
            return base.UpdateField(pqFieldUpdate);
        SourceQuoteReference = pqFieldUpdate.Value;
        return 0;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                     quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset, SourceQuoteReference);
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source);
        var pqSourcePvl = source as IPQSourceQuoteRefPriceVolumeLayer;
        if (source is ISourceQuoteRefPriceVolumeLayer sqrpvl && pqSourcePvl == null)
            SourceQuoteReference = sqrpvl.SourceQuoteReference;
        else if (pqSourcePvl != null)
            if (pqSourcePvl.IsSourceNameUpdated)
                SourceQuoteReference = pqSourcePvl.SourceQuoteReference;
        return this;
    }

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() =>
        (IPQSourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() =>
        (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourceQuoteRefPriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourceQuoteRefPriceVolumeLayer sourceQtRefOther)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var sourceQuoteRefSame = SourceQuoteReference == sourceQtRefOther.SourceQuoteReference;

        return baseSame && sourceQuoteRefSame;
    }

    public override bool Equals(object? obj) =>
        ReferenceEquals(this, obj) || AreEquivalent((ISourcePriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ sourceQuoteReference.GetHashCode();
        }
    }

    public override string ToString() =>
        $"PQSourceQuoteRefPriceVolumeLayer {{ {nameof(Price)}: {Price:N5}, " +
        $"{nameof(Volume)}: {Volume:N2}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference:N0} }}";
}
