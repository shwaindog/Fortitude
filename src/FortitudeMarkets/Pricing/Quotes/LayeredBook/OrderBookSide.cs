// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class OrderBookSide : ReusableObject<IOrderBookSide>, IMutableOrderBookSide
{
    private IList<IPriceVolumeLayer?> bookLayers;

    public OrderBookSide()
    {
        BookSide   = BookSide.Unknown;
        bookLayers = new List<IPriceVolumeLayer?>();
    }

    public OrderBookSide
    (BookSide bookSide, LayerType layerType = LayerType.PriceVolume, ushort maxPublishDepth = SourceTickerInfo.DefaultMaximumPublishedLayers
      , bool isLadder = false)
    {
        BookSide        = bookSide;
        MaxPublishDepth = maxPublishDepth;
        LayersOfType    = layerType;
        bookLayers      = Enumerable.Repeat(LayerSelector?.CreateExpectedImplementation(layerType), maxPublishDepth).ToList();
        IsLadder        = isLadder;
    }

    public OrderBookSide(BookSide bookSide, ushort numBookLayers, bool isLadder = false)
    {
        BookSide        = bookSide;
        MaxPublishDepth = numBookLayers;
        bookLayers      = Enumerable.Repeat(LayerSelector?.CreateExpectedImplementation(LayersOfType), numBookLayers).ToList();
        IsLadder        = isLadder;
    }

    public OrderBookSide(BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers, bool isLadder = false)
    {
        BookSide        = bookSide;
        IsLadder        = isLadder;
        MaxPublishDepth = (ushort)bookLayers.Count();
        this.bookLayers =
            bookLayers
                .Select(pvl => LayerSelector
                            .CreateExpectedImplementation(pvl.LayerType, pvl))
                .Cast<IPriceVolumeLayer?>()
                .ToList();
    }

    public OrderBookSide(IOrderBookSide toClone)
    {
        BookSide        = toClone.BookSide;
        MaxPublishDepth = toClone.MaxPublishDepth;
        bookLayers      = new List<IPriceVolumeLayer?>(toClone.Count());
        LayersOfType    = toClone.LayersOfType;
        IsLadder        = toClone.IsLadder;
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

        LayersOfType    = sourceTickerInfo.LayerFlags.MostCompactLayerType();
        IsLadder        = sourceTickerInfo.LayerFlags.HasLadder();
        MaxPublishDepth = sourceTickerInfo.MaximumPublishedLayers;
        bookLayers =
            Enumerable
                .Repeat(LayerSelector?.CreateExpectedImplementation(LayersOfType), MaxPublishDepth)
                .ToList();
    }

    public ushort MaxPublishDepth { get; private set; }

    public static ILayerFlagsSelector<IPriceVolumeLayer, ISourceTickerInfo> LayerSelector { get; set; } =
        new OrderBookLayerFactorySelector();

    public LayerType LayersOfType { get; private set; } = LayerType.PriceVolume;

    public LayerFlags LayersSupportsLayerFlags => LayersOfType.SupportedLayerFlags();

    public bool IsLadder { get; private set; }

    public OpenInterest? SourceOpenInterest { get; set; }

    public OpenInterest AdapterOpenInterest { get; set; }

    public OpenInterest PublishedOpenInterest
    {
        get
        {
            var vwapResult = this.CalculateVwap();
            return new OpenInterest(MarketDataSource.Published, vwapResult.VolumeAchieved, vwapResult.AchievedVwap);
        }
    }

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
            if (value > PQFieldKeys.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected OrderBook Capacity to be less than or equal to " +
                                            PQFieldKeys.SingleByteFieldIdMaxBookDepth);
            while (bookLayers.Count < value)
            {
                var cloneFirstLayer = LayerSelector.CreateExpectedImplementation(LayersOfType);
                cloneFirstLayer?.StateReset();
                if (cloneFirstLayer != null) bookLayers.Add(cloneFirstLayer);
            }
        }
    }

    public int AppendEntryAtEnd()
    {
        var index = bookLayers.Count;
        bookLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType));
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

        var ladderSame = IsLadder == other.IsLadder;
        var countSame  = Count == other.Count;
        var bookLayersSame = countSame && (exactTypes
            ? this.SequenceEqual(other)
            : bookLayers.Take(Count)
                        .Zip(other.Take(Count), (thisLayer, otherLayer) => new { thisLayer, otherLayer })
                        .All(joined => joined.thisLayer != null
                                    && joined.thisLayer.AreEquivalent(joined.otherLayer)));
        var allAreSame = ladderSame && countSame && bookLayersSame;
        return allAreSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPriceVolumeLayer> GetEnumerator() => bookLayers.Take(Count).GetEnumerator()!;

    public override void StateReset()
    {
        for (var i = 0; i < bookLayers.Count; i++) (bookLayers[i] as IMutablePriceVolumeLayer)?.StateReset();
        base.StateReset();
    }

    public override IOrderBookSide CopyFrom(IOrderBookSide source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayersOfType = source.LayersOfType;
        IsLadder     = source.IsLadder;
        var allSourceLayers = source.Capacity;
        for (var i = 0; i < allSourceLayers; i++)
        {
            var sourceLayer      = source[i];
            var destinationLayer = this[i];

            if (i < bookLayers.Count)
                bookLayers[i] = LayerSelector.UpgradeExistingLayer(destinationLayer, LayersOfType, sourceLayer);
            else
                bookLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType, sourceLayer));

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

    public override string ToString() =>
        $"{nameof(OrderBookSide)}{{{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count},, {nameof(IsLadder)}: {IsLadder},  " +
        $"{nameof(bookLayers)}:[{string.Join(", ", bookLayers.Take(Count))}] }}";
}
