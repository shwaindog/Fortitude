#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Header;

public interface ITimeSeriesFileHeader : IDisposable
{
    bool FileIsOpen { get; }
    ushort HeaderVersion { get; }
    FileFlags FileFlags { get; }
    ushort FileTypeEnum { get; }
    Type? BucketType { get; }
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
    ushort MaxHeaderTextSizeBytes { get; }
    uint HighestBucketId { get; }
    uint Buckets { get; }
    uint LastWriterBucket { get; }
    FileOperation LastWriterOperation { get; }
    ulong LastWriterLastWriteOffset { get; }
    DateTime LastWriterWriteTime { get; }
    uint TotalEntries { get; }
    ulong TotalDataSizeBytes { get; }
    uint TotalNonDataSizeBytes { get; }
    bool BucketsHaveChanged { get; }
    void CloseFileView();
    bool ReopenFileView(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None);

    IReadonlyBucketIndexDictionary BucketIndexes { get; }
    IEnumerable<BucketIndexInfo> EarliestOrderedBuckets { get; }
}


public interface IMutableTimeSeriesFileHeader : ITimeSeriesFileHeader
{
    new ushort HeaderVersion { get; set; }
    new FileFlags FileFlags { get; set; }
    new ushort FileTypeEnum { get; set; }
    new Type? BucketType { get; }
    new uint InternalIndexMaxSize { get; set; }
    new ushort MaxHeaderTextSizeBytes { get; set; }
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
    new ulong TotalDataSizeBytes { get; set; }
    new uint TotalNonDataSizeBytes { get; set; }
    int RemainingFirstBucketPadding { get; set; }
    new IBucketIndexDictionary BucketIndexes { get; }
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
    public ulong TotalDataSizeBytes;
    public uint FileHeaderSize;
    public uint InternalIndexMaxSize;
    public uint HighestBucketId;
    public uint Buckets;
    public uint LastWriterBucket;
    public uint TotalEntries;
    public uint TotalNonDataSizeBytes;
    public uint FirstBucketFileStartOffset;
    public ushort FileTypeEnum;
    public ushort MaxHeaderTextSizeBytes;
    public ushort BucketTypeTextFileStartOffset;
    public ushort OriginalSourceTextFileStartOffset;
    public ushort ExternalIndexFileNameFileStartOffset;
    public ushort AnnotationFileNameFileStartOffset;
    public ushort InternalIndexFileStartOffset;
    public ushort SerializationParameterDataSize;
    public TimeSeriesPeriod FilePeriod;
    public TimeSeriesPeriod BucketPeriod;
    public TimeSeriesPeriod EntriesPeriod;
    public TimeSeriesPeriod SubBucketPeriods;
    public TimeSeriesPeriod SummariesPeriods;
    public FileFlags FileFlags;
    public FileOperation LastWriterOperation;
    public byte SerializationParameterData;
}


public unsafe class TimeSeriesFileHeaderFromV1 : IMutableTimeSeriesFileHeader
{
    public const int SerializerParameterSizeBytes = 1024;
    public const ushort NewFileDefaultVersion = 1;
    public static readonly ushort[] SupportedFileVersions = [1];
    private ShiftableMemoryMappedFileView? headerMemoryMappedFileView;
    private bool isWritable;

    private ushort headerVersion;

    private TimeSeriesFileHeaderBodyV1* writableV1HeaderBody;
    private TimeSeriesFileHeaderBodyV1 cacheV1HeaderBody;
    private string? bucketTypeString;
    private BucketIndexDictionary? internalWritableIndexDictionary;
    private List<BucketIndexInfo>? cacheSortedBucketIndexOffsets;
    private BucketIndexEarliestEntryComparer? bucketIndexEarliestEntryComparer;

    public TimeSeriesFileHeaderFromV1(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None,
        uint internalIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        headerMemoryMappedFileView = memoryMappedFileView;
        isWritable = true;
        HeaderVersion = NewFileDefaultVersion;
        writableV1HeaderBody = (TimeSeriesFileHeaderBodyV1*)(memoryMappedFileView.LowerHalfViewVirtualMemoryAddress + 2);
        writableV1HeaderBody->FileFlags = FileFlags.WriterOpened | fileFlags;
        writableV1HeaderBody->InternalIndexMaxSize = internalIndexSize;
        writableV1HeaderBody->MaxHeaderTextSizeBytes = maxStringSizeBytes;
        writableV1HeaderBody->FileHeaderSize = CalculateHeaderSize(fileFlags, HeaderVersion, internalIndexSize, maxStringSizeBytes);
        if(fileFlags.HasInternalIndexInHeaderFlag() && internalIndexSize > 0)
        {
            writableV1HeaderBody->InternalIndexFileStartOffset = EndOfStringValuesFileOffset;
        }
        TotalNonDataSizeBytes = FileHeaderSize + 2;

        cacheV1HeaderBody = *writableV1HeaderBody;
    }

    public TimeSeriesFileHeaderFromV1(ShiftableMemoryMappedFileView memoryMappedFileView, bool writable)
    {
        headerMemoryMappedFileView = memoryMappedFileView;
        var ptr = headerMemoryMappedFileView.LowerHalfViewVirtualMemoryAddress;
        headerVersion = StreamByteOps.ToUShort(ref ptr);
        isWritable = writable;
        if (!SupportedFileVersions.Contains(headerVersion)) throw new ArgumentException($"File version {headerVersion} is not supported");
        var readFileFlags = (FileFlags)StreamByteOps.ToUShort(ref ptr);
        writableV1HeaderBody = (TimeSeriesFileHeaderBodyV1*)(memoryMappedFileView.LowerHalfViewVirtualMemoryAddress + 2);
        writableV1HeaderBody->FileFlags = readFileFlags | (isWritable ? FileFlags.WriterOpened : FileFlags.None) ;
        cacheV1HeaderBody = *writableV1HeaderBody;
    }

    public static TimeSeriesFileHeaderFromV1 OpenExistFileHeader(ShiftableMemoryMappedFileView memoryMappedFileView, bool writable)
    {
        return new TimeSeriesFileHeaderFromV1(memoryMappedFileView, writable);
    }

    public static TimeSeriesFileHeaderFromV1 NewFileCreateHeader(ShiftableMemoryMappedFileView memoryMappedFileView, 
        FileFlags fileFlags = FileFlags.None, uint internalIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        return new TimeSeriesFileHeaderFromV1(memoryMappedFileView, fileFlags, internalIndexSize, maxStringSizeBytes);
    }

    public ushort EndOfHeaderBodyFileOffset => (ushort)(2 + sizeof(TimeSeriesFileHeaderBodyV1));
    public ushort EndOfSerializerParametersFileOffset => (ushort)(EndOfHeaderBodyFileOffset + SerializerParameterSizeBytes);
    public ushort EndOfStringValuesFileOffset => (ushort)(EndOfSerializerParametersFileOffset + (MaxHeaderTextSizeBytes + 3)*4);

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
                var ptr = headerMemoryMappedFileView.LowerHalfViewVirtualMemoryAddress;
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
    public bool WriterOpen => FileFlags.HasWriterOpenedFlag();
    public bool HasInternalIndex => FileFlags.HasInternalIndexInHeaderFlag() && InternalIndexMaxSize > 0;
    public bool HasExternalBucketIndex => FileFlags.HasExternalIndexFileFlag();

    public ushort FileTypeEnum { get; set; }

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

    public ushort MaxHeaderTextSizeBytes
    {
        get => cacheV1HeaderBody.MaxHeaderTextSizeBytes;
        set
        {
            if (cacheV1HeaderBody.MaxHeaderTextSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->MaxHeaderTextSizeBytes = value;
                cacheV1HeaderBody.MaxHeaderTextSizeBytes = value;
            }
        }
    }

    public Type? BucketType
    {
        get
        {
            var typeNameString = BucketTypeString;
            return typeNameString == null ? null : Type.GetType(typeNameString);
        }
        set
        {
            if (bucketTypeString == value?.FullName || headerMemoryMappedFileView == null) return;
            BucketTypeString = value?.FullName;
        }
    }

    private string? BucketTypeString
    {
        get
        {
            if (bucketTypeString != null) return bucketTypeString;
            if (writableV1HeaderBody->BucketTypeTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.LowerHalfViewVirtualMemoryAddress + writableV1HeaderBody->BucketTypeTextFileStartOffset;
                bucketTypeString = StreamByteOps.ToStringWithSizeHeader(ref ptr);
            }
            return bucketTypeString;
        }
        set
        {
            if (bucketTypeString == value || headerMemoryMappedFileView == null) return;
            if (isWritable && value != null)
            {
                var ptr = headerMemoryMappedFileView.LowerHalfViewVirtualMemoryAddress + EndOfSerializerParametersFileOffset;
                var bytes = StreamByteOps.ToBytesWithSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes);
                writableV1HeaderBody->BucketTypeTextFileStartOffset = EndOfSerializerParametersFileOffset;
                cacheV1HeaderBody.BucketTypeTextFileStartOffset = EndOfSerializerParametersFileOffset;
            }
            else if (value == null)
            {
                writableV1HeaderBody->BucketTypeTextFileStartOffset = 0;
                cacheV1HeaderBody.BucketTypeTextFileStartOffset = 0;

            }
            bucketTypeString = value;
        }
    }

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

    public ulong TotalDataSizeBytes 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.TotalDataSizeBytes;
            cacheV1HeaderBody.TotalDataSizeBytes = writableV1HeaderBody->TotalDataSizeBytes;
            return cacheV1HeaderBody.TotalDataSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.TotalDataSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TotalDataSizeBytes = value;
                cacheV1HeaderBody.TotalDataSizeBytes = value;
            }
        }
    }

    public uint TotalNonDataSizeBytes 
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.TotalNonDataSizeBytes;
            cacheV1HeaderBody.TotalNonDataSizeBytes = writableV1HeaderBody->TotalNonDataSizeBytes;
            return cacheV1HeaderBody.TotalNonDataSizeBytes;
        }
        set
        {
            if (cacheV1HeaderBody.TotalNonDataSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody->TotalNonDataSizeBytes = value;
                cacheV1HeaderBody.TotalNonDataSizeBytes = value;
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
            if (!cacheSortedBucketIndexOffsets.Any() && !WriterOpen) return cacheSortedBucketIndexOffsets;
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
        headerMemoryMappedFileView?.Dispose();
        headerMemoryMappedFileView = null;
    }

    public bool ReopenFileView(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None)
    {
        if (FileIsOpen) return true;
        headerMemoryMappedFileView = memoryMappedFileView;
        isWritable = fileFlags.HasWriterOpenedFlag();
        internalWritableIndexDictionary?.OpenWithFileView(memoryMappedFileView, !isWritable);
        writableV1HeaderBody = (TimeSeriesFileHeaderBodyV1*)(memoryMappedFileView.LowerHalfViewVirtualMemoryAddress + 2);
        writableV1HeaderBody->FileFlags = FileFlags | fileFlags.Unset(FileFlags.HasInternalIndexInHeader);

        cacheV1HeaderBody = *writableV1HeaderBody;
        return true;
    }

    public ushort NextStringOffset => (ushort)(MaxHeaderTextSizeBytes + 3);

    public static uint CalculateMinimumBodyReadSize(ushort version)
    {
        if (!SupportedFileVersions.Contains(version))
            throw new NotImplementedException($"Only supports version [{SupportedFileVersions.JoinToString()}] of files");

        var versionStructSizeBytes = (uint)sizeof(FileStartVersionHeader);
        var versionedHeaderBodySizeBytes = versionStructSizeBytes + (uint)sizeof(TimeSeriesFileHeaderBodyV1) + TimeSeriesFileHeaderBodyV1.SerializationParameterDataMaxSize - 1;
        return versionedHeaderBodySizeBytes;
    }

    public static uint CalculateHeaderSize(FileFlags flags, ushort version, uint internalFixedSizeIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        if (!SupportedFileVersions.Contains(version))
            throw new NotImplementedException($"Only supports version [{SupportedFileVersions.JoinToString()}] of files");

        var stringSizeBytes = (uint)maxStringSizeBytes + 3; // two bytes to record size and a null terminator at the end of the string
        var totalStringBytes = 4 * stringSizeBytes; // bucketTypeText, sourceText, externalIndexFilename, annoationsFileName

        uint versionedFixedInternalIndexSizeBytes = 0;
        uint headerInfoSectionAndStringSection = CalculateMinimumBodyReadSize(version) + totalStringBytes; 
        if (flags.HasInternalIndexInHeaderFlag() && internalFixedSizeIndexSize > 0)
        {
            versionedFixedInternalIndexSizeBytes = BucketIndexDictionary.CalculateDictionarySizeInBytes(internalFixedSizeIndexSize, headerInfoSectionAndStringSection + 2);
        }

        var versionedHeaderBodySizeBytes = headerInfoSectionAndStringSection + versionedFixedInternalIndexSizeBytes;
        return versionedHeaderBodySizeBytes;
    }
}
