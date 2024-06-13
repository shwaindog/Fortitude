// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections.Concurrent;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.TimeSeries.FileSystem.Session;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;

public interface ITimeSeriesFileStructure : ITimeSeriesPathStructure
{
    IDictionary<Type, ITimeSeriesRepositoryFileFactory> FileEntryFactoryRegistry { get; set; }

    InstrumentMatch InstrumentFileMatch { get; set; }

    IWriterSession<TEntry> GetOrCreateTimeSeriesFileWriter<TEntry>(Instrument instrument, DateTime fileTimePeriod)
        where TEntry : ITimeSeriesEntry<TEntry>;

    IReaderSession<TEntry> GetRepoFilesReaderSession<TEntry>(IEnumerable<InstrumentRepoFile> instrumentFiles)
        where TEntry : ITimeSeriesEntry<TEntry>;
}

public class TimeSeriesFileStructure : TimeSeriesPathStructure, ITimeSeriesFileStructure
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TimeSeriesFileStructure));
    private readonly        string   fileExtension;

    public TimeSeriesFileStructure(string fileExtension, InstrumentMatch instrumentFileMatch)
        : base(new TimeSeriesPathNameFormat(TimeSeriesPathNameComponent.TimeSeriesType),
               new TimeSeriesPathNameFormat(TimeSeriesPathNameComponent.EntryPeriod),
               new TimeSeriesPathNameFormat(TimeSeriesPathNameComponent.SourceName),
               new TimeSeriesPathNameFormat(TimeSeriesPathNameComponent.InstrumentName),
               new TimeSeriesPathNameFormat(TimeSeriesPathNameComponent.FilePeriod),
               new TimeSeriesPathNameFormat(TimeSeriesPathNameComponent.Category))
    {
        this.fileExtension  = fileExtension;
        InstrumentFileMatch = instrumentFileMatch;

        if (NameParts.Any(np => np.PathPart == TimeSeriesPathNameComponent.Category))
            if (NameParts[^1].PathPart != TimeSeriesPathNameComponent.Category)
                throw new Exception("Category must be the last part of the name as it is optional");
    }

    public TimeSeriesFileStructure(string fileExtension, InstrumentMatch instrumentFileMatch, params TimeSeriesPathNameFormat[] nameParts)
        : base(nameParts)
    {
        this.fileExtension  = fileExtension;
        InstrumentFileMatch = instrumentFileMatch;
    }

    public IDictionary<Type, ITimeSeriesRepositoryFileFactory> FileEntryFactoryRegistry { get; set; }
        = new ConcurrentDictionary<Type, ITimeSeriesRepositoryFileFactory>();

    public IWriterSession<TEntry> GetOrCreateTimeSeriesFileWriter<TEntry>(Instrument instrument, DateTime fileTimePeriod)
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        var entryType = typeof(TEntry);

        var timeSeriesFileFactory = FileEntryFactoryRegistry[entryType] as ITimeSeriesRepositoryFileFactory<TEntry>;

        if (timeSeriesFileFactory == null) throw new Exception($"No Time Series file factory has been registered for type {entryType}");

        var fileInfo       = new FileInfo(FullPath(instrument, fileTimePeriod));
        var folderPeriod   = ParentDirectory!.PathTimeSeriesPeriod;
        var timeSeriesFile = timeSeriesFileFactory.OpenOrCreate(fileInfo, instrument, folderPeriod, fileTimePeriod);

        return timeSeriesFile.GetWriterSession()!;
    }

    public IReaderSession<TEntry> GetRepoFilesReaderSession<TEntry>(IEnumerable<InstrumentRepoFile> instrumentFiles)
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        var entryType = typeof(TEntry);

        var timeSeriesFileFactory = FileEntryFactoryRegistry[entryType] as ITimeSeriesRepositoryFileFactory<TEntry>;

        if (timeSeriesFileFactory == null) throw new Exception($"No Time Series file factory has been registered for type {entryType}");

        var listAvailableFile = new List<InstrumentRepoFile>();
        foreach (var instrumentRepoFile in instrumentFiles)
        {
            var timeSeriesFile = timeSeriesFileFactory.OpenExisting(instrumentRepoFile.TimeSeriesRepoFile.File);
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

    public InstrumentMatch InstrumentFileMatch { get; set; }

    public override TimeSeriesInstrumentStructureMatch InstrumentMatch(TimeSeriesInstrumentStructureMatch currentMatch)
    {
        if (InstrumentFileMatch.Matches(currentMatch.SearchInstrument)) currentMatch.TimeSeriesFile = this;
        return currentMatch;
    }

    public override TimeSeriesFileStructureMatch PathMatch(TimeSeriesFileStructureMatch currentMatch)
    {
        base.PathMatch(currentMatch);
        currentMatch.TimeSeriesFile = this;
        return currentMatch;
    }

    public override string PathName(Instrument instrument, DateTime timeInPeriod) =>
        string.Join(NamePartSeparator, NameParts.Select(tsfpf => tsfpf.GetString(instrument, timeInPeriod))) + "." + fileExtension;
}
