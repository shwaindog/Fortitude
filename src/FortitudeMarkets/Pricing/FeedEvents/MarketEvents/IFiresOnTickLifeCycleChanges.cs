using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public delegate void EventUpdateHandler<T>(T updatedItem, EventStateLifecycle eventType);

[Flags]
public enum EventStateLifecycle : ushort
{
    None            = 0
  , Pending         = 0x00_01
  , Active          = 0x00_02
  , Ended           = 0x00_04
  , DeadNeverActive = 0x00_08

  , StateMask       = 0x00_FF 

  , NewOnTick     = 0x10_00
  , UpdatedOnTick = 0x20_00
}

public static class EventStateLifecycleExtensions
{
    public static EventStateLifecycle JustLifecycleState(this EventStateLifecycle flags) => flags & EventStateLifecycle.StateMask;

    public static bool HasPendingFlag(this EventStateLifecycle flags)         => (flags & EventStateLifecycle.Pending) > 0;
    public static bool IsActive(this EventStateLifecycle flags)               => (flags & EventStateLifecycle.Active) > 0;
    public static bool HasEndedFlag(this EventStateLifecycle flags)           => (flags & EventStateLifecycle.Ended) > 0;
    public static bool HasDeadNeverActiveFlag(this EventStateLifecycle flags) => (flags & EventStateLifecycle.DeadNeverActive) > 0;
    public static bool IsEndedOrDead(this EventStateLifecycle flags)          => flags.HasDeadNeverActiveFlag() || flags.HasEndedFlag();

    public static bool HasNewOnTickFlag(this EventStateLifecycle flags) => (flags & EventStateLifecycle.NewOnTick) > 0;

    public static bool HasUpdatedOnTickFlag(this EventStateLifecycle flags) => (flags & EventStateLifecycle.UpdatedOnTick) > 0;
}


public struct ElementLifeCycleChange(int updateIndexLocation, EventStateLifecycle lifecycle)
{
    public readonly int UpdateIndexLocation = updateIndexLocation;

    public readonly EventStateLifecycle LifeCycleState = lifecycle;
}

public struct ElementLifeCycleChange<T>(T element, EventStateLifecycle lifecycle)
{
    public readonly T Element = element;

    public readonly EventStateLifecycle LifeCycleState = lifecycle;
}


public interface IFiresOnTickEvents
{
    void TriggerOnTickEvents();
}

public interface IFiresOnTickLifeCycleChanges<T> : IFiresOnTickEvents
{
    IEnumerable<ElementLifeCycleChange<T>> CalculateOnTickUpdates(IFiresOnTickLifeCycleChanges<T> nextUpdate);


    event EventUpdateHandler<T> AllLifeCycleChanges;

    event EventUpdateHandler<T> NewlyPendingOnTick;

    event EventUpdateHandler<T> NewlyActiveOnTick;

    event EventUpdateHandler<T> NewlyEndedOnTick;

    event EventUpdateHandler<T> AllUpdatesOnTick;
}

public interface IAlertLifeCycleChanges<in T>
{
    void ReceiveEventLifeCycleChange(T updatedItem, EventStateLifecycle eventType);
}

public interface IListensToLifeCycleChanges<T> : IAlertLifeCycleChanges<T>
{
    void SubscribeToUpdates(IFiresOnTickLifeCycleChanges<T> eventSource);
}