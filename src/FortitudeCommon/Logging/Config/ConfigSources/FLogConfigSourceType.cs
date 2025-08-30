// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.ConfigSources;

public enum FLogConfigSourceType
{
    Unknown
  , File
  , Network
  , Database
}

public static class FLoggerConfigSourceTypeExtensions
{
    public static FLogConfigSourceType? GetConfigSourceType(this IConfigurationRoot configRoot, string configPath)
    {
        var source = configRoot.GetSection($"{configPath}{ConfigurationPath.KeyDelimiter}{nameof(IFlogConfigSource.SourceType)}").Value;
        return Enum.TryParse<FLogConfigSourceType>(source, out var type) ? type : FLogConfigSourceType.Unknown;
    }

    public static IMutableFlogConfigSource? GetBuiltConfigSourceConfig(this IConfigurationRoot configRoot, string configPath)
    {
        var appenderTypeConfig = GetConfigSourceType(configRoot, configPath);
        switch (appenderTypeConfig)
        {
            case FLogConfigSourceType.File: return null;

            default: return null;
        }
    }
}
