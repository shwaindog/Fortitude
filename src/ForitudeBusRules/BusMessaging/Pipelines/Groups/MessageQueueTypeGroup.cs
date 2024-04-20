#region

using System.Collections;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.Config;
using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Groups;

public interface IMessageQueueTypeGroup<T> : IEnumerable<T> where T : IMessageQueue
{
    int Count { get; }
    bool HasStarted { get; }
    T? this[int index] { get; set; }
    bool Contains(T item);
    int AddNewQueues(DeploymentOptions deploymentOptions);
    void Add(T toAdd);
    bool StopRemoveEventQueue(T messageQueue);
    IMessageQueueList<T> AsMessageQueueList();
    new IEnumerator<T> GetEnumerator();
}

public interface IMessageQueueTypeGroup : IMessageQueueTypeGroup<IMessageQueue>{}

public abstract class MessageQueueTypeGroup<T> : IMessageQueueTypeGroup<T> where T : IMessageQueue
{
    private readonly List<T?> eventQueues = [];
    
    protected readonly IQueuesConfig QueuesConfig;
    private readonly IConfigureMessageBus owningMessageBus;
    private readonly MessageQueueType groupType;
    public MessageQueueTypeGroup(IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IRecycler recycler
        , IQueuesConfig queuesConfig)
    {
        this.owningMessageBus = owningMessageBus;
        this.groupType = groupType;
        QueuesConfig = queuesConfig;
        Recycler = recycler;
    }

    public IRecycler Recycler { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual IEnumerator<T> GetEnumerator()
    {
        var freshEnumerator = Recycler.Borrow<AutoRecycleEnumerator<T>>();
        for (var i = 0; i < eventQueues.Count; i++)
        {
            var checkQueue = eventQueues[i];
            if (checkQueue != null) freshEnumerator.Add(checkQueue);
        }

        return freshEnumerator;
    }

    public int Count => eventQueues.Count;

    public bool HasStarted => eventQueues.Any(eq => eq?.IsRunning ?? false);

    public T? this[int index]
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
        var defaultNewQueueSize = groupType == MessageQueueType.Event ? QueuesConfig.EventQueueSize : QueuesConfig.DefaultQueueSize;
        for (var i = existingCount; i < existingCount + deploymentOptions.Instances; i++)
        {
            
            var name = $"{groupType}-{i}";

            eventQueues.Add(CreateMessageQueue(owningMessageBus, groupType, i, name, defaultNewQueueSize, QueuesConfig.MessagePumpMaxWaitMs));
        }

        return eventQueues.Count;
    }

    public abstract T CreateMessageQueue(IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs);

    public void Add(T item)
    {
        if (item.QueueType != groupType)
            throw new InvalidOperationException(
                $"Can not add EventQueue of type {item.QueueType} to EventQueueGroup of type {groupType}");
        eventQueues.Add(item);
    }

    public bool StopRemoveEventQueue(T item)
    {
        var indexOf = eventQueues.IndexOf(item);
        if (indexOf < 0) return false;
        if (item.IsRunning) item.Shutdown();
        eventQueues[indexOf] = default;
        return true;
    }

    public IMessageQueueList<T> AsMessageQueueList()
    {
        var messageQueueList = Recycler.Borrow<MessageQueueList<T>>();
        messageQueueList.AddRange(this);
        return messageQueueList;
    }

    public bool Contains(T item) => eventQueues.Contains(item);
}

public class MessageQueueTypeGroup : MessageQueueTypeGroup<IMessageQueue>, IMessageQueueTypeGroup
{
    public MessageQueueTypeGroup(IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IRecycler recycler, IQueuesConfig queuesConfig) : base(owningMessageBus, groupType, recycler, queuesConfig) { }
    public override IMessageQueue CreateMessageQueue(IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs)
    {
        var ring = new AsyncValueValueTaskPollingRing<BusMessage>(
            $"MessageQueue-{name}",
            queueSize,
            () => new BusMessage(),
            ClaimStrategyType.MultiProducers, null, null, false);
        var ringPoller = new AsyncValueTaskRingPoller<BusMessage>(ring, noDataPauseTimeoutMs);
        return new MessageQueue(configureMessageBus, messageQueueType, id, ringPoller);
    }
}

public interface IIOInboundMessageTypeGroup : IMessageQueueTypeGroup<IIOInboundMessageQueue>, IMessageQueueTypeGroup
{
    new int Count { get; }
    new bool HasStarted { get; }
    new int AddNewQueues(DeploymentOptions deploymentOptions);
    new IEnumerator<IIOInboundMessageQueue> GetEnumerator();
    new IIOInboundMessageQueue? this[int index] { get; set; }
    new bool Contains(IIOInboundMessageQueue item);
    new bool StopRemoveEventQueue(IIOInboundMessageQueue messageQueue);
    new IMessageQueueList<IIOInboundMessageQueue> AsMessageQueueList();

}

class SocketListenerMessageQueueGroup : MessageQueueTypeGroup<IIOInboundMessageQueue>, IIOInboundMessageTypeGroup
{
    public static IUpdateableTimer timer = new UpdateableTimer("Fortitude.BusRules.SocketEventQueueListenerGroup.ThreadPoolTimer");
    public SocketListenerMessageQueueGroup(IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IRecycler recycler
        , IQueuesConfig queuesConfig) : base(owningMessageBus, groupType, recycler, queuesConfig)
    {
    }
    public override IIOInboundMessageQueue CreateMessageQueue(IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs)
    {
        var ring = new AsyncValueValueTaskPollingRing<BusMessage>(
            $"IOInboundMessageQueue-{name}",
            queueSize,
            () => new BusMessage(),
            ClaimStrategyType.MultiProducers, null, null, false);
        var ringPoller = new SocketAsyncValueTaskEventQueueListener(ring, noDataPauseTimeoutMs, new SocketSelector(QueuesConfig.SelectorPollIntervalMs), timer);
        return new IOInboundMessageQueue(configureMessageBus, messageQueueType, id, ringPoller);
    }

    IMessageQueue? IMessageQueueTypeGroup<IMessageQueue>.this[int index]
    {
        get => this[index];
        set => this[index] = value != null ? (IIOInboundMessageQueue)value : null;
    }

    public bool Contains(IMessageQueue item) => item is IIOInboundMessageQueue ioInboundMessageQueue && base.Contains(ioInboundMessageQueue);

    public void Add(IMessageQueue toAdd)
    {
        if (toAdd is IIOInboundMessageQueue ioInboundMessageQueue)
        {
            base.Add(ioInboundMessageQueue);
        }
    }

    public bool StopRemoveEventQueue(IMessageQueue messageQueue) => messageQueue is IIOInboundMessageQueue ioInboundMessageQueue && base.StopRemoveEventQueue(ioInboundMessageQueue);


    public new IMessageQueueList<IMessageQueue> AsMessageQueueList()
    {
        var messageQueueList = Recycler.Borrow<MessageQueueList<IMessageQueue>>();
        messageQueueList.AddRange(this);
        return messageQueueList;
    }
    
    IEnumerator<IMessageQueue> IEnumerable<IMessageQueue>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMessageQueue> IMessageQueueTypeGroup<IMessageQueue>.GetEnumerator() 
    {
        var freshEnumerator = Recycler.Borrow<AutoRecycleEnumerator<IMessageQueue>>();
        for (var i = 0; i < Count; i++)
        {
            var checkQueue = this[i];
            if (checkQueue != null) freshEnumerator.Add(checkQueue);
        }

        return freshEnumerator;
    }
}

public interface IIOOutboundMessageTypeGroup : IMessageQueueTypeGroup<IIOOutboundMessageQueue>, IMessageQueueTypeGroup
{
    new int Count { get; }
    new bool HasStarted { get; }
    new int AddNewQueues(DeploymentOptions deploymentOptions);
    new IEnumerator<IIOOutboundMessageQueue> GetEnumerator();
    new IIOOutboundMessageQueue? this[int index] { get; set; }
    new bool Contains(IIOOutboundMessageQueue item);
    new bool StopRemoveEventQueue(IIOOutboundMessageQueue messageQueue);
    new IMessageQueueList<IIOOutboundMessageQueue> AsMessageQueueList();

}

public class SocketSenderMessageQueueGroup(IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IRecycler recycler
    , IQueuesConfig queuesConfig) : MessageQueueTypeGroup<IIOOutboundMessageQueue>(owningMessageBus, groupType, recycler, queuesConfig), IIOOutboundMessageTypeGroup
{
    public override IIOOutboundMessageQueue CreateMessageQueue(IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs)
    {
        var ring = new AsyncValueValueTaskPollingRing<BusMessage>(
            $"IOOutboundMessageQueue-{name}",
            queueSize,
            () => new BusMessage(),
            ClaimStrategyType.MultiProducers, null, null, false);
        var ringPoller = new SocketAsyncValueTaskEventQueueSender(ring, noDataPauseTimeoutMs);
        return new IOOutboundMessageQueue(configureMessageBus, messageQueueType, id, ringPoller);
    }

    IEnumerator<IMessageQueue> IEnumerable<IMessageQueue>.GetEnumerator() => GetEnumerator();

    IMessageQueue? IMessageQueueTypeGroup<IMessageQueue>.this[int index]
    {
        get => this[index];
        set => this[index] = value != null ? (IIOOutboundMessageQueue)value : null;
    }

    public bool Contains(IMessageQueue item) => item is IIOOutboundMessageQueue ioOutboundMessageQueue && base.Contains(ioOutboundMessageQueue);

    public void Add(IMessageQueue toAdd)
    {
        if (toAdd is IIOOutboundMessageQueue ioOutboundMessageQueue)
        {
            base.Add(ioOutboundMessageQueue);
        }
    }

    public bool StopRemoveEventQueue(IMessageQueue messageQueue) => messageQueue is IIOOutboundMessageQueue ioOutboundMessageQueue && base.StopRemoveEventQueue(ioOutboundMessageQueue);
    
    IMessageQueueList<IMessageQueue> IMessageQueueTypeGroup<IMessageQueue>.AsMessageQueueList()
    {
        var messageQueueList = Recycler.Borrow<MessageQueueList<IMessageQueue>>();
        messageQueueList.AddRange(this);
        return messageQueueList;
    }

    IEnumerator<IMessageQueue> IMessageQueueTypeGroup<IMessageQueue>.GetEnumerator() 
    {
        var freshEnumerator = Recycler.Borrow<AutoRecycleEnumerator<IMessageQueue>>();
        for (var i = 0; i < Count; i++)
        {
            var checkQueue = this[i];
            if (checkQueue != null) freshEnumerator.Add(checkQueue);
        }

        return freshEnumerator;
    }
}
