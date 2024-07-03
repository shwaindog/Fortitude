// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeIO.TimeSeries.FileSystem.File.Session;
using FortitudeIO.TimeSeries.FileSystem.Session;
using FortitudeIO.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

public interface ITimeSeriesFile : IDisposable
{
    ushort                FileVersion           { get; }
    ITimeSeriesFileHeader Header                { get; }
    TimeSeriesPeriodRange TimeSeriesPeriodRange { get; }

    bool AutoCloseOnZeroSessions { get; set; }

    int            SessionCount   { get; }
    InstrumentType InstrumentType { get; }
    string         InstrumentName { get; }
    string         Category       { get; }
    string         SourceName     { get; }
    string         FileName       { get; }
    bool           IsOpen         { get; }

    Type                EntryType { get; }
    ITimeSeriesSession? GetWriterSession();
    ITimeSeriesSession  GetReaderSession();
    ITimeSeriesSession  GetInfoSession();

    bool Intersects(UnboundedTimeRange? periodRange = null);
    void Close();
    bool ReopenFile(FileFlags fileFlags = FileFlags.None);
}

public interface ITimeSeriesFile<TBucket, TEntry> : ITimeSeriesFile
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> where TEntry : ITimeSeriesEntry<TEntry> { }

public interface ITimeSeriesEntryFile<TEntry> : ITimeSeriesFile
    where TEntry : ITimeSeriesEntry<TEntry>
{
    Func<TEntry>? DefaultEntryFactory { get; set; }

    new IFileWriterSession<TEntry>? GetWriterSession();
    new IFileReaderSession<TEntry>  GetReaderSession();
    new IFileReaderSession<TEntry>  GetInfoSession();

    IEnumerable<TEntry>? EntriesFor
    (UnboundedTimeRange? periodRange = null, int? remainingLimit = null,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? entryFactory = null);
}

public interface IMutableTimeSeriesFile : ITimeSeriesFile
{
    new string SourceName     { get; set; }
    new string InstrumentName { get; set; }
    new string Category       { get; set; }

    new IMutableTimeSeriesFileHeader Header { get; }
}

public unsafe class TimeSeriesFile<TFile, TBucket, TEntry> : ITimeSeriesFile<TBucket, TEntry>, ITimeSeriesEntryFile<TEntry>, IMutableTimeSeriesFile
    where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry>

{
    private readonly List<IFileReaderSession<TEntry>> readerSessions = new();

    protected IBucketFactory<TBucket>? FileBucketFactory;

    private IBucketIndexDictionary?     fileBucketIndexOffsets;
    private IFileReaderSession<TEntry>? infoSession;
    private int                         numberOfOpenSessions;
    private IFileWriterSession<TEntry>? writerSession;

    // derived classes should implement equivalent parameter constructor to be open with OpenExistingTimeSeriesFile(string filePath)
    public TimeSeriesFile(PagedMemoryMappedFile pagedMemoryMappedFile, IMutableTimeSeriesFileHeader header)
    {
        FileName                   = pagedMemoryMappedFile.FileStream.Name;
        TimeSeriesMemoryMappedFile = pagedMemoryMappedFile;
        Header                     = header;
        if (Header.BucketType != typeof(TBucket)) throw new Exception("Attempting to open a file saved with a different bucket type");
        if (Header.EntryType != typeof(TEntry)) throw new Exception("Attempting to open a file saved with a different entry type");
    }

    public TimeSeriesFile(TimeSeriesFileParameters timeSeriesParameters)
    {
        FileName = timeSeriesParameters.TimeSeriesFileInfo.FullName;

        TimeSeriesMemoryMappedFile = new PagedMemoryMappedFile(FileName, timeSeriesParameters.InitialFileSize);

        var headerFileView = TimeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr            = headerFileView.StartAddress;
        FileVersion = StreamByteOps.ToUShort(ref ptr);
        if (FileVersion is 0 or 1) // select appropriate header file based on file version
            Header = TimeSeriesFileHeaderFromV1.NewFileCreateHeader(this, headerFileView, timeSeriesParameters);
        else
            throw new Exception("Unsupported file version being loaded");
    }

    internal PagedMemoryMappedFile? TimeSeriesMemoryMappedFile { get; private set; }

    public virtual IBucketFactory<TBucket> RootBucketFactory
    {
        get { return FileBucketFactory ??= new BucketFactory<TBucket>(true); }
        set => FileBucketFactory = value;
    }

    public virtual IStorageTimeResolver<TEntry>? StorageTimeResolver => null!;
    private        TimeSeriesPeriod              TimeSeriesPeriod    => Header.FilePeriod;
    private        DateTime                      PeriodStartTime     => Header.FileStartPeriod;

    public IMutableTimeSeriesFileHeader Header { get; }

    public Func<TEntry>? DefaultEntryFactory { get; set; }

    public IEnumerable<TEntry>? EntriesFor
    (UnboundedTimeRange? periodRange = null, int? remainingLimit = null,
        EntryResultSourcing entryResultSourcing = EntryResultSourcing.ReuseSingletonObject,
        Func<TEntry>? entryFactory = null)
    {
        var wasOpen       = IsOpen;
        var readerSession = GetReaderSession();
        var rangeReader   = readerSession.GetEntriesBetweenReader(periodRange, entryResultSourcing, entryFactory ?? DefaultEntryFactory);

        if (remainingLimit != null) rangeReader.MaxResults = remainingLimit.Value;
        foreach (var result in rangeReader.ResultEnumerable) yield return result;
        readerSession.Close();
        if (IsOpen && numberOfOpenSessions <= 0 && !wasOpen) Close();
    }

    public IFileWriterSession<TEntry>? GetWriterSession()
    {
        var existingOpen = writerSession?.IsOpen ?? false;
        if (existingOpen) return null;
        IncrementSessionCount();
        writerSession?.ReopenSession(FileFlags.WriterOpened);
        writerSession ??= new TimeSeriesFileSession<TFile, TBucket, TEntry>(this, true, ushort.MaxValue * 4);
        return writerSession;
    }

    public IFileReaderSession<TEntry> GetReaderSession()
    {
        if (!IsOpen) ReopenFile();
        IFileReaderSession<TEntry>? freeReaderSession   = null;
        var                         shouldAddToExisting = true;
        if (readerSessions.Any(rs => !rs.IsOpen))
        {
            freeReaderSession   = readerSessions.FirstOrDefault(rs => !rs.IsOpen);
            shouldAddToExisting = freeReaderSession == null;
        }

        IncrementSessionCount();
        freeReaderSession ??= new TimeSeriesFileSession<TFile, TBucket, TEntry>(this, false);
        if (shouldAddToExisting)
            readerSessions.Add(freeReaderSession);
        else
            freeReaderSession.ReopenSession();
        return freeReaderSession;
    }

    public IFileReaderSession<TEntry> GetInfoSession()
    {
        if (infoSession == null)
        {
            IncrementSessionCount();
            var wasOpen = IsOpen;
            if (!wasOpen) ReopenFile();
            infoSession = new TimeSeriesFileSession<TFile, TBucket, TEntry>(this, false);
            infoSession.VisitChildrenCacheAndClose();
            if (IsOpen && numberOfOpenSessions <= 0 && !wasOpen) Close();
        }

        return infoSession;
    }

    public TimeSeriesPeriodRange TimeSeriesPeriodRange => new(PeriodStartTime, TimeSeriesPeriod);

    public void Dispose()
    {
        Close();
    }

    public ushort FileVersion { get; }

    public bool AutoCloseOnZeroSessions { get; set; } = true;
    public int  SessionCount            => numberOfOpenSessions;

    public InstrumentType InstrumentType => Header.InstrumentType;

    ITimeSeriesFileHeader ITimeSeriesFile.Header => Header;

    ITimeSeriesSession? ITimeSeriesFile.GetWriterSession() => GetWriterSession();

    ITimeSeriesSession ITimeSeriesFile.GetReaderSession() => GetReaderSession();
    ITimeSeriesSession ITimeSeriesFile.GetInfoSession()   => GetInfoSession();

    public string SourceName
    {
        get => Header.SourceName!;
        set => Header.SourceName = value;
    }

    public Type EntryType => typeof(TEntry);

    public string InstrumentName
    {
        get => Header.InstrumentName!;
        set => Header.InstrumentName = value;
    }

    public string Category
    {
        get => Header.Category!;
        set => Header.Category = value;
    }

    public bool Intersects(UnboundedTimeRange? periodRange = null) => TimeSeriesPeriodRange.Intersects(periodRange);

    public string FileName { get; }
    public bool   IsOpen   => Header.FileIsOpen || numberOfOpenSessions > 0;

    public void Close()
    {
        if (!IsOpen) return;
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

    public virtual ISessionAppendContext<TEntry, TBucket> CreateAppendContext() => new AppendContext<TEntry, TBucket>();

    public static TFile OpenExistingTimeSeriesFile(string filePath)
    {
        var timeSeriesMemoryMappedFile = new PagedMemoryMappedFile(filePath);
        var headerFileView             = timeSeriesMemoryMappedFile.CreateShiftableMemoryMappedFileView("header");
        var ptr                        = headerFileView.StartAddress;
        var fileVersion                = StreamByteOps.ToUShort(ref ptr);
        if (fileVersion is not (0 or 1)) // select appropriate header file based on file version
            throw new Exception("Unsupported file version being loaded");
        var header   = new TimeSeriesFileHeaderFromV1(headerFileView, true);
        var fileType = header.TimeSeriesFileType;
        var fileTypeOpenExistingCtor =
            ReflectionHelper.CtorDerivedBinder<PagedMemoryMappedFile, IMutableTimeSeriesFileHeader, TFile>(fileType);
        if (fileTypeOpenExistingCtor == null)
            throw new Exception($"Could not create constructor for {fileType} with " +
                                $"parameters PagedMemoryMappedFile, IMutableTimeSeriesFileHeader");
        var existingFile = fileTypeOpenExistingCtor(timeSeriesMemoryMappedFile, header);
        return existingFile;
    }

    public int IncrementSessionCount() => Interlocked.Increment(ref numberOfOpenSessions);

    public int DecrementSessionCount()
    {
        var remainingSessions = Interlocked.Decrement(ref numberOfOpenSessions);
        if (AutoCloseOnZeroSessions && remainingSessions <= 0) Close();
        return remainingSessions;
    }
}
