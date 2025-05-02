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
    public Level2PriceQuote()
    {
        BidBookSide = new OrderBookSide(BookSide.BidBook, 20);
        AskBookSide = new OrderBookSide(BookSide.AskBook, 20);
    }

    public Level2PriceQuote
    (ISourceTickerInfo sourceTickerInfo, DateTime? sourceTime = null, bool isReplay = false, FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , decimal singlePrice = 0m, DateTime? clientReceivedTime = null, DateTime? adapterReceivedTime = null, DateTime? adapterSentTime = null
      , DateTime? sourceBidTime = null, bool isBidPriceTopChanged = false, DateTime? sourceAskTime = null, DateTime? validFrom = null
      , DateTime? validTo = null
      , bool isAskPriceTopChanged = false, bool executable = false, IPricePeriodSummary? periodSummary = null, IOrderBookSide? bidBook = null
      , bool isBidBookChanged = false, IOrderBookSide? askBook = null, bool isAskBookChanged = false)
        : base(sourceTickerInfo, sourceTime, isReplay, feedSyncStatus, singlePrice, clientReceivedTime, adapterReceivedTime,
               adapterSentTime, sourceBidTime, validFrom, validTo, 0m, isBidPriceTopChanged, sourceAskTime, 0m,
               isAskPriceTopChanged, executable, periodSummary)
    {
        if (bidBook is OrderBookSide mutBidOrderBook)
            BidBookSide = mutBidOrderBook;
        else
            BidBookSide = bidBook != null ? new OrderBookSide(bidBook) : new OrderBookSide(BookSide.BidBook, SourceTickerInfo!);
        IsBidBookChanged = isBidBookChanged;
        if (askBook is OrderBookSide mutAskOrderBook)
            AskBookSide = mutAskOrderBook;
        else
            AskBookSide = askBook != null ? new OrderBookSide(askBook) : new OrderBookSide(BookSide.AskBook, SourceTickerInfo!);
        IsAskBookChanged = isAskBookChanged;
    }

    public Level2PriceQuote(ITickInstant toClone) : base(toClone)
    {
        if (toClone is ILevel2Quote level2ToClone)
        {
            if (level2ToClone.BidBookSide is OrderBookSide bidOrdBook)
                BidBookSide = bidOrdBook.Clone();
            else
                BidBookSide = new OrderBookSide(level2ToClone.BidBookSide);

            IsBidBookChanged = level2ToClone.IsBidBookChanged;

            if (level2ToClone.AskBookSide is OrderBookSide askOrdBook)
                AskBookSide = askOrdBook.Clone();
            else
                AskBookSide = new OrderBookSide(level2ToClone.AskBookSide);

            IsAskBookChanged = level2ToClone.IsAskBookChanged;
        }
        else
        {
            BidBookSide = new OrderBookSide(BookSide.BidBook, 20);
            AskBookSide = new OrderBookSide(BookSide.AskBook, 20);
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

    public       IMutableOrderBookSide       BidBookSide { get; set; }
    [JsonIgnore] IOrderBookSide ILevel2Quote.BidBookSide => BidBookSide;

    [JsonIgnore] public bool IsBidBookChanged { get; set; }

    public       IMutableOrderBookSide       AskBookSide { get; set; }
    [JsonIgnore] IOrderBookSide ILevel2Quote.AskBookSide => AskBookSide;

    [JsonIgnore] public bool IsAskBookChanged { get; set; }

    [JsonIgnore]
    public override decimal BidPriceTop
    {
        get => BidBookSide.Any() ? BidBookSide[0]?.Price ?? 0 : 0m;
        set
        {
            if (BidBookSide.Capacity > 0 && BidBookSide[0] != null)
            {
                BidBookSide[0]!.Price = value;
            }
            else
            {
                BidBookSide[0]        = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!) as IMutablePriceVolumeLayer;
                BidBookSide[0]!.Price = value;
            }
        }
    }

    [JsonIgnore]
    public override decimal AskPriceTop
    {
        get => AskBookSide.Any() ? AskBookSide[0]?.Price ?? 0 : 0m;
        set
        {
            if (AskBookSide.Capacity > 0 && BidBookSide[0] != null)
            {
                AskBookSide[0]!.Price = value;
            }
            else
            {
                AskBookSide[0]        = OrderBookSide.LayerSelector.FindForLayerFlags(SourceTickerInfo!) as IMutablePriceVolumeLayer;
                AskBookSide[0]!.Price = value;
            }
        }
    }

    public override ITickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        if (source is ILevel2Quote level2Quote)
        {
            BidBookSide.CopyFrom(level2Quote.BidBookSide, copyMergeFlags);
            AskBookSide.CopyFrom(level2Quote.AskBookSide, copyMergeFlags);
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

        var bidBooksSame = BidBookSide?.AreEquivalent(otherL2.BidBookSide, exactTypes) ?? otherL2?.BidBookSide == null;
        var askBooksSame = AskBookSide?.AreEquivalent(otherL2!.AskBookSide, exactTypes) ?? otherL2?.AskBookSide == null;

        var bidBookChangedSame = true;
        var askBookChangedSame = true;
        if (exactTypes)
        {
            bidBookChangedSame = IsBidBookChanged == otherL2!.IsBidBookChanged;
            askBookChangedSame = IsAskBookChanged == otherL2.IsAskBookChanged;
        }

        var allAreSame = baseIsSame && bidBooksSame && bidBookChangedSame && askBooksSame && askBookChangedSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel2Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (BidBookSide != null ? BidBookSide.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsBidBookChanged.GetHashCode();
            hashCode = (hashCode * 397) ^ (AskBookSide != null ? AskBookSide.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ IsAskBookChanged.GetHashCode();
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
        $"{nameof(Executable)}: {Executable}, {nameof(SummaryPeriod)}: {SummaryPeriod}, {nameof(BidBookSide)}:" +
        $" {BidBookSide}, {nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(AskBookSide)}: {AskBookSide}, " +
        $"{nameof(IsAskBookChanged)}: {IsAskBookChanged}}}";
}
