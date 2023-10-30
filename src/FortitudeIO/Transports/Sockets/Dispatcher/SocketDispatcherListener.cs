#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Logging;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets.Dispatcher;

public interface ISocketDispatcherListener : ISocketDispatcherCommon
{
    void RegisterForListen(ISocketSessionConnection cx);
    void UnregisterForListen(ISocketSessionConnection cx);
}

public class SocketDispatcherListener : SocketDispatcherBase, ISocketDispatcherListener
{
    private readonly DispatchContext dispatchContext = new();
    private readonly IPerfLoggerPool receiveSocketDispatcherLatencyTraceLoggerPool;
    private readonly ISocketSelector selector;
    private readonly ISocketDataLatencyLogger socketDataLatencyLogger;

    public SocketDispatcherListener(ISocketSelector socketSelector,
        string dispatcherDescription)
        : base(dispatcherDescription)
    {
        selector = socketSelector;
        socketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance
            .GetSocketDataLatencyLogger(base.DispatcherDescription);
        receiveSocketDispatcherLatencyTraceLoggerPool =
            PerfLoggingPoolFactory.Instance
                .GetLatencyTracingLoggerPool(base.DispatcherDescription + ".Receive",
                    //  Heartbeats are normally set at 1 second so wait just over 1 second on select.
                    TimeSpan.FromMilliseconds(1100), typeof(ISocketDispatcher));
    }

    protected override string WorkerThreadName => "SocketReceivingThread";

    public void RegisterForListen(ISocketSessionConnection cx)
    {
        cx.Active = true;
        selector.Register(cx);
    }

    public void UnregisterForListen(ISocketSessionConnection cx)
    {
        cx.Active = false;
        selector.Unregister(cx);
    }

    protected override void DispatchWorker()
    {
        Receive();
    }

    protected override void CleanupForStop(IOSThread workerThread)
    {
        workerThread.Join();
    }

    private void Receive()
    {
        Logger.Info(Thread.CurrentThread.Name + " started");
        while (Running)
        {
            IPerfLogger? detectionToPublishLatencyTraceLogger = null;
            var numSockets = 0;
            try
            {
                detectionToPublishLatencyTraceLogger = receiveSocketDispatcherLatencyTraceLoggerPool.StartNewTrace();
                dispatchContext.DispatchLatencyLogger = detectionToPublishLatencyTraceLogger;
                numSockets = 0;
                var sccWithSelectReturn = selector.WatchSocketsForRecv(detectionToPublishLatencyTraceLogger);
                foreach (var scc in sccWithSelectReturn)
                {
                    numSockets++;
                    if (scc.SessionReceiver!.IsAcceptor)
                    {
                        scc.SessionReceiver.OnAccept();
                    }
                    else
                    {
                        bool connected;
                        try
                        {
                            dispatchContext.DetectTimestamp = selector.WakeTs;
                            connected = scc.SessionReceiver.ReceiveData(dispatchContext);
                        }
                        catch (Exception ex)
                        {
                            detectionToPublishLatencyTraceLogger.Indent();
                            detectionToPublishLatencyTraceLogger.Add("Read error: ", ex);
                            detectionToPublishLatencyTraceLogger.Dedent();
                            scc.OnError(scc, "Read error: " + ex, ex is SocketBufferTooFullException ? 0 : -1);
                            continue;
                        }

                        if (connected) continue;
                        detectionToPublishLatencyTraceLogger.Indent();
                        detectionToPublishLatencyTraceLogger.Add("Connection lost on dispatcher " +
                                                                 DispatcherDescription);
                        detectionToPublishLatencyTraceLogger.Dedent();
                        scc.OnError(scc, "Connection lost on dispatcher " + DispatcherDescription, -1);
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
                    socketDataLatencyLogger.ParseTraceLog(detectionToPublishLatencyTraceLogger);
                    receiveSocketDispatcherLatencyTraceLoggerPool.StopTrace(detectionToPublishLatencyTraceLogger);
                }
            }
        }

        Logger.Info(Thread.CurrentThread.Name + " stopped");
    }
}
