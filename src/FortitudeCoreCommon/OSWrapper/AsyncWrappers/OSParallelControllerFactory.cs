namespace FortitudeCommon.OSWrapper.AsyncWrappers;

public class OSParallelControllerFactory : IOSParallelControllerFactory
{
    private static volatile IOSParallelControllerFactory? instance;
    private static readonly object SyncLock = new();

    private volatile IOSParallelController? osParallelController;

    public static IOSParallelControllerFactory Instance
    {
        get
        {
            if (instance != null) return instance;
            lock (SyncLock)
            {
                // ReSharper disable once NonAtomicCompoundOperator
                instance ??= new OSParallelControllerFactory();
            }

            return instance;
        }
        set => instance = value;
    }

    public IOSParallelController GetOSParallelController
    {
        get
        {
            if (osParallelController != null) return osParallelController;
            lock (SyncLock)
            {
                // ReSharper disable once NonAtomicCompoundOperator
                osParallelController ??= new OSParallelController();
            }

            return osParallelController;
        }
    }
}
