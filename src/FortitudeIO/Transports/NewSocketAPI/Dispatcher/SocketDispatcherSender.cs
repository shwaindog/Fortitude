using System;
using System.Collections.Generic;
using System.Threading;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.NewSocketAPI.Publishing;

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher
{
    public interface ISocketDispatcherSender : ISocketDispatcherCommon
    {
        void AddToSendQueue(ISocketSender ss);
    }
    public sealed class SocketDispatcherSender : SocketDispatcherBase, ISocketDispatcherSender
    {
        private readonly ISyncLock sendLock = new SpinLockLight();
        private readonly IIntraOSThreadSignal canSend;

        private IDictionary<long, ISocketSender> toWrite = new Dictionary<long, ISocketSender>();
        private IDictionary<long, ISocketSender> writing = new Dictionary<long, ISocketSender>();

        public SocketDispatcherSender(string dispatcherDescription = null) 
            : base(dispatcherDescription)
        {
            canSend = OSParallelControllerFactory.Instance.GetOSParallelController.SingleOSThreadActivateSignal(false);
        }
        
        protected override string WorkerThreadName => "SocketSendingThread";

        protected override void DispatchWorker()
        {
            Send();
        }

        protected override void CleanupForStop(IOSThread workerThread)
        {
            canSend.Set();
            workerThread.Join();
        }
        
        public void AddToSendQueue(ISocketSender ss)
        {
            sendLock.Acquire();
            try
            {
                toWrite[ss.Id] = ss;
            }
            finally
            {
                sendLock.Release();
            }
            canSend.Set();
        }

/*        public void AddToSendQueue(IDoublyLinkedList<ISocketSender> cxs)
        {
            sendLock.Acquire();
            try
            {
                foreach (var cx in cxs) toWrite[cx.Id] = cx;
            }
            finally
            {
                sendLock.Release();
            }
            canSend.Set();
        }*/

        private void Send()
        {
            Logger.Info(Thread.CurrentThread.Name + " started");
            while (Running)
            {
                canSend.WaitOne();
                sendLock.Acquire();
                try
                {
                    var swap = toWrite;
                    toWrite = writing;
                    writing = swap;
                }
                finally
                {
                    sendLock.Release();
                }
                foreach (var ss in writing.Values)
                {
                    if (!ss.SendActive) continue;
                    try
                    {
                        var sent = ss.SendEnqueued();
                        if (!sent) AddToSendQueue(ss);
                    }
                    catch (Exception ex)
                    {
                        ss.HandleSendError("Write error: ", ex);
                    }
                }
                writing.Clear();
            }
            Logger.Info(Thread.CurrentThread.Name + " stopped");
        }
    }
}