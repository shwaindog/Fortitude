using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.LayeredBook
{
    public class OrderBook : IMutableOrderBook
    {
        public static ILayerFlagsSelector<IPriceVolumeLayer, ISourceTickerQuoteInfo> LayerSelector { get; set; } = 
            new OrderBookLayerFactorySelector();

        protected IList<IPriceVolumeLayer> BookLayers;
        
        public OrderBook(IEnumerable<IPriceVolumeLayer> bookLayers)
        {
            BookLayers = bookLayers?
                             .Select(pvl => LayerSelector.ConvertToExpectedImplementation(pvl))
                             .ToList() ?? new List<IPriceVolumeLayer>();
        }

        public OrderBook(IOrderBook toClone)
        {
            BookLayers = new List<IPriceVolumeLayer>(toClone.Count());
            foreach (var priceVolumeLayer in toClone)
            {
                BookLayers.Add(LayerSelector.ConvertToExpectedImplementation(priceVolumeLayer, true));
            }
            if (BookLayers.Count == 0 && toClone.Count == 0)
            {
                BookLayers.Add(LayerSelector.ConvertToExpectedImplementation(toClone[0], true));
            }
            Capacity = toClone.Capacity;
        }

        public OrderBook(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            BookLayers = new List<IPriceVolumeLayer>(sourceTickerQuoteInfo.MaximumPublishedLayers);
            for (int i = 0; i < sourceTickerQuoteInfo.MaximumPublishedLayers; i++)
            {
                BookLayers.Add(LayerSelector.FindForLayerFlags(sourceTickerQuoteInfo));
            }
        }

        public IMutablePriceVolumeLayer this[int level]
        {
            get => (IMutablePriceVolumeLayer)BookLayers[level];
            set
            {
                if (value == null && level == BookLayers.Count - 1)
                {
                    BookLayers.RemoveAt(level);
                    return;
                }
                BookLayers[level] = value;
            }
        }

        IPriceVolumeLayer IOrderBook.this[int level] => BookLayers[level];

        public int Count => BookLayers.Select((pvl, idx) => new {pvl, idx=(int?)(idx+1)})
                                      .Where(ipvl => ipvl.pvl != null && ipvl.pvl.Price > 0)
                                      .Select(ipvl => ipvl.idx)
                                      .Max() ?? 0;

        public int Capacity
        {
            get => BookLayers.Count;
            set
            {
                if (value > PQFieldKeys.SingleByteFieldIdMaxBookDepth)
                {
                    throw new ArgumentException("Expected OrderBook Capacity to be less than or equal to " +
                                                PQFieldKeys.SingleByteFieldIdMaxBookDepth);
                }
                while (BookLayers.Count < value)
                {
                    var cloneFirstLayer = (IMutablePriceVolumeLayer)BookLayers[0].Clone();
                    cloneFirstLayer.Reset();
                    BookLayers.Add(cloneFirstLayer);
                }
            }
        }

        public void Reset()
        {
            for (int i = 0; i < BookLayers.Count; i++)
            {
                (BookLayers[i] as IMutablePriceVolumeLayer)?.Reset();
            }
        }

        public virtual void CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            var currentDeepestLayerSet = Count;
            var sourceDeepestLayerSet = source.Count;
            for (int i = 0; i < sourceDeepestLayerSet; i++)
            {
                var sourceLayerToCopy = source[i];
                if (i < BookLayers.Count)
                {
                    if (!(BookLayers[i] is IMutablePriceVolumeLayer mutableLayer))
                    {
                        BookLayers[i] = LayerSelector.ConvertToExpectedImplementation(sourceLayerToCopy, true);
                    }
                    else if(sourceLayerToCopy != null)
                    {
                        mutableLayer.CopyFrom(source[i]);
                    }
                    else
                    {
                        BookLayers[i] = null;
                    }
                }
                else
                {
                    BookLayers.Add(LayerSelector.ConvertToExpectedImplementation(sourceLayerToCopy, true));
                }
            }
            for (int i = Math.Min(currentDeepestLayerSet, BookLayers.Count) - 1; i >= sourceDeepestLayerSet; i--)
            {
                (BookLayers[i] as IMutablePriceVolumeLayer)?.Reset();
            }
        }

        public virtual OrderBook Clone()
        {
            return new OrderBook(this);
        }

        IMutableOrderBook ICloneable<IMutableOrderBook>.Clone()
        {
            return Clone();
        }

        IMutableOrderBook IMutableOrderBook.Clone()
        {
            return Clone();
        }

        IOrderBook ICloneable<IOrderBook>.Clone()
        {
            return Clone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public bool AreEquivalent(IOrderBook other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var countSame = Count == other.Count;
            var bookLayersSame = countSame && (exactTypes ? this.SequenceEqual(other)
                : BookLayers.Zip(other, (thisLayer, otherLayer) => new {thisLayer, otherLayer})
                    .All(joined => joined.thisLayer.AreEquivalent(joined.otherLayer)));
            return countSame && bookLayersSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent((IOrderBook) obj, true);
        }

        public override int GetHashCode()
        {
            return BookLayers?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"OrderBook {{{nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, " +
                   $"{nameof(BookLayers)}:[{String.Join(", ", BookLayers.Take(Count))}] }}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IPriceVolumeLayer> GetEnumerator()
        {
            return BookLayers.Take(Count).GetEnumerator();
        }
    }
}