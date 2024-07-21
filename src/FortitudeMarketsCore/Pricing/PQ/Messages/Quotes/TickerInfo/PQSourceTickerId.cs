// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Maps.IdMap;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.TickerInfo;

public interface IPQSourceTickerId : ISourceTickerId, IReusableObject<IPQSourceTickerId>
  , IHasNameIdLookup, IPQSupportsFieldUpdates<IPQSourceTickerId>, IPQSupportsStringUpdates<IPQSourceTickerId>
{
    new ushort SourceId { get; set; }
    new ushort TickerId { get; set; }
    new string Source   { get; set; }
    new string Ticker   { get; set; }

    bool IsIdUpdated     { get; set; }
    bool IsSourceUpdated { get; set; }
    bool IsTickerUpdated { get; set; }

    new IPQNameIdLookupGenerator NameIdLookup { get; set; }

    new IPQSourceTickerId Clone();
}

public class PQSourceTickerId : ReusableObject<IPQSourceTickerId>, IPQSourceTickerId
{
    private string source = "";
    private ushort sourceId;
    private string ticker = "";
    private ushort tickerId;

    protected SourceTickerInfoUpdatedFlags UpdatedFlags;
    public PQSourceTickerId() { }

    public PQSourceTickerId(ushort sourceId, string source, ushort tickerId, string ticker)
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source   = source;
        Ticker   = ticker;
    }

    public PQSourceTickerId(ISourceTickerId toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        Source   = toClone.Source;
        Ticker   = toClone.Ticker;
        if (toClone is IPQSourceTickerId pqSourceTickerId)
        {
            IsIdUpdated     = pqSourceTickerId.IsIdUpdated;
            IsSourceUpdated = pqSourceTickerId.IsSourceUpdated;
            IsTickerUpdated = pqSourceTickerId.IsTickerUpdated;
        }
    }

    public PQSourceTickerId(SourceTickerIdentifier toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        Source   = toClone.Source;
        Ticker   = toClone.Ticker;
    }

    public PQSourceTickerId(SourceTickerIdValue toClone)
    {
        SourceId = toClone.SourceId;
        TickerId = toClone.TickerId;
        Source   = toClone.Source;
        Ticker   = toClone.Ticker;
    }

    public uint SourceTickerId => ((uint)SourceId << 16) | TickerId;
    public ushort SourceId
    {
        get => sourceId;
        set
        {
            if (sourceId == value) return;
            IsIdUpdated = true;
            sourceId    = value;
        }
    }

    public ushort TickerId
    {
        get => tickerId;
        set
        {
            if (tickerId == value) return;
            IsIdUpdated = true;
            tickerId    = value;
        }
    }

    public string Source
    {
        get => source;
        set
        {
            if (source == value) return;
            IsSourceUpdated = true;
            source          = value;
        }
    }

    public string Ticker
    {
        get => ticker;
        set
        {
            if (ticker == value) return;
            IsTickerUpdated = true;
            ticker          = value;
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

    public IPQNameIdLookupGenerator NameIdLookup { get; set; } = new PQNameIdLookupGenerator(PQFieldKeys.SourceTickerNames);

    INameIdLookup? IHasNameIdLookup.NameIdLookup => NameIdLookup;

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags updateStyle, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (updateStyle & StorageFlags.Complete) == 0;

        if (!updatedOnly || IsIdUpdated) yield return new PQFieldUpdate(PQFieldKeys.SourceTickerId, SourceTickerId);
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        var isUpdateOnly = (messageFlags & StorageFlags.Complete) == 0;
        if (!isUpdateOnly || IsSourceUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpsert), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 0, Value = Source, Command = CrudCommand.Upsert
                    }
            };
        if (!isUpdateOnly || IsTickerUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpsert), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 1, Value = Ticker, Command = CrudCommand.Upsert
                    }
            };
    }

    public bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id == PQFieldKeys.SourceTickerNames)
        {
            var stringUpdt = updates.StringUpdate;
            var upsert     = stringUpdt.Command == CrudCommand.Upsert;

            if (stringUpdt.DictionaryId == 0 && upsert) Source = updates.StringUpdate.Value;
            if (stringUpdt.DictionaryId == 1 && upsert) Ticker = updates.StringUpdate.Value;
            return true;
        }

        return false;
    }

    public virtual int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFieldKeys.SourceTickerId:
                SourceId = (ushort)(fieldUpdate.Value >> 16);
                TickerId = (ushort)(fieldUpdate.Value & 0xFFFF);
                return 0;
            case PQFieldKeys.SourceTickerNames: return (int)fieldUpdate.Value;
        }
        return -1;
    }

    public override IPQSourceTickerId CopyFrom(IPQSourceTickerId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            if (source.IsSourceUpdated) Source = source.Source;
            if (source.IsTickerUpdated) Ticker = source.Ticker;
        }
        else
        {
            SourceId = source.SourceId;
            Source   = source.Source;
            TickerId = source.TickerId;
            Ticker   = source.Ticker;
        }
        return this;
    }

    public override IPQSourceTickerId Clone() => Recycler?.Borrow<PQSourceTickerId>().CopyFrom(this) ?? new PQSourceTickerId(this);

    IReusableObject<ISourceTickerId> IStoreState<IReusableObject<ISourceTickerId>>.CopyFrom
        (IReusableObject<ISourceTickerId> source, CopyMergeFlags copyMergeFlags) =>
        (IPQSourceTickerId)CopyFrom((ISourceTickerId)source, copyMergeFlags);

    ISourceTickerId ICloneable<ISourceTickerId>.Clone() => Clone();

    ISourceTickerId IStoreState<ISourceTickerId>.CopyFrom(ISourceTickerId source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPQSourceTickerId pqSrcTkrId && copyMergeFlags == CopyMergeFlags.JustDifferences)
        {
            if (pqSrcTkrId.IsSourceUpdated) Source = pqSrcTkrId.Source;
            if (pqSrcTkrId.IsTickerUpdated) Ticker = pqSrcTkrId.Ticker;
        }
        else
        {
            SourceId = source.SourceId;
            Source   = source.Source;
            TickerId = source.TickerId;
            Ticker   = source.Ticker;
        }

        return this;
    }

    public virtual bool AreEquivalent(ISourceTickerInfo? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var sourceIdSame   = SourceId == other.SourceId;
        var tickerIdSame   = TickerId == other.TickerId;
        var sourceNameSame = string.Equals(Source, other.Source);
        var tickerNameSame = string.Equals(Ticker, other.Ticker);

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
            hashCode = (hashCode * 397) ^ TickerId;
            hashCode = (hashCode * 397) ^ Source.GetHashCode();
            hashCode = (hashCode * 397) ^ Ticker.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PQSourceTickerId)}({nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, {nameof(TickerId)}: {TickerId})";
}
