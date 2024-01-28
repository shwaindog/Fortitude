#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeCommon.Serdes;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using FortitudeIO.Transports.Sockets.Logging;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public interface ISocketDispatcherListener : ISocketDispatcherCommon
{
    void RegisterForListen(ISocketReceiver receiver);
    void UnregisterForListen(ISocketReceiver receiver);
}

public class SocketDispatcherListener : SocketDispatcherBase, ISocketDispatcherListener
{
    private readonly IIntraOSThreadSignal manualResetEvent;
    private readonly ReadSocketBufferContext readSocketBufferContext = new() { Direction = ContextDirection.Read };
    private readonly IPerfLoggerPool receiveSocketDispatcherLatencyTraceLoggerPool;
    private readonly ISocketSelector selector;
    private readonly ISocketDataLatencyLogger? socketDataLatencyLogger;

    public SocketDispatcherListener(ISocketSelector socketSelector,
        string dispatcherDescription)
        : base(dispatcherDescription)
    {
        selector = socketSelector;
        manualResetEvent = ParallelController.AllWaitingOSThreadActivateSignal(false);
        socketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance
            .GetSocketDataLatencyLogger(dispatcherDescription);
        receiveSocketDispatcherLatencyTraceLoggerPool =
            PerfLoggingPoolFactory.Instance
                .GetLatencyTracingLoggerPool(dispatcherDescription + ".Receive",
                    //  Heartbeats are normally set at 1 second so wait just over 1 second on select.
                    TimeSpan.FromMilliseconds(1100), typeof(ISocketDispatcher));
    }

    protected override string WorkerThreadName => "SocketReceivingThread";

    public void RegisterForListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = true;
        selector.Register(receiver);
        manualResetEvent.Set();
    }

    public void UnregisterForListen(ISocketReceiver receiver)
    {
        receiver.ListenActive = false;
        if (selector.HasRegisteredReceiver(receiver) && selector.CountRegisteredReceivers == 1)
            manualResetEvent.Reset();
        selector.Unregister(receiver);
        if (selector.CountRegisteredReceivers > 0) manualResetEvent.Set();
    }

    protected override void DispatchWorker()
    {
        Receive();
    }

    protected override void CleanupForStop(IOSThread workerThread)
    {
        manualResetEvent.Set();
        workerThread.Join();
    }

    private void Receive()
    {
        Logger.Info(Thread.CurrentThread.Name + " started");
        while (Running)
        {
            manualResetEvent.WaitOne();
            if (!Running) return;
            IPerfLogger? detectionToPublishLatencyTraceLogger = null;
            var numSockets = 0;
            try
            {
                detectionToPublishLatencyTraceLogger = receiveSocketDispatcherLatencyTraceLoggerPool.StartNewTrace();
                readSocketBufferContext.DispatchLatencyLogger = detectionToPublishLatencyTraceLogger;
                numSockets = 0;
                var socketReceivers = selector.WatchSocketsForRecv(detectionToPublishLatencyTraceLogger);
                foreach (var sockRecr in socketReceivers)
                {
                    numSockets++;
                    if (sockRecr.IsAcceptor)
                    {
                        sockRecr.NewClientSocketRequest();
                    }
                    else
                    {
                        bool connected;
                        try
                        {
                            readSocketBufferContext.DetectTimestamp = selector.WakeTs;
                            connected = sockRecr.Poll(readSocketBufferContext);
                        }
                        catch (Exception ex)
                        {
                            detectionToPublishLatencyTraceLogger.Indent();
                            detectionToPublishLatencyTraceLogger.Add("Read error: ", ex);
                            detectionToPublishLatencyTraceLogger.Dedent();
                            sockRecr.HandleReceiveError("Read error: ", ex);
                            continue;
                        }

                        if (connected) continue;
                        detectionToPublishLatencyTraceLogger.Indent();
                        detectionToPublishLatencyTraceLogger.Add("Connection lost on dispatcher " +
                                                                 DispatcherDescription);
                        detectionToPublishLatencyTraceLogger.Dedent();
                        sockRecr.HandleReceiveError("Connection lost on dispatcher " + DispatcherDescription
                            , new Exception("Connection Lost to Dispatcher " + DispatcherDescription));
                    }
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

        Logger.Info(Thread.CurrentThread.Name + " stopped");
    }
}
