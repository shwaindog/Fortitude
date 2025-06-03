// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using System.Linq;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Candles;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes;

public class Level2PriceQuote : Level1PriceQuote, IMutableLevel2Quote, ICloneable<Level2PriceQuote>
{
    public Level2PriceQuote()
    {
        OrderBook = new OrderBook(numBookLayers: SourceTickerInfo.DefaultMaximumPublishedLayers);
    }

    public Level2PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBookParam = null 
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool executable = false, decimal singlePrice = 0m) :
        base( sourceTime, 0m, 0m, isBidPriceTopChanged, isAskPriceTopChanged, sourceBidTime, 
             sourceAskTime, validFrom, validTo, executable, singlePrice)
    {
        if (orderBookParam is OrderBook mutOrderBook)
        {
            mutOrderBook.LayerSupportedFlags |= sourceTickerInfo.LayerFlags;

            OrderBook = mutOrderBook;
        }
        else if (orderBookParam != null)
            OrderBook = new OrderBook(orderBookParam);
        else
        {
            OrderBook = new OrderBook(sourceTickerInfo);
        }
    }

    public Level2PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel2Quote level2ToClone)
        {
            OrderBook = new OrderBook(level2ToClone.OrderBook);
        }
        else
        {
            OrderBook = new OrderBook(numBookLayers: SourceTickerInfo.DefaultMaximumPublishedLayers);
        }
    }

    public override Level2PriceQuote Clone() => Recycler?.Borrow<Level2PriceQuote>().CopyFrom(this) ?? new Level2PriceQuote(this);

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    IOrderBook ILevel2Quote.OrderBook => OrderBook;

    public IMutableOrderBook OrderBook { get; set; }

    public       IMutableOrderBookSide       BidBook => OrderBook.BidSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBook => OrderBook.BidSide;

    public       IMutableOrderBookSide       AskBook => OrderBook.AskSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBook => OrderBook.AskSide;


    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => ((IEnumerable<IMutablePriceVolumeLayer>)BidBook).Any() ? BidBook[0].Price : 0;
        set => BidBook[0].Price = value;
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => ((IEnumerable<IMutablePriceVolumeLayer>)AskBook).Any() ? AskBook[0].Price : 0m;
        set => AskBook[0].Price = value;
    }

    IMutableLevel2Quote ITrackableReset<IMutableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel2Quote IMutableLevel2Quote.                 ResetWithTracking() => ResetWithTracking();

    public override Level2PriceQuote ResetWithTracking()
    {
        OrderBook.ResetWithTracking();

        base.ResetWithTracking();
        return this;
    }

    public override Level2PriceQuote CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel2Quote level2Quote)
        {
            OrderBook.CopyFrom(level2Quote.OrderBook, copyMergeFlags);
        }
        if (source is ILevel1Quote level1Quote)
        {
            IsBidPriceTopChanged = level1Quote.IsBidPriceTopChanged;
            IsBidPriceTopChanged = level1Quote.IsBidPriceTopChanged;
        }

        return this;
    }

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other is not ILevel2Quote otherL2) return false;
        var baseIsSame = base.AreEquivalent(otherL2, exactTypes);

        var orderBookSame = OrderBook.AreEquivalent(otherL2.OrderBook, exactTypes);


        var allAreSame = baseIsSame && orderBookSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel2Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ BidBook.GetHashCode();
            hashCode = (hashCode * 397) ^ AskBook.GetHashCode();
            return hashCode;
        }
    }

    public override string QuoteToStringMembers => $"{base.QuoteToStringMembers}, {nameof(OrderBook)}: {OrderBook}";

    public override string ToString() => $"{nameof(Level2PriceQuote)}{{{QuoteToStringMembers}}}";
}

public class PublishableLevel2PriceQuote : PublishableLevel1PriceQuote, IMutablePublishableLevel2Quote, ICloneable<PublishableLevel2PriceQuote>
  , IDoublyLinkedListNode<PublishableLevel2PriceQuote>
{
    public PublishableLevel2PriceQuote()
    {
        OrderBook = new OrderBook(numBookLayers: TickerInfo.SourceTickerInfo.DefaultMaximumPublishedLayers);
    }

    public PublishableLevel2PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, IOrderBook? orderBook = null
      , bool isBidPriceTopChanged = false, bool isAskPriceTopChanged = false, DateTime? sourceBidTime = null, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool executable = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None, decimal singlePrice = 0m, ICandle? conflationTicksCandle = null) :
        this(new Level2PriceQuote(sourceTickerInfo,  sourceTime, orderBook, isBidPriceTopChanged, isAskPriceTopChanged, 
                                  sourceBidTime, sourceAskTime, validFrom, validTo, executable,  singlePrice), sourceTickerInfo,
             feedSyncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    protected PublishableLevel2PriceQuote
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus syncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None
      , ICandle? conflationTicksCandle = null)
        : base(initialisedQuoteContainer, sourceTickerInfo, syncStatus, feedConnectivityStatus, conflationTicksCandle) { }

    public PublishableLevel2PriceQuote(IPublishableTickInstant toClone) : this(toClone, null) { }

    protected PublishableLevel2PriceQuote
        (IPublishableTickInstant toClone, IMutableTickInstant? initializedQuoteContainer) : base(toClone, initializedQuoteContainer)
    {
        if (toClone is not IPublishableLevel2Quote && toClone.SourceTickerInfo != null)
        {
            OrderBook = new OrderBook(toClone.SourceTickerInfo);
        }
    }

    protected override IMutableLevel2Quote CreateEmptyQuoteContainerInstant() => new Level2PriceQuote();

    protected override IMutableLevel2Quote CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new Level2PriceQuote(tickInstant);

    protected override IMutableLevel2Quote CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new Level2PriceQuote(tickerInfo);

    public override PublishableLevel2PriceQuote Clone() =>
        Recycler?.Borrow<PublishableLevel2PriceQuote>().CopyFrom(this) ?? new PublishableLevel2PriceQuote(this, QuoteContainer.Clone());

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    [JsonIgnore]
    public new PublishableLevel2PriceQuote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as PublishableLevel2PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new PublishableLevel2PriceQuote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as PublishableLevel2PriceQuote;
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

    [JsonIgnore] public override TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.Level2Quote;


    IOrderBook ILevel2Quote.OrderBook => AsNonPublishable.OrderBook;

    public IMutableOrderBook OrderBook
    {
        get => AsNonPublishable.OrderBook;
        set
        {
            AsNonPublishable.OrderBook = value;
            if (SourceTickerInfo != null)
            {
                AsNonPublishable.OrderBook.LayerSupportedFlags = SourceTickerInfo.LayerFlags;
            }
        }
    }

    public       IMutableOrderBookSide       BidBook => AsNonPublishable.OrderBook.BidSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBook => AsNonPublishable.OrderBook.BidSide;

    // [JsonIgnore] public bool IsBidBookChanged => OrderBook.IsBidBookChanged;

    public       IMutableOrderBookSide       AskBook => AsNonPublishable.OrderBook.AskSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBook => AsNonPublishable.OrderBook.AskSide;

    // [JsonIgnore] public bool IsAskBookChanged => OrderBook.IsAskBookChanged;

    ILevel2Quote IPublishableLevel2Quote.AsNonPublishable => AsNonPublishable;

    public override IMutableLevel2Quote AsNonPublishable => (IMutableLevel2Quote)QuoteContainer;


    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => ((IEnumerable<IPriceVolumeLayer>)AsNonPublishable.OrderBook.BidSide).Any() ? AsNonPublishable.OrderBook.BidSide[0].Price : 0m;
        set
        {
            if (AsNonPublishable.BidBook.Capacity > 0)
            {
                AsNonPublishable.OrderBook.BidSide[0].Price = value;
            }
            else
            {
                AsNonPublishable.OrderBook.BidSide[0] = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!);
                AsNonPublishable.OrderBook.BidSide[0].Price = value;
            }
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => ((IEnumerable<IPriceVolumeLayer>)AsNonPublishable.OrderBook.AskSide).Any() ? AsNonPublishable.OrderBook.AskSide[0].Price : 0m;
        set
        {
            if (AsNonPublishable.OrderBook.AskSide.Capacity > 0)
            {
                AsNonPublishable.OrderBook.AskSide[0].Price = value;
            }
            else
            {
                AsNonPublishable.OrderBook.AskSide[0] = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!);
                AsNonPublishable.OrderBook.AskSide[0].Price = value;
            }
        }
    }

    IMutableLevel2Quote ITrackableReset<IMutableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IMutableLevel2Quote IMutableLevel2Quote.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel2Quote ITrackableReset<IMutablePublishableLevel2Quote>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableLevel2Quote IMutablePublishableLevel2Quote.ResetWithTracking() => ResetWithTracking();

    public override PublishableLevel2PriceQuote ResetWithTracking()
    {
        base.ResetWithTracking();
        return this;
    }

    public override PublishableLevel2PriceQuote CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }

    IPublishableLevel2Quote ICloneable<IPublishableLevel2Quote>.Clone() => Clone();

    IPublishableLevel2Quote IPublishableLevel2Quote.Clone() => Clone();

    IMutablePublishableLevel2Quote IMutablePublishableLevel2Quote.Clone() => Clone();

    bool ILevel2Quote.AreEquivalent(ITickInstant? other, bool exactTypes) => AreEquivalent(other as IPublishableTickInstant, exactTypes);

    bool IMutableLevel2Quote.AreEquivalent(ITickInstant? other, bool exactTypes) => AreEquivalent(other as IPublishableTickInstant, exactTypes);

    public override bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (!(other is IPublishableLevel2Quote otherL2)) return false;
        var baseIsSame = base.AreEquivalent(otherL2, exactTypes);
        var allAreSame = baseIsSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IPublishableLevel2Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ BidBook.GetHashCode();
            hashCode = (hashCode * 397) ^ AskBook.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() => $"{nameof(PublishableLevel2PriceQuote)}{{{QuoteToStringMembers}, {AsNonPublishable.QuoteToStringMembers}, " +
                                         $"{JustFeedSyncConnectivityStatusToStringMembers}}}";
}
