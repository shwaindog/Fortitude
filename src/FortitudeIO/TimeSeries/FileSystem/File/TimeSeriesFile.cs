#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public interface ITimeSeriesFile : IDisposable
{
    ushort FileVersion { get; }
    ITimeSeriesFileHeader Header { get; }
    TimeSeriesPeriod TimeSeriesPeriod { get; }
    bool AutoCloseOnZeroSessions { get; set; }
    int SessionCount { get; }
    DateTime PeriodStartTime { get; }
    string InstrumentName { get; }
    string Category { get; }
    string SourceName { get; }
    string FileName { get; }
    bool IsOpen { get; }
    Type EntryType { get; }
    ITimeSeriesFileSession? GetWriterSession();
    ITimeSeriesFileSession GetReaderSession();
    ITimeSeriesFileSession GetInfoSession();
    bool Intersects(PeriodRange? periodRange = null);
    void Close();
    bool ReopenFile(FileFlags fileFlags = FileFlags.None);
}

public interface ITimeSeriesFile<TBucket, TEntry> : ITimeSeriesFile
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    new IWriterFileSession<TBucket, TEntry>? GetWriterSession();
    new IReaderFileSession<TBucket, TEntry> GetReaderSession();
    new IReaderFileSession<TBucket, TEntry> GetInfoSession();
}

public interface ITimeSeriesEntryFile<TEntry> : ITimeSeriesFile
    where TEntry : ITimeSeriesEntry<TEntry>
{
    Func<TEntry>? DefaultEntryFactory { get; set; }
    IEnumerable<TEntry>? EntriesFor(PeriodRange? periodRange = null, int? remainingLimit = null);
}

public interface IMutableTimeSeriesFile : ITimeSeriesFile
{
    new string SourceName { get; set; }
    new string InstrumentName { get; set; }
    new string Category { get; set; }
    new IMutableTimeSeriesFileHeader Header { get; set; }
}

public unsafe class TimeSeriesFile<TBucket, TEntry> : ITimeSeriesFile<TBucket, TEntry>, ITimeSeriesEntryFile<TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private IBucketIndexDictionary? fileBucketIndexOffsets;
    private IReaderFileSession<TBucket, TEntry>? infoSession;
    private int numberOfOpenSessions;
    private List<IReaderFileSession<TBucket, TEntry>> readerSessions = new();
    private IWriterFileSession<TBucket, TEntry>? writerSession;

    public TimeSeriesFile(string filePath)
    {
        FileName = filePath;
        TimeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath);
        var headerFileView = TimeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr = headerFileView.LowerHalfViewVirtualMemoryAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            Header = new TimeSeriesFileHeaderFromV1(headerFileView, true);
        else
            throw new Exception("Unsupported file version being loaded");

        if (Header.BucketType != typeof(TBucket))
            throw new Exception("Attempting to open a file saved with a different bucket type");
        TimeSeriesPeriod = Header.FilePeriod;
        PeriodStartTime = Header.FileStartPeriod;
    }

    public TimeSeriesFile(string filePath, string instrumentName, string sourceName, TimeSeriesPeriod filePeriod,
        DateTime fileStartPeriod, uint internalIndexSize = 0, FileFlags fileFlags = FileFlags.None, int initialFileSizePages = 2,
        ushort maxStringSizeBytes = byte.MaxValue, string? instrumentCategory = null)
    {
        FileName = filePath;
        TimeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath, initialFileSizePages);
        var headerFileView = TimeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr = headerFileView.LowerHalfViewVirtualMemoryAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            Header = TimeSeriesFileHeaderFromV1.NewFileCreateHeader(headerFileView, fileFlags, internalIndexSize, maxStringSizeBytes);
        else
            throw new Exception("Unsupported file version being loaded");

        TimeSeriesPeriod = filePeriod;
        PeriodStartTime = TimeSeriesPeriod.ContainingPeriodBoundaryStart(fileStartPeriod);
        Header.FilePeriod = filePeriod;
        Header.FileStartPeriod = fileStartPeriod;
    }

    internal PagedMemoryMappedFile? TimeSeriesMemoryMappedFile { get; private set; }
    public IMutableTimeSeriesFileHeader Header { get; }

    public IEnumerable<TEntry>? EntriesFor(PeriodRange? periodRange = null, int? remainingLimit = null)
    {
        var wasOpen = IsOpen;
        var readerSession = GetReaderSession();
        var rangeReader = readerSession.GetEntriesBetweenReader(periodRange, DefaultEntryFactory);
        if (remainingLimit != null) rangeReader.MaxResults = remainingLimit.Value;
        foreach (var result in rangeReader.ResultEnumerable) yield return result;
        readerSession.Close();
        if (IsOpen && numberOfOpenSessions <= 0 && !wasOpen) Close();
    }

    public Func<TEntry>? DefaultEntryFactory { get; set; }

    public void Dispose()
    {
        Close();
    }

    public ushort FileVersion { get; }

    public bool AutoCloseOnZeroSessions { get; set; } = true;
    public int SessionCount => numberOfOpenSessions;

    ITimeSeriesFileHeader ITimeSeriesFile.Header => Header;
    public TimeSeriesPeriod TimeSeriesPeriod { get; }
    public DateTime PeriodStartTime { get; }

    ITimeSeriesFileSession? ITimeSeriesFile.GetWriterSession() => GetWriterSession();

    ITimeSeriesFileSession ITimeSeriesFile.GetReaderSession() => GetReaderSession();
    ITimeSeriesFileSession ITimeSeriesFile.GetInfoSession() => GetInfoSession();

    public string SourceName { get; set; }
    public Type EntryType => typeof(TEntry);
    public string InstrumentName { get; set; }
    public string Category { get; set; }
    public bool Intersects(PeriodRange? periodRange = null) => periodRange.IntersectsWith(TimeSeriesPeriod, PeriodStartTime);

    public string FileName { get; }
    public bool IsOpen => Header.FileIsOpen || numberOfOpenSessions > 0;

    public void Close()
    {
        writerSession?.Close();
        foreach (var readerSession in readerSessions) readerSession.Close();
        infoSession?.Close();
        fileBucketIndexOffsets?.CacheAndCloseFileView();
        fileBucketIndexOffsets = null;
        Header.CloseFileView();
        TimeSeriesMemoryMappedFile = null;
    }

    public bool ReopenFile(FileFlags fileFlags = FileFlags.None)
    {
        if (Header.FileIsOpen) return true;
        if (TimeSeriesMemoryMappedFile is not { IsOpen: true }) TimeSeriesMemoryMappedFile = new PagedMemoryMappedFile(FileName);
        var headerFileView = TimeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        Header.ReopenFileView(headerFileView, fileFlags);
        return true;
    }

    public IWriterFileSession<TBucket, TEntry>? GetWriterSession()
    {
        var existingOpen = writerSession?.IsOpen ?? false;
        if (existingOpen) return null;
        IncrementSessionCount();
        writerSession?.ReopenSession(FileFlags.WriterOpened);
        writerSession ??= new TimeSeriesFileSession<TBucket, TEntry>(this, true);
        return writerSession;
    }

    public IReaderFileSession<TBucket, TEntry> GetReaderSession()
    {
        if (!IsOpen) ReopenFile();
        IReaderFileSession<TBucket, TEntry>? freeReaderSession = null;
        var shouldAddToExisting = true;
        if (readerSessions.Any(rs => !rs.IsOpen))
        {
            freeReaderSession = readerSessions.FirstOrDefault(rs => !rs.IsOpen);
            shouldAddToExisting = freeReaderSession == null;
        }

        IncrementSessionCount();
        freeReaderSession ??= new TimeSeriesFileSession<TBucket, TEntry>(this, false);
        if (shouldAddToExisting)
            readerSessions.Add(freeReaderSession);
        else
            freeReaderSession.ReopenSession();
        return freeReaderSession;
    }

    public IReaderFileSession<TBucket, TEntry> GetInfoSession()
    {
        if (infoSession == null)
        {
            IncrementSessionCount();
            var wasOpen = IsOpen;
            if (!wasOpen) ReopenFile();
            infoSession = new TimeSeriesFileSession<TBucket, TEntry>(this, false);
            infoSession.VisitChildrenCacheAndClose();
            if (IsOpen && numberOfOpenSessions <= 0 && !wasOpen) Close();
        }

        return infoSession;
    }

    public int IncrementSessionCount() => Interlocked.Increment(ref numberOfOpenSessions);

    public int DecrementSessionCount()
    {
        var remainingSessions = Interlocked.Decrement(ref numberOfOpenSessions);
        if (AutoCloseOnZeroSessions && remainingSessions <= 0) Close();
        return remainingSessions;
    }
}
