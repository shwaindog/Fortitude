#region

using Fortitude.EventProcessing.BusRules.Config;
using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus;

public interface IEventBus
{
    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , IDispatchOptions dispatchOptions);

    ValueTask<IResponse<U>> RequestAsync<T, U>(string publishAddress, T msg, IDispatchOptions dispatchOptions);
    ValueTask<string> DeployRuleAsync(IRule rule, IDeploymentOptions options);
    ValueTask<string> DeployRuleAsync(Type ruleType, IDeploymentOptions options);

    ISubscription RegisterListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);
}

public interface IDispatchOptions { }

public class EventBus : IEventBus
{
    private readonly List<EventQueue> allQueues = new();
    private readonly List<EventQueue> eventQueues = new();
    private readonly List<EventQueue> workerQueues = new();

    public EventBus(BusRulesConfig config)
    {
        var eventQueueNum = Math.Min(config.MaxEventLoops, Environment.ProcessorCount);
        var queueSize = Math.Max(config.EventQueueSize, 1);
        for (var i = 1; i <= eventQueueNum; i++)
            eventQueues.Add(new EventQueue(this, EventQueueType.Event, i, queueSize));
        var workerQueueNum = Math.Min(config.MaxWorkerLoops, Environment.ProcessorCount);
        for (var i = 1; i <= eventQueueNum; i++)
            eventQueues.Add(new EventQueue(this, EventQueueType.Worker, i, queueSize));
        allQueues.AddRange(eventQueues);
        allQueues.AddRange(workerQueues);
    }

    public ValueTask<IResponse<U>> RequestAsync<T, U>(string publishAddress, T msg
        , IDispatchOptions dispatchOptions) =>
        throw new NotImplementedException();

    public ValueTask<string> DeployRuleAsync(Type ruleType, IDeploymentOptions options) =>
        throw new NotImplementedException();

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , IDispatchOptions dispatchOptions)
    {
        var count = 0;
        var processorRegistry = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        var dispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.SetResponse(dispatchResult);
        foreach (var eventQueue in allQueues.Where(eq => eq.IsListeningToAddress(publishAddress)))
        {
            count++;
            processorRegistry.IncrementRefCount();
            eventQueue.EnqueuePayloadWithStats(msg, sender, processorRegistry, publishAddress);
        }

        return new ValueTask<IDispatchResult>(processorRegistry, (short)(processorRegistry.Version + 1));
    }

    public ValueTask<string> DeployRuleAsync(IRule rule, IDeploymentOptions options) =>
        throw new NotImplementedException();

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
}
