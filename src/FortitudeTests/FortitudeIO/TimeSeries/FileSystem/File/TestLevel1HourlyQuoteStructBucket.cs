#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.Memory;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
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

    public override IEnumerable<Level1QuoteStruct> AllBucketEntriesFrom(long? fileCursorOffset = null)
    {
        if (!IsOpen) return Enumerable.Empty<Level1QuoteStruct>();
        // Logger.Info("Bucket {0} at {1} with HeaderView base {2} and required {3} attempt read of DataSize {4} at {5}",
        //     BucketId, FileCursorOffset, BucketHeaderFileView!.LowerViewFileCursorOffset, RequiredHeaderViewFileCursorOffset, DataSizeBytes
        //     , fileCursorOffset ?? BucketDataStartFileOffset);

        var endOffSet = BucketDataStartFileOffset + (long)DataSizeBytes;
        var returnResults = new List<Level1QuoteStruct>();
        var readFileOffset = fileCursorOffset ?? BucketDataStartFileOffset;
        var readEntryCount = 1;
        while (readFileOffset < endOffSet)
        {
            var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(readFileOffset);
            var checkEntry = *ptr;
            returnResults.Add(checkEntry);
            readFileOffset += sizeof(Level1QuoteStruct);
            Logger.Info("Bucket {0} with StartTime {1} read EntryNum: {2} at {3}- {4} ", BucketId, PeriodStartTime, readEntryCount++
                , readFileOffset, checkEntry);
        }

        return returnResults;
    }

    public override IEnumerable<TM> EntriesBetween<TM>(long fileCursorOffset, IMessageDeserializer<TM> usingMessageDeserializer
        , DateTime? fromTime = null
        , DateTime? toTime = null) =>
        throw new NotImplementedException();

    public override int CopyTo(List<Level1QuoteStruct> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null)
    {
        if (!IsOpen) return 0;
        var readFileOffset = BucketDataStartFileOffset;
        var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(readFileOffset);
        var count = 0;
        while (readFileOffset < BucketDataStartFileOffset + (long)DataSizeBytes)
        {
            var checkEntry = *ptr;
            if (EntryIntersects(checkEntry))
            {
                count++;
                destination.Add(checkEntry);
            }
        }

        return count;
    }

    public override StorageAttemptResult AppendEntry(Level1QuoteStruct entry)
    {
        if (!Writable || !IsOpen || !BucketFlags.HasBucketCurrentAppendingFlag()) return StorageAttemptResult.BucketClosedForAppend;
        var entryStorageTime = entry.StorageTime(StorageTimeResolver);
        var checkWithinRange = CheckTimeSupported(entryStorageTime);
        if (checkWithinRange != StorageAttemptResult.PeriodRangeMatched) return checkWithinRange;
        var writeFileOffset = BucketDataStartFileOffset + (long)DataSizeBytes;
        var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(writeFileOffset, shouldGrow: true);
        *ptr = entry;
        DataSizeBytes += (ulong)sizeof(Level1QuoteStruct);
        DataEntriesCount += 1;
        Logger.Info("Bucket {0} with StartTime {1} wrote EntryNumber: {2} for DataSize {3} at {4} - {5} ",
            BucketId, PeriodStartTime, DataEntriesCount, DataSizeBytes, writeFileOffset, entry);
        return StorageAttemptResult.PeriodRangeMatched;
    }
}
