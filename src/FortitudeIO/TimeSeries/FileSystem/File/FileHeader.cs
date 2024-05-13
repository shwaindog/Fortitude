#region

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using static FortitudeIO.TimeSeries.FileSystem.File.FortitudeTimeSeriesFileV1HeaderConstants;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

[StructLayout(LayoutKind.Sequential)]
public struct BucketIndexOffset
{
    public BucketIndexOffset(uint bucketId, DateTime earliestEntryDateTime, BucketIndexFlags bucketIndexFlags, ulong fileOffset, uint bucketSize)
    {
        BucketId = bucketId;
        earliestEntryDateTimeTicks = earliestEntryDateTime.Ticks;
        BucketIndexFlags = bucketIndexFlags;
        BucketSize = bucketSize;
        FileOffset = fileOffset;
    }

    public uint BucketId;
    public uint BucketSize;
    private long earliestEntryDateTimeTicks;
    public ulong FileOffset;
    public BucketIndexFlags BucketIndexFlags;
    
    public DateTime EarliestEntryDateTime
    {
        get => DateTime.FromBinary(earliestEntryDateTimeTicks);
        set => earliestEntryDateTimeTicks = value.Ticks;
    }
}

public class BucketIndexEarliestEntryComparer : IComparer<BucketIndexOffset>
{
    public int Compare(BucketIndexOffset lhs, BucketIndexOffset rhs)
    {
        if(lhs.EarliestEntryDateTime > rhs.EarliestEntryDateTime) return 1;
        if(lhs.EarliestEntryDateTime < rhs.EarliestEntryDateTime) return -1;
        return 0;
    }
}

[Flags]
public enum BucketIndexFlags : ushort
{
    EmptyEntry = 0
    , Deleted = 0x00_01
    , Corrupt = 0x00_02
    , HadUnexpectedClose = 0x00_04
    , HasData = 0x00_08
    , HasBucketStartDelimiter = 0x00_10
    , HasBucketEndDelimiter = 0x00_20
    , WriterCurrentlyOpened = 0x00_40
    , WriterAppending = 0x00_80
    , ClosedForReading = 0x01_00
    , HasSubBuckets = 0x02_00
    , HasSubBucketIndex = 0x04_00
    , HasSummary = 0x08_00
}

[Flags]
public enum BucketFlags : byte
{
    None = 0
    , FixedSizeBuckets = 1
    , FixedMaxBuckets = 2
    , HasBucketStartDelimiter = 4
    , HasBucketEndDelimiter = 8
    , NoInterBucketPadding = 16
}

public interface IReadonlyFileHeader : IDisposable
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
    uint InternalIndexMaxSize { get; }
    ulong FileSize { get; }
    ushort MaxHeaderTextSizeBytes { get; }
    uint Buckets { get; }
    uint LastWriterBucket { get; }
    FileOperation LastWriterOperation { get; }
    ulong LastWriterLastWriteOffset { get; }
    DateTime LastWriterWriteTime { get; }
    bool BucketsHaveChanged { get; }
    IReadOnlyDictionary<uint, BucketIndexOffset> BucketIndexes { get; }
    IEnumerable<BucketIndexOffset> EarliestOrderedBuckets { get; }
}

public interface IReadonlyFileHeader<TBucket> : IReadonlyFileHeader
{
    TBucket BucketAt(uint index, ShiftableMemoryMappedFileView usingMappedFileView);
    IEnumerable<TBucket> BucketsContaining(DateTime fromStorageDateTime, DateTime toStorageDateTime, ShiftableMemoryMappedFileView usingMappedFileView);
    IEnumerable<TBucket> ChronologicallyOrderedBuckets(ShiftableMemoryMappedFileView usingMappedFileView);
}

public interface IMutableFileHeader : IReadonlyFileHeader
{
    new ushort HeaderVersion { get; set; }
    new FileFlags FileFlags { get; set; }
    new ushort FileTypeEnum { get; set; }
    new Type? BucketType { get; }
    new uint InternalIndexMaxSize { get; set; }
    new ushort MaxHeaderTextSizeBytes { get; set; }
    new uint FileHeaderSize { get; set; }
    new ulong FileSize { get; set; }
    new uint Buckets { get; set; }
    new uint LastWriterBucket { get; set; }
    new FileOperation LastWriterOperation { get; set; }
    new ulong LastWriterLastWriteOffset { get; set; }
    new DateTime LastWriterWriteTime { get; set; }
    int RemainingFirstBucketPadding { get; set; }
    new IDictionary<uint, BucketIndexOffset> BucketIndexes { get; }
}

public enum FileOperation : byte
{
    CreateFile
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
public struct FileHeaderBodyV1
{
    public const ushort SerializationParameterDataMaxSize = 1024;
    public FileFlags FileHeaderFlags;
    public uint FileHeaderSize;
    public ushort FileTypeEnum;
    public ushort MaxHeaderTextSizeBytes;
    public ushort BucketTypeTextFileStartOffset;
    public uint InternalIndexSize;
    public ulong FileSize;
    public uint Buckets;
    public uint LastWriterBucket;
    public FileOperation LastWriterOperation;
    public ulong LastWriterLastWriteOffset;
    public long LastWriterWriteTime;
    public ushort OriginalSourceTextFileStartOffset;
    public ushort ExternalIndexFileNameFileStartOffset;
    public ushort AnnotationFileNameFileStartOffset;
    public ushort InternalIndexFileStartOffset;
    public uint FirstBucketFileStartOffset;
    public ushort SerializationParameterDataSize;
    public byte SerializationParameterData; 
}

[StructLayout(LayoutKind.Sequential)]
public struct InternalIndexHeaderSectionV1
{
    public uint LargestIndexSetEntry;
    public uint InternalIndexSizeBytes;
    public BucketIndexOffset FirstIndexEntry;
}

public static class FortitudeTimeSeriesFileV1HeaderConstants
{
    public static readonly unsafe ushort FirstTextLocationOffset = (ushort)(sizeof(FileHeaderBodyV1) + FileHeaderBodyV1.SerializationParameterDataMaxSize);
    public const long BodyFileOffset = 2;

    static unsafe FortitudeTimeSeriesFileV1HeaderConstants()
    {
        FileHeaderBodyV1 v1HeaderBody = new();
        byte* addr = (byte*)&v1HeaderBody - BodyFileOffset;
        FileFlagsFileOffset = (byte*)&v1HeaderBody.FileHeaderFlags - addr;
        FileTypeFileOffset = (byte*)&v1HeaderBody.FileTypeEnum - addr;
        HeaderSizeFileOffset = (byte*)&v1HeaderBody.FileHeaderSize - addr;
        MaxTextBytesFileOffset = (byte*)&v1HeaderBody.MaxHeaderTextSizeBytes - addr;
        BucketTypeTextFileStartOffsetFileOffset = (byte*)&v1HeaderBody.BucketTypeTextFileStartOffset - addr;
        InternalIndexSizeFileOffset = (byte*)&v1HeaderBody.InternalIndexSize - addr;
        FileSizeFileOffset = (byte*)&v1HeaderBody.FileSize - addr;
        BucketsFileOffset = (byte*)&v1HeaderBody.Buckets - addr;
        LastWriterBucketFileOffset = (byte*)&v1HeaderBody.LastWriterBucket - addr;
        LastWriterOperationFileOffset = (byte*)&v1HeaderBody.LastWriterOperation - addr;
        LastWriterLastWriteOffsetFileOffset = (byte*)&v1HeaderBody.LastWriterLastWriteOffset - addr;
        LastWriterWriteTimeFileOffset = (byte*)&v1HeaderBody.LastWriterWriteTime - addr;
        OriginalSourceTextFileStartOffsetFileOffset = (byte*)&v1HeaderBody.OriginalSourceTextFileStartOffset - addr;
        ExternalIndexFileNameFileStartOffsetFileOffset = (byte*)&v1HeaderBody.ExternalIndexFileNameFileStartOffset - addr;
        AnnotationFileNameFileStartOffsetFileOffset = (byte*)&v1HeaderBody.AnnotationFileNameFileStartOffset - addr;
        InternalIndexFileStartOffsetFileOffset = (byte*)&v1HeaderBody.InternalIndexFileStartOffset - addr;
        FirstBucketFileStartOffsetFileOffset = (byte*)&v1HeaderBody.FirstBucketFileStartOffset - addr;
        SerializationParameterDataSizeFileOffset = (byte*)&v1HeaderBody.SerializationParameterDataSize - addr;
        SerializationParameterDataFileOffset = &v1HeaderBody.SerializationParameterData - addr;
    }


    public const long VersionFileOffset = 0;
    public static readonly long FileFlagsFileOffset;
    public static readonly long FileTypeFileOffset;
    public static readonly long HeaderSizeFileOffset;
    public static readonly long MaxTextBytesFileOffset;
    public static readonly long BucketTypeTextFileStartOffsetFileOffset;
    public static readonly long InternalIndexSizeFileOffset;
    public static readonly long FileSizeFileOffset;
    public static readonly long BucketsFileOffset;
    public static readonly long LastWriterBucketFileOffset;
    public static readonly long LastWriterOperationFileOffset;
    public static readonly long LastWriterLastWriteOffsetFileOffset;
    public static readonly long LastWriterWriteTimeFileOffset;
    public static readonly long OriginalSourceTextFileStartOffsetFileOffset;
    public static readonly long ExternalIndexFileNameFileStartOffsetFileOffset;
    public static readonly long AnnotationFileNameFileStartOffsetFileOffset;
    public static readonly long InternalIndexFileStartOffsetFileOffset;
    public static readonly long FirstBucketFileStartOffsetFileOffset;
    public static readonly long SerializationParameterDataSizeFileOffset;
    public static readonly long SerializationParameterDataFileOffset;
}

public unsafe class FileHeader : IMutableFileHeader
{
    public const ushort NewFileDefaultVersion = 1;
    public static readonly ushort[] SupportedFileVersions = [1];
    private ShiftableMemoryMappedFileView? headerMemoryMappedFileView;
    private bool isWritable;

    private ushort headerVersion;

    private FileHeaderBodyV1 writableV1HeaderBody;
    private FileHeaderBodyV1 cacheV1HeaderBody;
    private string? bucketTypeString;
    private BucketIndexDictionary? internalReadonlyIndexDictionary;
    private BucketIndexDictionary? internalWritableIndexDictionary;
    private List<BucketIndexOffset>? cacheSortedBucketIndexOffsets;
    private BucketIndexEarliestEntryComparer? bucketIndexEarliestEntryComparer;


    public FileHeader(ShiftableMemoryMappedFileView memoryMappedFileView, FileFlags fileFlags = FileFlags.None,
        uint internalIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        headerMemoryMappedFileView = memoryMappedFileView;
        {
            var currPtr = headerMemoryMappedFileView.LowerChunkAddress;
            headerVersion = StreamByteOps.ToUShort(ref currPtr);
            if (headerVersion == 0) // new file
            {
                isWritable = true;
                HeaderVersion = NewFileDefaultVersion;
                writableV1HeaderBody = Unsafe.AsRef<FileHeaderBodyV1>(memoryMappedFileView.LowerChunkAddress + 2);
                writableV1HeaderBody.FileHeaderFlags = FileFlags.WriterOpened | fileFlags;
                writableV1HeaderBody.InternalIndexSize = internalIndexSize;
                writableV1HeaderBody.MaxHeaderTextSizeBytes = maxStringSizeBytes;
                writableV1HeaderBody.FileHeaderSize = CalculateHeaderSize(fileFlags, HeaderVersion, internalIndexSize, maxStringSizeBytes);


                cacheV1HeaderBody = writableV1HeaderBody;
            }
            else
            {
                isWritable = fileFlags.HasWriterOpenedFlag();
                if (!SupportedFileVersions.Contains(headerVersion)) throw new ArgumentException($"File version {headerVersion} is not supported");
                var readFileFlags = (FileFlags)StreamByteOps.ToUShort(ref currPtr);
                writableV1HeaderBody = Unsafe.AsRef<FileHeaderBodyV1>(memoryMappedFileView.LowerChunkAddress + 2);
                writableV1HeaderBody.FileHeaderFlags = readFileFlags | fileFlags.Unset(FileFlags.HasInternalIndexInHeader);
                cacheV1HeaderBody = writableV1HeaderBody;
            }
        }
    }

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
                var ptr = headerMemoryMappedFileView.LowerChunkAddress + VersionFileOffset;
                StreamByteOps.ToBytes(ref ptr, value);
                headerMemoryMappedFileView.FlushCursorDataToDisk(VersionFileOffset, sizeof(ushort));
            }
            headerVersion = value;
        }
    }

    public FileFlags FileFlags
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.FileHeaderFlags;
            cacheV1HeaderBody.FileHeaderFlags = writableV1HeaderBody.FileHeaderFlags;
            return cacheV1HeaderBody.FileHeaderFlags;
        }
        set
        {
            if (cacheV1HeaderBody.FileHeaderFlags == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.FileHeaderFlags = value;
                cacheV1HeaderBody.FileHeaderFlags = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(FileFlagsFileOffset, sizeof(FileFlags));
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
                writableV1HeaderBody.FileHeaderSize = value;
                cacheV1HeaderBody.FileHeaderSize = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(HeaderSizeFileOffset, sizeof(uint));
            }
        }
    }

    public ushort MaxHeaderTextSizeBytes
    {
        get => cacheV1HeaderBody.MaxHeaderTextSizeBytes;
        set
        {
            if (cacheV1HeaderBody.MaxHeaderTextSizeBytes == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.MaxHeaderTextSizeBytes = value;
                cacheV1HeaderBody.MaxHeaderTextSizeBytes = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(MaxTextBytesFileOffset, sizeof(ushort));
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
            if (writableV1HeaderBody.BucketTypeTextFileStartOffset != 0 && headerMemoryMappedFileView != null)
            {
                var ptr = headerMemoryMappedFileView.LowerChunkAddress + writableV1HeaderBody.BucketTypeTextFileStartOffset;
                bucketTypeString = StreamByteOps.ToStringWithSizeHeader(ref ptr);
            }
            return bucketTypeString;
        }
        set
        {
            if (bucketTypeString == value || headerMemoryMappedFileView == null) return;
            if (isWritable && value != null)
            {
                var ptr = headerMemoryMappedFileView.LowerChunkAddress + FirstTextLocationOffset;
                var bytes = StreamByteOps.ToBytesWithSizeHeader(ref ptr, value, MaxHeaderTextSizeBytes);
                headerMemoryMappedFileView.FlushCursorDataToDisk(FirstTextLocationOffset, bytes);
                writableV1HeaderBody.BucketTypeTextFileStartOffset = FirstTextLocationOffset;
                cacheV1HeaderBody.BucketTypeTextFileStartOffset = FirstTextLocationOffset;
                headerMemoryMappedFileView.FlushCursorDataToDisk(writableV1HeaderBody.BucketTypeTextFileStartOffset, sizeof(ushort));
            }
            else if(value == null) 
            {
                writableV1HeaderBody.BucketTypeTextFileStartOffset = 0;
                cacheV1HeaderBody.BucketTypeTextFileStartOffset = 0;
                headerMemoryMappedFileView.FlushCursorDataToDisk(writableV1HeaderBody.BucketTypeTextFileStartOffset, sizeof(ushort));

            }
            bucketTypeString = value;
        }
    }

    public uint InternalIndexMaxSize
    {
        get => cacheV1HeaderBody.InternalIndexSize;
        set
        {
            if (cacheV1HeaderBody.InternalIndexSize == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.InternalIndexSize = value;
                cacheV1HeaderBody.InternalIndexSize = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(InternalIndexSizeFileOffset, sizeof(uint));
            }
        }
    }

    public ulong FileSize
    {
        get
        {
            if ( headerMemoryMappedFileView == null) return cacheV1HeaderBody.FileSize;
            cacheV1HeaderBody.FileSize = writableV1HeaderBody.FileSize;
            return cacheV1HeaderBody.FileSize;
        }
        set
        {
            if (cacheV1HeaderBody.FileSize == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.FileSize = value;
                cacheV1HeaderBody.FileSize = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(FileSizeFileOffset, sizeof(ulong));
            }
        }
    }

    public uint Buckets
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.Buckets;
            cacheV1HeaderBody.Buckets = writableV1HeaderBody.Buckets;
            return cacheV1HeaderBody.Buckets;
        }
        set
        {
            if (cacheV1HeaderBody.Buckets == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.Buckets = value;
                cacheV1HeaderBody.Buckets = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(BucketsFileOffset, sizeof(uint));
            }
        }
    }

    public uint LastWriterBucket
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.LastWriterBucket;
            cacheV1HeaderBody.LastWriterBucket = writableV1HeaderBody.LastWriterBucket;
            return writableV1HeaderBody.LastWriterBucket;
        }
        set
        {
            if (cacheV1HeaderBody.LastWriterBucket == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.LastWriterBucket = value;
                cacheV1HeaderBody.LastWriterBucket = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(LastWriterBucketFileOffset, sizeof(uint));
            }
        }
    }

    public FileOperation LastWriterOperation
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.LastWriterOperation;
            cacheV1HeaderBody.LastWriterOperation = writableV1HeaderBody.LastWriterOperation;
            return cacheV1HeaderBody.LastWriterOperation;
        }
        set
        {
            if (cacheV1HeaderBody.LastWriterOperation == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.LastWriterOperation = value;
                cacheV1HeaderBody.LastWriterOperation = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(LastWriterOperationFileOffset, sizeof(FileOperation));
            }
        }
    }

    public ulong LastWriterLastWriteOffset
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.LastWriterLastWriteOffset;
            cacheV1HeaderBody.LastWriterLastWriteOffset = writableV1HeaderBody.LastWriterLastWriteOffset;
            return cacheV1HeaderBody.LastWriterLastWriteOffset;
        }
        set
        {
            if (cacheV1HeaderBody.LastWriterLastWriteOffset == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.LastWriterLastWriteOffset = value;
                cacheV1HeaderBody.LastWriterLastWriteOffset = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(LastWriterLastWriteOffsetFileOffset, sizeof(ulong));
            }
        }
    }

    public DateTime LastWriterWriteTime
    {
        get
        {
            if (headerMemoryMappedFileView == null) return DateTime.FromBinary(cacheV1HeaderBody.LastWriterWriteTime);
            cacheV1HeaderBody.LastWriterWriteTime = writableV1HeaderBody.LastWriterWriteTime;
            return DateTime.FromBinary(cacheV1HeaderBody.LastWriterWriteTime);
        }
        set
        {
            if (DateTime.FromBinary(cacheV1HeaderBody.LastWriterWriteTime) == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.LastWriterWriteTime = value.Ticks;
                cacheV1HeaderBody.LastWriterWriteTime = writableV1HeaderBody.LastWriterWriteTime;
                headerMemoryMappedFileView.FlushCursorDataToDisk(LastWriterWriteTimeFileOffset, sizeof(long));
            }
        }
    }

    public ushort InternalIndexFileStartOffset
    {
        get
        {
            if (headerMemoryMappedFileView == null) return cacheV1HeaderBody.InternalIndexFileStartOffset;
            cacheV1HeaderBody.InternalIndexFileStartOffset = writableV1HeaderBody.InternalIndexFileStartOffset;
            return cacheV1HeaderBody.InternalIndexFileStartOffset;
        }
        set
        {
            if (cacheV1HeaderBody.InternalIndexFileStartOffset == value || headerMemoryMappedFileView == null) return;
            if (isWritable)
            {
                writableV1HeaderBody.InternalIndexFileStartOffset = value;
                cacheV1HeaderBody.InternalIndexFileStartOffset = value;
                headerMemoryMappedFileView.FlushCursorDataToDisk(InternalIndexFileStartOffset, sizeof(ushort));
            }
        }
    }

    public bool BucketsHaveChanged => writableV1HeaderBody.Buckets != cacheV1HeaderBody.Buckets;


    IDictionary<uint, BucketIndexOffset> IMutableFileHeader.BucketIndexes
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

    public IReadOnlyDictionary<uint, BucketIndexOffset> BucketIndexes
    {
        get
        {
            if (headerMemoryMappedFileView == null) throw new Exception("File is closed");
            if (!HasInternalIndex && !HasExternalBucketIndex) throw new Exception("No indexes defined for file");
            
            // Todo add external indexes later

            internalReadonlyIndexDictionary
                ??= new BucketIndexDictionary(headerMemoryMappedFileView, InternalIndexFileStartOffset, InternalIndexMaxSize, true);
            return internalReadonlyIndexDictionary;

        }
    }

    public IEnumerable<BucketIndexOffset> EarliestOrderedBuckets
    {
        get
        {
            cacheSortedBucketIndexOffsets ??= new List<BucketIndexOffset>();
            if (!cacheSortedBucketIndexOffsets.Any() && !WriterOpen) return cacheSortedBucketIndexOffsets;
            bucketIndexEarliestEntryComparer ??= new BucketIndexEarliestEntryComparer();
            var bucketIndexes = BucketIndexes;
            cacheSortedBucketIndexOffsets.AddRange(bucketIndexes.Values);
            cacheSortedBucketIndexOffsets.Sort(bucketIndexEarliestEntryComparer);
            return cacheSortedBucketIndexOffsets;
        }
    }

    public int RemainingFirstBucketPadding { get; set; }

    public ushort NextStringOffset => (ushort)(MaxHeaderTextSizeBytes + 3);

    public static uint CalculateMinimumBodyReadSize(ushort version)
    {
        if (!SupportedFileVersions.Contains(version))
            throw new NotImplementedException($"Only supports version [{SupportedFileVersions.JoinToString()}] of files");
        
        var versionStructSizeBytes = (uint)sizeof(FileStartVersionHeader);
        var versionedHeaderBodySizeBytes = versionStructSizeBytes + (uint)sizeof(FileHeaderBodyV1) + FileHeaderBodyV1.SerializationParameterDataMaxSize -1;
        return versionedHeaderBodySizeBytes;
    }
    
    public static uint CalculateHeaderSize(FileFlags flags, ushort version, uint internalFixedSizeIndexSize = 0, ushort maxStringSizeBytes = 512)
    {
        if (!SupportedFileVersions.Contains(version))
            throw new NotImplementedException($"Only supports version [{SupportedFileVersions.JoinToString()}] of files");
        
        var stringSizeBytes = (uint)maxStringSizeBytes + 3; // two bytes to record size and a null terminator at the end of the string
        var totalStringBytes = 4 * stringSizeBytes; // bucketTypeText, sourceText, externalIndexFilename, annoationsFileName
    
        uint versionedFixedInternalIndexSizeBytes = 0;
        if (flags.HasInternalIndexInHeaderFlag() && internalFixedSizeIndexSize > 0)
        {
            var sizeOfBucketIndexOffset = (uint)sizeof(BucketIndexOffset);
            var sizeOfIndexHeaderSection = (uint)sizeof(InternalIndexHeaderSectionV1);
            versionedFixedInternalIndexSizeBytes = sizeOfIndexHeaderSection + sizeOfBucketIndexOffset * (internalFixedSizeIndexSize - 1);
        }
    
        var versionedHeaderBodySizeBytes = CalculateMinimumBodyReadSize(version) + totalStringBytes + versionedFixedInternalIndexSizeBytes;
        return versionedHeaderBodySizeBytes;
    }
}
