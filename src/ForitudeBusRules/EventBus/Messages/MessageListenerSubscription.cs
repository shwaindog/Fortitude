#region

using System.Threading.Tasks.Sources;
using Fortitude.EventProcessing.BusRules.EventBus.Tasks;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace Fortitude.EventProcessing.BusRules.EventBus.Messages;

public class MessageListenerSubscription
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(MessageListenerSubscription));

    public MessageListenerSubscription(IRule subscriberRule, string publishAddress, string subscriberId)
    {
        SubscriberId = subscriberId;
        SubscriberRule = subscriberRule;
        PublishAddress = publishAddress;
    }

    public string SubscriberId { get; set; }

    public IEventContext RegisteredContext { get; set; } = null!;
    public IRule SubscriberRule { get; set; }
    public string PublishAddress { get; set; }
    public Action<Message> Handler { get; private set; } = null!;

    public void SetHandlerFromSpecificMessageHandler<TPayLoad, TResponse>(
        Func<IMessage<TPayLoad, TResponse>, ValueTask<TResponse>> wrapHandler)
    {
        Handler = message =>
        {
            if (message.Type is MessageType.Publish or MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var typeResponse = message.Response as ReusableValueTaskSource<TResponse>;
                try
                {
                    typeResponse?.IncrementRefCount();
                    var response = wrapHandler(typeMessage);
                    if (typeResponse != null)
                    {
                        if (!response.IsCompleted)
                        {
                            message.ProcessorRegistry?.RegisterAwaiting(SubscriberRule);
                            typeResponse.OnCompleted(_ => typeResponse.SetResult(response.Result), null, 0
                                , ValueTaskSourceOnCompletedFlags.UseSchedulingContext);
                        }
                        else if (response.IsCompletedSuccessfully || response.IsFaulted)
                        {
                            try
                            {
                                message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                                typeResponse.TrySetResult(response.Result);
                            }
                            catch (Exception e)
                            {
                                typeResponse.SetException(e);
                            }
                        }
                        else
                        {
                            message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                        }
                    }
                }
                finally
                {
                    typeResponse?.DecrementRefCount();
                    typeMessage.DecrementRefCount();
                }
            }
            else
            {
                logger.Warn($"Unexpected call to MessageListenerSubscription with message: {message}");
            }
        };
    }

    public void SetHandlerFromSpecificMessageHandler<TPayLoad, TResponse>(
        Func<IMessage<TPayLoad, TResponse>, TResponse> wrapHandler)
    {
        Handler = message =>
        {
            if (message.Type is MessageType.Publish or MessageType.RequestResponse)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                var typeResponse = message.Response as ReusableValueTaskSource<TResponse>;
                try
                {
                    typeResponse?.IncrementRefCount();
                    if (typeResponse != null)
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
                    typeResponse?.DecrementRefCount();
                    typeMessage.DecrementRefCount();
                }
            }
        };
    }

    public void SetHandlerFromSpecificMessageHandler<TPayLoad, TResponse>(
        Action<IMessage<TPayLoad, TResponse>> wrapHandler)
    {
        Handler = message =>
        {
            if (message.Type is MessageType.Publish)
            {
                message.ProcessorRegistry?.RegisterStart(SubscriberRule);
                IMessage<TPayLoad, TResponse> typeMessage = message.BorrowCopy<TPayLoad, TResponse>(RegisteredContext);
                try
                {
                    wrapHandler(typeMessage);
                }
                catch (Exception e)
                {
                    logger.Warn("Uncaught Exception in {0} for SubscriptionId: {1}. Got {2}", SubscriberRule
                        , SubscriberId, e);
                }
                finally
                {
                    message.ProcessorRegistry?.RegisterFinish(SubscriberRule);
                    typeMessage.DecrementRefCount();
                }
            }
        };
    }
}
