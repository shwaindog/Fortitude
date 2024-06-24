// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Config;

public interface IRepositoryBuilderConfig
{
    string?        RepoPathBuilderClassName { get; set; }
    Type?          RepoPathBuilderType      { get; set; }
    bool           CreateIfNotExists        { get; set; }
    RepositoryType RepositoryType           { get; set; }

    IRepoPathBuilder      RepositoryPathBuilder(IFileRepositoryLocationConfig fileRepoLocationConfig);
    ITimeSeriesRepository BuildRepository();
}

public abstract class RepositoryBuilderConfig : ConfigSection, IRepositoryBuilderConfig
{
    protected RepositoryBuilderConfig(IConfigurationRoot root, string path) : base(root, path) { }
    protected RepositoryBuilderConfig() { }

    protected RepositoryBuilderConfig(IRepositoryBuilderConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        RepositoryType           = toClone.RepositoryType;
        CreateIfNotExists        = toClone.CreateIfNotExists;
        RepoPathBuilderClassName = toClone.RepoPathBuilderClassName;
    }

    protected RepositoryBuilderConfig(IRepositoryBuilderConfig toClone) : this(InMemoryConfigRoot, InMemoryPath)
    {
        RepositoryType           = toClone.RepositoryType;
        CreateIfNotExists        = toClone.CreateIfNotExists;
        RepoPathBuilderClassName = toClone.RepoPathBuilderClassName;
    }


    protected RepositoryBuilderConfig
    (RepositoryType repositoryType, bool createIfNotExists, IConfigurationRoot root
      , string path, string? repoPathBuilderClassName = null) : this(root, path)
    {
        RepositoryType           = repositoryType;
        CreateIfNotExists        = createIfNotExists;
        RepoPathBuilderClassName = repoPathBuilderClassName;
    }

    protected RepositoryBuilderConfig
    (RepositoryType repositoryType, bool createIfNotExists, IConfigurationRoot root
      , string path, Type? repoPathBuilderType) : this(root, path)
    {
        RepositoryType      = repositoryType;
        CreateIfNotExists   = createIfNotExists;
        RepoPathBuilderType = repoPathBuilderType;
    }

    public abstract ITimeSeriesRepository BuildRepository();

    public abstract IRepoPathBuilder RepositoryPathBuilder(IFileRepositoryLocationConfig fileRepoLocationConfig);

    public bool CreateIfNotExists { get; set; }

    public string? RepoPathBuilderClassName
    {
        get => this[nameof(RepoPathBuilderClassName)]!;
        set => this[nameof(RepoPathBuilderClassName)] = value;
    }

    public Type? RepoPathBuilderType
    {
        get => RepoPathBuilderClassName != null ? Type.GetType(RepoPathBuilderClassName) : null;
        set => RepoPathBuilderClassName = value?.AssemblyQualifiedName;
    }

    public RepositoryType RepositoryType
    {
        get
        {
            var checkValue = this[nameof(RepositoryType)];
            return checkValue != null ? Enum.Parse<RepositoryType>(checkValue) : RepositoryType.Custom;
        }
        set => this[nameof(RepositoryType)] = value.ToString();
    }
}
