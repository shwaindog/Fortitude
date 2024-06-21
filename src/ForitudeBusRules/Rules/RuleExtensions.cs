// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Messages;

#endregion

namespace FortitudeBusRules.Rules;

public static class RuleExtensions
{
    public static void Publish<T>(this IRule sender, string publishAddress, T msg, DispatchOptions dispatchOptions) =>
        sender.Context.MessageBus.Publish(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<IDispatchResult> PublishAsync<T>
    (this IRule sender, string publishAddress, T msg
      , DispatchOptions dispatchOptions) =>
        sender.Context.MessageBus.PublishAsync(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<U> RequestAsync<T, U>
    (this IRule sender, string publishAddress, T msg
      , DispatchOptions dispatchOptions) =>
        sender.Context.MessageBus.RequestAsync<T, U>(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<IDispatchResult>
        DeployRuleAsync(this IRule sender, IRule rule, DeploymentOptions options) =>
        sender.Context.MessageBus.DeployRuleAsync(sender, rule, options);

    public static void
        DeployRule(this IRule sender, IRule rule, DeploymentOptions options) =>
        sender.Context.MessageBus.DeployRule(sender, rule, options);

    public static ValueTask<IDispatchResult> UndeployRuleAsync(this IRule sender, IRule toUndeployRule) =>
        sender.Context.MessageBus.UndeployRuleAsync(sender, toUndeployRule);

    public static ISubscription RegisterListener<TPayload>
    (this IListeningRule rule, string publishAddress
      , Action<IBusMessage<TPayload>> handler) =>
        rule.AddOnStopResourceCleanup(rule.Context.MessageBus.RegisterListener(rule, publishAddress, handler));

    public static ISubscription RegisterListener<TPayload>
    (this IListeningRule rule, string publishAddress
      , Func<IBusMessage<TPayload>, ValueTask> handler) =>
        rule.AddOnStopResourceCleanup(rule.Context.MessageBus.RegisterListener(rule, publishAddress, handler));

    public static ISubscription RegisterRequestListener<TPayload, TResponse>
    (this IListeningRule rule
      , string publishAddress
      , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler) =>
        rule.AddOnStopResourceCleanup(rule.Context.MessageBus.RegisterRequestListener(rule, publishAddress, handler));

    public static ISubscription RegisterRequestListener<TPayload, TResponse>
    (this IListeningRule rule
      , string publishAddress
      , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler) =>
        rule.AddOnStopResourceCleanup(rule.Context.MessageBus.RegisterRequestListener(rule, publishAddress, handler));

    public static ISubscription RegisterRequestListener<TPayload, TResponse>
    (this IListeningRule rule
      , string publishAddress
      , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler) =>
        rule.AddOnStopResourceCleanup(rule.Context.MessageBus.RegisterRequestListener(rule, publishAddress, handler));

    public static async ValueTask<ISubscription> RegisterListenerAsync<TPayload>
    (this IListeningRule rule, string publishAddress
      , Action<IBusMessage<TPayload>> handler)
    {
        var subscription = await rule.Context.MessageBus.RegisterListenerAsync(rule, publishAddress, handler);
        rule.AddOnStopResourceCleanup(subscription);
        return subscription;
    }

    public static async ValueTask<ISubscription> RegisterListenerAsync<TPayload>
    (this IListeningRule rule, string publishAddress
      , Func<IBusMessage<TPayload>, ValueTask> handler)
    {
        var subscription = await rule.Context.MessageBus.RegisterListenerAsync(rule, publishAddress, handler);
        rule.AddOnStopResourceCleanup(subscription);
        return subscription;
    }

    public static async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
    (this IListeningRule rule, string publishAddress
      , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler)
    {
        var subscription = await rule.Context.MessageBus.RegisterRequestListenerAsync(rule, publishAddress, handler);
        rule.AddOnStopResourceCleanup(subscription);
        return subscription;
    }

    public static async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
    (this IListeningRule rule, string publishAddress
      , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler)
    {
        var subscription = await rule.Context.MessageBus.RegisterRequestListenerAsync(rule, publishAddress, handler);
        rule.AddOnStopResourceCleanup(subscription);
        return subscription;
    }

    public static async ValueTask<ISubscription> RegisterRequestListenerAsync<TPayload, TResponse>
    (this IListeningRule rule, string publishAddress
      , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler)
    {
        var subscription = await rule.Context.MessageBus.RegisterRequestListenerAsync(rule, publishAddress, handler);
        rule.AddOnStopResourceCleanup(subscription);
        return subscription;
    }

    public static async ValueTask<ISubscription> AddListenSubscribeInterceptor
    (this IRule sender, IListenSubscribeInterceptor interceptor
      , MessageQueueType onQueueTypes)
    {
        var subscription = await sender.Context.MessageBus.AddListenSubscribeInterceptor(sender, interceptor, onQueueTypes);
        sender.AddOnStopResourceCleanup(subscription);
        return subscription;
    }
}
