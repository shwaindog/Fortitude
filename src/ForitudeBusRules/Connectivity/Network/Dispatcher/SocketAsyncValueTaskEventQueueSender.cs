// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public interface ISocketSenderMessageQueueRingPoller : IAsyncValueTaskRingPoller<BusMessage>, ISocketDispatcherSender
{
    new int    UsageCount { get; }
    new string Name       { get; set; }
    new void   Start(Action? threadInitializer = null);
    new void   Stop();
}

public class SocketAsyncValueTaskEventQueueSender : SocketAsyncValueTaskRingPollerSender<BusMessage>, ISocketSenderMessageQueueRingPoller
{
    public SocketAsyncValueTaskEventQueueSender
    (IAsyncValueTaskPollingRing<BusMessage> ring, uint noDataPauseTimeoutMs, IRecycler? recycler = null
      , IEnumerableBatchPollSink<BusMessage>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController) =>
        PollSink = pollSink;

    public SocketAsyncValueTaskEventQueueSender
    (string name, int size, uint noDataPauseTimeoutMs, IRecycler? recycler = null
      , IEnumerableBatchPollSink<BusMessage>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : this(new AsyncValueTaskPollingRing<BusMessage>(name, size, () => new BusMessage(),
                                                         ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, recycler, pollSink
             , threadStartInitialization, parallelController) { }

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
}
