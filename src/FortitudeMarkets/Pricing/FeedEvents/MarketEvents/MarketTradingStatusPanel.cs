// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public class MarketTradingStatusPanel : ReusableObject<IMarketTradingStatusPanel>, IMutableMarketTradingStatusPanel
{
    protected static readonly TimeSpan DefaultRetainEndTimeSpan = TimeSpan.FromHours(1);

    protected static Func<IMarketTradingStateEvent, IMarketTradingStateEvent, bool> SameEventSequenceId = 
        (lhs, rhs) => lhs.EventSequenceId == rhs.EventSequenceId;

    private readonly List<ElementLifeCycleChange> onTickLifeCycleChanges = new();

    private readonly List<int> tempCalcFoundInOther = new(); // new location index
    private readonly List<int> tempCalcNewInOther   = new(); // new location index

    public MarketTradingStatusPanel()
    {
        CurrentlyActive   = new MarketTradingStateList();
        RecentEndedEvents = new MarketTradingStateList();
        UpcomingEvents    = new MarketTradingStateList();
    }

    public MarketTradingStatusPanel
    (IMarketTradingStateList currentlyActive, IMarketTradingStateList recentMarketTradingStateEvents
      , IMarketTradingStateList upcomingMarketTradingStateEvents)
    {
        UpcomingEvents    = Convert(upcomingMarketTradingStateEvents);
        CurrentlyActive   = Convert(currentlyActive);
        RecentEndedEvents = Convert(recentMarketTradingStateEvents);
    }

    public MarketTradingStatusPanel(IMarketTradingStatusPanel toClone)
    {
        UpcomingEvents    = Convert(toClone.UpcomingEvents);
        CurrentlyActive   = Convert(toClone.CurrentlyActive);
        RecentEndedEvents = Convert(toClone.RecentEndedEvents);
    }

    protected IMutableMarketTradingStateList Convert(IMarketTradingStateList marketTradingStateList, bool clone = false)
    {
        if (clone || marketTradingStateList is not MarketTradingStateList tradingList) return new MarketTradingStateList(marketTradingStateList);
        return tradingList;
    }

    IMarketTradingStateList IMarketTradingStatusPanel.UpcomingEvents    => UpcomingEvents;
    IMarketTradingStateList IMarketTradingStatusPanel.CurrentlyActive   => CurrentlyActive;
    IMarketTradingStateList IMarketTradingStatusPanel.RecentEndedEvents => RecentEndedEvents;

    public IMutableMarketTradingStateList UpcomingEvents    { get; set; }
    public IMutableMarketTradingStateList CurrentlyActive   { get; set; }
    public IMutableMarketTradingStateList RecentEndedEvents { get; set; }


    public event EventUpdateHandler<IMarketTradingStateEvent>? AllLifeCycleChanges;

    public event EventUpdateHandler<IMarketTradingStateEvent>? NewlyPendingOnTick;

    public event EventUpdateHandler<IMarketTradingStateEvent>? NewlyActiveOnTick;

    public event EventUpdateHandler<IMarketTradingStateEvent>? NewlyEndedOnTick;

    public event EventUpdateHandler<IMarketTradingStateEvent>? AllUpdatesOnTick;

    public void TriggerOnTickEvents()
    {
        foreach (var tradingStateOnTickChange in onTickLifeCycleChanges)
        {
            switch (tradingStateOnTickChange.LifeCycleState.JustLifecycleState())
            {
                case EventStateLifecycle.Pending:
                    if (tradingStateOnTickChange.LifeCycleState.HasNewOnTickFlag())
                    {
                        NewlyPendingOnTick?.Invoke(UpcomingEvents[tradingStateOnTickChange.UpdateIndexLocation]
                                                 , tradingStateOnTickChange.LifeCycleState);
                        AllLifeCycleChanges?.Invoke(UpcomingEvents[tradingStateOnTickChange.UpdateIndexLocation]
                                                  , tradingStateOnTickChange.LifeCycleState);
                    }
                    else
                    {
                        AllUpdatesOnTick?.Invoke(UpcomingEvents[tradingStateOnTickChange.UpdateIndexLocation]
                                               , tradingStateOnTickChange.LifeCycleState);
                    }
                    break;
                case EventStateLifecycle.Active:
                    if (tradingStateOnTickChange.LifeCycleState.HasNewOnTickFlag())
                    {
                        NewlyActiveOnTick?.Invoke(CurrentlyActive[tradingStateOnTickChange.UpdateIndexLocation]
                                                , tradingStateOnTickChange.LifeCycleState);
                        AllLifeCycleChanges?.Invoke(CurrentlyActive[tradingStateOnTickChange.UpdateIndexLocation]
                                                  , tradingStateOnTickChange.LifeCycleState);
                    }
                    else
                    {
                        AllUpdatesOnTick?.Invoke(CurrentlyActive[tradingStateOnTickChange.UpdateIndexLocation]
                                               , tradingStateOnTickChange.LifeCycleState);
                    }
                    break;
                case EventStateLifecycle.DeadNeverActive:
                case EventStateLifecycle.Ended:
                    if (tradingStateOnTickChange.LifeCycleState.HasNewOnTickFlag())
                    {
                        NewlyEndedOnTick?.Invoke(RecentEndedEvents[tradingStateOnTickChange.UpdateIndexLocation]
                                               , tradingStateOnTickChange.LifeCycleState);
                        AllLifeCycleChanges?.Invoke(RecentEndedEvents[tradingStateOnTickChange.UpdateIndexLocation]
                                                  , tradingStateOnTickChange.LifeCycleState);
                    }
                    else
                    {
                        AllUpdatesOnTick?.Invoke(RecentEndedEvents[tradingStateOnTickChange.UpdateIndexLocation]
                                               , tradingStateOnTickChange.LifeCycleState);
                    }
                    break;
            }
        }
    }

    public bool IsEmpty
    {
        get =>
            UpcomingEvents.IsEmpty
         && CurrentlyActive.IsEmpty
         && RecentEndedEvents.IsEmpty;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    public IMutableMarketTradingStatusPanel ResetWithTracking()
    {
        UpcomingEvents.ResetWithTracking();
        CurrentlyActive.ResetWithTracking();
        RecentEndedEvents.ResetWithTracking();

        return this;
    }

    public void UpdateComplete(uint updateSequenceId = 0)
    {
        onTickLifeCycleChanges.Clear();
        UpcomingEvents.UpdateComplete(updateSequenceId);
        CurrentlyActive.UpdateComplete(updateSequenceId);
        RecentEndedEvents.UpdateComplete(updateSequenceId);
    }

    public IEnumerable<ElementLifeCycleChange<IMarketTradingStateEvent>> CalculateOnTickUpdates
        (IFiresOnTickLifeCycleChanges<IMarketTradingStateEvent> nextUpdate)
    {
        if (nextUpdate is IMarketTradingStatusPanel nextTradingStatusPanel)
        {
            CalculateOnTickChanges(nextTradingStatusPanel);
            return
                onTickLifeCycleChanges
                    .Select(elcc =>
                                new ElementLifeCycleChange<IMarketTradingStateEvent>
                                    (elcc.LifeCycleState.JustLifecycleState() switch
                                     {
                                         EventStateLifecycle.Pending         => nextTradingStatusPanel.UpcomingEvents[elcc.UpdateIndexLocation]
                                       , EventStateLifecycle.Active          => nextTradingStatusPanel.CurrentlyActive[elcc.UpdateIndexLocation]
                                       , EventStateLifecycle.Ended           => nextTradingStatusPanel.RecentEndedEvents[elcc.UpdateIndexLocation]
                                       , EventStateLifecycle.DeadNeverActive => nextTradingStatusPanel.RecentEndedEvents[elcc.UpdateIndexLocation]
                                       , _ => throw new
                                             ArgumentException($"Did not expect to get EventStateLifeCycle {elcc.LifeCycleState.JustLifecycleState()}")
                                     }, elcc.LifeCycleState));
        }
        return [];
    }

    public int EnsureIsCurrentlyPending(IMutableMarketTradingStateEvent ensureToEnsureIsCurrentlyPending)
    {
        CheckRemoveInTradingStateList(ensureToEnsureIsCurrentlyPending, CurrentlyActive);
        CheckRemoveInTradingStateList(ensureToEnsureIsCurrentlyPending, RecentEndedEvents);
        return UpsertInTradingStateList(ensureToEnsureIsCurrentlyPending, UpcomingEvents);
    }

    public int EnsureIsCurrentlyActive(IMutableMarketTradingStateEvent ensureToEnsureIsCurrentlyActive)
    {
        CheckRemoveInTradingStateList(ensureToEnsureIsCurrentlyActive, UpcomingEvents);
        CheckRemoveInTradingStateList(ensureToEnsureIsCurrentlyActive, RecentEndedEvents);
        return UpsertInTradingStateList(ensureToEnsureIsCurrentlyActive, CurrentlyActive);
    }

    public int EnsureIsCurrentlyEnded(IMutableMarketTradingStateEvent ensureToEnsureIsCurrentlyActive, DateTime? earlyTerminationTime = null) 
    {
        CheckRemoveInTradingStateList(ensureToEnsureIsCurrentlyActive, UpcomingEvents);
        CheckRemoveInTradingStateList(ensureToEnsureIsCurrentlyActive, CurrentlyActive);
        var autoExpireTime = ensureToEnsureIsCurrentlyActive.StartTime.AddMilliseconds(ensureToEnsureIsCurrentlyActive.EstimatedLengthMs);
        ensureToEnsureIsCurrentlyActive.EndedAt = earlyTerminationTime < autoExpireTime ? earlyTerminationTime.Value : autoExpireTime;
        return UpsertInTradingStateList(ensureToEnsureIsCurrentlyActive, RecentEndedEvents);
    }

    public void CheckStartTimeAndExpiryTransitionThroughLifecycle(DateTime asAtTime)
    {
        // Reverse order so events are at least published once in each transition state
        RemovedEndedOlderThan(asAtTime, DefaultRetainEndTimeSpan);
        for (var i = 0; i < CurrentlyActive.Count; i++)
        {
            var activeEvent     = CurrentlyActive[i];
            var autoExpireTime = activeEvent.StartTime.AddMilliseconds(activeEvent.EstimatedLengthMs);
            if (autoExpireTime < asAtTime)
            {
                CurrentlyActive.DeleteAt(i);
                activeEvent.EndedAt = autoExpireTime;
                RecentEndedEvents.Add(activeEvent);
                i--;
            }
        }
        for (var i = 0; i < UpcomingEvents.Count; i++)
        {
            var upcomingEvent = UpcomingEvents[i];
            if (upcomingEvent.StartTime < asAtTime)
            {
                UpcomingEvents.DeleteAt(i);
                CurrentlyActive.Add(upcomingEvent);
                i--;
            }
        }
    }

    public int RemovedEndedOlderThan(DateTime asAtTime, TimeSpan timeSpanInEndedState)
    {
        var countRemoved = 0;
        for (var i = 0; i < RecentEndedEvents.Count; i++)
        {
            var recentEndedEvent = RecentEndedEvents[i];
            var endedPlusCache   = recentEndedEvent.EndedAt!.Value.Add(timeSpanInEndedState);
            if (endedPlusCache < asAtTime)
            {
                RecentEndedEvents.DeleteAt(i);
                countRemoved++;
                i--;
            }
        }
        return countRemoved;
    }

    private void CalculateOnTickChanges(IMarketTradingStatusPanel source)
    {
        bool wasEmpty = IsEmpty;
        onTickLifeCycleChanges.Clear();
        CalculateUpcomingChanges(source, wasEmpty);
        CalculateActiveChanges(source, wasEmpty);
        CalculateEndedChanges(source);
    }

    private void CalculateUpcomingChanges(IMarketTradingStatusPanel source, bool wasEmpty)
    {
        var newPending = NewInOther(UpcomingEvents, source.UpcomingEvents);
        for (var i = 0; i < newPending.Count; i++)
        {
            var newPendingIndex = newPending[i];
            onTickLifeCycleChanges.Add
                (new ElementLifeCycleChange
                    (newPendingIndex, EventStateLifecycle.Pending | EventStateLifecycle.NewOnTick));
        }
        var updatedPending = FoundInOther(UpcomingEvents, source.UpcomingEvents, true);
        for (var i = 0; i < updatedPending.Count; i++)
        {
            var newPendingIndex = updatedPending[i];
            onTickLifeCycleChanges.Add
                (new ElementLifeCycleChange
                    (newPendingIndex, EventStateLifecycle.Pending | EventStateLifecycle.UpdatedOnTick));
        }
        var allNewlyActive = FoundInOther(UpcomingEvents, source.CurrentlyActive);
        for (int i = 0; i < allNewlyActive.Count && !wasEmpty; i++)
        {
            var newlyActiveIndex = allNewlyActive[i];
            if (IsFoundIn(source.CurrentlyActive[newlyActiveIndex], CurrentlyActive)) continue;
            onTickLifeCycleChanges.Add
                (new ElementLifeCycleChange
                    (newlyActiveIndex, EventStateLifecycle.Active | EventStateLifecycle.NewOnTick));
        }
        var allNewlyDeadNotRun = FoundInOther(UpcomingEvents, source.RecentEndedEvents);
        for (int i = 0; i < allNewlyDeadNotRun.Count && !wasEmpty; i++)
        {
            var newlyEndedIndex = allNewlyDeadNotRun[i];
            if (IsFoundIn(source.RecentEndedEvents[newlyEndedIndex], RecentEndedEvents)) continue;
            onTickLifeCycleChanges.Add
                (new ElementLifeCycleChange
                    (newlyEndedIndex, EventStateLifecycle.DeadNeverActive | EventStateLifecycle.NewOnTick));
        }
    }

    private void CalculateActiveChanges(IMarketTradingStatusPanel source, bool wasEmpty)
    {
        var allNewlyEnded = NewInOther(CurrentlyActive, source.RecentEndedEvents);
        for (var i = 0; i < allNewlyEnded.Count && !wasEmpty; i++)
        {
            var newEndedIndex = allNewlyEnded[i];
            if (IsFoundIn(source.RecentEndedEvents[newEndedIndex], RecentEndedEvents)) continue;
            onTickLifeCycleChanges.Add
                (new ElementLifeCycleChange
                    (newEndedIndex, EventStateLifecycle.Ended | EventStateLifecycle.NewOnTick));
        }
        for (var i = 0; i < source.CurrentlyActive.Count && !wasEmpty; i++)
        {
            var checkStraightToActive = source.CurrentlyActive[i];
            if (!IsFoundIn(checkStraightToActive, RecentEndedEvents) && !IsFoundIn(checkStraightToActive, CurrentlyActive))
            {
                onTickLifeCycleChanges.Add(new ElementLifeCycleChange
                                               (i, EventStateLifecycle.Ended | EventStateLifecycle.NewOnTick));
            }
        }
        var allUpdatedActive = FoundInOther(CurrentlyActive, source.CurrentlyActive, true);
        for (var i = 0; i < allUpdatedActive.Count; i++)
        {
            var updatedActiveIndex = allUpdatedActive[i];
            onTickLifeCycleChanges.Add(new ElementLifeCycleChange
                                           (updatedActiveIndex, EventStateLifecycle.Active | EventStateLifecycle.UpdatedOnTick));
        }
    }

    private void CalculateEndedChanges(IMarketTradingStatusPanel source)
    {
        var allUpdatedEnded = FoundInOther(RecentEndedEvents, source.RecentEndedEvents, true);
        for (var i = 0; i < allUpdatedEnded.Count; i++)
        {
            var updatedActive = allUpdatedEnded[i];
            onTickLifeCycleChanges.Add
                (new ElementLifeCycleChange(updatedActive, EventStateLifecycle.Ended | EventStateLifecycle.UpdatedOnTick));
        }
    }

    private List<int> NewInOther(IMarketTradingStateList fromOld, IMarketTradingStateList toNew)
    {
        tempCalcNewInOther.Clear();
        for (var i = 0; i < toNew.Count; i++)
        {
            var updatedUpcomingEvent = toNew[i];
            var wasFound             = IsFoundIn(updatedUpcomingEvent, fromOld);
            if (!wasFound)
            {
                tempCalcNewInOther.Add(i);
            }
        }
        return tempCalcNewInOther;
    }

    private static bool IsFoundIn(IMarketTradingStateEvent checkItem, IReadOnlyList<IMarketTradingStateEvent> isInThisList)
    {
        return IndexOfEventSequenceId(checkItem, isInThisList) != -1;
    }

    private static int IndexOfEventSequenceId(IMarketTradingStateEvent checkItem, IReadOnlyList<IMarketTradingStateEvent> isInThisList)
    {
        for (var j = 0; j < isInThisList.Count; j++)
        {
            var preExisting = isInThisList[j];
            if (SameEventSequenceId(checkItem, preExisting))
            {
                return j;
            }
        }
        return -1;
    }

    private List<int> FoundInOther(IMarketTradingStateList updateDestination, IMarketTradingStateList updateSource, bool onlyAddIfNotEquals = false)
    {
        tempCalcFoundInOther.Clear();
        for (var i = 0; i < updateDestination.Count; i++)
        {
            var oldTradingState = updateDestination[i];
            foreach (var updatedTradingState in updateSource)
            {
                if (SameEventSequenceId(oldTradingState, updatedTradingState) &&
                    (!onlyAddIfNotEquals || !Equals(oldTradingState, updatedTradingState)))
                {
                    tempCalcFoundInOther.Add(i);
                }
            }
        }
        return tempCalcFoundInOther;
    }

    private void CheckRemoveInTradingStateList(IMutableMarketTradingStateEvent checkIsRemoved, IMutableMarketTradingStateList fromThisList)
    {
        var checkIndex = IndexOfEventSequenceId(checkIsRemoved, fromThisList);
        if (checkIndex >= 0)
        {
            var checkInstance = fromThisList[checkIndex];
            if (ReferenceEquals(checkIsRemoved, checkInstance))
            {
                fromThisList.RemoveAt(checkIndex);
            }
            else
            {
                fromThisList.DeleteAt(checkIndex);
            }
        }
    }

    private int UpsertInTradingStateList(IMutableMarketTradingStateEvent addOrUpdate, IMutableMarketTradingStateList inThisList)
    {
        var checkIndex = IndexOfEventSequenceId(addOrUpdate, inThisList);
        if (checkIndex >= 0)
        {
            var checkInstance = inThisList[checkIndex];
            if (!ReferenceEquals(addOrUpdate, checkInstance))
            {
                inThisList.RemoveAt(checkIndex);
                checkInstance.StateReset();
                inThisList.Add(checkInstance);
                inThisList.Insert(checkIndex, addOrUpdate);
            }
            return checkIndex;
        }
        else
        {
            var count = inThisList.Count;
            inThisList.Add(addOrUpdate);
            return count;
        }
    }

    public override IMarketTradingStatusPanel Clone() =>
        Recycler?.Borrow<MarketTradingStatusPanel>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new MarketTradingStatusPanel(this);

    public override MarketTradingStatusPanel CopyFrom(IMarketTradingStatusPanel source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CalculateOnTickChanges(source);
        
        UpcomingEvents.CopyFrom(source.UpcomingEvents, copyMergeFlags);
        CurrentlyActive.CopyFrom(source.CurrentlyActive, copyMergeFlags);
        RecentEndedEvents.CopyFrom(source.RecentEndedEvents, copyMergeFlags);

        return this;
    }

    public bool AreEquivalent(IMarketTradingStatusPanel? other, bool exactTypes = false)
    {
        if (other == null) return false;
        
        var upcomingSame = UpcomingEvents.AreEquivalent(other.UpcomingEvents, exactTypes);
        var activeSame   = CurrentlyActive.AreEquivalent(other.CurrentlyActive, exactTypes);
        var recentSame   = RecentEndedEvents.AreEquivalent(other.RecentEndedEvents, exactTypes);

        var allAreSame = activeSame && recentSame && upcomingSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMarketTradingStatusPanel, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = UpcomingEvents.GetHashCode();
            hashCode = (hashCode * 397) ^ CurrentlyActive.GetHashCode();
            hashCode = (hashCode * 397) ^ RecentEndedEvents.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(MarketTradingStatusPanel)}{{{nameof(UpcomingEvents)}: {UpcomingEvents}, {nameof(CurrentlyActive)}: {CurrentlyActive}, " +
        $"{nameof(RecentEndedEvents)}: {RecentEndedEvents}}}";
}
