// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.LayerSelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;

public class OrderBookSide : ReusableObject<IOrderBookSide>, IMutableOrderBookSide
{
    private IList<IPriceVolumeLayer?> bookLayers;

    private ushort maxPublishDepth;

    private LayerFlags            layerFlags = LayerFlagsExtensions.PriceVolumeLayerFlags;
    private IMutableMarketAggregate? openInterestSide;

    public OrderBookSide()
    {
        BookSide   = BookSide.Unknown;
        bookLayers = new List<IPriceVolumeLayer?>();
        bookLayers.Add(LayerSelector?.CreateExpectedImplementation(LayerSupportedType));
    }

    public OrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume, ushort maxPublishDepth = SourceTickerInfo.DefaultMaximumPublishedLayers
      , bool isLadder = false)
    {
        BookSide        = bookSide;
        MaxPublishDepth = maxPublishDepth;
        LayerSupportedFlags = bookLayers?.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags |
            (isLadder ? LayerFlags.Ladder : LayerFlags.None);
        layerFlags |= LayerSupportedType.SupportedLayerFlags();
        bookLayers =  Enumerable.Repeat(LayerSelector?.CreateExpectedImplementation(layerType), maxPublishDepth).ToList();
        IsLadder   =  isLadder;
    }

    public OrderBookSide(BookSide bookSide, ushort numBookLayers, bool isLadder = false)
    {
        BookSide                 = bookSide;
        MaxPublishDepth          = numBookLayers;
        LayerSupportedFlags = (isLadder ? LayerFlags.Ladder : LayerFlags.None);
        bookLayers               = Enumerable.Repeat(LayerSelector?.CreateExpectedImplementation(LayerSupportedType), numBookLayers).ToList();
        IsLadder                 = isLadder;
    }

    public OrderBookSide(BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers)
    {
        BookSide = bookSide;

        LayerSupportedFlags = bookLayers.FirstOrDefault()?.SupportsLayerFlags ?? LayerFlagsExtensions.PriceVolumeLayerFlags;
        MaxPublishDepth          = (ushort)bookLayers.Count();
        this.bookLayers =
            bookLayers
                .Select(pvl => LayerSelector
                            .CreateExpectedImplementation(pvl.LayerType, pvl))
                .Cast<IPriceVolumeLayer?>()
                .ToList();
    }

    public OrderBookSide(IOrderBookSide toClone)
    {
        BookSide                 =  toClone.BookSide;
        MaxPublishDepth          =  toClone.MaxPublishDepth;
        LayerSupportedFlags =  toClone.LayerSupportedFlags;
        layerFlags               |= LayerSupportedType.SupportedLayerFlags();
        IsLadder                 =  toClone.IsLadder;
        DailyTickUpdateCount = toClone.DailyTickUpdateCount;
        if (toClone.HasNonEmptyOpenInterest)
        {
            openInterestSide = new MarketAggregate(toClone.OpenInterestSide);
        }
        bookLayers =
            toClone
                .Select
                    (pvl => LayerSelector?.CreateExpectedImplementation(pvl.LayerType, pvl))
                .ToList();
        Capacity = toClone.Capacity;
    }

    public OrderBookSide(BookSide bookSide, ISourceTickerInfo sourceTickerInfo)
    {
        BookSide = bookSide;

        LayerSupportedFlags =  sourceTickerInfo.LayerFlags;
        layerFlags               |= LayerSupportedType.SupportedLayerFlags();

        MaxPublishDepth = sourceTickerInfo.MaximumPublishedLayers;
        bookLayers =
            Enumerable
                .Repeat(LayerSelector?.CreateExpectedImplementation(LayerSupportedType), MaxPublishDepth)
                .ToList();
    }


    public ushort MaxPublishDepth
    {
        get => maxPublishDepth;
        private set => maxPublishDepth = Math.Max((byte)1, Math.Min(value, PQQuoteFieldsExtensions.TwoByteFieldIdMaxBookDepth));
    }

    public static ILayerFlagsSelector<IPriceVolumeLayer> LayerSelector { get; set; } = new OrderBookLayerFactorySelector();

    public LayerType LayerSupportedType
    {
        get => LayerSupportedFlags.MostCompactLayerType();
        private set => LayerSupportedFlags |= value.SupportedLayerFlags();
    }

    public LayerFlags LayerSupportedFlags
    {
        get => layerFlags;
        set => layerFlags = layerFlags.Unset(LayerFlags.Ladder) | value;
    }
    
    public bool IsLadder
    {
        get => layerFlags.HasLadder();
        set => LayerSupportedFlags = value ? LayerFlags.Ladder : LayerFlags.None;
    }
    public bool HasNonEmptyOpenInterest
    {
        get => openInterestSide is {IsEmpty: false};
        set
        {
            if (value) return;
            if (openInterestSide != null)
            {
                openInterestSide.IsEmpty = true;
            }
        }
    }

    IMarketAggregate IOrderBookSide.OpenInterestSide => OpenInterestSideSide!;
    IMutableMarketAggregate? IMutableOrderBookSide.OpenInterestSide
    {
        get => OpenInterestSideSide;
        set => OpenInterestSideSide = (MarketAggregate?)value;
    }

    public IMutableMarketAggregate? OpenInterestSideSide
    {
        get
        {
            if (HasNonEmptyOpenInterest && openInterestSide is not {DataSource: (MarketDataSource.Published or MarketDataSource.None)}) return openInterestSide;
            var vwapResult = this.CalculateVwap();
            
            openInterestSide            ??= new MarketAggregate();
            openInterestSide.DataSource =   MarketDataSource.Published;
            openInterestSide.UpdateTime =   DateTime.Now;
            openInterestSide.Volume     =   vwapResult.VolumeAchieved;
            openInterestSide.Vwap       =   vwapResult.AchievedVwap;
            return openInterestSide;
        }
        set
        {
            if (value != null)
            {
                openInterestSide ??= new MarketAggregate();

                openInterestSide.DataSource = value.DataSource;
                openInterestSide.UpdateTime = value.UpdateTime;
                openInterestSide.Volume     = value.Volume;
                openInterestSide.Vwap       = value.Vwap;
            }
            else if (openInterestSide != null)
            {
                openInterestSide.IsEmpty = true;
            }
        }
    }

    public uint DailyTickUpdateCount { get; set; }


    public IMutablePriceVolumeLayer? this[int level]
    {
        get => level < bookLayers.Count && level >= 0 ? (IMutablePriceVolumeLayer)bookLayers[level]! : null;
        set
        {
            if (value == null && level == bookLayers.Count - 1)
            {
                bookLayers.RemoveAt(level);
                return;
            }

            bookLayers[level] = value!;
        }
    }

    public BookSide BookSide { get; }

    IPriceVolumeLayer? IOrderBookSide.this[int level] => this[level];

    public int Count
    {
        get
        {
            for (var i = bookLayers.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = bookLayers[i];
                if ((layerAtLevel?.Price ?? 0) > 0) return i + 1;
            }

            return 0;
        }
    }

    public int Capacity
    {
        get => bookLayers.Count;
        set
        {
            if (value > PQQuoteFieldsExtensions.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected OrderBook Capacity to be less than or equal to " +
                                            PQQuoteFieldsExtensions.SingleByteFieldIdMaxBookDepth);
            while (bookLayers.Count < value)
            {
                var cloneFirstLayer = LayerSelector.CreateExpectedImplementation(LayerSupportedType);
                cloneFirstLayer?.StateReset();
                if (cloneFirstLayer != null) bookLayers.Add(cloneFirstLayer);
            }
        }
    }

    public int AppendEntryAtEnd()
    {
        var index = bookLayers.Count;
        bookLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType));
        return index;
    }

    IMutableOrderBookSide ICloneable<IMutableOrderBookSide>.Clone() => Clone();

    IMutableOrderBookSide IMutableOrderBookSide.Clone() => Clone();

    IOrderBookSide ICloneable<IOrderBookSide>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public bool AreEquivalent(IOrderBookSide? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var ladderSame       = IsLadder == other.IsLadder;
        var countSame        = Count == other.Count;
        var maxPubDepthSame  = MaxPublishDepth == other.MaxPublishDepth;
        var openInterestSame = HasNonEmptyOpenInterest == other.HasNonEmptyOpenInterest;
        if (openInterestSame && other.HasNonEmptyOpenInterest && HasNonEmptyOpenInterest)
        {
            openInterestSame = Equals(openInterestSide, other.OpenInterestSide);
        }
        var bookLayersSame  = true;
        for (int i = 0; i < Count; i++)
        {
            var localLayer  = this[i];
            var otherLayer  = other[i];
            var layerIsSame = localLayer?.AreEquivalent(otherLayer, exactTypes) ?? otherLayer == null;
            bookLayersSame &= layerIsSame;
        }
        var allAreSame = ladderSame && countSame && maxPubDepthSame && bookLayersSame && openInterestSame;
        return allAreSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPriceVolumeLayer> GetEnumerator() => bookLayers.Take(Count).GetEnumerator()!;

    
    public OrderBookSide ResetWithTracking()
    {
        for (var i = 0; i < bookLayers.Count; i++) (bookLayers[i] as IMutablePriceVolumeLayer)?.ResetWithTracking();
        openInterestSide?.ResetWithTracking();

        return this;
    }
    
    IMutableOrderBookSide ITrackableReset<IMutableOrderBookSide>.ResetWithTracking() => ResetWithTracking();

    public override void StateReset()
    {
        for (var i = 0; i < bookLayers.Count; i++) (bookLayers[i] as IMutablePriceVolumeLayer)?.StateReset();
        openInterestSide?.StateReset();
        base.StateReset();
    }


    public override OrderBookSide CopyFrom(IOrderBookSide source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayerSupportedFlags = source.LayerSupportedFlags;
        MaxPublishDepth          = source.MaxPublishDepth;
        if (source.HasNonEmptyOpenInterest)
        {
            openInterestSide ??= new MarketAggregate();
            openInterestSide.CopyFrom(source.OpenInterestSide, copyMergeFlags);
        } else if (openInterestSide != null)
        {
            openInterestSide.IsEmpty = true;
        }
        var originalCount   = Count;
        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer = source[i];
            if (sourceLayer is null or { IsEmpty: true } && i >= originalCount) continue;
            var destinationLayer = this[i];

            if (i < bookLayers.Count)
                bookLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, LayerSupportedType, sourceLayer);
            else
                bookLayers.Add(LayerSelector.CreateExpectedImplementation(LayerSupportedType, sourceLayer));

            if (sourceLayer is { IsEmpty: false }) continue;
            if (destinationLayer is { } mutablePriceVolumeLayer) mutablePriceVolumeLayer.IsEmpty = true;
        }

        for (var i = source.Count; i < bookLayers.Count; i++)
            if (bookLayers[i] is IMutablePriceVolumeLayer mutablePvl)
                mutablePvl.IsEmpty = true;
        return this;
    }

    public override OrderBookSide Clone() => (OrderBookSide?)Recycler?.Borrow<OrderBookSide>().CopyFrom(this) ?? new OrderBookSide(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBookSide?)obj, true);

    public override int GetHashCode() => bookLayers?.GetHashCode() ?? 0;

    protected string OrderBookSideToStringMembers =>
        $"{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count},, {nameof(LayerSupportedFlags)}: {LayerSupportedFlags}, " +
        $"{nameof(OpenInterestSideSide)}: {OpenInterestSideSide},  {nameof(bookLayers)}:[{string.Join(", ", bookLayers.Take(Count))}]";

    public override string ToString() =>
        $"{nameof(OrderBookSide)}{{{OrderBookSideToStringMembers}}}";
}
