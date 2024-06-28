// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeIO.TimeSeries.FileSystem.Config;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarketsCore.Indicators.Config;

public interface IIndicatorServicesConfig
{
    IMarketsConfig?        MarketsConfig                  { get; set; }
    IFileRepositoryConfig? TimeSeriesFileRepositoryConfig { get; set; }
}

public class IndicatorServicesConfig : ConfigSection, IIndicatorServicesConfig
{
    private object? ignoreSuppressWarnings;
    public IndicatorServicesConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public IndicatorServicesConfig() { }

    public IMarketsConfig? MarketsConfig
    {
        get
        {
            if (GetSection(nameof(MarketsConfig)).GetChildren().Any())
                return new MarketsConfig(ConfigRoot, Path + ":" + nameof(MarketsConfig));
            return null;
        }
        set => ignoreSuppressWarnings = value != null ? new MarketsConfig(value, ConfigRoot, Path + ":" + nameof(MarketsConfig)) : null;
    }

    public IFileRepositoryConfig? TimeSeriesFileRepositoryConfig
    {
        get
        {
            if (GetSection(nameof(MarketsConfig)).GetChildren().Any())
                return new FileRepositoryConfig(ConfigRoot, Path + ":" + nameof(TimeSeriesFileRepositoryConfig));
            return null;
        }
        set =>
            ignoreSuppressWarnings
                = value != null ? new FileRepositoryConfig(value, ConfigRoot, Path + ":" + nameof(TimeSeriesFileRepositoryConfig)) : null;
    }
}
