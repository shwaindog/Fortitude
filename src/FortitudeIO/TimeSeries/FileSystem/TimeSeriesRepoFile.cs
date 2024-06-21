// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem;

public class TimeSeriesRepoFile
{
    public TimeSeriesRepoFile(FileInfo file, IRepositoryRootDirectory repositoryRoot, IPathFile pathFile)
    {
        RepositoryRoot = repositoryRoot;
        PathFile       = pathFile;
        File           = file;
    }

    public FileInfo                 File           { get; }
    public IRepositoryRootDirectory RepositoryRoot { get; }
    public IPathFile                PathFile       { get; }

    public RepositoryProximity Proximity => RepositoryRoot.Repository.Proximity;

    public string RootDirRelativePath => File.FullName.Replace(RepositoryRoot.DirPath + Path.DirectorySeparatorChar, "");

    public ITimeSeriesFile? TimeSeriesFile { get; set; }

    public string RepositoryRelativePath => File.FullName.Replace(RepositoryRoot.DirInfo.FullName, "." + Path.DirectorySeparatorChar);

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
