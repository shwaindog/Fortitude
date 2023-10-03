#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

#endregion

#pragma warning disable CS8604 // Possible null reference argument.
namespace FortitudeCommon.Monitoring.Logging.Diagnostics;

public enum UpdateType
{
    Updated
    , Added
    , Removed
}

public class HierarchicalConfigurationUpdater
{
    private static HierarchicalConfigurationUpdater? instance;
    public static readonly string[] ActivationKeyWords = { "on", "activate", "true", "enabled", "log" };
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");

    private HierarchicalConfigurationUpdater() { }

    public static HierarchicalConfigurationUpdater Instance
    {
        get
        {
            if (instance == null)
                lock (typeof(HierarchicalConfigurationUpdater))
                {
                    if (instance == null) instance = new HierarchicalConfigurationUpdater();
                }

            return instance;
        }
    }

    public static bool IsActivationKeyWord(string parameter) => ActivationKeyWords.Contains(parameter.ToLower());

    public void DiagnosticsUpdate(string loggerName, KeyValuePair<string, string?> entryKeyValuePair,
        UpdateType status)
    {
        Logger.Info("'{0}' logger(s) {1} with '{2}' and value '{3}'", loggerName, status,
            entryKeyValuePair.Key,
            entryKeyValuePair.Value);

        try
        {
            var fullnameSpace = entryKeyValuePair.Key!.ToLower();
            var value = entryKeyValuePair.Value?.ToLower();
            if (loggerName.Trim().ToLower() == "all" && status != UpdateType.Removed)
            {
                Logger.Info("Set all Method Loggers for '{0}' to '{1}'", fullnameSpace, value);
                HierarchicalLoggingConfigurator<PerfLoggerPool>.AddRule(fullnameSpace, value);
                HierarchicalLoggingConfigurator<ILatencyLogger>.AddRule(fullnameSpace, value);
                HierarchicalLoggingConfigurator<ITraceLogger>.AddRule(fullnameSpace, value);
                HierarchicalLoggingConfigurator<ICallStatsLogger>.AddRule(fullnameSpace, value);
                return;
            }

            if (loggerName.Trim() == "all" && status == UpdateType.Removed)
            {
                Logger.Info("Deleted all Method Logger settings for '{0}'", fullnameSpace);
                HierarchicalLoggingConfigurator<ILatencyLogger>.RemoveRule(fullnameSpace);
                HierarchicalLoggingConfigurator<ITraceLogger>.RemoveRule(fullnameSpace);
                HierarchicalLoggingConfigurator<ICallStatsLogger>.RemoveRule(fullnameSpace);
                HierarchicalLoggingConfigurator<PerfLoggerPool>.RemoveRule(fullnameSpace);
                return;
            }

            if (status != UpdateType.Removed)
            {
                switch (loggerName)
                {
                    case "lm":
                    case "latencymonitoring":
                        Logger.Info("latency monitoring for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<ILatencyLogger>.AddRule(fullnameSpace, value);

                        break;
                    case "tl":
                    case "tracelogging":
                        Logger.Info("trace logging for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<ITraceLogger>.AddRule(fullnameSpace, value);
                        break;
                    case "csl":
                    case "callstatslogging":
                        Logger.Info("call stats logging for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<ICallStatsLogger>.AddRule(fullnameSpace, value);
                        break;
                    case "fl":
                    case "flogger":
                        Logger.Info("flogger setting for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<IFLogger>.AddRule(fullnameSpace, value);
                        return;
                    default:
                        Logger.Info("Unknown update of loggername '{0}' with '{1}' no action taken", loggerName,
                            value);
                        return;
                }

                var aggregateLoggerShouldBeActivated = AnyLoggerInExactNameSpaceStillHasActivatedRule(fullnameSpace);
                var aggregateLoggerCurrentActivation =
                    HierarchicalLoggingConfigurator<PerfLoggerPool>.AggregateLoggerShouldActivate(
                        fullnameSpace);
                if (!aggregateLoggerCurrentActivation && aggregateLoggerShouldBeActivated)
                    HierarchicalLoggingConfigurator<PerfLoggerPool>.AddRule(fullnameSpace, "on");
                else if (aggregateLoggerCurrentActivation && !aggregateLoggerShouldBeActivated)
                    HierarchicalLoggingConfigurator<PerfLoggerPool>.RemoveRule(fullnameSpace);
            }
            else // if (status == UpdateType.Removed)
            {
                switch (loggerName)
                {
                    case "lm":
                    case "latencymonitoring":
                        Logger.Info("removing latency monitoring for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<ILatencyLogger>.RemoveRule(fullnameSpace);
                        break;
                    case "tl":
                    case "tracelogging":
                        Logger.Info("removing trace logging for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<ITraceLogger>.RemoveRule(fullnameSpace);
                        break;
                    case "csl":
                    case "callstatslogging":
                        Logger.Info("removing call stats logging for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<ICallStatsLogger>.RemoveRule(fullnameSpace);
                        break;
                    case "fl":
                    case "flogger":
                        Logger.Info("removing flogger setting logging for '{0}' is '{1}'", fullnameSpace, value);
                        HierarchicalLoggingConfigurator<IFLogger>.RemoveRule(fullnameSpace);
                        return;
                    default:
                        Logger.Info("Unknown removal of loggername '{0}' with '{1}' no action taken", loggerName,
                            value);
                        return;
                }

                if (!AnyLoggerInExactNameSpaceStillHasActivatedRule(fullnameSpace))
                    HierarchicalLoggingConfigurator<PerfLoggerPool>.RemoveRule(fullnameSpace);
            }
        }
        catch (Exception e)
        {
            Logger.Error("Error attempting to update logger settings.  Got {0}", e);
        }
    }

    public bool AnyLoggerInExactNameSpaceStillHasActivatedRule(string fullNamespace)
    {
        var latencyHasKey =
            HierarchicalLoggingConfigurator<ILatencyLogger>.AggregateLoggerShouldActivate(fullNamespace);
        var traceHasKey = HierarchicalLoggingConfigurator<ITraceLogger>.AggregateLoggerShouldActivate(fullNamespace);
        var callStatsHasKey =
            HierarchicalLoggingConfigurator<ICallStatsLogger>.AggregateLoggerShouldActivate(fullNamespace);

        return latencyHasKey || traceHasKey || callStatsHasKey;
    }
}
