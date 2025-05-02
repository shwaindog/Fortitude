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
    public OrderBook() : this(LayerType.PriceVolume, SourceTickerInfo.DefaultMaximumPublishedLayers, false) { }

    public OrderBook(LayerType layerType = LayerType.PriceVolume, int numBookLayers = 0, bool isLadder = false)
    {
        LayersOfType = layerType;
        IsLadder     = isLadder;

        AskSide = new OrderBookSide(BookSide.AskBook, layerType, isLadder);
        BidSide = new OrderBookSide(BookSide.BidBook, layerType, isLadder);
    }

    public OrderBook(IOrderBook toClone)
    {
        LayersOfType        = toClone.LayersOfType;
        IsLadder            = toClone.IsLadder;
        SourceOpenInterest  = toClone.SourceOpenInterest;
        AdapterOpenInterest = toClone.AdapterOpenInterest;

        AskSide = new OrderBookSide(toClone.AskSide);
        BidSide = new OrderBookSide(toClone.BidSide);
    }

    public LayerType  LayersOfType { get; set; }
    public LayerFlags LayerFlags   => LayersOfType.SupportedLayerFlags();

    public IOrderBookSide AskSide { get; set; }
    public IOrderBookSide BidSide { get; set; }

    public decimal? MidPrice => 0;

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
        var dailyTickCountSame      = DailyTickUpdateCount == other.DailyTickUpdateCount;
        var sourceOpenInterestSame  = Equals(SourceOpenInterest, other.SourceOpenInterest);
        var adapterOpenInterestSame = Equals(AdapterOpenInterest, other.AdapterOpenInterest);
        var askSideSame             = AskSide.AreEquivalent(other.AskSide, exactTypes);
        var bidSideSame             = BidSide.AreEquivalent(other.BidSide, exactTypes);

        var allSame = layerTypesSame && isLadderSame && dailyTickCountSame && sourceOpenInterestSame
                   && adapterOpenInterestSame && askSideSame && bidSideSame;
        return allSame;
    }

    public bool AreEquivalent(IMutableOrderBook? other, bool exactTypes = false) => AreEquivalent((IOrderBook?)other, exactTypes);

    public override OrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayersOfType         = source.LayersOfType;
        IsLadder             = source.IsLadder;
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
            hashCode.Add(DailyTickUpdateCount);
            hashCode.Add(SourceOpenInterest ?? default);
            hashCode.Add(AdapterOpenInterest);
            hashCode.Add(AskSide);
            hashCode.Add(BidSide);

            return hashCode.ToHashCode();
        }
    }

    public override string ToString() =>
        $"{nameof(OrderBook)}{{{nameof(LayersOfType)}: {LayersOfType}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(SourceOpenInterest)}: {SourceOpenInterest},  {nameof(AdapterOpenInterest)}: {AdapterOpenInterest}, " +
        $"{nameof(AskSide)}: {AskSide}, {nameof(BidSide)}: {BidSide}, {nameof(IsLadder)}: {IsLadder}, ";
}
