// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public interface IPQLevel2Quote : IPQLevel1Quote, IMutableLevel2Quote, ITrackableReset<IPQLevel2Quote>
{
    new IPQOrderBook OrderBook { get; set; }

    new IPQOrderBookSide BidBook { get; set; }
    new IPQOrderBookSide AskBook { get; set; }

    new IPQLevel2Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new bool AreEquivalent(ITickInstant source, bool exactTypes);

    new IPQLevel2Quote Clone();
    new IPQLevel2Quote ResetWithTracking();
}

public interface IPQPublishableLevel2Quote : IPQPublishableLevel1Quote, IMutablePublishableLevel2Quote, IPQLevel2Quote
  , IDoublyLinkedListNode<IPQPublishableLevel2Quote>, ITrackableReset<IPQPublishableLevel2Quote>
{
    new IPQOrderBook OrderBook { get; set; }

    new IPQOrderBookSide BidBook { get; set; }
    new IPQOrderBookSide AskBook { get; set; }

    new IPQLevel2Quote AsNonPublishable { get; }

    new IPQPublishableLevel2Quote? Next     { get; set; }
    new IPQPublishableLevel2Quote? Previous { get; set; }

    new IPQPublishableLevel2Quote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags);
    new bool                      AreEquivalent(IPublishableTickInstant source, bool exactTypes);

    new IPQPublishableLevel2Quote Clone();
    new IPQPublishableLevel2Quote ResetWithTracking();
}

public class PQLevel2Quote : PQLevel1Quote, IPQLevel2Quote, ICloneable<PQLevel2Quote>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublishableLevel2Quote));

    private IPQOrderBook orderBook;

    public PQLevel2Quote()
    {
        orderBook = new PQOrderBook();
        if (GetType() == typeof(PQPublishableLevel2Quote)) SequenceId = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQLevel2Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQLevel2Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null
       , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null,
        DateTime? validFrom = null, DateTime? validTo = null, bool executable = true,  decimal singlePrice = 0m)
        : base(sourceTickerInfo, sourceTime, 0m, 0m, isBidPriceTopChanged, isAskPriceTopChanged, sourceBidTime, sourceAskTime,
               validFrom, validTo,  executable,  singlePrice)
    {
        if (orderBook is IPQOrderBook pqOrderBook)
        {
            this.orderBook = pqOrderBook;
        }
        else if (orderBook != null)
        {
            this.orderBook = new PQOrderBook(orderBook);
        }
        else
        {
            this.orderBook = new PQOrderBook(sourceTickerInfo);
        }
        OrderBook.EnsureRelatedItemsAreConfigured(sourceTickerInfo, null);

        if (GetType() == typeof(PQLevel2Quote)) SequenceId = 0;
    }

    public PQLevel2Quote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is IPQLevel2Quote l2QToClone)
        {
            orderBook = l2QToClone.OrderBook.Clone();

            IsBidPriceTopUpdated = l2QToClone.IsBidPriceTopUpdated;
            IsAskPriceTopUpdated = l2QToClone.IsAskPriceTopUpdated;
            IsBidPriceTopChanged = l2QToClone.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2QToClone.IsAskPriceTopChanged;

            IsBidPriceTopChangedUpdated = l2QToClone.IsBidPriceTopChangedUpdated;
            IsAskPriceTopChangedUpdated = l2QToClone.IsAskPriceTopChangedUpdated;
        }
        else if (toClone is ILevel2Quote l2Q)
        {
            orderBook = new PQOrderBook(l2Q.OrderBook);

            IsBidPriceTopChanged = l2Q.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2Q.IsAskPriceTopChanged;
        }
        else
        {
            orderBook = new PQOrderBook();
        }

        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(toClone as IPQTickInstant);
        SetFlagsSame(toClone);

        if (GetType() == typeof(PQLevel2Quote)) SequenceId = 0;
    }

    public override string QuoteToStringMembers => $"{base.QuoteToStringMembers}, {nameof(OrderBook)}: {OrderBook}";

    public override PQLevel2Quote Clone() => Recycler?.Borrow<PQLevel2Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new PQLevel2Quote(this);


    [JsonIgnore]
    public override bool HasUpdates
    {
        get => base.HasUpdates || orderBook.HasUpdates;
        set
        {
            base.HasUpdates      = value;
            orderBook.HasUpdates = value;
        }
    }

    IOrderBook ILevel2Quote.OrderBook => orderBook;
    IMutableOrderBook IMutableLevel2Quote.OrderBook
    {
        get => orderBook;
        set => orderBook = (IPQOrderBook)value;
    }

    public IPQOrderBook OrderBook
    {
        get => orderBook;
        set { orderBook = value; }
    }

    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBook => BidBook;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBook => AskBook;

    public IPQOrderBookSide BidBook
    {
        get => orderBook.BidSide;
        set
        {
            orderBook.BidSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    public IPQOrderBookSide AskBook
    {
        get => orderBook.AskSide;
        set
        {
            orderBook.AskSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBook.Count > 0 ? BidBook[0]?.Price ?? 0 : 0;
        set
        {
            IsBidPriceTopUpdated = BidBook[0]!.Price != value || SequenceId == 0;
            if (BidBook[0]!.Price == value) return;
            BidBook[0]!.Price    = value;
            IsBidPriceTopUpdated = true;
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBook.Count > 0 ? AskBook[0]?.Price ?? 0 : 0;
        set
        {
            IsAskPriceTopUpdated = AskBook[0]!.Price != value || SequenceId == 0;
            if (AskBook[0]!.Price == value) return;
            AskBook[0]!.Price    = value;
            IsAskPriceTopUpdated = true;
        }
    }

    public override void UpdateStarted(uint updateSequenceId)
    {
        OrderBook.UpdateStarted(updateSequenceId);
        base.UpdateStarted(updateSequenceId);
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        OrderBook.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
    }

    IMutableLevel2Quote ITrackableReset<IMutableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel2Quote IMutableLevel2Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel2Quote ITrackableReset<IPQLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel2Quote IPQLevel2Quote.ResetWithTracking() => ResetWithTracking();

    public override PQLevel2Quote ResetWithTracking()
    {
        orderBook.ResetWithTracking();

        base.ResetWithTracking();

        return this;
    }

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSetting = null)
    {
        foreach (var updatedField in base.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
            yield return updatedField;
        foreach (var bidFields in orderBook.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSetting))
            yield return bidFields;
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        if (pqFieldUpdate.Id == PQFeedFields.QuoteLayerStringUpdates) return (int)pqFieldUpdate.Payload;
        if (pqFieldUpdate.Id is >= PQFeedFields.QuoteOpenInterestTotal and <= PQFeedFields.QuoteLayersRangeEnd)
        {
            var result = orderBook.UpdateField(pqFieldUpdate);
            // pass Best Price through to Level 1 quote 
            if (!(pqFieldUpdate.Id == PQFeedFields.QuoteLayerPrice && pqFieldUpdate.DepthId.KeyToDepth() == 0))
            {
                return result;
            }
        }

        return base.UpdateField(pqFieldUpdate);
    }

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    IPQLevel2Quote IPQLevel2Quote.Clone() => Clone();

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? item)
    {
        base.EnsureRelatedItemsAreConfigured(item);
        if (item is IPublishableLevel2Quote l2Quote)
        {
            OrderBook.EnsureRelatedItemsAreConfigured(l2Quote.SourceTickerInfo, l2Quote.OrderBook);
        }
    }

    public override void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? item)
    {
        orderBook.EnsureRelatedItemsAreConfigured(item, null);
        base.EnsureRelatedItemsAreConfigured(item);
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other is not IPQLevel2Quote && exactTypes) return false;
        if (other is not ILevel2Quote otherL2) return false;
        var baseSame      = base.AreEquivalent(other, exactTypes);
        var orderBookSame = OrderBook.AreEquivalent(otherL2.OrderBook, exactTypes);

        var allAreSame = baseSame && orderBookSame;
        return allAreSame;
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in orderBook.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);
        if (found) return true;

        if (stringUpdate.Field.Id == PQFeedFields.QuoteLayerStringUpdates) orderBook.UpdateFieldString(stringUpdate);

        return false;
    }

    IPQLevel2Quote IPQLevel2Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQLevel2Quote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is not ILevel2Quote l2Q) return this;
        orderBook.CopyFrom(l2Q.OrderBook, QuoteBehavior, copyMergeFlags);
        if (source is not IPQLevel1Quote pq1)
        {
            BidPriceTop          = l2Q.BidPriceTop;
            AskPriceTop          = l2Q.AskPriceTop;
            IsBidPriceTopChanged = l2Q.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2Q.IsAskPriceTopChanged;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pq1.IsBidPriceTopUpdated || isFullReplace)
            {
                BidPriceTop                 = pq1.BidPriceTop;
                IsBidPriceTopUpdated        = true;
                IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                IsBidPriceTopChangedUpdated = pq1.IsBidPriceTopChangedUpdated;
            }
            if (pq1.IsAskPriceTopUpdated || isFullReplace)
            {
                AskPriceTop                 = pq1.AskPriceTop;
                IsAskPriceTopUpdated        = true;
                IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                IsAskPriceTopChangedUpdated = pq1.IsAskPriceTopChangedUpdated;
            }
            if (pq1.IsBidPriceTopChangedUpdated || isFullReplace)
            {
                IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                IsBidPriceTopChangedUpdated = true;
            }
            if (pq1.IsAskPriceTopChangedUpdated || isFullReplace)
            {
                IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                IsAskPriceTopChangedUpdated = true;
            }

            if (isFullReplace && pq1 is PQLevel2Quote pq2) UpdatedFlags = pq2.UpdatedFlags;
        }
        return this;
    }

    public override PQTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        EnsureRelatedItemsAreConfigured(toSet);
        return this;
    }

    protected override IEnumerable<PQFieldUpdate> GetDeltaUpdateTopBookPriceFields
        (DateTime snapShotTime, bool updatedOnly, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        return [];
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPublishableTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            // ReSharper disable NonReadonlyMemberInGetHashCode
            hashCode = (hashCode * 397) ^ BidBook.GetHashCode();
            hashCode = (hashCode * 397) ^ AskBook.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {UpdatedFlagsToString})";
}

public class PQPublishableLevel2Quote : PQPublishableLevel1Quote, IPQPublishableLevel2Quote, ICloneable<PQPublishableLevel2Quote>
  , IDoublyLinkedListNode<PQPublishableLevel2Quote>
{
    // ReSharper disable once UnusedMember.Local
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublishableLevel2Quote));

    public PQPublishableLevel2Quote()
    {
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQPublishableLevel2Quote(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQPublishableLevel2Quote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool executable = true, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedStatus = FeedConnectivityStatusFlags.None, decimal singlePrice = 0m, ICandle? conflationTicksCandle = null)
        : this(new PQLevel2Quote(sourceTickerInfo, sourceTime, orderBook, isBidPriceTopChanged, isAskPriceTopChanged, 
                                 sourceBidTime, sourceAskTime, validFrom, validTo, executable, singlePrice),
               sourceTickerInfo, feedSyncStatus, feedStatus, conflationTicksCandle) { }

    protected PQPublishableLevel2Quote
    (IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good 
      , FeedConnectivityStatusFlags feedStatus = FeedConnectivityStatusFlags.None, ICandle? conflationTicksCandle = null)
        : base(initializedQuoteContainer, sourceTickerInfo, feedSyncStatus, feedStatus, conflationTicksCandle)
    {
        OrderBook.EnsureRelatedItemsAreConfigured(SourceTickerInfo, null);
    }


    public PQPublishableLevel2Quote(IPublishableTickInstant toClone) : this(toClone, null) { }

    protected PQPublishableLevel2Quote
        (IPublishableTickInstant toClone, IPQTickInstant? initializedQuoteContainer) : base(toClone, initializedQuoteContainer)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        EnsureRelatedItemsAreConfigured(toClone);
        SetFlagsSame(toClone);
    }

    protected override IPQLevel2Quote CreateEmptyQuoteContainerInstant() => new PQLevel2Quote();

    protected override IPQLevel2Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new PQLevel2Quote(tickInstant);

    protected override IPQLevel2Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new PQLevel2Quote(tickerInfo);

    public override PQPublishableLevel2Quote Clone() =>
        Recycler?.Borrow<PQPublishableLevel2Quote>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQPublishableLevel2Quote(this, PQQuoteContainer.Clone());

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    IPQLevel2Quote IPQLevel2Quote.Clone() => Clone();

    [JsonIgnore]
    public new PQPublishableLevel2Quote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PQPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PQPublishableLevel2Quote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PQPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQPublishableLevel2Quote? IPQPublishableLevel2Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel2Quote? IPQPublishableLevel2Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel2Quote? IPublishableLevel2Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel2Quote? IPublishableLevel2Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPublishableLevel2Quote? IDoublyLinkedListNode<IPublishableLevel2Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPublishableLevel2Quote? IDoublyLinkedListNode<IPublishableLevel2Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    IPQPublishableLevel2Quote? IDoublyLinkedListNode<IPQPublishableLevel2Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as IPQPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    IPQPublishableLevel2Quote? IDoublyLinkedListNode<IPQPublishableLevel2Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as IPQPublishableLevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level2Quote;

    ILevel2Quote IPublishableLevel2Quote.              AsNonPublishable => AsNonPublishable;
    IMutableLevel2Quote IMutablePublishableLevel2Quote.AsNonPublishable => AsNonPublishable;

    public override IPQLevel2Quote AsNonPublishable => (IPQLevel2Quote)PQQuoteContainer;

    IOrderBook ILevel2Quote.OrderBook => AsNonPublishable.OrderBook;

    IOrderBookSide ILevel2Quote.BidBook => AsNonPublishable.BidBook;

    IOrderBookSide ILevel2Quote.AskBook => AsNonPublishable.AskBook;

    IMutableOrderBook IMutableLevel2Quote.OrderBook
    {
        get => AsNonPublishable.OrderBook;
        set => AsNonPublishable.OrderBook = (IPQOrderBook)value;
    }

    IMutableOrderBook IMutablePublishableLevel2Quote.OrderBook
    {
        get => AsNonPublishable.OrderBook;
        set => AsNonPublishable.OrderBook = (IPQOrderBook)value;
    }

    public IPQOrderBook OrderBook
    {
        get => AsNonPublishable.OrderBook;
        set
        {
            AsNonPublishable.OrderBook = value;
            AsNonPublishable.OrderBook.EnsureRelatedItemsAreConfigured(SourceTickerInfo!, null);
        }
    }


    public IPQOrderBookSide BidBook
    {
        get => AsNonPublishable.OrderBook.BidSide;
        set
        {
            AsNonPublishable.OrderBook.BidSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    public IPQOrderBookSide AskBook
    {
        get => AsNonPublishable.OrderBook.AskSide;
        set
        {
            AsNonPublishable.OrderBook.AskSide = value;
            EnsureRelatedItemsAreConfigured(this);
        }
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        OrderBook.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
    }

    IMutableLevel2Quote ITrackableReset<IMutableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel2Quote IMutableLevel2Quote.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel2Quote ITrackableReset<IMutablePublishableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel2Quote IMutablePublishableLevel2Quote.ResetWithTracking() => ResetWithTracking();

    IPQLevel2Quote ITrackableReset<IPQLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IPQLevel2Quote IPQLevel2Quote.ResetWithTracking() => ResetWithTracking();

    IPQPublishableLevel2Quote ITrackableReset<IPQPublishableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IPQPublishableLevel2Quote IPQPublishableLevel2Quote.ResetWithTracking() => ResetWithTracking();

    public override PQPublishableLevel2Quote ResetWithTracking()
    {
        base.ResetWithTracking();

        return this;
    }

    IPublishableLevel2Quote ICloneable<IPublishableLevel2Quote>.Clone() => Clone();

    IPublishableLevel2Quote IPublishableLevel2Quote.Clone() => Clone();

    IMutablePublishableLevel2Quote IMutablePublishableLevel2Quote.Clone() => Clone();

    IPQPublishableLevel2Quote IPQPublishableLevel2Quote.Clone() => Clone();

    public override void EnsureRelatedItemsAreConfigured(ITickInstant? quote)
    {
        base.EnsureRelatedItemsAreConfigured(quote);
        if (quote is IPublishableLevel2Quote l2Quote)
        {
            OrderBook.EnsureRelatedItemsAreConfigured(l2Quote.SourceTickerInfo, l2Quote.OrderBook);
        }
        else if (quote is IPublishableTickInstant { SourceTickerInfo: not null } pubQuote)
        {
            EnsureOrderBookRelatedItemsAreConfigured(pubQuote.SourceTickerInfo);
        }
    }

    public override void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? item)
    {
        AsNonPublishable.EnsureRelatedItemsAreConfigured(item);
        base.EnsureRelatedItemsAreConfigured(item);
    }

    public virtual void EnsureOrderBookRelatedItemsAreConfigured(ISourceTickerInfo? sourceTickerInfo)
    {
        AsNonPublishable.EnsureRelatedItemsAreConfigured(sourceTickerInfo);
    }

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (other is not IPublishableLevel2Quote) return false;
        var baseSame      = base.AreEquivalent(other, exactTypes);

        var allAreSame = baseSame;
        return allAreSame;
    }

    // must include this event though it looks like it does the same as inherited.  It ensures that subtypes forward to the next level
    // down and not it's immediate parent
    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var level1QuoteUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings))
        {
            yield return level1QuoteUpdates;
        }
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        foreach (var pqFieldStringUpdate in base.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
        foreach (var pqFieldStringUpdate in AsNonPublishable.GetStringUpdates(snapShotTime, messageFlags)) yield return pqFieldStringUpdate;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate)
    {
        var found = base.UpdateFieldString(stringUpdate);
        if (found) return true;
        AsNonPublishable.UpdateFieldString(stringUpdate);

        return false;
    }

    IPQLevel2Quote IPQLevel2Quote.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags)
    {
        if (source is IPQMessage pqMessage)
        {
            CopyFrom(pqMessage, copyMergeFlags);
        }
        else
        {
            AsNonPublishable.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    IPQPublishableLevel2Quote IPQPublishableLevel2Quote.CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public override PQPublishableLevel2Quote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is not IPublishableLevel2Quote l2Q) return this;
        if (source is not IPQPublishableLevel1Quote pq1)
        {
            BidPriceTop          = l2Q.BidPriceTop;
            AskPriceTop          = l2Q.AskPriceTop;
            IsBidPriceTopChanged = l2Q.IsBidPriceTopChanged;
            IsAskPriceTopChanged = l2Q.IsAskPriceTopChanged;
        }
        else
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (pq1.IsBidPriceTopUpdated || isFullReplace)
            {
                BidPriceTop                 = pq1.BidPriceTop;
                IsBidPriceTopUpdated        = true;
                IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                IsBidPriceTopChangedUpdated = pq1.IsBidPriceTopChangedUpdated;
            }
            if (pq1.IsAskPriceTopUpdated || isFullReplace)
            {
                AskPriceTop                 = pq1.AskPriceTop;
                IsAskPriceTopUpdated        = true;
                IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                IsAskPriceTopChangedUpdated = pq1.IsAskPriceTopChangedUpdated;
            }
            if (pq1.IsBidPriceTopChangedUpdated || isFullReplace)
            {
                IsBidPriceTopChanged        = pq1.IsBidPriceTopChanged;
                IsBidPriceTopChangedUpdated = true;
            }
            if (pq1.IsAskPriceTopChangedUpdated || isFullReplace)
            {
                IsAskPriceTopChanged        = pq1.IsAskPriceTopChanged;
                IsAskPriceTopChangedUpdated = true;
            }

            if (isFullReplace && pq1 is PQPublishableLevel2Quote pq2) UpdatedFlags = pq2.UpdatedFlags;
        }
        return this;
    }

    public override PQPublishableTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        ((IMutablePublishableTickInstant)this).SourceTickerInfo = toSet;
        AsNonPublishable.EnsureRelatedItemsAreConfigured(this);
        return this;
    }


    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IPublishableTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            // ReSharper disable NonReadonlyMemberInGetHashCode
            hashCode = (hashCode * 397) ^ BidBook.GetHashCode();
            hashCode = (hashCode * 397) ^ AskBook.GetHashCode();
            // ReSharper restore NonReadonlyMemberInGetHashCode
            return hashCode;
        }
    }

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {PQQuoteContainer.QuoteToStringMembers}, {UpdatedFlagsToString})";
}
