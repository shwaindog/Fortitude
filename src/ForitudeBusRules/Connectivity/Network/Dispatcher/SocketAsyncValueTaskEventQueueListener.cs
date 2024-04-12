#region

using FortitudeBusRules.Messaging;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public class SocketAsyncValueTaskEventQueueListener : SocketAsyncValueTaskRingPollerListener<Message>
{
    public SocketAsyncValueTaskEventQueueListener(IAsyncValueTaskPollingRing<Message> ring, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IRecycler? recycler = null, IEnumerableBatchPollSink<Message>? pollSink = null, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, selector, threadStartInitialization, parallelController) =>
        PollSink = pollSink;

    public SocketAsyncValueTaskEventQueueListener(string name, int size, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IRecycler? recycler = null, IEnumerableBatchPollSink<Message>? pollSink = null, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : this(new AsyncValueValueTaskPollingRing<Message>(name, size, () => new Message(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, selector, recycler, pollSink, threadStartInitialization, parallelController) { }

    public IEnumerableBatchPollSink<Message>? PollSink { get; set; }

    protected override void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SetAsSocketReceiverItem(receiver, isAdd);
        Ring.Publish(seqId);
        RunPolling();
    }
}
