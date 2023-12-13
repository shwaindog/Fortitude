#region

using System.Threading.Tasks.Sources;
using FortitudeBusRules.MessageBus.Tasks;
using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.MessageBus.Messages;

public interface IMessageListenerSubscription
{
    string SubscriberId { get; }
    IEventContext RegisteredContext { get; }
    IListeningRule SubscriberRule { get; }
    string PublishAddress { get; }
    Action<Message> Handler { get; }
}

public class MessageListenerSubscription<TPayLoad, TResponse> : IMessageListenerSubscription
{
    private static IFLogger logger
        = FLoggerFactory.Instance.GetLogger(typeof(MessageListenerSubscription<TPayLoad, TResponse>));

    private readonly Action<Task<TResponse>, object?> onDependentTaskCompleted;
    private readonly Action<object?> onDependentValueTaskCompleted;

    public MessageListenerSubscription()
    {
        onDependentTaskCompleted = CallbackTaskCompleted;
        onDependentValueTaskCompleted = CallbackValueTaskCompleted;
    }

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
    public string PublishAddress { get; set; } = null!;
    public Action<Message> Handler { get; private set; } = null!;

    private void CallbackTaskCompleted(Task<TResponse> completed, object? state)
    {
        if (state is IMessage<TPayLoad> msgState)
        {
            if (msgState.Type is MessageType.Publish)
            {
                if (completed is { Result: IRecyclableObject recyclableObject }) recyclableObject.DecrementRefCount();
            }
            else if (msgState.Type is MessageType.RequestResponse)
            {
                var typeResponse = (IResponseValueTaskSource<TResponse>)msgState.Response;
                try
                {
                    typeResponse.TrySetResultFromAwaitingTask(completed);
                }
                catch (Exception e)
                {
                    logger.Warn("Error expected completed to be completed when this is called. Got {0}", e);
                }
            }

            msgState.ProcessorRegistry?.RegisterFinish(SubscriberRule);
            msgState.DecrementRefCount();
        }
    }

    private void CallbackValueTaskCompleted(object? state)
    {
        if (state is IMessage<TPayLoad> msgState)
        {
            if (msgState.Type is MessageType.Publish)
            {
                var typeResponse = msgState.Response as ReusableValueTaskSource<TResponse>;
                var awaitingTask = typeResponse?.AwaitingValueTask;
                if (awaitingTask is { Result: IRecyclableObject recyclableObject })
                    recyclableObject.DecrementRefCount();
            }
            else if (msgState.Type is MessageType.RequestResponse)
            {
                var typeResponse = (IResponseValueTaskSource<TResponse>)msgState.Response;
                try
                {
                    typeResponse.TrySetResultFromAwaitingTask(typeResponse.AwaitingValueTask!.Value);
                }
                catch (Exception e)
                {
                    logger.Warn("Error expected completed to be completed when this is called. Got {0}", e);
                }
            }

            msgState.ProcessorRegistry?.RegisterFinish(SubscriberRule);
            msgState.DecrementRefCount();
        }
    }

    public void SetHandlerFromSpecificMessageHandler(
        Func<IRespondingMessage<TPayLoad, TResponse>, ValueTask<TResponse>> wrapHandler)
    {
        Handler = message =>
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var response = wrapHandler(typeMessage);
                if (!response.IsCompleted)
                {
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    var responseAsReusable = response.ToReusableValueTaskSource();
                    if (responseAsReusable != null)
                    {
                        responseAsReusable.AwaitingValueTask = response;
                        responseAsReusable.OnCompleted(onDependentValueTaskCompleted, typeMessage
                            , responseAsReusable.Version, ValueTaskSourceOnCompletedFlags.None);
                    }
                    else
                    {
                        var responseAsTask = response.ToTask();
                        responseAsTask.ContinueWith(onDependentTaskCompleted, typeMessage);
                    }
                }
                else if (response.IsCompletedSuccessfully || response.IsFaulted)
                {
                    if (response is { Result: IRecyclableObject recyclableObject })
                        recyclableObject.DecrementRefCount();
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                    typeMessage.DecrementRefCount();
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var typeResponse = message.Response as IResponseValueTaskSource<TResponse>;
                var response = wrapHandler(typeMessage);
                if (typeResponse != null)
                {
                    if (!response.IsCompleted)
                    {
                        message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                        var responseAsReusable = response.ToReusableValueTaskSource();
                        if (responseAsReusable != null)
                        {
                            responseAsReusable.AwaitingValueTask = response;
                            responseAsReusable.OnCompleted(onDependentValueTaskCompleted, typeMessage
                                , responseAsReusable.Version, ValueTaskSourceOnCompletedFlags.None);
                        }
                        else
                        {
                            var responseAsTask = response.ToTask();
                            responseAsTask.ContinueWith(onDependentTaskCompleted, typeMessage);
                        }
                    }
                    else if (response.IsCompletedSuccessfully || response.IsFaulted)
                    {
                        typeResponse.TrySetResultFromAwaitingTask(response);
                        message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                        typeMessage.DecrementRefCount();
                    }
                }
            }
            else
            {
                logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        };
    }

    public void SetHandlerFromSpecificMessageHandler(
        Func<IRespondingMessage<TPayLoad, TResponse>, Task<TResponse>> wrapHandler)
    {
        Handler = message =>
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var response = wrapHandler(typeMessage);
                if (!response.IsCompleted)
                {
                    message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                    response.ContinueWith(onDependentTaskCompleted, typeMessage);
                }
                else if (response.IsCompletedSuccessfully || response.IsFaulted)
                {
                    if (response is { Result: IRecyclableObject recyclableObject })
                        recyclableObject.DecrementRefCount();

                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                    typeMessage.DecrementRefCount();
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var typeResponse = message.Response as IResponseValueTaskSource<TResponse>;
                var response = wrapHandler(typeMessage);
                if (typeResponse != null)
                {
                    if (!response.IsCompleted)
                    {
                        message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                        response.ContinueWith(onDependentTaskCompleted, typeMessage);
                    }
                    else if (response.IsCompletedSuccessfully || response.IsFaulted)
                    {
                        typeResponse.TrySetResultFromAwaitingTask(response);
                        message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                        typeMessage.DecrementRefCount();
                    }
                }
            }
            else
            {
                logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        };
    }

    public void SetHandlerFromSpecificMessageHandler(
        Func<IRespondingMessage<TPayLoad, TResponse>, TResponse> wrapHandler)
    {
        Handler = message =>
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                try
                {
                    try
                    {
                        var response = wrapHandler(typeMessage);
                        if (response is IRecyclableObject recyclableObject) recyclableObject.DecrementRefCount();
                    }
                    catch (Exception e)
                    {
                        logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}. Got {e}");
                    }
                    finally
                    {
                        message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                    }
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                }
            }
            else if (message.Type is MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                try
                {
                    if (message.Response is IResponseValueTaskSource<TResponse> requestResponse)
                        try
                        {
                            var response = wrapHandler(typeMessage);
                            requestResponse.TrySetResult(response);
                        }
                        catch (Exception e)
                        {
                            requestResponse.SetException(e);
                        }
                        finally
                        {
                            message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                        }
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                }
            }
        };
    }

    public void SetHandlerFromSpecificMessageHandler(
        Action<IMessage<TPayLoad>> wrapHandler)
    {
        Handler = message =>
        {
            if (!message.RuleFilter(SubscriberRule)) return;
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IMessage<TPayLoad> typeMessage = message.BorrowCopy<TPayLoad>(RegisteredContext);
                try
                {
                    wrapHandler(typeMessage);
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                }
                catch (Exception e)
                {
                    logger.Warn("Uncaught Exception in {0} for SubscriptionId: {1}. Got {2}", SubscriberRule
                        , SubscriberId, e);
                }
                finally
                {
                    typeMessage.DecrementRefCount();
                }
            }
        };
    }
}
