#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public enum TimeSeriesFileStatus
{
    Unknown
    , New
    , CompleteHealthy
    , IncompleteAppending
    , WriterInterrupted
    , Corrupt
}

public interface ITimeSeriesFile : IDisposable
{
    ushort FileVersion { get; }
    ITimeSeriesFileHeader Header { get; }

    TimeSeriesPeriod TimeSeriesPeriod { get; }

    DateTime PeriodStartTime { get; }

    string FileName { get; }
    bool IsOpen { get; }

    ITimeSeriesFileSession? GetWriterSession();
    ITimeSeriesFileSession GetReaderSession();
    ITimeSeriesFileSession GetInfoSession();
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

public interface IMutableTimeSeriesFile : ITimeSeriesFile
{
    new IMutableTimeSeriesFileHeader Header { get; set; }
}

public unsafe class TimeSeriesFile<TBucket, TEntry> : ITimeSeriesFile<TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry>
{
    private IBucketIndexDictionary? fileBucketIndexOffsets;
    private IReaderFileSession<TBucket, TEntry>? infoSession;
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

    public TimeSeriesFile(string filePath, TimeSeriesPeriod filePeriod, DateTime fileStartPeriod, int initialFileSizePages,
        FileFlags fileFlags = FileFlags.None, uint internalIndexSize = 0, ushort maxStringSizeBytes = 512)
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

    public void Dispose()
    {
        Close();
    }

    public ushort FileVersion { get; }

    ITimeSeriesFileHeader ITimeSeriesFile.Header => Header;
    public TimeSeriesPeriod TimeSeriesPeriod { get; }
    public DateTime PeriodStartTime { get; }
    ITimeSeriesFileSession? ITimeSeriesFile.GetWriterSession() => GetWriterSession();
    ITimeSeriesFileSession ITimeSeriesFile.GetReaderSession() => GetReaderSession();
    ITimeSeriesFileSession ITimeSeriesFile.GetInfoSession() => GetInfoSession();

    public string FileName { get; }

    public bool IsOpen =>
        Header.FileIsOpen || (writerSession?.IsOpen ?? false)
                          || readerSessions.Any(rs => rs.IsOpen) || (infoSession?.IsOpen ?? false);

    public void Close()
    {
        writerSession?.Close();
        foreach (var readerSession in readerSessions) readerSession.Close();
        infoSession?.Close();
        fileBucketIndexOffsets?.CacheAndCloseFileView();
        fileBucketIndexOffsets = null;
        Header.CloseFileView();
    }

    public bool ReopenFile(FileFlags fileFlags = FileFlags.None)
    {
        if (Header.FileIsOpen) return true;
        TimeSeriesMemoryMappedFile ??= new PagedMemoryMappedFile(FileName);
        var headerFileView = TimeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        Header.ReopenFileView(headerFileView, fileFlags);
        return true;
    }

    public IWriterFileSession<TBucket, TEntry>? GetWriterSession()
    {
        var existingOpen = writerSession?.IsOpen ?? false;
        if (existingOpen) return null;
        writerSession?.ReopenSession(FileFlags.WriterOpened);
        writerSession ??= new TimeSeriesFileSession<TBucket, TEntry>(this, true);
        return writerSession;
    }

    public IReaderFileSession<TBucket, TEntry> GetReaderSession()
    {
        IReaderFileSession<TBucket, TEntry>? freeReaderSession = null;
        var shouldAddToExisting = true;
        if (readerSessions.Any(rs => !rs.IsOpen))
        {
            freeReaderSession = readerSessions.FirstOrDefault(rs => !rs.IsOpen);
            shouldAddToExisting = freeReaderSession == null;
        }

        freeReaderSession ??= new TimeSeriesFileSession<TBucket, TEntry>(this, false);
        if (shouldAddToExisting)
            readerSessions.Add(freeReaderSession);
        else
            freeReaderSession.ReopenSession();
        return freeReaderSession;
    }

    public IReaderFileSession<TBucket, TEntry> GetInfoSession()
    {
        infoSession ??= new TimeSeriesFileSession<TBucket, TEntry>(this, false);
        return infoSession;
    }
}
