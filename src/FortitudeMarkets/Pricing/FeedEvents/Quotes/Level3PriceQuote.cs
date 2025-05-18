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
    (ISourceTickerInfo sourceTickerInfo, decimal singlePrice = 0m, bool isReplay = false, DateTime? sourceTime = null,  DateTime? sourceBidTime = null
      , bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null
      , bool isAskPriceTopChanged = false, bool executable = false, IOrderBook? orderBook = null, IOnTickLastTraded? onTickLastTraded = null, 
        uint batchId = 0u, uint sourceQuoteRef = 0u, DateTime? valueDate = null)
        : base(sourceTickerInfo, singlePrice, isReplay, sourceTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, 
               validFrom, validTo, isAskPriceTopChanged, executable
              ,
               orderBook ?? new OrderBook(sourceTickerInfo))
    {
        if (onTickLastTraded is OnTickLastTraded mutableOnTickLastTraded)
            OnTickLastTraded = mutableOnTickLastTraded;

        else if (onTickLastTraded != null)
            OnTickLastTraded = new OnTickLastTraded(onTickLastTraded);

        else if (sourceTickerInfo.LastTradedFlags != LastTradedFlags.None) OnTickLastTraded = new OnTickLastTraded(sourceTickerInfo);

        BatchId              = batchId;
        SourceQuoteReference = sourceQuoteRef;
        ValueDate            = valueDate ?? DateTime.MinValue;
    }

    public Level3PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel3Quote level3ToClone)
        {
            if (level3ToClone.OnTickLastTraded is OnTickLastTraded pqOnTickLast)
                OnTickLastTraded                                              = pqOnTickLast.Clone();
            else if (level3ToClone.OnTickLastTraded != null) OnTickLastTraded = new OnTickLastTraded(level3ToClone.OnTickLastTraded);

            BatchId              = level3ToClone.BatchId;
            SourceQuoteReference = level3ToClone.SourceQuoteReference;
            ValueDate            = level3ToClone.ValueDate;
        }
    }

    public override Level3PriceQuote Clone() => Recycler?.Borrow<Level3PriceQuote>().CopyFrom(this) ?? new Level3PriceQuote(this);

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IMutableOnTickLastTraded? OnTickLastTraded { get; set; }

    [JsonIgnore] IOnTickLastTraded? ILevel3Quote.OnTickLastTraded => OnTickLastTraded;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint BatchId { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public uint SourceQuoteReference { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime ValueDate { get; set; }

    public override Level3PriceQuote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel3Quote level3Quote)
        {
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

        var lastTradesSame = exactTypes
            ? Equals(OnTickLastTraded, otherL3.OnTickLastTraded)
            : OnTickLastTraded?.AreEquivalent(otherL3.OnTickLastTraded) ?? otherL3.OnTickLastTraded == null;
        var batchIdSame          = BatchId == otherL3.BatchId;
        var sourceSequenceIdSame = SourceQuoteReference == otherL3.SourceQuoteReference;
        var valueDateSame        = ValueDate == otherL3.ValueDate;

        return baseIsSame && lastTradesSame && batchIdSame && sourceSequenceIdSame && valueDateSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel3Quote, true);

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

    public override string QuoteToStringMembers =>
        $"{base.QuoteToStringMembers}, {nameof(OnTickLastTraded)}: {OnTickLastTraded}, {nameof(BatchId)}: {BatchId}, " +
        $"{nameof(SourceQuoteReference)}: {SourceQuoteReference}, {nameof(ValueDate)}: {ValueDate:u}";

    public override string ToString() => $"{nameof(Level3PriceQuote)}{{{QuoteToStringMembers}}}";
}

public class PublishableLevel3PriceQuote : PublishableLevel2PriceQuote, IMutablePublishableLevel3Quote, ICloneable<PublishableLevel3PriceQuote>
  , IDoublyLinkedListNode<PublishableLevel3PriceQuote>
{
    public PublishableLevel3PriceQuote() { }

    public PublishableLevel3PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , decimal singlePrice = 0m
      , DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, DateTime? sourceBidTime = null
      , bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null, DateTime? validTo = null
      , bool isAskPriceTopChanged = false
      , bool executable = false, ICandle? conflationTicksCandle = null, IOrderBook? orderBook = null,
        IOnTickLastTraded? onTickLastTraded = null, uint batchId = 0u, uint sourceQuoteRef = 0u
      , DateTime? valueDate = null)
        : this(new Level3PriceQuote(sourceTickerInfo, singlePrice, isReplay, sourceTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, validFrom, validTo, 
                                    isAskPriceTopChanged, executable, orderBook, onTickLastTraded, batchId, sourceQuoteRef, valueDate )
             , sourceTickerInfo, feedSyncStatus, clientReceivedTime, adapterReceivedTime, adapterSentTime, conflationTicksCandle)
    {
    }

    protected PublishableLevel3PriceQuote
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null
      , DateTime? adapterSentTime = null, ICandle? conflationTicksCandle = null)
        : base(initialisedQuoteContainer, sourceTickerInfo, feedSyncStatus, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, conflationTicksCandle)
    {
    }

    public PublishableLevel3PriceQuote(IPublishableTickInstant toClone) : this(toClone, null) { }

    protected PublishableLevel3PriceQuote(IPublishableTickInstant toClone, IMutableTickInstant? initializedQuoteContainer)
        : base(toClone, initializedQuoteContainer)
    {
    }

    protected override IMutableLevel3Quote CreateEmptyQuoteContainerInstant() => new Level3PriceQuote();

    protected override IMutableLevel3Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new Level3PriceQuote(tickInstant);

    protected override IMutableLevel3Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new Level3PriceQuote(tickerInfo);

    public override PublishableLevel3PriceQuote Clone() =>
        Recycler?.Borrow<PublishableLevel3PriceQuote>().CopyFrom(this) ?? new PublishableLevel3PriceQuote(this, QuoteContainer.Clone());

    ILevel3Quote ICloneable<ILevel3Quote>.Clone() => Clone();

    ILevel3Quote ILevel3Quote.Clone() => Clone();

    IMutableLevel3Quote IMutableLevel3Quote.Clone() => Clone();


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

    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level3Quote;

    public override IMutableLevel3Quote AsNonPublishable => (IMutableLevel3Quote)QuoteContainer;


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IMutableOnTickLastTraded? OnTickLastTraded
    {
        get => AsNonPublishable.OnTickLastTraded;
        set => AsNonPublishable.OnTickLastTraded = value;
    }

    [JsonIgnore] IOnTickLastTraded? ILevel3Quote.OnTickLastTraded => AsNonPublishable.OnTickLastTraded;


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

    public override PublishableLevel3PriceQuote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
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

        return baseIsSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPublishableTickInstant?)obj, true);

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

    public override string ToString() => $"{nameof(PublishableLevel3PriceQuote)}{{{QuoteToStringMembers}, {AsNonPublishable.QuoteToStringMembers}}}";
}
