#region

using System.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

#endregion

namespace FortitudeMarketsCore.Pricing.LayeredBook;

public class OrderBook : IMutableOrderBook
{
    protected IList<IPriceVolumeLayer?> BookLayers;

    public OrderBook(int numBookLayers) => BookLayers = new List<IPriceVolumeLayer?>(numBookLayers);

    public OrderBook(IEnumerable<IPriceVolumeLayer> bookLayers)
    {
        BookLayers = bookLayers?.Select(pvl => LayerSelector.ConvertToExpectedImplementation(pvl))
            ?.ToList() ?? new List<IPriceVolumeLayer?>();
    }

    public OrderBook(IOrderBook toClone)
    {
        BookLayers = new List<IPriceVolumeLayer?>(toClone.Count());
        foreach (var priceVolumeLayer in toClone)
            BookLayers.Add(LayerSelector.ConvertToExpectedImplementation(priceVolumeLayer, true));
        if (BookLayers.Count == 0 && toClone.Count == 0)
            BookLayers.Add(LayerSelector.ConvertToExpectedImplementation(toClone[0], true));
        Capacity = toClone.Capacity;
    }

    public OrderBook(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        BookLayers = new List<IPriceVolumeLayer?>(sourceTickerQuoteInfo.MaximumPublishedLayers);
        for (var i = 0; i < sourceTickerQuoteInfo.MaximumPublishedLayers; i++)
            BookLayers.Add(LayerSelector.FindForLayerFlags(sourceTickerQuoteInfo));
    }

    public static ILayerFlagsSelector<IPriceVolumeLayer, ISourceTickerQuoteInfo> LayerSelector { get; set; } =
        new OrderBookLayerFactorySelector();

    public IMutablePriceVolumeLayer? this[int level]
    {
        get => (IMutablePriceVolumeLayer?)BookLayers[level];
        set
        {
            if (value == null && level == BookLayers.Count - 1)
            {
                BookLayers.RemoveAt(level);
                return;
            }

            BookLayers[level] = value!;
        }
    }

    IPriceVolumeLayer? IOrderBook.this[int level] => BookLayers[level];

    public int Count =>
        BookLayers.Select((pvl, idx) => new { pvl, idx = (int?)(idx + 1) })
            .Where(ipvl => ipvl.pvl != null && ipvl.pvl.Price > 0)
            .Select(ipvl => ipvl.idx)
            .Max() ?? 0;

    public int Capacity
    {
        get => BookLayers.Count;
        set
        {
            if (value > PQFieldKeys.SingleByteFieldIdMaxBookDepth)
                throw new ArgumentException("Expected OrderBook Capacity to be less than or equal to " +
                                            PQFieldKeys.SingleByteFieldIdMaxBookDepth);
            while (BookLayers.Count < value)
            {
                var cloneFirstLayer = (IMutablePriceVolumeLayer?)BookLayers[0]?.Clone();
                cloneFirstLayer?.Reset();
                if (cloneFirstLayer != null) BookLayers.Add(cloneFirstLayer);
            }
        }
    }

    public void Reset()
    {
        for (var i = 0; i < BookLayers.Count; i++) (BookLayers[i] as IMutablePriceVolumeLayer)?.Reset();
    }

    public virtual void CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var sourceDeepestLayerSet = source.Count;
        for (var i = 0; i < sourceDeepestLayerSet; i++)
        {
            var sourceLayerToCopy = source[i];
            if (i < BookLayers.Count)
            {
                if (!(BookLayers[i] is IMutablePriceVolumeLayer mutableLayer))
                    BookLayers[i] = LayerSelector.ConvertToExpectedImplementation(sourceLayerToCopy, true);
                else if (sourceLayerToCopy != null)
                    mutableLayer.CopyFrom(sourceLayerToCopy);
                else
                    (BookLayers[i] as IMutablePriceVolumeLayer)?.Reset();
            }
            else
            {
                BookLayers.Add(LayerSelector.ConvertToExpectedImplementation(sourceLayerToCopy, true));
            }
        }

        for (var i = sourceDeepestLayerSet; i < BookLayers.Count; i++)
            (BookLayers[i] as IMutablePriceVolumeLayer)?.Reset();
    }

    IMutableOrderBook ICloneable<IMutableOrderBook>.Clone() => Clone();

    IMutableOrderBook IMutableOrderBook.Clone() => Clone();

    IOrderBook ICloneable<IOrderBook>.Clone() => Clone();

    object ICloneable.Clone() => Clone();

    public bool AreEquivalent(IOrderBook? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;

        var countSame = Count == other.Count;
        var bookLayersSame = countSame && (exactTypes ?
            this.SequenceEqual(other) :
            BookLayers.Zip(other, (thisLayer, otherLayer) => new { thisLayer, otherLayer })
                .All(joined => joined.thisLayer != null && joined.thisLayer.AreEquivalent(joined.otherLayer)));
        return countSame && bookLayersSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPriceVolumeLayer> GetEnumerator() => BookLayers.Take(Count).GetEnumerator()!;

    public virtual OrderBook Clone() => new(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBook?)obj, true);

    public override int GetHashCode() => BookLayers?.GetHashCode() ?? 0;

    public override string ToString() =>
        $"OrderBook {{{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, " +
        $"{nameof(BookLayers)}:[{string.Join(", ", BookLayers.Take(Count))}] }}";
}
