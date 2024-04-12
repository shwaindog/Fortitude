#region

using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public abstract class SocketBatchEnumerableRingPollerSender<T> : EnumerableBatchRingPoller<T>, ISocketDispatcherSender
    where T : class, ICanCarrySocketSenderPayload
{
    protected readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketBatchEnumerableRingPollerSender<>));
    protected readonly IOSParallelController ParallelController;

    protected SocketBatchEnumerableRingPollerSender(IEnumerableBatchPollingRing<T> ring, uint noDataPauseTimeoutMs
        , Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController)
    {
        Name = Ring.Name + "-SocketRingPollerSender";
        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
    }

    public abstract void EnqueueSocketSender(ISocketSender socketSender);

    protected void PutQueuedSenderMessageOnSocket(ISocketSender ss)
    {
        try
        {
            var sent = ss.SendQueued();
            if (!sent) EnqueueSocketSender(ss);
        }
        catch (SocketSendException se)
        {
            se.SocketSessionContext.OnSessionFailure($"Caught socket send error {se}");
        }
    }
}

public class SimpleSocketBatchEnumerableRingPollerSender : SocketBatchEnumerableRingPollerSender<SimpleSocketSenderPayload>
{
    public SimpleSocketBatchEnumerableRingPollerSender(IEnumerableBatchPollingRing<SimpleSocketSenderPayload> ring
        , uint noDataPauseTimeoutMs
        , Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController) { }

    public SimpleSocketBatchEnumerableRingPollerSender(string name, uint noDataPauseTimeoutMs, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(new EnumerableBatchPollingRing<SimpleSocketSenderPayload>(name, 4097, () => new SimpleSocketSenderPayload(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, threadStartInitialization, parallelController) { }

    public override void EnqueueSocketSender(ISocketSender socketSender)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SetAsSocketSenderItem(socketSender);
        Ring.Publish(seqId);
        WakeIfAsleep();
        if (!IsRunning) Start();
    }

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, SimpleSocketSenderPayload data
        , bool ringStartOfBatch, bool ringEndOfBatch)
    {
        PutQueuedSenderMessageOnSocket(data.SocketSender!);
    }
}
