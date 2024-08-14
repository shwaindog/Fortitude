// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public interface ISocketListenerMessageQueueRingPoller : IMessagePump, ISocketDispatcherListener
{
    new int    UsageCount { get; }
    new string Name       { get; set; }
    new void   Start(Action? threadInitializer = null);
    new void   Stop();
}

public class SocketAsyncValueTaskEventQueueListener : SocketAsyncValueTaskRingPollerListener<BusMessage>, ISocketListenerMessageQueueRingPoller
{
    public SocketAsyncValueTaskEventQueueListener
    (IQueueMessageRing queueMessageRing, uint noDataPauseTimeoutMs, ISocketSelector selector
      , IUpdateableTimer timer, IRecycler? recycler = null, IEnumerableBatchPollSink<BusMessage>? pollSink = null
      , Action? threadStartInitialization = null
      , IOSParallelController? parallelController = null)
        : base(queueMessageRing, noDataPauseTimeoutMs, selector, timer, threadStartInitialization, parallelController)
    {
        PollSink                  = pollSink;
        ThreadStartInitialization = QueueMessageRing.InitializeInPollingThread;
    }

    public SocketAsyncValueTaskEventQueueListener
    (string name, int size, uint noDataPauseTimeoutMs, ISocketSelector selector
      , IUpdateableTimer timer, IRecycler? recycler = null, IEnumerableBatchPollSink<BusMessage>? pollSink = null
      , Action? threadStartInitialization = null
      , IOSParallelController? parallelController = null)
        : this(new QueueMessageRing(name, size), noDataPauseTimeoutMs, selector, timer, recycler, pollSink, threadStartInitialization
             , parallelController) { }

    public IEnumerableBatchPollSink<BusMessage>? PollSink { get; set; }

    public QueueContext QueueContext
    {
        get => QueueMessageRing.QueueContext;
        set => QueueMessageRing.QueueContext = value;
    }

    public IQueueMessageRing QueueMessageRing => (QueueMessageRing)Ring;

    public bool IsListeningOn(string address) => QueueMessageRing.IsListeningOn(address);

    public IEnumerable<IMessageListenerRegistration> ListeningSubscriptionsOn(string address) => QueueMessageRing.ListeningSubscriptionsOn(address);

    public int CopyLivingRulesTo(IAutoRecycleEnumerable<IRule> toCopyTo) => QueueMessageRing.CopyLivingRulesTo(toCopyTo);

    protected override void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd)
    {
        var seqId = Ring.Claim();
        var evt   = Ring[seqId];
        evt.SetAsSocketReceiverItem(receiver, isAdd);
        Ring.Publish(seqId);
        RunPolling();
    }
}
