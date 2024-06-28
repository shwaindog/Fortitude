// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Session;

public struct InstrumentRepoFileReaderSession<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>
{
    public InstrumentRepoFileReaderSession(InstrumentRepoFile instrumentRepoFile) => InstrumentRepoFile = instrumentRepoFile;

    public InstrumentRepoFileReaderSession(InstrumentRepoFile instrumentRepoFile, IFileReaderSession<TEntry> readerSession)
    {
        InstrumentRepoFile = instrumentRepoFile;
        ReaderSession      = readerSession;
    }

    public InstrumentRepoFile          InstrumentRepoFile;
    public IFileReaderSession<TEntry>? ReaderSession;
}

public class RepositoryFilesReaderSession<TEntry> : IReaderSession<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly List<InstrumentRepoFileReaderSession<TEntry>> sortedInstrumentReaderSessions;

    public RepositoryFilesReaderSession(InstrumentRepoFileSet repoFiles)
    {
        repoFiles.Sort();
        sortedInstrumentReaderSessions = repoFiles.Select(irf => new InstrumentRepoFileReaderSession<TEntry>(irf)).ToList();
    }


    public void Dispose()
    {
        foreach (var fileReaderSession in sortedInstrumentReaderSessions) fileReaderSession.ReaderSession?.Dispose();
    }

    public bool IsOpen => sortedInstrumentReaderSessions.Any(irf => irf.ReaderSession?.IsOpen ?? false);

    public void Close()
    {
        foreach (var fileReaderSession in sortedInstrumentReaderSessions) fileReaderSession.ReaderSession?.Dispose();
    }

    public IEnumerable<TEntry> StartReaderContext(IReaderContext<TEntry> readerContext)
    {
        for (var i = 0; i < sortedInstrumentReaderSessions.Count && readerContext.ContinueSearching; i++)
        {
            var fileReaderSession = sortedInstrumentReaderSessions[i];
            if (fileReaderSession.ReaderSession is not { IsOpen: true })
            {
                var entryFile     = fileReaderSession.InstrumentRepoFile.TimeSeriesRepoFile.TimeSeriesFile as ITimeSeriesEntryFile<TEntry>;
                var readerSession = entryFile!.GetReaderSession();
                fileReaderSession                 = new InstrumentRepoFileReaderSession<TEntry>(fileReaderSession.InstrumentRepoFile, readerSession);
                sortedInstrumentReaderSessions[i] = fileReaderSession;
            }
            foreach (var fileEntry in fileReaderSession.ReaderSession!.StartReaderContext(readerContext)) yield return fileEntry;
        }
    }

    public void VisitChildrenCacheAndClose()
    {
        for (var i = 0; i < sortedInstrumentReaderSessions.Count; i++)
        {
            var fileReaderSession = sortedInstrumentReaderSessions[i];
            if (fileReaderSession.ReaderSession is not { IsOpen: true })
            {
                var entryFile     = fileReaderSession.InstrumentRepoFile.TimeSeriesRepoFile.TimeSeriesFile as ITimeSeriesEntryFile<TEntry>;
                var readerSession = entryFile!.GetReaderSession();
                fileReaderSession                 = new InstrumentRepoFileReaderSession<TEntry>(fileReaderSession.InstrumentRepoFile, readerSession);
                sortedInstrumentReaderSessions[i] = fileReaderSession;
            }
            fileReaderSession.ReaderSession!.VisitChildrenCacheAndClose();
        }
    }

    public IReaderContext<TEntry> GetAllEntriesReader
    (EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null) =>
        new TimeSeriesReaderContext<TEntry>(this, entryResultSourcing, createNew);

    public IReaderContext<TEntry> GetEntriesBetweenReader
    (UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null) =>
        new TimeSeriesReaderContext<TEntry>(this, entryResultSourcing, createNew)
        {
            PeriodRange = periodRange
        };
}
