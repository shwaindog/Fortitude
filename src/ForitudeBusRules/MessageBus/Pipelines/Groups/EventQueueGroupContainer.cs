#region

using System.Collections;
using FortitudeBusRules.Config;
using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.MessageBus.Pipelines.Groups;

public interface IEventQueueGroupContainer : IEnumerable<IEventQueue>
{
    bool HasStarted { get; }
    int Count { get; }
    IEventQueueGroup EventGroup { get; }
    IEventQueueGroup WorkerGroup { get; }
    IEventQueueGroup IOInboundGroup { get; }
    IEventQueueGroup IOOutboundGroup { get; }
    IEventQueueGroup CustomGroup { get; }
    IEventQueueGroupContainer Add(IEventQueue eventQueue);
    IEventQueueGroupContainer AddRange(IEnumerable<IEventQueue> eventQueue);
    bool Remove(IEventQueue toRemove);
    bool Contains(IEventQueue eventQueue);
    void Start();
    void Stop();
    ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule, DeploymentOptions deployment);

    ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions);
}

public class EventQueueGroupContainer : IEventQueueGroupContainer
{
    // ideal minimum number of cores is 6 to avoid excessive context switching
    public const int MaximumIOQueues = 10; // inbound, outbound, logging
    public const int ReservedCoresForIO = 3; // inbound, outbound, logging
    public const int MinimumEventQueues = 1; // at least two
    public const int MinimumWorkerQueues = 1; // at least one
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(EventQueueGroupContainer));

    private readonly IEventBus owningEventBus;
    private readonly IRecycler recycler;
    private int lastRequestIndex;


    public EventQueueGroupContainer(IEventBus owningEventBus, BusRulesConfig config)
    {
        this.owningEventBus = owningEventBus;
        recycler = new Recycler();
        this.owningEventBus = owningEventBus;
        var defaultQueueSize = Math.Max(config.DefaultQueueSize, 1);
        var ioInboundNum = Math.Max(config.RequiredIOInboundQueues, MaximumIOQueues);
        IOInboundGroup
            = new SpecificEventQueueGroup(owningEventBus, EventQueueType.IOInbound, recycler, defaultQueueSize);
        IOInboundGroup.AddNewQueues(new DeploymentOptions((uint)ioInboundNum, EventQueueType.IOInbound));
        var ioOutboundNum = Math.Max(config.RequiredIOOutboundQueues, MaximumIOQueues);
        IOOutboundGroup
            = new SpecificEventQueueGroup(owningEventBus, EventQueueType.IOOutbound, recycler, defaultQueueSize);
        IOOutboundGroup.AddNewQueues(new DeploymentOptions((uint)ioOutboundNum, EventQueueType.IOOutbound));

        var eventQueueSize = Math.Max(config.EventQueueSize, 1);
        var eventMinNum = Math.Max(config.MinEventQueues, MinimumEventQueues);
        var eventQueueNum = Math.Max(Math.Min(config.MaxEventQueues, Environment.ProcessorCount - ReservedCoresForIO)
            , eventMinNum);
        EventGroup
            = new SpecificEventQueueGroup(owningEventBus, EventQueueType.Event, recycler, eventQueueSize);
        EventGroup.AddNewQueues(new DeploymentOptions((uint)eventQueueNum));

        var workerMinNum = Math.Max(config.MinWorkerQueues, MinimumWorkerQueues);
        var workerQueueNum = Math.Max(Math.Min(config.MaxWorkerQueues, Environment.ProcessorCount - ReservedCoresForIO)
            , workerMinNum);
        WorkerGroup
            = new SpecificEventQueueGroup(owningEventBus, EventQueueType.Worker, recycler, defaultQueueSize);
        WorkerGroup.AddNewQueues(new DeploymentOptions((uint)workerQueueNum));

        // Runtime added group of custom queue use cases.
        CustomGroup = new SpecificEventQueueGroup(owningEventBus, EventQueueType.Custom, recycler, defaultQueueSize);
    }

    public EventQueueGroupContainer(IEventBus owningEventBus
        , Func<IEventBus, IEventQueueGroup>? createEventQueueGroup = null
        , Func<IEventBus, IEventQueueGroup>? createWorkerGroup = null
        , Func<IEventBus, IEventQueueGroup>? createIoInboundGroup = null
        , Func<IEventBus, IEventQueueGroup>? createIoOutboundGroup = null
        , Func<IEventBus, IEventQueueGroup>? createCustomGroup = null)
    {
        this.owningEventBus = owningEventBus;
        recycler = new Recycler();
        IOOutboundGroup = createIoOutboundGroup?.Invoke(owningEventBus) ??
                          new SpecificEventQueueGroup(owningEventBus, EventQueueType.IOOutbound, recycler, 1_000);
        IOInboundGroup = createIoInboundGroup?.Invoke(owningEventBus) ??
                         new SpecificEventQueueGroup(owningEventBus, EventQueueType.IOInbound, recycler, 1_000);
        EventGroup = createEventQueueGroup?.Invoke(owningEventBus) ??
                     new SpecificEventQueueGroup(owningEventBus, EventQueueType.Event, recycler, 1_000);
        WorkerGroup = createWorkerGroup?.Invoke(owningEventBus) ??
                      new SpecificEventQueueGroup(owningEventBus, EventQueueType.Worker, recycler, 1_000);
        CustomGroup = createCustomGroup?.Invoke(owningEventBus) ??
                      new SpecificEventQueueGroup(owningEventBus, EventQueueType.Custom, recycler, 1_000);
    }

    public void Start()
    {
        foreach (var eventQueue in this)
            if (!eventQueue.IsRunning)
                eventQueue.Start();
    }

    public void Stop()
    {
        foreach (var eventQueue in this)
            if (eventQueue.IsRunning)
                eventQueue.Shutdown();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IEventQueue> GetEnumerator()
    {
        var unfoldEnumerator = recycler?.Borrow<AutoRecycleUnfoldingEnumerator<IEventQueue>>() ??
                               new AutoRecycleUnfoldingEnumerator<IEventQueue>();
        unfoldEnumerator.Add(IOOutboundGroup.GetEnumerator());
        unfoldEnumerator.Add(IOInboundGroup.GetEnumerator());
        unfoldEnumerator.Add(EventGroup.GetEnumerator());
        unfoldEnumerator.Add(WorkerGroup.GetEnumerator());
        unfoldEnumerator.Add(CustomGroup.GetEnumerator());

        return unfoldEnumerator;
    }

    public ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule, DeploymentOptions options)
    {
        var queueGroup = options.EventGroupType;
        switch (queueGroup)
        {
            case EventQueueType.Event:
                return EventGroup.LaunchRule(sender, rule, options);
            case EventQueueType.Worker:
                return WorkerGroup.LaunchRule(sender, rule, options);
            case EventQueueType.IOInbound:
                return IOInboundGroup.LaunchRule(sender, rule, options);
            case EventQueueType.IOOutbound:
                return IOOutboundGroup.LaunchRule(sender, rule, options);
            case EventQueueType.Custom:
                return CustomGroup.LaunchRule(sender, rule, options);
            default:
                var message
                    = $"Could not find the required group to deploy rule {rule} to event Queue type {options.EventGroupType}";
                logger.Warn(message);
                throw new ArgumentException(message);
        }
    }

    public ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions)
    {
        IReusableList<IEventQueue>? allEventQueues = null;
        try
        {
            allEventQueues = GetAllQueues();
            var processorRegistry = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
            processorRegistry.DispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
            processorRegistry.IncrementRefCount();
            processorRegistry.DispatchResult.SentTime = DateTime.Now;
            processorRegistry.RecycleTimer = sender.Context.Timer;
            for (var i = lastRequestIndex + 1; i < lastRequestIndex + 1 + allEventQueues.Count; i++)
            {
                var currIndex = i % allEventQueues.Count;
                var currQueue = allEventQueues[currIndex];
                if (currQueue.IsListeningToAddress(publishAddress))
                {
                    lastRequestIndex = currIndex;
                    return currQueue.RequestFromPayload<T, U>(msg, sender, processorRegistry, publishAddress);
                }
            }
        }
        finally
        {
            allEventQueues?.DecrementRefCount();
        }

        throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
    }


    public IEventQueueGroupContainer Add(IEventQueue item)
    {
        var queueGroup = item.QueueType;
        switch (queueGroup)
        {
            case EventQueueType.IOOutbound:
                IOOutboundGroup.Add(item);
                break;
            case EventQueueType.IOInbound:
                IOInboundGroup.Add(item);
                break;
            case EventQueueType.Event:
                EventGroup.Add(item);
                break;
            case EventQueueType.Worker:
                WorkerGroup.Add(item);
                break;
            case EventQueueType.Custom:
                CustomGroup.Add(item);
                break;
            default:
                var message = $"Can not add queue of type {queueGroup} to EventBus ";
                logger.Warn(message);
                throw new ArgumentException(message);
        }

        return this;
    }

    public bool Contains(IEventQueue item)
    {
        var queueGroup = item.QueueType;

        var hasResult = queueGroup switch
        {
            EventQueueType.IOOutbound => IOOutboundGroup.Contains(item)
            , EventQueueType.IOInbound => IOOutboundGroup.Contains(item)
            , EventQueueType.Event => EventGroup.Contains(item), EventQueueType.Worker => WorkerGroup.Contains(item)
            , EventQueueType.Custom => CustomGroup.Contains(item), _ => false
        };
        return hasResult;
    }

    public bool Remove(IEventQueue item)
    {
        var queueGroup = item.QueueType;
        switch (queueGroup)
        {
            case EventQueueType.IOOutbound:
                return IOOutboundGroup.Remove(item);
            case EventQueueType.IOInbound:
                return IOInboundGroup.Remove(item);
            case EventQueueType.Event:
                return EventGroup.Remove(item);
            case EventQueueType.Worker:
                return WorkerGroup.Remove(item);
            case EventQueueType.Custom:
                return CustomGroup.Remove(item);
            default:
                var message = $"Can not add queue of type {queueGroup} to EventBus ";
                logger.Warn(message);
                throw new ArgumentException(message);
        }
    }

    public int Count =>
        EventGroup.Count + WorkerGroup.Count + IOInboundGroup.Count + IOOutboundGroup.Count + CustomGroup.Count;

    public bool HasStarted =>
        EventGroup.HasStarted || WorkerGroup.HasStarted || IOInboundGroup.HasStarted || IOOutboundGroup.HasStarted ||
        CustomGroup.HasStarted;

    public IEventQueueGroupContainer AddRange(IEnumerable<IEventQueue> eventQueue)
    {
        foreach (var queue in eventQueue) Add(queue);

        return this;
    }

    public IEventQueueGroup EventGroup { get; }
    public IEventQueueGroup WorkerGroup { get; }
    public IEventQueueGroup IOInboundGroup { get; }
    public IEventQueueGroup IOOutboundGroup { get; }
    public IEventQueueGroup CustomGroup { get; }

    public IReusableList<IEventQueue> GetAllQueues()
    {
        var reusableEventQueues = recycler.Borrow<ReusableList<IEventQueue>>();
        reusableEventQueues.AddRange(IOOutboundGroup).AddRange(IOInboundGroup).AddRange(EventGroup)
            .AddRange(WorkerGroup).AddRange(CustomGroup);
        return reusableEventQueues;
    }
}
