#region

using FortitudeBusRules.Messages;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeBusRules.Connectivity.Network.Dispatcher;

public interface ISocketListenerMessageQueueRingPoller : IAsyncValueTaskRingPoller<BusMessage>, ISocketDispatcherListener
{
    new int UsageCount { get; }
    new string Name { get; set; }
    new void Start(Action? threadInitializer = null);
    new void Stop();
}

public class SocketAsyncValueTaskEventQueueListener : SocketAsyncValueTaskRingPollerListener<BusMessage>, ISocketListenerMessageQueueRingPoller
{
    public SocketAsyncValueTaskEventQueueListener(IAsyncValueTaskPollingRing<BusMessage> ring, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IUpdateableTimer timer, IRecycler? recycler = null, IEnumerableBatchPollSink<BusMessage>? pollSink = null
        , Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, selector, timer, threadStartInitialization, parallelController) =>
        PollSink = pollSink;

    public SocketAsyncValueTaskEventQueueListener(string name, int size, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IUpdateableTimer timer, IRecycler? recycler = null, IEnumerableBatchPollSink<BusMessage>? pollSink = null
        , Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : this(new AsyncValueValueTaskPollingRing<BusMessage>(name, size, () => new BusMessage(),
                ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, selector, timer, recycler, pollSink, threadStartInitialization
            , parallelController) { }

    public IEnumerableBatchPollSink<BusMessage>? PollSink { get; set; }

    protected override void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SetAsSocketReceiverItem(receiver, isAdd);
        Ring.Publish(seqId);
        RunPolling();
    }
}
