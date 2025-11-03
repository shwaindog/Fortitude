// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers;
using FortitudeCommon.DataStructures.MemoryPools.Buffers.ByteBuffers.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.Storage.TimeSeries.FileSystem.File.Session;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session;
using FortitudeIO.Storage.TimeSeries.FileSystem.Session.Retrieval;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;

#endregion

namespace FortitudeTests.FortitudeIO.Storage.TimeSeries.FileSystem.File;

public unsafe class TestLevel1HourlyQuoteStructBucket : DataBucket<Level1QuoteStruct, TestLevel1HourlyQuoteStructBucket>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TestLevel1HourlyQuoteStructBucket));

    public TestLevel1HourlyQuoteStructBucket
    (IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null) :
        base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeBoundaryPeriod TimeBoundaryPeriod => TimeBoundaryPeriod.OneHour;

    public override IEnumerable<Level1QuoteStruct> ReadEntries(IMessageQueueBuffer readBuffer, IReaderContext<Level1QuoteStruct> readerContext)
    {
        if (!IsOpen) yield break;

        // var entryCount = 0;
        while (readerContext.ContinueSearching && readBuffer.ReadCursor < readBuffer.WriteCursor)
        {
            // var readCursor = readBuffer.ReadCursor;
            var checkEntry = GetEntryAt(readBuffer);
            if (readerContext.IsReverseChronologicalOrder)
            {
                if (!readerContext.CheckExceededPeriodRangeTime(checkEntry))
                    readerContext.ReadReverseAddToStart(checkEntry);
                else
                    break;
            }
            else if (readerContext.ProcessCandidateEntry(checkEntry))
            {
                yield return checkEntry;
            }

            // Logger.Info("Bucket {0} with StartTime {1} read EntryNum: {2} at {3} - {4} ", BucketId, PeriodStartTime, entryCount, checkEntry
            //           , readCursor);
            // entryCount++;
        }
        if (!readerContext.IsReverseChronologicalOrder) yield break;
        foreach (var checkEntry in readerContext.ReadReverse())
        {
            if (readerContext.ProcessCandidateEntry(checkEntry)) yield return checkEntry;
            if (!readerContext.ContinueSearching) break;
        }
        readerContext.ClearReadReverse();
    }

    protected Level1QuoteStruct GetEntryAt(IBuffer readBuffer)
    {
        var ptr = (Level1QuoteStruct*)(readBuffer.ReadBuffer + readBuffer.BufferRelativeReadCursor);
        readBuffer.ReadCursor += sizeof(Level1QuoteStruct);
        return *ptr;
    }

    public override AppendResult AppendEntry
    (IFixedByteArrayBuffer writeBuffer,
        IAppendContext<Level1QuoteStruct> entryContext, AppendResult appendResult)
    {
        // var writeCursor = growableBuffer.WriteCursor;
        // var writeEntry  = writeCursor / sizeof(Level1QuoteStruct);
        var ptr = (Level1QuoteStruct*)(writeBuffer.WriteBuffer + writeBuffer.BufferRelativeWriteCursor);
        *ptr = entryContext.CurrentEntry;

        appendResult.SerializedSize =  sizeof(Level1QuoteStruct);
        writeBuffer.WriteCursor     += sizeof(Level1QuoteStruct);
        ExpandedDataSize            += (ulong)sizeof(Level1QuoteStruct);
        TotalDataEntriesCount       += 1;
        // Logger.Info("Bucket {0} with StartTime {1} wrote EntryNumber: {2} for DataSize {3} at {4} {5:X} - {6}  ",
        //             BucketId, PeriodStartTime, writeEntry, sizeof(Level1QuoteStruct), writeCursor, (nint)ptr, *ptr);
        return appendResult;
    }
}
