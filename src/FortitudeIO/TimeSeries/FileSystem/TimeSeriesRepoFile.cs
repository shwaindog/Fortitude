// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public class TimeSeriesRepoFile
{
    public TimeSeriesRepoFile(FileInfo file, RepositoryRootDirectoryStructure repositoryRoot, ITimeSeriesFileStructure repositoryFileStructure)
    {
        RepositoryRoot          = repositoryRoot;
        RepositoryFileStructure = repositoryFileStructure;
        File                    = file;
    }

    public FileInfo                         File                    { get; }
    public RepositoryRootDirectoryStructure RepositoryRoot          { get; }
    public ITimeSeriesFileStructure         RepositoryFileStructure { get; }

    public ITimeSeriesFile? TimeSeriesFile { get; set; }

    public string RepositoryRelativePath => File.FullName.Replace(RepositoryRoot.RootDirectoryInfo.FullName, "." + Path.DirectorySeparatorChar);

    protected bool Equals(TimeSeriesRepoFile other) => File.Equals(other.File);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TimeSeriesRepoFile)obj);
    }

    public override int GetHashCode() => File.GetHashCode();
}
