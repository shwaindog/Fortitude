#region

using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.MessageBus.Tasks;
using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace Fortitude.EventProcessing.BusRules.MessageBus;

public class MessagePump : RingPoller<Message>
{
    public const string RunActionAddress = "RunActionAddress";
    public const string RunTaskAddress = "RunTaskAddress";
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(MessagePump));

    public IMap<string, List<MessageListenerSubscription>> listeners
        = new ConcurrentMap<string, List<MessageListenerSubscription>>();

    private List<IListeningRule> livingRules = new();

    private MessagePumpSyncContext syncContext = null!;

    public MessagePump(PollingRing<Message> ring, uint timeoutMs, int id)
        : base(ring, timeoutMs) { }


    public MessagePumpTaskScheduler RingPollerScheduler { get; private set; } = null!;

    protected override void InitializeInPollingThread()
    {
        syncContext = new MessagePumpSyncContext();
        SynchronizationContext.SetSynchronizationContext(syncContext);
        RingPollerScheduler = new MessagePumpTaskScheduler();
    }

    protected override void CurrentPollingIterationComplete()
    {
        syncContext.RunQueuedTasks();
    }

    protected override void Processor(long sequence, long batchSize, Message data, bool startOfBatch,
        bool endOfBatch)
    {
        try
        {
            if (data.Type == MessageType.RunActionPayload)
            {
                var actionBody = ((PayLoad<Action>)data.PayLoad!).Body!;
                actionBody();
            }
            else if (data.Type == MessageType.MessageUnsubscribe)
            {
                var unsubscribePayLoad = ((PayLoad<MessageListenerUnsubscribe>)data.PayLoad!).Body!;
                var listeningAddress = unsubscribePayLoad.PublishAddress;
                if (listeners.TryGetValue(unsubscribePayLoad.PublishAddress, out var ruleListeners))
                {
                    for (var i = 0; i < ruleListeners!.Count; i++)
                    {
                        var ruleListener = ruleListeners![i];
                        if (ruleListener.SubscriberId == unsubscribePayLoad.SubscriberId) ruleListeners.RemoveAt(i);
                    }

                    if (ruleListeners.Count == 0) listeners.Remove(unsubscribePayLoad.PublishAddress);
                }

                unsubscribePayLoad.SubscriberRule.DecrementLifeTimeCount();
            }
            else if (listeners.TryGetValue(data.DestinationAddress!, out var ruleListeners))
            {
                foreach (var ruleListener in ruleListeners!) ruleListener.Handler(data);
            }

            for (var i = 0; i < livingRules.Count; i++)
            {
                var checkRule = livingRules[i];
                if (checkRule.ShouldBeStopped()) checkRule.Stop();
                livingRules.RemoveAt(i--);
            }
        }
        catch (Exception e)
        {
            logger.Warn("MessagePump id: {0} caught the following exception. ", e);
        }
    }
}
