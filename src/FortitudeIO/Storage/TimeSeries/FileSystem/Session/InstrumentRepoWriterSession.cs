// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Storage.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.Session;

public class InstrumentRepoWriterSession<TEntry> : IWriterSession<TEntry> where TEntry : ITimeSeriesEntry
{
    private readonly IInstrument instrument;
    private readonly IPathFile   pathFile;

    private IWriterSession<TEntry>? currentFileWriterSession;

    private StorageAttemptResult lastStorageAttemptResult;

    public InstrumentRepoWriterSession(IInstrument instrument, IPathFile pathFile)
    {
        this.instrument = instrument;
        this.pathFile   = pathFile;
    }

    public void Reopen()
    {
        currentFileWriterSession?.Reopen();
    }

    public void Dispose()
    {
        currentFileWriterSession?.Close();
    }

    public bool IsOpen => currentFileWriterSession?.IsOpen ?? false;

    public void Close()
    {
        currentFileWriterSession?.Close();
    }

    public AppendResult AppendEntry(TEntry entry)
    {
        AppendResult appendResult;
        if (lastStorageAttemptResult == StorageAttemptResult.PeriodRangeMatched && currentFileWriterSession != null)
        {
            appendResult             = currentFileWriterSession.AppendEntry(entry);
            lastStorageAttemptResult = appendResult.StorageAttemptResult;
            if (lastStorageAttemptResult == StorageAttemptResult.PeriodRangeMatched) return appendResult;
            currentFileWriterSession.Close();
        }

        currentFileWriterSession = pathFile.GetOrCreateTimeSeriesFileWriter<TEntry>(instrument, entry.StorageTime());
        appendResult             = currentFileWriterSession.AppendEntry(entry);
        lastStorageAttemptResult = appendResult.StorageAttemptResult;
        return appendResult;
    }
}
