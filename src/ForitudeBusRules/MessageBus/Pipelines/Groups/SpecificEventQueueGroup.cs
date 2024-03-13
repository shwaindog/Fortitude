#region

using System.Collections;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Pipelines.Groups;

public interface IEventQueueGroup : IEnumerable<IEventQueue>
{
    int Count { get; }
    bool HasStarted { get; }
    IEventQueue? this[int index] { get; set; }
    bool Contains(IEventQueue item);
    int AddNewQueues(DeploymentOptions deploymentOptions);
    void Add(IEventQueue toAdd);
    bool StopRemoveEventQueue(IEventQueue eventQueue);
    new IEnumerator<IEventQueue> GetEnumerator();
}

public class SpecificEventQueueGroup(IEventBus owningEventBus, EventQueueType groupType, IRecycler recycler
        , int defaultNewQueueSize = 10_000)
    : IEventQueueGroup
{
    private readonly List<IEventQueue?> eventQueues = [];

    public IRecycler Recycler { get; set; } = recycler;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IEventQueue> GetEnumerator()
    {
        var freshEnumerator = Recycler.Borrow<AutoRecycleEnumerator<IEventQueue>>();
        for (var i = 0; i < eventQueues.Count; i++)
        {
            var checkQueue = eventQueues[i];
            if (checkQueue != null) freshEnumerator.Add(checkQueue);
        }

        return freshEnumerator;
    }

    public int Count => eventQueues.Count;

    public bool HasStarted => eventQueues.Any(eq => eq?.IsRunning ?? false);

    public IEventQueue? this[int index]
    {
        get => eventQueues[index];
        set
        {
            if (value != null && value.QueueType != groupType)
                throw new InvalidOperationException(
                    $"Can not add EventQueue of type {value.QueueType} to EventQueueGroup of type {groupType}");

            if (index < eventQueues.Count)
            {
                if (value != null || eventQueues[index] != null)
                    throw new ArgumentException(
                        "You can only append new queues to the end of an EventQueueGroup or replace a stopped queue.");

                var existingValue = eventQueues[index];
                if (value == null && existingValue is { IsRunning: true }) existingValue.Shutdown();
            }

            eventQueues.Add(value);
        }
    }

    public int AddNewQueues(DeploymentOptions deploymentOptions)
    {
        var existingCount = eventQueues.Count;
        for (var i = existingCount; i < existingCount + deploymentOptions.Instances; i++)
            eventQueues.Add(new EventQueue(owningEventBus, groupType, i, defaultNewQueueSize));
        return eventQueues.Count;
    }

    public void Add(IEventQueue item)
    {
        if (item.QueueType != groupType)
            throw new InvalidOperationException(
                $"Can not add EventQueue of type {item.QueueType} to EventQueueGroup of type {groupType}");
        eventQueues.Add(item);
    }

    public bool StopRemoveEventQueue(IEventQueue item)
    {
        var indexOf = eventQueues.IndexOf(item);
        if (indexOf < 0) return false;
        if (item.IsRunning) item.Shutdown();
        eventQueues[indexOf] = null;
        return true;
    }

    public bool Contains(IEventQueue item) => eventQueues.Contains(item);
}
