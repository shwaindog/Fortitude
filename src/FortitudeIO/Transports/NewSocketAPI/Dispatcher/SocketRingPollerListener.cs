#region

using FortitudeCommon.EventProcessing.Disruption.Rings.Batching;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI.Construction;
using FortitudeIO.Transports.NewSocketAPI.Logging;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public interface ISocketDispatcherListener : ISocketDispatcherCommon
{
    void RegisterForListen(ISocketReceiver receiver);
    void UnregisterForListen(ISocketReceiver receiver);
    void RegisterForListen(IStreamListener receiver);
    void UnregisterForListen(IStreamListener receiver);
}

public abstract class SocketRingPollerListener<T> : RingPollerBase<T>, ISocketDispatcherListener where T : class
{
    protected readonly IFLogger Logger;
    private readonly IIntraOSThreadSignal manualResetEvent;
    protected readonly IOSParallelController ParallelController;
    private readonly IPerfLoggerPool receiveSocketDispatcherLatencyTraceLoggerPool;
    private readonly ISocketSelector selector;
    private readonly SocketBufferReadContext socketBufferReadContext = new();
    private readonly IConfigurationSection socketConfigurationRepository = SocketsConfigurationContext.Instance;
    private readonly ISocketDataLatencyLogger? socketDataLatencyLogger;

    protected SocketRingPollerListener(IPollingRing<T> ring, uint noDataPauseTimeoutMs, ISocketSelector selector,
        Action? threadStartInitialization = null, IOSParallelController? parallelController = null) : base(ring, noDataPauseTimeoutMs
        , threadStartInitialization, parallelController)
    {
        this.selector = selector;
        Name = Ring.Name + "-SocketRingPollerListener";
        ParallelController = parallelController ?? OSParallelControllerFactory.Instance.GetOSParallelController;
        manualResetEvent = ParallelController.AllWaitingOSThreadActivateSignal(false);
        socketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance
            .GetSocketDataLatencyLogger(ring.Name);
        receiveSocketDispatcherLatencyTraceLoggerPool =
            PerfLoggingPoolFactory.Instance
                .GetLatencyTracingLoggerPool(Name + ".Receive",
                    //  Heartbeats are normally set at 1 second so wait just over 1 second on select.
                    TimeSpan.FromMilliseconds(1100), typeof(ISocketDispatcher));
        Logger = FLoggerFactory.Instance.GetLogger(socketConfigurationRepository["LoggerName"]!);
    }

    public override void Stop()
    {
        manualResetEvent.Set();
        base.Stop();
    }

    public abstract void RegisterForListen(ISocketReceiver receiver);

    public abstract void UnregisterForListen(ISocketReceiver receiver);

    public override int UsageCount => selector.CountRegisteredReceivers;

    public void RegisterForListen(IStreamListener receiver)
    {
        if (receiver is ISocketReceiver socketReceiver) RegisterForListen(socketReceiver);
    }

    public void UnregisterForListen(IStreamListener receiver)
    {
        if (receiver is ISocketReceiver socketReceiver) UnregisterForListen(socketReceiver);
    }

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
        PollSocketSelector();

        foreach (var data in Ring)
            try
            {
                if (IsSocketReceiverInRingData(data))
                {
                    var socketReceiver = ExtractSocketReceiverInRingData(data);
                    if (socketReceiver != null)
                    {
                        if (IsSocketReceiverAdd(data))
                            AddForListen(socketReceiver);
                        else
                            RemoveFromListen(socketReceiver);
                    }
                }

                Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
            }
            catch (Exception ex)
            {
                Logger.Warn($"SocketRingPollerListener '{Ring.Name}' caught exception while processing event: {data}.  {ex}");
            }
    }

    private void PollSocketSelector()
    {
        if (selector.CountRegisteredReceivers > 0)
        {
            IPerfLogger? detectionToPublishLatencyTraceLogger = null;
            var numSockets = 0;
            try
            {
                detectionToPublishLatencyTraceLogger = receiveSocketDispatcherLatencyTraceLoggerPool.StartNewTrace();
                socketBufferReadContext.DispatchLatencyLogger = detectionToPublishLatencyTraceLogger;
                numSockets = 0;
                var socketReceivers = selector.WatchSocketsForRecv(detectionToPublishLatencyTraceLogger);
                foreach (var sockRecr in socketReceivers)
                {
                    numSockets++;
                    ProcessSocketEvent(sockRecr, detectionToPublishLatencyTraceLogger);
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (detectionToPublishLatencyTraceLogger != null)
                {
                    detectionToPublishLatencyTraceLogger.AddContextMeasurement(numSockets);
                    detectionToPublishLatencyTraceLogger.Add("End Processing Socket Data");
                    socketDataLatencyLogger?.ParseTraceLog(detectionToPublishLatencyTraceLogger);
                    receiveSocketDispatcherLatencyTraceLoggerPool.StopTrace(detectionToPublishLatencyTraceLogger);
                }
            }
        }
    }

    private void ProcessSocketEvent(ISocketReceiver sockRecr, IPerfLogger detectionToPublishLatencyTraceLogger)
    {
        if (sockRecr.IsAcceptor)
        {
            sockRecr.NewClientSocketRequest();
        }
        else
        {
            bool connected;
            try
            {
                socketBufferReadContext.DetectTimestamp = selector.WakeTs;
                connected = sockRecr.Poll(socketBufferReadContext);
            }
            catch (Exception ex)
            {
                detectionToPublishLatencyTraceLogger.Indent();
                detectionToPublishLatencyTraceLogger.Add("Read error: ", ex);
                detectionToPublishLatencyTraceLogger.Dedent();
                sockRecr.HandleReceiveError("Read error: ", ex);
                return;
            }

            if (connected) return;
            detectionToPublishLatencyTraceLogger.Indent();
            detectionToPublishLatencyTraceLogger.Add("Connection lost on SocketRingPoller " + Name);
            detectionToPublishLatencyTraceLogger.Dedent();
            sockRecr.HandleReceiveError("Connection lost on SocketRingPoller " + Name
                , new Exception("Connection Lost to SocketRingPoller " + Name));
        }
    }

    public void AddForListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = true;
        selector.Register(receiver);
        manualResetEvent.Set();
    }

    public void RemoveFromListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = false;
        if (selector.HasRegisteredReceiver(receiver) && selector.CountRegisteredReceivers == 1)
            manualResetEvent.Reset();
        selector.Unregister(receiver);
        if (selector.CountRegisteredReceivers > 0) manualResetEvent.Set();
    }

    protected abstract bool IsSocketReceiverInRingData(T ringData);

    protected abstract bool IsSocketReceiverAdd(T ringData);

    protected abstract ISocketReceiver? ExtractSocketReceiverInRingData(T ringData);

    protected override void Processor(long ringCurrentSequence, long ringCurrentBatchSize, T data, bool ringStartOfBatch
        , bool ringEndOfBatch) { }
}

public enum SocketReceiverCommand
{
    Add
    , Remove
}

public class SocketReceiverUpdate
{
    public ISocketReceiver? SocketReceiver { get; set; }
    public SocketReceiverCommand SocketReceiverCommand { get; set; } = SocketReceiverCommand.Add;
}

public class SimpleSocketRingPollerListener : SocketRingPollerListener<SocketReceiverUpdate>
{
    public SimpleSocketRingPollerListener(IPollingRing<SocketReceiverUpdate> ring, uint noDataPauseTimeoutMs
        , ISocketSelector selector, Action? threadStartInitialization = null,
        IOSParallelController? parallelController = null)
        : base(ring, noDataPauseTimeoutMs, selector, threadStartInitialization, parallelController) { }

    public SimpleSocketRingPollerListener(string name, uint noDataPauseTimeoutMs, ISocketSelector selector, Action? threadStartInitialization = null
        , IOSParallelController? parallelController = null)
        : base(new PollingRing<SocketReceiverUpdate>(name, 13, () => new SocketReceiverUpdate(),
            ClaimStrategyType.MultiProducers), noDataPauseTimeoutMs, selector, threadStartInitialization, parallelController) { }

    public override void RegisterForListen(ISocketReceiver receiver)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SocketReceiver = receiver;
        evt.SocketReceiverCommand = SocketReceiverCommand.Add;
        Ring.Publish(seqId);
        RunPolling();
    }

    public override void UnregisterForListen(ISocketReceiver receiver)
    {
        var seqId = Ring.Claim();
        var evt = Ring[seqId];
        evt.SocketReceiver = receiver;
        evt.SocketReceiverCommand = SocketReceiverCommand.Remove;
        Ring.Publish(seqId);
        RunPolling();
    }

    protected override bool IsSocketReceiverInRingData(SocketReceiverUpdate ringData) => true;

    protected override bool IsSocketReceiverAdd(SocketReceiverUpdate ringData) => ringData.SocketReceiverCommand == SocketReceiverCommand.Add;

    protected override ISocketReceiver? ExtractSocketReceiverInRingData(SocketReceiverUpdate ringData) => ringData?.SocketReceiver;
}
