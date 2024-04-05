#region

using System.Collections;
using FortitudeBusRules.Config;
using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Routing.SelectionStrategies;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using static FortitudeBusRules.MessageBus.Pipelines.EventQueueType;

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
    IReusableList<IEventQueue> SelectEventQueues(EventQueueType selector);
    IEventQueueGroup SelectEventQueueGroup(IEventQueue childOfGroup);
    IEventQueueGroup SelectEventQueueGroup(EventQueueType selector);
    bool Remove(IEventQueue toRemove);
    bool Contains(IEventQueue eventQueue);
    void Start();
    void Stop();
    ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule, DeploymentOptions deployment);

    ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);

    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);
}

public class EventQueueGroupContainer : IEventQueueGroupContainer
{
    // ideal minimum number of cores is 6 to avoid excessive context switching
    public const int MaximumIOQueues = 10; // inbound, outbound

    public const int ReservedCoresForIO = 4; // inbound (deserializers), outbound (serializers), logging, acceptor listening thread

    public const int MinimumEventQueues = 1; // at least one
    public const int MinimumWorkerQueues = 1; // at least one
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(EventQueueGroupContainer));

    private readonly IEventBus owningEventBus;
    private readonly Recycler recycler;
    private readonly DeployDispatchStrategySelector strategySelector;

    public EventQueueGroupContainer(IEventBus owningEventBus, BusRulesConfig config)
    {
        this.owningEventBus = owningEventBus;
        recycler = new Recycler();
        strategySelector = new DeployDispatchStrategySelector(recycler);
        this.owningEventBus = owningEventBus;
        var ioInboundNum = Math.Max(config.QueuesConfig.RequiredIOInboundQueues, MaximumIOQueues);
        IOInboundGroup = new SocketEventQueueListenerGroup(owningEventBus, IOInbound, recycler, config.QueuesConfig);
        IOInboundGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, IOInbound, (uint)ioInboundNum));
        var ioOutboundNum = Math.Max(config.QueuesConfig.RequiredIOOutboundQueues, MaximumIOQueues);
        IOOutboundGroup = new SocketEventQueueSenderGroup(owningEventBus, IOOutbound, recycler, config.QueuesConfig);
        IOOutboundGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, IOOutbound, (uint)ioOutboundNum));

        var eventMinNum = Math.Max(config.QueuesConfig.MinEventQueues, MinimumEventQueues);
        var eventQueueNum = Math.Max(Math.Min(config.QueuesConfig.MaxEventQueues, Environment.ProcessorCount - ReservedCoresForIO), eventMinNum);
        EventGroup = new SpecificEventQueueGroup(owningEventBus, Event, recycler, config.QueuesConfig);
        EventGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, Event, (uint)eventQueueNum));

        var workerMinNum = Math.Max(config.QueuesConfig.MinWorkerQueues, MinimumWorkerQueues);
        var workerQueueNum = Math.Max(Math.Min(config.QueuesConfig.MaxWorkerQueues, Environment.ProcessorCount - ReservedCoresForIO), workerMinNum);
        WorkerGroup = new SpecificEventQueueGroup(owningEventBus, Worker, recycler, config.QueuesConfig);
        WorkerGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, Worker, (uint)workerQueueNum));

        // Runtime added group of custom queue use cases.
        CustomGroup = new SpecificEventQueueGroup(owningEventBus, Custom, recycler, config.QueuesConfig);
    }

    internal EventQueueGroupContainer(IEventBus owningEventBus
        , Func<IEventBus, IEventQueueGroup>? createEventQueueGroup = null
        , Func<IEventBus, IEventQueueGroup>? createWorkerGroup = null
        , Func<IEventBus, IEventQueueGroup>? createIoInboundGroup = null
        , Func<IEventBus, IEventQueueGroup>? createIoOutboundGroup = null
        , Func<IEventBus, IEventQueueGroup>? createCustomGroup = null)
    {
        this.owningEventBus = owningEventBus;
        recycler = new Recycler();
        strategySelector = new DeployDispatchStrategySelector(recycler);
        var defaultQueuesConfig = new QueuesConfig();
        IOOutboundGroup = createIoOutboundGroup?.Invoke(owningEventBus) ??
                          new SpecificEventQueueGroup(owningEventBus, IOOutbound, recycler, defaultQueuesConfig);
        IOInboundGroup = createIoInboundGroup?.Invoke(owningEventBus) ??
                         new SpecificEventQueueGroup(owningEventBus, IOInbound, recycler, defaultQueuesConfig);
        EventGroup = createEventQueueGroup?.Invoke(owningEventBus) ??
                     new SpecificEventQueueGroup(owningEventBus, Event, recycler, defaultQueuesConfig);
        WorkerGroup = createWorkerGroup?.Invoke(owningEventBus) ?? new SpecificEventQueueGroup(owningEventBus, Worker, recycler, defaultQueuesConfig);
        CustomGroup = createCustomGroup?.Invoke(owningEventBus) ?? new SpecificEventQueueGroup(owningEventBus, Custom, recycler, defaultQueuesConfig);
    }

    internal EventQueueGroupContainer(IEventBus owningEventBus, IEnumerable<IEventQueue> queuesToAdd)
    {
        recycler = new Recycler();
        var defaultQueuesConfig = new QueuesConfig();
        strategySelector = new DeployDispatchStrategySelector(recycler);
        IOOutboundGroup = new SpecificEventQueueGroup(owningEventBus, IOOutbound, recycler, defaultQueuesConfig);
        IOInboundGroup = new SpecificEventQueueGroup(owningEventBus, IOInbound, recycler, defaultQueuesConfig)!;
        EventGroup = new SpecificEventQueueGroup(owningEventBus, Event, recycler, defaultQueuesConfig)!;
        WorkerGroup = new SpecificEventQueueGroup(owningEventBus, Worker, recycler, defaultQueuesConfig)!;
        CustomGroup = new SpecificEventQueueGroup(owningEventBus, Custom, recycler, defaultQueuesConfig)!;
        this.owningEventBus = owningEventBus;
        foreach (var eventQueue in queuesToAdd) Add(eventQueue);
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
        var eventQueueEnumerator = recycler.Borrow<AutoRecycleUnfoldingEnumerator<IEventQueue>>();
        eventQueueEnumerator.Add(IOOutboundGroup.GetEnumerator());
        eventQueueEnumerator.Add(IOInboundGroup.GetEnumerator());
        eventQueueEnumerator.Add(EventGroup.GetEnumerator());
        eventQueueEnumerator.Add(WorkerGroup.GetEnumerator());
        eventQueueEnumerator.Add(CustomGroup.GetEnumerator());

        return eventQueueEnumerator;
    }

    public ValueTask<IDispatchResult> LaunchRule(IRule sender, IRule rule, DeploymentOptions options)
    {
        var selectionStrategy = strategySelector.SelectDeployStrategy(sender, rule, options);
        var selectionResult = selectionStrategy.Select(this, sender, rule, options);
        if (selectionResult != null)
        {
            var routeSelectionResult = selectionResult.Value;
            var destinationEventQueue = routeSelectionResult.EventQueue;
            return destinationEventQueue.LaunchRuleAsync(sender, rule, routeSelectionResult);
        }

        var message = $"Could not find the required group to deploy rule {rule} to event Queue type {options.EventGroupType}";
        Logger.Warn(message);
        throw new ArgumentException(message);
    }

    public ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions)
    {
        var selectionStrategy = strategySelector.SelectDispatchStrategy(sender, dispatchOptions, publishAddress);
        var selectionResult = selectionStrategy.Select(this, sender, dispatchOptions, publishAddress);
        if (selectionResult is { HasItems: true, Count: 1 })
        {
            var processorRegistry = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
            processorRegistry.DispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
            processorRegistry.DispatchResult.DispatchSelectionResultSet = selectionResult;
            processorRegistry.IncrementRefCount();
            processorRegistry.DispatchResult.SentTime = DateTime.Now;
            processorRegistry.RecycleTimer = sender.Context.Timer;
            var requestResponseSelectionResult = selectionResult.First();
            var destinationEventQueue = requestResponseSelectionResult.EventQueue;
            var destinationRule = requestResponseSelectionResult.Rule;
            return destinationEventQueue.RequestFromPayloadAsync<T, U>(msg, sender, processorRegistry, publishAddress
                , ruleFilter: destinationRule?.AppliesToThisRule);
        }

        selectionResult?.DecrementRefCount();
        throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
    }

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions)
    {
        var selectionStrategy = strategySelector.SelectDispatchStrategy(sender, dispatchOptions, publishAddress);
        var selectionResult = selectionStrategy.Select(this, sender, dispatchOptions, publishAddress);
        if (selectionResult is { HasItems: true })
        {
            var processorRegistry = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
            processorRegistry.DispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
            processorRegistry.DispatchResult.DispatchSelectionResultSet = selectionResult;
            processorRegistry.IncrementRefCount();
            processorRegistry.DispatchResult.SentTime = DateTime.Now;
            processorRegistry.RecycleTimer = sender.Context.Timer;

            foreach (var routeResult in selectionResult)
            {
                var destinationEventQueue = routeResult.EventQueue;
                var destinationRule = routeResult.Rule;
                var _ = destinationEventQueue.EnqueuePayloadWithStatsAsync(msg, sender, processorRegistry
                    , publishAddress
                    , ruleFilter: destinationRule?.AppliesToThisRule);
            }

            return processorRegistry.GenerateValueTask();
        }

        selectionResult?.DecrementRefCount();
        throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
    }

    public IEventQueueGroupContainer Add(IEventQueue item)
    {
        var queueGroup = item.QueueType;
        switch (queueGroup)
        {
            case IOOutbound:
                IOOutboundGroup.Add(item);
                break;
            case IOInbound:
                IOInboundGroup.Add(item);
                break;
            case Event:
                EventGroup.Add(item);
                break;
            case Worker:
                WorkerGroup.Add(item);
                break;
            case Custom:
                CustomGroup.Add(item);
                break;
            default:
                var message = $"Can not add queue of type {queueGroup} to EventBus ";
                Logger.Warn(message);
                throw new ArgumentException(message);
        }

        return this;
    }

    public bool Contains(IEventQueue item)
    {
        var queueGroup = item.QueueType;

        var hasResult = queueGroup switch
        {
            IOOutbound => IOOutboundGroup.Contains(item)
            , IOInbound => IOOutboundGroup.Contains(item)
            , Event => EventGroup.Contains(item), Worker => WorkerGroup.Contains(item)
            , Custom => CustomGroup.Contains(item), _ => false
        };
        return hasResult;
    }

    public bool Remove(IEventQueue item)
    {
        var queueGroup = item.QueueType;
        switch (queueGroup)
        {
            case IOOutbound:
                return IOOutboundGroup.StopRemoveEventQueue(item);
            case IOInbound:
                return IOInboundGroup.StopRemoveEventQueue(item);
            case Event:
                return EventGroup.StopRemoveEventQueue(item);
            case Worker:
                return WorkerGroup.StopRemoveEventQueue(item);
            case Custom:
                return CustomGroup.StopRemoveEventQueue(item);
            default:
                var message = $"Can not add queue of type {queueGroup} to EventBus ";
                Logger.Warn(message);
                throw new ArgumentException(message);
        }
    }

    public int Count => EventGroup.Count + WorkerGroup.Count + IOInboundGroup.Count + IOOutboundGroup.Count + CustomGroup.Count;

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

    public IEventQueueGroup SelectEventQueueGroup(IEventQueue childOfGroup) => SelectEventQueueGroup(childOfGroup.QueueType);

    public IEventQueueGroup SelectEventQueueGroup(EventQueueType selector)
    {
        return selector switch
        {
            IOOutbound => IOOutboundGroup
            , IOInbound => IOInboundGroup
            , Event => EventGroup
            , Worker => WorkerGroup
            , Custom => CustomGroup
            , _ => throw new ArgumentException(
                $"Unexpected selector EventQueueType selector value '{selector}' when selecting a specific event group type")
        };
    }

    public IReusableList<IEventQueue> SelectEventQueues(EventQueueType selector)
    {
        var result = recycler.Borrow<ReusableList<IEventQueue>>();
        if (selector.IsIOOutbound()) result.AddRange(IOOutboundGroup);
        if (selector.IsIOInbound()) result.AddRange(IOInboundGroup);
        if (selector.IsEvent()) result.AddRange(EventGroup);
        if (selector.IsWorker()) result.AddRange(WorkerGroup);
        if (selector.IsCustom()) result.AddRange(CustomGroup);

        return result;
    }
}
