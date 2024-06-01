// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeIO.TimeSeries.FileSystem.File.Reading;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

public unsafe class TestLevel1HourlyQuoteStructBucket : DataBucket<Level1QuoteStruct, TestLevel1HourlyQuoteStructBucket>
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(TestLevel1HourlyQuoteStructBucket));

    public TestLevel1HourlyQuoteStructBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable,
        ShiftableMemoryMappedFileView? alternativeFileView = null) :
        base(bucketContainer, bucketFileCursorOffset, writable, alternativeFileView) { }

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneHour;

    public override IEnumerable<Level1QuoteStruct> ReadEntries(IBuffer readBuffer, IReaderContext<Level1QuoteStruct> readerContext)
    {
        if (!IsOpen) yield break;

        // var entryCount = 0;
        while (readerContext.ContinueSearching && readBuffer.ReadCursor < readBuffer.WriteCursor)
        {
            // var readCursor = readBuffer.ReadCursor;
            var checkEntry = GetEntryAt(readBuffer);
            if (readerContext.ProcessCandidateEntry(checkEntry))
                yield return checkEntry;

            // Logger.Info("Bucket {0} with StartTime {1} read EntryNum: {2} at {3} - {4} ", BucketId, PeriodStartTime, entryCount, checkEntry
            //           , readCursor);
            // entryCount++;
        }
    }

    protected Level1QuoteStruct GetEntryAt(IBuffer readBuffer)
    {
        var ptr = (Level1QuoteStruct*)(readBuffer.ReadBuffer + readBuffer.BufferRelativeReadCursor);
        readBuffer.ReadCursor += sizeof(Level1QuoteStruct);
        return *ptr;
    }

    public override StorageAttemptResult AppendEntry(IGrowableUnmanagedBuffer growableBuffer, Level1QuoteStruct entry)
    {
        if (!Writable || !IsOpen || !BucketFlags.HasBucketCurrentAppendingFlag()) return StorageAttemptResult.BucketClosedForAppend;
        var entryStorageTime = entry.StorageTime(StorageTimeResolver);
        var checkWithinRange = CheckTimeSupported(entryStorageTime);
        if (checkWithinRange != StorageAttemptResult.PeriodRangeMatched) return checkWithinRange;

        // var writeCursor = growableBuffer.WriteCursor;
        // var writeEntry  = writeCursor / sizeof(Level1QuoteStruct);
        var ptr = (Level1QuoteStruct*)(growableBuffer.WriteBuffer + growableBuffer.BufferRelativeWriteCursor);
        *ptr = entry;

        growableBuffer.WriteCursor += sizeof(Level1QuoteStruct);
        ExpandedDataSize           += (ulong)sizeof(Level1QuoteStruct);
        TotalDataEntriesCount      += 1;
        // Logger.Info("Bucket {0} with StartTime {1} wrote EntryNumber: {2} for DataSize {3} at {4} {5:X} - {6}  ",
        //             BucketId, PeriodStartTime, writeEntry, sizeof(Level1QuoteStruct), writeCursor, (nint)ptr, *ptr);
        return StorageAttemptResult.PeriodRangeMatched;
    }
}
