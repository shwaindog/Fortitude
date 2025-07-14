// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public interface ISocketSenderMessageQueueRingPoller : IMessagePump, ISocketDispatcherSender
{
    new int    UsageCount { get; }
    new string Name       { get; set; }
    new void   Start(Action? threadInitializer = null);
    new void   Stop();
}

public class SocketAsyncValueTaskEventQueueSender : SocketAsyncValueTaskRingPollerSender<BusMessage>, ISocketSenderMessageQueueRingPoller
{
    public SocketAsyncValueTaskEventQueueSender
    (IQueueMessageRing queueMessageRing, uint noDataPauseTimeoutMs, IRecycler? recycler = null
      , IEnumerableBatchPollSink<BusMessage>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(queueMessageRing, noDataPauseTimeoutMs, threadStartInitialization, parallelController)
    {
        PollSink                  = pollSink;
        ThreadStartInitialization = QueueMessageRing.InitializeInPollingThread;
    }

    public SocketAsyncValueTaskEventQueueSender
    (string name, int size, uint noDataPauseTimeoutMs, IRecycler? recycler = null
      , IEnumerableBatchPollSink<BusMessage>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : this(new QueueMessageRing(name, size), noDataPauseTimeoutMs, recycler, pollSink, threadStartInitialization, parallelController) { }

    public IEnumerableBatchPollSink<BusMessage>? PollSink { get; set; }

    public override void EnqueueSocketSender(ISocketSender socketSender)
    {
        var seqId = Ring.Claim();
        var evt   = Ring[seqId];
        evt.SetAsSocketSenderItem(socketSender);
        Ring.Publish(seqId);
        WakeIfAsleep();
        if (!IsRunning) Start();
    }

    public QueueContext QueueContext
    {
        get => QueueMessageRing.QueueContext;
        set => QueueMessageRing.QueueContext = value;
    }

    IEnqueueAsyncValueTaskPollingRing<BusMessage> IEnqueueAsyncValueTaskRingPoller<BusMessage>.Ring => (IQueueMessageRing)Ring;

    public IQueueMessageRing QueueMessageRing => (QueueMessageRing)Ring;

    public bool IsListeningOn(string address) => QueueMessageRing.IsListeningOn(address);

    public IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address) => QueueMessageRing.ListeningSubscriptionsOn(address);

    public int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo) => QueueMessageRing.CopyLivingRulesTo(toCopyTo);
}
