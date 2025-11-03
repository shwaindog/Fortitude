// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface IMessageListenerRegistration : IAsyncValueTaskDisposable
{
    IEnumerable<IListenSubscribeInterceptor> ActiveListenSubscribeInterceptors { get; }

    string           SubscriberId      { get; }
    IQueueContext    RegisteredContext { get; }
    IListeningRule   SubscriberRule    { get; }
    string           PublishAddress    { get; }
    IAddressMatcher? Matcher           { get; }
    Type             PayloadType       { get; }

    Func<BusMessageValue, ValueTask> Handler { get; }

    void AddRunListenSubscriptionInterceptor(IListenSubscribeInterceptor interceptor);
    void RemoveListenSubscriptionInterceptor(string name);

    event Func<IRule, IMessageListenerRegistration, string, ValueTask>? Unsubscribed;
}

public class MessageListenerRegistration<TPayload, TResponse> : IMessageListenerRegistration
{
    private static readonly IFLogger Logger
        = FLoggerFactory.Instance.GetLogger(typeof(MessageListenerRegistration<TPayload, TResponse>));

    private readonly IList<IListenSubscribeInterceptor> subscribeInterceptors = new List<IListenSubscribeInterceptor>();

    private string publishAddress = null!;

    public MessageListenerRegistration() { }

    public MessageListenerRegistration(IListeningRule subscriberRule, string publishAddress, string subscriberId) :
        this()
    {
        SubscriberId   = subscriberId;
        SubscriberRule = subscriberRule;
        PublishAddress = publishAddress;
    }

    public string SubscriberId { get; set; } = null!;

    public IQueueContext  RegisteredContext => SubscriberRule.Context;
    public IListeningRule SubscriberRule    { get; set; } = null!;

    public Type PayloadType => typeof(TPayload);

    public string PublishAddress
    {
        get => publishAddress;
        set
        {
            publishAddress = value;
            Matcher        = AddressMatcher.IsMatcherPattern(publishAddress) ? new AddressMatcher(publishAddress) : null;
        }
    }

    public IEnumerable<IListenSubscribeInterceptor> ActiveListenSubscribeInterceptors => subscribeInterceptors;

    public void AddRunListenSubscriptionInterceptor(IListenSubscribeInterceptor interceptor)
    {
        subscribeInterceptors.Add(interceptor);
    }

    public void RemoveListenSubscriptionInterceptor(string name)
    {
        var foundExisting = subscribeInterceptors.FirstOrDefault(lsi => lsi.Name == name);
        if (foundExisting != null) subscribeInterceptors.Remove(foundExisting);
    }

    public IAddressMatcher? Matcher { get; set; }

    public Func<BusMessageValue, ValueTask> Handler { get; private set; } = null!;

    public ValueTask DisposeAwaitValueTask { get; set; }

    public async ValueTask Dispose()
    {
        if (Unsubscribed != null) await Unsubscribed.Invoke(SubscriberRule, this, PublishAddress);
    }

    public ValueTask DisposeAsync() => Dispose();

    public event Func<IRule, IMessageListenerRegistration, string, ValueTask>? Unsubscribed;

    public void SetHandlerFromSpecificMessageHandler(Func<IBusRespondingMessage<TPayload, TResponse>, ValueTask<TResponse>> wrapHandler)
    {
        async ValueTask HandlerWrapper(BusMessageValue message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            var processRegistry = message.ProcessorRegistry;
            if (message.Type is MessageType.Publish)
            {
                processRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);

                try
                {
                    var response = await wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, ValueTask<{1}>> wrapHandler) " +
                         "listening on '{2}' with MessageType.Publish on Rule {3} when processing message IBusRespondingMessage<{4},{5}> caught exception {6}"
                       , typeof(TPayload).Name, typeof(TResponse).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                processRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var typedResponse = typeBusMessage.Response;
                    var response      = await wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                    typedResponse.SetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, ValueTask<{1}>> wrapHandler) " +
                         "listening on '{2}' with MessageType.RequestResponse on Rule {3} when processing message IBusRespondingMessage<{4},{5}> caught exception {6}"
                       , typeof(TPayload).Name, typeof(TResponse).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                    message.Response?.SetException(ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }
            else
            {
                Logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(Func<IBusMessage<TPayload>, ValueTask> wrapHandler)
    {
        async ValueTask HandlerWrapper(BusMessageValue message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                var processRegistry = message.ProcessorRegistry;
                processRegistry?.RegisterStart(SubscriberRule);
                IBusMessage<TPayload> typeBusMessage = message.BorrowCopy<TPayload>(RegisteredContext);
                try
                {
                    await wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IBusMessage<{0}>, ValueTask> wrapHandler) " +
                         "listening on '{1}' with MessageType.Publish on Rule {2} when processing message IBusRespondingMessage<{3},{4}> caught exception {5}"
                       , typeof(TPayload).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(Func<IBusRespondingMessage<TPayload, TResponse>, Task<TResponse>> wrapHandler)
    {
        async ValueTask HandlerWrapper(BusMessageValue message)
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            var processRegistry = message.ProcessorRegistry;
            if (message.Type is MessageType.Publish)
            {
                processRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var response = await wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, Task<{1}>> wrapHandler) " +
                         "listening on '{2}' with MessageType.Publish on Rule {3} when processing message IBusRespondingMessage<{4},{5}> caught exception {6}"
                       , typeof(TPayload).Name, typeof(TResponse).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                processRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var typedResponse = typeBusMessage.Response;
                    var response      = await wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                    typedResponse.SetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, Task<{1}>> wrapHandler) " +
                         "listening on '{2}' with MessageType.RequestResponse on Rule {3} when processing message IBusRespondingMessage<{4},{5}> caught exception {6}"
                       , typeof(TPayload).Name, typeof(TResponse).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                    message.Response?.SetException(ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }
            else
            {
                Logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(Func<IBusRespondingMessage<TPayload, TResponse>, TResponse> wrapHandler)
    {
        ValueTask HandlerWrapper(BusMessageValue message)
        {
            if (!message.RuleFilter(SubscriberRule)) return ValueTask.CompletedTask;
            var processRegistry = message.ProcessorRegistry;
            if (message.Type is MessageType.Publish)
            {
                processRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var response = wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                    if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, {1}> wrapHandler) " +
                         "listening on '{2}' with MessageType.Publish on Rule {3} when processing message IBusRespondingMessage<{4},{5}> caught exception {6}"
                       , typeof(TPayload).Name, typeof(TResponse).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                processRegistry?.RegisterStart(SubscriberRule);
                IBusRespondingMessage<TPayload, TResponse> typeBusMessage = message.BorrowCopy<TPayload, TResponse>(RegisteredContext);
                try
                {
                    var typedResponse = typeBusMessage.Response;
                    var response      = wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                    typedResponse.SetResult(response);
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Func<IRespondingMessage<{0}, {1}>, {1}> wrapHandler)" +
                         " MessageType.RequestResponse on Rule {2} caught exception {3}"
                       , typeof(TPayload).Name, typeof(TResponse).Name, SubscriberRule.FriendlyName, ex);
                    message.Response?.SetException(ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }

            return ValueTask.CompletedTask;
        }

        Handler = HandlerWrapper;
    }

    public void SetHandlerFromSpecificMessageHandler(Action<IBusMessage<TPayload>> wrapHandler)
    {
        ValueTask HandlerWrapper(BusMessageValue message)
        {
            if (!message.RuleFilter(SubscriberRule)) return ValueTask.CompletedTask;
            if (message.Type is MessageType.Publish)
            {
                var processRegistry = message.ProcessorRegistry;
                processRegistry?.RegisterStart(SubscriberRule);
                IBusMessage<TPayload> typeBusMessage = message.BorrowCopy<TPayload>(RegisteredContext);
                try
                {
                    wrapHandler(typeBusMessage);
                    processRegistry?.RegisterAwaiting(SubscriberRule);
                }
                catch (Exception ex)
                {
                    Logger.Warn
                        ("SetHandlerFromSpecificMessageHandler(Action<IBusMessage<{0}>> wrapHandler) " +
                         "listening on '{1}' with MessageType.Publish on Rule {2} when processing message IBusRespondingMessage<{3},{4}> caught exception {5}"
                       , typeof(TPayload).Name, PublishAddress, SubscriberRule.FriendlyName
                       , message.Payload.BodyType.Name, typeof(TResponse).Name, ex);
                }
                finally
                {
                    processRegistry?.RegisterFinish(SubscriberRule);
                    typeBusMessage.DecrementRefCount();
                }
            }

            return ValueTask.CompletedTask;
        }

        Handler = HandlerWrapper;
    }
}
