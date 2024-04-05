#region

using FortitudeBusRules.MessageBus.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messaging;

#endregion

namespace FortitudeBusRules.Rules;

public static class RuleExtensions
{
    public static ValueTask<IDispatchResult> PublishAsync<T>(this IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions) =>
        sender.Context.EventBus.PublishAsync(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<RequestResponse<U>> RequestAsync<T, U>(this IRule sender, string publishAddress, T msg
        , DispatchOptions dispatchOptions) =>
        sender.Context.EventBus.RequestAsync<T, U>(sender, publishAddress, msg, dispatchOptions);

    public static ValueTask<IDispatchResult>
        DeployRuleAsync(this IRule sender, IRule rule, DeploymentOptions options) =>
        sender.Context.EventBus.DeployRuleAsync(sender, rule, options);

    public static ISubscription RegisterListener<TPayload>(this IListeningRule rule, string publishAddress
        , Action<IMessage<TPayload>> handler) =>
        rule.Context.EventBus.RegisterListener(rule, publishAddress, handler);

    public static ISubscription RegisterRequestListener<TPayload, TResponse>(this IListeningRule rule
        , string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> handler) =>
        rule.Context.EventBus.RegisterRequestListener(rule, publishAddress, handler);

    public static ISubscription RegisterRequestListener<TPayload, TResponse>(this IListeningRule rule
        , string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, TResponse> handler) =>
        rule.Context.EventBus.RegisterRequestListener(rule, publishAddress, handler);

    public static ISubscription RegisterRequestListener<TPayload, TResponse>(this IListeningRule rule
        , string publishAddress
        , Func<IRespondingMessage<TPayload, TResponse>, Task<TResponse>> handler) =>
        rule.Context.EventBus.RegisterRequestListener(rule, publishAddress, handler);
}
