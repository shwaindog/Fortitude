// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public class Level3PriceQuote : Level2PriceQuote, IMutableLevel3Quote, ICloneable<Level3PriceQuote>
{
    public Level3PriceQuote() { }

    public Level3PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null
      , QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None, uint batchId = 0u, uint sourceQuoteRef = 0u
      , DateTime? valueDate = null, bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false,
        DateTime? sourceBidTime = null, DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null
      , bool executable = false, decimal singlePrice = 0m)
        : base(sourceTickerInfo, sourceTime, orderBook, quoteBehavior, isBidPriceTopChanged, isAskPriceTopChanged, sourceBidTime
             , sourceAskTime, validFrom, validTo, executable, singlePrice)
    {
        BatchId              = batchId;
        SourceQuoteReference = sourceQuoteRef;
        ValueDate            = valueDate ?? DateTime.MinValue;
    }

    public Level3PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel3Quote level3ToClone)
        {
            BatchId              = level3ToClone.BatchId;
            SourceQuoteReference = level3ToClone.SourceQuoteReference;
            ValueDate            = level3ToClone.ValueDate;
        }
    }

    public override Level3PriceQuote Clone() => Recycler?.Borrow<Level3PriceQuote>().CopyFrom(this) ?? new Level3PriceQuote(this);

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate { get; set; }


    IMutableLevel3Quote ITrackableReset<IMutableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel3Quote IMutableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    public override Level3PriceQuote ResetWithTracking()
    {
        BatchId              = 0;
        SourceQuoteReference = 0;
        ValueDate            = DateTime.MinValue;

        base.ResetWithTracking();
        return this;
    }

    public override Level3PriceQuote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel3Quote level3Quote)
        {
            BatchId              = level3Quote.BatchId;
            SourceQuoteReference = level3Quote.SourceQuoteReference;
            ValueDate            = level3Quote.ValueDate;
        }

        return this;
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other is not ILevel3Quote otherL3) return false;
        var baseIsSame = base.AreEquivalent(otherL3, exactTypes);

        var batchIdSame          = BatchId == otherL3.BatchId;
        var sourceSequenceIdSame = SourceQuoteReference == otherL3.SourceQuoteReference;
        var valueDateSame        = ValueDate == otherL3.ValueDate;

        return baseIsSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel3Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ BatchId.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SourceQuoteReference;
            hashCode = (hashCode * 397) ^ ValueDate.GetHashCode();
            return hashCode;
        }
    }

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(BatchId)}: {BatchId}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference}, {nameof(ValueDate)}: {ValueDate:u}";

    public override string ToString() => $"{nameof(Level3PriceQuote)}{{{QuoteToStringMembers}}}";
}

public class PublishableLevel3PriceQuote : PublishableLevel2PriceQuote, IMutablePublishableLevel3Quote, ICloneable<PublishableLevel3PriceQuote>
  , IDoublyLinkedListNode<PublishableLevel3PriceQuote>
{
    public PublishableLevel3PriceQuote() { }

    public PublishableLevel3PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null, IOnTickLastTraded? onTickLastTraded = null
       ,PublishableQuoteInstantBehaviorFlags quoteBehavior = PublishableQuoteInstantBehaviorFlags.None, uint batchId = 0u, uint sourceQuoteRef = 0u
      , DateTime? valueDate = null, bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null
      , DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null, bool executable = false
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , decimal singlePrice = 0m, ICandle? conflationTicksCandle = null)
        : this(new Level3PriceQuote
                   (sourceTickerInfo, sourceTime, orderBook, (QuoteInstantBehaviorFlags)quoteBehavior, batchId, sourceQuoteRef, valueDate
                  , isBidPriceTopChanged, isAskPriceTopChanged, sourceBidTime, sourceAskTime, validFrom, validTo, executable, singlePrice)
             , sourceTickerInfo, onTickLastTraded, feedSyncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    protected PublishableLevel3PriceQuote
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo, IOnTickLastTraded? onTickLastTraded = null
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , ICandle? conflationTicksCandle = null)
        : base(initialisedQuoteContainer, sourceTickerInfo, feedSyncStatus, feedConnectivityStatus, conflationTicksCandle)
    {
        if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return;
        if (onTickLastTraded is OnTickLastTraded mutableOnTickLastTraded)
            OnTickLastTraded = mutableOnTickLastTraded;
        else if (onTickLastTraded != null)
            OnTickLastTraded = new OnTickLastTraded(onTickLastTraded);

        else if (sourceTickerInfo.LastTradedFlags != LastTradedFlags.None) OnTickLastTraded = new OnTickLastTraded(sourceTickerInfo);
    }

    public PublishableLevel3PriceQuote(IPublishableTickInstant toClone) : this(toClone, null)
    {
    }

    protected PublishableLevel3PriceQuote(IPublishableTickInstant toClone, IMutableTickInstant? initializedQuoteContainer)
        : base(toClone, initializedQuoteContainer)
    {
        if (toClone is IPublishableLevel3Quote level3ToClone)
        {
            if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return;
            if (level3ToClone.OnTickLastTraded is OnTickLastTraded pqOnTickLast)
                OnTickLastTraded = pqOnTickLast.Clone();

            else if (level3ToClone.OnTickLastTraded != null) OnTickLastTraded = new OnTickLastTraded(level3ToClone.OnTickLastTraded);
        }
    }

    protected override IMutableLevel3Quote CreateEmptyQuoteContainerInstant() => new Level3PriceQuote();

    protected override IMutableLevel3Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new Level3PriceQuote(tickInstant);

    protected override IMutableLevel3Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new Level3PriceQuote(tickerInfo);


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IMutableOnTickLastTraded? OnTickLastTraded { get; set; }

    [JsonIgnore] IOnTickLastTraded? IPublishableLevel3Quote.OnTickLastTraded => OnTickLastTraded;


    ILevel3Quote IPublishableLevel3Quote.AsNonPublishable => AsNonPublishable;

    public override IMutableLevel3Quote AsNonPublishable => (IMutableLevel3Quote)QuoteContainer;


    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level3Quote;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId
    {
        get => AsNonPublishable.BatchId;
        set => AsNonPublishable.BatchId = value;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference
    {
        get => AsNonPublishable.SourceQuoteReference;
        set => AsNonPublishable.SourceQuoteReference = value;
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate
    {
        get => AsNonPublishable.ValueDate;
        set => AsNonPublishable.ValueDate = value;
    }
    

    [JsonIgnore]
    public new PublishableLevel3PriceQuote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PublishableLevel3PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PublishableLevel3PriceQuote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PublishableLevel3PriceQuote;
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

    public override PublishableLevel3PriceQuote Clone() =>
        Recycler?.Borrow<PublishableLevel3PriceQuote>().CopyFrom(this) ?? new PublishableLevel3PriceQuote(this, QuoteContainer.Clone());

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();

    IMutableLevel3Quote ITrackableReset<IMutableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel3Quote IMutableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel3Quote ITrackableReset<IMutablePublishableLevel3Quote>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel3Quote IMutablePublishableLevel3Quote.ResetWithTracking() => ResetWithTracking();

    public override PublishableLevel3PriceQuote ResetWithTracking()
    {
        OnTickLastTraded?.ResetWithTracking();
        base.ResetWithTracking();
        return this;
    }

    public override PublishableLevel3PriceQuote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is IPublishableLevel3Quote level3Quote)
        {
            if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return this;
            if (level3Quote.OnTickLastTraded != null)
            {
                if (OnTickLastTraded != null)
                    OnTickLastTraded.CopyFrom(level3Quote.OnTickLastTraded);
                else
                    OnTickLastTraded = new OnTickLastTraded(level3Quote.OnTickLastTraded);
            }
            else if (OnTickLastTraded != null)
            {
                OnTickLastTraded = null;
            }
        }
        return this;
    }

    IPublishableLevel3Quote ICloneable<IPublishableLevel3Quote>.Clone() => Clone();

    IPublishableLevel3Quote IPublishableLevel3Quote.Clone() => Clone();

    IMutablePublishableLevel3Quote IMutablePublishableLevel3Quote.Clone() => Clone();

    bool IMutableLevel3Quote.AreEquivalent(ITickInstant? other, bool exactTypes) => AreEquivalent(other as IPublishableTickInstant, exactTypes);

    bool ILevel3Quote.AreEquivalent(ITickInstant? other, bool exactTypes) => AreEquivalent(other as IPublishableTickInstant, exactTypes);

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (!(other is IPublishableLevel3Quote otherL3)) return false;
        var baseIsSame = base.AreEquivalent(otherL3, exactTypes);

        if (QuoteBehavior.HasNoPublishableQuoteUpdatesFlag()) return baseIsSame;

        var lastTradesSame = exactTypes
            ? Equals(OnTickLastTraded, otherL3.OnTickLastTraded)
            : OnTickLastTraded?.AreEquivalent(otherL3.OnTickLastTraded) ?? otherL3.OnTickLastTraded == null;

        return baseIsSame && lastTradesSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPublishableTickInstant?)obj, true);

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

    public override string ToString() => 
        $"{nameof(PublishableLevel3PriceQuote)}{{{QuoteToStringMembers}, {AsNonPublishable.QuoteToStringMembers}, " +
        $"{JustFeedSyncConnectivityStatusToStringMembers}}}";
}
