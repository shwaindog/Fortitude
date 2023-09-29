using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.LastTraded.EntrySelector;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.LastTraded
{
    public class RecentlyTraded : IMutableRecentlyTraded
    {
        public static ILastTradeEntryFlagsSelector<IMutableLastTrade, ISourceTickerQuoteInfo> 
            LastTradeEntrySelector { get; set; } = new RecentlyTradedLastTradeEntrySelector();

        protected readonly IList<IMutableLastTrade> LastTrades;

        public RecentlyTraded(IEnumerable<ILastTrade> lastTrades)
        {
            LastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
        }
        
        public RecentlyTraded(IRecentlyTraded toClone)
        {
           LastTrades = new List<IMutableLastTrade>();
            for (int i = 0; i < toClone.Count; i++)
            {
                LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(toClone[i], true));
            }
        }

        public RecentlyTraded(ISourceTickerQuoteInfo sourceTickerQuoteInfo)
        {
            LastTrades = new List<IMutableLastTrade>(PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);
            for (int i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
            {
                LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(sourceTickerQuoteInfo));
            }
        }

        public bool HasLastTrades => LastTrades.Any( lt => lt.TradePrice != 0);

        public int Capacity
        {
            get
            {
                for (var i = LastTrades.Count - 1; i >= 0; i--)
                {
                    var layerAtLevel = LastTrades[i];
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
                    throw new ArgumentException("Expected RecentlyTraded Capacity to be less than or equal to " +
                                                PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);
                }
                while (LastTrades.Count < value)
                {
                    var cloneFirstLastTrade = LastTrades[0].Clone();
                    cloneFirstLastTrade.Reset();
                    LastTrades.Add(cloneFirstLastTrade);
                }
            }
        }

        public int Count
        {
            get
            {
                for (var i = LastTrades.Count - 1; i >= 0; i--)
                {
                    var layerAtLevel = LastTrades[i];
                    if ((layerAtLevel?.TradePrice ?? 0) > 0)
                    {
                        return i + 1;
                    }
                }
                return 0;
            }
        }

        ILastTrade  IRecentlyTraded.this[int i] => LastTrades[i];

        public IMutableLastTrade this[int i]
        {
            get
            {
                while (LastTrades.Count <= i)
                {
                    LastTrades.Add(new LastTrade());
                }
                return LastTrades[i];
            }
            set
            {
                while (LastTrades.Count <= i)
                {
                    LastTrades.Add(new LastTrade());
                }
                LastTrades[i] = value;
            }
        }

        public void Reset()
        {
            LastTrades.Clear();
        }

        public void Add(IMutableLastTrade newLastTrade)
        {
            if (LastTrades.Count == Count)
            {
                LastTrades.Add(newLastTrade);
            }
            else
            {
                LastTrades[Count] = newLastTrade;
            }
        }

        public void CopyFrom(IRecentlyTraded source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
        {
            var currentDeepestLayerSet = Count;
            var sourceDeepestLayerSet = source.Count;
            for (int i = 0; i < sourceDeepestLayerSet; i++)
            {
                var sourceLayerToCopy = source[i];
                if (i < LastTrades.Count)
                {
                    if (!(LastTrades[i] is IMutableLastTrade mutableLayer))
                    {
                        LastTrades[i] = LastTradeEntrySelector.ConvertToExpectedImplementation(sourceLayerToCopy, true);
                    }
                    else if (sourceLayerToCopy != null)
                    {
                        mutableLayer.CopyFrom(source[i]);
                    }
                    else
                    {
                        LastTrades[i] = null;
                    }
                }
                else
                {
                    LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(sourceLayerToCopy, true));
                }
            }
            for (int i = Math.Min(currentDeepestLayerSet, LastTrades.Count) - 1; i >= sourceDeepestLayerSet; i--)
            {
                LastTrades[i]?.Reset();
            }
        }

        public virtual IMutableRecentlyTraded Clone()
        {
            return new RecentlyTraded(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IRecentlyTraded ICloneable<IRecentlyTraded>.Clone()
        {
            return Clone();
        }
        
        public virtual bool AreEquivalent(IRecentlyTraded other, bool exactTypes = false)
        {
            if (other == null) return false;
            if (exactTypes && other.GetType() != GetType()) return false;
            var bookLayersSame = exactTypes ? LastTrades.SequenceEqual(other)
                : LastTrades.Zip(other, (thisLastTrade, otherLastTrade) => new {thisLastTrade, otherLastTrade})
                .All(joined => joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
            return bookLayersSame;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);
        }

        public override int GetHashCode()
        {
            return LastTrades?.GetHashCode() ?? 0;
        }

        public override string ToString()
        {
            return $"RecentlyTraded {{ {nameof(LastTrades)}: [{string.Join(",", (IEnumerable<ILastTrade>)this)}], " +
                   $"{nameof(Count)}: {Count} }}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<IMutableLastTrade> GetEnumerator()
        {
            return LastTrades.GetEnumerator();
        }
    }
}