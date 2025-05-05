// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;

public interface IPQSourceTickerId : ISourceTickerId, IReusableObject<IPQSourceTickerId>
  , IHasNameIdLookup, IPQSupportsFieldUpdates<IPQSourceTickerId>, IPQSupportsStringUpdates<IPQSourceTickerId>
{
    new ushort SourceId       { get; set; }
    new ushort InstrumentId   { get; set; }
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }

    bool IsIdUpdated     { get; set; }
    bool IsSourceUpdated { get; set; }
    bool IsTickerUpdated { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourceTickerId Clone();
}

public class PQSourceTickerId : ReusableObject<IPQSourceTickerId>, IPQSourceTickerId
{
    protected uint NumUpdates = uint.MaxValue;

    private ushort sourceId;
    private string sourceName = "";
    private string ticker     = "";
    private ushort tickerId;

    protected SourceTickerInfoUpdatedFlags UpdatedFlags;

    public PQSourceTickerId()
    {
        if (GetType() == typeof(PQSourceTickerId)) NumUpdates = 0;
    }

    public PQSourceTickerId(ushort sourceId, string sourceName, ushort tickerId, string ticker)
    {
        SourceId       = sourceId;
        InstrumentId   = tickerId;
        SourceName     = sourceName;
        InstrumentName = ticker;

        if (GetType() == typeof(PQSourceTickerId)) NumUpdates = 0;
    }

    public PQSourceTickerId(ISourceTickerId toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.InstrumentId;
        SourceName     = toClone.SourceName;
        InstrumentName = toClone.InstrumentName;
        if (toClone is IPQSourceTickerId pqSourceTickerId)
        {
            IsIdUpdated     = pqSourceTickerId.IsIdUpdated;
            IsSourceUpdated = pqSourceTickerId.IsSourceUpdated;
            IsTickerUpdated = pqSourceTickerId.IsTickerUpdated;
        }

        if (GetType() == typeof(PQSourceTickerId)) NumUpdates = 0;
    }

    public PQSourceTickerId(SourceTickerIdentifier toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.TickerId;
        SourceName     = toClone.Source;
        InstrumentName = toClone.Ticker;

        if (GetType() == typeof(PQSourceTickerId)) NumUpdates = 0;
    }

    public PQSourceTickerId(SourceTickerIdValue toClone)
    {
        SourceId       = toClone.SourceId;
        InstrumentId   = toClone.TickerId;
        SourceName     = toClone.Source;
        InstrumentName = toClone.Ticker;

        if (GetType() == typeof(PQSourceTickerId)) NumUpdates = 0;
    }

    public uint SourceTickerId => ((uint)SourceId << 16) | InstrumentId;
    public ushort SourceId
    {
        get => sourceId;
        set
        {
            IsIdUpdated |= sourceId != value || NumUpdates == 0;
            sourceId    =  value;
        }
    }

    public ushort InstrumentId
    {
        get => tickerId;
        set
        {
            IsIdUpdated |= tickerId != value || NumUpdates == 0;
            tickerId    =  value;
        }
    }

    public string SourceName
    {
        get => sourceName;
        set
        {
            IsSourceUpdated |= sourceName != value || NumUpdates == 0;
            sourceName      =  value;
        }
    }

    public string InstrumentName
    {
        get => ticker;
        set
        {
            IsTickerUpdated |= ticker != value || NumUpdates == 0;
            ticker          =  value;
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

    public bool IsSourceUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.SourceName) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.SourceName;

            else if (IsSourceUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.SourceName;
        }
    }

    public bool IsTickerUpdated
    {
        get => (UpdatedFlags & SourceTickerInfoUpdatedFlags.TickerName) > 0;
        set
        {
            if (value)
                UpdatedFlags |= SourceTickerInfoUpdatedFlags.TickerName;

            else if (IsTickerUpdated) UpdatedFlags ^= SourceTickerInfoUpdatedFlags.TickerName;
        }
    }

    public virtual bool HasUpdates
    {
        get => UpdatedFlags > 0;
        set => UpdatedFlags = value ? UpdatedFlags.AllFlags() : SourceTickerInfoUpdatedFlags.None;
    }

    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = new PQNameIdLookupGenerator(PQQuoteFields.SourceTickerNames);

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public uint UpdateCount => NumUpdates;

    public void UpdateComplete()
    {
        if (HasUpdates)
        {
            NumUpdates++;
            HasUpdates = false;
        }
    }

    public override void StateReset()
    {
        sourceId   = 0;
        tickerId   = 0;
        ticker     = "";
        sourceName = "";

        NumUpdates   = 0;
        UpdatedFlags = SourceTickerInfoUpdatedFlags.None;
        base.StateReset();
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (updateStyle & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsIdUpdated) yield return new PQFieldUpdate(PQQuoteFields.SourceTickerId, SourceTickerId);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        var isUpdateOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!isUpdateOnly || IsSourceUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQQuoteFields.SourceTickerNames, CrudCommand.Upsert.ToPQSubFieldId(), 0), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 0, Value = SourceName, Command = CrudCommand.Upsert
                    }
            };
        if (!isUpdateOnly || IsTickerUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQQuoteFields.SourceTickerNames, CrudCommand.Upsert.ToPQSubFieldId(), 0), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 1, Value = InstrumentName, Command = CrudCommand.Upsert
                    }
            };
    }

    public bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id == PQQuoteFields.SourceTickerNames)
        {
            var stringUpdt = updates.StringUpdate;
            var upsert     = stringUpdt.Command == CrudCommand.Upsert;

            if (stringUpdt.DictionaryId == 0 && upsert) SourceName     = updates.StringUpdate.Value;
            if (stringUpdt.DictionaryId == 1 && upsert) InstrumentName = updates.StringUpdate.Value;
            return true;
        }

        return false;
    }

    public virtual int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQQuoteFields.SourceTickerId:
                SourceId     = (ushort)(fieldUpdate.Payload >> 16);
                InstrumentId = (ushort)(fieldUpdate.Payload & 0xFFFF);
                return 0;
            case PQQuoteFields.SourceTickerNames: return (int)fieldUpdate.Payload;
        }
        return -1;
    }

    public override IPQSourceTickerId CopyFrom(IPQSourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            if (source.IsSourceUpdated) SourceName     = source.SourceName;
            if (source.IsTickerUpdated) InstrumentName = source.InstrumentName;
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

    IReusableObject<ISourceTickerId> ITransferState<IReusableObject<ISourceTickerId>>.CopyFrom
        (IReusableObject<ISourceTickerId> source, CopyMergeFlags copyMergeFlags) =>
        (IPQSourceTickerId)CopyFrom((ISourceTickerId)source, copyMergeFlags);

    ISourceTickerId ICloneable<ISourceTickerId>.Clone() => Clone();

    ISourceTickerId ITransferState<ISourceTickerId>.CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPQSourceTickerId pqSrcTkrId && copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            if (pqSrcTkrId.IsSourceUpdated) SourceName     = pqSrcTkrId.SourceName;
            if (pqSrcTkrId.IsTickerUpdated) InstrumentName = pqSrcTkrId.InstrumentName;
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

    public virtual bool AreEquivalent(ISourceTickerInfo? other, bool exactTypes = false)
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
            sourceUpdateSame = IsSourceUpdated == pqSrcTkrId?.IsSourceUpdated;
            tickerUpdateSame = IsTickerUpdated == pqSrcTkrId?.IsTickerUpdated;
        }

        var allAreSame = sourceIdSame && tickerIdSame && sourceNameSame && tickerNameSame && idUpdateSame && sourceUpdateSame && tickerUpdateSame;
        return allAreSame;
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

    public override string ToString() =>
        $"{nameof(PQSourceTickerId)}({nameof(SourceId)}: {SourceId}, {nameof(SourceName)}: {SourceName}, {nameof(InstrumentId)}: {InstrumentId})";
}
