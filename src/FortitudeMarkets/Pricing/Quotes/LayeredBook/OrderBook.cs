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
    private LayerFlags layerFlags;
    public OrderBook() : this(LayerType.PriceVolume) { }

    public OrderBook
    (LayerType layerType = LayerType.PriceVolume,
        int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false)
    {
        layerFlags      = layerType.SupportedLayerFlags();
        layerFlags      |= isLadder ? LayerFlags.Ladder : LayerFlags.None;
        MaxPublishDepth = (ushort)numBookLayers;

        AskSide = new OrderBookSide(BookSide.AskBook, layerType, MaxPublishDepth, isLadder);
        BidSide = new OrderBookSide(BookSide.BidBook, layerType, MaxPublishDepth, isLadder);
    }

    public OrderBook(IOrderBook toClone)
    {
        layerFlags          =  toClone.LayerSupportedFlags;
        layerFlags          |= LayersSupportedType.SupportedLayerFlags();
        SourceOpenInterest  =  toClone.SourceOpenInterest;
        AdapterOpenInterest =  toClone.AdapterOpenInterest;
        MaxPublishDepth     =  toClone.MaxPublishDepth;

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

        layerFlags =  bidSide.LayerSupportedFlags | askBookSide.LayerSupportedFlags;
        layerFlags |= LayersSupportedType.SupportedLayerFlags();
        layerFlags |=  isLadder ? LayerFlags.Ladder : LayerFlags.None;

        MaxPublishDepth = Math.Max(BidSide.MaxPublishDepth, AskSide.MaxPublishDepth);
    }

    public OrderBook(ISourceTickerInfo sourceTickerInfo)
    {
        layerFlags =  sourceTickerInfo.LayerFlags;
        layerFlags |= LayersSupportedType.SupportedLayerFlags();

        MaxPublishDepth = sourceTickerInfo.MaximumPublishedLayers;

        AskSide = new OrderBookSide(BookSide.AskBook, sourceTickerInfo);
        BidSide = new OrderBookSide(BookSide.BidBook, sourceTickerInfo);
    }

    public LayerType LayersSupportedType
    {
        get => LayerSupportedFlags.MostCompactLayerType();
        set => LayerSupportedFlags = value.SupportedLayerFlags();
    }
    public LayerFlags LayerSupportedFlags
    {
        get => layerFlags;
        set
        {
            layerFlags = layerFlags.Unset(LayerFlags.Ladder) | value;

            AskSide.LayerSupportedFlags = layerFlags;
            BidSide.LayerSupportedFlags = layerFlags;
        }
    }

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

    public bool IsLadder
    {
        get => LayerSupportedFlags.HasLadder();
        set => LayerSupportedFlags = value ? LayerFlags.Ladder : LayerFlags.None;
    }

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    public virtual bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != typeof(OrderBook)) return false;

        var layerFlagsSame          = LayerSupportedFlags == other.LayerSupportedFlags;
        var maxDepthSame            = MaxPublishDepth == other.MaxPublishDepth;
        var dailyTickCountSame      = DailyTickUpdateCount == other.DailyTickUpdateCount;
        var sourceOpenInterestSame  = Equals(SourceOpenInterest, other.SourceOpenInterest);
        var adapterOpenInterestSame = Equals(AdapterOpenInterest, other.AdapterOpenInterest);
        var askSideSame             = AskSide.AreEquivalent(other.AskSide, exactTypes);
        var bidSideSame             = BidSide.AreEquivalent(other.BidSide, exactTypes);

        var allSame = layerFlagsSame && maxDepthSame && dailyTickCountSame && sourceOpenInterestSame
                   && adapterOpenInterestSame && askSideSame && bidSideSame;
        if (!allSame)
        {
            Console.Out.WriteLine("");
        }
        return allSame;
    }

    public bool AreEquivalent(IMutableOrderBook? other, bool exactTypes = false) => AreEquivalent((IOrderBook?)other, exactTypes);

    public override OrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayerSupportedFlags           = source.LayerSupportedFlags;
        MaxPublishDepth      = source.MaxPublishDepth;
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
            hashCode.Add(LayersSupportedType);
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
        $"{nameof(LayersSupportedType)}: {LayersSupportedType}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(SourceOpenInterest)}: {SourceOpenInterest},  {nameof(AdapterOpenInterest)}: {AdapterOpenInterest}, " +
        $"{nameof(IsAskBookChanged)}: {IsAskBookChanged},  {nameof(IsBidBookChanged)}: {IsBidBookChanged}, " +
        $"{nameof(AskSide)}: {AskSide}, {nameof(BidSide)}: {BidSide}, {nameof(IsLadder)}: {IsLadder}";

    public override string ToString() => $"{nameof(OrderBook)}{{{OrderBookToStringMembers}}}";
}
