// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class OrderBook : ReusableObject<IOrderBook>, IMutableOrderBook
{
    public OrderBook() : this(LayerType.PriceVolume) { }

    public OrderBook
    (LayerType layerType = LayerType.PriceVolume,
        int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false)
    {
        LayersOfType    = layerType;
        IsLadder        = isLadder;
        MaxPublishDepth = (ushort)numBookLayers;

        AskSide = new OrderBookSide(BookSide.AskBook, layerType, MaxPublishDepth, isLadder);
        BidSide = new OrderBookSide(BookSide.BidBook, layerType, MaxPublishDepth, isLadder);
    }

    public OrderBook(IOrderBook toClone)
    {
        LayersOfType        = toClone.LayersOfType;
        IsLadder            = toClone.IsLadder;
        SourceOpenInterest  = toClone.SourceOpenInterest;
        AdapterOpenInterest = toClone.AdapterOpenInterest;
        MaxPublishDepth     = toClone.MaxPublishDepth;

        AskSide = new OrderBookSide(toClone.AskSide);
        BidSide = new OrderBookSide(toClone.BidSide);
    }

    public OrderBook(IOrderBookSide bidSide, IOrderBookSide askBookSide, bool isLadder = false)
    {
        if (bidSide is OrderBookSide orderBookBid)
        {
            BidSide = orderBookBid;
        }
        else
        {
            BidSide = new OrderBookSide(bidSide);
        }
        if (askBookSide is OrderBookSide orderBookAsk)
        {
            AskSide = orderBookAsk;
        }
        else
        {
            AskSide = new OrderBookSide(askBookSide);
        }

        LayersOfType        = BidSide.LayersOfType;
        IsLadder            = isLadder;
        MaxPublishDepth     = Math.Max(BidSide.MaxPublishDepth, AskSide.MaxPublishDepth);
    }

    public OrderBook(ISourceTickerInfo sourceTickerInfo)
    {
        LayersOfType    = sourceTickerInfo.LayerFlags.MostCompactLayerType();
        IsLadder        = sourceTickerInfo.LayerFlags.HasLadder();
        MaxPublishDepth = sourceTickerInfo.MaximumPublishedLayers;

        AskSide = new OrderBookSide(BookSide.AskBook, sourceTickerInfo);
        BidSide = new OrderBookSide(BookSide.BidBook, sourceTickerInfo);
    }

    public LayerType  LayersOfType { get; set; }
    public LayerFlags LayerFlags   => LayersOfType.SupportedLayerFlags();

    IOrderBookSide IOrderBook.AskSide => AskSide;
    IOrderBookSide IOrderBook.BidSide => BidSide;

    public IMutableOrderBookSide AskSide { get; set; }
    public IMutableOrderBookSide BidSide { get; set; }

    public bool IsBidBookChanged { get; set; }
    public bool IsAskBookChanged { get; set; }

    public ushort MaxPublishDepth { get; private set; }

    public decimal? MidPrice => (BidSide[0]?.Price ?? 0 + AskSide[0]?.Price ?? 0) / 2;

    public OpenInterest? SourceOpenInterest  { get; set; }
    public OpenInterest  AdapterOpenInterest { get; set; }
    public OpenInterest PublishedOpenInterest
    {
        get
        {
            var bidOpenInterest = BidSide.PublishedOpenInterest;
            var askOpenInterest = AskSide.PublishedOpenInterest;

            var totalVolume = bidOpenInterest.Volume + askOpenInterest.Volume;
            var totalPriceVolume = totalVolume != 0
                ? (bidOpenInterest.Volume * bidOpenInterest.Vwap + askOpenInterest.Volume * askOpenInterest.Vwap) / totalVolume
                : 0m;

            return new OpenInterest(MarketDataSource.Published, totalVolume, totalPriceVolume);
        }
    }

    public uint DailyTickUpdateCount { get; set; }
    public bool IsLadder             { get; set; }

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    public virtual bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != typeof(OrderBook)) return false;

        var layerTypesSame          = LayersOfType == other.LayersOfType;
        var isLadderSame            = IsLadder == other.IsLadder;
        var maxDepthSame            = MaxPublishDepth == other.MaxPublishDepth;
        var dailyTickCountSame      = DailyTickUpdateCount == other.DailyTickUpdateCount;
        var sourceOpenInterestSame  = Equals(SourceOpenInterest, other.SourceOpenInterest);
        var adapterOpenInterestSame = Equals(AdapterOpenInterest, other.AdapterOpenInterest);
        var askSideSame             = AskSide.AreEquivalent(other.AskSide, exactTypes);
        var bidSideSame             = BidSide.AreEquivalent(other.BidSide, exactTypes);

        var allSame = layerTypesSame && isLadderSame && maxDepthSame && dailyTickCountSame && sourceOpenInterestSame
                   && adapterOpenInterestSame && askSideSame && bidSideSame;
        return allSame;
    }

    public bool AreEquivalent(IMutableOrderBook? other, bool exactTypes = false) => AreEquivalent((IOrderBook?)other, exactTypes);

    public override OrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayersOfType         = source.LayersOfType;
        IsLadder             = source.IsLadder;
        MaxPublishDepth      = MaxPublishDepth;
        IsBidBookChanged     = source.IsBidBookChanged;
        IsAskBookChanged     = source.IsAskBookChanged;
        DailyTickUpdateCount = source.DailyTickUpdateCount;
        SourceOpenInterest   = source.SourceOpenInterest;
        AdapterOpenInterest  = source.AdapterOpenInterest;
        BidSide.CopyFrom(source.BidSide, copyMergeFlags);
        AskSide.CopyFrom(source.AskSide, copyMergeFlags);
        return this;
    }

    public override OrderBook Clone() => Recycler?.Borrow<OrderBook>().CopyFrom(this) ?? new OrderBook(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBook?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = new HashCode();
            hashCode.Add(LayersOfType);
            hashCode.Add(IsLadder);
            hashCode.Add(MaxPublishDepth);
            hashCode.Add(IsBidBookChanged);
            hashCode.Add(IsAskBookChanged);
            hashCode.Add(DailyTickUpdateCount);
            hashCode.Add(SourceOpenInterest ?? default);
            hashCode.Add(AdapterOpenInterest);
            hashCode.Add(AskSide);
            hashCode.Add(BidSide);

            return hashCode.ToHashCode();
        }
    }

    protected string OrderBookToStringMembers =>
        $"{nameof(LayersOfType)}: {LayersOfType}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(SourceOpenInterest)}: {SourceOpenInterest},  {nameof(AdapterOpenInterest)}: {AdapterOpenInterest}, " +
        $"{nameof(IsAskBookChanged)}: {IsAskBookChanged},  {nameof(IsBidBookChanged)}: {IsBidBookChanged}, " +
        $"{nameof(AskSide)}: {AskSide}, {nameof(BidSide)}: {BidSide}, {nameof(IsLadder)}: {IsLadder}";

    public override string ToString() => $"{nameof(OrderBook)}{{{OrderBookToStringMembers}}}";
}
