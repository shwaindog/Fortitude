using System;
using System.Threading;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Receiving;
using FortitudeIO.Transports.Sockets.Logging;
using ISocketSelector = FortitudeIO.Transports.NewSocketAPI.Receiving.ISocketSelector;

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher
{
    public interface ISocketDispatcherListener : ISocketDispatcherCommon
    {
        void RegisterForListen(ISocketReceiver receiver);
        void UnregisterForListen(ISocketReceiver receiver);
    }
    public class SocketDispatcherListener : SocketDispatcherBase, ISocketDispatcherListener
    {
        private readonly ISocketSelector selector;
        private IPerfLoggerPool receiveSocketDispatcherLatencyTraceLoggerPool;
        private ISocketDataLatencyLogger socketDataLatencyLogger;
        private readonly DispatchContext dispatchContext = new DispatchContext();
        private readonly IIntraOSThreadSignal manualResetEvent;

        public SocketDispatcherListener(ISocketSelector socketSelector,
            string dispatcherDescription = null) 
            : base(dispatcherDescription)
        {
            selector = socketSelector;
            DispatcherDescription = dispatcherDescription ?? "NoDescriptionGiven";
            manualResetEvent = ParallelController.AllWaitingOSThreadActivateSignal(false);
        }
        
        public override string DispatcherDescription
        {
            get => base.DispatcherDescription;
            set
            {
                base.DispatcherDescription = value;
                if (DispatcherDescription == null) return;
                socketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance
                    .GetSocketDataLatencyLogger(DispatcherDescription);
                receiveSocketDispatcherLatencyTraceLoggerPool =
                    PerfLoggingPoolFactory.Instance
                        .GetLatencyTracingLoggerPool(DispatcherDescription + ".Receive",
                            //  Heartbeats are normally set at 1 second so wait just over 1 second on select.
                            TimeSpan.FromMilliseconds(1100), typeof(ISocketDispatcher));
            }
        }
        
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
            {
                manualResetEvent.Reset();
            }
            selector.Unregister(receiver);
            if (selector.CountRegisteredReceivers > 0)
            {
                manualResetEvent.Set();
            }
        }

        protected override string WorkerThreadName => "SocketReceivingThread";

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
                IPerfLogger detectionToPublishLatencyTraceLogger = null;
                var numSockets = 0;
                try
                {
                    detectionToPublishLatencyTraceLogger = receiveSocketDispatcherLatencyTraceLoggerPool.StartNewTrace();
                    dispatchContext.DispatchLatencyLogger = detectionToPublishLatencyTraceLogger;
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
                                dispatchContext.DetectTimestamp = selector.WakeTs;
                                connected = sockRecr.Poll(dispatchContext);
                            }
                            catch (Exception ex)
                            {
                                detectionToPublishLatencyTraceLogger.Indent();
                                detectionToPublishLatencyTraceLogger.Add("Read error: ", ex);
                                detectionToPublishLatencyTraceLogger.Dedent();
                                sockRecr.HandleReceiveError( "Read error: ", ex);
                                continue;
                            }
                            if (connected) continue;
                            detectionToPublishLatencyTraceLogger.Indent();
                            detectionToPublishLatencyTraceLogger.Add("Connection lost on dispatcher " +
                                                                     DispatcherDescription);
                            detectionToPublishLatencyTraceLogger.Dedent();
                            sockRecr.HandleReceiveError("Connection lost on dispatcher " + DispatcherDescription, new Exception("Connection Lost to Dispatcher " + DispatcherDescription));
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
}