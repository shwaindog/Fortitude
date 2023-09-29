namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public class OSParallelControllerFactory : IOSParallelControllerFactory
    {
        private static volatile IOSParallelControllerFactory instance;
        private static readonly object SyncLock = new object();

        private volatile IOSParallelController osParallelController;

        public static IOSParallelControllerFactory Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (SyncLock)
                {
                    if (instance == null)
                    {
                        instance = new OSParallelControllerFactory();
                    }
                }
                return instance;
            }
            set { instance = value; }
        }

        public IOSParallelController GetOSParallelController
        {
            get
            {
                if (osParallelController != null) return osParallelController;
                lock (SyncLock)
                {
                    if (osParallelController == null)
                    {
                        osParallelController = new OSParallelController();
                    }
                }
                return osParallelController;
            }
        }
    }
}
