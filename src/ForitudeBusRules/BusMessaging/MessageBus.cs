// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeBusRules.Config;
using FortitudeBusRules.Injection;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.BusMessaging;

public interface IMessageBus
{
    IDependencyResolver DependencyResolver { get; set; }

    IBusNetworkResolver BusNetworkResolver { get; }

    void Publish<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);
    void Publish<T>(IRule sender, string publishAddress, T msg);
    void Send<T>(IRule sender, T msg, MessageType messageType, DispatchOptions dispatchOptions);
    void Send<T>(IRule sender, T msg, MessageType messageType);

    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);
    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg);

    ValueTask<TU> RequestAsync<T, TU>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions);
    ValueTask<TU> RequestAsync<T, TU>(IRule sender, string publishAddress, T msg);

    void DeployRule(IRule sender, IRule toDeployRule, DeploymentOptions options);
    void DeployRule(IRule sender, IRule toDeployRule);

    ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, IRule toDeployRule, DeploymentOptions options);
    ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, IRule toDeployRule);
    ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, Type toDeployRuleType, DeploymentOptions options);
    ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, Type toDeployRuleType);

    ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeployRule);

    ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Action<IBusMessage<TPayload>> handler);

    ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Action<TPayload> handler);

    ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Func<IBusMessage<TPayload>, ValueTask> handler);

    ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask<TResponse>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, TResponse> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, Task<TResponse>> handler);

    ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Action<IBusMessage<TPayload>> handler);

    ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Action<TPayload> handler);

    ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Func<IBusMessage<TPayload>, ValueTask> handler);

    ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask<TResponse>> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, TResponse> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler);

    ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, Task<TResponse>> handler);


    ValueTask<ISubscription> AddListenSubscribeInterceptor(IRule sender, IListenSubscribeInterceptor interceptor, MessageQueueType onQueueTypes);
    IEnumerable<IRule>       RulesMatching(Func<IRule, bool> predicate);
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
    private IBusNetworkResolver? busIOResolver;

    public MessageBus(BusRulesConfig config)
    {
        DependencyResolver = config.Resolver ?? new BasicDependencyResolver();
        AllMessageQueues   = new MessageQueueGroupContainer(this, config);
    }

    internal MessageBus
    (Func<IConfigureMessageBus, IMessageQueueGroupContainer> createGroupContainerFunc
      , IDependencyResolver? dependencyResolver = null)
    {
        DependencyResolver = dependencyResolver ?? new BasicDependencyResolver();
        AllMessageQueues   = createGroupContainerFunc(this);
    }

    public IMessageQueueGroupContainer AllMessageQueues { get; }

    public IBusNetworkResolver BusNetworkResolver => busIOResolver ??= new BusNetworkResolver(this);

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

    public ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, Type toDeployRuleType, DeploymentOptions options)
    {
        var resolvedRuled = DependencyResolver.Resolve<IRule>();
        return DeployRuleAsync(sender, resolvedRuled, options);
    }

    public ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, Type toDeployRuleType)
    {
        var resolvedRuled = DependencyResolver.Resolve<IRule>();
        return DeployRuleAsync(sender, resolvedRuled, new DeploymentOptions());
    }

    public void DeployRule(IRule sender, IRule toDeployRule, DeploymentOptions options)
    {
        AllMessageQueues.LaunchRule(sender, toDeployRule, options);
    }

    public void DeployRule(IRule sender, IRule toDeployRule)
    {
        AllMessageQueues.LaunchRule(sender, toDeployRule, new DeploymentOptions());
    }

    public ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, IRule toDeployRule, DeploymentOptions options) =>
        AllMessageQueues.LaunchRuleAsync(sender, toDeployRule, options);

    public ValueTask<IRuleDeploymentLifeTime> DeployRuleAsync(IRule sender, IRule toDeployRule) =>
        AllMessageQueues.LaunchRuleAsync(sender, toDeployRule, new DeploymentOptions());

    public void Publish<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        AllMessageQueues.Publish(sender, publishAddress, msg, dispatchOptions);

    public void Publish<T>(IRule sender, string publishAddress, T msg) =>
        AllMessageQueues.Publish(sender, publishAddress, msg, new DispatchOptions());

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        AllMessageQueues.PublishAsync(sender, publishAddress, msg, dispatchOptions);

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg) =>
        AllMessageQueues.PublishAsync(sender, publishAddress, msg, new DispatchOptions());

    public void Send<T>(IRule sender, T msg, MessageType messageType, DispatchOptions dispatchOptions)
    {
        AllMessageQueues.Send(sender, msg, messageType, dispatchOptions);
    }

    public void Send<T>(IRule sender, T msg, MessageType messageType)
    {
        AllMessageQueues.Send(sender, msg, messageType, new DispatchOptions());
    }

    public IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate) => AllMessageQueues.RulesMatching(predicate);

    public ValueTask<TU> RequestAsync<T, TU>(IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        AllMessageQueues.RequestAsync<T, TU>(sender, publishAddress, msg, dispatchOptions);

    public ValueTask<TU> RequestAsync<T, TU>(IRule sender, string publishAddress, T msg) =>
        AllMessageQueues.RequestAsync<T, TU>(sender, publishAddress, msg, new DispatchOptions());

    public ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Action<IBusMessage<TPayload>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Action<TPayload> handler)
    {
        void UnwrapPayload(IBusMessage<TPayload> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return RegisterListener(rule, publishAddress, (Action<IBusMessage<TPayload>>)UnwrapPayload);
    }

    public ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Func<IBusMessage<TPayload>, ValueTask> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterListener<TPayload>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask> handler)
    {
        ValueTask UnwrapPayload(IBusMessage<TPayload> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return RegisterListener(rule, publishAddress, (Func<IBusMessage<TPayload>, ValueTask>)UnwrapPayload);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask<TResponse>> handler)
    {
        ValueTask<TResponse> UnwrapPayload
            (IBusRespondingMessage<TPayload, TResponse> messageWithPayLoad) =>
            handler(messageWithPayLoad.Payload.Body());

        return RegisterRequestListener(rule, publishAddress, (Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>>)UnwrapPayload);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, TResponse> handler)
    {
        TResponse UnwrapPayload(IBusRespondingMessage<TPayload, TResponse> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return RegisterRequestListener(rule, publishAddress, (Func<IBusRespondingMessage<TPayload, TResponse>, TResponse>)UnwrapPayload);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayloadBody(msgListener, rule, MessageType.ListenerSubscribe, publishAddress);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, Task<TResponse>> handler)
    {
        Task<TResponse> UnwrapPayload(IBusRespondingMessage<TPayload, TResponse> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return RegisterRequestListener(rule, publishAddress, (Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>>)UnwrapPayload);
    }

    public async ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Action<IBusMessage<TPayload>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();
        processorRegistry.Result = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.Result.SentTime                = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.QueueTimer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync
                (msgListener, rule, MessageType.ListenerSubscribe, publishAddress, processorRegistry);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Action<TPayload> handler)
    {
        void UnwrapPayload(IBusMessage<TPayload> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return await RegisterListenerAsync(rule, publishAddress, (Action<IBusMessage<TPayload>>)UnwrapPayload);
    }

    public async ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Func<IBusMessage<TPayload>, ValueTask> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, object>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();
        processorRegistry.Result = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.Result.SentTime                = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.QueueTimer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync
                (msgListener, rule, MessageType.ListenerSubscribe, publishAddress, processorRegistry);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterListenerAsync<TPayload>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask> handler)
    {
        ValueTask UnwrapPayload(IBusMessage<TPayload> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return await RegisterListenerAsync(rule, publishAddress, (Func<IBusMessage<TPayload>, ValueTask>)UnwrapPayload);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();
        processorRegistry.Result = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.Result.SentTime                = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.QueueTimer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync
                (msgListener, rule, MessageType.ListenerSubscribe, publishAddress, processorRegistry);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, ValueTask<TResponse>> handler)
    {
        ValueTask<TResponse> UnwrapResponsePayload
            (IBusRespondingMessage<TPayload, TResponse> messageWithPayLoad) =>
            handler(messageWithPayLoad.Payload.Body());

        return await RegisterRequestListenerAsync(rule, publishAddress
                                                , (Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>>)UnwrapResponsePayload);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();
        processorRegistry.Result = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.Result.SentTime                = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.QueueTimer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync
                (msgListener, rule, MessageType.ListenerSubscribe, publishAddress, processorRegistry);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, TResponse> handler)
    {
        TResponse UnwrapPayload(IBusRespondingMessage<TPayload, TResponse> messageWithPayLoad) => handler(messageWithPayLoad.Payload.Body());
        return await RegisterRequestListenerAsync(rule, publishAddress, (Func<IBusRespondingMessage<TPayload, TResponse>, TResponse>)UnwrapPayload);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerRegistration<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        var processorRegistry = rule.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();
        processorRegistry.Result = rule.Context.PooledRecycler.Borrow<DispatchResult>();
        processorRegistry.IncrementRefCount();
        processorRegistry.Result.SentTime                = DateTime.Now;
        processorRegistry.ResponseTimeoutAndRecycleTimer = rule.Context.QueueTimer;
        var dispatchResult
            = await rule.Context.RegisteredOn.EnqueuePayloadBodyWithStatsAsync
                (msgListener, rule, MessageType.ListenerSubscribe, publishAddress, processorRegistry);
        return new MessageListenerSubscription(rule, publishAddress, subscriberId, dispatchResult);
    }

    public async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
        (IListeningRule rule, string publishAddress, Func<TPayload, Task<TResponse>> handler)
    {
        Task<TResponse> UnwrapPayload
            (IBusRespondingMessage<TPayload, TResponse> messageWithPayLoad) =>
            handler(messageWithPayLoad.Payload.Body());

        return await RegisterRequestListenerAsync(rule, publishAddress
                                                , (Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>>)UnwrapPayload);
    }

    public async ValueTask<ISubscription> AddListenSubscribeInterceptor
        (IRule sender, IListenSubscribeInterceptor interceptor, MessageQueueType onQueueTypes)
    {
        var addressInterceptorSubscription = sender.Context.PooledRecycler.Borrow<QueueTypeInterceptorSubscription>();
        addressInterceptorSubscription.RegisteredInterceptor = interceptor;
        addressInterceptorSubscription.RegistrationRule      = sender;
        addressInterceptorSubscription.RegisteredQueueTypes  = onQueueTypes;
        addressInterceptorSubscription.ConfigureMessageBus   = this;

        await AllMessageQueues.AddListenSubscribeInterceptor(sender, interceptor, onQueueTypes);

        return addressInterceptorSubscription;
    }

    public ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeployRule) =>
        toUndeployRule.Context.RegisteredOn.StopRuleAsync(sender, toUndeployRule);
}
