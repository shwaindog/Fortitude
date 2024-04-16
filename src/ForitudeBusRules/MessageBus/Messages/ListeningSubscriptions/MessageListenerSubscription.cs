#region

using FortitudeBusRules.MessageBus.Tasks;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.MessageBus.Messages.ListeningSubscriptions;

public interface IMessageListenerSubscription : IDisposable
{
    string SubscriberId { get; }
    IEventContext RegisteredContext { get; }
    IListeningRule SubscriberRule { get; }
    string PublishAddress { get; }
    IAddressMatcher? Matcher { get; }
    Func<Message, ValueTask> Handler { get; }

    event Action<IListeningRule, string>? Unsubscribed;
}

public class MessageListenerSubscription<TPayLoad, TResponse> : IMessageListenerSubscription
{
    private static readonly IFLogger Logger
        = FLoggerFactory.Instance.GetLogger(typeof(MessageListenerSubscription<TPayLoad, TResponse>));

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
    public IEventContext RegisteredContext => SubscriberRule.Context;
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
    public Func<Message, ValueTask> Handler { get; private set; } = null!;

    public void Dispose()
    {
        Unsubscribed?.Invoke(SubscriberRule, PublishAddress);
    }

    public event Action<IListeningRule, string>? Unsubscribed;

    public void SetHandlerFromSpecificMessageHandler(
        Func<IRespondingMessage<TPayLoad, TResponse>, ValueTask<TResponse>> wrapHandler)
    {
        async ValueTask HandlerWrapper(Message message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);

                try
                {
                    var response = await wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, ValueTask<{1}>> wrapHandler) MessageType.Publish on Rule {2} caught exception {3}"
                        ,
                        typeof(TPayLoad).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                if (message.Response is not IResponseValueTaskSource<TResponse> typeResponse) return;
                try
                {
                    var response = await wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    typeResponse.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, ValueTask<{1}>> wrapHandler) MessageType.RequestResponse on Rule {2} caught exception {3}"
                        ,
                        typeof(TPayLoad).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    typeResponse.SetException(ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
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
        Func<IRespondingMessage<TPayLoad, TResponse>, Task<TResponse>> wrapHandler)
    {
        async ValueTask HandlerWrapper(Message message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                try
                {
                    var response = await wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, Task<{1}>> wrapHandler) MessageType.Publish on Rule {2} caught exception {3}"
                        ,
                        typeof(TPayLoad).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                if (message.Response is not IResponseValueTaskSource<TResponse> typeResponse) return;
                try
                {
                    var response = await wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    typeResponse.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, Task<{1}>> wrapHandler) MessageType.RequestResponse on Rule {2} caught exception {3}"
                        ,
                        typeof(TPayLoad).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    typeResponse.SetException(ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
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
        Func<IRespondingMessage<TPayLoad, TResponse>, TResponse> wrapHandler)
    {
        ValueTask HandlerWrapper(Message message)
        {
            if (!message.RuleFilter(SubscriberRule)) return ValueTask.CompletedTask;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                try
                {
                    var response = wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, {1}> wrapHandler) MessageType.Publish on Rule {2} caught exception {3}"
                        ,
                        typeof(TPayLoad).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                if (message.Response is not IResponseValueTaskSource<TResponse> requestResponse) return ValueTask.CompletedTask;
                try
                {
                    var response = wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    requestResponse.TrySetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, {1}> wrapHandler) MessageType.RequestResponse on Rule {2} caught exception {3}"
                        ,
                        typeof(TPayLoad).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    requestResponse.SetException(ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }

            return ValueTask.CompletedTask;
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(
        Action<IMessage<TPayLoad>> wrapHandler)
    {
        ValueTask HandlerWrapper(Message message)
        {
            if (!message.RuleFilter(SubscriberRule)) return ValueTask.CompletedTask;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IMessage<TPayLoad> typeMessage = message.BorrowCopy<TPayLoad>(RegisteredContext);
                try
                {
                    wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                }
                catch (Exception ex)
                {
                    Logger.Warn(
                        "SetHandlerFromSpecificMessageHandler(Action<IMessage<{0}>> wrapHandler) MessageType.RequestResponse on Rule {1} caught exception {2}"
                        ,
                        typeof(TPayLoad).Name, SubscriberRule.FriendlyName, ex);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
            }

            return ValueTask.CompletedTask;
        }

        Handler = HandlerWrapper;
    }
}
