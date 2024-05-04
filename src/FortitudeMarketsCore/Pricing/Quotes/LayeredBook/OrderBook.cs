#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes.LayeredBook;

public class OrderBook : ReusableObject<IOrderBook>, IMutableOrderBook
{
    protected IList<IPriceVolumeLayer?> BookLayers;

    public OrderBook()
    {
        BookSide = BookSide.Unknown;
        BookLayers = new List<IPriceVolumeLayer?>();
    }

    public OrderBook(BookSide bookSide, LayerType? layerType = LayerType.PriceVolume)
    {
        BookSide = bookSide;
        BookLayers = new List<IPriceVolumeLayer?>();
    }

    public OrderBook(BookSide bookSide, int numBookLayers)
    {
        BookSide = bookSide;
        BookLayers = new List<IPriceVolumeLayer?>(numBookLayers);
    }

    public OrderBook(BookSide bookSide, IEnumerable<IPriceVolumeLayer> bookLayers)
    {
        BookSide = bookSide;
        BookLayers = bookLayers
            .Select(pvl => LayerSelector.UpgradeExistingLayer(pvl, pvl.LayerType, pvl)).Cast<IPriceVolumeLayer?>()
            .ToList();
    }

    public OrderBook(IOrderBook toClone)
    {
        BookSide = toClone.BookSide;
        BookLayers = new List<IPriceVolumeLayer?>(toClone.Count());
        LayersOfType = toClone.LayersOfType;
        foreach (var priceVolumeLayer in toClone)
            BookLayers.Add(LayerSelector.CreateExpectedImplementation(priceVolumeLayer.LayerType, priceVolumeLayer));
        Capacity = toClone.Capacity;
    }

    public OrderBook(BookSide bookSide, ISourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        BookSide = bookSide;

        LayersOfType = sourceTickerQuoteInfo.LayerFlags.MostCompactLayerType();
        BookLayers = new List<IPriceVolumeLayer?>(sourceTickerQuoteInfo.MaximumPublishedLayers);
        for (var i = 0; i < sourceTickerQuoteInfo.MaximumPublishedLayers; i++)
            BookLayers.Add(LayerSelector.FindForLayerFlags(sourceTickerQuoteInfo));
    }

    public static ILayerFlagsSelector<IPriceVolumeLayer, ISourceTickerQuoteInfo> LayerSelector { get; set; } =
        new OrderBookLayerFactorySelector();

    public LayerType LayersOfType { get; private set; } = LayerType.PriceVolume;

    public LayerFlags LayersSupportsLayerFlags => LayersOfType.SupportedLayerFlags();

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

    public BookSide BookSide { get; }

    IPriceVolumeLayer? IOrderBook.this[int level] => BookLayers[level];

    public int Count
    {
        get
        {
            for (var i = BookLayers.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = BookLayers[i];
                if ((layerAtLevel?.Price ?? 0) > 0) return i + 1;
            }

            return 0;
        }
    }

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
                var cloneFirstLayer = LayerSelector.CreateExpectedImplementation(LayersOfType);
                cloneFirstLayer?.StateReset();
                if (cloneFirstLayer != null) BookLayers.Add(cloneFirstLayer);
            }
        }
    }

    public override void StateReset()
    {
        for (var i = 0; i < BookLayers.Count; i++) (BookLayers[i] as IMutablePriceVolumeLayer)?.StateReset();
        base.StateReset();
    }

    public override IOrderBook CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LayersOfType = source.LayersOfType;
        var sourceDeepestLayerSet = source.Count;
        for (var i = 0; i < sourceDeepestLayerSet; i++)
        {
            var sourceLayerToCopy = source[i];
            if (i < BookLayers.Count)
            {
                if (sourceLayerToCopy != null)
                    BookLayers[i] = LayerSelector.UpgradeExistingLayer(BookLayers[i], LayersOfType, sourceLayerToCopy);
                else
                    (BookLayers[i] as IMutablePriceVolumeLayer)?.StateReset();
            }
            else
            {
                BookLayers.Add(LayerSelector.CreateExpectedImplementation(LayersOfType, sourceLayerToCopy));
            }
        }

        for (var i = sourceDeepestLayerSet; i < BookLayers.Count; i++)
            (BookLayers[i] as IMutablePriceVolumeLayer)?.StateReset();
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

        var countSame = Count == other.Count;
        var bookLayersSame = countSame && (exactTypes ?
            this.SequenceEqual(other) :
            BookLayers.Zip(other, (thisLayer, otherLayer) => new { thisLayer, otherLayer })
                .All(joined => joined.thisLayer != null && joined.thisLayer.AreEquivalent(joined.otherLayer)));
        return countSame && bookLayersSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IPriceVolumeLayer> GetEnumerator() => BookLayers.Take(Count).GetEnumerator()!;

    public override OrderBook Clone() => (OrderBook?)Recycler?.Borrow<OrderBook>().CopyFrom(this) ?? new OrderBook(this);

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((IOrderBook?)obj, true);

    public override int GetHashCode() => BookLayers?.GetHashCode() ?? 0;

    public override string ToString() =>
        $"OrderBook {{{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, " +
        $"{nameof(BookLayers)}:[{string.Join(", ", BookLayers.Take(Count))}] }}";
}
