// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Config;

public enum RepositoryType
{
    Dymwi              // decade, year, month, Week, Instrument Details folder structure
  , RemoteLocalCaching // copies files from a network mapped remote drive of the same structure to local disk then reads
  , Custom             // TODO implement config for custom folder structure
}

public enum RepositoryProximity
{
    Local
  , Remote
  , Both
}

public interface IFileRepositoryLocationConfig
{
    string? RepositoryName          { get; set; }
    string  RootDirectoryPath       { get; set; }
    string  TimeSeriesFileExtension { get; set; }

    RepositoryProximity Proximity { get; set; }
}

public class FileRepositoryLocationConfig : ConfigSection, IFileRepositoryLocationConfig
{
    public FileRepositoryLocationConfig(IConfigurationRoot root, string path) : base(root, path) { }
    public FileRepositoryLocationConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FileRepositoryLocationConfig(IFileRepositoryLocationConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        RootDirectoryPath       = toClone.RootDirectoryPath;
        Proximity               = toClone.Proximity;
        RepositoryName          = toClone.RepositoryName;
        TimeSeriesFileExtension = toClone.TimeSeriesFileExtension;
    }

    public FileRepositoryLocationConfig(IFileRepositoryLocationConfig toClone) : this(InMemoryConfigRoot, InMemoryPath)
    {
        RootDirectoryPath       = toClone.RootDirectoryPath;
        Proximity               = toClone.Proximity;
        RepositoryName          = toClone.RepositoryName;
        TimeSeriesFileExtension = toClone.TimeSeriesFileExtension;
    }

    public FileRepositoryLocationConfig
    (string repositoryRootDirectoryPath, RepositoryProximity proximity
      , IConfigurationRoot root, string path, string? repoName = null, string timeSeriesFileExtension = ".tsf") : this(root, path)
    {
        RootDirectoryPath       = repositoryRootDirectoryPath;
        Proximity               = proximity;
        RepositoryName          = repoName;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public FileRepositoryLocationConfig
        (string repositoryRootDirectoryPath, RepositoryProximity proximity, string? repoName = null, string timeSeriesFileExtension = ".tsf")
        : this(InMemoryConfigRoot, InMemoryPath)
    {
        RootDirectoryPath       = repositoryRootDirectoryPath;
        Proximity               = proximity;
        RepositoryName          = repoName;
        TimeSeriesFileExtension = timeSeriesFileExtension;
    }

    public string? RepositoryName
    {
        get => this[nameof(RepositoryName)]!;
        set => this[nameof(RepositoryName)] = value;
    }

    public string RootDirectoryPath
    {
        get => this[nameof(RootDirectoryPath)]!;
        set => this[nameof(RootDirectoryPath)] = value;
    }

    public string TimeSeriesFileExtension
    {
        get => this[nameof(TimeSeriesFileExtension)]!;
        set => this[nameof(TimeSeriesFileExtension)] = value;
    }

    public RepositoryProximity Proximity
    {
        get
        {
            var checkValue = this[nameof(Proximity)];
            return checkValue != null ? Enum.Parse<RepositoryProximity>(checkValue) : RepositoryProximity.Local;
        }
        set => this[nameof(Proximity)] = value.ToString();
    }
}
