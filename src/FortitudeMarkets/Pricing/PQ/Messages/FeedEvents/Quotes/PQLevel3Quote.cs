// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public interface IPQLevel3Quote : IPQLevel2Quote, IMutableLevel3Quote, ITrackableReset<IPQLevel3Quote>
{
    new IPQOnTickLastTraded? OnTickLastTraded { get; set; }

    bool IsValueDateUpdated            { get; set; }
    bool IsBatchIdUpdated              { get; set; }
    bool IsSourceQuoteReferenceUpdated { get; set; }

    new IPQLevel3Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new bool AreEquivalent(ITickInstant source, bool exactTypes);

    new IPQLevel3Quote Clone();
    new IPQLevel3Quote ResetWithTracking();
}

public interface IPQPublishableLevel3Quote : IPQPublishableLevel2Quote, IMutablePublishableLevel3Quote, IPQLevel3Quote
  , IDoublyLinkedListNode<IPQPublishableLevel3Quote>, ITrackableReset<IPQPublishableLevel3Quote>
{
    new IPQOnTickLastTraded? OnTickLastTraded { get; set; }

    new IPQLevel3Quote AsNonPublishable { get; }

    new IPQPublishableLevel3Quote? Next     { get; set; }
    new IPQPublishableLevel3Quote? Previous { get; set; }
    new IPQPublishableLevel3Quote  CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags);

    new bool AreEquivalent(IPublishableTickInstant source, bool exactTypes);

    new IPQPublishableLevel3Quote Clone();
    new IPQPublishableLevel3Quote ResetWithTracking();
}

public class PQLevel3Quote : PQLevel2Quote, IPQLevel3Quote, ICloneable<PQLevel3Quote>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQLevel3Quote));

    private uint batchId;

    private IPQOnTickLastTraded? onTickLastTraded;

    private uint     sourceQuoteRef;
    private DateTime valueDate;

    public PQLevel3Quote()
    {
        // recentlyTraded = new PQRecentlyTraded();

        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQLevel3Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m)
    {
        if (sourceTickerInfo.LastTradedFlags != LastTradedFlags.None)
        {
            onTickLastTraded ??= new PQOnTickLastTraded();
        }
    }

    public PQLevel3Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null, IOnTickLastTraded? onTickLastTraded = null
      , uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null
      , DateTime? validFrom = null, DateTime? validTo = null, bool executable = true, decimal singlePrice = 0m)
        : base(sourceTickerInfo, sourceTime, orderBook, isBidPriceTopChanged, isAskPriceTopChanged, sourceBidTime, sourceAskTime
             , validFrom, validTo, executable, singlePrice)
    {
        if (onTickLastTraded is IPQOnTickLastTraded pqRecentlyTraded)
        {
            this.onTickLastTraded = pqRecentlyTraded;
        }
        else if (onTickLastTraded != null)
        {
            this.onTickLastTraded = new PQOnTickLastTraded(onTickLastTraded);
        }
        else if (sourceTickerInfo.LastTradedFlags != LastTradedFlags.None)
        {
            this.onTickLastTraded = new PQOnTickLastTraded(sourceTickerInfo);
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
            onTickLastTraded     = ipql3QToClone?.OnTickLastTraded?.Clone();
        }
        else if (toClone is ILevel3Quote l3QToClone)
        {
            BatchId              = l3QToClone.BatchId;
            SourceQuoteReference = l3QToClone.SourceQuoteReference;
            ValueDate            = l3QToClone.ValueDate;
            if (l3QToClone.OnTickLastTraded != null)
            {
                onTickLastTraded = new PQOnTickLastTraded(l3QToClone.OnTickLastTraded);
            }
        }
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel3Quote)) NumOfUpdates = 0;
    }

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(BatchId)}: {BatchId}, {nameof(SourceQuoteReference)}: {SourceQuoteReference}, " +
        $"{nameof(ValueDate)}: {ValueDate}, {nameof(OnTickLastTraded)}: {OnTickLastTraded}";

    public override PQLevel3Quote Clone() => Recycler?.Borrow<PQLevel3Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLevel3Quote(this);


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
    public IPQOnTickLastTraded? OnTickLastTraded
    {
        get => onTickLastTraded;
        set => onTickLastTraded = value as PQOnTickLastTraded;
    }

    [JsonIgnore]
    IMutableOnTickLastTraded? IMutableLevel3Quote.OnTickLastTraded
    {
        get => OnTickLastTraded;
        set => OnTickLastTraded = value as IPQOnTickLastTraded;
    }

    [JsonIgnore] IOnTickLastTraded? ILevel3Quote.OnTickLastTraded => OnTickLastTraded;

    [JsonIgnore]
    public override bool HasUpdates
    {
        get =>
            base.HasUpdates || (onTickLastTraded?.HasUpdates ?? false) || IsBatchIdUpdated ||
            IsSourceQuoteReferenceUpdated || IsValueDateUpdated;
        set
        {
            base.HasUpdates = IsBatchIdUpdated                        = IsSourceQuoteReferenceUpdated = IsValueDateUpdated = value;
            if (onTickLastTraded != null) onTickLastTraded.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        onTickLastTraded?.UpdateComplete();
        base.UpdateComplete();
    }

    IMutableLevel3Quote ITrackableReset<IMutableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel3Quote IMutableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel3Quote ITrackableReset<IPQLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel3Quote IPQLevel3Quote.ResetWithTracking() => ResetWithTracking();

    public override PQLevel3Quote ResetWithTracking()
    {
        onTickLastTraded?.StateReset();

        BatchId   = SourceQuoteReference = 0;
        ValueDate = default;
        base.ResetWithTracking();

        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        var updatedOnly = (messageFlags & StorageFlags.Complete) == 0;

        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime,
                                                               messageFlags, quotePublicationPrecisionSetting))
            yield return updatedField;
        if (onTickLastTraded != null)
            foreach (var recentlyTradedFields in onTickLastTraded.GetDeltaUpdateFields(snapShotTime,
                                                                                       messageFlags, quotePublicationPrecisionSetting))
                yield return recentlyTradedFields;
        if (!updatedOnly || IsBatchIdUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteBatchId, BatchId);

        if (!updatedOnly || IsSourceQuoteReferenceUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteSourceQuoteRef, SourceQuoteReference);
        if (!updatedOnly || IsValueDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteValueDate, valueDate.Get2MinIntervalsFromUnixEpoch());
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQFeedFields.LastTradedStringUpdates) return (int)pqFieldUpdate.Payload;
        if (onTickLastTraded != null
         && pqFieldUpdate.Id is >= PQFeedFields.LastTradedTickTrades and <= PQFeedFields.LastTradedRecently)
            return onTickLastTraded!.UpdateField(pqFieldUpdate);
        if (pqFieldUpdate.Id == PQFeedFields.QuoteBatchId)
        {
            IsBatchIdUpdated = true; // incase of reset and sending 0;
            BatchId          = pqFieldUpdate.Payload;
            return 0;
        }

        if (pqFieldUpdate.Id == PQFeedFields.QuoteSourceQuoteRef)
        {
            IsSourceQuoteReferenceUpdated = true; // incase of reset and sending 0;
            SourceQuoteReference          = pqFieldUpdate.Payload;
            return 0;
        }

        if (pqFieldUpdate.Id == PQFeedFields.QuoteValueDate)
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
        if (onTickLastTraded != null)
            foreach (var stringUpdate in onTickLastTraded.GetStringUpdates(snapShotTime, messageFlags))
                yield return stringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);
        if (found) return true;
        if (stringUpdate.Field.Id == PQFeedFields.LastTradedStringUpdates)
        {
            onTickLastTraded ??= new PQOnTickLastTraded();
            return onTickLastTraded.UpdateFieldString(stringUpdate);
        }

        return false;
    }

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);
        if (quote is IPQLevel3Quote pqLevel3Quote)
        {
            if (pqLevel3Quote.OnTickLastTraded != null)
            {
                onTickLastTraded ??= new PQOnTickLastTraded(pqLevel3Quote.OnTickLastTraded!);
            }
            onTickLastTraded?.EnsureRelatedItemsAreConfigured(pqLevel3Quote.OnTickLastTraded?.NameIdLookup);
        }
        if (quote is IPQPublishableLevel3Quote pqPubL3Quote)
        {
            if (pqPubL3Quote.OnTickLastTraded != null || pqPubL3Quote.SourceTickerInfo?.LastTradedFlags != LastTradedFlags.None)
            {
                onTickLastTraded ??= new PQOnTickLastTraded(pqPubL3Quote.SourceTickerInfo!);
            }
            onTickLastTraded?.EnsureRelatedItemsAreConfigured(pqPubL3Quote.OnTickLastTraded?.NameIdLookup);
        }
    }

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    IPQLevel3Quote IPQLevel3Quote.Clone() => Clone();

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other is not IPQLevel3Quote && exactTypes) return false;
        if (other is not ILevel3Quote otherL3) return false;
        var baseSame = base.AreEquivalent(otherL3, exactTypes);
        var recentlyTradedSame = onTickLastTraded?.AreEquivalent(otherL3.OnTickLastTraded, exactTypes)
                              ?? otherL3.OnTickLastTraded == null;
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
        var pql3Q = source as IPQLevel3Quote;
        if (pql3Q == null && source is ILevel3Quote l3Q)
        {
            // Only copy if changed
            if (onTickLastTraded == null && l3Q.OnTickLastTraded != null)
                onTickLastTraded = new PQOnTickLastTraded(l3Q.OnTickLastTraded);
            else if (l3Q.OnTickLastTraded != null) onTickLastTraded?.CopyFrom(l3Q.OnTickLastTraded, copyMergeFlags);
            BatchId              = l3Q.BatchId;
            SourceQuoteReference = l3Q.SourceQuoteReference;
            ValueDate            = l3Q.ValueDate;
        }
        else if (pql3Q != null)
        {
            // Only copy if changed
            if (onTickLastTraded == null && pql3Q.OnTickLastTraded != null)
                onTickLastTraded = pql3Q.OnTickLastTraded.Clone();
            else if (pql3Q.OnTickLastTraded != null) onTickLastTraded?.CopyFrom(pql3Q.OnTickLastTraded, copyMergeFlags);

            var hasFullReplace = copyMergeFlags.HasFullReplace();

            if (pql3Q.IsBatchIdUpdated || hasFullReplace) BatchId                           = pql3Q.BatchId;
            if (pql3Q.IsSourceQuoteReferenceUpdated || hasFullReplace) SourceQuoteReference = pql3Q.SourceQuoteReference;
            if (pql3Q.IsValueDateUpdated || hasFullReplace) ValueDate                       = pql3Q.ValueDate;
            // ensure flags still match source

            if (hasFullReplace) SetFlagsSame(pql3Q);
        }

        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishableTickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (onTickLastTraded != null ? onTickLastTraded.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (int)batchId;
            hashCode = (hashCode * 397) ^ (int)sourceQuoteRef;
            hashCode = (hashCode * 397) ^ valueDate.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {UpdatedFlagsToString})";
}

public class PQPublishableLevel3Quote : PQPublishableLevel2Quote, IPQPublishableLevel3Quote, ICloneable<PQPublishableLevel3Quote>
  , IDoublyLinkedListNode<PQPublishableLevel3Quote>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublishableLevel3Quote));

    public PQPublishableLevel3Quote()
    {
        // recentlyTraded = new PQRecentlyTraded();

        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQPublishableLevel3Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQPublishableLevel3Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null , IOrderBook? orderBook = null, IOnTickLastTraded? onTickLastTraded = null
      , uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null, DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false
      , DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null, bool isAskPriceTopChanged = false, bool executable = true
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , decimal singlePrice = 0m, ICandle? conflationTicksCandle = null)
        : this(new PQLevel3Quote
                   (sourceTickerInfo, sourceTime, orderBook, onTickLastTraded, batchId, sourceQuoteRef, valueDate, isBidPriceTopChanged, isAskPriceTopChanged
                  , sourceBidTime, sourceAskTime, validFrom, validTo, executable, singlePrice),
               sourceTickerInfo, feedSyncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    protected PQPublishableLevel3Quote
    (IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None, ICandle? conflationTicksCandle = null)
        : base(initializedQuoteContainer, sourceTickerInfo, feedSyncStatus,feedConnectivityStatus, conflationTicksCandle)
    {
        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    public PQPublishableLevel3Quote(IPublishableTickInstant toClone) : this(toClone, null) { }

    protected PQPublishableLevel3Quote(IPublishableTickInstant toClone, IPQTickInstant? initializedQuoteContainer)
        : base(toClone, initializedQuoteContainer)
    {
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    protected override IPQLevel3Quote CreateEmptyQuoteContainerInstant() => new PQLevel3Quote();

    protected override IPQLevel3Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new PQLevel3Quote(tickInstant);

    protected override IPQLevel3Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new PQLevel3Quote(tickerInfo);

    public override PQPublishableLevel3Quote Clone() =>
        Recycler?.Borrow<PQPublishableLevel3Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQPublishableLevel3Quote(this, PQQuoteContainer.Clone());


    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    IPQLevel3Quote IPQLevel3Quote.Clone() => Clone();

    [JsonIgnore]
    public new PQPublishableLevel3Quote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PQPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PQPublishableLevel3Quote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PQPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQPublishableLevel3Quote? IPQPublishableLevel3Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel3Quote? IPQPublishableLevel3Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel3Quote? IPublishableLevel3Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel3Quote? IPublishableLevel3Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel3Quote? IDoublyLinkedListNode<IPublishableLevel3Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel3Quote? IDoublyLinkedListNode<IPublishableLevel3Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQPublishableLevel3Quote? IDoublyLinkedListNode<IPQPublishableLevel3Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel3Quote? IDoublyLinkedListNode<IPQPublishableLevel3Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQPublishableLevel3Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level3Quote;

    public override IPQLevel3Quote AsNonPublishable => (IPQLevel3Quote)PQQuoteContainer;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId
    {
        get => AsNonPublishable.BatchId;
        set => AsNonPublishable.BatchId = value;
    }

    [JsonIgnore]
    public bool IsBatchIdUpdated
    {
        get => AsNonPublishable.IsBatchIdUpdated;
        set => AsNonPublishable.IsBatchIdUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference
    {
        get => AsNonPublishable.SourceQuoteReference;
        set => AsNonPublishable.SourceQuoteReference = value;
    }

    [JsonIgnore]
    public bool IsSourceQuoteReferenceUpdated
    {
        get => AsNonPublishable.IsSourceQuoteReferenceUpdated;
        set => AsNonPublishable.IsSourceQuoteReferenceUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate
    {
        get => AsNonPublishable.ValueDate;
        set => AsNonPublishable.ValueDate = value;
    }

    [JsonIgnore]
    public bool IsValueDateUpdated
    {
        get => AsNonPublishable.IsValueDateUpdated;
        set => AsNonPublishable.IsValueDateUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IPQOnTickLastTraded? OnTickLastTraded
    {
        get => AsNonPublishable.OnTickLastTraded;
        set => AsNonPublishable.OnTickLastTraded = value;
    }

    [JsonIgnore]
    IMutableOnTickLastTraded? IMutableLevel3Quote.OnTickLastTraded
    {
        get => AsNonPublishable.OnTickLastTraded;
        set => AsNonPublishable.OnTickLastTraded = value as IPQOnTickLastTraded;
    }

    [JsonIgnore] IOnTickLastTraded? ILevel3Quote.OnTickLastTraded => OnTickLastTraded;

    [JsonIgnore]
    public override bool HasUpdates
    {
        get =>
            base.HasUpdates || (OnTickLastTraded?.HasUpdates ?? false) || IsBatchIdUpdated ||
            IsSourceQuoteReferenceUpdated || IsValueDateUpdated;
        set
        {
            base.HasUpdates = IsBatchIdUpdated = IsSourceQuoteReferenceUpdated = IsValueDateUpdated = value;
            if (AsNonPublishable.OnTickLastTraded != null) AsNonPublishable.OnTickLastTraded.HasUpdates = value;
        }
    }

    public override void UpdateComplete()
    {
        AsNonPublishable.OnTickLastTraded?.UpdateComplete();
        base.UpdateComplete();
    }

    IMutableLevel3Quote ITrackableReset<IMutableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel3Quote IMutableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel3Quote ITrackableReset<IMutablePublishableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel3Quote IMutablePublishableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel3Quote ITrackableReset<IPQLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel3Quote IPQLevel3Quote.ResetWithTracking() => ResetWithTracking();

    IPQPublishableLevel3Quote ITrackableReset<IPQPublishableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IPQPublishableLevel3Quote IPQPublishableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    public override PQPublishableLevel3Quote ResetWithTracking()
    {
        AsNonPublishable.OnTickLastTraded?.StateReset();

        BatchId   = SourceQuoteReference = 0;
        ValueDate = default;
        base.ResetWithTracking();
        return this;
    }


    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);
        if (quote is IPQPublishableLevel3Quote pqLevel3Quote)
        {
            if (pqLevel3Quote.OnTickLastTraded != null || PQSourceTickerInfo?.LastTradedFlags != LastTradedFlags.None)
            {
                AsNonPublishable.OnTickLastTraded ??= new PQOnTickLastTraded(PQSourceTickerInfo!);
            }
            AsNonPublishable.OnTickLastTraded?.EnsureRelatedItemsAreConfigured(pqLevel3Quote.OnTickLastTraded?.NameIdLookup);
        }
    }

    // must include this event though it looks like it does the same as default inherited.  It ensures that subtypes forward to the next level
    // down and not it's immediate parent
    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, StorageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var level2QuoteUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return level2QuoteUpdates;
        }
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);

        return found;
    }

    IPublishableLevel3Quote ICloneable<IPublishableLevel3Quote>.Clone() => Clone();

    IPublishableLevel3Quote IPublishableLevel3Quote.Clone() => Clone();

    IMutablePublishableLevel3Quote IMutablePublishableLevel3Quote.Clone() => Clone();

    IPQPublishableLevel3Quote IPQPublishableLevel3Quote.Clone() => Clone();

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (!(other is IPublishableLevel3Quote otherL3)) return false;
        var baseSame   = base.AreEquivalent(otherL3, exactTypes);
        var allAreSame = baseSame;
        return allAreSame;
    }

    IPQPublishableLevel3Quote IPQPublishableLevel3Quote.CopyFrom
        (IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPQLevel3Quote IPQLevel3Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPublishableTickInstant pubTickInstant)
        {
            CopyFrom(pubTickInstant, copyMergeFlags);
        }
        else
        {
            AsNonPublishable.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    public override PQPublishableLevel3Quote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var hasFullReplace = copyMergeFlags.HasFullReplace();

        if (hasFullReplace) SetFlagsSame(source);

        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishableTickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (OnTickLastTraded?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ BatchId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SourceQuoteReference;
            hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
            return hashCode;
        }
    }


    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {PQQuoteContainer.QuoteToStringMembers}, {UpdatedFlagsToString})";
}
