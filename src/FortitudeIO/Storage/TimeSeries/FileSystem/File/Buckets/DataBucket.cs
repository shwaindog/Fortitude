// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.DataStructures.MemoryPools.Compression.Lzma;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Session;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;

#endregion

namespace FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct DataBucketHeader
{
    public ulong ExpandedDataSize;
}

public interface IDataBucket : IBucket
{
    bool                IsDataCompressed { get; }
    ulong               ExpandedDataSize { get; }
    IMessageQueueBuffer ResolveDataReader(long? atFileCursorOffset = null);
}

public interface IDataBucket<TEntry> : IDataBucket
    where TEntry : ITimeSeriesEntry
{
    IEnumerable<TEntry> ReadEntries(IMessageQueueBuffer readBuffer, IReaderContext<TEntry> readerContext);
}

public interface IMutableDataBucket<TEntry> : IBucket<TEntry>, IMutableBucket where TEntry : ITimeSeriesEntry
{
    IGrowableUnmanagedBuffer? DataWriterAtAppendLocation { get; }
    StorageAttemptResult      AppendEntry(IGrowableUnmanagedBuffer growableBuffer, TEntry entry);
}

public abstract unsafe class DataBucket<TEntry, TBucket> : BucketBase<TEntry, TBucket>, IDataBucket<TEntry>
    where TEntry : ITimeSeriesEntry where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(DataBucket<,>));

    protected ShiftableMemoryMappedFileView? BucketAppenderOrDataReaderFileView;

    private DataBucketHeader  cacheDataHeaderExtension;
    private LzmaEncoderParams compressionParams = new();

    private long         dataBucketHeaderExtensionRealignmentDelta;
    private LzmaDecoder? lzmaDecoder;
    private LzmaEncoder? lzmaEncoder;

    private DataBucketHeader*     mappedDataHeader;
    private FixedByteArrayBuffer? readerBuffer;

    private IFixedByteArrayBuffer? writerBuffer;

    protected DataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected virtual bool CanAccessDataBucketHeaderFromFileView =>
        CanAccessHeaderFromFileView && BucketHeaderFileView!.HighestFileCursor > EndAllHeadersSectionFileOffset
                                    && mappedDataHeader != null;

    protected long EndOfDataBucketHeaderSectionOffset => StartDataBucketHeaderSectionOffset + sizeof(DataBucketHeader);

    internal virtual long EndOfBaseBucketHeaderSectionOffset => base.EndAllHeadersSectionFileOffset;

    internal long StartDataBucketHeaderSectionOffset => EndOfBaseBucketHeaderSectionOffset + dataBucketHeaderExtensionRealignmentDelta;

    internal long StartOfDataSectionOffset => EndAllHeadersSectionFileOffset;

    public IFixedByteArrayBuffer? DataWriterAtAppendLocation
    {
        get
        {
            if (!IsOpenForAppend || BucketAppenderOrDataReaderFileView == null) return null;

            if (writerBuffer == null)
            {
                if (IsDataCompressed)
                {
                    writerBuffer = OwningSession.UncompressedBuffer;
                    if (TotalFileDataSizeBytes > 0)
                    {
                        var uncompressedRange = (IByteArray)writerBuffer.BackingMemoryAddressRange;
                        var viewOffset        = StartOfDataSectionOffset - BucketAppenderOrDataReaderFileView!.LowerViewFileCursorOffset;
                        var compressedRange
                            = BucketAppenderOrDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)TotalFileDataSizeBytes);
                        var uncompressedStream = new ByteArrayMemoryStream(uncompressedRange, true, false);
                        var compressedStream   = new ByteArrayMemoryStream(compressedRange, true);
                        lzmaDecoder ??= new LzmaDecoder();
                        lzmaDecoder.Decompress(compressedStream, uncompressedStream);
                    }
                    writerBuffer.SetLength(writerBuffer.BackingMemoryAddressRange.Length);
                }
                else
                {
                    BucketAppenderOrDataReaderFileView!.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, ushort.MaxValue * 16, true);
                    var viewOffset            = StartOfDataSectionOffset - BucketAppenderOrDataReaderFileView!.LowerViewFileCursorOffset;
                    var uncompressedFileRange = BucketAppenderOrDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, ushort.MaxValue * 16);
                    writerBuffer = new GrowableUnmanagedBuffer(uncompressedFileRange)
                    {
                        GrowRemainingBytesThreshold = ushort.MaxValue
                    };
                }
                writerBuffer.WriteCursor = (long)ExpandedDataSize;
            }
            if (writerBuffer is GrowableUnmanagedBuffer growableBuffer && writerBuffer.RemainingStorage < growableBuffer.GrowRemainingBytesThreshold)
            {
                writerBuffer = growableBuffer.GrowByDefaultSize();
            }
            return writerBuffer;
        }
    }
    public override long EndAllHeadersSectionFileOffset => EndOfDataBucketHeaderSectionOffset;

    public bool IsDataCompressed => BucketFlags.HasCompressedDataFlag();

    public ulong ExpandedDataSize
    {
        get
        {
            if (!CanAccessDataBucketHeaderFromFileView) return cacheDataHeaderExtension.ExpandedDataSize;
            cacheDataHeaderExtension.ExpandedDataSize = mappedDataHeader->ExpandedDataSize;
            return cacheDataHeaderExtension.ExpandedDataSize;
        }

        set
        {
            if (value == cacheDataHeaderExtension.ExpandedDataSize || !Writable || !CanAccessHeaderFromFileView) return;
            mappedDataHeader->ExpandedDataSize        = value;
            cacheDataHeaderExtension.ExpandedDataSize = value;
            if (!IsDataCompressed)
            {
                var additionalWriteSize = Writable ? 64 * 1024 : 0;
                BucketAppenderOrDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset
                                                                                        , (long)value + additionalWriteSize);
            }
        }
    }

    public abstract IEnumerable<TEntry> ReadEntries(IMessageQueueBuffer readBuffer, IReaderContext<TEntry> readerContext);

    public IMessageQueueBuffer ResolveDataReader(long? atFileCursorOffset = null)
    {
        if (readerBuffer != null)
        {
            if (atFileCursorOffset != null)
            {
                var dataStartDelta = atFileCursorOffset.Value - (StartOfDataSectionOffset - FileCursorOffset);
                readerBuffer.ReadCursor  = dataStartDelta;
                readerBuffer.WriteCursor = (long)ExpandedDataSize;
            }
            return readerBuffer;
        }
        if (IsDataCompressed)
        {
            // var start            = DateTime.Now;
            var uncompressionBufferSize = Math.Max(ExpandedDataSize, 4_096*4_096);
            var uncompressedBuffer      = OwningSession.UncompressedBuffer;
            uncompressedBuffer.SetLength((long)uncompressionBufferSize);
            BucketAppenderOrDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, (long)TotalFileDataSizeBytes);
            var viewOffset         = StartOfDataSectionOffset - BucketAppenderOrDataReaderFileView!.LowerViewFileCursorOffset;
            var compressedRange    = BucketAppenderOrDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)TotalFileDataSizeBytes);
            var uncompressedStream = new ByteArrayMemoryStream(uncompressedBuffer.BackingByteArray, true, false);
            var compressedStream   = new ByteArrayMemoryStream(compressedRange, true, false);
            lzmaDecoder ??= new LzmaDecoder();
            lzmaDecoder.Decompress(compressedStream, uncompressedStream);
            readerBuffer             = uncompressedBuffer;
            readerBuffer.WriteCursor = uncompressedStream.Position;
            readerBuffer.SetLength(readerBuffer.BackingMemoryAddressRange.Length);
            // Logger.Info("Bucket {0} took {1} ms to decompress and read compressed data", BucketId, (DateTime.Now - start).TotalMilliseconds);
        }
        else
        {
            BucketAppenderOrDataReaderFileView!.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, (int)ExpandedDataSize);
            var viewOffset            = StartOfDataSectionOffset - BucketAppenderOrDataReaderFileView!.LowerViewFileCursorOffset;
            var uncompressedFileRange = BucketAppenderOrDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)ExpandedDataSize);
            readerBuffer             = new FixedByteArrayBuffer(uncompressedFileRange);
            readerBuffer.WriteCursor = (long)ExpandedDataSize;
        }
        if (atFileCursorOffset != null)
        {
            var dataStartDelta = atFileCursorOffset.Value - (StartOfDataSectionOffset - FileCursorOffset);
            readerBuffer.ReadCursor = dataStartDelta;
        }
        else
        {
            readerBuffer.ReadCursor = 0;
        }
        return readerBuffer;
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        dataBucketHeaderExtensionRealignmentDelta = EndOfBaseBucketHeaderSectionOffset % 8 > 0 ? 8 - EndOfBaseBucketHeaderSectionOffset % 8 : 0;
        mappedDataHeader = (DataBucketHeader*)BucketHeaderFileView!
            .FixedFileCursorBufferPointer(StartDataBucketHeaderSectionOffset, shouldGrow: Writable);
        cacheDataHeaderExtension         =   *mappedDataHeader;
        alternativeHeaderAndDataFileView ??= BucketContainer.ContainingSession.ActiveBucketDataFileView;
        BucketAppenderOrDataReaderFileView =   alternativeHeaderAndDataFileView;
        if (asWritable && OwningSession.FileHeader.FileFlags.HasWriteDataCompressedFlag() && ExpandedDataSize == 0)
            BucketFlags |= BucketFlags.CompressedData;
        if (BucketHeaderFileView != BucketAppenderOrDataReaderFileView)
        {
            var requiredSize = IsDataCompressed ? TotalFileDataSizeBytes : ExpandedDataSize;
            requiredSize += Writable ? (ulong)ushort.MaxValue : 0;
            BucketAppenderOrDataReaderFileView.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, (long)requiredSize);
        }
        return this;
    }

    public override void CloseBucketFileViews()
    {
        if (!IsOpen) return;
        if (Writable)
        {
            if (IsDataCompressed && writerBuffer != null)
            {
                // var start                 = DateTime.Now;
                var safeCompressionBufferSize = Math.Max((long)ExpandedDataSize / 2, 4_096);
                var uncompressedByteArray     = writerBuffer.BackingByteArray;
                var originalByteArraySize     = uncompressedByteArray.Length;
                uncompressedByteArray.SetLength((long)ExpandedDataSize);
                var uncompressedStream = new ByteArrayMemoryStream(uncompressedByteArray, true, false);
                BucketAppenderOrDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, safeCompressionBufferSize, true);
                var viewOffset = StartOfDataSectionOffset - BucketAppenderOrDataReaderFileView!.LowerViewFileCursorOffset;
                var compressedRange
                    = BucketAppenderOrDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)safeCompressionBufferSize);
                var compressedStream = new ByteArrayMemoryStream(compressedRange, true, false);
                lzmaEncoder                      ??= new LzmaEncoder();
                compressionParams.DictionarySize =   (int)ExpandedDataSize / 2;
                compressionParams.InputSize      =   (int)ExpandedDataSize;
                lzmaEncoder.Compress(compressionParams, uncompressedStream, compressedStream);
                TotalFileDataSizeBytes = (ulong)compressedStream.Position;
                uncompressedByteArray.SetLength(originalByteArraySize);
                // Logger.Info("Bucket {0} took {1} ms to compress and write data", BucketId, (DateTime.Now - start).TotalMilliseconds);
            }
            else if (!IsDataCompressed && writerBuffer != null)
            {
                TotalFileDataSizeBytes = ExpandedDataSize;
            }
            if (OwningSession.FileHeader.FileSize < (ulong)StartOfDataSectionOffset + TotalFileDataSizeBytes)
                OwningSession.FileHeader.FileSize = (ulong)StartOfDataSectionOffset + TotalFileDataSizeBytes;
            // if (writerBuffer is { BackingMemoryAddressRange: IByteArray toFlash })
            //     toFlash.Flush();

            writerBuffer?.Dispose();
            writerBuffer = null;
        }
        readerBuffer?.Dispose();
        readerBuffer                     = null;
        BucketAppenderOrDataReaderFileView = null;
        base.CloseBucketFileViews();
    }


    protected override void EnsureHeaderViewReferencesCorrectlyMapped()
    {
        if (BucketHeaderFileView == null) return;
        base.EnsureHeaderViewReferencesCorrectlyMapped();
        mappedDataHeader = (DataBucketHeader*)BucketHeaderFileView!
            .FileCursorBufferPointer(StartDataBucketHeaderSectionOffset, shouldGrow: Writable);
        cacheDataHeaderExtension = *mappedDataHeader;
    }

    public override IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext)
    {
        var reader = ResolveDataReader();
        return ReadEntries(reader, readerContext);
    }

    public override AppendResult AppendEntry(IAppendContext<TEntry> entryContext)
    {
        if (!Writable || !IsOpen || !BucketFlags.HasBucketCurrentAppendingFlag()) return new AppendResult(StorageAttemptResult.BucketClosedForAppend);
        var entryStorageTime = entryContext.StorageTime;
        var checkWithinRange = CheckTimeSupported(entryStorageTime);
        if (checkWithinRange != StorageAttemptResult.PeriodRangeMatched) return new AppendResult(checkWithinRange);
        entryContext.LastAddedLeafBucket = this;
        var dataWriter = DataWriterAtAppendLocation!;
        var appendResult = new AppendResult(StorageAttemptResult.PeriodRangeMatched)
        {
            BucketId = BucketId, FileOffset = dataWriter.WriteCursor, StorageTime = entryContext.StorageTime
        };
        var result = AppendEntry(DataWriterAtAppendLocation!, entryContext, appendResult);
        if (result.StorageAttemptResult == StorageAttemptResult.PeriodRangeMatched) BucketContainer.EntryWrittenAt(entryStorageTime);
        return result;
    }

    public abstract AppendResult AppendEntry(IFixedByteArrayBuffer writeBuffer, IAppendContext<TEntry> entryContext, AppendResult appendResult);

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => OwningSession.ActiveBucketHeaderFileView;
}

public class ProxyDataBucket<TEntry, TBucket> : DataBucket<TEntry, TBucket>, IDataBucket<TEntry>
    where TEntry : ITimeSeriesEntry where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private long? endOfHeaderSectionFileOffset;

    public ProxyDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    internal override long EndOfBaseBucketHeaderSectionOffset => endOfHeaderSectionFileOffset!.Value;

    public long SetEndOfBaseBucketHeaderSectionOffset
    {
        set => endOfHeaderSectionFileOffset = value;
    }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.Tick;

    public override IEnumerable<TEntry> ReadEntries(IMessageQueueBuffer readBuffer, IReaderContext<TEntry> readerContext) =>
        // should never be called;
        Enumerable.Empty<TEntry>();

    public override AppendResult AppendEntry
    (IFixedByteArrayBuffer writeBuffer, IAppendContext<TEntry> entryContext
      , AppendResult appendResult) =>
        // should never be called;
        new(StorageAttemptResult.NoBucketChecked);
}

public abstract unsafe class IndexedDataBucket<TEntry, TBucket> : IndexedBucket<TEntry, TBucket>, IDataBucket
    where TEntry : ITimeSeriesEntry where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private ProxyDataBucket<TEntry, TBucket>? dualMappedBucket;

    protected IndexedDataBucket
    (IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected long EndOfDataBucketHeaderSectionOffset => dualMappedBucket!.StartDataBucketHeaderSectionOffset + sizeof(DataBucketHeader);

    public IFixedByteArrayBuffer? DataWriterAtAppendLocation => dualMappedBucket!.DataWriterAtAppendLocation;

    public bool IsDataCompressed => BucketFlags.HasCompressedDataFlag();

    public ulong ExpandedDataSize
    {
        get => dualMappedBucket!.ExpandedDataSize;
        set => dualMappedBucket!.ExpandedDataSize = value;
    }

    public IMessageQueueBuffer ResolveDataReader(long? atFileCursorOffset = null) => dualMappedBucket!.ResolveDataReader(atFileCursorOffset);

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);

        var firstOpen = dualMappedBucket == null;
        dualMappedBucket ??= new ProxyDataBucket<TEntry, TBucket>(BucketContainer, FileCursorOffset, Writable, alternativeHeaderAndDataFileView);
        if (firstOpen)
            dualMappedBucket.SetEndOfBaseBucketHeaderSectionOffset = EndAllHeadersSectionFileOffset;
        else
            dualMappedBucket.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        return this;
    }

    public override void CloseBucketFileViews()
    {
        if (!IsOpen) return;
        dualMappedBucket!.CloseBucketFileViews();
        base.CloseBucketFileViews();
    }

    public override IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext)
    {
        var matchingIndexOffsets =
            BucketIndexes.Values
                         .Where(bii => bii.Intersects(readerContext.PeriodRange));
        if (matchingIndexOffsets.Any())
        {
            var first = matchingIndexOffsets.First();
            return ReadEntries(ResolveDataReader(first.ParentOrFileOffset + FileCursorOffset), readerContext);
        }
        return Enumerable.Empty<TEntry>();
    }

    public abstract IEnumerable<TEntry> ReadEntries(IMessageQueueBuffer buffer, IReaderContext<TEntry> readerContext);

    public override AppendResult AppendEntry(IAppendContext<TEntry> entryContext)
    {
        if (!Writable || !IsOpen || !BucketFlags.HasBucketCurrentAppendingFlag()) return new AppendResult(StorageAttemptResult.BucketClosedForAppend);
        var entryStorageTime = entryContext.StorageTime;
        var checkWithinRange = CheckTimeSupported(entryStorageTime);
        if (checkWithinRange != StorageAttemptResult.PeriodRangeMatched) return new AppendResult(checkWithinRange);
        entryContext.LastAddedLeafBucket = this;
        var dataWriter = dualMappedBucket!.DataWriterAtAppendLocation!;
        var appendResult = new AppendResult(StorageAttemptResult.PeriodRangeMatched)
        {
            BucketId = BucketId, FileOffset = dataWriter.WriteCursor, StorageTime = entryContext.StorageTime
        };
        return AppendEntry(DataWriterAtAppendLocation!, entryContext, appendResult);
    }

    public abstract AppendResult AppendEntry(IFixedByteArrayBuffer growableBuffer, IAppendContext<TEntry> entryContext, AppendResult appendResult);

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => OwningSession.ActiveBucketHeaderFileView;
}

public interface IMessageDataBucket<TEntry> : IDataBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry, IVersionedMessage
{
    IMessageSerializer<TEntry>   MessageSerializer   { get; }
    IMessageDeserializer<TEntry> MessageDeserializer { get; }
}

public abstract class MessageDataBucket<TEntry, TBucket> : DataBucket<TEntry, TBucket>, IMessageDataBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry, IVersionedMessage
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected MessageDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public abstract IMessageSerializer<TEntry>   MessageSerializer   { get; }
    public abstract IMessageDeserializer<TEntry> MessageDeserializer { get; }
}

public abstract class IndexedMessageDataBucket<TEntry, TBucket> : IndexedDataBucket<TEntry, TBucket>, IMessageDataBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry, IVersionedMessage
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected IndexedMessageDataBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable
      , ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public abstract IMessageSerializer<TEntry>   MessageSerializer   { get; }
    public abstract IMessageDeserializer<TEntry> MessageDeserializer { get; }
}
