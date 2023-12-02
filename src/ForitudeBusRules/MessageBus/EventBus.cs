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

    ISubscription RegisterListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);
}

public interface IDispatchOptions { }

public class EventBus : IEventBus
{
    // ideal minimum number of cores is 6 to avoid excessive context switching
    public const int ReservedCoresForIO = 3; // inbound, outbound, logging
    public const int MinimumEventQueues = 2; // at least two
    public const int MinimumWorkerQueues = 1; // at least one

    private readonly List<EventQueue> allQueues = new();
    private readonly List<EventQueue> eventQueues = new();
    private readonly EventQueue ioInboundQueue;
    private readonly EventQueue ioOutboundQueue;
    private readonly List<EventQueue> workerQueues = new();
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
        var dispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.DispatchResult = dispatchResult;
        foreach (var eventQueue in allQueues.Where(eq => eq.IsListeningToAddress(publishAddress)))
        {
            count++;
            processorRegistry.IncrementRefCount();
            eventQueue.EnqueuePayloadWithStats(msg, sender, processorRegistry, publishAddress);
        }

        if (count == 0) throw new KeyNotFoundException($"Address: {publishAddress} has no registered listeners");
        processorRegistry.DecrementRefCount();

        return new ValueTask<IDispatchResult>(processorRegistry, (short)(processorRegistry.Version + 1));
    }

    public ISubscription RegisterListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayload(msgListener, rule, publishAddress, MessageType.MessageListener);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
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
        var dispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.DispatchResult = dispatchResult;
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

    public int ResolveLeastBusy(List<EventQueue> queues)
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
