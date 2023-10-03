#region

using System.Configuration;
using System.Reflection;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;

public class PerfLoggerPool : IPerfLoggerPool
{
    private const string LatencyLoggerPoolDisabledValue = "off";

    // ReSharper disable once CollectionNeverQueried.Local
    private static readonly List<DiagnosticFileWatcher> FileWatchers = new();
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");

    private readonly bool canBeEnabled = true;
    private readonly ObjectPool<IPerfLogger> latencyTraceLoggerPool;
    private bool enabled;

    static PerfLoggerPool()
    {
        var oldWorkingDir = Directory.GetCurrentDirectory();
        try
        {
            var diagnosticConfig =
                (DiagnosticLoggingConfigSection)ConfigurationManager.GetSection(
                    "diagnosticSettingsPropertyFile");

            var currentAssemblyPath = Assembly.GetExecutingAssembly().Location;
            var uri = new UriBuilder(currentAssemblyPath);
            currentAssemblyPath = Uri.UnescapeDataString(uri.Path);
            currentAssemblyPath = Path.GetDirectoryName(currentAssemblyPath);
            if (currentAssemblyPath != null) Directory.SetCurrentDirectory(currentAssemblyPath);

            if (diagnosticConfig?.Settings == null) return;
            foreach (PropertyFile propertyFile in diagnosticConfig.Settings)
                try
                {
                    FileWatchers.Add(new DiagnosticFileWatcher(propertyFile.Path, propertyFile.LiveUpdate));
                }
                catch (Exception e)
                {
                    Logger.Error("Error whilst reading diagnostic config file {0}.  Got {1}", propertyFile.Path,
                        e);
                }
        }
        catch (Exception e)
        {
            Logger.Error("Error whilst loading diagnostic config files. Got {0}", e);
        }
        finally
        {
            Directory.SetCurrentDirectory(oldWorkingDir);
        }
    }

    public PerfLoggerPool(Func<IPerfLogger> newObjFunc, string fullName)
    {
        FullNameOfLogger = fullName;
        latencyTraceLoggerPool = new ObjectPool<IPerfLogger>(newObjFunc);
        try
        {
            HierarchicalLoggingConfigurator<PerfLoggerPool>.Register(this);

            var spinUpALatencyLogger = latencyTraceLoggerPool.Borrow();
            latencyTraceLoggerPool.Return(spinUpALatencyLogger);
        }
        catch (Exception e)
        {
            Logger.Warn("PerfLoggerPool.ctor disabling trace logging as caught and swallowed. {0}"
                , e);
            canBeEnabled = false;
        }
    }

    public string FullNameOfLogger { get; }

    public Action<IHierarchicalLogger, string> SettingTranslation
    {
        get
        {
            return (hl, setting) =>
            {
                if (!(hl is PerfLoggerPool ltlp) || setting == null) return;
                if (HierarchicalConfigurationUpdater.IsActivationKeyWord(setting))
                {
                    ltlp.Enabled = true;
                    return;
                }

                ltlp.Enabled = false;
            };
        }
    }

    public bool Enabled
    {
        get => enabled && canBeEnabled;
        set => enabled = value;
    }

    public string DefaultStringValue => LatencyLoggerPoolDisabledValue;

    public IPerfLogger StartNewTrace()
    {
        if (!Enabled) return NoOpPerfLogger.Instance;
        var tl = latencyTraceLoggerPool.Borrow();
        tl.Start();
        return tl;
    }

    public void StopTrace<T1, Tu, Tv>(IPerfLogger traceLogger, T1 optionalData,
        Tu moreOptionalData, Tv evenMoreOptionalData)
    {
        if (traceLogger is NoOpPerfLogger) return;
        if (Enabled) traceLogger.StopTiming(optionalData, moreOptionalData, evenMoreOptionalData);
        latencyTraceLoggerPool.Return(traceLogger);
    }

    public void StopTrace<T1, Tu>(IPerfLogger traceLogger, T1 optionalData,
        Tu moreOptionalData)
    {
        if (traceLogger is NoOpPerfLogger) return;
        if (Enabled) traceLogger.StopTiming(optionalData, moreOptionalData);
        latencyTraceLoggerPool.Return(traceLogger);
    }

    public void StopTrace<T1>(IPerfLogger traceLogger, T1 optionalData)
    {
        if (traceLogger is NoOpPerfLogger) return;
        if (Enabled) traceLogger.StopTiming(optionalData);
        latencyTraceLoggerPool.Return(traceLogger);
    }

    public void StopTrace(IPerfLogger traceLogger)
    {
        if (traceLogger is NoOpPerfLogger) return;
        if (Enabled) traceLogger.StopTiming();
        latencyTraceLoggerPool.Return(traceLogger);
    }
}
