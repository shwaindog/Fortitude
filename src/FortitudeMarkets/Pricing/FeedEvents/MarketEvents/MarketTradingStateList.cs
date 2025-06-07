// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public class MarketTradingStateList : ReusableObject<IMarketTradingStateList>, IMutableMarketTradingStateList
{
    protected readonly IList<IMutableMarketTradingStateEvent> TradingEventsList;


    private readonly TracksListReorderingRegistry<IMutableMarketTradingStateEvent, IMarketTradingStateEvent> elementShiftRegistry;

    public MarketTradingStateList()
    {
        TradingEventsList = new List<IMutableMarketTradingStateEvent>();

        elementShiftRegistry = new TracksListReorderingRegistry<IMutableMarketTradingStateEvent, IMarketTradingStateEvent>(this, NewElementFactory, SameEventSequenceId);
    }

    public MarketTradingStateList(IEnumerable<IMarketTradingStateEvent> tradingStates)
    {
        TradingEventsList = tradingStates.Select(ts => Convert(ts, true)).ToList();

        elementShiftRegistry = new TracksListReorderingRegistry<IMutableMarketTradingStateEvent, IMarketTradingStateEvent>(this, NewElementFactory, SameEventSequenceId);
    }

    public MarketTradingStateList(IList<IMutableMarketTradingStateEvent> tradingStates)
    {
        TradingEventsList = tradingStates.Select(ts => Convert(ts)).ToList();

        elementShiftRegistry = new TracksListReorderingRegistry<IMutableMarketTradingStateEvent, IMarketTradingStateEvent>(this, NewElementFactory, SameEventSequenceId);
    }

    public MarketTradingStateList(IMarketTradingStateList toClone)
    {
        TradingEventsList = new List<IMutableMarketTradingStateEvent>();
        for (var i = 0; i < toClone.Count; i++) TradingEventsList.Add(Convert(toClone[i], true));

        elementShiftRegistry = new TracksListReorderingRegistry<IMutableMarketTradingStateEvent, IMarketTradingStateEvent>(this, NewElementFactory, SameEventSequenceId);
    }

    public MarketTradingStateList(MarketTradingStateList toClone) : this((IMarketTradingStateList)toClone) { }

    public IMutableMarketTradingStateEvent Convert(IMarketTradingStateEvent marketTradingStateEvent, bool clone = false)
    {
        if (clone || marketTradingStateEvent is not MarketTradingStateEvent) return new MarketTradingStateEvent(marketTradingStateEvent);
        return (IMutableMarketTradingStateEvent)marketTradingStateEvent;
    }

    public bool IsReadOnly => false;

    protected Func<IMutableMarketTradingStateEvent> NewElementFactory => () => new MarketTradingStateEvent();


    protected static Func<IMarketTradingStateEvent, IMarketTradingStateEvent, bool> SameEventSequenceId = (lhs, rhs) => lhs.EventSequenceId == rhs.EventSequenceId;

    public int Capacity
    {
        get => TradingEventsList.Count;
        set
        {
            if (value > PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades)
                throw new ArgumentException("Expected TradingStateList Capacity to be less than or equal to " +
                                            PQFeedFieldsExtensions.SingleByteFieldIdMaxPossibleLastTrades);
            while (TradingEventsList.Count < value)
            {
                var clonedTradingState = NewElementFactory();
                clonedTradingState.StateReset();
                TradingEventsList.Add(clonedTradingState);
            }
        }
    }

    public int Count
    {
        get
        {
            for (var i = TradingEventsList.Count - 1; i >= 0; i--)
            {
                var tradingState = TradingEventsList[i];

                if (tradingState is { IsEmpty: false }) return i + 1;
            }

            return 0;
        }
        set
        {
            for (var i = TradingEventsList.Count - 1; i >= value; i--)
            {
                var tradingState = TradingEventsList[i];

                if (tradingState is { IsEmpty: false }) tradingState.IsEmpty = true;
            }
        }
    }

    IMarketTradingStateEvent IReadOnlyList<IMarketTradingStateEvent>.this[int index] => TradingEventsList[index];

    public virtual IMutableMarketTradingStateEvent this[int i]
    {
        get
        {
            while (TradingEventsList.Count <= i) TradingEventsList.Add(NewElementFactory());
            return TradingEventsList[i];
        }
        set
        {
            while (TradingEventsList.Count <= i) TradingEventsList.Add(NewElementFactory());
            TradingEventsList[i] = value;
        }
    }

    public ushort MaxAllowedSize { get; set; }

    public virtual bool IsEmpty
    {
        get => TradingEventsList.All(lt => lt.IsEmpty);
        set
        {
            foreach (var tradingState in TradingEventsList)
            {
                tradingState.IsEmpty = value;
            }
        }
    }

    public IReadOnlyList<ListShiftCommand> ShiftCommands
    {
        get => elementShiftRegistry.ShiftCommands;
        set => elementShiftRegistry.ShiftCommands = value;
    }

    public bool HasUnreliableListTracking
    {
        get => elementShiftRegistry.HasUnreliableListTracking;
        set => elementShiftRegistry.HasUnreliableListTracking = value;
    }

    public int? ClearRemainingElementsFromIndex
    {
        get => elementShiftRegistry.ClearRemainingElementsFromIndex;
        set => elementShiftRegistry.ClearRemainingElementsFromIndex = value;
    }

    public ListShiftCommand AppendShiftCommand(ListShiftCommand toAppendAtEnd) => elementShiftRegistry.AppendShiftCommand(toAppendAtEnd);

    public void ClearShiftCommands()
    {
        elementShiftRegistry.ClearShiftCommands();
    }

    public ListShiftCommand InsertAtStart(IMutableMarketTradingStateEvent toInsertAtStart) => elementShiftRegistry.InsertAtStart(toInsertAtStart);

    public bool AppendAtEnd(IMutableMarketTradingStateEvent toAppendAtEnd) => elementShiftRegistry.AppendAtEnd(toAppendAtEnd);

    public ListShiftCommand InsertAt(int index, IMutableMarketTradingStateEvent toInsertAtStart) => elementShiftRegistry.InsertAt(index, toInsertAtStart);

    public ListShiftCommand DeleteAt(int index) => elementShiftRegistry.DeleteAt(index);

    public ListShiftCommand Delete(IMutableMarketTradingStateEvent toDelete) => elementShiftRegistry.Delete(toDelete);

    public ListShiftCommand ApplyListShiftCommand(ListShiftCommand shiftCommandToApply) =>
        elementShiftRegistry.ApplyListShiftCommand(shiftCommandToApply);

    public ListShiftCommand ClearAll() => elementShiftRegistry.ClearAll();

    public ListShiftCommand ShiftElements(int byElements) => elementShiftRegistry.ShiftElements(byElements);

    public ListShiftCommand MoveToStart(IMutableMarketTradingStateEvent existingItem) => elementShiftRegistry.MoveToStart(existingItem);

    public ListShiftCommand MoveToStart(int indexToMoveToStart) => elementShiftRegistry.MoveToStart(indexToMoveToStart);

    public ListShiftCommand MoveToEnd(int indexToMoveToEnd) => elementShiftRegistry.MoveToEnd(indexToMoveToEnd);

    public ListShiftCommand MoveSingleElementBy(int indexToMoveToEnd, int shift) => elementShiftRegistry.MoveSingleElementBy(indexToMoveToEnd, shift);

    public ListShiftCommand MoveSingleElementBy(IMutableMarketTradingStateEvent existingItem, int shift) =>
        elementShiftRegistry.MoveSingleElementBy(existingItem, shift);

    public ListShiftCommand MoveToEnd(IMutableMarketTradingStateEvent existingItem) => elementShiftRegistry.MoveToEnd(existingItem);

    public ListShiftCommand ShiftElementsFrom(int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry.ShiftElementsFrom(byElements, pinElementsFromIndex);

    public ListShiftCommand ShiftElementsUntil(int byElements, int pinElementsFromIndex) =>
        elementShiftRegistry.ShiftElementsUntil(byElements, pinElementsFromIndex);

    public bool CalculateShift(DateTime asAtTime, IReadOnlyList<IMarketTradingStateEvent> updatedCollection) => 
        elementShiftRegistry.CalculateShift(asAtTime, updatedCollection);

    public IReadOnlyList<IMarketTradingStateEvent> TradingStateEvents => TradingEventsList.Take(Count).ToList().AsReadOnly();

    public int IndexOf(IMutableMarketTradingStateEvent item) => TradingEventsList.IndexOf(item);

    public bool Contains(IMutableMarketTradingStateEvent item) => TradingEventsList.Contains(item);

    public void CopyTo(IMutableMarketTradingStateEvent[] array, int arrayIndex) => TradingEventsList.CopyTo(array, arrayIndex);

    public virtual void Insert(int index, IMutableMarketTradingStateEvent item) => TradingEventsList.Insert(index, item);

    public virtual bool Remove(IMutableMarketTradingStateEvent toRemove) => TradingEventsList.Remove(toRemove);

    public virtual void RemoveAt(int index) => TradingEventsList.RemoveAt(index);

    public virtual void Add(IMutableMarketTradingStateEvent newMarketTradingState)
    {
        var nonEmptyCount = Count;
        if (TradingEventsList.Count == nonEmptyCount)
            TradingEventsList.Add(newMarketTradingState);
        else
            TradingEventsList[nonEmptyCount] = newMarketTradingState;
    }

    public virtual void Clear()
    {
        foreach (var tradingState in TradingEventsList)
        {
            tradingState.ResetWithTracking();
        }
    }

    public int AppendEntryAtEnd()
    {
        var index = TradingEventsList.Count;
        TradingEventsList.Add(NewElementFactory());
        return index;
    }

    IMutableMarketTradingStateList ITrackableReset<IMutableMarketTradingStateList>.ResetWithTracking() => ResetWithTracking();

    IMutableMarketTradingStateList IMutableMarketTradingStateList.ResetWithTracking() => ResetWithTracking();


    ITracksResetCappedCapacityList<IMutableMarketTradingStateEvent> ITrackableReset<ITracksResetCappedCapacityList<IMutableMarketTradingStateEvent>>.
        ResetWithTracking() => ResetWithTracking();

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        elementShiftRegistry.IsEmpty = true;
    }

    public virtual MarketTradingStateList ResetWithTracking()
    {
        foreach (var tradingState in TradingEventsList)
        {
            tradingState.ResetWithTracking();
        }
        return this;
    }

    public override void StateReset()
    {
        TradingEventsList.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<IMarketTradingStateEvent> IEnumerable<IMarketTradingStateEvent>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMutableMarketTradingStateEvent> IMutableMarketTradingStateList.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMutableMarketTradingStateEvent> GetEnumerator() => TradingEventsList.GetEnumerator();

    IMutableMarketTradingStateList IMutableMarketTradingStateList.Clone() => Clone();

    public override MarketTradingStateList Clone() => Recycler?.Borrow<MarketTradingStateList>().CopyFrom(this) ?? new MarketTradingStateList(this);


    public override MarketTradingStateList CopyFrom(IMarketTradingStateList source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        elementShiftRegistry.CopyFrom(source, copyMergeFlags);
        var currentDeepestLayerSet = Count;
        var sourceDeepestLayerSet  = source.Count;
        for (var i = 0; i < sourceDeepestLayerSet; i++)
        {
            var sourceLayerToCopy = source[i];
            if (i < TradingEventsList.Count)
            {
                if (!(TradingEventsList[i] is { } mutableTradingState))
                    TradingEventsList[i] = Convert(source[i], true);
                else if (sourceLayerToCopy.IsEmpty == false)
                    mutableTradingState.CopyFrom(source[i]);
                else
                    TradingEventsList[i].IsEmpty = true;
            }
            else
            {
                TradingEventsList.Add(Convert(source[i], true));
            }
        }

        for (var i = Math.Min(currentDeepestLayerSet, TradingEventsList.Count) - 1; i >= sourceDeepestLayerSet; i--)
            if (TradingEventsList[i] is { } mutableTradingState)
                mutableTradingState.IsEmpty = true;
        return this;
    }

    public virtual bool AreEquivalent(IMarketTradingStateList? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var countSame = Count == other.Count;
        if (!countSame) return false;

        var allTradingStateSame = true;
        for (int i = 0; i < Count && allTradingStateSame; i++)
        {
            var localLastTrade = this[i];
            var otherLastTrade = other[i];
            allTradingStateSame &= localLastTrade.AreEquivalent(otherLastTrade, exactTypes);
        }
        return countSame && allTradingStateSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMutableMarketTradingStateList, true);

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        foreach (var tradingState in TradingEventsList)
        {
            hashCode.Add(tradingState);
        }
        return hashCode.ToHashCode();
    }

    public string EachTradingStateByIndexOnNewLines()
    {
        var count = Count;
        var sb    = new StringBuilder(100 * count);
        for (var i = 0; i < count; i++)
        {
            var tradingState = TradingEventsList[i];
            sb.Append("\t\tTradingStateEvent[").Append(i).Append("] = ").Append(tradingState);
            if (i < count - 1)
            {
                sb.AppendLine(",");
            }
        }
        return sb.ToString();
    }

    protected string TradingStateListToStringMembers => $"{nameof(Count)}: {Count}, {nameof(MaxAllowedSize)}: {MaxAllowedSize:N0}";

    protected string TradingStateEventListToString => $"{nameof(TradingStateEvents)}: [\n{EachTradingStateByIndexOnNewLines()}]";

    public override string ToString() => $"{nameof(MarketTradingStateList)}{{{TradingStateListToStringMembers}, {TradingStateEventListToString}}}";
}
