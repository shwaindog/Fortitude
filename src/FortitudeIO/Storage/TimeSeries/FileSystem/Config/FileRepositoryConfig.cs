// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.Config;

public interface IFileRepositoryConfig : IRepositoryBuilder, IStyledToStringObject
{
    string RepositoryName { get; set; }

    IRepositoryBuilderConfig RepositoryConfig { get; set; }
}

public class FileRepositoryConfig : ConfigSection, IFileRepositoryConfig
{
    private ITimeSeriesRepository? repository;
    public FileRepositoryConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public FileRepositoryConfig() { }

    public FileRepositoryConfig(IFileRepositoryConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        RepositoryName   = toClone.RepositoryName;
        RepositoryConfig = toClone.RepositoryConfig;
    }

    public FileRepositoryConfig(IFileRepositoryConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string RepositoryName
    {
        get => this[nameof(RepositoryName)]!;
        set => this[nameof(RepositoryName)] = value;
    }

    public IRepositoryBuilderConfig RepositoryConfig
    {
        get
        {
            var builderTypeSection = GetSection(nameof(RepositoryConfig));
            if (builderTypeSection[nameof(IRepositoryBuilderConfig.RepositoryType)] == nameof(RepositoryType.Dymwi))
                return new SingleRepositoryBuilderConfig(ConfigRoot, $"{Path}{Split}{nameof(RepositoryConfig)}");
            if (builderTypeSection[nameof(IRepositoryBuilderConfig.RepositoryType)] == nameof(RepositoryType.RemoteLocalCaching))
                return new RemoteLocalCachingFileRepositoryConfig(ConfigRoot, $"{Path}{Split}{nameof(RepositoryConfig)}");

            throw new Exception("Could not construct a Repository Builder Config for the repository");
        }
        set
        {
            if (value is RemoteLocalCachingFileRepositoryConfig remoteLocalCaching)
            {
                _ = new RemoteLocalCachingFileRepositoryConfig(remoteLocalCaching, ConfigRoot, $"{Path}{Split}{nameof(RepositoryConfig)}");
                return;
            }
            _ = new SingleRepositoryBuilderConfig((ISingleRepositoryBuilderConfig)value, ConfigRoot, $"{Path}{Split}{nameof(RepositoryConfig)}");
        }
    }

    public ITimeSeriesRepository BuildRepository
        (string? repositoryName = null) =>
        repository ??= RepositoryConfig.BuildRepository(repositoryName ?? RepositoryName);

    public RepositoryInfo BuildRepositoryInfo
        (string? repositoryName = null) =>
        RepositoryConfig.BuildRepositoryInfo(repositoryName ?? RepositoryName);

    public static FileRepositoryConfig CreateDymwiSingleLocalRepository<T>
        (DirectoryInfo singleRepositoryRootDir, string repositoryName) where T : IRepoPathBuilder
    {
        var repoConfig = new FileRepositoryConfig
        {
            RepositoryConfig = new SingleRepositoryBuilderConfig(singleRepositoryRootDir, repositoryName)
            {
                RepositoryType      = RepositoryType.Dymwi
              , RepoPathBuilderType = typeof(T)
            }
          , RepositoryName = repositoryName
        };
        return repoConfig;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) => 
        stsa.StartComplexType(this, nameof(FileRepositoryConfig))
            .Field.AlwaysAdd(nameof(RepositoryName), RepositoryName)
            .Field.AlwaysAdd(nameof(RepositoryConfig), RepositoryConfig)
            .Complete();
}
