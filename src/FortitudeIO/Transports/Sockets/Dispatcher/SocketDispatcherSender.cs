#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Transports.Sockets.Dispatcher;

public interface ISocketDispatcherSender : ISocketDispatcherCommon
{
    void AddToSendQueue(ISocketSessionConnection cx);
    void AddToSendQueue(IDoublyLinkedList<ISocketSessionConnection> cxs);
}

public sealed class SocketDispatcherSender(string dispatcherDescription) : SocketDispatcherBase(dispatcherDescription)
    , ISocketDispatcherSender
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private IIntraOSThreadSignal canSend =
        OSParallelControllerFactory.Instance.GetOSParallelController.SingleOSThreadActivateSignal(false);

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private ISyncLock sendLock = new SpinLockLight();

    private IDictionary<long, ISocketSessionConnection> toWrite = new Dictionary<long, ISocketSessionConnection>();
    private IDictionary<long, ISocketSessionConnection> writing = new Dictionary<long, ISocketSessionConnection>();

    protected override string WorkerThreadName => "SocketSendingThread";

    public void AddToSendQueue(ISocketSessionConnection cx)
    {
        sendLock.Acquire();
        try
        {
            toWrite[cx.Id] = cx;
        }
        finally
        {
            sendLock.Release();
        }

        canSend.Set();
    }

    public void AddToSendQueue(IDoublyLinkedList<ISocketSessionConnection> cxs)
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
    }

    protected override void DispatchWorker()
    {
        Send();
    }

    protected override void CleanupForStop(IOSThread workerThread)
    {
        canSend.Set();
        workerThread.Join();
    }

    private void Send()
    {
        Logger.Info(Thread.CurrentThread.Name + " started");
        while (Running)
        {
            canSend.WaitOne();
            sendLock.Acquire();
            try
            {
                (toWrite, writing) = (writing, toWrite);
            }
            finally
            {
                sendLock.Release();
            }

            foreach (var cx in writing.Values)
            {
                if (!cx.Active) continue;
                try
                {
                    var sent = cx.SessionSender!.SendData();
                    if (!sent) AddToSendQueue(cx);
                }
                catch (Exception ex)
                {
                    cx.OnError(cx, "Write error: " + ex, -1);
                }
            }

            writing.Clear();
        }

        Logger.Info(Thread.CurrentThread.Name + " stopped");
    }
}
