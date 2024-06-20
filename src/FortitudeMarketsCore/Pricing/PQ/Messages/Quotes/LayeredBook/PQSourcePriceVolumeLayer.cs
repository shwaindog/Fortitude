// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

public interface IPQSourcePriceVolumeLayer : IMutableSourcePriceVolumeLayer, IPQPriceVolumeLayer,
    IPQSupportsStringUpdates<IPriceVolumeLayer>, ISupportsPQNameIdLookupGenerator
{
    ushort SourceId            { get; set; }
    bool   IsSourceNameUpdated { get; set; }
    bool   IsExecutableUpdated { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourcePriceVolumeLayer Clone();
}

public class PQSourcePriceVolumeLayer : PQPriceVolumeLayer, IPQSourcePriceVolumeLayer
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQSourcePriceVolumeLayer));

    protected LayerBooleanFlags        LayerBooleanFlags;
    private   IPQNameIdLookupGenerator nameIdLookup = null!;
    private   ushort                   sourceId;

    public PQSourcePriceVolumeLayer
    (IPQNameIdLookupGenerator sourceIdToNameIdLookup, decimal price = 0m, decimal volume = 0m,
        string? sourceName = null, bool executable = false)
        : base(price, volume)
    {
        NameIdLookup = sourceIdToNameIdLookup;
        SourceName   = sourceName;
        Executable   = executable;
    }

    public PQSourcePriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator nameIdLookupGenerator) : base(toClone)
    {
        NameIdLookup = nameIdLookupGenerator;
        if (toClone is IPQSourcePriceVolumeLayer pqSourcePvToClone)
        {
            SourceId            = (ushort)NameIdLookup.GetOrAddId(pqSourcePvToClone.SourceName);
            Executable          = pqSourcePvToClone.Executable;
            IsSourceNameUpdated = pqSourcePvToClone.IsSourceNameUpdated;
            IsExecutableUpdated = pqSourcePvToClone.IsExecutableUpdated;
        }
        else if (toClone is ISourcePriceVolumeLayer sourcePvToClone)
        {
            SourceName = sourcePvToClone.SourceName;
            Executable = sourcePvToClone.Executable;
        }

        SetFlagsSame(toClone);
    }

    protected string PQSourcePriceVolumeLayerToStringMembers =>
        $"{PQPriceVolumeLayerToStringMembers}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable}";

    public override LayerType  LayerType          => LayerType.SourcePriceVolume;
    public override LayerFlags SupportsLayerFlags => LayerFlags.SourceName | LayerFlags.Executable | base.SupportsLayerFlags;

    public ushort SourceId
    {
        get => sourceId;
        set
        {
            if (SourceId == value) return;
            IsSourceNameUpdated = true;
            sourceId            = value;
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
        get => NameIdLookup[SourceId];
        set
        {
            var convertedSourceId = NameIdLookup.GetOrAddId(value);
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

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;
            string? cacheSourceName           = null;
            if (sourceId > 0) cacheSourceName = SourceName;
            nameIdLookup = value;
            if (sourceId > 0) sourceId = (ushort)nameIdLookup.GetOrAddId(cacheSourceName);
        }
    }

    public override bool IsEmpty
    {
        get => base.IsEmpty && SourceId == 0 && !Executable;
        set
        {
            if (!value) return;
            SourceId     = 0;
            Executable   = false;
            base.IsEmpty = true;
        }
    }

    public override void StateReset()
    {
        SourceId   = 0;
        Executable = false;
        base.StateReset();
    }

    public override bool HasUpdates
    {
        get => base.HasUpdates || NameIdLookup.HasUpdates;
        set
        {
            NameIdLookup.HasUpdates = value;
            base.HasUpdates         = value;
        }
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (!updatedOnly || IsSourceNameUpdated) yield return new PQFieldUpdate(PQFieldKeys.LayerSourceIdOffset, SourceId);

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

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFieldKeys.LayerNameDictionaryUpsertCommand) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public override IPriceVolumeLayer CopyFrom
    (IPriceVolumeLayer source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var pqspvl = source as IPQSourcePriceVolumeLayer;
        if (source is ISourcePriceVolumeLayer spvl && pqspvl == null)
        {
            if (SourceName != spvl.SourceName) SourceId = (ushort)NameIdLookup.GetOrAddId(spvl.SourceName);
            Executable = spvl.Executable;
        }
        else if (pqspvl != null)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pqspvl.IsSourceNameUpdated || isFullReplace) SourceId   = (ushort)NameIdLookup.GetOrAddId(pqspvl.SourceName);
            if (pqspvl.IsExecutableUpdated || isFullReplace) Executable = pqspvl.Executable;

            if (isFullReplace) SetFlagsSame(pqspvl);
        }

        return this;
    }

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.Clone() => (IPQSourcePriceVolumeLayer)Clone();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.Clone() => (IMutableSourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ICloneable<ISourcePriceVolumeLayer>.Clone() => (ISourcePriceVolumeLayer)Clone();

    ISourcePriceVolumeLayer ISourcePriceVolumeLayer.Clone() => (ISourcePriceVolumeLayer)Clone();

    public override IPQPriceVolumeLayer Clone() => new PQSourcePriceVolumeLayer(this, NameIdLookup);

    public override bool AreEquivalent(IPriceVolumeLayer? other, bool exactTypes = false)
    {
        if (!(other is ISourcePriceVolumeLayer sourceOther)) return false;
        var baseSame             = base.AreEquivalent(other, exactTypes);
        var sourceExecutableSame = Executable == sourceOther.Executable;
        var sourceNameSame       = SourceName == sourceOther.SourceName;

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

    public override string ToString() => $"{GetType().Name}({PQSourcePriceVolumeLayerToStringMembers})";
}
