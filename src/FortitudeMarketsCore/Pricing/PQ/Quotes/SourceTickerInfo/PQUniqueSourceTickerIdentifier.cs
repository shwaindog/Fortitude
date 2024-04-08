#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

public class PQUniqueSourceTickerIdentifier : IPQUniqueSourceTickerIdentifier
{
    private uint id;
    private string source = "";
    private string ticker = "";
    protected SourceTickerInfoUpdatedFlags UpdatedFlags;

    public PQUniqueSourceTickerIdentifier() { }

    public PQUniqueSourceTickerIdentifier(uint id, string source, string ticker)
    {
        Id = id;
        Source = source;
        Ticker = ticker;
    }

    public PQUniqueSourceTickerIdentifier(string source, string ticker, ushort sourceId, ushort tickerId)
    {
        Id = UniqueSourceTickerIdentifier.GenerateUniqueSourceTickerId(sourceId, tickerId);
        Source = source;
        Ticker = ticker;
    }

    public PQUniqueSourceTickerIdentifier(IUniqueSourceTickerIdentifier toClone)
    {
        Id = toClone.Id;
        Source = toClone.Source;
        Ticker = toClone.Ticker;
        if (toClone is PQUniqueSourceTickerIdentifier pqClone)
        {
            IsIdUpdated = pqClone.IsIdUpdated;
            IsSourceUpdated = pqClone.IsSourceUpdated;
            IsTickerUpdated = pqClone.IsTickerUpdated;
            UpdatedFlags = pqClone.UpdatedFlags;
        }
    }

    public uint Id
    {
        get => id;
        set
        {
            if (id == value) return;
            IsIdUpdated = true;
            id = value;
        }
    }

    public ushort SourceId => (ushort)(Id >> 16);
    public ushort TickerId => (ushort)(0xFFFF & Id);

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

    public string Source
    {
        get => source;
        set
        {
            if (source == value) return;
            IsSourceUpdated = true;
            source = value;
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

    public string Ticker
    {
        get => ticker;
        set
        {
            if (ticker == value) return;
            IsTickerUpdated = true;
            ticker = value;
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

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
        IPQQuotePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var updatedOnly = (updateStyle & UpdateStyle.Updates) > 0;
        if (!updatedOnly || IsIdUpdated) yield return new PQFieldUpdate(PQFieldKeys.SourceTickerId, id);
    }

    public virtual int UpdateField(PQFieldUpdate fieldUpdate)
    {
        switch (fieldUpdate.Id)
        {
            case PQFieldKeys.SourceTickerNames:
                return (int)fieldUpdate.Value;
            case PQFieldKeys.SourceTickerId:
                Id = fieldUpdate.Value;
                return 0;
        }

        return 0;
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
    {
        var isUpdateOnly = updatedStyle == UpdateStyle.Updates;
        if (!isUpdateOnly || IsSourceUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpdate), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 0, Value = Source, Command = CrudCommand.Update
                    }
            };
        if (!isUpdateOnly || IsTickerUpdated)
            yield return new PQFieldStringUpdate
            {
                Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpdate), StringUpdate
                    = new PQStringUpdate
                    {
                        DictionaryId = 1, Value = Ticker, Command = CrudCommand.Update
                    }
            };
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate updates)
    {
        if (updates.Field.Id == PQFieldKeys.SourceTickerNames)
        {
            var stringUpdt = updates.StringUpdate;
            var upsert = stringUpdt.Command == CrudCommand.Update || stringUpdt.Command == CrudCommand.Insert;
            if (stringUpdt.DictionaryId == 0 && upsert) Source = updates.StringUpdate.Value;
            if (stringUpdt.DictionaryId == 1 && upsert) Ticker = updates.StringUpdate.Value;
            return true;
        }

        return false;
    }

    public virtual IUniqueSourceTickerIdentifier CopyFrom(IUniqueSourceTickerIdentifier sourceUniqueTickerId
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (sourceUniqueTickerId == null) return this;

        Id = sourceUniqueTickerId.Id;
        Source = sourceUniqueTickerId.Source;
        Ticker = sourceUniqueTickerId.Ticker;
        if (sourceUniqueTickerId is PQUniqueSourceTickerIdentifier pqSrcTkrId) UpdatedFlags = pqSrcTkrId.UpdatedFlags;
        return this;
    }

    public IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags) =>
        (IStoreState)CopyFrom((IUniqueSourceTickerIdentifier)source, copyMergeFlags);

    public virtual object Clone() => new PQUniqueSourceTickerIdentifier(this);

    IUniqueSourceTickerIdentifier ICloneable<IUniqueSourceTickerIdentifier>.Clone() => (IUniqueSourceTickerIdentifier)Clone();

    public virtual bool AreEquivalent(IUniqueSourceTickerIdentifier? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var idsSame = Id == other.Id;
        var sourceNameSame = string.Equals(Source, other.Source);
        var tickerNameSame = string.Equals(Ticker, other.Ticker);
        var updatesSame = true;
        if (exactTypes)
        {
            var pqUniqSrcTrkId = other as PQUniqueSourceTickerIdentifier;
            updatesSame = UpdatedFlags == pqUniqSrcTrkId!.UpdatedFlags;
        }

        return idsSame && sourceNameSame && tickerNameSame && updatesSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (!(obj is PQUniqueSourceTickerIdentifier)) return false;
        return AreEquivalent((IUniqueSourceTickerIdentifier)obj, true);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)Id;
            hashCode = (hashCode * 397) ^ (Source != null ? Source.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (Ticker != null ? Ticker.GetHashCode() : 0);
            return hashCode;
        }
    }
}
