﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

public interface IPQSourcePriceVolumeLayer : IMutableSourcePriceVolumeLayer, IPQPriceVolumeLayer,
    IPQSupportsStringUpdates, ISupportsPQNameIdLookupGenerator, ITrackableReset<IPQSourcePriceVolumeLayer>
{
    ushort SourceId            { get; set; }
    bool   IsSourceNameUpdated { get; set; }
    bool   IsExecutableUpdated { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourcePriceVolumeLayer Clone();
    new IPQSourcePriceVolumeLayer ResetWithTracking();
}

public class PQSourcePriceVolumeLayer : PQPriceVolumeLayer, IPQSourcePriceVolumeLayer
{
    protected LayerBooleanValues       BooleanValues;
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
        if (GetType() == typeof(PQSourcePriceVolumeLayer)) SequenceId = 0;
    }

    public PQSourcePriceVolumeLayer(IPriceVolumeLayer toClone, IPQNameIdLookupGenerator nameIdLookupGenerator) : base(toClone)
    {
        NameIdLookup = nameIdLookupGenerator;
        if (toClone is IPQSourcePriceVolumeLayer pqSourcePvToClone)
        {
            SourceName          = pqSourcePvToClone.SourceName;
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
        if (GetType() == typeof(PQSourcePriceVolumeLayer)) SequenceId = 0;
    }

    protected string PQSourcePriceVolumeLayerToStringMembers =>
        $"{PQPriceVolumeLayerToStringMembers}, {nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, " +
        $"{nameof(Executable)}: {Executable}";

    [JsonIgnore] public override LayerType  LayerType          => LayerType.SourcePriceVolume;
    [JsonIgnore] public override LayerFlags SupportsLayerFlags => LayerFlagsExtensions.AdditionSourceLayerFlags | base.SupportsLayerFlags;

    public QuoteLayerInstantBehaviorFlags LayerBehavior
    {
        get => BooleanValues.ExtractLayerBehaviorFlags();
        set => BooleanValues = (LayerBooleanValues)(((uint)BooleanValues & 0xFF_FF_00_00) | (uint)value);
    }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public ushort SourceId
    {
        get => sourceId;
        set
        {
            IsSourceNameUpdated |= sourceId != value || SequenceId == 0;

            sourceId = value;
        }
    }

    [JsonIgnore]
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Executable
    {
        get => (BooleanValues & LayerBooleanValues.Executable) > 0;
        set
        {
            IsExecutableUpdated |= Executable != value || SequenceId == 0;
            if (value)
                BooleanValues |= LayerBooleanValues.Executable;

            else if (Executable) BooleanValues ^= LayerBooleanValues.Executable;
        }
    }

    [JsonIgnore]
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

    [JsonIgnore] INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;


    [JsonIgnore]
    public IPQNameIdLookupGenerator NameIdLookup
    {
        get => nameIdLookup;
        set
        {
            if (nameIdLookup == value) return;
            string? cacheSourceName           = null;
            if (sourceId > 0) cacheSourceName = SourceName;
            nameIdLookup = value;
            if (cacheSourceName != null && sourceId > 0)
                try
                {
                    nameIdLookup.SetIdToName(sourceId, cacheSourceName);
                }
                catch
                {
                    sourceId = (ushort)nameIdLookup.GetOrAddId(cacheSourceName);
                }
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
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

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        NameIdLookup.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
    }

    IMutableSourcePriceVolumeLayer ITrackableReset<IMutableSourcePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IMutableSourcePriceVolumeLayer IMutableSourcePriceVolumeLayer.       ResetWithTracking() => ResetWithTracking();

    IPQSourcePriceVolumeLayer ITrackableReset<IPQSourcePriceVolumeLayer>.ResetWithTracking() => ResetWithTracking();

    IPQSourcePriceVolumeLayer IPQSourcePriceVolumeLayer.ResetWithTracking() => ResetWithTracking();

    public override PQSourcePriceVolumeLayer ResetWithTracking()
    {
        SourceId   = 0;
        Executable = false;
        base.ResetWithTracking();
        return this;
    }

    public override void StateReset()
    {
        SourceId   = 0;
        Executable = false;
        base.StateReset();
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates;
        set
        {
            NameIdLookup.HasUpdates = value;
            base.HasUpdates         = value;
        }
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var fullPicture = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) > 0;
        foreach (var pqFieldUpdate in base.GetDeltaUpdateFields(snapShotTime, messageFlags,
                                                                quotePublicationPrecisionSetting))
            yield return pqFieldUpdate;
        if (fullPicture || IsSourceNameUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteLayerSourceId, SourceId);

        if (fullPicture || IsExecutableUpdated)
        {
            var boolValues = LayerBehavior.HasPublishQuoteInstantBehaviorFlagsFlag()
                ? (uint)BooleanValues
                : BooleanValues.JustLayerBooleanValuesMask();
            yield return new PQFieldUpdate(PQFeedFields.QuoteLayerBooleanFlags, boolValues);
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        // assume the book has already forwarded this through to the correct layer
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.QuoteLayerStringUpdates:
                return NameIdLookup.VerifyDictionaryAndExtractSize(pqFieldUpdate);
            case PQFeedFields.QuoteLayerSourceId:
                IsSourceNameUpdated = true; // in-case of reset and sending 0;
                SourceId            = (ushort)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.QuoteLayerBooleanFlags:
                IsExecutableUpdated = true; // in-case of reset and sending 0;
                var newFlags = pqFieldUpdate.Payload.JustLayerBooleanValuesMask();
                Executable = (newFlags & LayerBooleanValues.Executable) != 0;
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        foreach (var stringUpdate in NameIdLookup.GetStringUpdates(snapShotTime, messageFlags)) yield return stringUpdate;
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.QuoteLayerStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public override PQSourcePriceVolumeLayer CopyFrom(IPriceVolumeLayer source, QuoteInstantBehaviorFlags behaviorFlags
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, behaviorFlags, copyMergeFlags);
        var pqSPvl = source as IPQSourcePriceVolumeLayer;
        if (source is ISourcePriceVolumeLayer sPvl && pqSPvl == null)
        {
            if (SourceName != sPvl.SourceName) SourceId = (ushort)NameIdLookup.GetOrAddId(sPvl.SourceName);
            Executable = sPvl.Executable;
        }
        else if (pqSPvl != null)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if(!copyMergeFlags.HasSkipReferenceLookups()) NameIdLookup.CopyFrom(pqSPvl.NameIdLookup, copyMergeFlags);

            if (pqSPvl.IsSourceNameUpdated || isFullReplace)
            {
                IsSourceNameUpdated = true;

                SourceId   = pqSPvl.SourceId;
            }
            if (pqSPvl.IsExecutableUpdated || isFullReplace)
            {
                IsExecutableUpdated = true;

                Executable = pqSPvl.Executable;
            }

            if (isFullReplace) SetFlagsSame(pqSPvl);
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
        if (other is not ISourcePriceVolumeLayer sourceOther) return false;
        var baseSame             = base.AreEquivalent(other, exactTypes);
        var sourceExecutableSame = Executable == sourceOther.Executable;
        var sourceNameSame       = SourceName == sourceOther.SourceName;

        return baseSame && sourceExecutableSame && sourceNameSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPriceVolumeLayer?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (base.GetHashCode() * 397) ^ sourceId.GetHashCode();
            hashCode = (hashCode * 397) ^ Executable.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({PQSourcePriceVolumeLayerToStringMembers}, {UpdatedFlagsToString})";
}
