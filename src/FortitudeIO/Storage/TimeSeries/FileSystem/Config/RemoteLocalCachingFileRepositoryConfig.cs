// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeIO.Storage.TimeSeries.FileSystem;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.Config;

public interface IRemoteLocalCachingFileRepositoryConfig : IRepositoryBuilderConfig
{
    RepositoryType                LocalRemoteRepositoriesType    { get; set; }
    IFileRepositoryLocationConfig LocalRepositoryLocationConfig  { get; set; }
    IFileRepositoryLocationConfig RemoteRepositoryLocationConfig { get; set; }
}

public class RemoteLocalCachingFileRepositoryConfig : RepositoryBuilderConfig, IRemoteLocalCachingFileRepositoryConfig
{
    public RemoteLocalCachingFileRepositoryConfig
        (IConfigurationRoot root, string path) : base(root, path) =>
        RepositoryType = RepositoryType.RemoteLocalCaching;

    public RemoteLocalCachingFileRepositoryConfig() { }

    public RemoteLocalCachingFileRepositoryConfig(IRemoteLocalCachingFileRepositoryConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        LocalRemoteRepositoriesType = toClone.LocalRemoteRepositoriesType;
        LocalRepositoryLocationConfig
            = new FileRepositoryLocationConfig(toClone.LocalRepositoryLocationConfig, root, $"{path}{Split}{nameof(LocalRepositoryLocationConfig)}");
        RemoteRepositoryLocationConfig
            = new FileRepositoryLocationConfig(toClone.RemoteRepositoryLocationConfig, root, $"{path}{Split}{nameof(RemoteRepositoryLocationConfig)}");
    }

    public RemoteLocalCachingFileRepositoryConfig(IRemoteLocalCachingFileRepositoryConfig toClone) : base(toClone)
    {
        LocalRemoteRepositoriesType    = toClone.LocalRemoteRepositoriesType;
        LocalRepositoryLocationConfig  = new FileRepositoryLocationConfig(toClone.LocalRepositoryLocationConfig);
        RemoteRepositoryLocationConfig = new FileRepositoryLocationConfig(toClone.RemoteRepositoryLocationConfig);
    }

    public RemoteLocalCachingFileRepositoryConfig
    (RepositoryType localRemoteRepositoryType, IFileRepositoryLocationConfig localRepositoryLocationConfig
      , IFileRepositoryLocationConfig remoteRepositoryLocationConfig
      , bool createIfNotExists, string repoPathBuilderClassName)
        : this(localRemoteRepositoryType, localRepositoryLocationConfig, remoteRepositoryLocationConfig, createIfNotExists, InMemoryConfigRoot
             , InMemoryPath, repoPathBuilderClassName) { }

    public RemoteLocalCachingFileRepositoryConfig
    (RepositoryType localRemoteRepositoryType, IFileRepositoryLocationConfig localRepositoryLocationConfig
      , IFileRepositoryLocationConfig remoteRepositoryLocationConfig
      , bool createIfNotExists, IConfigurationRoot root, string path, string repoPathBuilderClassName)
        : base(RepositoryType.RemoteLocalCaching, createIfNotExists, root, path, repoPathBuilderClassName)
    {
        LocalRemoteRepositoriesType = localRemoteRepositoryType;
        LocalRepositoryLocationConfig
            = new FileRepositoryLocationConfig(localRepositoryLocationConfig, ConfigRoot, $"{Path}{Split}{nameof(LocalRepositoryLocationConfig)}");
        RemoteRepositoryLocationConfig
            = new FileRepositoryLocationConfig(remoteRepositoryLocationConfig, ConfigRoot, $"{Path}{Split}{nameof(RemoteRepositoryLocationConfig)}");
    }

    public RemoteLocalCachingFileRepositoryConfig
    (RepositoryType localRemoteRepositoryType, IFileRepositoryLocationConfig localRepositoryLocationConfig
      , IFileRepositoryLocationConfig remoteRepositoryLocationConfig
      , Type repoPathBuilderType, bool createIfNotExists, IConfigurationRoot root, string path)
        : this(localRemoteRepositoryType, localRepositoryLocationConfig, remoteRepositoryLocationConfig, createIfNotExists, root, path
             , repoPathBuilderType.AssemblyQualifiedName!) { }

    public RemoteLocalCachingFileRepositoryConfig
    (RepositoryType localRemoteRepositoryType, IFileRepositoryLocationConfig localRepositoryLocationConfig
      , IFileRepositoryLocationConfig remoteRepositoryLocationConfig
      , Type repoPathBuilderType, bool createIfNotExists)
        : this(localRemoteRepositoryType, localRepositoryLocationConfig, remoteRepositoryLocationConfig, createIfNotExists, InMemoryConfigRoot
             , InMemoryPath, repoPathBuilderType.AssemblyQualifiedName!) { }

    public RepositoryType LocalRemoteRepositoriesType
    {
        get
        {
            var checkValue = this[nameof(RepositoryType)];
            return checkValue != null ? Enum.Parse<RepositoryType>(checkValue) : RepositoryType.Custom;
        }
        set => this[nameof(RepositoryType)] = value.ToString();
    }

    public override RepositoryInfo BuildRepositoryInfo(string? repositoryName = "NoRepositoryName") =>
        new(new RepositoryRootDirectory(repositoryName, new DirectoryInfo(LocalRepositoryLocationConfig.RootDirectoryPath)), RepositoryProximity.Both
          , LocalRepositoryLocationConfig.TimeSeriesFileExtension, RequiredInstrumentAttributeFieldNames, OptionalInstrumentAttributeFieldNames);

    public override ITimeSeriesRepository BuildRepository(string? repositoryName = "NoRepositoryName")
    {
        if (LocalRemoteRepositoriesType == RepositoryType.Dymwi)
        {
            var localRepositoryPathBuilder  = RepositoryPathBuilder(LocalRepositoryLocationConfig);
            _                   = DymwiTimeSeriesDirectoryRepository.OpenRepository(localRepositoryPathBuilder);
            var remoteRepositoryPathBuilder = RepositoryPathBuilder(RemoteRepositoryLocationConfig);
            _ = DymwiTimeSeriesDirectoryRepository.OpenRepository(remoteRepositoryPathBuilder);
            return new RemoteLocalCachingRepository(this, repositoryName ?? "NoRepositoryName");
        }
        throw new NotImplementedException("Until custom repository config is implemented you can only construct a Dymwi repository");
    }

    public override IRepoPathBuilder RepositoryPathBuilder(IFileRepositoryLocationConfig fileRepoLocationConfig)
    {
        var repositoryPathBuilder = RepoPathBuilderType!;
        var fileTypeOpenExistingCtor =
            ReflectionHelper.CtorDerivedBinder<ISingleRepositoryBuilderConfig, IRepoPathBuilder>(repositoryPathBuilder);
        var instance = fileTypeOpenExistingCtor(new SingleRepositoryBuilderConfig(fileRepoLocationConfig));
        return instance;
    }

    public IFileRepositoryLocationConfig LocalRepositoryLocationConfig
    {
        get => new FileRepositoryLocationConfig(ConfigRoot, $"{Path}{Split}{nameof(LocalRepositoryLocationConfig)}");
        set => _ = new FileRepositoryLocationConfig(value, ConfigRoot, $"{Path}{Split}{nameof(LocalRepositoryLocationConfig)}");
    }

    public IFileRepositoryLocationConfig RemoteRepositoryLocationConfig
    {
        get => new FileRepositoryLocationConfig(ConfigRoot, $"{Path}{Split}{nameof(RemoteRepositoryLocationConfig)}");
        set => _ = new FileRepositoryLocationConfig(value, ConfigRoot, $"{Path}{Split}{nameof(RemoteRepositoryLocationConfig)}");
    }
}
