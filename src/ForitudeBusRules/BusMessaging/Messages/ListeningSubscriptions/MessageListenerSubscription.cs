#region

using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface IMessageListenerSubscription : IDisposable
{
    string SubscriberId { get; }
    IQueueContext RegisteredContext { get; }
    IListeningRule SubscriberRule { get; }
    string PublishAddress { get; }
    IAddressMatcher? Matcher { get; }
    Func<BusMessage, ValueTask> Handler { get; }

    event Action<IRule, string>? Unsubscribed;
}

public class MessageListenerSubscription<TPayload, TResponse> : IMessageListenerSubscription
{
    private static readonly IFLogger Logger
        = FLoggerFactory.Instance.GetLogger(typeof(MessageListenerSubscription<TPayload, TResponse>));

    private string publishAddress = null!;

    public MessageListenerSubscription() { }

    public MessageListenerSubscription(IListeningRule subscriberRule, string publishAddress, string subscriberId) :
        this()
    {
        SubscriberId = subscriberId;
        SubscriberRule = subscriberRule;
        PublishAddress = publishAddress;
    }

    public string SubscriberId { get; set; } = null!;
    public IQueueContext RegisteredContext => SubscriberRule.Context;
    public IListeningRule SubscriberRule { get; set; } = null!;

    public string PublishAddress
    {
        get => publishAddress;
        set
        {
            publishAddress = value;
            Matcher = AddressMatcher.IsMatcherPattern(publishAddress) ? new AddressMatcher(publishAddress) : null;
        }
    }

    public IAddressMatcher? Matcher { get; set; }
    public Func<BusMessage, ValueTask> Handler { get; private set; } = null!;

    public void Dispose()
    {
        Unsubscribed?.Invoke(SubscriberRule, PublishAddress);
    }

    public event Action<IRule, string>? Unsubscribed;

    public void SetHandlerFromSpecificMessageHandler(
        Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> wrapHandler)
    {
        async ValueTask HandlerWrapper(BusMessage message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);

                try
                {
                    var response = await wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, ValueTask<{1}>> wrapHandler) " +
                        "MessageType.Publish on Rule {2} caught exception {3}",
                        typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                if (message.Response is not IResponseValueTaskSource<TResponse> typeResponse) return;
                try
                {
                    var response = await wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    typeResponse.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, ValueTask<{1}>> wrapHandler) " +
                        "MessageType.RequestResponse on Rule {2} caught exception {3}"
                        , typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    typeResponse.SetException(ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else
            {
                Logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(
        Func<IBusMessage<TPayload>, ValueTask> wrapHandler)
    {
        async ValueTask HandlerWrapper(BusMessage message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusMessage<TPayload> typeBusMessage = message.BorrowCopy<TPayload>(RegisteredContext);
                try
                {
                    await wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IBusMessage<{0}>, ValueTask> wrapHandler) " +
                        "MessageType.Publish on Rule {1} caught exception {2}"
                        , typeof(TPayload).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(
        Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> wrapHandler)
    {
        async ValueTask HandlerWrapper(BusMessage message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var response = await wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, Task<{1}>> wrapHandler) " +
                        "MessageType.Publish on Rule {2} caught exception {3}"
                        , typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                if (message.Response is not IResponseValueTaskSource<TResponse> typeResponse) return;
                try
                {
                    var response = await wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    typeResponse.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, Task<{1}>> wrapHandler) " +
                        "MessageType.RequestResponse on Rule {2} caught exception {3}"
                        , typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    typeResponse.SetException(ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else
            {
                Logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(
        Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> wrapHandler)
    {
        ValueTask HandlerWrapper(BusMessage message)
        {
            if (!message.RuleFilter(SubscriberRule)) return ValueTask.CompletedTask;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var response = wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, {1}> wrapHandler) " +
                        "MessageType.Publish on Rule {2} caught exception {3}"
                        , typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                if (message.Response is not IResponseValueTaskSource<TResponse> requestResponse) return ValueTask.CompletedTask;
                try
                {
                    var response = wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    requestResponse.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, {1}> wrapHandler)" +
                        " MessageType.RequestResponse on Rule {2} caught exception {3}"
                        , typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    requestResponse.SetException(ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }

            return ValueTask.CompletedTask;
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(
        Action<IBusMessage<TPayload>> wrapHandler)
    {
        ValueTask HandlerWrapper(BusMessage message)
        {
            if (!message.RuleFilter(SubscriberRule)) return ValueTask.CompletedTask;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IBusMessage<TPayload> typeBusMessage = message.BorrowCopy<TPayload>(RegisteredContext);
                try
                {
                    wrapHandler(typeBusMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Action<IMessage<{0}>> wrapHandler) " +
                        "MessageType.RequestResponse on Rule {1} caught exception {2}",
                        typeof(TPayload).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeBusMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }

            return ValueTask.CompletedTask;
        }

        Handler = HandlerWrapper;
    }
}
