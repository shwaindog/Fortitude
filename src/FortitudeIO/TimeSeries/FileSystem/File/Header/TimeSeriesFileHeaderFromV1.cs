#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Header;

public interface ITimeSeriesFileHeader : IDisposable
{
    bool FileIsOpen { get; }
    bool HasSubHeader { get; }
    ushort HeaderVersion { get; }
    FileFlags FileFlags { get; }
    TimeSeriesEntryType TimeSeriesEntryType { get; }
    bool WriterOpen { get; }
    bool HasInternalIndex { get; }
    bool HasExternalBucketIndex { get; }
    uint FileHeaderSize { get; }
    long EndOfHeaderFileOffset { get; }
    uint InternalIndexMaxSize { get; }
    ulong FileSize { get; }
    DateTime FileStartPeriod { get; }
    TimeSeriesPeriod FilePeriod { get; }
    TimeSeriesPeriod BucketPeriod { get; }
    TimeSeriesPeriod EntriesPeriod { get; }
    TimeSeriesPeriod SubBucketPeriods { get; }
    TimeSeriesPeriod SummariesPeriods { get; }
    uint HighestBucketId { get; }
    uint Buckets { get; }
    uint LastWriterBucket { get; }
    FileOperation LastWriterOperation { get; }
    ulong LastWriterLastWriteOffset { get; }
    DateTime LastWriterWriteTime { get; }
    uint TotalEntries { get; }
    ulong TotalFileDataSizeBytes { get; }
    uint TotalHeaderSizeBytes { get; }
    uint TotalFileIndexSizeBytes { get; }
    ushort MaxHeaderTypeTextSizeBytes { get; }
    ushort MaxHeaderTextSizeBytes { get; }
    Type TimeSeriesFileType { get; }
    Type EntryType { get; }
    Type BucketType { get; }
    string? InstrumentName { get; }
    string? Category { get; }
    string? SourceName { get; }
    string? OriginSourceText { get; }
    string? ExternalIndexFileRelativePath { get; }
    string? AnnotationFileRelativePath { get; }
    IFileSubHeader? SubHeader { get; }
    bool BucketsHaveChanged { get; }
    void CloseFileView();
    bool ReopenFileView(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None);
    IReadonlyBucketIndexDictionary BucketIndexes { get; }
    IEnumerable<BucketIndexInfo> EarliestOrderedBuckets { get; }
    Func<ShiftableMemoryMappedFileView, ushort, bool, IFileSubHeader>? SubHeaderFactory { get; set; }
}


public unsafe interface IMutableTimeSeriesFileHeader : ITimeSeriesFileHeader
{
    new ushort HeaderVersion { get; set; }
    new FileFlags FileFlags { get; set; }
    new TimeSeriesEntryType TimeSeriesEntryType { get; set; }
    new uint InternalIndexMaxSize { get; set; }
    new uint FileHeaderSize { get; set; }
    new ulong FileSize { get; set; }
    new DateTime FileStartPeriod { get; set; }
    new TimeSeriesPeriod FilePeriod { get; set; }
    new TimeSeriesPeriod BucketPeriod { get; set; }
    new TimeSeriesPeriod EntriesPeriod { get; set; }
    new TimeSeriesPeriod SubBucketPeriods { get; set; }
    new TimeSeriesPeriod SummariesPeriods { get; set; }
    new uint HighestBucketId { get; set; }
    new uint Buckets { get; set; }
    new uint LastWriterBucket { get; set; }
    new FileOperation LastWriterOperation { get; set; }
    new ulong LastWriterLastWriteOffset { get; set; }
    new DateTime LastWriterWriteTime { get; set; }
    new uint TotalEntries { get; set; }
    new ulong TotalFileDataSizeBytes { get; set; }
    new uint TotalFileIndexSizeBytes { get; set; }
    new uint TotalHeaderSizeBytes { get; set; }
    new ushort MaxHeaderTypeTextSizeBytes { get; set; }
    new ushort MaxHeaderTextSizeBytes { get; set; }
    new Type TimeSeriesFileType { get; set; }
    new Type EntryType { get; set; }
    new Type BucketType { get; set; }
    new string? InstrumentName { get; set; }
    new string? Category { get; set; }
    new string? SourceName { get; set; }
    new string? OriginSourceText { get; set; }
    new string? ExternalIndexFileRelativePath { get; set; }
    new string? AnnotationFileRelativePath { get; set; }
    int RemainingFirstBucketPadding { get; set; }
    new IBucketIndexDictionary BucketIndexes { get; }
    ushort SubHeaderMaxSize { get; }
    byte* SubHeaderPointer { get; }
}

public enum FileOperation : byte
{
    CreateFile = 0
    , CreateBucket
    , AppendingToHighestBucket
    , EditingBucket
    , FinishBucket
    , ClosingFile
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.None, Pack = 1, Size = 1)]
public struct FileStartVersionHeader
{
    public ushort HeaderVersion;
}

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct TimeSeriesFileHeaderBodyV1
{
    public const ushort SerializationParameterDataMaxSize = 1024;
    public ulong FileSize;
    public long FileStartPeriod;
    public ulong LastWriterLastWriteOffset;
    public long LastWriterWriteTime;
    public ulong TotalFileDataSizeBytes;
    public uint FileHeaderSize;
    public uint InternalIndexMaxSize;
    public uint HighestBucketId;
    public uint Buckets;
    public uint LastWriterBucket;
    public uint TotalEntries;
    public uint TotalHeaderSizeBytes;
    public uint TotalFileIndexSizeBytes;
    public uint FirstBucketFileStartOffset;
    public TimeSeriesEntryType TimeSeriesEntryTypeEnum;
    public ushort MaxHeaderTypeTextSizeBytes;                   // Default 1023 bytes
    public ushort MaxHeaderTextSizeBytes;                       // Default 255 bytes
    public ushort TimeSeriesFileTypeTextFileStartOffset;        // 1 TypeString
    public ushort EntryTypeTextFileStartOffset;                 // 2 TypeString
    public ushort BucketTypeTextFileStartOffset;                // 3 TypeString
    public ushort InstrumentNameTextFileStartOffset;            // 1 HeaderString
    public ushort SourceNameTextFileStartOffset;                // 2 HeaderString
    public ushort CategoryTextFileStartOffset;                  // 3 HeaderString
    public ushort OriginSourceTextFileStartOffset;              // 4 HeaderString
    public ushort ExternalIndexFileRelativePathFileStartOffset; // 5 HeaderString
    public ushort AnnotationFileRelativePathFileStartOffset;    // 6 HeaderString
    public ushort InternalIndexFileStartOffset;
    public TimeSeriesPeriod FilePeriod;
    public TimeSeriesPeriod BucketPeriod;
    public TimeSeriesPeriod EntriesPeriod;
    public TimeSeriesPeriod SubBucketPeriods;
    public TimeSeriesPeriod SummariesPeriods;
    public FileFlags FileFlags;
    public FileOperation LastWriterOperation;
}

public unsafe class TimeSeriesFileHeaderFromV1 : IMutableTimeSeriesFileHeader
{
    public const int SubHeaderReservedSpaceSizeBytes = 1024;
    public const ushort NewFileDefaultVersion = 1;
    public static readonly ushort[] SupportedFileVersions = [1];
    private ShiftableMemoryMappedFileView? headerMemoryMappedFileView;
    private readonly ushort stringSizeHeaderSize;
    private readonly ushort typeStringSizeHeaderSize;
    private bool isWritable;
    private const int HeaderStringCount = 6;
    private const int HeaderTypeStringCount = 3;

    private ushort headerVersion;

    private TimeSeriesFileHeaderBodyV1* writableV1HeaderBody;
    private TimeSeriesFileHeaderBodyV1 cacheV1HeaderBody;
    private string? timeSeriesFileTypeString;
    private string? entryTypeString;
    private string? bucketTypeString;
    private string? instrumentNameString;
    private string? categoryString;
    private string? sourceNameString;
    private string? originSourceTextString;
    private string? externalIndexFileRelativePathString;
    private string? annotationFileRelativePathString;
    private BucketIndexDictionary? internalWritableIndexDictionary;
    private List<BucketIndexInfo>? cacheSortedBucketIndexOffsets;
    private BucketIndexEarliestEntryComparer? bucketIndexEarliestEntryComparer;
    private IFileSubHeader? subHeader;

    public TimeSeriesFileHeaderFromV1(ShiftableMemoryMappedFileView memoryMappedFileView, 
        Type fileType, Type bucketType, Type entryType, 
        CreateFileParameters createFileParameters)
    {
        headerMemoryMappedFileView = memoryMappedFileView;
        isWritable = true;
        HeaderVersion = NewFileDefaultVersion;
        writableV1HeaderBody = (TimeSeriesFileHeaderBodyV1*)(memoryMappedFileView.StartAddress + 2);
        FileFlags = FileFlags.WriterOpened | createFileParameters.InitialFileFlags;
        InternalIndexMaxSize = createFileParameters.InternalIndexSize;
        MaxHeaderTypeTextSizeBytes = createFileParameters.MaxTypeStringSizeBytes;
        MaxHeaderTextSizeBytes = createFileParameters.MaxStringSizeBytes;
        // ReSharper disable once VirtualMemberCallInConstructor
        FileHeaderSize = (uint)(EndOfHeaderSectionFileOffset - 2);
        if(FileFlags.HasInternalIndexInHeaderFlag() && InternalIndexMaxSize > 0)
        {
            writableV1HeaderBody->InternalIndexFileStartOffset = StartOfIndexFileOffset;
        }
        typeStringSizeHeaderSize = StreamByteOps.StringAutoHeaderSize(MaxHeaderTypeTextSizeBytes);
        writableV1HeaderBody->TimeSeriesFileTypeTextFileStartOffset = CalculateTypeStringStart(0);
        writableV1HeaderBody->EntryTypeTextFileStartOffset = CalculateTypeStringStart(1);
        writableV1HeaderBody->BucketTypeTextFileStartOffset = CalculateTypeStringStart(2);
        TimeSeriesFileType = fileType;
        BucketType = bucketType;
        EntryType = entryType;
        
        stringSizeHeaderSize = StreamByteOps.StringAutoHeaderSize(MaxHeaderTextSizeBytes);
        writableV1HeaderBody->InstrumentNameTextFileStartOffset = CalculateStringStart(0);
        writableV1HeaderBody->SourceNameTextFileStartOffset = CalculateStringStart(1);
        writableV1HeaderBody->CategoryTextFileStartOffset = CalculateStringStart(2);
        writableV1HeaderBody->OriginSourceTextFileStartOffset = CalculateStringStart(3);
        writableV1HeaderBody->ExternalIndexFileRelativePathFileStartOffset = CalculateStringStart(4);
        writableV1HeaderBody->AnnotationFileRelativePathFileStartOffset = CalculateStringStart(5);
        InstrumentName = createFileParameters.InstrumentName;
        SourceName = createFileParameters.SourceName;
        Category = createFileParameters.Category;
        OriginSourceText = createFileParameters.OriginSourceText;
        ExternalIndexFileRelativePath = createFileParameters.ExternalIndexFileRelativePath;
        AnnotationFileRelativePath = createFileParameters.AnnotationFileRelativePath;

        FilePeriod = createFileParameters.FilePeriod;
        FileStartPeriod = createFileParameters.FilePeriod.ContainingPeriodBoundaryStart(createFileParameters.FileStartPeriod);
        TimeSeriesEntryType = createFileParameters.TimeSeriesEntryType;

        TotalHeaderSizeBytes = FileHeaderSize + 2;
        cacheV1HeaderBody = *writableV1HeaderBody;
    }

    private ushort CalculateTypeStringStart(byte position)
    {
        var firstStringStart = StartOfTypeStringStorageFileOffset;
        return (ushort)(firstStringStart + StringSizeBytesStorage(MaxHeaderTypeTextSizeBytes) * position);
    }

    private ushort CalculateStringStart(byte position)
    {
        var firstStringStart = StartOfStringStorageFileOffset;
        return (ushort)(firstStringStart + StringSizeBytesStorage(MaxHeaderTextSizeBytes) * position);
    }

    public TimeSeriesFileHeaderFromV1(ShiftableMemoryMappedFileView memoryMappedFileView, bool writable)
    {
        headerMemoryMappedFileView = memoryMappedFileView;
        var ptr = headerMemoryMappedFileView.StartAddress;
        headerVersion = StreamByteOps.ToUShort(ref ptr);
        isWritable = writable;
        if (!SupportedFileVersions.Contains(headerVersion)) throw new ArgumentException($"File version {headerVersion} is not supported");
        var readFileFlags = (FileFlags)StreamByteOps.ToUShort(ref ptr);
        writableV1HeaderBody = (TimeSeriesFileHeaderBodyV1*)(memoryMappedFileView.StartAddress + 2);
        writableV1HeaderBody->FileFlags = readFileFlags | (isWritable ? FileFlags.WriterOpened : FileFlags.None) ;
        
        typeStringSizeHeaderSize = StreamByteOps.StringAutoHeaderSize(MaxHeaderTypeTextSizeBytes);
        stringSizeHeaderSize = StreamByteOps.StringAutoHeaderSize(MaxHeaderTextSizeBytes);
        cacheV1HeaderBody = *writableV1HeaderBody;
    }

    public static TimeSeriesFileHeaderFromV1 NewFileCreateHeader<TFile, TBucket, TEntry>(TimeSeriesFile<TFile, TBucket, TEntry> timeSeriesFile,
        ShiftableMemoryMappedFileView memoryMappedFileView, 
        CreateFileParameters createFileParameters)
        where TFile : TimeSeriesFile<TFile, TBucket, TEntry>
        where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry> 
        where TEntry : ITimeSeriesEntry<TEntry>
    {
        return new TimeSeriesFileHeaderFromV1(memoryMappedFileView, typeof(TFile), typeof(TBucket), typeof(TEntry), createFileParameters);
    }

    public ushort EndOfHeaderBodyFileOffset  => (ushort)(2 + sizeof(TimeSeriesFileHeaderBodyV1));
    public ushort StartOfSubHeaderFileOffset => (ushort)(EndOfHeaderBodyFileOffset);
    public ushort EndOfSubHeaderFileOffset   => (ushort)(EndOfHeaderBodyFileOffset + SubHeaderReservedSpaceSizeBytes);

    public ushort StartOfTypeStringStorageFileOffset => EndOfSubHeaderFileOffset;
    public ushort EndOfTypeStringStorageFileOffset =>  (ushort)(StartOfTypeStringStorageFileOffset + 
                                                                StringSizeBytesStorage(MaxHeaderTypeTextSizeBytes)*HeaderTypeStringCount);
    public ushort StartOfStringStorageFileOffset => EndOfTypeStringStorageFileOffset;
    public ushort EndOfStringValuesFileOffset => (ushort)(StartOfStringStorageFileOffset + 
                                                          StringSizeBytesStorage(MaxHeaderTextSizeBytes)*HeaderStringCount);
    public ushort StartOfIndexFileOffset => EndOfStringValuesFileOffset;
    public virtual long EndOfHeaderSectionFileOffset => StartOfIndexFileOffset + 
          BucketIndexDictionary.CalculateDictionarySizeInBytes(InternalIndexMaxSize, EndOfStringValuesFileOffset);

    public bool FileIsOpen => headerMemoryMappedFileView != null;

    public void Dispose()
    {
        isWritable = false;
        headerMemoryMappedFileView?.Dispose();
        headerMemoryMappedFileView = null;
    }

    public ushort HeaderVersion
    {
        get => headerVersion;
        set
        {
            if (headerVersion == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress;
                StreamByteOps.ToBytes(ref ptr, value);
            }
            headerVersion = value;
        }
    }

    public FileFlags FileFlags
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.FileFlags;
            cacheV1HeaderBody.FileFlags = writableV1HeaderBody->FileFlags;
            return cacheV1HeaderBody.FileFlags;
        }
        set
        {
            if (cacheV1HeaderBody.FileFlags == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->FileFlags = value;
                cacheV1HeaderBody.FileFlags = value;
            }
        }
    }

    public ushort SubHeaderMaxSize => SubHeaderReservedSpaceSizeBytes;
    public byte* SubHeaderPointer => FileIsOpen 
        ? headerMemoryMappedFileView!.StartAddress + EndOfHeaderBodyFileOffset
        : null;

    public bool WriterOpen => FileFlags.HasWriterOpenedFlag();
    public bool HasSubHeader => FileFlags.HasSubFileHeaderFileFlag();
    public bool HasInternalIndex => FileFlags.HasInternalIndexInHeaderFlag() && InternalIndexMaxSize > 0;
    public bool HasExternalBucketIndex => FileFlags.HasExternalIndexFileFlag();

    public TimeSeriesEntryType TimeSeriesEntryType 
    {
        get => cacheV1HeaderBody.TimeSeriesEntryTypeEnum;
        set
        {
            if (cacheV1HeaderBody.TimeSeriesEntryTypeEnum == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TimeSeriesEntryTypeEnum = value;
                cacheV1HeaderBody.TimeSeriesEntryTypeEnum = value;
            }
        }
    }

    public uint FileHeaderSize
    {
        get => cacheV1HeaderBody.FileHeaderSize;
        set
        {
            if (cacheV1HeaderBody.FileHeaderSize == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->FileHeaderSize = value;
                cacheV1HeaderBody.FileHeaderSize = value;
            }
        }
    }

    public long EndOfHeaderFileOffset => FileHeaderSize + 2;

    public uint InternalIndexMaxSize
    {
        get => cacheV1HeaderBody.InternalIndexMaxSize;
        set
        {
            if (cacheV1HeaderBody.InternalIndexMaxSize == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->InternalIndexMaxSize = value;
                cacheV1HeaderBody.InternalIndexMaxSize = value;
            }
        }
    }

    public ulong FileSize
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.FileSize;
            cacheV1HeaderBody.FileSize = writableV1HeaderBody->FileSize;
            return cacheV1HeaderBody.FileSize;
        }
        set
        {
            if (cacheV1HeaderBody.FileSize == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->FileSize = value;
                cacheV1HeaderBody.FileSize = value;
            }
        }
    }

    public DateTime FileStartPeriod
    {
        get
        {
            if (headerMemoryMappedFileView == null) return DateTime.FromBinary(cacheV1HeaderBody.FileStartPeriod);
            cacheV1HeaderBody.FileStartPeriod = writableV1HeaderBody->FileStartPeriod;
            return DateTime.FromBinary(cacheV1HeaderBody.FileStartPeriod);
        }
        set
        {
            if (DateTime.FromBinary(cacheV1HeaderBody.FileStartPeriod) == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->FileStartPeriod = value.Ticks;
                cacheV1HeaderBody.FileStartPeriod = writableV1HeaderBody->LastWriterWriteTime;
            }
        }
    }

    public TimeSeriesPeriod FilePeriod 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.FilePeriod;
            cacheV1HeaderBody.FilePeriod = writableV1HeaderBody->FilePeriod;
            return cacheV1HeaderBody.FilePeriod;
        }
        set
        {
            if (cacheV1HeaderBody.FilePeriod == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->FilePeriod = value;
                cacheV1HeaderBody.FilePeriod = value;
            }
        }
    }

    public TimeSeriesPeriod BucketPeriod 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.BucketPeriod;
            cacheV1HeaderBody.BucketPeriod = writableV1HeaderBody->BucketPeriod;
            return cacheV1HeaderBody.BucketPeriod;
        }
        set
        {
            if (cacheV1HeaderBody.BucketPeriod == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->BucketPeriod = value;
                cacheV1HeaderBody.BucketPeriod = value;
            }
        }
    }

    public TimeSeriesPeriod EntriesPeriod 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.EntriesPeriod;
            cacheV1HeaderBody.EntriesPeriod = writableV1HeaderBody->EntriesPeriod;
            return cacheV1HeaderBody.EntriesPeriod;
        }
        set
        {
            if (cacheV1HeaderBody.EntriesPeriod == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->EntriesPeriod = value;
                cacheV1HeaderBody.EntriesPeriod = value;
            }
        }
    }

    public TimeSeriesPeriod SubBucketPeriods 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.SubBucketPeriods;
            cacheV1HeaderBody.SubBucketPeriods = writableV1HeaderBody->SubBucketPeriods;
            return cacheV1HeaderBody.SubBucketPeriods;
        }
        set
        {
            if (cacheV1HeaderBody.SubBucketPeriods == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->SubBucketPeriods = value;
                cacheV1HeaderBody.SubBucketPeriods = value;
            }
        }
    }

    public TimeSeriesPeriod SummariesPeriods 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.SummariesPeriods;
            cacheV1HeaderBody.SummariesPeriods = writableV1HeaderBody->SummariesPeriods;
            return cacheV1HeaderBody.SummariesPeriods;
        }
        set
        {
            if (cacheV1HeaderBody.SummariesPeriods == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->SummariesPeriods = value;
                cacheV1HeaderBody.SummariesPeriods = value;
            }
        }
    }

    public uint HighestBucketId
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.HighestBucketId;
            cacheV1HeaderBody.HighestBucketId = writableV1HeaderBody->HighestBucketId;
            return cacheV1HeaderBody.HighestBucketId;
        }
        set
        {
            if (cacheV1HeaderBody.HighestBucketId == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->HighestBucketId = value;
                cacheV1HeaderBody.HighestBucketId = value;
            }
        }
    }

    public uint Buckets
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.Buckets;
            cacheV1HeaderBody.Buckets = writableV1HeaderBody->Buckets;
            return cacheV1HeaderBody.Buckets;
        }
        set
        {
            if (cacheV1HeaderBody.Buckets == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->Buckets = value;
                cacheV1HeaderBody.Buckets = value;
            }
        }
    }

    public uint LastWriterBucket
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.LastWriterBucket;
            cacheV1HeaderBody.LastWriterBucket = writableV1HeaderBody->LastWriterBucket;
            return writableV1HeaderBody->LastWriterBucket;
        }
        set
        {
            if (cacheV1HeaderBody.LastWriterBucket == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->LastWriterBucket = value;
                cacheV1HeaderBody.LastWriterBucket = value;
            }
        }
    }

    public FileOperation LastWriterOperation
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.LastWriterOperation;
            cacheV1HeaderBody.LastWriterOperation = writableV1HeaderBody->LastWriterOperation;
            return cacheV1HeaderBody.LastWriterOperation;
        }
        set
        {
            if (cacheV1HeaderBody.LastWriterOperation == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->LastWriterOperation = value;
                cacheV1HeaderBody.LastWriterOperation = value;
            }
        }
    }

    public ulong LastWriterLastWriteOffset
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.LastWriterLastWriteOffset;
            cacheV1HeaderBody.LastWriterLastWriteOffset = writableV1HeaderBody->LastWriterLastWriteOffset;
            return cacheV1HeaderBody.LastWriterLastWriteOffset;
        }
        set
        {
            if (cacheV1HeaderBody.LastWriterLastWriteOffset == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->LastWriterLastWriteOffset = value;
                cacheV1HeaderBody.LastWriterLastWriteOffset = value;
            }
        }
    }

    public DateTime LastWriterWriteTime
    {
        get
        {
            if (headerMemoryMappedFileView == null) return DateTime.FromBinary(cacheV1HeaderBody.LastWriterWriteTime);
            cacheV1HeaderBody.LastWriterWriteTime = writableV1HeaderBody->LastWriterWriteTime;
            return DateTime.FromBinary(cacheV1HeaderBody.LastWriterWriteTime);
        }
        set
        {
            if (DateTime.FromBinary(cacheV1HeaderBody.LastWriterWriteTime) == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->LastWriterWriteTime = value.Ticks;
                cacheV1HeaderBody.LastWriterWriteTime = writableV1HeaderBody->LastWriterWriteTime;
            }
        }
    }

    public uint TotalEntries 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.TotalEntries;
            cacheV1HeaderBody.TotalEntries = writableV1HeaderBody->TotalEntries;
            return cacheV1HeaderBody.TotalEntries;
        }
        set
        {
            if (cacheV1HeaderBody.TotalEntries == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TotalEntries = value;
                cacheV1HeaderBody.TotalEntries = value;
            }
        }
    }

    public ulong TotalFileDataSizeBytes 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.TotalFileDataSizeBytes;
            cacheV1HeaderBody.TotalFileDataSizeBytes = writableV1HeaderBody->TotalFileDataSizeBytes;
            return cacheV1HeaderBody.TotalFileDataSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.TotalFileDataSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TotalFileDataSizeBytes = value;
                cacheV1HeaderBody.TotalFileDataSizeBytes = value;
            }
        }
    }

    public uint TotalHeaderSizeBytes 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.TotalHeaderSizeBytes;
            cacheV1HeaderBody.TotalHeaderSizeBytes = writableV1HeaderBody->TotalHeaderSizeBytes;
            return cacheV1HeaderBody.TotalHeaderSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.TotalHeaderSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TotalHeaderSizeBytes = value;
                cacheV1HeaderBody.TotalHeaderSizeBytes = value;
            }
        }
    }

    public uint TotalFileIndexSizeBytes 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.TotalFileIndexSizeBytes;
            cacheV1HeaderBody.TotalFileIndexSizeBytes = writableV1HeaderBody->TotalFileIndexSizeBytes;
            return cacheV1HeaderBody.TotalFileIndexSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.TotalFileIndexSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TotalFileIndexSizeBytes = value;
                cacheV1HeaderBody.TotalFileIndexSizeBytes     = value;
            }
        }
    }

    public ushort InternalIndexFileStartOffset
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.InternalIndexFileStartOffset;
            cacheV1HeaderBody.InternalIndexFileStartOffset = writableV1HeaderBody->InternalIndexFileStartOffset;
            return cacheV1HeaderBody.InternalIndexFileStartOffset;
        }
        set
        {
            if (cacheV1HeaderBody.InternalIndexFileStartOffset == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->InternalIndexFileStartOffset = value;
                cacheV1HeaderBody.InternalIndexFileStartOffset = value;
            }
        }
    }

    public ushort MaxHeaderTypeTextSizeBytes
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.MaxHeaderTypeTextSizeBytes;
            cacheV1HeaderBody.MaxHeaderTypeTextSizeBytes = writableV1HeaderBody->MaxHeaderTypeTextSizeBytes;
            return cacheV1HeaderBody.MaxHeaderTypeTextSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.MaxHeaderTypeTextSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->MaxHeaderTypeTextSizeBytes = value;
                cacheV1HeaderBody.MaxHeaderTypeTextSizeBytes     = value;
            }
        }
    }

    public ushort MaxHeaderTextSizeBytes
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.MaxHeaderTextSizeBytes;
            cacheV1HeaderBody.MaxHeaderTextSizeBytes = writableV1HeaderBody->MaxHeaderTextSizeBytes;
            return cacheV1HeaderBody.MaxHeaderTextSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.MaxHeaderTextSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->MaxHeaderTextSizeBytes = value;
                cacheV1HeaderBody.MaxHeaderTextSizeBytes     = value;
            }
        }
    }

    public Type TimeSeriesFileType
    {
        get
        {
            var typeNameString = TimeSeriesFileTypeString;
            return Type.GetType(typeNameString)!;
        }
        set
        {
            if (timeSeriesFileTypeString == value.FullName || headerMemoryMappedFileView == null) return;
            TimeSeriesFileTypeString = value.AssemblyQualifiedName!;
        }
    }

    private string TimeSeriesFileTypeString
    {
        get
        {
            if (timeSeriesFileTypeString != null) return timeSeriesFileTypeString;
            if (writableV1HeaderBody->TimeSeriesFileTypeTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->TimeSeriesFileTypeTextFileStartOffset;
                timeSeriesFileTypeString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTypeTextSizeBytes + typeStringSizeHeaderSize);
            }
            return timeSeriesFileTypeString!;
        }
        set
        {
            if (timeSeriesFileTypeString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->TimeSeriesFileTypeTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTypeTextSizeBytes + typeStringSizeHeaderSize);
            }
            timeSeriesFileTypeString = null;
            timeSeriesFileTypeString = TimeSeriesFileTypeString;
        }
    }

    public Type EntryType
    {
        get
        {
            var typeNameString = EntryTypeString;
            return Type.GetType(typeNameString)!;
        }
        set
        {
            if (entryTypeString == value.FullName || headerMemoryMappedFileView == null) return;
            EntryTypeString = value.AssemblyQualifiedName!;
        }
    }

    private string EntryTypeString
    {
        get
        {
            if (entryTypeString != null) return entryTypeString;
            if (writableV1HeaderBody->BucketTypeTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->EntryTypeTextFileStartOffset;
                entryTypeString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTypeTextSizeBytes + typeStringSizeHeaderSize);
            }
            return entryTypeString!;
        }
        set
        {
            if (entryTypeString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->EntryTypeTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTypeTextSizeBytes + typeStringSizeHeaderSize);
            }
            entryTypeString = null;
            entryTypeString = EntryTypeString;
        }
    }

    public Type BucketType
    {
        get
        {
            var typeNameString = BucketTypeString;
            return Type.GetType(typeNameString)!;
        }
        set
        {
            if (bucketTypeString == value.FullName || headerMemoryMappedFileView == null) return;
            BucketTypeString = value.AssemblyQualifiedName!;
        }
    }

    private string BucketTypeString
    {
        get
        {
            if (bucketTypeString != null) return bucketTypeString;
            if (writableV1HeaderBody->BucketTypeTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->BucketTypeTextFileStartOffset;
                bucketTypeString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTypeTextSizeBytes + typeStringSizeHeaderSize);
            }
            return bucketTypeString!;
        }
        set
        {
            if (bucketTypeString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->BucketTypeTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTypeTextSizeBytes + typeStringSizeHeaderSize);
            }
            bucketTypeString = null;
            bucketTypeString = BucketTypeString;
        }
    }

    public string? InstrumentName 
    {
        get
        {
            if (instrumentNameString != null) return instrumentNameString;
            if (writableV1HeaderBody->InstrumentNameTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->InstrumentNameTextFileStartOffset;
                instrumentNameString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            return instrumentNameString;
        }
        set
        {
            if (instrumentNameString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->InstrumentNameTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            instrumentNameString = null;
            instrumentNameString = InstrumentName;
        }
    }

    public string? Category 
    {
        get
        {
            if (categoryString != null) return categoryString;
            if (writableV1HeaderBody->CategoryTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->CategoryTextFileStartOffset;
                categoryString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            return categoryString;
        }
        set
        {
            if (categoryString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->CategoryTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            categoryString = null;
            categoryString = Category;
        }
    }

    public string? SourceName 
    {
        get
        {
            if (sourceNameString != null) return sourceNameString;
            if (writableV1HeaderBody->SourceNameTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->SourceNameTextFileStartOffset;
                sourceNameString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            return sourceNameString;
        }
        set
        {
            if (sourceNameString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->SourceNameTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            sourceNameString = null;
            sourceNameString = SourceName;
        }
    }

    public string? OriginSourceText 
    {
        get
        {
            if (originSourceTextString != null) return originSourceTextString;
            if (writableV1HeaderBody->OriginSourceTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->OriginSourceTextFileStartOffset;
                originSourceTextString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            return originSourceTextString;
        }
        set
        {
            if (originSourceTextString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->OriginSourceTextFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            originSourceTextString = null;
            originSourceTextString = OriginSourceText;
        }
    }

    public string? ExternalIndexFileRelativePath 
    {
        get
        {
            if (externalIndexFileRelativePathString != null) return externalIndexFileRelativePathString;
            if (writableV1HeaderBody->ExternalIndexFileRelativePathFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->ExternalIndexFileRelativePathFileStartOffset;
                externalIndexFileRelativePathString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            return externalIndexFileRelativePathString;
        }
        set
        {
            if (externalIndexFileRelativePathString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->ExternalIndexFileRelativePathFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            externalIndexFileRelativePathString = null;
            externalIndexFileRelativePathString = ExternalIndexFileRelativePath;
        }
    }

    public string? AnnotationFileRelativePath 
    {
        get
        {
            if (annotationFileRelativePathString != null) return annotationFileRelativePathString;
            if (writableV1HeaderBody->AnnotationFileRelativePathFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->AnnotationFileRelativePathFileStartOffset;
                annotationFileRelativePathString = StreamByteOps.ToStringWithAutoSizeHeader(ref ptr, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            return annotationFileRelativePathString;
        }
        set
        {
            if (annotationFileRelativePathString == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                var ptr = headerMemoryMappedFileView.StartAddress + writableV1HeaderBody->AnnotationFileRelativePathFileStartOffset;
                StreamByteOps.ToBytesWithAutoSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes + stringSizeHeaderSize);
            }
            annotationFileRelativePathString = null;
            annotationFileRelativePathString = AnnotationFileRelativePath;
        }
    }

    public IFileSubHeader? SubHeader
    {
        get
        {
            if (headerMemoryMappedFileView != null && subHeader == null && FileFlags.HasSubFileHeaderFileFlag() && SubHeaderFactory != null)
            {
                subHeader = SubHeaderFactory(headerMemoryMappedFileView!, StartOfSubHeaderFileOffset, isWritable);
            }
            return subHeader;
        }
        protected set => subHeader = value;
    }

    public Func<ShiftableMemoryMappedFileView, ushort, bool, IFileSubHeader>? SubHeaderFactory { get; set; }

    public bool BucketsHaveChanged => writableV1HeaderBody->Buckets != cacheV1HeaderBody.Buckets;


    public IBucketIndexDictionary BucketIndexes
    {
        get
        {
            if (headerMemoryMappedFileView == null) throw new Exception("File is closed");
            if (!HasInternalIndex && !HasExternalBucketIndex) throw new Exception("No indexes defined for file");

            // Todo add external indexes later

            internalWritableIndexDictionary
                ??= new BucketIndexDictionary(headerMemoryMappedFileView, InternalIndexFileStartOffset, InternalIndexMaxSize, false);
            return internalWritableIndexDictionary;
        }
    }

    IReadonlyBucketIndexDictionary ITimeSeriesFileHeader.BucketIndexes => BucketIndexes;

    public IEnumerable<BucketIndexInfo> EarliestOrderedBuckets
    {
        get
        {
            cacheSortedBucketIndexOffsets ??= new List<BucketIndexInfo>();
            if (cacheSortedBucketIndexOffsets.Any() && !WriterOpen) return cacheSortedBucketIndexOffsets;
            bucketIndexEarliestEntryComparer ??= new BucketIndexEarliestEntryComparer();
            var bucketIndexes = BucketIndexes;
            cacheSortedBucketIndexOffsets.AddRange(bucketIndexes.Values);
            cacheSortedBucketIndexOffsets.Sort(bucketIndexEarliestEntryComparer);
            return cacheSortedBucketIndexOffsets;
        }
    }

    public int RemainingFirstBucketPadding { get; set; }

    public void CloseFileView()
    {
        headerMemoryMappedFileView?.FlushCursorDataToDisk(0, (int)FileHeaderSize + 2);
        if (HasSubHeader && SubHeader != null)
        {
            SubHeader.CloseFileView();
        }
        headerMemoryMappedFileView?.Dispose();
        headerMemoryMappedFileView = null;
    }

    public bool ReopenFileView(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None)
    {
        if (FileIsOpen) return true;
        headerMemoryMappedFileView = memoryMappedFileView;
        isWritable = fileFlags.HasWriterOpenedFlag();
        internalWritableIndexDictionary?.OpenWithFileView(memoryMappedFileView, !isWritable);
        writableV1HeaderBody = (TimeSeriesFileHeaderBodyV1*)(memoryMappedFileView.StartAddress + 2);
        writableV1HeaderBody->FileFlags = FileFlags | fileFlags.Unset(FileFlags.HasInternalIndexInHeader);
        if (HasSubHeader && SubHeader != null)
        {
            SubHeader.ReopenFileView(memoryMappedFileView, fileFlags);
        }

        cacheV1HeaderBody = *writableV1HeaderBody;
        return true;
    }

    private static ushort StringSizeBytesStorage(ushort maxStringBytes) => (ushort)(maxStringBytes + StreamByteOps.StringAutoHeaderSize(maxStringBytes));
}