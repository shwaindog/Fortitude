#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
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
    protected readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketRingPollerSender<>));
    protected readonly IOSParallelController ParallelController;

    protected SocketRingPollerSender(IPollingRing<T> ring, uint noDataPauseTimeoutMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController)
    {
        Name = Ring.Name + "-SocketRingPollerSender";
        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
    }

    public abstract void AddToSendQueue(ISocketSender ss);

    protected void PutQueuedSenderMessageOnSocket(ISocketSender ss)
    {
        try
        {
            var sent = ss.SendQueued();
            if (!sent) AddToSendQueue(ss);
        }
        catch (SocketSendException se)
        {
            se.SocketSessionContext.OnSessionFailure($"Caught socket send error {se}");
        }
    }
}

public class SocketSenderContainer
{
    public ISocketSender? SocketSender { get; set; }
}

public class SimpleSocketRingPollerSender : SocketRingPollerSender<SocketSenderContainer>
{
    public SimpleSocketRingPollerSender(IPollingRing<SocketSenderContainer> ring, uint noDataPauseTimeoutMs, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController) { }

    public SimpleSocketRingPollerSender(string name, uint noDataPauseTimeoutMs, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(new PollingRing<SocketSenderContainer>(name, 4097, () => new SocketSenderContainer(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, threadStartInitialization, parallelController) { }

    public override void AddToSendQueue(ISocketSender ss)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SocketSender = ss;
        Ring.Publish(seqId);
        WakeIfAsleep();
        if (!IsRunning) Start();
    }

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, SocketSenderContainer data
        , bool ringStartOfBatch, bool ringEndOfBatch)
    {
        PutQueuedSenderMessageOnSocket(data.SocketSender!);
    }
}
