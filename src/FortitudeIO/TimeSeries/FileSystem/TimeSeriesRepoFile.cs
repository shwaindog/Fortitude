// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.Config;
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

    public ITimeSeriesFile GetOrOpenTimeSeriesFile(IInstrument instrument)
    {
        TimeSeriesFile ??= FileFactory(instrument).OpenExisting(File);
        return TimeSeriesFile!;
    }

    public ITimeSeriesEntryFile<TEntry> GetOrOpenTimeSeriesFile<TEntry>() where TEntry : ITimeSeriesEntry<TEntry>
    {
        var openTimeSeriesFileAsEntry = TimeSeriesFile as ITimeSeriesEntryFile<TEntry>;
        openTimeSeriesFileAsEntry ??= FileFactory<TEntry>().OpenExistingEntryFile(File);
        TimeSeriesFile            =   openTimeSeriesFileAsEntry;
        return openTimeSeriesFileAsEntry!;
    }

    private ITimeSeriesRepositoryFileFactory FileFactory(IInstrument instrument) =>
        PathFile.FileEntryFactoryRegistry.Values.First(ff => ff.IsBestFactoryFor(instrument));

    private ITimeSeriesRepositoryFileFactory<TEntry> FileFactory<TEntry>() where TEntry : ITimeSeriesEntry<TEntry> =>
        (ITimeSeriesRepositoryFileFactory<TEntry>)PathFile.FileEntryFactoryRegistry[typeof(TEntry)];

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
