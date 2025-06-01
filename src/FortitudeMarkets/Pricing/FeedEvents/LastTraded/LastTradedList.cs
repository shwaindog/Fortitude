// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
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
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;

        LastTrades             = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
    }

    public LastTradedList(IEnumerable<IMutableLastTrade> lastTrades)
    {
        LastTradesSupportFlags = lastTrades.FirstOrDefault()?.SupportsLastTradedFlags ?? LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;
        LastTrades             = lastTrades.Select(lt => LastTradeEntrySelector.ConvertToExpectedImplementation(lt)).ToList();
    }

    public LastTradedList(ILastTradedList toClone)
    {
        LastTradesSupportFlags = toClone.LastTradesSupportFlags;
        LastTrades = new List<IMutableLastTrade>();
        for (var i = 0; i < toClone.Count; i++) LastTrades.Add(LastTradeEntrySelector.ConvertToExpectedImplementation(toClone[i], true));
    }

    public LastTradedList(LastTradedList toClone) : this((ILastTradedList)toClone) { }

    public LastTradedList(ISourceTickerInfo sourceTickerInfo)
    {
        LastTradesSupportFlags = sourceTickerInfo.LastTradedFlags;
        LastTrades             = new List<IMutableLastTrade>();
        // for (var i = 0; i < PQQuoteFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades; i++)
        LastTrades.Add(LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags));
    }

    public static ILastTradeEntryFlagsSelector<IMutableLastTrade>
        LastTradeEntrySelector { get; set; } = new LastTradedLastTradeEntrySelector();
    
    public LastTradeType LastTradesOfType => LastTradesSupportFlags.MostCompactLayerType();
    
    public LastTradedFlags LastTradesSupportFlags { get; private set; } = LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags;

    public bool IsReadOnly => false;

    protected Func<IMutableLastTrade> NewElementFactory => () => LastTradeEntrySelector.FindForLastTradeFlags(LastTradesSupportFlags);

    public int Capacity
    {
        get => LastTrades.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected RecentlyTraded Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
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

    public ushort MaxAllowedSize { get; set; }

    public virtual bool IsEmpty
    {
        get => LastTrades.All(lt => lt.IsEmpty);
        set
        {
            foreach (var lastTrade in LastTrades)
            {
                lastTrade.IsEmpty = value;
            }
        }
    }

    public int IndexOf(IMutableLastTrade item) => LastTrades.IndexOf(item);

    public bool Contains(IMutableLastTrade item) => LastTrades.Contains(item);

    public void CopyTo(IMutableLastTrade[] array, int arrayIndex) => LastTrades.CopyTo(array, arrayIndex);

    public virtual void Insert(int index, IMutableLastTrade item) => LastTrades.Insert(index, item);

    public virtual bool Remove(IMutableLastTrade toRemove) => LastTrades.Remove(toRemove);

    public virtual void RemoveAt (int index) => LastTrades.RemoveAt(index);

    public virtual void Add(IMutableLastTrade newLastTrade)
    {
        var nonEmptyCount = Count;
        if (LastTrades.Count == nonEmptyCount)
            LastTrades.Add(newLastTrade);
        else
            LastTrades[nonEmptyCount] = newLastTrade;
    }

    public virtual void Clear()
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

    IMutableLastTradedList IMutableLastTradedList.ResetWithTracking() => ResetWithTracking();

    ITracksResetCappedCapacityList<IMutableLastTrade> ITrackableReset<ITracksResetCappedCapacityList<IMutableLastTrade>>.ResetWithTracking() => ResetWithTracking();

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
    }

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

    public override ILastTradedList CopyFrom
    (ILastTradedList source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        LastTradesSupportFlags |= source.LastTradesSupportFlags;
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
        
        var allLastTradesSame = true;
        for (int i = 0; i < Count && allLastTradesSame; i++)
        {
            var localLastTrade = this[i];
            var otherLastTrade = other[i];
            allLastTradesSame &= localLastTrade.AreEquivalent(otherLastTrade, exactTypes);
        }
        return countSame && allLastTradesSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<ILastTrade> IEnumerable<ILastTrade>. GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableLastTrade> IMutableLastTradedList.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableLastTrade> GetEnumerator() => LastTrades.GetEnumerator();

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILastTradedList, true);
    
    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var lastTrade in LastTrades)
        {
            hashCode.Add(lastTrade);
        }
        return hashCode.ToHashCode();
    }

    public string EachLastTradeByIndexOnNewLines()
    {
        var count = Count;
        var sb    = new StringBuilder(100 * count);
        for (var i = 0; i < count; i++)
        {
            var lastTrade = LastTrades[i];
            sb.Append("[").Append(i).Append("] = ").Append(lastTrade);
            if (i < count - 1)
            {
                sb.AppendLine(",");
            }
        }
        return sb.ToString();
    }

    protected string NonLastTradedListToStringMembers => $"{nameof(Count)}: {Count}, {nameof(MaxAllowedSize)}: {MaxAllowedSize:N0}";
    protected string LastTradedListToString => $"{nameof(LastTrades)}: [{EachLastTradeByIndexOnNewLines()}]";

    public override string ToString() =>
        $"{nameof(LastTradedList)}{{{NonLastTradedListToStringMembers}, {LastTradedListToString}}}";
}
