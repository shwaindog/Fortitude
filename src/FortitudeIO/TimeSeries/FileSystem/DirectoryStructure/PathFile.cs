// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections.Concurrent;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem.Session;
using static FortitudeIO.TimeSeries.FileSystem.DirectoryStructure.RepositoryPathName;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface IPathFile : IPathPart
{
    IDictionary<Type, ITimeSeriesRepositoryFileFactory> FileEntryFactoryRegistry { get; set; }

    InstrumentEntryRangeMatch InstrumentEntryRangeFileMatch { get; set; }

    IWriterSession<TEntry> GetOrCreateTimeSeriesFileWriter<TEntry>(IInstrument instrument, DateTime fileTimePeriod)
        where TEntry : ITimeSeriesEntry<TEntry>;

    IReaderSession<TEntry> GetRepoFilesReaderSession<TEntry>(IEnumerable<InstrumentRepoFile> instrumentFiles)
        where TEntry : ITimeSeriesEntry<TEntry>;
}

public class PathFile : PathPart, IPathFile
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PathFile));
    private readonly        string   fileExtension;

    public PathFile(string fileExtension, InstrumentEntryRangeMatch instrumentEntryRangeFileMatch)
        : base(new PathName(RepositoryPathName.InstrumentType),
               new PathName(EntryPeriod),
               new PathName(MarketProductType),
               new PathName(InstrumentSource),
               new PathName(InstrumentName),
               new PathName(DirectoryStartDate),
               new PathName(FilePeriod),
               new PathName(Category))
    {
        this.fileExtension            = fileExtension;
        InstrumentEntryRangeFileMatch = instrumentEntryRangeFileMatch;

        if (NameParts.Any(np => np.PathPart == Category))
            if (NameParts[^1].PathPart != Category)
                throw new Exception("Category must be the last part of the name as it is optional");
    }

    public PathFile(string fileExtension, InstrumentEntryRangeMatch instrumentEntryRangeFileMatch, params PathName[] nameParts)
        : base(nameParts)
    {
        this.fileExtension            = fileExtension;
        InstrumentEntryRangeFileMatch = instrumentEntryRangeFileMatch;
    }

    public IDictionary<Type, ITimeSeriesRepositoryFileFactory> FileEntryFactoryRegistry { get; set; }
        = new ConcurrentDictionary<Type, ITimeSeriesRepositoryFileFactory>();

    public IWriterSession<TEntry> GetOrCreateTimeSeriesFileWriter<TEntry>(IInstrument instrument, DateTime fileTimePeriod)
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        var entryType = typeof(TEntry);

        var timeSeriesFileFactory = FileEntryFactoryRegistry[entryType] as ITimeSeriesRepositoryFileFactory<TEntry>;

        if (timeSeriesFileFactory == null) throw new Exception($"No Time Series file factory has been registered for type {entryType}");

        var folderPeriod = ParentDirectory!.PathTimeSeriesPeriod;
        var fileInfo     = new FileInfo(FullPath(instrument, fileTimePeriod, folderPeriod));
        var fileExists   = fileInfo.Exists;
        if (!(fileInfo.Directory?.Exists ?? false)) Directory.CreateDirectory(ParentDirectory.FullPath(instrument, fileTimePeriod, folderPeriod));
        var timeSeriesFile = timeSeriesFileFactory.OpenOrCreate(fileInfo, instrument, folderPeriod, fileTimePeriod);

        if (!fileExists)
            RepositoryRootDirectory.OnFileInstrumentUpdated
                (new RepositoryInstrumentFileUpdateEvent
                    (instrument, this, fileInfo, timeSeriesFile.TimeSeriesPeriodRange, timeSeriesFile, PathUpdateType.Available));
        return timeSeriesFile.GetWriterSession()!;
    }

    public IReaderSession<TEntry> GetRepoFilesReaderSession<TEntry>(IEnumerable<InstrumentRepoFile> instrumentFiles)
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        var entryType = typeof(TEntry);

        var timeSeriesFileFactory = FileEntryFactoryRegistry[entryType] as ITimeSeriesRepositoryFileFactory<TEntry>;

        if (timeSeriesFileFactory == null) throw new Exception($"No Time Series file factory has been registered for type {entryType}");

        var listAvailableFile = new InstrumentRepoFileSet();
        foreach (var instrumentRepoFile in instrumentFiles)
        {
            var timeSeriesFile = timeSeriesFileFactory.OpenExistingEntryFile(instrumentRepoFile.TimeSeriesRepoFile.File);
            if (timeSeriesFile != null)
            {
                instrumentRepoFile.TimeSeriesRepoFile.TimeSeriesFile = timeSeriesFile;
                listAvailableFile.Add(instrumentRepoFile);
            }
            else
            {
                Logger.Warn("File {0} has been moved or deleted and can not be scanned for entries"
                          , instrumentRepoFile.TimeSeriesRepoFile.File.FullName);
            }
        }
        return new RepositoryFilesReaderSession<TEntry>(listAvailableFile);
    }

    public InstrumentEntryRangeMatch InstrumentEntryRangeFileMatch { get; set; }

    public override PathInstrumentMatch InstrumentMatch(PathInstrumentMatch currentMatch)
    {
        if (InstrumentEntryRangeFileMatch.Matches(currentMatch.SearchInstrument)) currentMatch.TimeSeriesFile = this;
        return currentMatch;
    }

    public override PathFileMatch PathMatch(PathFileMatch currentMatch)
    {
        base.PathMatch(currentMatch);
        currentMatch.TimeSeriesFile = this;
        return currentMatch;
    }

    public override string PathName(IInstrument instrument, DateTime timeInPeriod, TimeSeriesPeriod forPeriod) =>
        string.Join(NamePartSeparator, NameParts.Select(tsfpf => tsfpf.GetString(instrument, timeInPeriod, forPeriod))) + fileExtension;
}
