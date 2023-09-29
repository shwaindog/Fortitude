using System.Collections.Concurrent;

namespace FortitudeIO.Transports.Sockets.Logging
{
    class SocketDataLatencyLoggerFactory : ISocketDataLatencyLoggerFactory
    {
        private static volatile ISocketDataLatencyLoggerFactory instance;
        private static readonly object SyncLock = new object();

        private readonly ConcurrentDictionary<string, ISocketDataLatencyLogger> instanceTracker =
            new ConcurrentDictionary<string, ISocketDataLatencyLogger>();

        public static ISocketDataLatencyLoggerFactory Instance
        {
            get
            {
                if (instance != null) return instance;
                lock (SyncLock)
                {
                    if (instance == null)
                    {
                        instance = new SocketDataLatencyLoggerFactory();
                    }
                }
                return instance;
            }
            set => instance = value;
        }

        public ISocketDataLatencyLogger GetSocketDataLatencyLogger(string key)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            if (instanceTracker.TryGetValue(key, out var getInstance)) return getInstance;
            lock (SyncLock)
            {
                if (instanceTracker.TryGetValue(key, out getInstance)) return getInstance;
                getInstance = new SocketDataLatencyLogger(key);
                instanceTracker.TryAdd(key, getInstance);
            }
            return getInstance;
        }
    }
}
