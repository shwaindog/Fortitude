#region

using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeIO.TimeSeries.FileSystem.File.Buckets;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeTests.FortitudeIO.TimeSeries.FileSystem.File;

public class TestLevel1DailyQuoteStructBucket :
    SubBucketOnlyBucket<Level1QuoteStruct, TestLevel1DailyQuoteStructBucket, TestLevel1HourlyQuoteStructBucket>
{
    private static BucketFactory<TestLevel1DailyQuoteStructBucket> bucketBucketFactory = new();

    public TestLevel1DailyQuoteStructBucket(IBucketTrackingTimeSeriesFile containingTimeSeriesFile, long bucketFileCursorOffset, bool writable)
        : base(containingTimeSeriesFile, bucketFileCursorOffset, writable) =>
        MaxSubBucketCount = 24;

    public override TimeSeriesPeriod BucketPeriod => TimeSeriesPeriod.OneDay;
}
