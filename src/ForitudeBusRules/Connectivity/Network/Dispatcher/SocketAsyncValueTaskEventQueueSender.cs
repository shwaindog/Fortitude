#region

using FortitudeBusRules.Messaging;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public class SocketAsyncValueTaskEventQueueSender : SocketAsyncValueTaskRingPollerSender<Message>
{
    public SocketAsyncValueTaskEventQueueSender(IAsyncValueTaskPollingRing<Message> ring, uint noDataPauseTimeoutMs, IRecycler? recycler = null
        , IEnumerableBatchPollSink<Message>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController) =>
        PollSink = pollSink;

    public SocketAsyncValueTaskEventQueueSender(string name, int size, uint noDataPauseTimeoutMs, IRecycler? recycler = null
        , IEnumerableBatchPollSink<Message>? pollSink = null,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : this(new AsyncValueValueTaskPollingRing<Message>(name, size, () => new Message(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, recycler, pollSink, threadStartInitialization, parallelController) { }

    public IEnumerableBatchPollSink<Message>? PollSink { get; set; }

    public override void EnqueueSocketSender(ISocketSender socketSender)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SetAsSocketSenderItem(socketSender);
        WakeIfAsleep();
        if (!IsRunning) Start();
    }
}
