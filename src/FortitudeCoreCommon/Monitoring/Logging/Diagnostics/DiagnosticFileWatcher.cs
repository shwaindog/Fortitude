#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics;

public class DiagnosticFileWatcher
{
    private readonly IDisposable? autoReloadFile;
    private readonly IChangeToken? changeToken;
    private readonly IConfiguration source;
    private readonly bool watchForChanges;
    private string path;

    public DiagnosticFileWatcher(string path, bool watchForChanges)
    {
        this.path = path;
        this.watchForChanges = watchForChanges;
        var configManager = new ConfigurationManager();
        var builder = configManager.AddIniFile(path, false, watchForChanges);
        source = builder.Build();
        if (watchForChanges)
        {
            changeToken = source.GetReloadToken();
            autoReloadFile = changeToken.RegisterChangeCallback(o => source_Reloaded(o, EventArgs.Empty), source);
        }

        ListenToConfigs();
    }

    private void source_Reloaded(object? sender, EventArgs e)
    {
        ListenToConfigs();
    }

    private void ListenToConfigs()
    {
        ListenToConfig(allConfig, source.GetSection("AllLoggers"));
        ListenToConfig(callStatsConfig, source.GetSection("CallStats"));
        ListenToConfig(latencyConfig, source.GetSection("Latency"));
        ListenToConfig(traceLogging, source.GetSection("TraceLogging"));
        ListenToConfig(fLogger, source.GetSection("FLogger"));
    }

    private void ListenToConfig(DiagnosticConfig diagConfig, IConfigurationSection newSectionConfig)
    {
        if (!diagConfig.SameConfiguration(newSectionConfig)) diagConfig.ConfigurationSection = newSectionConfig;
    }

    // ReSharper disable NotAccessedField.Local
    private readonly DiagnosticConfig allConfig = new("all");
    private readonly DiagnosticConfig callStatsConfig = new("callstatslogging");
    private readonly DiagnosticConfig latencyConfig = new("latencymonitoring");
    private readonly DiagnosticConfig traceLogging = new("tracelogging");

    private readonly DiagnosticConfig fLogger = new("flogger");
    // ReSharper restore NotAccessedField.Local
}
