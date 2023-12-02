#region

using Fortitude.EventProcessing.BusRules.Config;
using Fortitude.EventProcessing.BusRules.Injection;
using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.MessageBus.Pipelines;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus;

public interface IEventBus
{
    IDependencyResolver DependencyResolver { get; set; }

    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , IDispatchOptions dispatchOptions);

    ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , IDispatchOptions dispatchOptions);

    ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, IRule rule, IDeploymentOptions options);
    ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, Type ruleType, IDeploymentOptions options);

    ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule rule);

    ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Action<IMessage<TPayload>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);
}

public class EventBus : IEventBus
{
    // ideal minimum number of cores is 6 to avoid excessive context switching
    public const int ReservedCoresForIO = 3; // inbound, outbound, logging
    public const int MinimumEventQueues = 2; // at least two
    public const int MinimumWorkerQueues = 1; // at least one

    private readonly List<IEventQueue> allQueues = new();
    private readonly List<IEventQueue> eventQueues = new();
    private readonly IEventQueue ioInboundQueue;
    private readonly IEventQueue ioOutboundQueue;
    private readonly List<IEventQueue> workerQueues = new();
    private int lastRequestIndex;

    public EventBus(BusRulesConfig config)
    {
        DependencyResolver = config.Resolver ?? new BasicDependencyResolver();
        var queueSize = Math.Max(config.EventQueueSize, 1);
        ioInboundQueue = new EventQueue(this, EventQueueType.IOInbound, 1, queueSize);
        ioOutboundQueue = new EventQueue(this, EventQueueType.IOOutbound, 1, queueSize);
        var eventQueueNum = Math.Max(Math.Min(config.MaxEventLoops, Environment.ProcessorCount - ReservedCoresForIO)
            , MinimumEventQueues);

        for (var i = 1; i <= eventQueueNum; i++)
            eventQueues.Add(new EventQueue(this, EventQueueType.Event, i, queueSize));
        var workerQueueNum
            = Math.Max(Math.Min(config.MaxWorkerLoops, Environment.ProcessorCount - ReservedCoresForIO)
                , MinimumWorkerQueues);
        for (var i = 1; i <= eventQueueNum; i++)
            eventQueues.Add(new EventQueue(this, EventQueueType.Worker, i, queueSize));
        allQueues.Add(ioOutboundQueue);
        allQueues.AddRange(eventQueues);
        allQueues.AddRange(workerQueues);
        allQueues.Add(ioInboundQueue);
    }

    internal EventBus(Func<IEventBus, List<IEventQueue>?>? eventQueues = null
        , IDependencyResolver? dependencyResolver = null
        , Func<IEventBus, List<IEventQueue>?>? workerQueues = null,
        Func<IEventBus, IEventQueue?>? ioInboundQueue = null, Func<IEventBus, IEventQueue?>? ioOutboundQueue = null)
    {
        DependencyResolver = dependencyResolver ?? new BasicDependencyResolver();
        this.eventQueues = eventQueues != null ? eventQueues(this) ?? new List<IEventQueue>() : new List<IEventQueue>();
        this.workerQueues = workerQueues != null ?
            workerQueues(this) ?? new List<IEventQueue>() :
            new List<IEventQueue>();
        this.ioInboundQueue = ioInboundQueue != null ? ioInboundQueue(this) ?? null! : null!;
        this.ioOutboundQueue = ioOutboundQueue != null ? ioOutboundQueue(this) ?? null! : null!;
        allQueues.AddRange(this.eventQueues);
        allQueues.AddRange(this.workerQueues);
        if (ioInboundQueue != null) allQueues.Add(this.ioInboundQueue);
        if (ioOutboundQueue != null) allQueues.Add(this.ioOutboundQueue);
    }

    public IDependencyResolver DependencyResolver { get; set; }

    public ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, Type ruleType, IDeploymentOptions options)
    {
        var resolvedRuled = DependencyResolver.Resolve<IRule>();
        return DeployRuleAsync(sender, resolvedRuled, options);
    }

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , IDispatchOptions dispatchOptions)
    {
        var count = 0;
        var processorRegistry = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult ??= sender.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.Reset();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.RecycleTimer = sender.Context.Timer;
        foreach (var eventQueue in allQueues.Where(eq => eq.IsListeningToAddress(publishAddress)))
        {
            count++;
            processorRegistry.IncrementRefCount();
            eventQueue.EnqueuePayloadWithStats(msg, sender, processorRegistry, publishAddress);
        }

        if (count == 0) throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
        processorRegistry.DecrementRefCount();

        return processorRegistry.GenerateValueTask();
    }

    public ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, IRule rule, IDeploymentOptions options)
    {
        if (options.IsIOOutbound) return ioOutboundQueue.LaunchRule(sender, rule);
        if (options.IsIOInbound) return ioInboundQueue.LaunchRule(sender, rule);
        if (options.IsWorker)
        {
            var workQueueIndex = ResolveLeastBusy(workerQueues);
            return workerQueues[workQueueIndex].LaunchRule(sender, rule);
        }

        var eventQueueIndex = ResolveLeastBusy(eventQueues);
        return eventQueues[eventQueueIndex].LaunchRule(sender, rule);
    }

    public ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , IDispatchOptions dispatchOptions)
    {
        var processorRegistry = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult ??= sender.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.Reset();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.RecycleTimer = sender.Context.Timer;
        for (var i = lastRequestIndex + 1; i < lastRequestIndex + allQueues.Count; i++)
        {
            var currIndex = i % allQueues.Count;
            var currQueue = allQueues[currIndex];
            if (currQueue.IsListeningToAddress(publishAddress))
            {
                lastRequestIndex = currIndex;
                return currQueue.RequestFromPayload<T, U>(msg, sender, processorRegistry, publishAddress);
            }
        }

        throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
    }

    public ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Action<IMessage<TPayload>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayload(msgListener, rule, publishAddress, MessageType.ListenerSubscribe);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayload(msgListener, rule, publishAddress, MessageType.ListenerSubscribe);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeploy) =>
        toUndeploy.Context.RegisteredOn.StopRule(sender, toUndeploy);

    public int ResolveLeastBusy(List<IEventQueue> queues)
    {
        var minIndex = 0;
        var currentMinMessageCount = uint.MaxValue;

        for (var i = 0; i < queues.Count; i++)
        {
            var checkQueue = queues[i];
            var checkQueueMessageCount = checkQueue.NumOfMessagesReceivedRecently();
            if (checkQueueMessageCount < currentMinMessageCount)
            {
                minIndex = i;
                currentMinMessageCount = checkQueueMessageCount;
            }
        }

        return minIndex;
    }
}
