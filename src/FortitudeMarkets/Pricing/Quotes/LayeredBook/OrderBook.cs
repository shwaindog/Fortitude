// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public class OrderBook : ReusableObject<IOrderBook>, IMutableOrderBook
{
    private IList<IPriceVolumeLayer?> bookLayers;

    public OrderBook()
    {
        BookSide   = BookSide.Unknown;
        bookLayers = new List<IPriceVolumeLayer?>();
    }

    public OrderBook(BookSide bookSide, LayerType layerType = LayerType.PriceVolume, bool isLadder = false)
    {
        BookSide     = bookSide;
        LayersOfType = layerType;
        bookLayers   = new List<IPriceVolumeLayer?>();
        IsLadder     = isLadder;
    }

    public OrderBook(BookSide bookSide, int numBookLayers, bool isLadder = false)
    {
        BookSide   = bookSide;
        bookLayers = new List<IPriceVolumeLayer?>(numBookLayers);
        IsLadder   = isLadder;
    }

    public OrderBook(BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers, bool isLadder = false)
    {
        BookSide = bookSide;
        IsLadder = isLadder;
        this.bookLayers = bookLayers
                          .Select(pvl => LayerSelector
                                      .UpgradeExistingLayer(pvl, pvl.LayerType, pvl))
                          .Cast<IPriceVolumeLayer?>()
                          .ToList();
    }

    public OrderBook(IOrderBook toClone)
    {
        BookSide     = toClone.BookSide;
        bookLayers   = new List<IPriceVolumeLayer?>(toClone.Count());
        LayersOfType = toClone.LayersOfType;
        IsLadder     = toClone.IsLadder;
        foreach (var priceVolumeLayer in toClone)
            bookLayers.Add(LayerSelector.CreateExpectedImplementation(priceVolumeLayer.LayerType, priceVolumeLayer));
        Capacity = toClone.Capacity;
    }

    public OrderBook(BookSide bookSide, ISourceTickerInfo sourceTickerInfo)
    {
        BookSide = bookSide;

        LayersOfType = sourceTickerInfo.LayerFlags.MostCompactLayerType();
        IsLadder     = sourceTickerInfo.LayerFlags.HasLadder();
        bookLayers   = new List<IPriceVolumeLayer?>(sourceTickerInfo.MaximumPublishedLayers);
        for (var i = 0; i < sourceTickerInfo.MaximumPublishedLayers; i++) bookLayers.Add(LayerSelector.FindForLayerFlags(sourceTickerInfo));
    }

    public static ILayerFlagsSelector<IPriceVolumeLayer, ISourceTickerInfo> LayerSelector { get; set; } =
        new OrderBookLayerFactorySelector();

    public LayerType LayersOfType { get; private set; } = LayerType.PriceVolume;

    public LayerFlags LayersSupportsLayerFlags => LayersOfType.SupportedLayerFlags();

    public bool IsLadder { get; private set; }

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

    IPriceVolumeLayer? IOrderBook.this[int level] => this[level];

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

    public override void StateReset()
    {
        for (var i = 0; i < bookLayers.Count; i++) (bookLayers[i] as IMutablePriceVolumeLayer)?.StateReset();
        base.StateReset();
    }

    public override IOrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayersOfType = source.LayersOfType;
        IsLadder     = source.IsLadder;
        var sourceDeepestLayerSet = source.Count;
        for (var i = 0; i < sourceDeepestLayerSet; i++)
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
        for (var i = sourceDeepestLayerSet; i < source.Capacity; i++)
            bookLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType, null, copyMergeFlags));

        for (var i = sourceDeepestLayerSet; i < bookLayers.Count; i++)
            if (bookLayers[i] is IMutablePriceVolumeLayer mutablePvl)
                mutablePvl.IsEmpty = true;
        return this;
    }

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    IOrderBook ICloneable<IOrderBook>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
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

    public override OrderBook Clone() => (OrderBook?)Recycler?.Borrow<OrderBook>().CopyFrom(this) ?? new OrderBook(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBook?)obj, true);

    public override int GetHashCode() => bookLayers?.GetHashCode() ?? 0;

    public override string ToString() =>
        $"OrderBook {{{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count},, {nameof(IsLadder)}: {IsLadder},  " +
        $"{nameof(bookLayers)}:[{string.Join(", ", bookLayers.Take(Count))}] }}";
}
