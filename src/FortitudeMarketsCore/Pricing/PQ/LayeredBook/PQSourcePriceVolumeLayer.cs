#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

public class PQSourcePriceVolumeLayer : PQPriceVolumeLayer, IPQSourcePriceVolumeLayer
{
    protected LayerBooleanFlags LayerBooleanFlags;
    private ushort sourceId;

    public PQSourcePriceVolumeLayer(decimal price = 0m, decimal volume = 0m,
        IPQNameIdLookupGenerator? sourceIdToNameIdLookup = null, string? sourceName = null,
        bool executable = false)
        : base(price, volume)
    {
        SourceNameIdLookup = sourceIdToNameIdLookup ??
                             new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                 PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        SourceName = sourceName;
        Executable = executable;
    }

    public PQSourcePriceVolumeLayer(IPriceVolumeLayer toClone) : base(toClone)
    {
        if (toClone is IPQSourcePriceVolumeLayer pqSourcePvToClone)
        {
            SourceNameIdLookup = pqSourcePvToClone.SourceNameIdLookup ??
                                 new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                                     PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            IsSourceNameUpdated = pqSourcePvToClone.IsSourceNameUpdated;
            Executable = pqSourcePvToClone.Executable;
            SourceId = pqSourcePvToClone.SourceId;
        }
        else if (toClone is ISourcePriceVolumeLayer sourcePvToCloen)
        {
            SourceNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
            SourceName = sourcePvToCloen.SourceName;
            Executable = sourcePvToCloen.Executable;
        }
        else
        {
            SourceNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                PQFieldFlags.SourceNameIdLookupSubDictionaryKey);
        }

        SetFlagsSame(toClone);
    }

    public ushort SourceId
    {
        get => sourceId;
        set
        {
            if (SourceId == value) return;
            IsSourceNameUpdated = true;
            sourceId = value;
        }
    }

    public bool IsSourceNameUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.SourceNameUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.SourceNameUpdatedFlag;
            else if (IsSourceNameUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.SourceNameUpdatedFlag;
        }
    }

    public string? SourceName
    {
        get => SourceNameIdLookup[SourceId];
        set
        {
            var convertedSourceId = SourceNameIdLookup.GetOrAddId(value);
            if (convertedSourceId <= 0 && value != null)
                throw new Exception("Error attempted to set the Source Name to something " +
                                    "not defined in the source name to Id table.");
            SourceId = (ushort)convertedSourceId;
        }
    }

    public bool Executable
    {
        get => (LayerBooleanFlags & LayerBooleanFlags.IsExecutableFlag) != 0;
        set
        {
            if (Executable == value) return;
            IsExecutableUpdated = true;
            if (value)
                LayerBooleanFlags |= LayerBooleanFlags.IsExecutableFlag;
            else if (Executable) LayerBooleanFlags ^= LayerBooleanFlags.IsExecutableFlag;
        }
    }

    public bool IsExecutableUpdated
    {
        get => (UpdatedFlags & LayerFieldUpdatedFlags.ExecutableUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= LayerFieldUpdatedFlags.ExecutableUpdatedFlag;
            else if (IsExecutableUpdated) UpdatedFlags ^= LayerFieldUpdatedFlags.ExecutableUpdatedFlag;
        }
    }

    public IPQNameIdLookupGenerator SourceNameIdLookup { get; set; }

    public override bool IsEmpty => base.IsEmpty && SourceId == 0 && !Executable;

    public override void Reset()
    {
        SourceId = 0;
        Executable = false;
        base.Reset();
    }

    public override bool HasUpdates
    {
        get => base.HasUpdates || SourceNameIdLookup.HasUpdates;
        set
        {
            SourceNameIdLookup.HasUpdates = value;
            base.HasUpdates = value;
        }
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, updateStyle,
                     quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsSourceNameUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset, SourceId);
        if (!updatedOnly || IsExecutableUpdated)
            yield return new PQFieldUpdate(PQFieldKeys.LayerBooleanFlagsOffset,
                Executable ? PQFieldFlags.LayerExecutableFlag : 0);
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        if (pqFieldUpdate.Id >= PQFieldKeys.LayerSourceIdOffset && pqFieldUpdate.Id <
            PQFieldKeys.LayerSourceIdOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            SourceId = (ushort)pqFieldUpdate.Value;
            return 0;
        }

        if (pqFieldUpdate.Id >= PQFieldKeys.LayerBooleanFlagsOffset && pqFieldUpdate.Id <
            PQFieldKeys.LayerBooleanFlagsOffset + PQFieldKeys.SingleByteFieldIdMaxBookDepth)
        {
            Executable = (pqFieldUpdate.Value & PQFieldFlags.LayerExecutableFlag) != 0;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
    {
        if (SourceNameIdLookup != null)
            foreach (var stringUpdate in SourceNameIdLookup.GetStringUpdates(snapShotTime, updatedStyle))
                yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id != PQFieldKeys.LayerNameDictionaryUpsertCommand) return false;
        if (SourceNameIdLookup != null) return SourceNameIdLookup.UpdateFieldString(updates);
        return false;
    }

    public override void CopyFrom(IPriceVolumeLayer source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var pqspvl = source as IPQSourcePriceVolumeLayer;
        if (source is ISourcePriceVolumeLayer spvl && pqspvl == null)
        {
            if (SourceName != spvl.SourceName) SourceId = (ushort)SourceNameIdLookup[spvl.SourceName];
            Executable = spvl.Executable;
        }
        else if (pqspvl != null)
        {
            SourceNameIdLookup.CopyFrom(pqspvl.SourceNameIdLookup);
            if (pqspvl.IsSourceNameUpdated) SourceId = pqspvl.SourceId;
            if (pqspvl.IsExecutableUpdated) Executable = pqspvl.Executable;
        }
    }

    public override void EnsureRelatedItemsAreConfigured(ISourceTickerQuoteInfo? referenceInstance)
    {
        if (referenceInstance is IPQSourceTickerQuoteInfo pqSrcTkrQtInfo)
            if (ReferenceEquals(pqSrcTkrQtInfo.SourceNameIdLookup, SourceNameIdLookup))
                SourceNameIdLookup = pqSrcTkrQtInfo.SourceNameIdLookup.Clone();
        if (SourceNameIdLookup == null)
            SourceNameIdLookup = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand,
                1);
    }

    public override void EnsureRelatedItemsAreConfigured(IPQPriceVolumeLayer? referenceInstance)
    {
        if (referenceInstance is IPQSourcePriceVolumeLayer pqSrcPvLayer)
            SourceNameIdLookup = pqSrcPvLayer.SourceNameIdLookup;
    }

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.Clone() => (IPQSourcePriceVolumeLayer)Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => (IMutableSourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => (ISourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => (ISourcePriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourcePriceVolumeLayer(this);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourcePriceVolumeLayer sourceOther)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var sourceExecutableSame = Executable == sourceOther.Executable;
        var sourceNameSame = SourceName == sourceOther.SourceName;

        return baseSame && sourceExecutableSame && sourceNameSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (!(obj is PQSourcePriceVolumeLayer)) return false;
        return AreEquivalent((IPriceVolumeLayer)obj, true);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (base.GetHashCode() * 397) ^ sourceId.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQSourcePriceVolumeLayer {{ {nameof(Price)}: {Price:N5}, " +
        $"{nameof(Volume)}: {Volume:N2}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable} }}";
}
