// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Config;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using static FortitudeBusRules.BusMessaging.Pipelines.MessageQueueType;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Groups;

public interface IMessageQueueGroupContainer : IEnumerable<IMessageQueue>
{
    bool HasStarted { get; }
    int  Count      { get; }

    IMessageQueueTypeGroup EventMessageQueueGroup  { get; }
    IMessageQueueTypeGroup WorkerMessageQueueGroup { get; }

    INetworkInboundMessageTypeGroup  NetworkInboundMessageQueueGroup  { get; }
    INetworkOutboundMessageTypeGroup NetworkOutboundMessageQueueGroup { get; }

    IMessageQueueTypeGroup CustomMessageQueueGroup     { get; }
    IMessageQueueTypeGroup DataAccessMessageQueueGroup { get; }

    IMessageQueueGroupContainer Add(IMessageQueue messageQueue);
    IMessageQueueGroupContainer AddRange(IEnumerable<IMessageQueue> eventQueue);

    IMessageQueueList<IMessageQueue> SelectEventQueues(MessageQueueType selector);

    IMessageQueueTypeGroup SelectEventQueueGroup(IMessageQueue childOfGroup);
    IMessageQueueTypeGroup SelectEventQueueGroup(MessageQueueType selector);

    bool Remove(IMessageQueue toRemove);
    bool Contains(IMessageQueue messageQueue);
    void Start();
    void Stop();
    void LaunchRule(IRule parent, IRule rule, DeploymentOptions deployment);

    ValueTask<IRuleDeploymentLifeTime> LaunchRuleAsync(IRule parent, IRule rule, DeploymentOptions deployment);

    ValueTask<U> RequestAsync<T, U>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);

    ValueTask AddListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor, MessageQueueType onQueueTypes);
    ValueTask RemoveListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor, MessageQueueType onQueueTypes);
    void      Publish<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);

    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);

    void Send<T>(IRule sender, T messageOrEvent, MessageType messageType, DispatchOptions dispatchOptions);

    IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate);
}

public class MessageQueueGroupContainer : IMessageQueueGroupContainer
{
    // ideal minimum number of cores is 6 to avoid excessive context switching
    public const int MaximumIOQueues        = 10; // inbound, outbound
    public const int MaximumAuxiliaryQueues = 10; // inbound, outbound

    public const int ReservedCoresForIO = 4; // inbound (deserializers), outbound (serializers), logging

    public const int MinimumEventQueues  = 1; // at least one
    public const int MinimumWorkerQueues = 1; // at least one

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessageQueueGroupContainer));

    [ThreadStatic] private static DeployDispatchStrategySelector? strategySelector;

    private readonly IConfigureMessageBus owningMessageBus;

    public MessageQueueGroupContainer(IConfigureMessageBus owningMessageBus, BusRulesConfig config)
    {
        this.owningMessageBus = owningMessageBus;
        this.owningMessageBus = owningMessageBus;
        var ioInboundNum = Math.Max(0, Math.Min(config.QueuesConfig.RequiredNetworkInboundQueues, MaximumIOQueues));
        NetworkInboundMessageQueueGroup = new SocketListenerMessageQueueGroup(owningMessageBus, NetworkInbound, config.QueuesConfig);
        NetworkInboundMessageQueueGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, NetworkInbound, (uint)ioInboundNum));
        var ioOutboundNum = Math.Max(0, Math.Min(config.QueuesConfig.RequiredNetworkOutboundQueues, MaximumIOQueues));
        NetworkOutboundMessageQueueGroup = new SocketSenderMessageQueueGroup(owningMessageBus, NetworkOutbound, config.QueuesConfig);
        NetworkOutboundMessageQueueGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, NetworkOutbound, (uint)ioOutboundNum));

        var eventMinNum   = Math.Max(config.QueuesConfig.MinEventQueues, MinimumEventQueues);
        var eventQueueNum = Math.Max(Math.Min(config.QueuesConfig.MaxEventQueues, Environment.ProcessorCount - ReservedCoresForIO), eventMinNum);
        EventMessageQueueGroup = new MessageQueueTypeGroup(owningMessageBus, Event, config.QueuesConfig);
        EventMessageQueueGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, Event, (uint)eventQueueNum));

        var workerMinNum   = Math.Max(config.QueuesConfig.MinWorkerQueues, MinimumWorkerQueues);
        var workerQueueNum = Math.Max(Math.Min(config.QueuesConfig.MaxWorkerQueues, Environment.ProcessorCount - ReservedCoresForIO), workerMinNum);
        WorkerMessageQueueGroup = new MessageQueueTypeGroup(owningMessageBus, Worker, config.QueuesConfig);
        WorkerMessageQueueGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, Worker, (uint)workerQueueNum));

        // Runtime added group of custom queue use cases.
        var customNum = Math.Max(0, Math.Min(config.QueuesConfig.RequiredCustomQueues, MaximumAuxiliaryQueues));
        CustomMessageQueueGroup = new MessageQueueTypeGroup(owningMessageBus, Custom, config.QueuesConfig);
        CustomMessageQueueGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, Custom, (uint)customNum));
        var dataAccessNum = Math.Max(0, Math.Min(config.QueuesConfig.RequiredDataAccessQueues, MaximumAuxiliaryQueues));
        DataAccessMessageQueueGroup = new MessageQueueTypeGroup(owningMessageBus, DataAccess, config.QueuesConfig);
        CustomMessageQueueGroup.AddNewQueues(new DeploymentOptions(RoutingFlags.None, DataAccess, (uint)dataAccessNum));
    }

    internal MessageQueueGroupContainer
    (IConfigureMessageBus owningMessageBus
      , Func<IConfigureMessageBus, IMessageQueueTypeGroup>? createEventQueueGroup = null
      , Func<IConfigureMessageBus, IMessageQueueTypeGroup>? createWorkerGroup = null
      , Func<IConfigureMessageBus, INetworkInboundMessageTypeGroup>? createIoInboundGroup = null
      , Func<IConfigureMessageBus, INetworkOutboundMessageTypeGroup>? createIoOutboundGroup = null
      , Func<IConfigureMessageBus, IMessageQueueTypeGroup>? createCustomGroup = null)
    {
        this.owningMessageBus = owningMessageBus;
        var defaultQueuesConfig = new QueuesConfig();
        NetworkOutboundMessageQueueGroup = createIoOutboundGroup?.Invoke(owningMessageBus) ??
                                           new SocketSenderMessageQueueGroup(owningMessageBus, NetworkOutbound, defaultQueuesConfig);
        NetworkInboundMessageQueueGroup = createIoInboundGroup?.Invoke(owningMessageBus) ??
                                          new SocketListenerMessageQueueGroup(owningMessageBus, NetworkInbound, defaultQueuesConfig);
        EventMessageQueueGroup = createEventQueueGroup?.Invoke(owningMessageBus) ??
                                 new MessageQueueTypeGroup(owningMessageBus, Event, defaultQueuesConfig);
        WorkerMessageQueueGroup = createWorkerGroup?.Invoke(owningMessageBus) ??
                                  new MessageQueueTypeGroup(owningMessageBus, Worker, defaultQueuesConfig);
        CustomMessageQueueGroup = createCustomGroup?.Invoke(owningMessageBus) ??
                                  new MessageQueueTypeGroup(owningMessageBus, Custom, defaultQueuesConfig);
        DataAccessMessageQueueGroup = createCustomGroup?.Invoke(owningMessageBus) ??
                                      new MessageQueueTypeGroup(owningMessageBus, DataAccess, defaultQueuesConfig);
    }

    internal MessageQueueGroupContainer(IConfigureMessageBus owningMessageBus, IEnumerable<IMessageQueue> queuesToAdd)
    {
        var defaultQueuesConfig = new QueuesConfig();
        NetworkOutboundMessageQueueGroup = new SocketSenderMessageQueueGroup(owningMessageBus, NetworkOutbound, defaultQueuesConfig);
        NetworkInboundMessageQueueGroup  = new SocketListenerMessageQueueGroup(owningMessageBus, NetworkInbound, defaultQueuesConfig)!;
        EventMessageQueueGroup           = new MessageQueueTypeGroup(owningMessageBus, Event, defaultQueuesConfig)!;
        WorkerMessageQueueGroup          = new MessageQueueTypeGroup(owningMessageBus, Worker, defaultQueuesConfig)!;
        CustomMessageQueueGroup          = new MessageQueueTypeGroup(owningMessageBus, Custom, defaultQueuesConfig)!;
        DataAccessMessageQueueGroup      = new MessageQueueTypeGroup(owningMessageBus, DataAccess, defaultQueuesConfig)!;
        this.owningMessageBus            = owningMessageBus;
        foreach (var eventQueue in queuesToAdd) Add(eventQueue);
    }

    private static DeployDispatchStrategySelector StrategySelector =>
        strategySelector ??= new DeployDispatchStrategySelector(Recycler.ThreadStaticRecycler);

    public void Start()
    {
        foreach (var messageQueue in this)
            if (!messageQueue.IsRunning)
                messageQueue.Start();
    }

    public void Stop()
    {
        foreach (var messageQueue in this)
            if (messageQueue.IsRunning)
                messageQueue.Shutdown();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<IMessageQueue> GetEnumerator()
    {
        var eventQueueEnumerator = Recycler.ThreadStaticRecycler.Borrow<AutoRecycleUnfoldingEnumerator<IMessageQueue>>();
        eventQueueEnumerator.Add(((IMessageQueueTypeGroup<IMessageQueue>)NetworkOutboundMessageQueueGroup).GetEnumerator());
        eventQueueEnumerator.Add(((IMessageQueueTypeGroup<IMessageQueue>)NetworkInboundMessageQueueGroup).GetEnumerator());
        eventQueueEnumerator.Add(EventMessageQueueGroup.GetEnumerator());
        eventQueueEnumerator.Add(WorkerMessageQueueGroup.GetEnumerator());
        if (CustomMessageQueueGroup != null) eventQueueEnumerator.Add(CustomMessageQueueGroup.GetEnumerator());
        eventQueueEnumerator.Add(DataAccessMessageQueueGroup.GetEnumerator());

        return eventQueueEnumerator;
    }

    public IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate)
    {
        var eventQueueEnumerator = Recycler.ThreadStaticRecycler.Borrow<AutoRecycledEnumerable<IRule>>();
        foreach (var messageQueue in this) eventQueueEnumerator.AddRange(messageQueue.RulesMatching(predicate));
        return eventQueueEnumerator;
    }

    public void LaunchRule(IRule parent, IRule rule, DeploymentOptions options)
    {
        var selectionStrategy = StrategySelector.SelectDeployStrategy(parent, rule, options);
        var selectionResult   = selectionStrategy.Select(this, parent, rule, options);
        if (selectionResult != null)
        {
            parent.AddChild(rule);
            var routeSelectionResult  = selectionResult.Value;
            var destinationEventQueue = routeSelectionResult.MessageQueue;
            destinationEventQueue.LaunchRule(parent, rule);
            return;
        }

        var message = $"Could not find the required group to deploy rule {rule} to event Queue type {options.MessageGroupType}";
        Logger.Warn(message);
        throw new ArgumentException(message);
    }

    public ValueTask<IRuleDeploymentLifeTime> LaunchRuleAsync(IRule parent, IRule rule, DeploymentOptions options)
    {
        var selectionStrategy = StrategySelector.SelectDeployStrategy(parent, rule, options);
        var selectionResult   = selectionStrategy.Select(this, parent, rule, options);
        if (selectionResult != null)
        {
            parent.AddChild(rule);
            var routeSelectionResult  = selectionResult.Value;
            var destinationEventQueue = routeSelectionResult.MessageQueue;
            return destinationEventQueue.LaunchRuleAsync(parent, rule, routeSelectionResult);
        }

        var message = $"Could not find the required group to deploy rule {rule} to event Queue type {options.MessageGroupType}";
        Logger.Warn(message);
        throw new ArgumentException(message);
    }

    public ValueTask<U> RequestAsync<T, U>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions)
    {
        var selectionStrategy = StrategySelector.SelectDispatchStrategy(sender, dispatchOptions, publishAddress);
        var selectionResult   = selectionStrategy.Select(this, sender, dispatchOptions, publishAddress);
        if (selectionResult is { HasItems: true, Count: 1 })
        {
            var requestResponseSelectionResult = selectionResult.First();
            var destinationEventQueue          = requestResponseSelectionResult.MessageQueue;
            var destinationRule                = requestResponseSelectionResult.Rule;
            selectionResult.DecrementRefCount();
            return destinationEventQueue.RequestWithPayloadAsync<T, U>
                (msg, sender, publishAddress, ruleFilter: destinationRule?.AppliesToThisRule);
        }

        selectionResult?.DecrementRefCount();
        throw new KeyNotFoundException($"No target rule was selected possibly the destination can not be found or accept  ");
    }

    public void Publish<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions)
    {
        var selectionStrategy = StrategySelector.SelectDispatchStrategy(sender, dispatchOptions, publishAddress);
        var selectionResult   = selectionStrategy.Select(this, sender, dispatchOptions, publishAddress);
        if (selectionResult is { HasItems: true })
        {
            var payLoadMarshaller = dispatchOptions.PayloadMarshalOptions.ResolvePayloadMarshaller(msg, sender.Context.PooledRecycler);
            var payload           = payLoadMarshaller != null ? payLoadMarshaller.GetMarshalled(msg, PayloadRequestType.Dispatch) : msg;
            payLoadMarshaller?.IncrementRefCount(); // while QueueingEnsure queue doesn't finish before all is enqueued
            foreach (var routeResult in selectionResult)
            {
                var destinationEventQueue = routeResult.MessageQueue;
                var destinationRule       = routeResult.Rule;
                destinationEventQueue.EnqueuePayloadBody
                    (payload, sender, MessageType.Publish, publishAddress, destinationRule?.AppliesToThisRule, payLoadMarshaller);
            }

            selectionResult.DecrementRefCount();
            payLoadMarshaller?.DecrementRefCount();

            return;
        }

        selectionResult?.DecrementRefCount();
        if ((dispatchOptions.RoutingFlags & RoutingFlags.SendToAll) != RoutingFlags.SendToAll)
            throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
    }

    public ValueTask<IDispatchResult> PublishAsync<T>
    (IRule sender, string publishAddress, T msg
      , DispatchOptions dispatchOptions)
    {
        var selectionStrategy = StrategySelector.SelectDispatchStrategy(sender, dispatchOptions, publishAddress);
        var selectionResult   = selectionStrategy.Select(this, sender, dispatchOptions, publishAddress);
        if (selectionResult is { HasItems: true })
        {
            var processorRegistry = sender.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();
            processorRegistry.ShouldCollectMetrics              = dispatchOptions.RoutingFlags.HasShouldCollectMetricsFlag();
            processorRegistry.Result                            = sender.Context.PooledRecycler.Borrow<DispatchResult>();
            processorRegistry.Result.DispatchSelectionResultSet = selectionResult;
            processorRegistry.IncrementRefCount();
            processorRegistry.Result.SentTime = DateTime.Now;
            processorRegistry.ResponseTimeoutAndRecycleTimer
                = dispatchOptions.RoutingFlags.HasShouldCollectMetricsFlag() ? sender.Context.QueueTimer : null;
            var payLoadMarshaller = dispatchOptions.PayloadMarshalOptions.ResolvePayloadMarshaller(msg, sender.Context.PooledRecycler);
            var payload           = payLoadMarshaller != null ? payLoadMarshaller.GetMarshalled(msg, PayloadRequestType.Dispatch) : msg;
            payLoadMarshaller?.IncrementRefCount(); // while QueueingEnsure queue doesn't finish before all is queued

            foreach (var routeResult in selectionResult)
            {
                var destinationEventQueue = routeResult.MessageQueue;
                var destinationRule       = routeResult.Rule;
                var _ = destinationEventQueue.EnqueuePayloadBodyWithStatsAsync
                    (payload, sender, MessageType.Publish, publishAddress, processorRegistry
                   , dispatchOptions.RoutingFlags.HasShouldCollectMetricsFlag()
                   , destinationRule?.AppliesToThisRule, payLoadMarshaller);
            }

            payLoadMarshaller?.DecrementRefCount();
            selectionResult.DecrementRefCount();

            return processorRegistry.GenerateValueTask();
        }

        selectionResult?.DecrementRefCount();
        if ((dispatchOptions.RoutingFlags & RoutingFlags.SendToAll) != RoutingFlags.SendToAll)
            throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
        var emptyDispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
        return new ValueTask<IDispatchResult>(emptyDispatchResult);
    }

    public void Send<T>(IRule sender, T messageOrEvent, MessageType messageType, DispatchOptions dispatchOptions)
    {
        var selectionStrategy = StrategySelector.SelectDispatchStrategy(Rule.NoKnownSender, dispatchOptions, null);
        var selectionResult = selectionStrategy.Select(this, Rule.NoKnownSender, dispatchOptions, "");
        var payLoadMarshaller = dispatchOptions.PayloadMarshalOptions.ResolvePayloadMarshaller(messageOrEvent, sender.Context.PooledRecycler);
        var payload = payLoadMarshaller != null ? payLoadMarshaller.GetMarshalled(messageOrEvent, PayloadRequestType.Dispatch) : messageOrEvent;
        payLoadMarshaller?.IncrementRefCount(); // while QueueingEnsure queue doesn't finish before all is enqueued
        if (selectionResult is { HasItems: true })
        {
            foreach (var routeResult in selectionResult)
            {
                var destinationEventQueue = routeResult.MessageQueue;
                destinationEventQueue.EnqueuePayloadBody(payload, sender, messageType, payloadMarshaller: payLoadMarshaller);
            }
            selectionResult.DecrementRefCount();
        }
        else
        {
            Logger.Warn("Could not find dispatch target for {0}, {1}, {2}, {3}", sender.FriendlyName, messageOrEvent, messageType, dispatchOptions);
        }
    }

    public IMessageQueueGroupContainer Add(IMessageQueue item)
    {
        var queueGroup = item.QueueType;
        switch (queueGroup)
        {
            case NetworkOutbound:
                NetworkOutboundMessageQueueGroup.Add(item);
                break;
            case NetworkInbound:
                NetworkInboundMessageQueueGroup.Add(item);
                break;
            case Event:
                EventMessageQueueGroup.Add(item);
                break;
            case Worker:
                WorkerMessageQueueGroup.Add(item);
                break;
            case Custom:
                CustomMessageQueueGroup.Add(item);
                break;
            case DataAccess:
                DataAccessMessageQueueGroup.Add(item);
                break;
            default:
                var message = $"Can not add queue of type {queueGroup} to EventBus ";
                Logger.Warn(message);
                throw new ArgumentException(message);
        }

        return this;
    }

    public bool Contains(IMessageQueue item)
    {
        var queueGroup = item.QueueType;

        var hasResult =
            queueGroup switch
            {
                NetworkOutbound => NetworkOutboundMessageQueueGroup.Contains(item)
              , NetworkInbound  => NetworkInboundMessageQueueGroup.Contains(item)
              , Event           => EventMessageQueueGroup.Contains(item)
              , Worker          => WorkerMessageQueueGroup.Contains(item)
              , Custom          => CustomMessageQueueGroup.Contains(item)
              , DataAccess      => DataAccessMessageQueueGroup.Contains(item)
              , _               => false
            };
        return hasResult;
    }

    public bool Remove(IMessageQueue item)
    {
        var queueGroup = item.QueueType;
        switch (queueGroup)
        {
            case NetworkOutbound: return NetworkOutboundMessageQueueGroup.StopRemoveEventQueue((INetworkOutboundMessageQueue)item);
            case NetworkInbound:  return NetworkInboundMessageQueueGroup.StopRemoveEventQueue((INetworkInboundMessageQueue)item);
            case Event:           return EventMessageQueueGroup.StopRemoveEventQueue(item);
            case Worker:          return WorkerMessageQueueGroup.StopRemoveEventQueue(item);
            case Custom:          return CustomMessageQueueGroup.StopRemoveEventQueue(item);
            case DataAccess:      return DataAccessMessageQueueGroup.StopRemoveEventQueue(item);
            default:
                var message = $"Can not add queue of type {queueGroup} to EventBus ";
                Logger.Warn(message);
                throw new ArgumentException(message);
        }
    }

    public int Count =>
        EventMessageQueueGroup.Count + WorkerMessageQueueGroup.Count + NetworkInboundMessageQueueGroup.Count
      + NetworkOutboundMessageQueueGroup.Count + CustomMessageQueueGroup.Count;

    public bool HasStarted =>
        EventMessageQueueGroup.HasStarted || WorkerMessageQueueGroup.HasStarted || NetworkInboundMessageQueueGroup.HasStarted ||
        NetworkOutboundMessageQueueGroup.HasStarted || CustomMessageQueueGroup.HasStarted;

    public IMessageQueueGroupContainer AddRange(IEnumerable<IMessageQueue> eventQueue)
    {
        foreach (var queue in eventQueue) Add(queue);

        return this;
    }

    public IMessageQueueTypeGroup           EventMessageQueueGroup           { get; }
    public IMessageQueueTypeGroup           WorkerMessageQueueGroup          { get; }
    public INetworkInboundMessageTypeGroup  NetworkInboundMessageQueueGroup  { get; }
    public INetworkOutboundMessageTypeGroup NetworkOutboundMessageQueueGroup { get; }
    public IMessageQueueTypeGroup           CustomMessageQueueGroup          { get; }
    public IMessageQueueTypeGroup           DataAccessMessageQueueGroup      { get; }

    public IMessageQueueTypeGroup SelectEventQueueGroup(IMessageQueue childOfGroup) => SelectEventQueueGroup(childOfGroup.QueueType);

    public IMessageQueueTypeGroup SelectEventQueueGroup(MessageQueueType selector)
    {
        return selector switch
               {
                   NetworkOutbound => NetworkOutboundMessageQueueGroup
                 , NetworkInbound  => NetworkInboundMessageQueueGroup
                 , Event           => EventMessageQueueGroup
                 , Worker          => WorkerMessageQueueGroup
                 , Custom          => CustomMessageQueueGroup
                 , DataAccess      => DataAccessMessageQueueGroup
                 , _ => throw new ArgumentException
                       ($"Unexpected selector EventQueueType selector value '{selector}' when selecting a specific event group type")
               };
    }

    public IMessageQueueList<IMessageQueue> SelectEventQueues(MessageQueueType selector)
    {
        var result = Recycler.ThreadStaticRecycler.Borrow<MessageQueueList<IMessageQueue>>();
        if (selector.IsNetworkOutbound()) result.AddRange(NetworkOutboundMessageQueueGroup);
        if (selector.IsNetworkInbound()) result.AddRange(NetworkInboundMessageQueueGroup);
        if (selector.IsEvent()) result.AddRange(EventMessageQueueGroup);
        if (selector.IsWorker()) result.AddRange(WorkerMessageQueueGroup);
        if (selector.IsCustom()) result.AddRange(CustomMessageQueueGroup);
        if (selector.IsDataAccess()) result.AddRange(DataAccessMessageQueueGroup);

        return result;
    }

    public async ValueTask AddListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , MessageQueueType onQueueTypes)
    {
        var asyncDispatchResults = sender.Context.PooledRecycler.Borrow<ReusableList<ValueTask<IDispatchResult>>>();
        foreach (var messageQueue in SelectEventQueues(onQueueTypes))
            asyncDispatchResults.Add(messageQueue.AddListenSubscribeInterceptor(sender, interceptor));

        foreach (var asyncDispatchResult in asyncDispatchResults) await asyncDispatchResult;
    }

    public async ValueTask RemoveListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , MessageQueueType onQueueTypes)
    {
        var asyncDispatchResults = sender.Context.PooledRecycler.Borrow<ReusableList<ValueTask<IDispatchResult>>>();
        foreach (var messageQueue in SelectEventQueues(onQueueTypes))
            asyncDispatchResults.Add(messageQueue.RemoveListenSubscribeInterceptor(sender, interceptor));

        foreach (var asyncDispatchResult in asyncDispatchResults) await asyncDispatchResult;
    }
}
