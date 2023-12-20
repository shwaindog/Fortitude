#region

using FortitudeBusRules.Config;
using FortitudeBusRules.Injection;
using FortitudeBusRules.MessageBus.Messages;
using FortitudeBusRules.MessageBus.Pipelines;
using FortitudeBusRules.MessageBus.Pipelines.Groups;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;

#endregion

namespace FortitudeBusRules.MessageBus;

public interface IEventBus
{
    IDependencyResolver DependencyResolver { get; set; }

    ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions);

    ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions);

    ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, IRule toDeployRule, DeploymentOptions options);
    ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, Type toDeployRuleType, DeploymentOptions options);

    ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeployRule);

    ISubscription RegisterListener<TPayload>(IListeningRule rule, string publishAddress
        , Action<IMessage<TPayload>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, TResponse> handler);

    ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler);
}

public interface IConfigureEventBus : IEventBus
{
    void Start();
    void Stop();
    void StartNewEventQueue(IEventQueue freshEventQueue);
}

public class EventBus : IConfigureEventBus
{
    private readonly IEventQueueGroupContainer allEventQueues;

    public EventBus(BusRulesConfig config)
    {
        DependencyResolver = config.Resolver ?? new BasicDependencyResolver();
        allEventQueues = new EventQueueGroupContainer(this, config);
    }

    internal EventBus(Func<IEventBus, IEventQueueGroupContainer> createGroupContainerFunc
        , IDependencyResolver? dependencyResolver = null)
    {
        DependencyResolver = dependencyResolver ?? new BasicDependencyResolver();
        allEventQueues = createGroupContainerFunc(this);
    }

    public void Start()
    {
        allEventQueues.Start();
    }

    public void Stop()
    {
        allEventQueues.Stop();
    }

    public void StartNewEventQueue(IEventQueue freshEventQueue)
    {
        allEventQueues.Add(freshEventQueue);
        if (!freshEventQueue.IsRunning) freshEventQueue.Start();
    }

    public IDependencyResolver DependencyResolver { get; set; }

    public ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, Type toDeployRuleType, DeploymentOptions options)
    {
        var resolvedRuled = DependencyResolver.Resolve<IRule>();
        return DeployRuleAsync(sender, resolvedRuled, options);
    }

    public ValueTask<IDispatchResult> PublishAsync<T>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions) =>
        allEventQueues.PublishAsync(sender, publishAddress, msg, dispatchOptions);

    public ValueTask<IDispatchResult> DeployRuleAsync(IRule sender, IRule toDeployRule, DeploymentOptions options) =>
        allEventQueues.LaunchRule(sender, toDeployRule, options);

    public ValueTask<RequestResponse<U>> RequestAsync<T, U>(IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions) =>
        allEventQueues.RequestAsync<T, U>(sender, publishAddress, msg, dispatchOptions);

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

    public ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, TResponse> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayload(msgListener, rule, publishAddress, MessageType.ListenerSubscribe);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ISubscription RegisterRequestListener<TPayload, TResponse>(IListeningRule rule, string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler)
    {
        var subscriberId = $"{rule.FriendlyName}_{publishAddress}_{rule.Id}";
        var msgListener
            = new MessageListenerSubscription<TPayload, TResponse>(rule, publishAddress, subscriberId);
        rule.IncrementLifeTimeCount();
        msgListener.SetHandlerFromSpecificMessageHandler(handler);
        rule.Context.RegisteredOn.EnqueuePayload(msgListener, rule, publishAddress, MessageType.ListenerSubscribe);
        return new MessageListenerUnsubscribe(rule, publishAddress, subscriberId);
    }

    public ValueTask<IDispatchResult> UndeployRuleAsync(IRule sender, IRule toUndeployRule) =>
        toUndeployRule.Context.RegisteredOn.StopRuleAsync(sender, toUndeployRule);
}
