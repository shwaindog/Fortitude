// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded.EntrySelector;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public class LastTradedList : ReusableObject<ILastTradedList>, IMutableLastTradedList
{
    protected readonly IList<IMutableLastTrade> LastTrades;

    public LastTradedList() => LastTrades = new List<IMutableLastTrade>();

    public LastTradedList(IEnumerable<ILastTrade> lastTrades)
    {
        LastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
    }

    public LastTradedList(IEnumerable<IMutableLastTrade> lastTrades)
    {
        LastTrades = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
    }

    public LastTradedList(ILastTradedList toClone)
    {
        LastTrades = new List<IMutableLastTrade>();
        for (var i = 0; i < toClone.Count; i++) LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(toClone[i], true));
    }

    public LastTradedList(LastTradedList toClone) : this((ILastTradedList)toClone) { }

    public LastTradedList(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        LastTradesOfType       = LastTradesSupportFlags.MostCompactLayerType();
        LastTrades             = new List<IMutableLastTrade>();
        // for (var i = 0; i < PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades; i++)
        LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
    }

    public static ILastTradeEntryFlagsSelector<IMutableLastTrade>
        LastTradeEntrySelector { get; set; } = new LastTradedLastTradeEntrySelector();

    public LastTradeType LastTradesOfType { get; }

    public LastTradedFlags LastTradesSupportFlags { get; }

    public bool IsReadOnly => false;

    public bool HasLastTrades => LastTrades.Any(lt => !lt.IsEmpty);

    protected Func<IMutableLastTrade> NewElementFactory => () => LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);

    public int Capacity
    {
        get => LastTrades.Count;
        set
        {
            if (value > PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected RecentlyTraded Capacity to be less than or equal to " +
                                            PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            while (LastTrades.Count < value)
            {
                var cloneFirstLastTrade = LastTrades[0].Clone();
                cloneFirstLastTrade.StateReset();
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
                if (layerAtLevel is { IsEmpty: false}) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = LastTrades.Count - 1; i >= value; i--)
            {
                var layerAtLevel = LastTrades[i];
                if (layerAtLevel is { IsEmpty: false}) layerAtLevel.IsEmpty = true;
            }
        }
    }


    ILastTrade IReadOnlyList<ILastTrade>.this[int index] => LastTrades[index];

    public virtual IMutableLastTrade this[int i]
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

    public bool Contains(IMutableLastTrade item) => LastTrades.Contains(item);

    public void CopyTo(IMutableLastTrade[] array, int arrayIndex) => LastTrades.CopyTo(array, arrayIndex);

    public void Insert(int index, IMutableLastTrade item) => LastTrades.Insert(index, item);

    public int IndexOf(IMutableLastTrade item) => LastTrades.IndexOf(item);

    public bool Remove(IMutableLastTrade toRemove) => LastTrades.Remove(toRemove);

    public void RemoveAt (int index) => LastTrades.RemoveAt(index);

    public void Clear()
    {
        foreach (var lastTrade in LastTrades)
        {
            lastTrade.ResetWithTracking();
        }
    }

    public int AppendEntryAtEnd()
    {
        var index = LastTrades.Count;
        LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
        return index;
    }

    IMutableLastTradedList ITrackableReset<IMutableLastTradedList>.ResetWithTracking() => ResetWithTracking();

    public virtual LastTradedList ResetWithTracking()
    {
        foreach (var mutableLastTrade in LastTrades)
        {
            mutableLastTrade.ResetWithTracking();
        }
        return this;
    }

    public override void StateReset()
    {
        LastTrades.Clear();
        base.StateReset();
    }

    public void Add(IMutableLastTrade? newLastTrade)
    {
        if (newLastTrade is null) return;
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
                else if (sourceLayerToCopy.IsEmpty == false)
                    mutableLayer.CopyFrom(source[i]);
                else
                    LastTrades[i].IsEmpty = true;
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
                        .All(joined => joined.thisLastTrade.AreEquivalent(joined.otherLastTrade));
        return bookLayersSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>. GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableLastTrade> IMutableLastTradedList.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableLastTrade> GetEnumerator() => LastTrades.GetEnumerator();

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTradedList, true);

    public override int GetHashCode() => LastTrades.GetHashCode();

    protected string LastTradedListToStringMembers => $"{nameof(LastTrades)}: [{string.Join(",", LastTrades.Take(Count))}], {nameof(Count)}: {Count}";

    public override string ToString() =>
        $"{nameof(LastTradedList)}{{{LastTradedListToStringMembers}}}";
}
