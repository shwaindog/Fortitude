#region

using System.Collections.Concurrent;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;

public class PerfLoggingPoolFactory : IPerfLoggingPoolFactory
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");
    private static volatile IPerfLoggingPoolFactory? instance;
    private static readonly object SyncLock = new();
    private readonly ConcurrentDictionary<string, PerfLoggerPool> instanceTracker = new();

    public static IPerfLoggingPoolFactory Instance
    {
        get
        {
            if (instance != null) return instance;
            lock (SyncLock)
            {
                if (instance == null) instance = new PerfLoggingPoolFactory();
            }

            return instance;
        }
        set => instance = value;
    }

    public IPerfLoggerPool GetLatencyTracingLoggerPool(string name, TimeSpan timeout,
        Type callingClass) =>
        GetLatencyTracingLoggerPool(name, timeout, callingClass.FullName!);

    public IPerfLoggerPool GetLatencyTracingLoggerPool(string name, TimeSpan timeout,
        string prefixName)
    {
        try
        {
            var loggerName = GenKey(prefixName, name);
            if (instanceTracker.TryGetValue(loggerName, out var getInstance)) return getInstance;
            lock (SyncLock)
            {
                if (instanceTracker.TryGetValue(loggerName, out getInstance)) return getInstance;
                getInstance = new PerfLoggerPool(() => new PerfLogger(
                    name, timeout, prefixName), loggerName);
                instanceTracker.TryAdd(loggerName, getInstance);
            }

            return getInstance;
        }
        catch (Exception e)
        {
            Logger.Warn("PerfLoggingPoolFactory.GetLatencyTracingLoggerPool" +
                        " failed to allocate pool due to {0}", e);
            throw new ArgumentException($"GetLatencyTracingLoggerPool could not find name '{name}'", e);
        }
    }

    public static void ClearPerfLoggingPoolFactory() => instance = null;

    private string GenKey(string prefixName, string name) => prefixName + "." + name;
}
