// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;
using FortitudeCommon.Logging.Config.ConfigSources;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLoggerConfigRegistry
{
    IFLogAppConfig AppConfig { get; }

    Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?>  ConfigPathToAppenderConfig       { get; }
    Func<IConfigurationRoot, string, IMutableFlogConfigSource?>         ConfigPathToConfigSourceConfig   { get; }
    Func<IConfigurationRoot, string, IMutableMatchConditionConfig?> ConfigPathToMatchConditionConfig { get; }

    TimeSpan ExpireConfigCacheIntervalTimeSpan { get; }
}

public interface IMutableFLoggerConfigRegistry : IFLoggerConfigRegistry
{
    new IFLogAppConfig AppConfig { get; set; }

    new Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> ConfigPathToAppenderConfig     { get; set; }
    new Func<IConfigurationRoot, string, IMutableFlogConfigSource?>        ConfigPathToConfigSourceConfig { get; set; }
    new Func<IConfigurationRoot, string, IMutableMatchConditionConfig?>    ConfigPathToMatchConditionConfig { get; set; }

    new TimeSpan ExpireConfigCacheIntervalTimeSpan { get; set; }
}

public class FLoggerConfigRegistry : IMutableFLoggerConfigRegistry
{
    private IFLogAppConfig appConfig = null!;

    public IFLogAppConfig AppConfig
    {
        get => appConfig;
        set
        {
            appConfig = value;

            ExpireConfigCacheIntervalTimeSpan = appConfig.ConfigSourcesLookup.ExpireConfigCacheIntervalTimeSpan.ToTimeSpan();
        }
    }

    public Func<IConfigurationRoot, string, IMutableAppenderReferenceConfig?> ConfigPathToAppenderConfig { get; set; }
        = FloggerBuiltinAppenderTypeExtensions.GetBuiltAppenderReferenceConfig;

    public Func<IConfigurationRoot, string, IMutableFlogConfigSource?>     ConfigPathToConfigSourceConfig { get; set; }
        = FLoggerConfigSourceTypeExtensions.GetBuiltConfigSourceConfig;

    public Func<IConfigurationRoot, string, IMutableMatchConditionConfig?>     ConfigPathToMatchConditionConfig { get; set; }
        = FLoggerEntryMatchTypeExtensions.GetMatchConditionConfigNoParent;

    public TimeSpan ExpireConfigCacheIntervalTimeSpan { get; set; }
}
