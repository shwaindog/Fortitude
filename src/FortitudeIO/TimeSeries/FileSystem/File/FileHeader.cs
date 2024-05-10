#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using static FortitudeIO.TimeSeries.FileSystem.File.FortitudeTimeSeriesFileV1HeaderConstants;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File;

[StructLayout(LayoutKind.Explicit)]
public struct BucketIndexOffset
{
    public BucketIndexOffset(uint index, DateTime earliestEntryDateTime, BucketIndexFlags bucketIndexFlags, ulong fileOffset)
    {
        Index = index;
        earliestEntryDateTimeTicks = (ulong)earliestEntryDateTime.Ticks;
        BucketIndexFlags = bucketIndexFlags;
        FileOffset = (ulong)fileOffset;
    }

    [FieldOffset(4)] private ulong earliestEntryDateTimeTicks;

    [field: FieldOffset(0)] public uint Index { get; set; }

    public DateTime EarliestEntryDateTime
    {
        get => DateTime.FromBinary((long)earliestEntryDateTimeTicks);
        set => earliestEntryDateTimeTicks = (ulong)value.Ticks;
    }

    [field: FieldOffset(12)] public ulong FileOffset { get; set; }

    [field: FieldOffset(20)] public BucketIndexFlags BucketIndexFlags { get; set; }
}

[Flags]
public enum BucketIndexFlags : ushort
{
    EmptyEntry = 0
    , Deleted = 1
    , Corrupt = 4
    , HasData = 8
    , HasBucketStartDelimiter = 16
    , HasBucketEndDelimiter = 32
    , WriterCurrentlyOpened = 64
    , WriterAppending = 128
    , ClosedForReading = 256
    , ContainsSubBuckets = 512
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
    ushort HeaderVersion { get; }
    FileFlags FileFlags { get; }
    bool WriterOpen { get; }
    uint FileHeaderSize { get; }
    long FileSize { get; }

    uint Buckets { get; }
    bool BucketsHaveChanged { get; }
    bool FixedMaximumBuckets { get; }
    bool HasBucketIndexes { get; }
    byte[] BucketStartDelimiterPattern { get; }
    byte[] BucketEndDelimiterPattern { get; }
    IReadOnlyDictionary<int, BucketIndexOffset> BucketIndexes { get; }
    IEnumerable<BucketIndexOffset> EarliestOrderedBuckets { get; }
    IEnumerable<IMessageDeserializer> RequiredDeserializers { get; }
}

public interface IReadonlyFileHeader<TBucket>
{
    TBucket BucketAt(int index);
    IEnumerable<TBucket> BucketsContaining(DateTime fromStorageDateTime, DateTime toStorageDateTime);
    IEnumerable<TBucket> EarliestOrderedBuckets();
}

public interface IMutableFileHeader : IReadonlyFileHeader
{
    new ushort HeaderVersion { get; set; }
    new FileFlags FileFlags { get; set; }
    new uint FileHeaderSize { get; set; }
    new long FileSize { get; set; }
    new uint Buckets { get; set; }
    int RemainingFirstBucketPadding { get; set; }
    new IReadOnlyDictionary<int, BucketIndexOffset> BucketIndexes { get; }
}

public interface IExternalBucketIndexFile
{
    IDictionary<int, BucketIndexOffset> BucketIndexes { get; }
}

public interface IExternalFileIndexMutableFileHeader : IMutableFileHeader
{
    string? ExternalBucketIndexFileName { get; }
    IExternalBucketIndexFile ExternalBucketIndexFile(string externalBucketIndexFileName);
}

public interface IInternalIndexMutableFileHeader : IMutableFileHeader
{
    new IDictionary<int, BucketIndexOffset> BucketIndexes { get; }
}

public enum NextEntryType : byte
{
    InternalIndexHeaderSection
    , FirstDataBucket
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.None, Pack = 1, Size = 1)]
public struct FileStartVersionHeader
{
    public ushort HeaderVersion;
}

[StructLayout(LayoutKind.Explicit)]
public struct FileHeaderBodyV1
{
    [FieldOffset(0)] public FileFlags FileHeaderFlags;
    [FieldOffset(2)] public uint FileHeaderSize;
    [FieldOffset(6)] public ushort MaxHeaderTextSizeBytes;
    [FieldOffset(8)] public ushort InternalIndexNumberOfEntries;
    [FieldOffset(10)] public ulong FileSize;
    [FieldOffset(18)] public uint Buckets;
    [FieldOffset(22)] public ushort OriginalSourceTextFileStartOffset;
    [FieldOffset(24)] public ushort ExternalIndexFileNameFileStartOffset;
    [FieldOffset(26)] public ushort AnnotationFileNameFileStartOffset;
    [FieldOffset(28)] public ushort InternalIndexFileStartOffset;
    [FieldOffset(30)] public ulong FirstBucketFileStartOffset;
}

[StructLayout(LayoutKind.Explicit)]
public struct InternalIndexHeaderSectionV1
{
    [FieldOffset(2)] public ushort LargestIndexOfSetEntry;
    [FieldOffset(4)] public uint InternalIndexSizeBytes;
    [FieldOffset(8)] public BucketIndexOffset FirstIndexEntry;
}

public static class FortitudeTimeSeriesFileV1HeaderConstants
{
    public const long VersionFileOffset = 0;
    public const long FileFlagsFileOffset = 2;
    public const long HeaderSizeFileOffset = 4;
    public const long MaxTextBytesFileOffset = 8;
    public const long InternalIndexSizeFileOffset = 10;
    public const long FileSizeFileOffset = 12;
}

public unsafe class FileHeader : IMutableFileHeader
{
    private readonly ushort internalIndexNumberOfEntries;
    private readonly ushort maxHeaderTextSizeBytes;
    private TwoContiguousPagedFileChunks? fileHeaderMemoryChunk;

    private ushort headerVersion;
    private DateTime lastBucketsFileReadDateTime;
    private DateTime lastFileSizeFileReadDateTime;
    private DateTime nextFlagsFileReadDateTime;
    private FileFlags shortTermCacheFileFlags;

    public FileHeader(TwoContiguousPagedFileChunks startOfFileChunk)
    {
        fileHeaderMemoryChunk = startOfFileChunk;
        {
            var currPtr = fileHeaderMemoryChunk.ChunkAddress;
            headerVersion = StreamByteOps.ToUShort(ref currPtr);
            if (headerVersion != 1) throw new ArgumentException("File version is not supported");
            shortTermCacheFileFlags = (FileFlags)StreamByteOps.ToUShort(ref currPtr);
            FileHeaderSize = StreamByteOps.ToUInt(ref currPtr);
            maxHeaderTextSizeBytes = StreamByteOps.ToUShort(ref currPtr);
            internalIndexNumberOfEntries = StreamByteOps.ToUShort(ref currPtr);
        }

        nextFlagsFileReadDateTime = DateTime.UtcNow;
    }

    public void Dispose()
    {
        if (fileHeaderMemoryChunk != null) fileHeaderMemoryChunk.Dispose();
        fileHeaderMemoryChunk = null;
    }

    public ushort HeaderVersion
    {
        get => headerVersion;
        set
        {
            if (headerVersion == value || fileHeaderMemoryChunk == null) return;
            var currPtr = fileHeaderMemoryChunk.ChunkAddress;
            StreamByteOps.ToBytes(ref currPtr, value);
            headerVersion = value;
        }
    }

    public bool WriterOpen { get; }

    public FileFlags FileFlags
    {
        get
        {
            if (DateTime.UtcNow < nextFlagsFileReadDateTime || fileHeaderMemoryChunk == null) return shortTermCacheFileFlags;
            var currPtr = fileHeaderMemoryChunk.ChunkAddress + FileFlagsFileOffset;
            shortTermCacheFileFlags = (FileFlags)StreamByteOps.ToUShort(ref currPtr);
            return shortTermCacheFileFlags;
        }
        set
        {
            if (shortTermCacheFileFlags == value || fileHeaderMemoryChunk == null) return;
            var currPtr = fileHeaderMemoryChunk.ChunkAddress + FileFlagsFileOffset;
            StreamByteOps.ToBytes(ref currPtr, (ushort)value);
            shortTermCacheFileFlags = value;

            nextFlagsFileReadDateTime = DateTime.UtcNow.AddSeconds(10);
        }
    }

    public uint FileHeaderSize { get; set; }
    public long FileSize { get; set; }
    public uint Buckets { get; set; }
    public bool BucketsHaveChanged { get; }
    public bool FixedMaximumBuckets { get; }
    public bool HasBucketIndexes { get; }
    public byte[] BucketStartDelimiterPattern { get; }
    public byte[] BucketEndDelimiterPattern { get; }
    public IReadOnlyDictionary<int, BucketIndexOffset> BucketIndexes { get; }
    public IEnumerable<BucketIndexOffset> EarliestOrderedBuckets { get; }
    public IEnumerable<IMessageDeserializer> RequiredDeserializers { get; }
    public int RemainingFirstBucketPadding { get; set; }

    // public static uint CalculateMinimumBodyReadSize(ushort version)
    // {
    //     if (version != 1)
    //         throw new NotImplementedException("Only support version 1 of files");
    //     
    //     var versionStructSizeBytes = (uint)sizeof(FileStartVersionHeader);
    //     var versionedHeaderBodySizeBytes = versionStructSizeBytes + (uint)sizeof(FileHeaderBodyV1);
    //     return versionedHeaderBodySizeBytes;
    // }
    //
    // public static uint CalculateHeaderSize(FileFlags flags, ushort version, ushort maxStringSizeBytes = 512, uint internalFixedSizeIndexSize = 0)
    // {
    //     if (version != 1)
    //         throw new NotImplementedException("Only support version 1 of files");
    //     
    //     var stringSizeBytes = (uint)maxStringSizeBytes + 3; // two bytes to record size and a null terminator at the end of the string
    //     var totalStringBytes = 3 * stringSizeBytes; // sourceText, externalIndexFilename, annoationsFileName
    //
    //     uint versionedFixedInternalIndexSizeBytes = 0;
    //     if (flags.HasInternalIndexInHeaderFlag() && internalFixedSizeIndexSize > 0)
    //     {
    //         var sizeOfBucketIndexOffset = (uint)sizeof(BucketIndexOffset);
    //         var sizeOfIndexHeaderSection = (uint)sizeof(InternalIndexHeaderSectionV1);
    //         versionedFixedInternalIndexSizeBytes = sizeOfIndexHeaderSection + sizeOfBucketIndexOffset * (internalFixedSizeIndexSize - 1);
    //     }
    //
    //     var versionedHeaderBodySizeBytes = CalculateMinimumBodyReadSize(version) + totalStringBytes + versionedFixedInternalIndexSizeBytes;
    //     return versionedHeaderBodySizeBytes;
    // }
}
