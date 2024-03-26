#region

using System.Collections.Concurrent;
using FortitudeIO.Transports.NewSocketAPI.Logging;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Construction;

internal class SocketDataLatencyLoggerFactory : ISocketDataLatencyLoggerFactory
{
    private static volatile ISocketDataLatencyLoggerFactory? instance;
    private static readonly object SyncLock = new();

    private readonly ConcurrentDictionary<string, ISocketDataLatencyLogger> instanceTracker
        = new();

    public static ISocketDataLatencyLoggerFactory Instance
    {
        get
        {
            if (instance != null) return instance;
            lock (SyncLock)
            {
                instance ??= new SocketDataLatencyLoggerFactory();
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

    public static void ClearSocketDataLatencyLoggerFactory() => instance = null;
}
