// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQSourceQuoteRefPriceVolumeLayer : IMutableSourceQuoteRefPriceVolumeLayer,
    IPQSourcePriceVolumeLayer

{
    bool IsSourceQuoteReferenceUpdated { get; set; }

    new IPQSourceQuoteRefPriceVolumeLayer Clone();
}

public class PQSourceQuoteRefPriceVolumeLayer : PQSourcePriceVolumeLayer, IPQSourceQuoteRefPriceVolumeLayer
{
    private uint sourceQuoteReference;

    public PQSourceQuoteRefPriceVolumeLayer
    (IPQNameIdLookupGenerator sourceIdToNameIdLookup, decimal price = 0m, decimal volume = 0m,
        string? sourceName = null, bool executable = false, uint quoteRef = 0u)
        : base(sourceIdToNameIdLookup, price, volume, sourceName, executable) =>
        SourceQuoteReference = quoteRef;

    public PQSourceQuoteRefPriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator nameIdLookupGenerator) : base(toClone
   , nameIdLookupGenerator)
    {
        if (toClone is ISourceQuoteRefPriceVolumeLayer sourceQtRefPvLayer)
            SourceQuoteReference = sourceQtRefPvLayer.SourceQuoteReference;
        SetFlagsSame(toClone);
    }

    protected string PQSourceQuoteRefPriceVolumeLayerToStringMembers =>
        $"{PQSourcePriceVolumeLayerToStringMembers}, {nameof(SourceQuoteReference)}: {SourceQuoteReference:N0}";

    public override LayerType  LayerType          => LayerType.SourceQuoteRefPriceVolume;
    public override LayerFlags SupportsLayerFlags => LayerFlags.SourceQuoteReference | base.SupportsLayerFlags;

    public uint SourceQuoteReference
    {
        get => sourceQuoteReference;
        set
        {
            if (sourceQuoteReference == value) return;
            IsSourceQuoteReferenceUpdated = true;
            sourceQuoteReference          = value;
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

    public override bool IsEmpty
    {
        get => base.IsEmpty && SourceQuoteReference == 0;
        set
        {
            if (!value) return;
            SourceQuoteReference = 0;

            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        SourceQuoteReference = 0;
        base.StateReset();
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

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsSourceQuoteReferenceUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerSourceQuoteRefOffset, SourceQuoteReference);
    }

    public override IPriceVolumeLayer CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var pqSourcePvl   = source as IPQSourceQuoteRefPriceVolumeLayer;
        var isFullReplace = copyMergeFlags.HasFullReplace();
        if (source is ISourceQuoteRefPriceVolumeLayer sqrpvl && pqSourcePvl == null)
        {
            SourceQuoteReference = sqrpvl.SourceQuoteReference;
        }
        else if (pqSourcePvl != null)
        {
            if (pqSourcePvl.IsSourceNameUpdated || isFullReplace)
                SourceQuoteReference = pqSourcePvl.SourceQuoteReference;
            if (isFullReplace) SetFlagsSame(pqSourcePvl);
        }

        return this;
    }

    IPQSourceQuoteRefPriceVolumeLayer IPQSourceQuoteRefPriceVolumeLayer.Clone() => (IPQSourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ICloneable<ISourceQuoteRefPriceVolumeLayer>.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    ISourceQuoteRefPriceVolumeLayer ISourceQuoteRefPriceVolumeLayer.Clone() => (ISourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>.Clone() =>
        (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    IMutableSourceQuoteRefPriceVolumeLayer IMutableSourceQuoteRefPriceVolumeLayer.Clone() => (IMutableSourceQuoteRefPriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourceQuoteRefPriceVolumeLayer(this, NameIdLookup);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourceQuoteRefPriceVolumeLayer sourceQtRefOther)) return false;
        var baseSame           = base.AreEquivalent(other, exactTypes);
        var sourceQuoteRefSame = SourceQuoteReference == sourceQtRefOther.SourceQuoteReference;

        return baseSame && sourceQuoteRefSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourcePriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            return (base.GetHashCode() * 397) ^ sourceQuoteReference.GetHashCode();
        }
    }

    public override string ToString() => $"{GetType().Name}({PQSourceQuoteRefPriceVolumeLayerToStringMembers})";
}
