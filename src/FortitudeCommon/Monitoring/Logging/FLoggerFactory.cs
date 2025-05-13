#region

using FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging.Diagnostics;
using FortitudeCommon.Monitoring.Logging.NLogAdapter;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Monitoring.Logging;

public class FLoggerFactory : IFLoggerFactory
{
    private static readonly IFLoggerFactory LoggerFactory;

    private static readonly Dictionary<string, IFLogger> Loggers = [];

    private static readonly EnumerableBatchPollingRing<FLogEvent>? Ring;

    private static readonly FLogEventPoller? RingPoller;
    private static readonly object           SyncLock = new();
    private static volatile IFLoggerFactory? instance;

    static FLoggerFactory()
    {
        if (NoOpLoggerFactory.StartWithNoOpLoggerFactory)
        {
            LoggerFactory = new NoOpLoggerFactory();
            return;
        }
        try
        {
            LoggerFactory = new NLogFactory();
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not Load FLoggerFactory, Loading Noop :" + e);
            LoggerFactory = new NoOpLoggerFactory();
        }

        var configManager = new ConfigurationManager();
        var builder = configManager.AddIniFile("NLog.ini", true, true);

        var configContext = builder.Build();

        Ring = new EnumerableBatchPollingRing<FLogEvent>(
            "AsyncFLogger",
            configContext.GetSection("QueueSize").Value?.ToInt() ?? 50000,
            () => new FLogEvent(),
            ClaimStrategyType.MultiProducers,
            false);
        RingPoller = new FLogEventPoller(Ring, configContext["TimeoutMs"]?.ToUInt() ?? 500);
        RingPoller.Start();

        AppDomain.CurrentDomain.DomainUnload += (_, _) => { RingPoller.Dispose(); };
        //AppDomain.CurrentDomain.ProcessExit += (s, e) => { RingPoller.Dispose(); };
    }

    public static IFLoggerFactory Instance
    {
        get
        {
            if (instance != null) return instance;
            lock (SyncLock)
            {
                // ReSharper disable once ConvertIfStatementToNullCoalescingAssignment
                if (instance == null)
                {
                    instance = new FLoggerFactory();
                }
            }

            return instance;
        }
        set => instance = value;
    }

    public IFLogger GetLogger(string loggerName)
    {
        IFLogger? logger;
        lock (Loggers)
        {
            if (!Loggers.TryGetValue(loggerName, out logger))
            {
                if (Ring == null)
                    Loggers.Add(loggerName, logger = LoggerFactory.GetLogger(loggerName));
                else
                    Loggers.Add(loggerName, logger = new AsyncFLogger(Ring, LoggerFactory.GetLogger(loggerName)));
            }

            HierarchicalLoggingConfigurator<IFLogger>.Register(logger);
        }

        return logger;
    }

    public IFLogger GetLogger(Type type) => GetLogger(type.FullName!);

    public static void GracefullyTerminateProcessLogging()
    {
        RingPoller?.Dispose();
    }

    public static void WaitUntilDrained()
    {
        RingPoller?.WaitUntilDrained();
    }
}
