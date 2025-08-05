// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;
using FortitudeCommon.Logging.Config.ConfigSources;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLogConfigRegistry
{
    IFLogAppConfig AppConfig { get; }

    IMutableFLoggerDescendantConfig? FindLoggerConfigIfGiven(string loggerFullName);

    Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> ConfigPathToAppenderConfig       { get; }
    Func<IConfigurationRoot, string, IMutableFlogConfigSource?>        ConfigPathToConfigSourceConfig   { get; }
    Func<IConfigurationRoot, string, IMutableMatchConditionConfig?>    ConfigPathToMatchConditionConfig { get; }

    TimeSpan ExpireConfigCacheIntervalTimeSpan { get; }
}

public interface IMutableFLogConfigRegistry : IFLogConfigRegistry
{
    new IMutableFLogAppConfig AppConfig { get; set; }

    new Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> ConfigPathToAppenderConfig       { get; set; }
    new Func<IConfigurationRoot, string, IMutableFlogConfigSource?>        ConfigPathToConfigSourceConfig   { get; set; }
    new Func<IConfigurationRoot, string, IMutableMatchConditionConfig?>    ConfigPathToMatchConditionConfig { get; set; }

    new TimeSpan ExpireConfigCacheIntervalTimeSpan { get; set; }
}

public class FLogConfigRegistry(IMutableFLogAppConfig newAppConfig) : IMutableFLogConfigRegistry
{
    private IMutableFLogAppConfig appConfig = newAppConfig;

    private IMutableNamedChildLoggersLookupConfig? allConfigDefinedLoggers;

    IFLogAppConfig IFLogConfigRegistry.AppConfig => AppConfig;

    public IMutableFLogAppConfig AppConfig
    {
        get => appConfig;
        set
        {
            appConfig = value;

            ExpireConfigCacheIntervalTimeSpan = appConfig.ConfigSourcesLookup.ExpireConfigCacheIntervalTimeSpan.ToTimeSpan();

            allConfigDefinedLoggers = appConfig.RootLogger.AllLoggers();
        }
    }

    public IMutableFLoggerDescendantConfig? FindLoggerConfigIfGiven(string loggerFullName)
    {
        return allConfigDefinedLoggers?[loggerFullName];
    }

    public Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> ConfigPathToAppenderConfig { get; set; }
        = FloggerBuiltinAppenderTypeExtensions.GetBuiltAppenderReferenceConfig;

    public Func<IConfigurationRoot, string, IMutableFlogConfigSource?> ConfigPathToConfigSourceConfig { get; set; }
        = FLoggerConfigSourceTypeExtensions.GetBuiltConfigSourceConfig;

    public Func<IConfigurationRoot, string, IMutableMatchConditionConfig?> ConfigPathToMatchConditionConfig { get; set; }
        = FLoggerEntryMatchTypeExtensions.GetMatchConditionConfigNoParent;

    public TimeSpan ExpireConfigCacheIntervalTimeSpan { get; set; }
}

public static class FLogConfigRegistryExtensions
{
    public static IMutableFLogConfigRegistry? UpdateConfig
        (this IFLogConfigRegistry? maybeCreated, IMutableFLogAppConfig flogAppConfig)
    {
        if (maybeCreated is IMutableFLogConfigRegistry maybeMutable)
        {
            maybeMutable.AppConfig = flogAppConfig;
            return maybeMutable;
        }
        return null;
    }
}
