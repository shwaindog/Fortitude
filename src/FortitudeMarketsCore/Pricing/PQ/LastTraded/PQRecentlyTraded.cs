using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;
using FortitudeMarketsCore.Pricing.PQ.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.LastTraded
{
    public class PQRecentlyTraded : IPQRecentlyTraded
    {
        private IList<IPQLastTrade> lastTrades;

        public PQRecentlyTraded(PQSourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            EnsureRelatedItemsAreConfigured(sourceTickerQuoteInfo);
        }

        public PQRecentlyTraded(IEnumerable<IPQLastTrade> lastTrades)
        {
            this.lastTrades = lastTrades.ToList();
            EnsureRelatedItemsAreConfigured(this.lastTrades[0]);
        }

        public PQRecentlyTraded(IList<IPQLastTrade> lastTrades)
        {
            this.lastTrades = lastTrades;
            EnsureRelatedItemsAreConfigured(lastTrades[0]);
        }

        public PQRecentlyTraded(IRecentlyTraded toClone)
        {
            lastTrades = toClone
                .Select(lt => (IPQLastTrade)LastTradeEntrySelector.ConvertToExpectedImplementation(lt, true)).ToList();
        }

        public static IPQLastTradeTypeSelector LastTradeEntrySelector { get; set; } = new PQLastTradeEntrySelector();
        
        public IPQLastTrade this[int i]
        {
            get => lastTrades[i];
            set => lastTrades[i] = value;
        }

        ILastTrade IRecentlyTraded.this[int i] => this[i];

        IMutableLastTrade IMutableRecentlyTraded.this[int i]
        {
            get => this[i];
            set => this[i] = (IPQLastTrade) value;
        }

        public int Capacity
        {
            get
            {
                for (var i = lastTrades.Count - 1; i >= 0; i--)
                {
                    var layerAtLevel = lastTrades[i];
                    if (layerAtLevel != null)
                    {
                        return i + 1;
                    }
                }
                return 0;
            }
            set
            {
                if (value > PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
                {
                    throw new ArgumentException("Expected PQRecentlyTraded Capacity to be less than or equal to " + 
                        PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);
                }
                while (lastTrades.Count < value)
                {
                    var firstLastTrade = lastTrades[0].Clone();
                    firstLastTrade.Reset();
                    lastTrades.Add(firstLastTrade);
                } 
            }
        }

        public int Count
        {
            get
            {
                for (var i = lastTrades.Count - 1; i >= 0; i--)
                {
                    var layerAtLevel = lastTrades[i];
                    if ((layerAtLevel?.TradePrice ?? 0) > 0)
                    {
                        return i + 1;
                    }
                }
                return 0;
            }
        }

        public bool HasUpdates
        {
            get { return lastTrades.Any(pqpvl => pqpvl.HasUpdates); }
            set
            {
                foreach (var pqLastTrade in lastTrades)
                {
                    pqLastTrade.HasUpdates = value;
                }
            }
        }

        public bool HasLastTrades => Count > 0;

        public void Reset()
        {
            foreach (var lastTrade in lastTrades)
            {
                lastTrade.Reset();
            }
        }

        public void Add(IMutableLastTrade newLastTrade)
        {
            if (lastTrades.Count == Count)
            {
                lastTrades.Add((IPQLastTrade)newLastTrade);
            }
            else
            {
                lastTrades[Count] = (IPQLastTrade)newLastTrade;
            }
        }

        public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, UpdateStyle updateStyle,
            IPQQuotePublicationPrecisionSettings quotePublicationPrecisionSetting = null)
        {
            for (int i = 0; i < lastTrades.Count; i++)
            {
                PQLastTrade lastTrade = (PQLastTrade)this[i];
                foreach (var layerFields in lastTrade.GetDeltaUpdateFields(snapShotTime, updateStyle,
                    quotePublicationPrecisionSetting))
                {
                    yield return new PQFieldUpdate((byte)(layerFields.Id + i), layerFields.Value, layerFields.Flag);
                }
            }
        }

        public int UpdateField(PQFieldUpdate pqFieldUpdate)
        {
            if (pqFieldUpdate.Id >= PQFieldKeys.LastTradedRangeStart 
                && pqFieldUpdate.Id <= PQFieldKeys.LastTradedRangeEnd)
            {
                var index = (pqFieldUpdate.Id - PQFieldKeys.LastTradedRangeStart) % 
                    PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades;
                PQLastTrade pqLastTrade = (PQLastTrade)this[index];
                var result = pqLastTrade.UpdateField(pqFieldUpdate);
                return result;
            }
            return -1;
        }

        public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, UpdateStyle updatedStyle)
        {
            if (lastTrades.Any() && lastTrades[0] is IPQSupportsStringUpdates<ILastTrade> firstLastTrade)
            {
                foreach (var pqLastTradeStringUpdate in firstLastTrade.GetStringUpdates(snapShotTime, updatedStyle))
                {
                    yield return pqLastTradeStringUpdate;
                }
            }
        }

        public bool UpdateFieldString(PQFieldStringUpdate updates)
        {
            if (updates.Field.Id != PQFieldKeys.LastTraderDictionaryUpsertCommand) return false;
            if (lastTrades.Any() && lastTrades[0] is IPQSupportsStringUpdates<ILastTrade> firstLastTrade)
            {
                // all trade layers share same dictionary
                firstLastTrade.UpdateFieldString(updates);
                return true;
            }
            return false;
        }

        public void CopyFrom(IRecentlyTraded source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            IPQLastTrade destinationLayer = null;
            ILastTrade sourcelayer = null;
            for (int i = 0; i < source.Capacity; i++)
            {
                sourcelayer = source[i];
                bool foundAtIndex = false;
                if (i < lastTrades.Count)
                {
                    var newDestinationLayer = lastTrades[i] ?? destinationLayer;
                    foundAtIndex = !ReferenceEquals(newDestinationLayer, destinationLayer);
                    destinationLayer = newDestinationLayer;
                }
                destinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(foundAtIndex
                        ? destinationLayer : destinationLayer?.Clone()
                    , sourcelayer);
                lastTrades[i] = destinationLayer;
                destinationLayer.CopyFrom(sourcelayer);
            }
            for (int i = source.Capacity; i < lastTrades.Count; i++)
            {
                lastTrades[i]?.Reset();
                if (lastTrades[i]?.GetType() == destinationLayer?.GetType()) continue; 
                var newDestinationLayer = lastTrades[i] ?? destinationLayer.Clone();
                destinationLayer = LastTradeEntrySelector.SelectLastTradeEntry(newDestinationLayer, sourcelayer);
                lastTrades[i] = newDestinationLayer;
            }
        }

        public void EnsureRelatedItemsAreConfigured(IPQSourceTickerQuoteInfo referenceInstance)
        {
            var entiresFactory = LastTradeEntrySelector.FindForLastTradeFlags(referenceInstance);

            lastTrades = lastTrades ?? new List<IPQLastTrade>(PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);

            var maxEntries = PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades;

            IPQLastTrade firstEntry = null;
            for (int i = 0; i < maxEntries; i++)
            {
                if (i >= lastTrades.Count || lastTrades[i] == null)
                {
                    lastTrades.Add(entiresFactory.CreateNewLastTradeEntry());
                }
                else if (i < lastTrades.Count && lastTrades[i] != null
                         && !LastTradeEntrySelector.TypeCanWholeyContain(entiresFactory.EntryCreationType,
                             lastTrades[i].GetType()))
                {
                    lastTrades[i] = entiresFactory.UpgradeLayer(lastTrades[i]);
                }
                if (i == 0)
                {
                    firstEntry = lastTrades[i];
                    firstEntry.EnsureRelatedItemsAreConfigured(referenceInstance);
                }
                else
                {
                    lastTrades[i].EnsureRelatedItemsAreConfigured(firstEntry);
                }
            }
            for (int i = lastTrades.Count; i < lastTrades.Count; i++)
            {
                lastTrades.RemoveAt(i);
            }
        }

        public void EnsureRelatedItemsAreConfigured(IPQLastTrade referenceInstance)
        {
            for (var i = 0; i < lastTrades.Count; i++)
            {
                lastTrades[i]?.EnsureRelatedItemsAreConfigured(referenceInstance);
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IRecentlyTraded ICloneable<IRecentlyTraded>.Clone()
        {
            return Clone();
        }

        IPQRecentlyTraded IPQRecentlyTraded.Clone()
        {
            return Clone();
        }

        IMutableRecentlyTraded IMutableRecentlyTraded.Clone()
        {
            return Clone();
        }

        public IPQRecentlyTraded Clone()
        {
            return new PQRecentlyTraded((IRecentlyTraded)this);
        }

        public bool AreEquivalent(IRecentlyTraded other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;
            var bookLayersSame = exactTypes ? lastTrades.SequenceEqual(other)
                : lastTrades.Zip(other, (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                    .All(joined => joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
            return bookLayersSame;
        }

        public override string ToString()
        {
            return $"PQRecentlyTraded {{ {nameof(lastTrades)}: [{string.Join(",", (IEnumerable<ILastTrade>)(this))}], " +
                   $"{nameof(Count)}: {Count}, {nameof(HasUpdates)}: {HasUpdates} }}";
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);
        }

        public override int GetHashCode()
        {
            return lastTrades?.GetHashCode() ?? 0;
        }

        public IEnumerator<IPQLastTrade> GetEnumerator()
        {
            return lastTrades.Take(Count).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<IMutableLastTrade> IMutableRecentlyTraded.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
