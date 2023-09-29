using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook
{
    public class PQOrderBook : IPQOrderBook
    {
        private IList<IPQPriceVolumeLayer> bookLayers;

        public PQOrderBook(IPQSourceTickerQuoteInfo srcTickerQuoteInfo)
        {
            EnsureRelatedItemsAreConfigured(srcTickerQuoteInfo);
        }

        public PQOrderBook(IEnumerable<IPriceVolumeLayer> bookLayers = null)
        {
            this.bookLayers = bookLayers?
                               .Select(pvl => (IPQPriceVolumeLayer)LayerSelector.ConvertToExpectedImplementation(pvl))
                               .ToList() ?? new List<IPQPriceVolumeLayer>();
            if (this.bookLayers.Any())
            {
                this.bookLayers[0].EnsureRelatedItemsAreConfigured((IPQSourceTickerQuoteInfo)null);
                EnsureRelatedItemsAreConfigured(this.bookLayers[0]);
            }
        }

        public PQOrderBook(IOrderBook toClone)
        {
            int size = toClone.Count;
            bookLayers = new List<IPQPriceVolumeLayer>(size);
            for (int i = 0; i < size; i++)
            {
                var pqLayer = (IPQPriceVolumeLayer)LayerSelector.ConvertToExpectedImplementation(toClone[i], true);
                bookLayers.Add(pqLayer);
            }
            if (bookLayers.Count == 0 && toClone.Count == 0)
            {
                bookLayers.Add((IPQPriceVolumeLayer)LayerSelector.ConvertToExpectedImplementation(toClone[0], true));
            }
            Capacity = toClone.Capacity;
        }

        public IPQPriceVolumeLayer this[int level]
        {
            get => bookLayers[level];
            set
            {
                if (value == null && level == bookLayers.Count - 1)
                {
                    bookLayers.RemoveAt(level);
                    return;
                }
                bookLayers[level] = value;
            }
        }

        IPriceVolumeLayer IOrderBook.this[int level] => this[level];
        IMutablePriceVolumeLayer IMutableOrderBook.this[int level]
        {
            get => this[level];
            set => this[level] = (IPQPriceVolumeLayer) value;
        }

        public IList<IPQPriceVolumeLayer> AllLayers
        {
            get => bookLayers;
            set => bookLayers = value;
        }

        public int Capacity
        {
            get => bookLayers.Count;
            set
            {
                if (value > PQFieldKeys.SingleByteFieldIdMaxBookDepth)
                {
                    throw new ArgumentException("Expected PQOrderBook Capacity to be less than or equal to " +
                                                PQFieldKeys.SingleByteFieldIdMaxBookDepth);
                }
                while (bookLayers.Count < value)
                {
                    var clonedFirstLayer = bookLayers[0].Clone();
                    clonedFirstLayer.Reset();
                    bookLayers.Add(clonedFirstLayer);
                } 
            }
        }

        public int Count => bookLayers.Select((pvl, idx) => new { pvl, idx = (int?)(idx + 1) })
                                      .Where(ipvl => ipvl.pvl != null && ipvl.pvl.Price > 0)
                                      .Select(ipvl => ipvl.idx)
                                      .Max() ?? 0;

        public bool HasUpdates
        {
            get { return bookLayers.Any(pqpvl => pqpvl.HasUpdates); }
            set
            {
                foreach (var pqPvLayer in bookLayers)
                {
                    pqPvLayer.HasUpdates = value;
                }
            }
        }

        public static IPQOrderBookLayerFactorySelector LayerSelector { get; set; } 
            = new PQOrderBookLayerFactorySelector();

        public void Reset()
        {
            for (int i = 0; i < bookLayers.Count; i++)
            {
                bookLayers[i]?.Reset();
            }
        }

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle, 
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSetting = null)
        {
            for (int i = 0; i < bookLayers.Count; i++)
            {
                foreach (var layerFields in this[i].GetDeltaUpdateFields(snapShotTime,
                    updateStyle, quotePublicationPrecisionSetting))
                {
                    yield return new PQFieldUpdate((ushort)(layerFields.Id + i), layerFields.Value, layerFields.Flag);
                }
            }
        }

        public int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            if (pqFieldUpdate.Id >= PQFieldKeys.FirstLayersRangeStart 
                && pqFieldUpdate.Id <= PQFieldKeys.FirstLayersRangeEnd)
            {
                var index = (pqFieldUpdate.Id - PQFieldKeys.LayerPriceOffset)%PQFieldKeys.SingleByteFieldIdMaxBookDepth;
                var pqPriceVolumeLayer = this[index];
                return pqPriceVolumeLayer.UpdateField(pqFieldUpdate);
            }
            if (pqFieldUpdate.Id >= PQFieldKeys.SecondLayersRangeStart
                && pqFieldUpdate.Id <= PQFieldKeys.SecondLayersRangeEnd)
            {
                var index = (pqFieldUpdate.Id - PQFieldKeys.SecondLayersRangeStart) % PQFieldKeys.SingleByteFieldIdMaxBookDepth;
                var pqPriceVolumeLayer = this[index];
                return pqPriceVolumeLayer.UpdateField(pqFieldUpdate);
            }
            return -1;
        }


        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
        {
            if (bookLayers.Count <= 0) yield break;
            // All layers share same dictionary or should do anyway
            if (!(this[0] is IPQSupportsStringUpdates<IPriceVolumeLayer> stringUpdateable)) yield break;
            foreach (var stringUpdate in stringUpdateable.GetStringUpdates(snapShotTime, updatedStyle))
            {
                yield return stringUpdate;
            }
        }

        public bool UpdateFieldString(PQFieldStringUpdate updates)
        {
            if (bookLayers.Count > 0)
            {
                //all layers share same dictionary so update any layers dictionary.
                if (this[0] is IPQSupportsStringUpdates<IPriceVolumeLayer> stringUpdateable)
                {
                    return stringUpdateable.UpdateFieldString(updates);
                }
            }
            return false;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IOrderBook ICloneable<IOrderBook>.Clone()
        {
            return Clone();
        }

        IMutableOrderBook ICloneable<IMutableOrderBook>.Clone()
        {
            return Clone();
        }

        IMutableOrderBook IMutableOrderBook.Clone()
        {
            return Clone();
        }

        public IPQOrderBook Clone()
        {
            return new PQOrderBook(this);
        }
        
        public virtual void CopyFrom(IOrderBook source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            IPQPriceVolumeLayer destinationLayer = null;
            IPriceVolumeLayer sourcelayer = null;
            for (int i = 0; i < source.Capacity; i++)
            {
                sourcelayer = source[i];
                bool foundAtIndex = false;
                if(i < bookLayers.Count)
                {
                    var newDestinationLayer = bookLayers[i] ?? destinationLayer;
                    foundAtIndex = !ReferenceEquals(newDestinationLayer, destinationLayer);
                    destinationLayer = newDestinationLayer;
                }
                destinationLayer = LayerSelector.SelectPriceVolumeLayer(foundAtIndex 
                    ? destinationLayer : destinationLayer?.Clone()
                    , sourcelayer);
                bookLayers[i] = destinationLayer;
                destinationLayer?.CopyFrom(sourcelayer);
            }
            for (int i = source.Capacity; i < bookLayers.Count; i++)
            {
                if (bookLayers[i].GetType() == destinationLayer?.GetType())
                {
                    bookLayers[i]?.Reset();
                    continue;
                }
                var newDestinationLayer = bookLayers[i] ?? destinationLayer?.Clone();
                destinationLayer = LayerSelector.SelectPriceVolumeLayer(newDestinationLayer, sourcelayer);
                bookLayers[i] = newDestinationLayer;
            }
        }

        public void EnsureRelatedItemsAreConfigured(IPQSourceTickerQuoteInfo referenceInstance)
        {
            int maxBookDepth = Math.Max((byte)1, Math.Min(referenceInstance.MaximumPublishedLayers,
                PQFieldKeys.SingleByteFieldIdMaxBookDepth));

            var layerFactory = LayerSelector.FindForLayerFlags(referenceInstance);

            bookLayers = bookLayers ?? new List<IPQPriceVolumeLayer>(maxBookDepth);

            for (int i = 0; i < maxBookDepth; i++)
            {
                if (i >= bookLayers.Count || bookLayers[i] == null)
                {
                    bookLayers.Add(layerFactory.CreateNewLayer());
                }
                else if (i < bookLayers.Count && bookLayers[i] != null 
                    && !LayerSelector.TypeCanWholeyContain(layerFactory.LayerCreationType, bookLayers[i].GetType()))
                {
                    bookLayers[i] = layerFactory.UpgradeLayer(bookLayers[i]);
                }
            }
            for (int i = maxBookDepth; i < bookLayers.Count; i++)
            {
                bookLayers.RemoveAt(i);
            }
        }

        public void EnsureRelatedItemsAreConfigured(IPQPriceVolumeLayer referenceInstance)
        {
            int numberOfItemsToCopy = Capacity;
            // first layer copied twice only on bidBook layer.
            for (int i = 0; i < numberOfItemsToCopy; i++)
            {
                bookLayers[i].EnsureRelatedItemsAreConfigured(referenceInstance);
            }
        }

        public virtual bool AreEquivalent(IOrderBook other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;

            var deepestPricedSame = Count == other.Count;
            var bookLayersSame = exactTypes ? bookLayers.SequenceEqual(other)
                : bookLayers.Zip(other, (thisLayer, otherLayer) => new { thisLayer, otherLayer })
                    .All(joined => joined.thisLayer.AreEquivalent(joined.otherLayer));

            var deepestPossibleSame = true;
            if (other is IMutableOrderBook mutableOrderBook)
            {
                deepestPossibleSame = Capacity == mutableOrderBook.Capacity;
            }

            return deepestPossibleSame && deepestPricedSame && bookLayersSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as IOrderBook, true);
        }

        public override int GetHashCode()
        {
            return (bookLayers != null ? bookLayers.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return $"PQOrderBook {{ {nameof(Capacity)}: {Capacity}, {nameof(Count)}: {Count}, " +
                   $"{nameof(AllLayers)}:[{String.Join(", ", bookLayers.Take(Count))}] }}";
        }

        public IEnumerator<IPQPriceVolumeLayer> GetEnumerator()
        {
            return bookLayers.Take(Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<IPriceVolumeLayer> IEnumerable<IPriceVolumeLayer>.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}