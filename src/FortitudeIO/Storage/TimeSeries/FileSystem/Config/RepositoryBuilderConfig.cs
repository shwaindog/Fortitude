// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.Config;

public interface IRepositoryBuilder
{
    RepositoryInfo        BuildRepositoryInfo(string? repositoryName = null);
    ITimeSeriesRepository BuildRepository(string? repositoryName = null);
}

public interface IRepositoryBuilderConfig : IRepositoryBuilder, IStringBearer
{
    string?        RepoPathBuilderClassName              { get; set; }
    Type?          RepoPathBuilderType                   { get; set; }
    bool           CreateIfNotExists                     { get; set; }
    RepositoryType RepositoryType                        { get; set; }
    string[]       RequiredInstrumentAttributeFieldNames { get; set; }
    string[]?      OptionalInstrumentAttributeFieldNames { get; set; }

    IRepoPathBuilder RepositoryPathBuilder(IFileRepositoryLocationConfig fileRepoLocationConfig);
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

    public abstract ITimeSeriesRepository BuildRepository(string? repositoryName = "NoRepositoryName");

    public abstract RepositoryInfo BuildRepositoryInfo(string? repositoryName = "NoRepositoryName");

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

    public string[] RequiredInstrumentAttributeFieldNames
    {
        get => GetSection(nameof(RequiredInstrumentAttributeFieldNames)).GetChildren().Select(c => c.Value).OfType<string>().ToArray();
        set
        {
            var existingSection = GetSection(nameof(RequiredInstrumentAttributeFieldNames));
            var existingKeys    = existingSection.GetChildren().Select(c => c.Key).ToList();
            if (!value.Any())
            {
                foreach (var key in existingKeys) existingSection[key] = null;
                return;
            }
            for (var i = 0; i < value.Length; i++)
            {
                var key = $"{i}";
                existingSection[key] = value[i];
                if (existingKeys.Count > 0) existingKeys.RemoveAt(0);
            }
            foreach (var extraKey in existingKeys) existingSection[extraKey] = null;
        }
    }

    public string[]? OptionalInstrumentAttributeFieldNames
    {
        get => GetSection(nameof(OptionalInstrumentAttributeFieldNames)).GetChildren().Select(c => c.Value).OfType<string>().ToArray();
        set
        {
            var existingSection = GetSection(nameof(OptionalInstrumentAttributeFieldNames));
            var existingKeys    = existingSection.GetChildren().Select(c => c.Key).ToList();
            if (value == null || !value.Any())
            {
                foreach (var key in existingKeys) existingSection[key] = null;
                return;
            }
            for (var i = 0; i < value.Length; i++)
            {
                var key = $"{i}";
                existingSection[key] = value[i];
                if (existingKeys.Count > 0) existingKeys.RemoveAt(0);
            }
            foreach (var extraKey in existingKeys) existingSection[extraKey] = null;
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString stsa) => 
        stsa.StartComplexType(this)
            .Field.AlwaysAdd(nameof(RepoPathBuilderClassName), RepoPathBuilderClassName)
            .Field.AlwaysAdd(nameof(RepositoryType), RepositoryType)
            .Field.AlwaysAdd(nameof(CreateIfNotExists), CreateIfNotExists)
            .CollectionField.AlwaysAddAll(nameof(RequiredInstrumentAttributeFieldNames), RequiredInstrumentAttributeFieldNames)
            .CollectionField.AlwaysAddAll(nameof(OptionalInstrumentAttributeFieldNames), OptionalInstrumentAttributeFieldNames)
            .Complete();
}
