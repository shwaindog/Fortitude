// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.PQ.Summaries;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public interface IPQLevel3Quote : IPQLevel2Quote, IMutableLevel3Quote, IDoublyLinkedListNode<IPQLevel3Quote>
{
    new IPQRecentlyTraded? RecentlyTraded { get; set; }

    bool IsValueDateUpdated            { get; set; }
    bool IsBatchIdUpdated              { get; set; }
    bool IsSourceQuoteReferenceUpdated { get; set; }

    new IPQLevel3Quote? Next     { get; set; }
    new IPQLevel3Quote? Previous { get; set; }
    new IPQLevel3Quote  CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQLevel3Quote Clone();
}

public class PQLevel3Quote : PQLevel2Quote, IPQLevel3Quote, ICloneable<PQLevel3Quote>, IDoublyLinkedListNode<PQLevel3Quote>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQLevel3Quote));

    private uint batchId;

    private IPQRecentlyTraded? recentlyTraded;

    private uint     sourceQuoteRef;
    private DateTime valueDate;

    public PQLevel3Quote()
    {
        recentlyTraded = new PQRecentlyTraded();

        if (GetType() == typeof(PQLevel3Quote)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQLevel3Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m)
    {
    }

    public PQLevel3Quote(ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , decimal singlePrice = 0m, DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null
      , DateTime? sourceBidTime = null , bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool isAskPriceTopChanged = false, bool executable = true, IPricePeriodSummary? periodSummary = null
      , IOrderBook? orderBook = null, IRecentlyTraded? recentlyTraded = null, uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null)
        : base(sourceTickerInfo, sourceTime, isReplay, feedSyncStatus, singlePrice, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, validFrom, validTo, isAskPriceTopChanged, executable,
               periodSummary, orderBook)
    {
        if (recentlyTraded is IPQRecentlyTraded pqRecentlyTraded)
        {
            this.recentlyTraded = pqRecentlyTraded;
        }
        else if(recentlyTraded != null)
        {
            this.recentlyTraded = new PQRecentlyTraded(recentlyTraded);
        } 
        else if (PQSourceTickerInfo!.LastTradedFlags != LastTradedFlags.None)
        {
            this.recentlyTraded = new PQRecentlyTraded(PQSourceTickerInfo);
        }
        BatchId              = batchId;
        SourceQuoteReference = sourceQuoteRef;
        ValueDate            = valueDate ?? DateTime.MinValue;

        if (GetType() == typeof(PQLevel3Quote)) NumOfUpdates = 0;
    }

    public PQLevel3Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is IPQLevel3Quote ipql3QToClone)
        {
            BatchId              = ipql3QToClone.BatchId;
            SourceQuoteReference = ipql3QToClone.SourceQuoteReference;
            ValueDate            = ipql3QToClone.ValueDate;
            recentlyTraded       = ipql3QToClone?.RecentlyTraded?.Clone();
        }

        if (toClone is PQLevel3Quote pql3QToClone)
        {
            IsValueDateUpdated = pql3QToClone.IsValueDateUpdated;
            IsBatchIdUpdated   = pql3QToClone.IsBatchIdUpdated;

            IsSourceQuoteReferenceUpdated = pql3QToClone.IsSourceQuoteReferenceUpdated;

            IsValueDateUpdated = pql3QToClone.IsSourceQuoteReferenceUpdated;

            UpdatedFlags = pql3QToClone.UpdatedFlags;
        }
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel3Quote)) NumOfUpdates = 0;
    }

    protected string Level3ToStringMembers =>
        $"{Level2ToStringMembers}, {nameof(BatchId)}: {BatchId}, {nameof(IsBatchIdUpdated)}: {IsBatchIdUpdated}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference}, " +
        $"{nameof(IsSourceQuoteReferenceUpdated)}: {IsSourceQuoteReferenceUpdated}, {nameof(ValueDate)}: {ValueDate}, " +
        $"{nameof(IsValueDateUpdated)}: {IsValueDateUpdated}, {nameof(RecentlyTraded)}: {RecentlyTraded}";

    public override PQLevel3Quote Clone() =>
        Recycler?.Borrow<PQLevel3Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) as PQLevel3Quote ?? new PQLevel3Quote(this);

    [JsonIgnore]
    public new PQLevel3Quote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PQLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PQLevel3Quote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PQLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQLevel3Quote? IPQLevel3Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQLevel3Quote? IPQLevel3Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel3Quote? ILevel3Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel3Quote? ILevel3Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel3Quote? IDoublyLinkedListNode<ILevel3Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel3Quote? IDoublyLinkedListNode<ILevel3Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQLevel3Quote? IDoublyLinkedListNode<IPQLevel3Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQLevel3Quote? IDoublyLinkedListNode<IPQLevel3Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] public override TickerDetailLevel TickerDetailLevel => TickerDetailLevel.Level3Quote;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId
    {
        get => batchId;
        set
        {
            IsBatchIdUpdated = batchId != value || NumOfUpdates == 0;

            batchId = value;
        }
    }

    [JsonIgnore]
    public bool IsBatchIdUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag;

            else if (IsBatchIdUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.BatchIdeUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference
    {
        get => sourceQuoteRef;
        set
        {
            IsSourceQuoteReferenceUpdated = sourceQuoteRef != value || NumOfUpdates == 0;

            sourceQuoteRef = value;
        }
    }

    [JsonIgnore]
    public bool IsSourceQuoteReferenceUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag;

            else if (IsSourceQuoteReferenceUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceQuoteReferenceUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate
    {
        get => valueDate;
        set
        {
            IsValueDateUpdated = valueDate != value || NumOfUpdates == 0;

            valueDate = value;
        }
    }

    [JsonIgnore]
    public bool IsValueDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.ValueDateUpdatedFlag) > 0 && valueDate != default;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.ValueDateUpdatedFlag;

            else if (IsValueDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.ValueDateUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IPQRecentlyTraded? RecentlyTraded
    {
        get => recentlyTraded;
        set => recentlyTraded = value as PQRecentlyTraded;
    }

    [JsonIgnore]
    IMutableRecentlyTraded? IMutableLevel3Quote.RecentlyTraded
    {
        get => RecentlyTraded;
        set => RecentlyTraded = value as IPQRecentlyTraded;
    }

    [JsonIgnore] IRecentlyTraded? ILevel3Quote.RecentlyTraded => RecentlyTraded;

    [JsonIgnore]
    public override bool HasUpdates
    {
        get =>
            base.HasUpdates || (recentlyTraded?.HasUpdates ?? false) || IsBatchIdUpdated ||
            IsSourceQuoteReferenceUpdated || IsValueDateUpdated;
        set
        {
            base.HasUpdates = IsBatchIdUpdated                    = IsSourceQuoteReferenceUpdated = IsValueDateUpdated = value;
            if (recentlyTraded != null) recentlyTraded.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        recentlyTraded?.UpdateComplete();
        base.UpdateComplete();
    }

    public override void ResetFields()
    {
        recentlyTraded?.StateReset();

        BatchId   = SourceQuoteReference = 0;
        ValueDate = default;
        base.ResetFields();
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;
        quotePublicationPrecisionSetting = quotePublicationPrecisionSetting ?? PQSourceTickerInfo;

        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime,
                                                               messageFlags, quotePublicationPrecisionSetting))
            yield return updatedField;
        if (recentlyTraded != null)
            foreach (var recentlyTradedFields in recentlyTraded.GetDeltaUpdateFields(snapShotTime,
                                                                                     messageFlags, quotePublicationPrecisionSetting))
                yield return recentlyTradedFields;
        if (!updatedOnly || IsBatchIdUpdated) yield return new PQFieldUpdate(PQQuoteFields.BatchId, BatchId);

        if (!updatedOnly || IsSourceQuoteReferenceUpdated) yield return new PQFieldUpdate(PQQuoteFields.QuoteSourceQuoteRef, SourceQuoteReference);
        if (!updatedOnly || IsValueDateUpdated) yield return new PQFieldUpdate(PQQuoteFields.QuoteValueDate, valueDate.Get2MinIntervalsFromUnixEpoch());
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQQuoteFields.LastTradedDictionaryUpsertCommand) return (int)pqFieldUpdate.Payload;
        if (recentlyTraded != null
         && pqFieldUpdate.Id is PQQuoteFields.LastTradedTickTrades)
            return recentlyTraded!.UpdateField(pqFieldUpdate);
        if (pqFieldUpdate.Id == PQQuoteFields.BatchId)
        {
            IsBatchIdUpdated = true; // incase of reset and sending 0;
            BatchId          = pqFieldUpdate.Payload;
            return 0;
        }

        if (pqFieldUpdate.Id == PQQuoteFields.QuoteSourceQuoteRef)
        {
            IsSourceQuoteReferenceUpdated = true; // incase of reset and sending 0;
            SourceQuoteReference          = pqFieldUpdate.Payload;
            return 0;
        }

        if (pqFieldUpdate.Id == PQQuoteFields.QuoteValueDate)
        {
            IsValueDateUpdated = true; // incase of reset and sending 0;
            PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref valueDate, pqFieldUpdate.Payload);
            if (valueDate == DateTime.UnixEpoch) valueDate = default;
            return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        if (recentlyTraded != null)
            foreach (var stringUpdate in recentlyTraded.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);
        if (found) return true;
        if (stringUpdate.Field.Id == PQQuoteFields.LastTradedDictionaryUpsertCommand)
        {
            if (recentlyTraded == null) recentlyTraded = new PQRecentlyTraded();
            return recentlyTraded.UpdateFieldString(stringUpdate);
        }

        return false;
    }

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);
        if (quote is IPQLevel3Quote pqLevel3Quote) recentlyTraded?.EnsureRelatedItemsAreConfigured(pqLevel3Quote.RecentlyTraded?.NameIdLookup);
    }

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    IPQLevel3Quote IPQLevel3Quote.Clone() => Clone();

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel3Quote otherL3)) return false;
        var baseSame = base.AreEquivalent(otherL3, exactTypes);
        var recentlyTradedSame = recentlyTraded?.AreEquivalent(otherL3.RecentlyTraded, exactTypes)
                              ?? otherL3.RecentlyTraded == null;
        var batchIdSame          = batchId == otherL3.BatchId;
        var sourceSequenceIdSame = sourceQuoteRef == otherL3.SourceQuoteReference;
        var valueDateSame        = ValueDate == otherL3.ValueDate;
        var allAreSame           = baseSame && recentlyTradedSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
        return allAreSame;
    }

    IPQLevel3Quote IPQLevel3Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel3Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var l3Q   = source as ILevel3Quote;
        var pql3Q = source as IPQLevel3Quote;
        if (pql3Q == null && l3Q != null)
        {
            // Only copy if changed
            if (recentlyTraded == null && l3Q.RecentlyTraded != null)
                recentlyTraded = new PQRecentlyTraded(l3Q.RecentlyTraded);
            else if (l3Q.RecentlyTraded != null) recentlyTraded?.CopyFrom(l3Q.RecentlyTraded, copyMergeFlags);
            BatchId              = l3Q.BatchId;
            SourceQuoteReference = l3Q.SourceQuoteReference;
            ValueDate            = l3Q.ValueDate;
        }
        else if (pql3Q != null)
        {
            // Only copy if changed
            if (recentlyTraded == null && pql3Q.RecentlyTraded != null)
                recentlyTraded = pql3Q.RecentlyTraded.Clone();
            else if (pql3Q.RecentlyTraded != null) recentlyTraded?.CopyFrom(pql3Q.RecentlyTraded, copyMergeFlags);

            var hasFullReplace = copyMergeFlags.HasFullReplace();

            if (pql3Q.IsBatchIdUpdated || hasFullReplace) BatchId                           = pql3Q.BatchId;
            if (pql3Q.IsSourceQuoteReferenceUpdated || hasFullReplace) SourceQuoteReference = pql3Q.SourceQuoteReference;
            if (pql3Q.IsValueDateUpdated || hasFullReplace) ValueDate                       = pql3Q.ValueDate;
            // ensure flags still match source

            if (hasFullReplace && pql3Q is PQLevel3Quote pqLevel3Quote) UpdatedFlags = pqLevel3Quote.UpdatedFlags;
        }

        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (recentlyTraded != null ? recentlyTraded.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)batchId;
            hashCode = (hashCode * 397) ^ (int)sourceQuoteRef;
            hashCode = (hashCode * 397) ^ valueDate.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({Level3ToStringMembers}, {UpdatedFlagsToString})";
}
