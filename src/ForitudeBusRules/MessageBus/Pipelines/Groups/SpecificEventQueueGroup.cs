#region

using System.Collections;
using FortitudeBusRules.Config;
using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeIO.Transports.Network.Receiving;

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

public class SpecificEventQueueGroup : IEventQueueGroup
{
    private readonly List<IEventQueue?> eventQueues = [];
    
    protected readonly IQueuesConfig QueuesConfig;
    private readonly IConfigureEventBus owningEventBus;
    private readonly EventQueueType groupType;
    public SpecificEventQueueGroup(IConfigureEventBus owningEventBus, EventQueueType groupType, IRecycler recycler
        , IQueuesConfig queuesConfig)
    {
        this.owningEventBus = owningEventBus;
        this.groupType = groupType;
        QueuesConfig = queuesConfig;
        Recycler = recycler;
    }

    public IRecycler Recycler { get; set; }

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
        var defaultNewQueueSize = groupType == EventQueueType.Event ? QueuesConfig.EventQueueSize : QueuesConfig.DefaultQueueSize;
        for (var i = existingCount; i < existingCount + deploymentOptions.Instances; i++)
        {
            
            var name = $"{groupType}-{i}";

            eventQueues.Add(new EventQueue(owningEventBus, groupType, i, CreateMessageRingPoller(name, i, defaultNewQueueSize, QueuesConfig.MessagePumpMaxWaitMs)));
        }

        return eventQueues.Count;
    }

    public virtual IAsyncValueTaskRingPoller<Message> CreateMessageRingPoller(string name, int id, int size, uint noDataPauseTimeoutMs)
    {
        var ring = new AsyncValueValueTaskPollingRing<Message>(
            $"EventQueue-{name}",
            size,
            () => new Message(),
            ClaimStrategyType.MultiProducers, null, null, false);
        return new AsyncValueTaskRingPoller<Message>(ring, noDataPauseTimeoutMs);
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

class SocketEventQueueListenerGroup : SpecificEventQueueGroup
{
    public static IUpdateableTimer timer = new UpdateableTimer("Fortitude.BusRules.SocketEventQueueListenerGroup.ThreadPoolTimer");
    public SocketEventQueueListenerGroup(IConfigureEventBus owningEventBus, EventQueueType groupType, IRecycler recycler
        , IQueuesConfig queuesConfig) : base(owningEventBus, groupType, recycler, queuesConfig)
    {
    }

    public override IAsyncValueTaskRingPoller<Message> CreateMessageRingPoller(string name, int id, int size, uint noDataPauseTimeoutMs)
    {
        var ring = new AsyncValueValueTaskPollingRing<Message>(
            $"EventQueue-{name}",
            size,
            () => new Message(),
            ClaimStrategyType.MultiProducers, null, null, false);
        return new SocketAsyncValueTaskEventQueueListener(ring, noDataPauseTimeoutMs, new SocketSelector(QueuesConfig.SelectorPollIntervalMs), timer);
    }
}

public class SocketEventQueueSenderGroup(IConfigureEventBus owningEventBus, EventQueueType groupType, IRecycler recycler
    , IQueuesConfig queuesConfig) : SpecificEventQueueGroup(owningEventBus, groupType, recycler, queuesConfig)
{
    public override IAsyncValueTaskRingPoller<Message> CreateMessageRingPoller(string name, int id, int size, uint noDataPauseTimeoutMs)
    {
        var ring = new AsyncValueValueTaskPollingRing<Message>(
            $"EventQueue-{name}",
            size,
            () => new Message(),
            ClaimStrategyType.MultiProducers, null, null, false);
        return new SocketAsyncValueTaskEventQueueSender(ring, noDataPauseTimeoutMs);
    }
}
