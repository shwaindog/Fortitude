#region

using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public abstract class SocketBatchEnumerableRingPollerListener<T> : EnumerableBatchRingPoller<T>, ISocketDispatcherListener
    where T : class, ICanCarrySocketReceiverPayload
{
    protected readonly IFLogger Logger;
    private readonly IIntraOSThreadSignal manualResetEvent;
    protected readonly IOSParallelController ParallelController;
    private readonly SocketsPollerAndDecoding socketsPollerAndDecoding;

    protected SocketBatchEnumerableRingPollerListener(IEnumerableBatchPollingRing<T> ring, uint noDataPauseTimeoutMs, ISocketSelector selector,
        IUpdateableTimer backingTimer, Action? threadStartInitialization = null, IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, threadStartInitialization, parallelController)
    {
        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
        manualResetEvent = ParallelController.AllWaitingOSThreadActivateSignal(false);
        Name = Ring.Name + "-SocketRingPollerListener";
        socketsPollerAndDecoding = new SocketsPollerAndDecoding(Name, selector, manualResetEvent, new ActionListTimer(backingTimer, Recycler));
        Logger = FLoggerFactory.Instance.GetLogger("FortitudeIO.Transports.Network.Dispatcher.SocketRingPollerListener." + Name);
    }

    public override void Stop()
    {
        manualResetEvent.Set();
        base.Stop();
    }

    public void RegisterForListen(ISocketReceiver receiver)
    {
        EnqueueSocketReceiver(receiver, true);
    }

    public void UnregisterForListen(ISocketReceiver receiver)
    {
        EnqueueSocketReceiver(receiver, false);
    }

    public override int UsageCount => socketsPollerAndDecoding.CountRegisteredReceivers;

    public void RegisterForListen(IStreamListener receiver)
    {
        if (receiver is ISocketReceiver socketReceiver) RegisterForListen(socketReceiver);
    }

    public void UnregisterForListen(IStreamListener receiver)
    {
        if (receiver is ISocketReceiver socketReceiver)
        {
            socketReceiver.ListenActive = false;
            UnregisterForListen(socketReceiver);
        }
    }

    protected abstract void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd);

    protected void RunPolling()
    {
        if (!IsRunning) Start();
        manualResetEvent.Set();
    }

    protected void StopPolling()
    {
        manualResetEvent.Reset();
    }

    protected override void PollAttempt()
    {
        manualResetEvent.WaitOne();
        if (!IsRunning) return;
        socketsPollerAndDecoding.PollSocketsAndDecodeData();

        ProcessEventMessages();
    }

    private void ProcessEventMessages()
    {
        foreach (var data in Ring)
            try
            {
                if (data.IsSocketReceiverItem)
                {
                    var socketReceiver = data.SocketReceiver;
                    if (socketReceiver != null)
                    {
                        if (data.IsSocketAdd)
                            socketsPollerAndDecoding.AddForListen(socketReceiver);
                        else
                            socketsPollerAndDecoding.RemoveFromListen(socketReceiver);
                    }
                }
                else
                {
                    Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($"SocketRingPollerListener '{Ring.Name}' caught exception while processing event: {data}.  {ex}");
            }
    }

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch) { }
}

public class SimpleSocketBatchEnumerableRingPollerListener : SocketBatchEnumerableRingPollerListener<SimpleSocketReceiverPayload>
{
    public SimpleSocketBatchEnumerableRingPollerListener(IEnumerableBatchPollingRing<SimpleSocketReceiverPayload> ring
        , uint noDataPauseTimeoutMs
        , ISocketSelector selector, IUpdateableTimer timer, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, selector, timer, threadStartInitialization, parallelController) { }

    public SimpleSocketBatchEnumerableRingPollerListener(string name, uint noDataPauseTimeoutMs, ISocketSelector selector
        , IUpdateableTimer timer, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(new EnumerableBatchPollingRing<SimpleSocketReceiverPayload>(name, 13
            , () => new SimpleSocketReceiverPayload(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, selector, timer, threadStartInitialization, parallelController) { }

    protected override void EnqueueSocketReceiver(ISocketReceiver receiver, bool isAdd)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SetAsSocketReceiverItem(receiver, isAdd);
        receiver.ListenActive = isAdd;
        Ring.Publish(seqId);
        RunPolling();
    }
}
