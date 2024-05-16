#region

using FortitudeCommon.Monitoring.Logging;
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

    public TestLevel1HourlyQuoteStructBucket(IBucketTrackingTimeSeriesFile containingTimeSeriesFile, long bucketFileCursorOffset, bool writable) :
        base(containingTimeSeriesFile, bucketFileCursorOffset, writable) { }

    public override TimeSeriesPeriod BucketPeriod => TimeSeriesPeriod.OneHour;

    public override IEnumerable<Level1QuoteStruct> AllEntries
    {
        get
        {
            if (!IsOpen) return Enumerable.Empty<Level1QuoteStruct>();
            var endOffSet = EntriesBufferFileOffset + SerializedEntriesBytes;
            var returnResults = new List<Level1QuoteStruct>();
            var readFileOffset = EntriesBufferFileOffset;
            var readEntryCount = 1;
            while (readFileOffset < endOffSet)
            {
                var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(readFileOffset);
                var checkEntry = *ptr;
                returnResults.Add(checkEntry);
                readFileOffset += sizeof(Level1QuoteStruct);
                Logger.Info("Bucket {0} with StartTime {1} read EntryNum: {2} at {3}- {4} ", BucketId, BucketPeriodStart, readEntryCount++
                    , readFileOffset, checkEntry);
            }

            return returnResults;
        }
    }

    public override IEnumerable<TM> EntriesBetween<TM>(IMessageDeserializer<TM> usingMessageDeserializer, DateTime? fromDateTime = null
        , DateTime? toDateTime = null) =>
        throw new NotImplementedException();

    public override int CopyTo(List<Level1QuoteStruct> destination, DateTime? fromDateTime = null, DateTime? toDateTime = null)
    {
        if (!IsOpen) return 0;
        var readFileOffset = EntriesBufferFileOffset;
        var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(readFileOffset);
        var count = 0;
        while (readFileOffset < EntriesBufferFileOffset + SerializedEntriesBytes)
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
        var writeFileOffset = EntriesBufferFileOffset + SerializedEntriesBytes;
        var ptr = (Level1QuoteStruct*)BucketAppenderFileView!.FileCursorBufferPointer(writeFileOffset, true);
        *ptr = entry;
        SerializedEntriesBytes += sizeof(Level1QuoteStruct);
        EntryCount += 1;
        Logger.Info("Bucket {0} with StartTime {1} wrote EntryNumber: {2} at {3} - {4} ", BucketId, BucketPeriodStart, EntryCount, writeFileOffset
            , entry);
        return StorageAttemptResult.PeriodRangeMatched;
    }
}
