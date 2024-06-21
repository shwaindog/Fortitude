// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.Execution;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.BusMessaging.Tasks;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines;

public interface IMessageQueue : IComparable<IMessageQueue>
{
    string           Name      { get; }
    MessageQueueType QueueType { get; }
    int              Id        { get; }

    bool IsRunning { get; }

    uint RecentlyReceivedMessagesCount { get; }

    QueueEventTime LatestMessageStartProcessing    { get; }
    QueueEventTime LatestMessageFinishedProcessing { get; }

    IQueueContext Context { get; }
    void          Start();
    void          EnqueueMessage(BusMessage msg);

    ValueTask<IDispatchResult> EnqueuePayloadBodyWithStatsAsync<TPayload>
    (TPayload payload, IRule sender,
        MessageType msgType = MessageType.Publish, string? destinationAddress = null
       ,
        ProcessorRegistry? processorRegistry = null, RuleFilter? ruleFilter = null, IPayloadMarshaller<TPayload>? payloadMarshaller = null);

    void EnqueuePayloadBody<TPayload>
    (TPayload payload, IRule sender,
        MessageType msgType = MessageType.Publish,
        string? destinationAddress = null, RuleFilter? ruleFilter = null
      , IPayloadMarshaller<TPayload>? payloadMarshaller = null);

    ValueTask<TResponse> RequestFromPayloadAsync<TPayload, TResponse>
    (TPayload payload, IRule sender,
        string? destinationAddress = null,
        MessageType msgType = MessageType.RequestResponse,
        ProcessorRegistry? processorRegistry = null, RuleFilter? ruleFilter = null);

    void                       LaunchRule(IRule sender, IRule rule);
    ValueTask<IDispatchResult> LaunchRuleAsync(IRule sender, IRule rule, RouteSelectionResult selectionResult);

    ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule);

    ValueTask<IDispatchResult> AddListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , ProcessorRegistry? processorRegistry = null);

    ValueTask<IDispatchResult> RemoveListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , ProcessorRegistry? processorRegistry = null);

    bool IsListeningToAddress(string destinationAddress);
    int  RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress);

    IAlternativeExecutionContextAction<TP>       GetExecutionContextAction<TP>(IRule sender);
    IAlternativeExecutionContextAction<TP1, TP2> GetExecutionContextAction<TP1, TP2>(IRule sender);
    IAlternativeExecutionContextResult<TR>       GetExecutionContextResult<TR>(IRule sender);
    IAlternativeExecutionContextResult<TR, TP>   GetExecutionContextResult<TR, TP>(IRule sender);

    void RunOn(IRule sender, Action action);

    IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate);

    void Shutdown();
}

public class MessageQueue : IMessageQueue
{
    public const            int      MessageCountHistoryEntries = 60;
    private static readonly IFLogger Logger                     = FLoggerFactory.Instance.GetLogger(typeof(MessageQueue));
    private readonly        string   name;

    private readonly QueueContext queueContext;

    private readonly uint[]                   recentMessageCountReceived = new uint[MessageCountHistoryEntries];
    private readonly IPollingRing<BusMessage> ring;

    private DateTime lastUpDateTime = DateTime.Now;

    public MessageQueue(IConfigureMessageBus messageBus, MessageQueueType queueType, int id, IAsyncValueTaskRingPoller<BusMessage> ringPoller)
    {
        QueueType                               =  queueType;
        Id                                      =  id;
        name                                    =  ringPoller.Ring.Name;
        ring                                    =  ringPoller.Ring;
        queueContext                            =  new QueueContext(this, messageBus);
        ringPoller.Recycler                     =  queueContext.PooledRecycler;
        LatestMessageStartProcessing            =  new QueueEventTime(-1, DateTime.UtcNow);
        LatestMessageFinishedProcessing         =  new QueueEventTime(-1, DateTime.UtcNow);
        MessagePump                             =  new MessagePump(ringPoller, queueContext);
        MessagePump.MessageStartProcessingTime  += SetLatestStarted;
        MessagePump.MessageFinishProcessingTime += SetLatestFinished;
    }

    public MessagePump MessagePump { get; set; }

    public string Name => name;

    public MessageQueueType QueueType { get; }

    public IQueueContext Context => queueContext;

    public int Id { get; }

    public bool IsRunning => MessagePump.IsRunning;

    public QueueEventTime LatestMessageStartProcessing    { get; private set; }
    public QueueEventTime LatestMessageFinishedProcessing { get; private set; }

    public void EnqueueMessage(BusMessage msg)
    {
        // Logger.Debug("Sending {0} on {1}", msg, Name);
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt   = ring[seqId];
        evt.Type               = msg.Type;
        evt.Payload            = msg.Payload;
        evt.DestinationAddress = msg.DestinationAddress;
        evt.Response           = msg.Response;
        evt.Sender             = msg.Sender;
        evt.SentTime           = msg.SentTime;
        evt.ProcessorRegistry  = msg.ProcessorRegistry;
        evt.RuleFilter         = msg.RuleFilter;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public ValueTask<IDispatchResult> EnqueuePayloadBodyWithStatsAsync<TPayload>
    (TPayload payload, IRule sender,
        MessageType msgType = MessageType.Publish, string? destinationAddress = null
       ,
        ProcessorRegistry? processorRegistry = null, RuleFilter? ruleFilter = null, IPayloadMarshaller<TPayload>? payloadMarshaller = null)
    {
        if (processorRegistry == null)
        {
            processorRegistry                = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
            processorRegistry.DispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
            processorRegistry.IncrementRefCount();
            processorRegistry.DispatchResult.SentTime        = DateTime.Now;
            processorRegistry.ResponseTimeoutAndRecycleTimer = sender.Context.QueueTimer;
        }

        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<TPayload>>();
        payLoadContainer.PayloadMarshaller = payloadMarshaller?.QueueMarshaller(this);
        payLoadContainer.SetBody = payLoadContainer.PayloadMarshaller != null
            ? payLoadContainer.PayloadMarshaller.GetMarshalled(payload, PayloadRequestType.QueueSend)
            : payload;
        evt.Type               = msgType;
        evt.Payload            = payLoadContainer;
        evt.DestinationAddress = destinationAddress;
        evt.Response           = BusMessage.NoOpCompletionSource;
        evt.Sender             = sender;
        evt.SentTime           = DateTime.Now;
        // logger.Debug("EnqueuePayloadWithStats processorRegistry: {0}", processorRegistry.ToString());
        evt.ProcessorRegistry = processorRegistry;
        evt.RuleFilter        = ruleFilter ?? BusMessage.AppliesToAll;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
        return processorRegistry.GenerateValueTask();
    }

    public ValueTask<TResponse> RequestFromPayloadAsync<TPayload, TResponse>
    (TPayload payload
      , IRule sender,
        string? destinationAddress = null
       ,
        MessageType msgType = MessageType.RequestResponse,
        ProcessorRegistry? processorRegistry = null, RuleFilter? ruleFilter = null)
    {
        if (processorRegistry == null)
        {
            processorRegistry                = sender.Context.PooledRecycler.Borrow<ProcessorRegistry>();
            processorRegistry.DispatchResult = sender.Context.PooledRecycler.Borrow<DispatchResult>();
            processorRegistry.IncrementRefCount();
            processorRegistry.DispatchResult.SentTime        = DateTime.Now;
            processorRegistry.ResponseTimeoutAndRecycleTimer = sender.Context.QueueTimer;
        }

        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<TPayload>>();
        payLoadContainer.SetBody = payload;
        evt.Type                 = msgType;
        evt.Payload              = payLoadContainer;
        evt.DestinationAddress   = destinationAddress;
        var reusableValueTaskSource
            = queueContext.PooledRecycler.Borrow<ReusableResponseValueTaskSource<TResponse>>();
        reusableValueTaskSource.DispatchResult                 = processorRegistry.DispatchResult;
        reusableValueTaskSource.ResponseTimeoutAndRecycleTimer = queueContext.QueueTimer;
        evt.Response                                           = reusableValueTaskSource;
        evt.Sender                                             = sender;
        evt.SentTime                                           = DateTime.Now;
        evt.ProcessorRegistry                                  = processorRegistry;
        evt.RuleFilter                                         = ruleFilter ?? BusMessage.AppliesToAll;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
        return reusableValueTaskSource.GenerateValueTask();
    }

    public ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.ShutDownRequested;
        rule.Context        = queueContext;
        return EnqueuePayloadBodyWithStatsAsync(rule, sender, MessageType.UnloadRule, null, null);
    }

    public bool IsListeningToAddress(string destinationAddress) => MessagePump.IsListeningOn(destinationAddress);

    public async ValueTask<IDispatchResult> AddListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , ProcessorRegistry? processorRegistry = null) =>
        await EnqueuePayloadBodyWithStatsAsync(interceptor, sender, MessageType.AddListenSubscribeInterceptor, null, processorRegistry);

    public async ValueTask<IDispatchResult> RemoveListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , ProcessorRegistry? processorRegistry = null) =>
        await EnqueuePayloadBodyWithStatsAsync(interceptor, sender, MessageType.RemoveListenSubscribeInterceptor, null, processorRegistry);

    public int RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress)
    {
        var listenersAtDestination = MessagePump.ListeningSubscriptionsOn(destinationAddress);
        var count                  = 0;
        if (listenersAtDestination == null) return count;
        foreach (var subscription in listenersAtDestination)
            if (subscription.SubscriberRule.LifeCycleState is RuleLifeCycle.Started)
            {
                count++;
                toAddRules.Add(subscription.SubscriberRule);
            }

        return count;
    }

    public void RunOn(IRule sender, Action action)
    {
        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<Action>>();
        payLoadContainer.SetBody = action;
        evt.Type                 = MessageType.RunActionPayload;
        evt.Payload              = payLoadContainer;
        evt.DestinationAddress   = null;
        evt.Response             = BusMessage.NoOpCompletionSource;
        evt.Sender               = sender;
        evt.SentTime             = DateTime.Now;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public void EnqueuePayloadBody<TPayload>
    (TPayload payload, IRule sender,
        MessageType msgType = MessageType.Publish,
        string? destinationAddress = null, RuleFilter? ruleFilter = null
      , IPayloadMarshaller<TPayload>? payloadMarshaller = null)
    {
        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<TPayload>>();
        payLoadContainer.PayloadMarshaller = payloadMarshaller?.QueueMarshaller(this);
        payLoadContainer.SetBody = payLoadContainer.PayloadMarshaller != null
            ? payLoadContainer.PayloadMarshaller.GetMarshalled(payload, PayloadRequestType.QueueSend)
            : payload;
        evt.Type               = msgType;
        evt.Payload            = payLoadContainer;
        evt.DestinationAddress = destinationAddress;
        evt.Response           = BusMessage.NoOpCompletionSource;
        evt.Sender             = sender;
        evt.SentTime           = DateTime.Now;
        evt.ProcessorRegistry  = null;
        evt.RuleFilter         = ruleFilter ?? BusMessage.AppliesToAll;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public uint RecentlyReceivedMessagesCount
    {
        get
        {
            uint total               = 0;
            var  currentTimeToSecond = DateTime.Now.TruncToSecondBoundary();
            if (currentTimeToSecond != lastUpDateTime)
            {
                var timeSpanBetweenLastMessage = currentTimeToSecond - lastUpDateTime;
                if (timeSpanBetweenLastMessage > TimeSpan.FromSeconds(60))
                {
                    for (var i = 0; i < MessageCountHistoryEntries; i++) recentMessageCountReceived[i] = 0;
                }
                else
                {
                    var secondsSinceLastMessage = (int)timeSpanBetweenLastMessage.TotalSeconds;
                    for (var i = lastUpDateTime.Second + 1; i < secondsSinceLastMessage; i++)
                        recentMessageCountReceived[i % MessageCountHistoryEntries] = 0;
                }

                lastUpDateTime = currentTimeToSecond;
            }

            for (var i = 0; i < MessageCountHistoryEntries; i++) total += recentMessageCountReceived[i];
            return total;
        }
    }

    public int CompareTo(IMessageQueue? other) => (int)RecentlyReceivedMessagesCount - (int)(other?.RecentlyReceivedMessagesCount ?? 0);

    public void Start()
    {
        MessagePump?.Start();
    }

    public void Shutdown()
    {
        MessagePump.Dispose();
    }

    public void LaunchRule(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context        = queueContext;
        EnqueuePayloadBody(rule, sender, MessageType.LoadRule, null);
    }

    public ValueTask<IDispatchResult> LaunchRuleAsync(IRule sender, IRule rule, RouteSelectionResult selectionResult)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context        = queueContext;
        return EnqueuePayloadBodyWithStatsAsync(rule, sender, MessageType.LoadRule, null, null);
    }

    public IAlternativeExecutionContextAction<TP> GetExecutionContextAction<TP>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<MessageQueueExecutionContextAction<TP>>();
        executionContext.Configure(this, sender);
        return executionContext;
    }

    public IAlternativeExecutionContextAction<TP1, TP2> GetExecutionContextAction<TP1, TP2>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<MessageQueueExecutionContextAction<TP1, TP2>>();
        executionContext.Configure(this, sender);
        return executionContext;
    }

    public IAlternativeExecutionContextResult<TR> GetExecutionContextResult<TR>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<MessageQueueExecutionContextResult<TR>>();
        executionContext.Configure(this, sender);
        return executionContext;
    }

    public IAlternativeExecutionContextResult<TR, TP> GetExecutionContextResult<TR, TP>(IRule sender)
    {
        var executionContext = Context.PooledRecycler.Borrow<MessageQueueExecutionContextResult<TR, TP>>();
        executionContext.Configure(this, sender);
        return executionContext;
    }

    public IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate)
    {
        var livingRules      = Context.PooledRecycler.Borrow<AutoRecycledEnumerable<IRule>>();
        var countLivingRules = MessagePump.CopyLivingRulesTo(livingRules);
        return livingRules.Where(predicate);
    }

    private void IncrementRecentMessageReceived()
    {
        var currentTimeToSecond = DateTime.Now.TruncToSecondBoundary();
        if (currentTimeToSecond != lastUpDateTime)
        {
            var timeSpanBetweenLastMessage = currentTimeToSecond - lastUpDateTime;
            if (timeSpanBetweenLastMessage > TimeSpan.FromSeconds(60))
            {
                for (var i = 0; i < MessageCountHistoryEntries; i++) recentMessageCountReceived[i] = 0;
            }
            else
            {
                var secondsSinceLastMessage = (int)timeSpanBetweenLastMessage.TotalSeconds;
                for (var i = lastUpDateTime.Second + 1; i < secondsSinceLastMessage; i++)
                    recentMessageCountReceived[i % MessageCountHistoryEntries] = 0;
            }

            lastUpDateTime = currentTimeToSecond;
        }

        recentMessageCountReceived[currentTimeToSecond.Second] += 1;
    }

    private void SetLatestStarted(QueueEventTime queueEventTime)
    {
        LatestMessageStartProcessing = queueEventTime;
    }

    private void SetLatestFinished(QueueEventTime queueEventTime)
    {
        LatestMessageFinishedProcessing = queueEventTime;
    }

    public override string ToString() => $"{GetType().Name}({nameof(name)}: \"{name}\")";
}
