using System;
using System.Collections.Generic;
using FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles;

namespace FortitudeCommon.Monitoring.Logging.Diagnostics
{
    public class DiagnosticFileWatcher
    {
        private readonly IniConfigSource source;

        public DiagnosticFileWatcher(string path, bool watchForChanges)
        {
            this.path = path;
            this.watchForChanges = watchForChanges;

            source = new IniConfigSource(path);
            source.FileChanged += (src, args) => source.Reload();
            source.Reloaded += source_Reloaded;
            ListenToConfigs(watchForChanges);
        }

        private void source_Reloaded(object sender, EventArgs e)
        {
            ListenToConfigs(watchForChanges);
        }

        private void ListenToConfigs(bool watchForChanges)
        {
            allConfig = ListenToConfig(allConfig, "AllLoggers", watchForChanges, "all");
            callStatsConfig = ListenToConfig(callStatsConfig, "CallStats", watchForChanges, "callstatslogging");
            latencyConfig = ListenToConfig(latencyConfig, "Latency", watchForChanges, "latencymonitoring");
            traceLogging = ListenToConfig(traceLogging, "TraceLogging", watchForChanges, "tracelogging");
            fLogger = ListenToConfig(fLogger, "FLogger", watchForChanges, "flogger");
        }

        private IConfig ListenToConfig(IConfig previousConfig, string sectionName, bool watchForChanges,
            string diagnosticName)
        {
            var currentConfigSection = source.Configs[sectionName];
            if (currentConfigSection != previousConfig)
            {
                var addHandler = new ConfigKeyEventHandler(
                    (sender, args) => HierarchialConfigurationUpdater.Instance.DiagnosticsUpdate(diagnosticName,
                        new KeyValuePair<string, string>(args.KeyName, args.KeyValue), UpdateType.Updated));
                var removeHandler = new ConfigKeyEventHandler(
                    (sender, args) => HierarchialConfigurationUpdater.Instance.DiagnosticsUpdate(diagnosticName,
                        new KeyValuePair<string, string>(args.KeyName, args.KeyValue), UpdateType.Removed));

                if (previousConfig != null)
                {
                    foreach (var key in previousConfig.GetKeys())
                    {
                        HierarchialConfigurationUpdater.Instance.DiagnosticsUpdate(diagnosticName,
                            new KeyValuePair<string, string>(key, previousConfig.Get(key)), UpdateType.Removed);
                    }
                    previousConfig.KeySet -= addHandler;
                    previousConfig.KeyRemoved -= removeHandler;
                }
                if (currentConfigSection != null)
                {
                    if (watchForChanges)
                    {
                        currentConfigSection.KeySet += addHandler;
                        currentConfigSection.KeyRemoved += removeHandler;
                    }
                    foreach (var key in currentConfigSection.GetKeys())
                    {
                        HierarchialConfigurationUpdater.Instance.DiagnosticsUpdate(diagnosticName,
                            new KeyValuePair<string, string>(key, currentConfigSection.Get(key)), UpdateType.Added);
                    }
                }
                return currentConfigSection;
            }
            return previousConfig;
        }

        // ReSharper disable NotAccessedField.Local
        private string path;
        private readonly bool watchForChanges;
        private IConfig allConfig;
        private IConfig callStatsConfig;
        private IConfig latencyConfig;
        private IConfig traceLogging;
        private IConfig fLogger;
        // ReSharper restore NotAccessedField.Local
    }
}