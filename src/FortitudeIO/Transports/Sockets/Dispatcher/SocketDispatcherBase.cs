using System.Threading;
using FortitudeCommon.Configuration.KeyValueProperties;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.AsyncWrappers;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.Dispatcher
{
    public abstract class SocketDispatcherBase : ISocketDispatcherCommon
    {
        private static long uid;
        private IOSThread workerThread;
        protected volatile bool Running;
        private readonly object initLock = new object();
        protected readonly IOSParallelController ParallelController;
        protected readonly IFLogger Logger;
        private readonly IConfigurationRepository socketConfigurationRepository = SocketsConfigurationContext.Instance;

        protected SocketDispatcherBase(string dispatcherDescription = null) 
        {
            ParallelController = OSParallelControllerFactory.Instance.GetOSParallelController;
            Logger = FLoggerFactory.Instance.GetLogger(socketConfigurationRepository.GetConfigItem("LoggerName").Value);
            // ReSharper disable once VirtualMemberCallInConstructor
            DispatcherDescription = dispatcherDescription;
        }
        
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

        protected abstract string WorkerThreadName { get; }

        protected abstract void DispatchWorker();

        public virtual void Stop()
        {
            lock (initLock)
            {
                --UsageCount;
                if (UsageCount != 0 || !Running) return;
                Running = false;
                if (ParallelController.CurrentOSThread.Name == workerThread.Name) return;
                Logger.Info(workerThread.Name + " waited on");
                CleanupForStop(workerThread);
            }
        }

        protected abstract void CleanupForStop(IOSThread workerThread);
    }
}