#region

using System.Threading.Tasks.Sources;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Messages;

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
        if (state is IMessage msgState)
        {
            var typeResponse = (ReusableValueTaskSource<TResponse>)msgState.Response;
            try
            {
                typeResponse.TrySetResultFromAwaitingTask(completed);
            }
            catch (Exception e)
            {
                logger.Warn("Error expected completed to be completed when this is called. Got {0}", e);
            }

            msgState.ProcessorRegistry?.RegisterFinish(SubscriberRule);
            msgState.DecrementRefCount();
        }
    }

    private void CallbackValueTaskCompleted(object? state)
    {
        if (state is IMessage msgState)
        {
            var typeResponse = (ReusableValueTaskSource<TResponse>)msgState.Response;
            try
            {
                typeResponse.TrySetResultFromAwaitingTask(typeResponse.AwaitingValueTask!.Value);
            }
            catch (Exception e)
            {
                logger.Warn("Error expected completed to be completed when this is called. Got {0}", e);
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
            if (message.Type is MessageType.Publish or MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var typeResponse = message.Response as ReusableValueTaskSource<TResponse>;
                var response = wrapHandler(typeMessage);
                if (typeResponse != null)
                {
                    if (!response.IsCompleted)
                    {
                        message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                        var responseAsReusable = response.ToReusableValueTaskSource();
                        if (responseAsReusable != null)
                        {
                            responseAsReusable.OnCompleted(onDependentValueTaskCompleted, message
                                , responseAsReusable.Version, ValueTaskSourceOnCompletedFlags.None);
                        }
                        else
                        {
                            var responseAsTask = response.ToTask();
                            responseAsTask.ContinueWith(onDependentTaskCompleted, message);
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
            if (message.Type is MessageType.Publish or MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var typeResponse = message.Response as ReusableValueTaskSource<TResponse>;
                var response = wrapHandler(typeMessage);
                if (typeResponse != null)
                {
                    if (!response.IsCompleted)
                    {
                        message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                        response.ContinueWith(onDependentTaskCompleted, message);
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
            if (message.Type is MessageType.Publish or MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IRespondingMessage<TPayLoad, TResponse> typeMessage
                    = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                try
                {
                    if (message.Response is ReusableValueTaskSource<TResponse> typeResponse)
                        try
                        {
                            var response = wrapHandler(typeMessage);
                            typeResponse.TrySetResult(response);
                        }
                        catch (Exception e)
                        {
                            typeResponse.SetException(e);
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
