// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

public interface IPQSourceTickerId : ISourceTickerId, IHasNameIdLookup, IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates
{
    new ushort SourceId     { get; set; }
    new ushort InstrumentId { get; set; }

    int        SourceNameId     { get; set; }
    new string SourceName       { get; set; }
    int        InstrumentNameId { get; set; }
    new string InstrumentName   { get; set; }

    bool IsIdUpdated             { get; set; }
    bool IsSourceNameUpdated     { get; set; }
    bool IsInstrumentNameUpdated { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourceTickerId Clone();
}

public class PQSourceTickerId : ReusableObject<ISourceTickerId>, IPQSourceTickerId
{
    protected uint SequenceId = uint.MaxValue;

    private ushort sourceId;
    private ushort tickerId;

    private int sourceNameId;
    private int instrumentNameId;

    protected SourceTickerInfoUpdatedFlags UpdatedFlags;

    public PQSourceTickerId()
    {
        if (GetType() == typeof(PQSourceTickerId)) SequenceId = 0;
    }

    public PQSourceTickerId(ushort sourceId, string sourceName, ushort tickerId, string ticker)
    {
        SourceId       = sourceId;
        InstrumentId   = tickerId;
        SourceName     = sourceName;
        InstrumentName = ticker;

        if (GetType() == typeof(PQSourceTickerId)) SequenceId = 0;
    }

    public PQSourceTickerId(ISourceTickerId toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;
        if (toClone is IPQSourceTickerId pqSourceTickerId)
        {
            IsIdUpdated             = pqSourceTickerId.IsIdUpdated;
            IsSourceNameUpdated     = pqSourceTickerId.IsSourceNameUpdated;
            IsInstrumentNameUpdated = pqSourceTickerId.IsInstrumentNameUpdated;
        }

        SetFlagsSame(toClone);

        if (GetType() == typeof(PQSourceTickerId)) SequenceId = 0;
    }

    public PQSourceTickerId(SourceTickerIdentifier toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;

        if (GetType() == typeof(PQSourceTickerId)) SequenceId = 0;
    }

    public PQSourceTickerId(SourceTickerIdValue toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;

        if (GetType() == typeof(PQSourceTickerId)) SequenceId = 0;
    }

    public uint SourceInstrumentId => ((uint)SourceId << 16) | InstrumentId;

    public ushort SourceId
    {
        get => sourceId;
        set
        {
            IsIdUpdated |= sourceId != value || SequenceId == 0;
            sourceId    =  value;
        }
    }

    public ushort InstrumentId
    {
        get => tickerId;
        set
        {
            IsIdUpdated |= tickerId != value || SequenceId == 0;
            tickerId    =  value;
        }
    }

    public int SourceNameId
    {
        get => sourceNameId;
        set
        {
            IsSourceNameUpdated |= sourceNameId != value || SequenceId == 0;
            sourceNameId        =  value;
        }
    }

    public string SourceName
    {
        get => NameIdLookup[sourceNameId]!;
        set
        {
            if (value == NameIdLookup[sourceNameId]) return;

            var nameId = value.IsNullOrEmpty() ? 0 : NameIdLookup.GetOrAddId(value);
            SourceNameId = nameId;
        }
    }

    public int InstrumentNameId
    {
        get => instrumentNameId;
        set
        {
            IsInstrumentNameUpdated |= instrumentNameId != value || SequenceId == 0;
            instrumentNameId        =  value;
        }
    }

    public string InstrumentName
    {
        get => NameIdLookup[instrumentNameId]!;
        set
        {
            if (value == NameIdLookup[instrumentNameId]) return;
            
            var nameId = value.IsNullOrEmpty() ? 0 : NameIdLookup.GetOrAddId(value);
            InstrumentNameId = nameId;
        }
    }

    public bool IsIdUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SourceTickerId) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SourceTickerId;

            else if (IsIdUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SourceTickerId;
        }
    }

    public bool IsSourceNameUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SourceName) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SourceName;

            else if (IsSourceNameUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SourceName;
        }
    }

    public bool IsInstrumentNameUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.InstrumentName) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.InstrumentName;

            else if (IsInstrumentNameUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.InstrumentName;
        }
    }

    public virtual bool HasUpdates
    {
        get => UpdatedFlags != SourceTickerInfoUpdatedFlags.None;
        set
        {
            NameIdLookup.HasUpdates = value;
            UpdatedFlags            = value ? UpdatedFlags.AllFlags() : SourceTickerInfoUpdatedFlags.None;
        }
    }

    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = new PQNameIdLookupGenerator(PQFeedFields.SourceTickerDefinitionStringUpdates);

    INameIdLookup IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public uint UpdateSequenceId => SequenceId;

    public void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates)
        {
            SequenceId++;
            HasUpdates = false;
        }
    }

    public override void StateReset()
    {
        sourceId = 0;
        tickerId = 0;

        sourceNameId     = 0;
        instrumentNameId = 0;

        SequenceId   = 0;
        UpdatedFlags = SourceTickerInfoUpdatedFlags.None;
        base.StateReset();
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, PQMessageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (updateStyle & PQMessageFlags.Complete) == 0;

        if (!updatedOnly || IsIdUpdated) yield return new PQFieldUpdate(PQFeedFields.SourceTickerId, SourceInstrumentId);

        if (!updatedOnly || IsSourceNameUpdated) 
            yield return new PQFieldUpdate
            (PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.SourceNameId,  SourceNameId);

        if (!updatedOnly || IsInstrumentNameUpdated) 
            yield return new PQFieldUpdate
            (PQFeedFields.SourceTickerDefinition, PQTickerDefSubFieldKeys.InstrumentNameId, InstrumentNameId);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        return NameIdLookup.GetStringUpdates(snapShotTime, messageFlags);
    }

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        if (stringUpdate.Field.Id != PQFeedFields.SourceTickerDefinitionStringUpdates) return false;
        return NameIdLookup.UpdateFieldString(stringUpdate);
    }

    public virtual int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFeedFields.SourceTickerId:
                SourceId     = (ushort)(fieldUpdate.Payload >> 16);
                InstrumentId = (ushort)(fieldUpdate.Payload & 0xFFFF);
                return 0;
            case PQFeedFields.SourceTickerDefinitionStringUpdates:
                return NameIdLookup.VerifyDictionaryAndExtractSize(fieldUpdate);
            case PQFeedFields.SourceTickerDefinition:
                switch (fieldUpdate.DefinitionSubId)
                {
                    case PQTickerDefSubFieldKeys.SourceNameId: 
                        SourceNameId = (int)fieldUpdate.Payload;
                        return 0;
                    case PQTickerDefSubFieldKeys.InstrumentNameId: 
                        InstrumentNameId = (int)fieldUpdate.Payload;
                        return 0;   
                }
                break;
        }
        return -1;
    }

    public override PQSourceTickerId CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQSourceTickerId pqSrcTkrId)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            NameIdLookup.CopyFrom(pqSrcTkrId.NameIdLookup, copyMergeFlags);

            if (pqSrcTkrId.IsIdUpdated || isFullReplace)
            {
                IsIdUpdated  = true;

                SourceId     = source.SourceId;
                InstrumentId = source.InstrumentId;
            }
            if (pqSrcTkrId.IsSourceNameUpdated || isFullReplace)
            {
                IsSourceNameUpdated = true;

                SourceNameId         = pqSrcTkrId.SourceNameId;
            }
            if (pqSrcTkrId.IsInstrumentNameUpdated || isFullReplace)
            {
                IsInstrumentNameUpdated = true;

                InstrumentNameId = pqSrcTkrId.InstrumentNameId;
            }

            if(isFullReplace) SetFlagsSame(pqSrcTkrId);
        }
        else
        {
            SourceId       = source.SourceId;
            SourceName     = source.SourceName;
            InstrumentId   = source.InstrumentId;
            InstrumentName = source.InstrumentName;
        }
        return this;
    }

    public override IPQSourceTickerId Clone() => Recycler?.Borrow<PQSourceTickerId>().CopyFrom(this) ?? new PQSourceTickerId(this);


    ISourceTickerId ICloneable<ISourceTickerId>.Clone() => Clone();

    IReusableObject<ISourceTickerId> ITransferState<IReusableObject<ISourceTickerId>>.CopyFrom
        (IReusableObject<ISourceTickerId> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((ISourceTickerId)source, copyMergeFlags);

    public IPQSourceTickerId CopyFrom(IPQSourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((ISourceTickerId)source, copyMergeFlags);

    ISourceTickerId ITransferState<ISourceTickerId>.CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags) => 
        CopyFrom(source, copyMergeFlags);

    public virtual bool AreEquivalent(ISourceTickerId? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var sourceIdSame   = SourceId == other.SourceId;
        var tickerIdSame   = InstrumentId == other.InstrumentId;
        var sourceNameSame = string.Equals(SourceName, other.SourceName);
        var tickerNameSame = string.Equals(InstrumentName, other.InstrumentName);

        var idUpdateSame     = true;
        var sourceUpdateSame = true;
        var tickerUpdateSame = true;

        if (exactTypes)
        {
            var pqSrcTkrId = other as IPQSourceTickerId;
            idUpdateSame     = IsIdUpdated == pqSrcTkrId?.IsIdUpdated;
            sourceUpdateSame = IsSourceNameUpdated == pqSrcTkrId?.IsSourceNameUpdated;
            tickerUpdateSame = IsInstrumentNameUpdated == pqSrcTkrId?.IsInstrumentNameUpdated;
        }

        var allAreSame = sourceIdSame && tickerIdSame && sourceNameSame && tickerNameSame && idUpdateSame && sourceUpdateSame && tickerUpdateSame;
        return allAreSame;
    }

    protected void SetFlagsSame(ISourceTickerId toSetSame)
    {
        if (toSetSame is PQSourceTickerId pqSourceTickerId) UpdatedFlags = pqSourceTickerId.UpdatedFlags;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((ISourceTickerInfo?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ InstrumentId;
            hashCode = (hashCode * 397) ^ SourceName.GetHashCode();
            hashCode = (hashCode * 397) ^ InstrumentName.GetHashCode();
            return hashCode;
        }
    }

    protected string PQSourceTickerIdToStringMembers =>
        $"{nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, {nameof(SourceNameId)}: {SourceNameId}, " +
        $"{nameof(InstrumentId)}: {InstrumentId} {nameof(InstrumentName)}: {InstrumentName}, {nameof(InstrumentNameId)}: {InstrumentNameId}";

    protected string SourceInstrumentIdToString => $"{nameof(SourceInstrumentId)}: {SourceInstrumentId}";

    protected string UpdateFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override string ToString() => $"{nameof(PQSourceTickerId)}{{{PQSourceTickerIdToStringMembers}, {SourceInstrumentIdToString}, {UpdateFlagsToString}}}";
}
