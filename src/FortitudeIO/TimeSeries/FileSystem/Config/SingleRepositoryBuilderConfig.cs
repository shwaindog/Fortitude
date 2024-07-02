// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Config;

public interface ISingleRepositoryBuilderConfig : IRepositoryBuilderConfig
{
    IFileRepositoryLocationConfig FileRepositoryLocationConfig { get; set; }

    public IRepoPathBuilder? RepoPathBuilder { get; set; }
}

public class SingleRepositoryBuilderConfig : RepositoryBuilderConfig, ISingleRepositoryBuilderConfig
{
    private object? ignoreSuppressWarnings;

    private IRepoPathBuilder? repoPathBuilder;
    public SingleRepositoryBuilderConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public SingleRepositoryBuilderConfig() { }

    public SingleRepositoryBuilderConfig
        (ISingleRepositoryBuilderConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path) =>
        FileRepositoryLocationConfig
            = new FileRepositoryLocationConfig(toClone.FileRepositoryLocationConfig, root, path + $":{nameof(FileRepositoryLocationConfig)}");

    public SingleRepositoryBuilderConfig(ISingleRepositoryBuilderConfig toClone)
        : base(toClone, InMemoryConfigRoot, InMemoryPath) =>
        FileRepositoryLocationConfig = new FileRepositoryLocationConfig(toClone.FileRepositoryLocationConfig);


    public SingleRepositoryBuilderConfig
    (string repositoryRootDirectoryPath, RepositoryProximity proximity, string repoPathBuilderClassName
      , RepositoryType repositoryType, bool createIfNotExists, IConfigurationRoot root, string path, string? repoName = null
      , string timeSeriesFileExtension = ".tsf")
        : base(repositoryType, createIfNotExists, root, path, repoPathBuilderClassName) =>
        FileRepositoryLocationConfig = new FileRepositoryLocationConfig
            (repositoryRootDirectoryPath, proximity, root, path + $":{nameof(FileRepositoryLocationConfig)}"
           , repoName, timeSeriesFileExtension);

    public SingleRepositoryBuilderConfig
    (string repositoryRootDirectoryPath, RepositoryProximity proximity, Type repoClassBuilderType, RepositoryType repositoryType
      , bool createIfNotExists, string? repoName = null, string timeSeriesFileExtension = ".tsf")
        : base(repositoryType, createIfNotExists, InMemoryConfigRoot, InMemoryPath, repoClassBuilderType) =>
        FileRepositoryLocationConfig = new FileRepositoryLocationConfig
            (repositoryRootDirectoryPath, proximity, ConfigRoot, Path + $":{nameof(FileRepositoryLocationConfig)}"
           , repoName, timeSeriesFileExtension);


    public SingleRepositoryBuilderConfig(IFileRepositoryLocationConfig fileLocationConfig) : base(InMemoryConfigRoot, InMemoryPath) =>
        FileRepositoryLocationConfig = new FileRepositoryLocationConfig(fileLocationConfig);

    public override ITimeSeriesRepository BuildRepository(string? repositoryName = "NoRepositoryName")
    {
        if (RepositoryType == RepositoryType.Dymwi)
            return DymwiTimeSeriesDirectoryRepository.OpenRepository(RepositoryPathBuilder(FileRepositoryLocationConfig));
        throw new NotImplementedException("Until custom repository config is implemented you can only construct a Dymwi repository");
    }

    public override RepositoryInfo BuildRepositoryInfo(string? repositoryName = "NoRepositoryName") =>
        new(new RepositoryRootDirectory(repositoryName, new DirectoryInfo(FileRepositoryLocationConfig.RootDirectoryPath))
          , FileRepositoryLocationConfig.Proximity
          , FileRepositoryLocationConfig.TimeSeriesFileExtension, RequiredInstrumentAttributeFieldNames, OptionalInstrumentAttributeFieldNames);

    public override IRepoPathBuilder RepositoryPathBuilder(IFileRepositoryLocationConfig fileRepoLocationConfig)
    {
        var repositoryPathBuilder = RepoPathBuilderType!;
        var fileTypeOpenExistingCtor =
            ReflectionHelper.CtorDerivedBinder<ISingleRepositoryBuilderConfig, IRepoPathBuilder>(repositoryPathBuilder);
        var instance = fileTypeOpenExistingCtor(this);
        return instance;
    }

    public IRepoPathBuilder? RepoPathBuilder
    {
        get
        {
            if (repoPathBuilder != null) return repoPathBuilder;
            var repositoryPathBuilder = RepoPathBuilderType;
            if (repositoryPathBuilder != null)
            {
                var fileTypeOpenExistingCtor =
                    ReflectionHelper.CtorDerivedBinder<ISingleRepositoryBuilderConfig, IRepoPathBuilder>(repositoryPathBuilder);
                repoPathBuilder = fileTypeOpenExistingCtor(this);
            }
            return repoPathBuilder;
        }
        set
        {
            if (repoPathBuilder == value) return;
            if (value == null)
            {
                repoPathBuilder = null;
                return;
            }
            RepoPathBuilderType = value.GetType();
            repoPathBuilder     = value;
        }
    }

    public IFileRepositoryLocationConfig FileRepositoryLocationConfig
    {
        get => new FileRepositoryLocationConfig(ConfigRoot, Path + ":" + nameof(FileRepositoryLocationConfig));
        set => ignoreSuppressWarnings = new FileRepositoryLocationConfig(value, ConfigRoot, Path + ":" + nameof(FileRepositoryLocationConfig));
    }
}
