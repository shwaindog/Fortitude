// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.Quotes.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

public class LastTradedList : ReusableObject<ILastTradedList>, IMutableLastTradedList
{
    protected readonly IList<IMutableLastTrade?> LastTrades;

    public LastTradedList() => LastTrades = new List<IMutableLastTrade?>();

    public LastTradedList(IEnumerable<ILastTrade> lastTrades)
    {
        LastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
    }

    public LastTradedList(IRecentlyTraded toClone)
    {
        LastTrades = new List<IMutableLastTrade?>();
        for (var i = 0; i < toClone.Count; i++) LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(toClone[i], true));
    }

    public LastTradedList(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        LastTradesOfType       = LastTradesSupportFlags.MostCompactLayerType();
        LastTrades             = new List<IMutableLastTrade?>();
        // for (var i = 0; i < PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades; i++)
        LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
    }

    public static ILastTradeEntryFlagsSelector<IMutableLastTrade>
        LastTradeEntrySelector { get; set; } = new RecentlyTradedLastTradeEntrySelector();

    public LastTradeType LastTradesOfType { get; }

    public LastTradedFlags LastTradesSupportFlags { get; }

    public bool HasLastTrades => LastTrades.Any(lt => lt?.TradePrice != null);

    public int Capacity
    {
        get
        {
            for (var i = LastTrades.Count - 1; i >= 0; i--)
            {
                var layerAtLevel = LastTrades[i];
                if (layerAtLevel != null) return i + 1;
            }

            return 0;
        }
        set
        {
            if (value > PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected RecentlyTraded Capacity to be less than or equal to " +
                                            PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            while (LastTrades.Count < value)
            {
                var cloneFirstLastTrade = LastTrades[0]?.Clone();
                cloneFirstLastTrade?.StateReset();
                if (cloneFirstLastTrade != null) LastTrades.Add(cloneFirstLastTrade);
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
                if ((layerAtLevel?.TradePrice ?? 0) > 0) return i + 1;
            }

            return 0;
        }
    }

    ILastTrade? ILastTradedList.this[int i] => LastTrades[i];

    public IMutableLastTrade? this[int i]
    {
        get
        {
            while (LastTrades.Count <= i) LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
            return LastTrades[i];
        }
        set
        {
            while (LastTrades.Count <= i) LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
            LastTrades[i] = value;
        }
    }

    public int AppendEntryAtEnd()
    {
        var index = LastTrades.Count;
        LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
        return index;
    }

    public override void StateReset()
    {
        LastTrades.Clear();
        base.StateReset();
    }

    public void Add(IMutableLastTrade newLastTrade)
    {
        if (LastTrades.Count == Count)
            LastTrades.Add(newLastTrade);
        else
            LastTrades[Count] = newLastTrade;
    }

    public override ILastTradedList CopyFrom
    (ILastTradedList source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        var currentDeepestLayerSet = Count;
        var sourceDeepestLayerSet  = source.Count;
        for (var i = 0; i < sourceDeepestLayerSet; i++)
        {
            var sourceLayerToCopy = source[i];
            if (i < LastTrades.Count)
            {
                if (!(LastTrades[i] is { } mutableLayer))
                    LastTrades[i] = LastTradeEntrySelector.ConvertToExpectedImplementation(sourceLayerToCopy, true);
                else if (sourceLayerToCopy != null)
                    mutableLayer.CopyFrom(source[i]!);
                else
                    LastTrades[i] = null;
            }
            else
            {
                LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(sourceLayerToCopy, true));
            }
        }

        for (var i = Math.Min(currentDeepestLayerSet, LastTrades.Count) - 1; i >= sourceDeepestLayerSet; i--)
            if (LastTrades[i] is { } mutableLastTrade)
                mutableLastTrade.IsEmpty = true;
        return this;
    }

    public override IMutableLastTradedList Clone() =>
        (IMutableLastTradedList?)Recycler?.Borrow<LastTradedList>().CopyFrom(this) ?? new LastTradedList(this);

    object ICloneable.Clone() => Clone();

    ILastTradedList ICloneable<ILastTradedList>.Clone() => Clone();

    public virtual bool AreEquivalent(ILastTradedList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var countSame = Count == other.Count;
        if (!countSame) return false;
        var bookLayersSame = exactTypes
            ? LastTrades.Take(Count).SequenceEqual(other.Take(Count))
            : LastTrades.Take(Count).Zip(other.Take(Count), (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                        .All(joined =>
                                 joined.thisLastTrade != null && joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
        return bookLayersSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableLastTrade> GetEnumerator() => LastTrades.GetEnumerator()!;

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);

    public override int GetHashCode() => LastTrades?.GetHashCode() ?? 0;

    protected string LastTradedListToStringMembers => $"{nameof(LastTrades)}: [{string.Join(",", LastTrades.Take(Count))}], {nameof(Count)}: {Count}";

    public override string ToString() =>
        $"{nameof(LastTradedList)}{{{LastTradedListToStringMembers}}}";
}
