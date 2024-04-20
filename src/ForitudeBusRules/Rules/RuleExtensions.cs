#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;

#endregion

namespace FortitudeBusRules.Rules;

public static class RuleExtensions
{
    public static ValueTask<IDispatchResult> PublishAsync<T>(this IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions) =>
        sender.Context.MessageBus.PublishAsync(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<RequestResponse<U>> RequestAsync<T, U>(this IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions) =>
        sender.Context.MessageBus.RequestAsync<T, U>(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<IDispatchResult>
        DeployRuleAsync(this IRule sender, IRule rule, DeploymentOptions options) =>
        sender.Context.MessageBus.DeployRuleAsync(sender, rule, options);

    public static ISubscription RegisterListener<TPayload>(this IListeningRule rule, string publishAddress
        , Action<IBusMessage<TPayload>> handler) =>
        rule.Context.MessageBus.RegisterListener(rule, publishAddress, handler);

    public static ISubscription RegisterRequestListener<TPayload, TResponse>(this IListeningRule rule
        , string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler) =>
        rule.Context.MessageBus.RegisterRequestListener(rule, publishAddress, handler);

    public static ISubscription RegisterRequestListener<TPayload, TResponse>(this IListeningRule rule
        , string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> handler) =>
        rule.Context.MessageBus.RegisterRequestListener(rule, publishAddress, handler);

    public static ISubscription RegisterRequestListener<TPayload, TResponse>(this IListeningRule rule
        , string publishAddress
        , Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler) =>
        rule.Context.MessageBus.RegisterRequestListener(rule, publishAddress, handler);
}
