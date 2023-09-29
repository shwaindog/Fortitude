using System;
using System.Collections.Generic;
using System.Linq;
using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics
{
    public enum UpdateType
    {
        Updated,
        Added,
        Removed
    }

    public class HierarchialConfigurationUpdater
    {
        private static HierarchialConfigurationUpdater instance;
        public static readonly string[] ActivationKeyWords = {"on", "activate", "true", "enabled", "log"};
        private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");
        private readonly string memoryMonitoringKeyName = GcMonitor.FullNameOfLogger.ToLower();

        private HierarchialConfigurationUpdater()
        {
        }

        public static HierarchialConfigurationUpdater Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (typeof (HierarchialConfigurationUpdater))
                    {
                        if (instance == null)
                        {
                            instance = new HierarchialConfigurationUpdater();
                        }
                    }
                }
                return instance;
            }
        }

        private static GcMonitor GcMonitor
        {
            get { return GcMonitor.GetInstance(FLoggerFactory.Instance.GetLogger("Memory")); }
        }

        public static bool IsActivationKeyWord(string parameter)
        {
            return ActivationKeyWords.Contains(parameter.ToLower());
        }

        public void DiagnosticsUpdate(string loggerName, KeyValuePair<string, string> entryKeyValuePair,
            UpdateType status)
        {
            Logger.Info("'{0}' logger(s) {1} with '{2}' and value '{3}'", loggerName, status, entryKeyValuePair.Key,
                entryKeyValuePair.Value);

            try
            {
                var fullnameSpace = entryKeyValuePair.Key != null ? entryKeyValuePair.Key.ToLower() : null;
                var value = entryKeyValuePair.Value != null ? entryKeyValuePair.Value.ToLower() : null;
                if (fullnameSpace == memoryMonitoringKeyName)
                {
                    GcMonitor.SettingTranslation(GcMonitor,
                        status != UpdateType.Removed ? value : GcMonitor.DefaultStringValue);
                }
                if ((loggerName == null || loggerName.Trim().ToLower() == "all") && status != UpdateType.Removed)
                {
                    Logger.Info("Set all Method Loggers for '{0}' to '{1}'", fullnameSpace, value);
                    HierarchialLoggingConfigurator<PerfLoggerPool>.AddRule(fullnameSpace, value);
                    HierarchialLoggingConfigurator<ILatencyLogger>.AddRule(fullnameSpace, value);
                    HierarchialLoggingConfigurator<ITraceLogger>.AddRule(fullnameSpace, value);
                    HierarchialLoggingConfigurator<ICallStatsLogger>.AddRule(fullnameSpace, value);
                    return;
                }
                if ((loggerName == null || loggerName.Trim() == "all") && status == UpdateType.Removed)
                {
                    Logger.Info("Deleted all Method Logger settings for '{0}'", fullnameSpace);
                    HierarchialLoggingConfigurator<ILatencyLogger>.RemoveRule(fullnameSpace);
                    HierarchialLoggingConfigurator<ITraceLogger>.RemoveRule(fullnameSpace);
                    HierarchialLoggingConfigurator<ICallStatsLogger>.RemoveRule(fullnameSpace);
                    HierarchialLoggingConfigurator<PerfLoggerPool>.RemoveRule(fullnameSpace);
                    return;
                }
                if (status != UpdateType.Removed)
                {
                    switch (loggerName)
                    {
                        case "lm":
                        case "latencymonitoring":
                            Logger.Info("latency monitoring for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<ILatencyLogger>.AddRule(fullnameSpace, value);

                            break;
                        case "tl":
                        case "tracelogging":
                            Logger.Info("trace logging for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<ITraceLogger>.AddRule(fullnameSpace, value);
                            break;
                        case "csl":
                        case "callstatslogging":
                            Logger.Info("call stats logging for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<ICallStatsLogger>.AddRule(fullnameSpace, value);
                            break;
                        case "fl":
                        case "flogger":
                            Logger.Info("flogger setting for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<IFLogger>.AddRule(fullnameSpace, value);
                            return;
                        default:
                            Logger.Info("Unknown update of loggername '{0}' with '{1}' no action taken", loggerName,
                                value);
                            return;
                    }
                    var aggregateLoggerShouldBeActivated = AnyLoggerInExactNameSpaceStillHasActivatedRule(fullnameSpace);
                    var aggregateLoggerCurrentActivation =
                        HierarchialLoggingConfigurator<PerfLoggerPool>.AggregateLoggerShouldActivate(
                            fullnameSpace);
                    if (!aggregateLoggerCurrentActivation && aggregateLoggerShouldBeActivated)
                    {
                        HierarchialLoggingConfigurator<PerfLoggerPool>.AddRule(fullnameSpace, "on");
                    }
                    else if (aggregateLoggerCurrentActivation && !aggregateLoggerShouldBeActivated)
                    {
                        HierarchialLoggingConfigurator<PerfLoggerPool>.RemoveRule(fullnameSpace);
                    }
                }
                else if (status == UpdateType.Removed)
                {
                    switch (loggerName)
                    {
                        case "lm":
                        case "latencymonitoring":
                            Logger.Info("removing latency monitoring for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<ILatencyLogger>.RemoveRule(fullnameSpace);
                            break;
                        case "tl":
                        case "tracelogging":
                            Logger.Info("removing trace logging for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<ITraceLogger>.RemoveRule(fullnameSpace);
                            break;
                        case "csl":
                        case "callstatslogging":
                            Logger.Info("removing call stats logging for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<ICallStatsLogger>.RemoveRule(fullnameSpace);
                            break;
                        case "fl":
                        case "flogger":
                            Logger.Info("removing flogger setting logging for '{0}' is '{1}'", fullnameSpace, value);
                            HierarchialLoggingConfigurator<IFLogger>.RemoveRule(fullnameSpace);
                            return;
                        default:
                            Logger.Info("Unknown removal of loggername '{0}' with '{1}' no action taken", loggerName,
                                value);
                            return;
                    }
                    if (!AnyLoggerInExactNameSpaceStillHasActivatedRule(fullnameSpace))
                    {
                        HierarchialLoggingConfigurator<PerfLoggerPool>.RemoveRule(fullnameSpace);
                    }
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
                HierarchialLoggingConfigurator<ILatencyLogger>.AggregateLoggerShouldActivate(fullNamespace);
            var traceHasKey = HierarchialLoggingConfigurator<ITraceLogger>.AggregateLoggerShouldActivate(fullNamespace);
            var callStatsHasKey =
                HierarchialLoggingConfigurator<ICallStatsLogger>.AggregateLoggerShouldActivate(fullNamespace);

            return latencyHasKey || traceHasKey || callStatsHasKey;
        }
    }
}