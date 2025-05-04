// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;

#endregion

namespace FortitudeMarkets.Pricing.Quotes;

public class Level2PriceQuote : Level1PriceQuote, IMutableLevel2Quote, ICloneable<Level2PriceQuote>, IDoublyLinkedListNode<Level2PriceQuote>
{
    private IMutableOrderBook orderBook;

    public Level2PriceQuote()
    {
        OrderBook = new OrderBook(numBookLayers: Quotes.SourceTickerInfo.DefaultMaximumPublishedLayers);
    }

    public Level2PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , decimal singlePrice = 0m, DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null
      , DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null, bool isAskPriceTopChanged = false, bool executable = false, IPricePeriodSummary? periodSummary = null
      , IOrderBook? orderBook = null) :
        base(sourceTickerInfo, sourceTime, isReplay, feedSyncStatus, singlePrice, clientReceivedTime, adapterReceivedTime,
             adapterSentTime, sourceBidTime, validFrom, validTo, 0m, isBidPriceTopChanged, sourceAskTime, 0m,
             isAskPriceTopChanged, executable, periodSummary)
    {
        if (orderBook is OrderBook mutOrderBook)
        {
            mutOrderBook.LayerSupportedFlags |= sourceTickerInfo.LayerFlags;

            OrderBook               =  mutOrderBook;
        } 
            
        else if (orderBook != null)
            OrderBook = new OrderBook(orderBook);
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
        else if( toClone.SourceTickerInfo != null)
        {
            OrderBook = new OrderBook(toClone.SourceTickerInfo);
        }
        else
        {
            OrderBook = new OrderBook(numBookLayers: Quotes.SourceTickerInfo.DefaultMaximumPublishedLayers);
        }
    }

    public override Level2PriceQuote Clone() => Recycler?.Borrow<Level2PriceQuote>().CopyFrom(this) as Level2PriceQuote ?? new Level2PriceQuote(this);

    [JsonIgnore]
    public new Level2PriceQuote? Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as Level2PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    public new Level2PriceQuote? Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as Level2PriceQuote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel2Quote? ILevel2Quote.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel2Quote? ILevel2Quote.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore]
    ILevel2Quote? IDoublyLinkedListNode<ILevel2Quote>.Previous
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Previous = value;
    }

    [JsonIgnore]
    ILevel2Quote? IDoublyLinkedListNode<ILevel2Quote>.Next
    {
        get => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next as ILevel2Quote;
        set => ((IDoublyLinkedListNode<IBidAskInstant>)this).Next = value;
    }

    [JsonIgnore] public override TickerDetailLevel TickerDetailLevel => TickerDetailLevel.Level2Quote;

    IOrderBook ILevel2Quote.OrderBook => OrderBook;

    public IMutableOrderBook OrderBook
    {
        get => orderBook;
        set
        {
            orderBook = value;
            if (SourceTickerInfo != null)
            {
                orderBook.LayerSupportedFlags = SourceTickerInfo.LayerFlags;
            }
        }
    }

    public       IMutableOrderBookSide       BidBook => OrderBook.BidSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBook => OrderBook.BidSide;

    // [JsonIgnore] public bool IsBidBookChanged => OrderBook.IsBidBookChanged;

    public       IMutableOrderBookSide       AskBook => OrderBook.AskSide;
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBook => OrderBook.AskSide;

    // [JsonIgnore] public bool IsAskBookChanged => OrderBook.IsAskBookChanged;

    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBook.Any() ? BidBook[0]?.Price ?? 0 : 0m;
        set
        {
            if (BidBook.Capacity > 0 && BidBook[0] != null)
            {
                BidBook[0]!.Price = value;
            }
            else
            {
                BidBook[0]        = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!) as IMutablePriceVolumeLayer;
                BidBook[0]!.Price = value;
            }
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBook.Any() ? AskBook[0]?.Price ?? 0 : 0m;
        set
        {
            if (AskBook.Capacity > 0 && BidBook[0] != null)
            {
                AskBook[0]!.Price = value;
            }
            else
            {
                AskBook[0]        = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!) as IMutablePriceVolumeLayer;
                AskBook[0]!.Price = value;
            }
        }
    }

    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
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

    ILevel2Quote ICloneable<ILevel2Quote>.Clone() => Clone();

    ILevel2Quote ILevel2Quote.Clone() => Clone();

    IMutableLevel2Quote IMutableLevel2Quote.Clone() => Clone();

    public override bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (!(other is ILevel2Quote otherL2)) return false;
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

    public override string ToString() =>
        $"Level2PriceQuote {{{nameof(SourceTickerInfo)}: {SourceTickerInfo}, " +
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SingleTickValue)}: " +
        $"{SingleTickValue:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(AdapterSentTime)}: " +
        $"{AdapterSentTime:O}, {nameof(SourceBidTime)}: {SourceBidTime:O}, {nameof(BidPriceTop)}: {BidPriceTop:N5}, " +
        $"{nameof(IsBidPriceTopChanged)}: {IsBidPriceTopChanged}, {nameof(SourceAskTime)}: {SourceAskTime:O}, " +
        $"{nameof(AskPriceTop)}: {AskPriceTop:N5}, {nameof(IsAskPriceTopChanged)}: {IsAskPriceTopChanged}, " +
        $"{nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(OrderBook)}:" +
        $" {OrderBook}}}";
}
