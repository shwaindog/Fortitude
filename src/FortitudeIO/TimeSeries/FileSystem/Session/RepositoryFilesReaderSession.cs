// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
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

    private List<InstrumentRepoFileReaderSession<TEntry>>? reverseSortedInstrumentReaderSessions;

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

    public void Reopen() { }

    public IEnumerable<TEntry> StartReaderContext(IReaderContext<TEntry> readerContext)
    {
        var instrumentSessions = sortedInstrumentReaderSessions;

        if (readerContext.IsReverseChronologicalOrder)
        {
            reverseSortedInstrumentReaderSessions ??= new List<InstrumentRepoFileReaderSession<TEntry>>();
            reverseSortedInstrumentReaderSessions.Clear();
            reverseSortedInstrumentReaderSessions.AddRange(((IEnumerable<InstrumentRepoFileReaderSession<TEntry>>)instrumentSessions).Reverse());
            instrumentSessions = reverseSortedInstrumentReaderSessions;
        }


        for (var i = 0; i < instrumentSessions.Count && readerContext.ContinueSearching; i++)
        {
            var fileReaderSession = instrumentSessions[i];
            if (fileReaderSession.ReaderSession is not { IsOpen: true })
            {
                if (fileReaderSession.ReaderSession != null)
                {
                    fileReaderSession.ReaderSession.Reopen();
                }
                else
                {
                    var entryFile     = fileReaderSession.InstrumentRepoFile.TimeSeriesEntryFile<TEntry>();
                    var readerSession = entryFile!.GetReaderSession();
                    fileReaderSession     = new InstrumentRepoFileReaderSession<TEntry>(fileReaderSession.InstrumentRepoFile, readerSession);
                    instrumentSessions[i] = fileReaderSession;
                }
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

    public IReaderContext<TEntry> AllChronologicalEntriesReader
    (IRecycler resultsRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled,
        Func<TEntry>? createNew = null)
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure(this, resultsRecycler, entryResultSourcing, readerOptions, createNew);
        return readerContext;
    }

    public IReaderContext<TEntry> ChronologicalEntriesBetweenTimeRangeReader
    (IRecycler resultsRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled,
        Func<TEntry>? createNew = null)
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure(this, resultsRecycler, entryResultSourcing, readerOptions, createNew);
        readerContext.PeriodRange = periodRange;
        return readerContext;
    }

    public IReaderContext<TEntry> AllChronologicalEntriesReader<TConcreteEntry>
    (IRecycler resultsRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled)
        where TConcreteEntry : class, TEntry, ITimeSeriesEntry<TConcreteEntry>, new()
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure<TConcreteEntry>(this, resultsRecycler, entryResultSourcing, readerOptions);
        return readerContext;
    }

    public IReaderContext<TEntry> ChronologicalEntriesBetweenTimeRangeReader<TConcreteEntry>
    (IRecycler resultsRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        ReaderOptions readerOptions = ReaderOptions.ConsumerControlled)
        where TConcreteEntry : class, TEntry, ITimeSeriesEntry<TConcreteEntry>, new()
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure<TConcreteEntry>(this, resultsRecycler, entryResultSourcing, readerOptions);
        readerContext.PeriodRange = periodRange;
        return readerContext;
    }

    public IReaderContext<TEntry> AllReverseChronologicalEntriesReader
    (IRecycler resultsRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null)
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure(this, resultsRecycler, entryResultSourcing, ReaderOptions.ReverseChronologicalOrder, createNew);
        return readerContext;
    }

    public IReaderContext<TEntry> ReverseChronologicalEntriesBetweenTimeRangeReader
    (IRecycler resultsRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? createNew = null)
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure(this, resultsRecycler, entryResultSourcing, ReaderOptions.ReverseChronologicalOrder, createNew);
        readerContext.PeriodRange = periodRange;
        return readerContext;
    }

    public IReaderContext<TEntry> AllReverseChronologicalEntriesReader<TConcreteEntry>
        (IRecycler resultsRecycler, EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject)
        where TConcreteEntry : class, TEntry, ITimeSeriesEntry<TConcreteEntry>, new()
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure<TConcreteEntry>(this, resultsRecycler, entryResultSourcing, ReaderOptions.ReverseChronologicalOrder);
        return readerContext;
    }

    public IReaderContext<TEntry> ReverseChronologicalEntriesBetweenTimeRangeReader<TConcreteEntry>
    (IRecycler resultsRecycler, UnboundedTimeRange? periodRange,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject)
        where TConcreteEntry : class, TEntry, ITimeSeriesEntry<TConcreteEntry>, new()
    {
        var readerContext = resultsRecycler.Borrow<TimeSeriesReaderContext<TEntry>>();
        readerContext.Configure<TConcreteEntry>(this, resultsRecycler, entryResultSourcing, ReaderOptions.ReverseChronologicalOrder);
        readerContext.PeriodRange = periodRange;
        return readerContext;
    }
}
