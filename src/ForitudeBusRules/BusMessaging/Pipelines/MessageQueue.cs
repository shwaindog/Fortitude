// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages;
using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.Execution;
using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.Extensions;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines;

public interface IMessageQueue : IComparable<IMessageQueue>, IStringBearer
{
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMemberInSuper.Global
    string Name { get; }

    int Id { get; }

    bool IsRunning { get; }

    MessageQueueType QueueType { get; }
    IQueueContext    Context   { get; }

    uint RecentlyReceivedMessagesCount { get; }

    QueueEventTime LatestMessageStartProcessing    { get; }
    QueueEventTime LatestMessageFinishedProcessing { get; }

    void Start();
    void EnqueueMessage(BusMessage msg);

    ValueTask<IDispatchResult> EnqueuePayloadBodyWithStatsAsync<TPayload>
    (TPayload payload, IRule sender, MessageType msgType = MessageType.Publish, string? destinationAddress = null
      , DispatchProcessorRegistry? processorRegistry = null, bool timerSourceRecycle = false, RuleFilter? ruleFilter = null
      , IPayloadMarshaller<TPayload>? payloadMarshaller = null);

    void EnqueuePayloadBody<TPayload>
    (TPayload payload, IRule sender, MessageType msgType = MessageType.Publish, string? destinationAddress = null
      , RuleFilter? ruleFilter = null, IPayloadMarshaller<TPayload>? payloadMarshaller = null);

    void EnqueueLaunchRule(LaunchRulePayload launchRulePayload, IRule sender);

    ValueTask<TResponse> RequestWithPayloadAsync<TPayload, TResponse>
    (TPayload payload, IRule sender, string? destinationAddress = null, MessageType msgType = MessageType.RequestResponse
      , RuleFilter? ruleFilter = null);

    void LaunchRule(IRule sender, IRule rule, DeploymentOptions? deploymentOptions = null);
    void StopRule(IRule sender, IRule rule);
    bool IsListeningToAddress(string destinationAddress);
    int  RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress);

    void RunOn(IRule sender, Action action);
    void RunOn(IRule sender, Func<ValueTask> action);

    ValueTask          RunOnAndWait(IRule sender, Func<ValueTask> action);
    ValueTask<TResult> RunOnFetchResult<TResult>(IRule sender, Func<TResult> action);
    ValueTask<TResult> RunOnFetchResult<TResult>(IRule sender, Func<ValueTask<TResult>> action);

    ValueTask<IRuleDeploymentLifeTime> LaunchRuleAsync
        (IRule sender, IRule rule, RouteSelectionResult selectionResult, DeploymentOptions? deploymentOptions = null);

    ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule);

    ValueTask<IDispatchResult> AddListenSubscribeInterceptor
        (IRule sender, IListenSubscribeInterceptor interceptor, DispatchProcessorRegistry? processorRegistry = null);

    ValueTask<IDispatchResult> RemoveListenSubscribeInterceptor
        (IRule sender, IListenSubscribeInterceptor interceptor, DispatchProcessorRegistry? processorRegistry = null);

    IAlternativeExecutionContextAction<TP>       GetExecutionContextAction<TP>(IRule sender);
    IAlternativeExecutionContextAction<TP1, TP2> GetExecutionContextAction<TP1, TP2>(IRule sender);
    IAlternativeExecutionContextResult<TR>       GetExecutionContextResult<TR>(IRule sender);
    IAlternativeExecutionContextResult<TR, TP>   GetExecutionContextResult<TR, TP>(IRule sender);


    IEnumerable<IRule> RulesMatching(Func<IRule, bool> predicate);

    void Shutdown();
    
    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}

public class MessageQueue : IMessageQueue
{
    // ReSharper disable once UnusedMember.Local
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(MessageQueue));

    public const int MessageCountHistoryEntries = 60;

    private readonly QueueContext queueContext;

    private readonly string name;
    private readonly uint[] recentMessageCountReceived = new uint[MessageCountHistoryEntries];

    private readonly IEnqueueTaskCallbackPollingRing<BusMessage> ring;

    private DateTime lastUpDateTime = DateTime.Now;

    public MessageQueue(IConfigureMessageBus messageBus, MessageQueueType queueType, int id, IMessagePump messagePump)
    {
        QueueType   = queueType;
        MessagePump = messagePump;

        Id   = id;
        name = MessagePump.Ring.Name;
        ring = MessagePump.Ring;

        queueContext = new QueueContext(this, messageBus);

        MessagePump.QueueContext        = queueContext;
        LatestMessageStartProcessing    = new QueueEventTime(-1, DateTime.UtcNow);
        LatestMessageFinishedProcessing = new QueueEventTime(-1, DateTime.UtcNow);

        MessagePump.QueueEntryStart    += SetLatestStarted;
        MessagePump.QueueEntryComplete += SetLatestFinished;
    }

    public IMessagePump MessagePump { get; set; }

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
        ring.Enqueue(msg);
        MessagePump.WakeIfAsleep();
    }

    public ValueTask<IDispatchResult> EnqueuePayloadBodyWithStatsAsync<TPayload>
    (TPayload payload, IRule sender, MessageType msgType = MessageType.Publish, string? destinationAddress = null
      , DispatchProcessorRegistry? processorRegistry = null, bool timerSourceRecycle = false, RuleFilter? ruleFilter = null
      , IPayloadMarshaller<TPayload>? payloadMarshaller = null)
    {
        if (processorRegistry == null)
        {
            processorRegistry = sender.Context.PooledRecycler.Borrow<DispatchProcessorRegistry>();

            processorRegistry.Result = sender.Context.PooledRecycler.Borrow<DispatchResult>();

            processorRegistry.Result.SentTime = DateTime.Now;

            processorRegistry.ResponseTimeoutAndRecycleTimer = timerSourceRecycle ? sender.Context.QueueTimer : null;
        }
        processorRegistry.IncrementRefCount();

        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<TPayload>>();
        payLoadContainer.PayloadMarshaller = payloadMarshaller?.QueueMarshaller(this);
        payLoadContainer.SetBody = payLoadContainer.PayloadMarshaller != null
            ? payLoadContainer.PayloadMarshaller.GetMarshalled(payload, PayloadRequestType.QueueSend)
            : payload;

        evt.Type     = msgType;
        evt.Payload  = payLoadContainer;
        evt.Response = null;
        evt.Sender   = sender;
        evt.SentTime = DateTime.Now;

        // logger.Debug("EnqueuePayloadWithStats processorRegistry: {0}", processorRegistry.ToString());
        evt.DestinationAddress = destinationAddress;
        evt.ProcessorRegistry  = processorRegistry;
        evt.RuleFilter         = ruleFilter ?? BusMessage.AppliesToAll;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
        return processorRegistry.GenerateValueTask();
    }

    public ValueTask<TResponse> RequestWithPayloadAsync<TPayload, TResponse>
    (TPayload payload, IRule sender, string? destinationAddress = null, MessageType msgType = MessageType.RequestResponse
      , RuleFilter? ruleFilter = null)
    {
        IncrementRecentMessageReceived();
        var seqId = ring.Claim();
        var evt   = ring[seqId];

        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<TPayload>>();
        payLoadContainer.SetBody = payload;

        var reusableValueTaskSource = queueContext.PooledRecycler.Borrow<ReusableValueTaskSource<TResponse>>();
        reusableValueTaskSource.IncrementRefCount(); // GetResult will do the final decrement

        evt.Type       = msgType;
        evt.Payload    = payLoadContainer;
        evt.Response   = reusableValueTaskSource;
        evt.Sender     = sender;
        evt.SentTime   = DateTime.Now;
        evt.RuleFilter = ruleFilter ?? BusMessage.AppliesToAll;

        evt.DestinationAddress = destinationAddress;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
        return reusableValueTaskSource.GenerateValueTask();
    }

    public ValueTask<IDispatchResult> StopRuleAsync(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.ShutDownRequested;
        rule.Context        = queueContext;
        return EnqueuePayloadBodyWithStatsAsync(rule, sender, MessageType.UnloadRule);
    }

    public bool IsListeningToAddress(string destinationAddress) => MessagePump.IsListeningOn(destinationAddress);

    public async ValueTask<IDispatchResult> AddListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , DispatchProcessorRegistry? processorRegistry = null) =>
        await EnqueuePayloadBodyWithStatsAsync(interceptor, sender, MessageType.AddListenSubscribeInterceptor, null, processorRegistry);

    public async ValueTask<IDispatchResult> RemoveListenSubscribeInterceptor
    (IRule sender, IListenSubscribeInterceptor interceptor
      , DispatchProcessorRegistry? processorRegistry = null) =>
        await EnqueuePayloadBodyWithStatsAsync(interceptor, sender, MessageType.RemoveListenSubscribeInterceptor, null, processorRegistry);

    public int RulesListeningToAddress(ISet<IRule> toAddRules, string destinationAddress)
    {
        var listenersAtDestination = MessagePump.ListeningSubscriptionsOn(destinationAddress);
        var count                  = 0;
        foreach (var subscription in listenersAtDestination)
            if (subscription.SubscriberRule.LifeCycleState is RuleLifeCycle.Started)
            {
                count++;
                toAddRules.Add(subscription.SubscriberRule);
            }

        return count;
    }

    public void RunOn(IRule sender, Func<ValueTask> action)
    {
        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<Func<ValueTask>>>();
        payLoadContainer.SetBody = action;

        evt.Type     = MessageType.RunAsyncActionPayload;
        evt.Payload  = payLoadContainer;
        evt.Response = null;
        evt.Sender   = sender;
        evt.SentTime = DateTime.Now;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        evt.DestinationAddress = null;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public void RunOn(IRule sender, Action action)
    {
        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<Action>>();
        payLoadContainer.SetBody = action;

        evt.Type     = MessageType.RunActionPayload;
        evt.Payload  = payLoadContainer;
        evt.Response = null;
        evt.Sender   = sender;
        evt.SentTime = DateTime.Now;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        evt.DestinationAddress = null;
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public async ValueTask RunOnAndWait(IRule sender, Func<ValueTask> action)
    {
        var payLoadContainer = queueContext.PooledRecycler.Borrow<NoParamsAsyncValueTaskResultPayload>();
        payLoadContainer.IncrementRefCount();
        payLoadContainer.Configure(action);
        var asyncResult = payLoadContainer.GenerateValueTask();
        EnqueuePayloadBody(payLoadContainer, sender, MessageType.InvokeablePayload);

        await asyncResult;
        payLoadContainer.DecrementRefCount();
    }

    public async ValueTask<TResult> RunOnFetchResult<TResult>(IRule sender, Func<TResult> action)
    {
        var payLoadContainer = queueContext.PooledRecycler.Borrow<NoParamsSyncResultPayload<TResult>>();
        payLoadContainer.IncrementRefCount();
        payLoadContainer.Configure(action);
        var asyncResult = payLoadContainer.GenerateValueTask();
        EnqueuePayloadBody(payLoadContainer, sender, MessageType.InvokeablePayload);

        var result = await asyncResult;
        payLoadContainer.DecrementRefCount();
        return result;
    }

    public async ValueTask<TResult> RunOnFetchResult<TResult>(IRule sender, Func<ValueTask<TResult>> action)
    {
        var payLoadContainer = queueContext.PooledRecycler.Borrow<NoParamsAsyncResultPayload<TResult>>();
        payLoadContainer.IncrementRefCount();
        var asyncResult = payLoadContainer.GenerateValueTask();
        payLoadContainer.Configure(action);

        var result = await asyncResult;
        payLoadContainer.DecrementRefCount();
        return result;
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

        evt.Type       = msgType;
        evt.Payload    = payLoadContainer;
        evt.Response   = null;
        evt.Sender     = sender;
        evt.SentTime   = DateTime.Now;
        evt.RuleFilter = ruleFilter ?? BusMessage.AppliesToAll;

        evt.ProcessorRegistry  = null;
        evt.DestinationAddress = destinationAddress;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
    }

    public void EnqueueLaunchRule(LaunchRulePayload launchRulePayload, IRule sender)
    {
        IncrementRecentMessageReceived();
        var seqId            = ring.Claim();
        var evt              = ring[seqId];
        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<LaunchRulePayload>>();
        payLoadContainer.SetBody = launchRulePayload;

        evt.Type       = MessageType.LoadRule;
        evt.Payload    = payLoadContainer;
        evt.Response   = null;
        evt.Sender     = sender;
        evt.SentTime   = DateTime.Now;
        evt.RuleFilter = BusMessage.AppliesToAll;

        evt.ProcessorRegistry  = null;
        evt.DestinationAddress = null;
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
        MessagePump.Start();
    }

    public void Shutdown()
    {
        MessagePump.Dispose();
    }

    public void LaunchRule(IRule sender, IRule rule, DeploymentOptions? deploymentOptions = null)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context        = queueContext;
        EnqueueLaunchRule(new LaunchRulePayload(rule, deploymentOptions), sender);
    }

    public void StopRule(IRule sender, IRule rule)
    {
        rule.LifeCycleState = RuleLifeCycle.ShutDownRequested;
        rule.Context        = queueContext;
        EnqueuePayloadBody(rule, sender, MessageType.UnloadRule);
    }

    public ValueTask<IRuleDeploymentLifeTime> LaunchRuleAsync
        (IRule sender, IRule rule, RouteSelectionResult selectionResult, DeploymentOptions? deploymentOptions = null)
    {
        rule.LifeCycleState = RuleLifeCycle.Starting;
        rule.Context        = queueContext;
        return EnqueueRuleWithLifeTimeAsync(new LaunchRulePayload(rule, deploymentOptions), sender, MessageType.LoadRule);
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
        var livingRules = Context.PooledRecycler.Borrow<AutoRecycledEnumerable<IRule>>();
        return livingRules.Where(predicate);
    }

    public ValueTask<IRuleDeploymentLifeTime> EnqueueRuleWithLifeTimeAsync
    (LaunchRulePayload launchRulePayload, IRule sender, MessageType msgType = MessageType.Publish, string? destinationAddress = null
      , DeploymentLifeTimeProcessorRegistry? processorRegistry = null, RuleFilter? ruleFilter = null)
    {
        if (processorRegistry == null)
        {
            processorRegistry = sender.Context.PooledRecycler.Borrow<DeploymentLifeTimeProcessorRegistry>();

            processorRegistry.Result = sender.Context.PooledRecycler.Borrow<RuleDeploymentLifeTime>();

            processorRegistry.Result.DeployedRule = launchRulePayload.Rule;
            processorRegistry.Result.SenderRule   = sender;
            processorRegistry.Result.SentTime     = DateTime.Now;

            processorRegistry.ResponseTimeoutAndRecycleTimer = null;
        }
        processorRegistry.IncrementRefCount();

        IncrementRecentMessageReceived();

        var seqId = ring.Claim();
        var evt   = ring[seqId];

        var payLoadContainer = queueContext.PooledRecycler.Borrow<Payload<LaunchRulePayload>>();
        payLoadContainer.SetBody = launchRulePayload;

        evt.Type       = msgType;
        evt.Payload    = payLoadContainer;
        evt.Response   = null;
        evt.Sender     = sender;
        evt.SentTime   = DateTime.Now;
        evt.RuleFilter = ruleFilter ?? BusMessage.AppliesToAll;

        // logger.Debug("EnqueuePayloadWithStats processorRegistry: {0}", processorRegistry.ToString());
        evt.DestinationAddress = destinationAddress;
        evt.ProcessorRegistry  = processorRegistry;
        // Logger.Debug("Sending {0} on {1}", evt, Name);
        ring.Publish(seqId);
        MessagePump.WakeIfAsleep();
        return processorRegistry.GenerateValueTask();
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

    public virtual StateExtractStringRange RevealState(ITheOneString stsa) => 
        stsa.StartComplexType(this)
            .Field.AlwaysAdd(nameof(Name), Name)
            .Field.AlwaysAdd(nameof(Id), Id)
            .Field.AlwaysAdd(nameof(IsRunning), IsRunning)
            .Field.AlwaysAdd(nameof(Context), Context)
            .Complete() ;

    public override string ToString() => $"{nameof(MessageQueue)}({nameof(name)}: \"{name}\")";
}
