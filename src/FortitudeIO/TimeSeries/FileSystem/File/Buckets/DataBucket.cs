// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics;
using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Compression.Lzma;
using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct DataBucketHeader
{
    public ulong ExpandedDataSize;
}

public interface IDataBucket : IBucket
{
    bool    IsDataCompressed { get; }
    ulong   ExpandedDataSize { get; }
    IBuffer ResolveDataReader(long? atFileCursorOffset = null);
}

public interface IDataBucket<TEntry> : IDataBucket
    where TEntry : ITimeSeriesEntry<TEntry>
{
    IEnumerable<TEntry> ReadEntries(IBuffer readBuffer, IReaderContext<TEntry> readerContext);
}

public interface IMutableDataBucket<TEntry> : IBucket<TEntry>, IMutableBucket where TEntry : ITimeSeriesEntry<TEntry>
{
    IGrowableUnmanagedBuffer? DataWriterAtAppendLocation { get; }
    StorageAttemptResult      AppendEntry(IGrowableUnmanagedBuffer growableBuffer, TEntry entry);
}

public abstract unsafe class DataBucket<TEntry, TBucket> : BucketBase<TEntry, TBucket>, IDataBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private static readonly IFLogger     Logger = FLoggerFactory.Instance.GetLogger(typeof(DataBucket<,>));
    private static          LzmaDecoder? lzmaDecoder;
    private static          LzmaEncoder? lzmaEncoder;

    protected ShiftableMemoryMappedFileView? BucketAppenderDataReaderFileView;
    private   DataBucketHeader               cacheDataHeaderExtension;
    private   LzmaEncoderParams              compressionParams = new();
    private   long                           dataBucketHeaderExtensionRealignmentDelta;
    private   DataBucketHeader*              mappedDataHeader;
    private   FixedByteArrayBuffer?          readerBuffer;

    private IGrowableUnmanagedBuffer? writerBuffer;

    protected DataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected virtual bool CanAccessDataBucketHeaderFromFileView =>
        CanAccessHeaderFromFileView && BucketHeaderFileView!.HighestFileCursor > EndAllHeadersSectionFileOffset
                                    && mappedDataHeader != null;

    protected long EndOfDataBucketHeaderSectionOffset => StartDataBucketHeaderSectionOffset + sizeof(DataBucketHeader);

    internal virtual long EndOfBaseBucketHeaderSectionOffset => base.EndAllHeadersSectionFileOffset;

    internal long StartDataBucketHeaderSectionOffset => EndOfBaseBucketHeaderSectionOffset + dataBucketHeaderExtensionRealignmentDelta;
    internal long StartOfDataSectionOffset           => EndAllHeadersSectionFileOffset;

    public IGrowableUnmanagedBuffer? DataWriterAtAppendLocation
    {
        get
        {
            if (!IsOpenForAppend || BucketAppenderDataReaderFileView == null) return null;

            if (writerBuffer == null)
            {
                if (IsDataCompressed)
                {
                    writerBuffer = OwningSession.UncompressedBuffer;
                    while (writerBuffer.Length < (long)ExpandedDataSize + ushort.MaxValue) writerBuffer = writerBuffer.GrowByDefaultSize();
                    if (TotalFileDataSizeBytes > 0)
                    {
                        var uncompressedRange = (IByteArray)writerBuffer.BackingMemoryAddressRange;
                        var viewOffset        = StartOfDataSectionOffset - BucketAppenderDataReaderFileView!.LowerViewFileCursorOffset;
                        var compressedRange
                            = BucketAppenderDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)TotalFileDataSizeBytes);
                        var uncompressedStream = new ByteArrayMemoryStream(uncompressedRange, true, false);
                        var compressedStream   = new ByteArrayMemoryStream(compressedRange, true);
                        lzmaDecoder ??= new LzmaDecoder();
                        lzmaDecoder.Decompress(compressedStream, uncompressedStream);
                        if ((long)ExpandedDataSize != uncompressedStream.Position) Debugger.Break();
                    }
                    writerBuffer.WriteCursor = (long)ExpandedDataSize;
                }
                else
                {
                    var viewOffset            = StartOfDataSectionOffset - BucketAppenderDataReaderFileView!.LowerViewFileCursorOffset;
                    var uncompressedFileRange = BucketAppenderDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, ushort.MaxValue);
                    writerBuffer             = new GrowableUnmanagedBuffer(uncompressedFileRange);
                    writerBuffer.WriteCursor = (long)ExpandedDataSize;
                }
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
                var additionalWriteSize = Writable ? 4 * 1024 : 0;
                BucketAppenderDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset
                                                                                        , (long)value + additionalWriteSize);
            }
        }
    }

    public override IBucket OpenBucket(ShiftableMemoryMappedFileView? alternativeHeaderAndDataFileView = null, bool asWritable = false)
    {
        base.OpenBucket(alternativeHeaderAndDataFileView, asWritable);
        dataBucketHeaderExtensionRealignmentDelta = EndOfBaseBucketHeaderSectionOffset % 8 > 0 ? 8 - EndOfBaseBucketHeaderSectionOffset % 8 : 0;
        mappedDataHeader = (DataBucketHeader*)BucketHeaderFileView!
            .FixedFileCursorBufferPointer(StartDataBucketHeaderSectionOffset, shouldGrow: Writable);
        cacheDataHeaderExtension         =   *mappedDataHeader;
        alternativeHeaderAndDataFileView ??= BucketContainer.ContainingSession.ActiveBucketDataFileView;
        BucketAppenderDataReaderFileView =   alternativeHeaderAndDataFileView;
        if (asWritable && OwningSession.FileHeader.FileFlags.HasWriteDataCompressedFlag() && ExpandedDataSize == 0)
            BucketFlags |= BucketFlags.CompressedData;
        if (BucketHeaderFileView != BucketAppenderDataReaderFileView)
        {
            var requiredSize = IsDataCompressed ? TotalFileDataSizeBytes : ExpandedDataSize;
            requiredSize += Writable ? (ulong)ushort.MaxValue : 0;
            BucketAppenderDataReaderFileView.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, (long)requiredSize);
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
                var uncompressedByteArray = writerBuffer.BackingByteArray;
                var originalByteArraySize = uncompressedByteArray.Length;
                uncompressedByteArray.SetLength((long)ExpandedDataSize);
                var uncompressedStream = new ByteArrayMemoryStream(uncompressedByteArray, true, false);
                BucketAppenderDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, (long)ExpandedDataSize / 2, true);
                var viewOffset       = StartOfDataSectionOffset - BucketAppenderDataReaderFileView!.LowerViewFileCursorOffset;
                var compressedRange  = BucketAppenderDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)ExpandedDataSize / 2);
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
        BucketAppenderDataReaderFileView = null;
        base.CloseBucketFileViews();
    }

    public abstract IEnumerable<TEntry> ReadEntries(IBuffer readBuffer, IReaderContext<TEntry> readerContext);

    public IBuffer ResolveDataReader(long? atFileCursorOffset = null)
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
            var uncompressedSize      = ExpandedDataSize;
            var uncompressedBuffer    = OwningSession.UncompressedBuffer;
            var uncompressedByteArray = OwningSession.UncompressedBuffer.BackingByteArray;
            var originalLength        = uncompressedByteArray.Length;
            uncompressedByteArray.SetLength((long)uncompressedSize);
            BucketAppenderDataReaderFileView?.EnsureViewCoversFileCursorOffsetAndSize(StartOfDataSectionOffset, (long)TotalFileDataSizeBytes);
            var viewOffset         = StartOfDataSectionOffset - BucketAppenderDataReaderFileView!.LowerViewFileCursorOffset;
            var compressedRange    = BucketAppenderDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)TotalFileDataSizeBytes);
            var uncompressedStream = new ByteArrayMemoryStream(uncompressedByteArray, true, false);
            var compressedStream   = new ByteArrayMemoryStream(compressedRange, true);
            lzmaDecoder ??= new LzmaDecoder();
            lzmaDecoder.Decompress(compressedStream, uncompressedStream);
            if ((long)uncompressedSize != uncompressedStream.Position) Debugger.Break();
            readerBuffer             = new FixedByteArrayBuffer(uncompressedBuffer);
            readerBuffer.WriteCursor = uncompressedStream.Position;
            uncompressedByteArray.SetLength(originalLength);
            // Logger.Info("Bucket {0} took {1} ms to decompress and read compressed data", BucketId, (DateTime.Now - start).TotalMilliseconds);
        }
        else
        {
            var viewOffset            = StartOfDataSectionOffset - BucketAppenderDataReaderFileView!.LowerViewFileCursorOffset;
            var uncompressedFileRange = BucketAppenderDataReaderFileView.CreateUnmanagedByteArrayInThisRange(viewOffset, (int)ExpandedDataSize);
            readerBuffer             = new FixedByteArrayBuffer(uncompressedFileRange);
            readerBuffer.WriteCursor = (long)ExpandedDataSize;
        }
        if (atFileCursorOffset != null)
        {
            var dataStartDelta = atFileCursorOffset.Value - (StartOfDataSectionOffset - FileCursorOffset);
            readerBuffer.ReadCursor = dataStartDelta;
        }
        return readerBuffer;
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

    public override StorageAttemptResult AppendEntry(TEntry entry) => AppendEntry(DataWriterAtAppendLocation!, entry);

    public abstract StorageAttemptResult AppendEntry(IGrowableUnmanagedBuffer growableBuffer, TEntry entry);

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => OwningSession.ActiveBucketHeaderFileView;
}

public class ProxyDataBucket<TEntry, TBucket> : DataBucket<TEntry, TBucket>, IDataBucket<TEntry>
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private long? endOfHeaderSectionFileOffset;

    public ProxyDataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    internal override long EndOfBaseBucketHeaderSectionOffset => endOfHeaderSectionFileOffset!.Value;

    public long SetEndOfBaseBucketHeaderSectionOffset
    {
        set => endOfHeaderSectionFileOffset = value;
    }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.None;

    public override IEnumerable<TEntry> ReadEntries(IBuffer readBuffer, IReaderContext<TEntry> readerContext) =>
        // should never be called;
        Enumerable.Empty<TEntry>();

    public override StorageAttemptResult AppendEntry(IGrowableUnmanagedBuffer growableBuffer, TEntry entry) =>
        // should never be called;
        StorageAttemptResult.NoBucketChecked;
}

public abstract unsafe class IndexedDataBucket<TEntry, TBucket> : IndexedBucket<TEntry, TBucket>, IDataBucket
    where TEntry : ITimeSeriesEntry<TEntry> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    private ProxyDataBucket<TEntry, TBucket>? dualMappedBucket;

    protected IndexedDataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    protected long EndOfDataBucketHeaderSectionOffset => dualMappedBucket!.StartDataBucketHeaderSectionOffset + sizeof(DataBucketHeader);

    public IGrowableUnmanagedBuffer? DataWriterAtAppendLocation => dualMappedBucket!.DataWriterAtAppendLocation;

    public bool IsDataCompressed => BucketFlags.HasCompressedDataFlag();

    public ulong ExpandedDataSize
    {
        get => dualMappedBucket!.ExpandedDataSize;
        set => dualMappedBucket!.ExpandedDataSize = value;
    }

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

    public IBuffer ResolveDataReader(long? atFileCursorOffset = null) => dualMappedBucket!.ResolveDataReader(atFileCursorOffset);

    public override IEnumerable<TEntry> ReadEntries(IReaderContext<TEntry> readerContext)
    {
        var matchingIndexOffsets = BucketIndexes.Values
                                                .Where(bii => bii.Intersects(readerContext.PeriodRange));
        if (matchingIndexOffsets.Any())
        {
            var first = matchingIndexOffsets.First();
            return ReadEntries(ResolveDataReader(first.ParentOrFileOffset + FileCursorOffset), readerContext);
        }
        return Enumerable.Empty<TEntry>();
    }

    public abstract IEnumerable<TEntry> ReadEntries(IBuffer buffer, IReaderContext<TEntry> readerContext);

    public override StorageAttemptResult AppendEntry(TEntry entry) => AppendEntry(DataWriterAtAppendLocation!, entry);

    public abstract StorageAttemptResult AppendEntry(IGrowableUnmanagedBuffer growableBuffer, TEntry entry);

    protected override ShiftableMemoryMappedFileView SelectBucketHeaderFileView() => OwningSession.ActiveBucketHeaderFileView;
}

public interface IMessageDataBucket<TEntry> : IDataBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry<TEntry>, IVersionedMessage
{
    IMessageSerializer<TEntry>   MessageSerializer   { get; }
    IMessageDeserializer<TEntry> MessageDeserializer { get; }
}

public abstract class MessageDataBucket<TEntry, TBucket> : DataBucket<TEntry, TBucket>, IMessageDataBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry<TEntry>, IVersionedMessage
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected MessageDataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public abstract IMessageSerializer<TEntry>   MessageSerializer   { get; }
    public abstract IMessageDeserializer<TEntry> MessageDeserializer { get; }
}

public abstract class IndexedMessageDataBucket<TEntry, TBucket> : IndexedDataBucket<TEntry, TBucket>, IMessageDataBucket<TEntry>
    where TEntry : class, ITimeSeriesEntry<TEntry>, IVersionedMessage
    where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket<TEntry>
{
    protected IndexedMessageDataBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool writable, ShiftableMemoryMappedFileView? alternativeFileView = null)
        : base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public abstract IMessageSerializer<TEntry>   MessageSerializer   { get; }
    public abstract IMessageDeserializer<TEntry> MessageDeserializer { get; }
}
