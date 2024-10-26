// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.Quotes.LastTraded.EntrySelector;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LastTraded;

public class RecentlyTraded : ReusableObject<IRecentlyTraded>, IMutableRecentlyTraded
{
    protected readonly IList<IMutableLastTrade?> LastTrades;

    public RecentlyTraded() => LastTrades = new List<IMutableLastTrade?>();

    public RecentlyTraded(IEnumerable<ILastTrade> lastTrades)
    {
        LastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
    }

    public RecentlyTraded(IRecentlyTraded toClone)
    {
        LastTrades = new List<IMutableLastTrade?>();
        for (var i = 0; i < toClone.Count; i++) LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(toClone[i], true));
    }

    public RecentlyTraded(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        LastTradesOfType       = LastTradesSupportFlags.MostCompactLayerType();
        LastTrades             = new List<IMutableLastTrade?>(PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);
        for (var i = 0; i < PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades; i++)
            LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(sourceTickerInfo.LastTradedFlags));
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
            if (value > PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected RecentlyTraded Capacity to be less than or equal to " +
                                            PQFieldKeys.SingleByteFieldIdMaxPossibleLastTrades);
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

    ILastTrade? IRecentlyTraded.this[int i] => LastTrades[i];

    public IMutableLastTrade? this[int i]
    {
        get
        {
            while (LastTrades.Count <= i) LastTrades.Add(new LastTrade());
            return LastTrades[i];
        }
        set
        {
            while (LastTrades.Count <= i) LastTrades.Add(new LastTrade());
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

    public override IRecentlyTraded CopyFrom
    (IRecentlyTraded source
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

    public override IMutableRecentlyTraded Clone() =>
        (IMutableRecentlyTraded?)Recycler?.Borrow<RecentlyTraded>().CopyFrom(this) ?? new RecentlyTraded(this);

    object ICloneable.Clone() => Clone();

    IRecentlyTraded ICloneable<IRecentlyTraded>.Clone() => Clone();

    public virtual bool AreEquivalent(IRecentlyTraded? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var bookLayersSame = exactTypes
            ? LastTrades.SequenceEqual(other)
            : LastTrades.Zip(other, (thisLastTrade, otherLastTrade) => new { thisLastTrade, otherLastTrade })
                        .All(joined =>
                                 joined.thisLastTrade != null && joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
        return bookLayersSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableLastTrade> GetEnumerator() => LastTrades.GetEnumerator()!;

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IRecentlyTraded, true);

    public override int GetHashCode() => LastTrades?.GetHashCode() ?? 0;

    public override string ToString() =>
        $"RecentlyTraded {{ {nameof(LastTrades)}: [{string.Join(",", this)}], " +
        $"{nameof(Count)}: {Count} }}";
}
