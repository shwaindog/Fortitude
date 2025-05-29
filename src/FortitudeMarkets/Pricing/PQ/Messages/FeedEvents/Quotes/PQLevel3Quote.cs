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

    private uint     sourceQuoteRef;
    private DateTime valueDate;

    public PQLevel3Quote()
    {
        // recentlyTraded = new PQRecentlyTraded();

        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQLevel3Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQLevel3Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null
      , uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null
      , DateTime? validFrom = null, DateTime? validTo = null, bool executable = true, decimal singlePrice = 0m)
        : base(sourceTickerInfo, sourceTime, orderBook, isBidPriceTopChanged, isAskPriceTopChanged, sourceBidTime, sourceAskTime
             , validFrom, validTo, executable, singlePrice)
    {
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
        }
        else if (toClone is ILevel3Quote l3QToClone)
        {
            BatchId              = l3QToClone.BatchId;
            SourceQuoteReference = l3QToClone.SourceQuoteReference;
            ValueDate            = l3QToClone.ValueDate;
        }
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel3Quote)) NumOfUpdates = 0;
    }


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

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates;
        set => base.HasUpdates = value;
    }

    IMutableLevel3Quote ITrackableReset<IMutableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel3Quote IMutableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel3Quote ITrackableReset<IPQLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel3Quote IPQLevel3Quote.ResetWithTracking() => ResetWithTracking();

    public override PQLevel3Quote ResetWithTracking()
    {
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
        if (!updatedOnly || IsBatchIdUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteBatchId, BatchId);

        if (!updatedOnly || IsSourceQuoteReferenceUpdated) yield return new PQFieldUpdate(PQFeedFields.QuoteSourceQuoteRef, SourceQuoteReference);
        if (!updatedOnly || IsValueDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.QuoteValueDate, valueDate.Get2MinIntervalsFromUnixEpoch());
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.QuoteBatchId:
                IsBatchIdUpdated = true; // incase of reset and sending 0;
                BatchId          = pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.QuoteSourceQuoteRef:
                IsSourceQuoteReferenceUpdated = true; // incase of reset and sending 0;
                SourceQuoteReference          = pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.QuoteValueDate:
                IsValueDateUpdated = true; // incase of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref valueDate, pqFieldUpdate.Payload);
                if (valueDate == DateTime.UnixEpoch) valueDate = default;
                return 0;
        }

        return base.UpdateField(pqFieldUpdate);
    }


    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);

        return found;
    }

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);
    }

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    IPQLevel3Quote IPQLevel3Quote.Clone() => Clone();

    public override PQLevel3Quote Clone() => Recycler?.Borrow<PQLevel3Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLevel3Quote(this);

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other is not IPQLevel3Quote && exactTypes) return false;
        if (other is not ILevel3Quote otherL3) return false;
        var baseSame             = base.AreEquivalent(otherL3, exactTypes);
        var batchIdSame          = batchId == otherL3.BatchId;
        var sourceSequenceIdSame = sourceQuoteRef == otherL3.SourceQuoteReference;
        var valueDateSame        = ValueDate == otherL3.ValueDate;
        var allAreSame           = baseSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
        return allAreSame;
    }

    IPQLevel3Quote IPQLevel3Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel3Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        var pql3Q = source as IPQLevel3Quote;
        if (pql3Q == null && source is ILevel3Quote l3Q)
        {
            BatchId              = l3Q.BatchId;
            SourceQuoteReference = l3Q.SourceQuoteReference;
            ValueDate            = l3Q.ValueDate;
        }
        else if (pql3Q != null)
        {
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
            hashCode = (hashCode * 397) ^ (int)batchId;
            hashCode = (hashCode * 397) ^ (int)sourceQuoteRef;
            hashCode = (hashCode * 397) ^ valueDate.GetHashCode();
            return hashCode;
        }
    }

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(BatchId)}: {BatchId}, {nameof(SourceQuoteReference)}: {SourceQuoteReference}, " +
        $"{nameof(ValueDate)}: {ValueDate}";

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {UpdatedFlagsToString})";
}

public class PQPublishableLevel3Quote : PQPublishableLevel2Quote, IPQPublishableLevel3Quote, ICloneable<PQPublishableLevel3Quote>
  , IDoublyLinkedListNode<PQPublishableLevel3Quote>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublishableLevel3Quote));

    private IPQOnTickLastTraded? onTickLastTraded;

    public PQPublishableLevel3Quote()
    {
        // recentlyTraded = new PQRecentlyTraded();

        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQPublishableLevel3Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m)
    {
        if (sourceTickerInfo.LastTradedFlags != LastTradedFlags.None)
        {
            onTickLastTraded ??= new PQOnTickLastTraded();
        }
    }

    public PQPublishableLevel3Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null, IOnTickLastTraded? onTickLastTraded = null
      , uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null, DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false
      , DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null, bool isAskPriceTopChanged = false
      , bool executable = true
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , decimal singlePrice = 0m, ICandle? conflationTicksCandle = null)
        : this(new PQLevel3Quote
                   (sourceTickerInfo, sourceTime, orderBook, batchId, sourceQuoteRef, valueDate, isBidPriceTopChanged, isAskPriceTopChanged
                  , sourceBidTime, sourceAskTime, validFrom, validTo, executable, singlePrice),
               sourceTickerInfo, onTickLastTraded, feedSyncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    protected PQPublishableLevel3Quote
    (IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo, IOnTickLastTraded? onTickLastTraded = null
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , ICandle? conflationTicksCandle = null)
        : base(initializedQuoteContainer, sourceTickerInfo, feedSyncStatus, feedConnectivityStatus, conflationTicksCandle)
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
        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    public PQPublishableLevel3Quote(IPublishableTickInstant toClone) : this(toClone, null) { }

    protected PQPublishableLevel3Quote(IPublishableTickInstant toClone, IPQTickInstant? initializedQuoteContainer)
        : base(toClone, initializedQuoteContainer)
    {
        if (toClone is IPQPublishableLevel3Quote ipql3QToClone)
        {
            onTickLastTraded = ipql3QToClone?.OnTickLastTraded?.Clone();
        }
        else if (toClone is IPublishableLevel3Quote { OnTickLastTraded: not null } l3QToClone)
        {
            onTickLastTraded = new PQOnTickLastTraded(l3QToClone.OnTickLastTraded);
        }
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQPublishableLevel3Quote)) NumOfUpdates = 0;
    }

    protected override IPQLevel3Quote CreateEmptyQuoteContainerInstant() => new PQLevel3Quote();

    protected override IPQLevel3Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new PQLevel3Quote(tickInstant);

    protected override IPQLevel3Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new PQLevel3Quote(tickerInfo);

    ILevel3Quote IPublishableLevel3Quote.AsNonPublishable => AsNonPublishable;

    IMutableLevel3Quote IMutablePublishableLevel3Quote.AsNonPublishable => AsNonPublishable;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IPQOnTickLastTraded? OnTickLastTraded
    {
        get => onTickLastTraded;
        set => onTickLastTraded = value as PQOnTickLastTraded;
    }

    [JsonIgnore]
    IMutableOnTickLastTraded? IMutablePublishableLevel3Quote.OnTickLastTraded
    {
        get => OnTickLastTraded;
        set => OnTickLastTraded = value as IPQOnTickLastTraded;
    }

    [JsonIgnore] IOnTickLastTraded? IPublishableLevel3Quote.OnTickLastTraded => OnTickLastTraded;

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

    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || (OnTickLastTraded?.HasUpdates ?? false);
        set
        {
            base.HasUpdates = value;
            if (OnTickLastTraded != null) OnTickLastTraded.HasUpdates = value;
        }
    }

    public override void UpdateComplete(uint updateId = 0)
    {
        OnTickLastTraded?.UpdateComplete(updateId);
        base.UpdateComplete(updateId);
    }

    public override PQPublishableLevel3Quote Clone() =>
        Recycler?.Borrow<PQPublishableLevel3Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQPublishableLevel3Quote(this, PQQuoteContainer.Clone());


    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    IPQLevel3Quote IPQLevel3Quote.Clone() => Clone();

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
        OnTickLastTraded?.StateReset();

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
                OnTickLastTraded ??= new PQOnTickLastTraded(PQSourceTickerInfo!);
            }
            OnTickLastTraded?.EnsureRelatedItemsAreConfigured(pqLevel3Quote.OnTickLastTraded?.NameIdLookup);
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
        if (onTickLastTraded != null)
            foreach (var recentlyTradedFields in onTickLastTraded.GetDeltaUpdateFields(snapShotTime,
                                                                                       messageFlags, quotePublicationPrecisionSettings))
                yield return recentlyTradedFields;
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQFeedFields.LastTradedStringUpdates) return (int)pqFieldUpdate.Payload;
        if (onTickLastTraded != null
         && pqFieldUpdate.Id is PQFeedFields.LastTradedTickTrades)
            return onTickLastTraded!.UpdateField(pqFieldUpdate);

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

    IPublishableLevel3Quote ICloneable<IPublishableLevel3Quote>.Clone() => Clone();

    IPublishableLevel3Quote IPublishableLevel3Quote.Clone() => Clone();

    IMutablePublishableLevel3Quote IMutablePublishableLevel3Quote.Clone() => Clone();

    IPQPublishableLevel3Quote IPQPublishableLevel3Quote.Clone() => Clone();

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (!(other is IPublishableLevel3Quote otherL3)) return false;
        var baseSame = base.AreEquivalent(otherL3, exactTypes);
        var onTickLastTradedSame = onTickLastTraded?.AreEquivalent(otherL3.OnTickLastTraded, exactTypes)
                                ?? otherL3.OnTickLastTraded == null;
        var allAreSame = baseSame && onTickLastTradedSame;
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
        var pql3Q = source as IPQPublishableLevel3Quote;
        if (pql3Q == null && source is IPublishableLevel3Quote l3Q)
        {
            // Only copy if changed
            if (onTickLastTraded == null && l3Q.OnTickLastTraded != null)
                onTickLastTraded = new PQOnTickLastTraded(l3Q.OnTickLastTraded);
            else if (l3Q.OnTickLastTraded != null) onTickLastTraded?.CopyFrom(l3Q.OnTickLastTraded, copyMergeFlags);
        }
        else if (pql3Q != null)
        {
            // Only copy if changed
            if (onTickLastTraded == null && pql3Q.OnTickLastTraded != null)
                onTickLastTraded = pql3Q.OnTickLastTraded.Clone();
            else if (pql3Q.OnTickLastTraded != null) onTickLastTraded?.CopyFrom(pql3Q.OnTickLastTraded, copyMergeFlags);

            var hasFullReplace = copyMergeFlags.HasFullReplace();

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
            hashCode = (hashCode * 397) ^ (OnTickLastTraded?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public override string QuoteToStringMembers => $"{base.QuoteToStringMembers}, {nameof(OnTickLastTraded)}: {OnTickLastTraded}";


    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {PQQuoteContainer.QuoteToStringMembers}, {UpdatedFlagsToString})";
}
