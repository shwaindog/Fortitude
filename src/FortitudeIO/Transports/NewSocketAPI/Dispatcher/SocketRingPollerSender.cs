#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.NewSocketAPI.Publishing;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public interface ISocketDispatcherSender : ISocketDispatcherCommon
{
    void AddToSendQueue(ISocketSender ss);
}

public abstract class SocketRingPollerSender<T> : RingPollerBase<T>, ISocketDispatcherSender where T : class
{
    protected readonly IOSParallelController ParallelController;

    protected SocketRingPollerSender(IPollingRing<T> ring, uint noDataPauseTimeoutMs,
        IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs, parallelController)
    {
        Name = Ring.Name + "-SocketRingPollerSender";
        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
    }

    public abstract void AddToSendQueue(ISocketSender ss);

    protected void EnqueueSocketSenderToSocket(ISocketSender ss)
    {
        var sent = ss.SendQueued();
        if (!sent) AddToSendQueue(ss);
    }
}

public class SocketSenderContainer
{
    public ISocketSender? SocketSender { get; set; }
}

public class SimpleSocketRingPollerSender : SocketRingPollerSender<SocketSenderContainer>
{
    public SimpleSocketRingPollerSender(IPollingRing<SocketSenderContainer> ring, uint noDataPauseTimeoutMs,
        IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs, parallelController) { }

    public SimpleSocketRingPollerSender(string name, uint noDataPauseTimeoutMs
        , IOSParallelController? parallelController = null)
        : base(new PollingRing<SocketSenderContainer>(name, 4097, () => new SocketSenderContainer(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, parallelController) { }

    public override void AddToSendQueue(ISocketSender ss)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SocketSender = ss;
        Ring.Publish(seqId);
        WakeIfAsleep();
    }

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, SocketSenderContainer data
        , bool ringStartOfBatch, bool ringEndOfBatch)
    {
        EnqueueSocketSenderToSocket(data.SocketSender!);
    }
}
