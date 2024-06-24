// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Config;

public interface IFileRepositoryConfig
{
    string                   RepositoryName   { get; set; }
    IRepositoryBuilderConfig RepositoryConfig { get; set; }
    ITimeSeriesRepository    BuildRepository();
}

public class FileRepositoryConfig : ConfigSection, IFileRepositoryConfig
{
    private object? ignoreSuppressWarnings;
    public FileRepositoryConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public FileRepositoryConfig() { }

    public string RepositoryName
    {
        get => this[nameof(RepositoryName)]!;
        set => this[nameof(RepositoryName)] = value;
    }

    public IRepositoryBuilderConfig RepositoryConfig
    {
        get
        {
            var builderTypeSection = GetSection(nameof(RepositoryConfig))!;
            if (builderTypeSection[nameof(IRepositoryBuilderConfig.RepositoryType)] == nameof(RepositoryType.Dymwi))
                return new SingleRepositoryBuilderConfig(ConfigRoot, Path + $":{nameof(RepositoryConfig)}");
            if (builderTypeSection[nameof(IRepositoryBuilderConfig.RepositoryType)] == nameof(RepositoryType.RemoteLocalCaching))
                return new RemoteLocalCachingFileRepositoryConfig(ConfigRoot, Path + $":{nameof(RepositoryConfig)}");

            throw new Exception("Could not construct a Repository Builder Config for the repository");
        }
        set
        {
            if (value is RemoteLocalCachingFileRepositoryConfig remoteLocalCaching)
            {
                ignoreSuppressWarnings
                    = new RemoteLocalCachingFileRepositoryConfig(remoteLocalCaching, ConfigRoot, Path + $":{nameof(RepositoryConfig)}");
                return;
            }
            ignoreSuppressWarnings
                = new SingleRepositoryBuilderConfig((ISingleRepositoryBuilderConfig)value, ConfigRoot, Path + $":{nameof(RepositoryConfig)}");
        }
    }

    public ITimeSeriesRepository BuildRepository() => RepositoryConfig.BuildRepository();
}
