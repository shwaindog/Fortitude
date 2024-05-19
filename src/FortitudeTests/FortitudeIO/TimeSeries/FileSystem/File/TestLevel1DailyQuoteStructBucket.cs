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

    public TestLevel1DailyQuoteStructBucket(IMutableBucketContainer bucketContainer, long bucketFileCursorOffset, bool writable)
        : base(bucketContainer, bucketFileCursorOffset, writable) =>
        IndexCount = 24;

    public override TimeSeriesPeriod TimeSeriesPeriod => TimeSeriesPeriod.OneDay;
}
