#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Transports.Sockets;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public abstract class SocketDispatcherBase : ISocketDispatcherCommon
{
    private static long uid;
    private readonly object initLock = new();
    protected readonly IFLogger Logger;
    protected readonly IOSParallelController ParallelController;
    private readonly IConfigurationSection socketConfigurationRepository = SocketsConfigurationContext.Instance;
    protected volatile bool Running;
    private IOSThread? workerThread;

    protected SocketDispatcherBase(string dispatcherDescription)
    {
        ParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
        Logger = FLoggerFactory.Instance.GetLogger(socketConfigurationRepository["LoggerName"]!);
        // ReSharper disable once VirtualMemberCallInConstructor
        DispatcherDescription = dispatcherDescription;
    }

    protected abstract string WorkerThreadName { get; }

    public int UsageCount { get; private set; }

    public virtual string DispatcherDescription { get; set; }

    public virtual void Start()
    {
        lock (initLock)
        {
            ++UsageCount;
            if (!Running)
            {
                Running = true;

                var id = Interlocked.Increment(ref uid);

                workerThread = ParallelController.CreateNewOSThread(DispatchWorker);
                workerThread.Name = WorkerThreadName + id;
                workerThread.IsBackground = true;
                workerThread.Start();
            }
        }
    }

    public virtual void Stop()
    {
        lock (initLock)
        {
            --UsageCount;
            if (UsageCount != 0 || !Running) return;
            Running = false;
            if (ParallelController.CurrentOSThread.Name == workerThread!.Name) return;
            Logger.Info(workerThread.Name + " waited on");
            CleanupForStop(workerThread);
        }
    }

    protected abstract void DispatchWorker();

    protected abstract void CleanupForStop(IOSThread workerThread);
}
