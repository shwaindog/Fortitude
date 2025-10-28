// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.Config;
using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Groups;

public interface IMessageQueueTypeGroup<T> : IEnumerable<T> where T : IMessageQueue
{
    int  Count      { get; }
    bool HasStarted { get; }
    T? this[int index] { get; set; }

    bool Contains(T item);
    int  AddNewQueues(DeploymentOptions deploymentOptions);
    void Add(T toAdd);
    bool StopRemoveEventQueue(T messageQueue);

    IMessageQueueList<T> AsMessageQueueList();
    new IEnumerator<T>   GetEnumerator();
}

public interface IMessageQueueTypeGroup : IMessageQueueTypeGroup<IMessageQueue>;

public abstract class MessageQueueTypeGroup<T> : IMessageQueueTypeGroup<T> where T : IMessageQueue
{
    private readonly List<T?>             eventQueues = new();
    private readonly MessageQueueType     groupType;
    private readonly IConfigureMessageBus owningMessageBus;

    protected readonly IQueuesConfig QueuesConfig;

    protected MessageQueueTypeGroup
        (IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IQueuesConfig queuesConfig)
    {
        this.owningMessageBus = owningMessageBus;
        this.groupType        = groupType;
        QueuesConfig          = queuesConfig;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual IEnumerator<T> GetEnumerator()
    {
        var freshEnumerator = Recycler.ThreadStaticRecycler.Borrow<AutoRecycleEnumerator<T>>();
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
                throw new InvalidOperationException
                    ($"Can not add EventQueue of type {value.QueueType} to EventQueueGroup of type {groupType}");

            if (index < eventQueues.Count)
            {
                if (value != null || eventQueues[index] != null)
                    throw new ArgumentException
                        ("You can only append new queues to the end of an EventQueueGroup or replace a stopped queue.");

                var existingValue = eventQueues[index];
                if (value == null && existingValue is { IsRunning: true }) existingValue.Shutdown();
            }

            eventQueues.Add(value);
        }
    }

    public int AddNewQueues(DeploymentOptions deploymentOptions)
    {
        var existingCount       = eventQueues.Count;
        var defaultNewQueueSize = groupType == MessageQueueType.Event ? QueuesConfig.EventQueueSize : QueuesConfig.DefaultQueueSize;
        var emptyQueueSleepMs   = groupType == MessageQueueType.Event ? QueuesConfig.EmptyEventQueueSleepMs : QueuesConfig.DefaultEmptyQueueSleepMs;
        for (var i = existingCount; i < existingCount + deploymentOptions.Instances; i++)
        {
            var name = $"{groupType}-{i}";

            eventQueues.Add(CreateMessageQueue(owningMessageBus, groupType, i, name, defaultNewQueueSize, emptyQueueSleepMs));
        }

        return eventQueues.Count;
    }

    public void Add(T item)
    {
        if (item.QueueType != groupType)
            throw new InvalidOperationException
                ($"Can not add EventQueue of type {item.QueueType} to EventQueueGroup of type {groupType}");
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
        var messageQueueList = Recycler.ThreadStaticRecycler.Borrow<MessageQueueList<T>>();
        messageQueueList.AddRange(this);
        return messageQueueList;
    }

    public bool Contains(T item) => eventQueues.Contains(item);

    public abstract T CreateMessageQueue
        (IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs);
}

public class MessageQueueTypeGroup : MessageQueueTypeGroup<IMessageQueue>, IMessageQueueTypeGroup
{
    public MessageQueueTypeGroup(IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IQueuesConfig queuesConfig)
        : base(owningMessageBus, groupType, queuesConfig) { }

    public override IMessageQueue CreateMessageQueue
        (IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs)
    {
        var messagePump = new MessagePump($"MessageQueue-{name}", queueSize, noDataPauseTimeoutMs);
        return new MessageQueue(configureMessageBus, messageQueueType, id, messagePump);
    }
}

public interface INetworkInboundMessageTypeGroup : IMessageQueueTypeGroup<INetworkInboundMessageQueue>, IMessageQueueTypeGroup
{
    new int  Count      { get; }
    new bool HasStarted { get; }

    new INetworkInboundMessageQueue? this[int index] { get; set; }

    new int  AddNewQueues(DeploymentOptions deploymentOptions);
    new bool Contains(INetworkInboundMessageQueue item);
    new bool StopRemoveEventQueue(INetworkInboundMessageQueue messageQueue);

    new IEnumerator<INetworkInboundMessageQueue>       GetEnumerator();
    new IMessageQueueList<INetworkInboundMessageQueue> AsMessageQueueList();
}

internal class SocketListenerMessageQueueGroup : MessageQueueTypeGroup<INetworkInboundMessageQueue>, INetworkInboundMessageTypeGroup
{
    public static IUpdateableTimer timer = TimerContext.CreateUpdateableTimer("Fortitude.BusRules.SocketEventQueueListenerGroup.ThreadPoolTimer");

    public SocketListenerMessageQueueGroup
        (IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IQueuesConfig queuesConfig) : base(owningMessageBus, groupType
   , queuesConfig) { }

    IMessageQueue? IMessageQueueTypeGroup<IMessageQueue>.this[int index]
    {
        get => this[index];
        set => this[index] = (INetworkInboundMessageQueue?)value;
    }

    public bool Contains(IMessageQueue item) => item is INetworkInboundMessageQueue ioInboundMessageQueue && base.Contains(ioInboundMessageQueue);

    public void Add(IMessageQueue toAdd)
    {
        if (toAdd is INetworkInboundMessageQueue ioInboundMessageQueue) base.Add(ioInboundMessageQueue);
    }

    public bool StopRemoveEventQueue
        (IMessageQueue messageQueue) =>
        messageQueue is INetworkInboundMessageQueue ioInboundMessageQueue && base.StopRemoveEventQueue(ioInboundMessageQueue);


    public new IMessageQueueList<IMessageQueue> AsMessageQueueList()
    {
        var messageQueueList = Recycler.ThreadStaticRecycler.Borrow<MessageQueueList<IMessageQueue>>();
        messageQueueList.AddRange(this);
        return messageQueueList;
    }

    IEnumerator<IMessageQueue> IEnumerable<IMessageQueue>.GetEnumerator() => GetEnumerator();

    IEnumerator<IMessageQueue> IMessageQueueTypeGroup<IMessageQueue>.GetEnumerator()
    {
        var freshEnumerator = Recycler.ThreadStaticRecycler.Borrow<AutoRecycleEnumerator<IMessageQueue>>();
        for (var i = 0; i < Count; i++)
        {
            var checkQueue = this[i];
            if (checkQueue != null) freshEnumerator.Add(checkQueue);
        }

        return freshEnumerator;
    }

    public override INetworkInboundMessageQueue CreateMessageQueue
        (IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs)
    {
        var ring = new QueueMessageRing($"NetworkInboundMessageQueue-{name}", queueSize);
        var ringPoller = new SocketAsyncValueTaskEventQueueListener
            (ring, noDataPauseTimeoutMs, new SocketSelector(QueuesConfig.SelectorPollIntervalMs), timer);
        return new NetworkInboundMessageQueue(configureMessageBus, messageQueueType, id, ringPoller);
    }
}

public interface INetworkOutboundMessageTypeGroup : IMessageQueueTypeGroup<INetworkOutboundMessageQueue>, IMessageQueueTypeGroup
{
    new int  Count      { get; }
    new bool HasStarted { get; }

    new INetworkOutboundMessageQueue? this[int index] { get; set; }

    new int  AddNewQueues(DeploymentOptions deploymentOptions);
    new bool Contains(INetworkOutboundMessageQueue item);
    new bool StopRemoveEventQueue(INetworkOutboundMessageQueue messageQueue);

    new IEnumerator<INetworkOutboundMessageQueue>       GetEnumerator();
    new IMessageQueueList<INetworkOutboundMessageQueue> AsMessageQueueList();
}

public class SocketSenderMessageQueueGroup
    (IConfigureMessageBus owningMessageBus, MessageQueueType groupType, IQueuesConfig queuesConfig)
    : MessageQueueTypeGroup<INetworkOutboundMessageQueue>(owningMessageBus, groupType, queuesConfig), INetworkOutboundMessageTypeGroup
{
    IEnumerator<IMessageQueue> IEnumerable<IMessageQueue>.GetEnumerator() => GetEnumerator();

    IMessageQueue? IMessageQueueTypeGroup<IMessageQueue>.this[int index]
    {
        get => this[index];
        set => this[index] = (INetworkOutboundMessageQueue?)value;
    }

    public bool Contains(IMessageQueue item) => item is INetworkOutboundMessageQueue ioOutboundMessageQueue && base.Contains(ioOutboundMessageQueue);

    public void Add(IMessageQueue toAdd)
    {
        if (toAdd is INetworkOutboundMessageQueue ioOutboundMessageQueue) base.Add(ioOutboundMessageQueue);
    }

    public bool StopRemoveEventQueue
        (IMessageQueue messageQueue) =>
        messageQueue is INetworkOutboundMessageQueue ioOutboundMessageQueue && base.StopRemoveEventQueue(ioOutboundMessageQueue);

    IMessageQueueList<IMessageQueue> IMessageQueueTypeGroup<IMessageQueue>.AsMessageQueueList()
    {
        var messageQueueList = Recycler.ThreadStaticRecycler.Borrow<MessageQueueList<IMessageQueue>>();
        messageQueueList.AddRange(this);
        return messageQueueList;
    }

    IEnumerator<IMessageQueue> IMessageQueueTypeGroup<IMessageQueue>.GetEnumerator()
    {
        var freshEnumerator = Recycler.ThreadStaticRecycler.Borrow<AutoRecycleEnumerator<IMessageQueue>>();
        for (var i = 0; i < Count; i++)
        {
            var checkQueue = this[i];
            if (checkQueue != null) freshEnumerator.Add(checkQueue);
        }

        return freshEnumerator;
    }

    public override INetworkOutboundMessageQueue CreateMessageQueue
        (IConfigureMessageBus configureMessageBus, MessageQueueType messageQueueType, int id, string name, int queueSize, uint noDataPauseTimeoutMs)
    {
        var ring       = new QueueMessageRing($"NetworkOutboundMessageQueue-{name}", queueSize);
        var ringPoller = new SocketAsyncValueTaskEventQueueSender(ring, noDataPauseTimeoutMs);
        return new NetworkOutboundMessageQueue(configureMessageBus, messageQueueType, id, ringPoller);
    }
}
