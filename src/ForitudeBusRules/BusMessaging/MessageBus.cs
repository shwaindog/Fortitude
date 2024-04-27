#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeBusRules.Config;
using FortitudeBusRules.Injection;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.BusMessaging;

public interface IMessageBus
{
    IDependencyResolver DependencyResolver { get; set; }

    IBusIOResolver BusIOResolver { get; }

    void Publish<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);

    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions);

    ValueTask<RequestResponse<TU>> RequestAsync<T, TU>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions);

    void DeployRule(IRule sender, IRule toDeployRule, DeploymentOptions options);
    ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, IRule toDeployRule, DeploymentOptions options);
    ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, Type toDeployRuleType, DeploymentOptions options);

    ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeployRule);

    ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Action<IBusMessage<TPayload>> handler);

    ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Func<IBusMessage<TPayload>, ValueTask> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler);

    ValueTask<ISubscription> RegisterListenerAsync<TPayload>(IListeningRule rule, string publishAddress
        , Action<IBusMessage<TPayload>> handler);

    ValueTask<ISubscription> RegisterListenerAsync<TPayload>(IListeningRule rule, string publishAddress
        , Func<IBusMessage<TPayload>, ValueTask> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler);


    ValueTask AddListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor, MessageQueueType onQueueTypes);
    ValueTask RemoveListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor, MessageQueueType onQueueTypes);
    IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate);
}

public interface IConfigureMessageBus : IMessageBus
{
    IMessageQueueGroupContainer AllMessageQueues { get; }
    void Start();
    void Stop();
    void StartNewEventQueue(IMessageQueue freshMessageQueue);
}

public class MessageBus : IConfigureMessageBus
{
    private IBusIOResolver? busIOResolver;

    public MessageBus(BusRulesConfig config)
    {
        DependencyResolver = config.Resolver ?? new BasicDependencyResolver();
        AllMessageQueues = new MessageQueueGroupContainer(this, config);
    }

    internal MessageBus(Func<IConfigureMessageBus, IMessageQueueGroupContainer> createGroupContainerFunc
        , IDependencyResolver? dependencyResolver = null)
    {
        DependencyResolver = dependencyResolver ?? new BasicDependencyResolver();
        AllMessageQueues = createGroupContainerFunc(this);
    }

    public IMessageQueueGroupContainer AllMessageQueues { get; }

    public IBusIOResolver BusIOResolver => busIOResolver ??= new BusIOResolver(this);

    public void Start()
    {
        AllMessageQueues.Start();
    }

    public void Stop()
    {
        AllMessageQueues.Stop();
    }

    public void StartNewEventQueue(IMessageQueue freshMessageQueue)
    {
        AllMessageQueues.Add(freshMessageQueue);
        if (!freshMessageQueue.IsRunning) freshMessageQueue.Start();
    }

    public IDependencyResolver DependencyResolver { get; set; }

    public ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, Type toDeployRuleType, DeploymentOptions options)
    {
        var resolvedRuled = DependencyResolver.Resolve<IRule>();
        return DeployRuleAsync(sender, resolvedRuled, options);
    }

    public void Publish<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        AllMessageQueues.Publish(sender, publishAddress, msg, dispatchOptions);

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        AllMessageQueues.PublishAsync(sender, publishAddress, msg, dispatchOptions);

    public void DeployRule(IRule sender, IRule toDeployRule, DeploymentOptions options)
    {
        AllMessageQueues.LaunchRule(sender, toDeployRule, options);
    }

    public IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate) => AllMessageQueues.RulesMatching(predicate);

    public ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, IRule toDeployRule, DeploymentOptions options) =>
        AllMessageQueues.LaunchRuleAsync(sender, toDeployRule, options);

    public ValueTask<RequestResponse<TU>> RequestAsync<T, TU>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        AllMessageQueues.RequestAsync<T, TU>(sender, publishAddress, msg, dispatchOptions);

    public ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Action<IBusMessage<TPayload>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Func<IBusMessage<TPayload>, ValueTask> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public async ValueTask<ISubscription> RegisterListenerAsync<TPayload>(IListeningRule rule, string publishAddress
        , Action<IBusMessage<TPayload>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.Timer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync(msgListener, rule, MessageType.ListenerSubscribe, publishAddress
                , processorRegistry);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterListenerAsync<TPayload>(IListeningRule rule, string publishAddress
        , Func<IBusMessage<TPayload>, ValueTask> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.Timer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync(msgListener, rule, MessageType.ListenerSubscribe, publishAddress
                , processorRegistry);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.Timer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync(msgListener, rule, MessageType.ListenerSubscribe, publishAddress
                , processorRegistry);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.Timer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync(msgListener, rule, MessageType.ListenerSubscribe, publishAddress
                , processorRegistry);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<ProcessorRegistry>();
        processorRegistry.DispatchResult = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.DispatchResult.SentTime = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.Timer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync(msgListener, rule, MessageType.ListenerSubscribe, publishAddress
                , processorRegistry);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId, dispatchResult);
    }

    public ValueTask AddListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor
        , MessageQueueType onQueueTypes) =>
        AllMessageQueues.AddListenSubscribeInterceptor(sender, interceptor, onQueueTypes);

    public ValueTask RemoveListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor
        , MessageQueueType onQueueTypes) =>
        AllMessageQueues.RemoveListenSubscribeInterceptor(sender, interceptor, onQueueTypes);

    public ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeployRule) =>
        toUndeployRule.Context.RegisteredOn.StopRuleAsync(sender, toUndeployRule);
}
