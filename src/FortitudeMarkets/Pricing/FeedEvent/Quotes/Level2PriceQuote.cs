// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public class Level2PriceQuote : Level1PriceQuote, IMutableLevel2Quote, ICloneable<Level2PriceQuote>
{
    private IMutableOrderBook orderBook;

    public Level2PriceQuote()
    {
        orderBook = new OrderBook(numBookLayers: TickerInfo.SourceTickerInfo.DefaultMaximumPublishedLayers);
    }

    public Level2PriceQuote
    (ISourceTickerInfo sourceTickerInfo, decimal singlePrice = 0m, bool isReplay = false, DateTime? sourceTime = null
      , DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool isAskPriceTopChanged = false, bool executable = false, IOrderBook? orderBookParam = null) :
        base(singlePrice, isReplay, sourceTime, sourceBidTime, validFrom, validTo, 0m, isBidPriceTopChanged, sourceAskTime, 0m,
             isAskPriceTopChanged, executable)
    {
        if (orderBookParam is OrderBook mutOrderBook)
        {
            mutOrderBook.LayerSupportedFlags |= sourceTickerInfo.LayerFlags;

            orderBook = mutOrderBook;
        }
        else if (orderBookParam != null)
            orderBook = new OrderBook(orderBookParam);
        else
        {
            orderBook = new OrderBook(sourceTickerInfo);
        }
    }

    public Level2PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel2Quote level2ToClone)
        {
            orderBook = new OrderBook(level2ToClone.OrderBook);
        }
        else
        {
            orderBook = new OrderBook(numBookLayers: TickerInfo.SourceTickerInfo.DefaultMaximumPublishedLayers);
        }
    }

    public override Level2PriceQuote Clone() => Recycler?.Borrow<Level2PriceQuote>().CopyFrom(this) ?? new Level2PriceQuote(this);

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    IOrderBook ILevel2Quote.OrderBook => OrderBook;

    public IMutableOrderBook OrderBook
    {
        get => orderBook;
        set { orderBook = value; }
    }

    public       IMutableOrderBookSide       BidBook => OrderBook.BidSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBook => OrderBook.BidSide;

    public       IMutableOrderBookSide       AskBook => OrderBook.AskSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBook => OrderBook.AskSide;


    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBook.Any() ? BidBook[0]?.Price ?? 0 : 0m;
        set { BidBook[0]!.Price = value; }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBook.Any() ? AskBook[0]?.Price ?? 0 : 0m;
        set { AskBook[0]!.Price = value; }
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

        var orderBookSame = OrderBook?.AreEquivalent(otherL2.OrderBook, exactTypes) ?? otherL2?.BidBook == null;


        var allAreSame = baseIsSame && orderBookSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel2Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (BidBook != null ? BidBook.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (AskBook != null ? AskBook.GetHashCode() : 0);
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
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , decimal singlePrice = 0m, DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null
      , DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool isAskPriceTopChanged = false, bool executable = false, IPricePeriodSummary? periodSummary = null
      , IOrderBook? orderBook = null) :
        this(new Level2PriceQuote(sourceTickerInfo, singlePrice, isReplay, sourceTime, sourceBidTime, isBidPriceTopChanged, sourceAskTime, validFrom, 
                                  validTo, isAskPriceTopChanged, executable, orderBook), sourceTickerInfo,  
             feedSyncStatus, clientReceivedTime, adapterReceivedTime, adapterSentTime,  periodSummary)
    {
    }

    protected PublishableLevel2PriceQuote
    (IMutableTickInstant? initialisedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus syncStatus = FeedSyncStatus.Good, DateTime? clientReceivedTime = null,
        DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null, IPricePeriodSummary? periodSummary = null)
        : base(initialisedQuoteContainer, sourceTickerInfo, syncStatus, clientReceivedTime,
               adapterReceivedTime, adapterSentTime, periodSummary)
    {
    }

    public PublishableLevel2PriceQuote(IPublishableTickInstant toClone) : this(toClone, null)
    {
    }

    protected PublishableLevel2PriceQuote(IPublishableTickInstant toClone, IMutableTickInstant? initializedQuoteContainer) : base(toClone, initializedQuoteContainer)
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
        get => AsNonPublishable.OrderBook.BidSide.Any() ? AsNonPublishable.OrderBook.BidSide[0]?.Price ?? 0 : 0m;
        set
        {
            if (AsNonPublishable.BidBook.Capacity > 0 && AsNonPublishable.OrderBook.BidSide[0] != null)
            {
                AsNonPublishable.OrderBook.BidSide[0]!.Price = value;
            }
            else
            {
                AsNonPublishable.OrderBook.BidSide[0]        = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!) as IMutablePriceVolumeLayer;
                AsNonPublishable.OrderBook.BidSide[0]!.Price = value;
            }
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AsNonPublishable.OrderBook.AskSide.Any() ? AsNonPublishable.OrderBook.AskSide[0]?.Price ?? 0 : 0m;
        set
        {
            if (AsNonPublishable.OrderBook.AskSide.Capacity > 0 && AsNonPublishable.OrderBook.AskSide[0] != null)
            {
                AsNonPublishable.OrderBook.AskSide[0]!.Price = value;
            }
            else
            {
                AsNonPublishable.OrderBook.AskSide[0]        = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!) as IMutablePriceVolumeLayer;
                AsNonPublishable.OrderBook.AskSide[0]!.Price = value;
            }
        }
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
            hashCode = (hashCode * 397) ^ (BidBook != null ? BidBook.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ (AskBook != null ? AskBook.GetHashCode() : 0);
            return hashCode;
        }
    }

    public override string ToString() => $"{nameof(PublishableLevel2PriceQuote)}{{{QuoteToStringMembers}, {AsNonPublishable.QuoteToStringMembers}}}";
}
