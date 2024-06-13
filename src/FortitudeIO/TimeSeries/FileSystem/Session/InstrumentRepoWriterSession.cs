// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.DirectoryStructure;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.Session;

public class InstrumentRepoWriterSession<TEntry> : IWriterSession<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private readonly ITimeSeriesFileStructure fileStructure;
    private readonly Instrument               instrument;

    private IWriterSession<TEntry>? currentFileWriterSession;

    private StorageAttemptResult lastStorageAttemptResult;

    public InstrumentRepoWriterSession(Instrument instrument, ITimeSeriesFileStructure fileStructure)
    {
        this.instrument    = instrument;
        this.fileStructure = fileStructure;
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
        }

        currentFileWriterSession = fileStructure.GetOrCreateTimeSeriesFileWriter<TEntry>(instrument, entry.StorageTime());
        appendResult             = currentFileWriterSession.AppendEntry(entry);
        lastStorageAttemptResult = appendResult.StorageAttemptResult;
        return appendResult;
    }
}
