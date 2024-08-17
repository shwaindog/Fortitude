// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines.Processing;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;

#endregion


namespace FortitudeBusRules.BusMessaging.Pipelines;

public interface IQueueMessageRing : IAsyncValueTaskPollingRing<BusMessage>
{
    QueueContext QueueContext { get; set; }
    bool         IsListeningOn(string address);
    int          CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo);

    IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address);

    void InitializeInPollingThread();
}

public struct DaemonRuleStart
{
    public DaemonRuleStart(IListeningRule rule, ValueTask startTask)
    {
        Rule      = rule;
        StartTask = startTask;
    }

    public IListeningRule Rule      { get; }
    public ValueTask      StartTask { get; }
}

public class QueueMessageRing : AsyncValueTaskPollingRing<BusMessage>, IQueueMessageRing
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(QueueMessageRing));

    internal readonly List<DaemonRuleStart> DaemonExecutions = new();
    internal readonly List<IListeningRule>  LivingRules      = new();

    internal ListenerRegistry ListenerRegistry = null!;
    private  QueueContext     queueContext     = null!;

    public QueueMessageRing(string name, int size)
        : base(name, size, () => new BusMessage(), ClaimStrategyType.MultiProducers) { }

    public SyncContextTaskScheduler RingPollerScheduler { get; private set; } = null!;

    public QueueContext QueueContext
    {
        get => queueContext;
        set
        {
            queueContext     = value;
            ListenerRegistry = new ListenerRegistry(queueContext.PooledRecycler);
        }
    }

    public bool IsListeningOn(string address) => ListenerRegistry.IsListeningOn(address);

    public int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo)
    {
        var i = 0;
        try
        {
            for (; i < LivingRules.Count; i++) toCopyTo.Add(LivingRules[i]);
        }
        catch (IndexOutOfRangeException)
        {
            // swallow
        }

        return i;
    }

    public IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address) => ListenerRegistry.MatchingSubscriptions(address);

    public void InitializeInPollingThread()
    {
        var queueRecycler = QueueContext.PooledRecycler;

        Recycler.ThreadStaticRecycler = queueRecycler;

        for (var i = 0; i < Size; i++)
        {
            var ringBusMessage = this[i];
            ringBusMessage.Recycler = queueRecycler;

            ringBusMessage.AutoRecycleAtRefCountZero = false;
        }

        RingPollerScheduler = new SyncContextTaskScheduler();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        QueueContext.QueueTimer.Dispose();
    }

    public void Stop()
    {
        QueueContext.QueueTimer.Dispose();
    }

    public override ValueTask ProcessEntry(BusMessage entry) => ProcessMessage(entry);

    public ValueTask ProcessMessage(BusMessage entry)
    {
        try
        {
            var processBusMessage = QueueContext.PooledRecycler.Borrow<AsyncBusMessageProcessor>();
            return processBusMessage.Start(entry.ToBusMessageValue(), this);
        }
        catch (Exception e)
        {
            Logger.Warn("MessagePump id: {0} caught the following exception. ", e);
        }
        return ValueTask.CompletedTask;
    }
}
