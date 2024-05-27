// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Monitoring.Logging;
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

    public override IEnumerable<Level1QuoteStruct> ReadEntries(IReaderContext<Level1QuoteStruct> readerContext, long? fromFileCursorOffset = null)
    {
        if (!IsOpen) yield break;

        var endOffSet      = BucketDataStartFileOffset + (long)DataSizeBytes;
        var readFileOffset = fromFileCursorOffset ?? BucketDataStartFileOffset;
        while (readFileOffset < endOffSet)
        {
            var checkEntry = GetEntryAt(readFileOffset);
            if (readerContext.ProcessCandidateEntry(checkEntry))
                yield return checkEntry;

            readFileOffset = MoveNextEntry(readFileOffset);
            // Logger.Info("Bucket {0} with StartTime {1} read EntryNum: {2} at {3}- {4} ", BucketId, PeriodStartTime, readEntryCount++
            //     , readFileOffset, checkEntry);
        }
    }

    protected Level1QuoteStruct GetEntryAt(long fileCursorOffset)
    {
        var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(fileCursorOffset);
        return *ptr;
    }

    protected long MoveNextEntry(long currentFileCursorOffset) => currentFileCursorOffset + sizeof(Level1QuoteStruct);

    public override StorageAttemptResult AppendEntry(Level1QuoteStruct entry)
    {
        if (!Writable || !IsOpen || !BucketFlags.HasBucketCurrentAppendingFlag()) return StorageAttemptResult.BucketClosedForAppend;
        var entryStorageTime = entry.StorageTime(StorageTimeResolver);
        var checkWithinRange = CheckTimeSupported(entryStorageTime);
        if (checkWithinRange != StorageAttemptResult.PeriodRangeMatched) return checkWithinRange;
        var writeFileOffset = BucketDataStartFileOffset + (long)DataSizeBytes;
        var ptr             = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(writeFileOffset, shouldGrow: true);
        *ptr             =  entry;
        DataSizeBytes    += (ulong)sizeof(Level1QuoteStruct);
        DataEntriesCount += 1;
        // Logger.Info("Bucket {0} with StartTime {1} wrote EntryNumber: {2} for DataSize {3} at {4} - {5} ",
        //     BucketId, PeriodStartTime, DataEntriesCount, DataSizeBytes, writeFileOffset, entry);
        return StorageAttemptResult.PeriodRangeMatched;
    }
}
