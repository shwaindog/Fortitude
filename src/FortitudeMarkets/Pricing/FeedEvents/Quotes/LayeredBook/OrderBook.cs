﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public class OrderBook : ReusableQuoteElement<IOrderBook>, IMutableOrderBook
{
    private LayerFlags               layerFlags;
    private IMutableMarketAggregate? openInterest;

    private QuoteInstantBehaviorFlags cacheBehaviorFlags;
    private IMutableOrderBookSide     askSide = null!;
    private IMutableOrderBookSide     bidSide = null!;
    public OrderBook() : this(LayerType.PriceVolume) { }

    public OrderBook(LayerType layerType = LayerType.PriceVolume, QuoteInstantBehaviorFlags quoteBehavior = QuoteInstantBehaviorFlags.None,
        int numBookLayers = SourceTickerInfo.DefaultMaximumPublishedLayers, bool isLadder = false)
    {
        cacheBehaviorFlags =  quoteBehavior;
        layerFlags         =  layerType.SupportedLayerFlags();
        layerFlags         |= isLadder ? LayerFlags.Ladder : LayerFlags.None;
        MaxAllowedSize     =  (ushort)numBookLayers;

        AskSide = new OrderBookSide(BookSide.AskBook, layerType, cacheBehaviorFlags, MaxAllowedSize, isLadder);
        BidSide = new OrderBookSide(BookSide.BidBook, layerType, cacheBehaviorFlags, MaxAllowedSize, isLadder);
    }

    public OrderBook(IOrderBook toClone)
    {
        cacheBehaviorFlags   =  toClone.QuoteBehavior;
        layerFlags           =  toClone.LayerSupportedFlags;
        layerFlags           |= LayersSupportedType.SupportedLayerFlags();
        MaxAllowedSize       =  toClone.MaxAllowedSize;
        DailyTickUpdateCount =  toClone.DailyTickUpdateCount;
        if (toClone.HasNonEmptyOpenInterest)
        {
            openInterest = new MarketAggregate(toClone.OpenInterest);
        }

        AskSide = new OrderBookSide(toClone.AskSide);
        BidSide = new OrderBookSide(toClone.BidSide);
    }

    public OrderBook(IOrderBookSide bidSide, IOrderBookSide askBookSide, uint dailyTickCount = 0, bool isLadder = false)
    {
        DailyTickUpdateCount = dailyTickCount;
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
        layerFlags |= isLadder ? LayerFlags.Ladder : LayerFlags.None;

        MaxAllowedSize = Math.Max(((IOrderBookSide)BidSide).MaxAllowedSize, ((IOrderBookSide)AskSide).MaxAllowedSize);
    }

    public OrderBook(ISourceTickerInfo sourceTickerInfo)
    {
        layerFlags =  sourceTickerInfo.LayerFlags;
        layerFlags |= LayersSupportedType.SupportedLayerFlags();

        MaxAllowedSize = sourceTickerInfo.MaximumPublishedLayers;

        AskSide = new OrderBookSide(BookSide.AskBook, sourceTickerInfo);
        BidSide = new OrderBookSide(BookSide.BidBook, sourceTickerInfo);
    }

    public QuoteInstantBehaviorFlags QuoteBehavior => cacheBehaviorFlags = Parent?.QuoteBehavior ?? cacheBehaviorFlags;

    public IParentQuoteElement? Parent { get; set; }

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

    public IMutableOrderBookSide AskSide
    {
        get => askSide;
        set
        {
            askSide = value;

            askSide.Parent = this;
        }
    }
    public IMutableOrderBookSide BidSide
    {
        get => bidSide;
        set
        {
            bidSide = value;

            bidSide.Parent = this;
        }
    }

    public bool IsBidBookChanged { get; set; }
    public bool IsAskBookChanged { get; set; }

    public ushort MaxAllowedSize { get; private set; }

    public decimal MidPrice => (BidSide[0].Price + AskSide[0].Price) / 2;

    public bool HasNonEmptyOpenInterest
    {
        get => openInterest is { IsEmpty: false };
        set
        {
            if (value) return;
            if (openInterest != null)
            {
                openInterest.IsEmpty = true;
            }
        }
    }

    IMarketAggregate IOrderBook.OpenInterest => OpenInterest!;


    public IMutableMarketAggregate? OpenInterest
    {
        get
        {
            if (HasNonEmptyOpenInterest && openInterest is not { DataSource: (MarketDataSource.Published or MarketDataSource.None) })
                return openInterest;

            var bidOpenInterest = BidSide.OpenInterestSide;
            var askOpenInterest = AskSide.OpenInterestSide;

            var totalVolume = (bidOpenInterest?.Volume + askOpenInterest?.Volume) ?? 0;
            var totalPriceVolume = totalVolume != 0
                ? ((bidOpenInterest?.Volume ?? 0) * (bidOpenInterest?.Vwap ?? 0)
                 + (askOpenInterest?.Volume ?? 0) * (askOpenInterest?.Vwap ?? 0)) / totalVolume
                : 0m;
            openInterest            ??= new MarketAggregate();
            openInterest.DataSource =   MarketDataSource.Published;
            openInterest.UpdateTime =   DateTime.Now;
            openInterest.Volume     =   totalVolume;
            openInterest.Vwap       =   totalPriceVolume;

            return openInterest;
        }
        set
        {
            if (value != null)
            {
                openInterest ??= new MarketAggregate();

                openInterest.DataSource = value.DataSource;
                openInterest.UpdateTime = value.UpdateTime;
                openInterest.Volume     = value.Volume;
                openInterest.Vwap       = value.Vwap;
            }
            else if (openInterest != null)
            {
                openInterest.IsEmpty = true;
            }
        }
    }

    public uint DailyTickUpdateCount { get; set; }

    public bool IsLadder
    {
        get => LayerSupportedFlags.HasLadder();
        set => LayerSupportedFlags = value ? LayerFlags.Ladder : LayerFlags.None;
    }

    IMutableOrderBook ITrackableReset<IMutableOrderBook>.ResetWithTracking() => ResetWithTracking();

    public OrderBook ResetWithTracking()
    {
        DailyTickUpdateCount = 0;
        openInterest?.ResetWithTracking();
        BidSide.ResetWithTracking();
        AskSide.ResetWithTracking();
        IsAskBookChanged = false;
        IsBidBookChanged = false;

        return this;
    }

    public override void StateReset()
    {
        DailyTickUpdateCount = 0;
        openInterest?.StateReset();
        BidSide.StateReset();
        AskSide.StateReset();
        IsAskBookChanged = false;
        IsBidBookChanged = false;
        base.StateReset();
    }

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    public override OrderBook Clone() =>
        Recycler?.Borrow<OrderBook>().CopyFrom(this, QuoteInstantBehaviorFlags.DisableUpgradeLayer) ?? new OrderBook(this);

    public override OrderBook CopyFrom
    (IOrderBook source, QuoteInstantBehaviorFlags behaviorFlags,
        CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        cacheBehaviorFlags  = behaviorFlags;
        LayerSupportedFlags = source.LayerSupportedFlags;
        if (source.HasNonEmptyOpenInterest)
        {
            openInterest ??= new MarketAggregate();
            openInterest.CopyFrom(source.OpenInterest, copyMergeFlags);
        }
        else if (openInterest != null)
        {
            openInterest.IsEmpty = true;
        }
        MaxAllowedSize       = source.MaxAllowedSize;
        IsBidBookChanged     = source.IsBidBookChanged;
        IsAskBookChanged     = source.IsAskBookChanged;
        DailyTickUpdateCount = source.DailyTickUpdateCount;
        BidSide.CopyFrom(source.BidSide, behaviorFlags, copyMergeFlags);
        AskSide.CopyFrom(source.AskSide, behaviorFlags, copyMergeFlags);
        return this;
    }

    public bool AreEquivalent(IMutableOrderBook? other, bool exactTypes = false) => AreEquivalent((IOrderBook?)other, exactTypes);

    public override bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != typeof(OrderBook)) return false;

        var layerFlagsSame     = LayerSupportedFlags == other.LayerSupportedFlags;
        var maxDepthSame       = MaxAllowedSize == other.MaxAllowedSize;
        var dailyTickCountSame = DailyTickUpdateCount == other.DailyTickUpdateCount;
        var openInterestSame   = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = openInterest?.AreEquivalent(other.OpenInterest, exactTypes) ?? false;
        }
        var askSideSame = AskSide.AreEquivalent(other.AskSide, exactTypes);
        var bidSideSame = BidSide.AreEquivalent(other.BidSide, exactTypes);

        var allSame = layerFlagsSame && maxDepthSame && dailyTickCountSame && askSideSame && bidSideSame && openInterestSame;
        return allSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBook?)obj, true);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(LayersSupportedType);
        hashCode.Add(IsLadder);
        hashCode.Add(MaxAllowedSize);
        hashCode.Add(IsBidBookChanged);
        hashCode.Add(IsAskBookChanged);
        hashCode.Add(DailyTickUpdateCount);
        hashCode.Add(AskSide);
        hashCode.Add(BidSide);

        return hashCode.ToHashCode();
    }

    protected string OrderBookToStringMembers =>
        $"{nameof(LayersSupportedType)}: {LayersSupportedType}, {nameof(DailyTickUpdateCount)}: {DailyTickUpdateCount}, " +
        $"{nameof(OpenInterest)}: {OpenInterest}, {nameof(IsAskBookChanged)}: {IsAskBookChanged}, " +
        $"{nameof(IsBidBookChanged)}: {IsBidBookChanged}, {nameof(AskSide)}: {AskSide}, {nameof(BidSide)}: {BidSide}, " +
        $"{nameof(IsLadder)}: {IsLadder}";

    public override string ToString() => $"{nameof(OrderBook)}{{{OrderBookToStringMembers}}}";
}
