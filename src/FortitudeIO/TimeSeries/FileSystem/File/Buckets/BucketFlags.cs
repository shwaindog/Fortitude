namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[Flags]
public enum BucketFlags : ushort
{
    None = 0
    , IsHighestFileOffsetBucket = 1
    , BucketClosedGracefully = 2
    , BucketCurrentAppending = 4
    , BucketClosedForReading = 8
    , FixedEntrySizeBucket = 16
    , HasBucketStartDelimiter = 32
    , HasBucketEndDelimiter = 64
    , NoInterBucketPadding = 128
}
