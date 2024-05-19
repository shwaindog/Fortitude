namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[Flags]
public enum BucketFlags : ushort
{
    None = 0
    , IsHighestSibling = 1
    , IsLastPossibleSibling = 2
    , BucketClosedGracefully = 4
    , BucketCurrentAppending = 8
    , BucketClosedForReading = 16
    , HasBucketIndex = 32
    , HasOnlySubBuckets = 64 // else is DataBucket
    , BufferZipCompressed = 128
}

public static class BucketFlagsExtensions
{
    public static bool HasIsHighestFileOffsetBucketFlag(this BucketFlags flags) => (flags & BucketFlags.IsHighestSibling) > 0;
    public static bool HasIsLastPossibleSiblingFlag(this BucketFlags flags) => (flags & BucketFlags.IsLastPossibleSibling) > 0;
    public static bool HasBucketClosedGracefullyFlag(this BucketFlags flags) => (flags & BucketFlags.BucketClosedGracefully) > 0;
    public static bool HasBucketCurrentAppendingFlag(this BucketFlags flags) => (flags & BucketFlags.BucketCurrentAppending) > 0;
    public static bool HasBucketClosedForReadingFlag(this BucketFlags flags) => (flags & BucketFlags.BucketClosedForReading) > 0;
    public static bool HasFixedEntrySizeBucketFlag(this BucketFlags flags) => (flags & BucketFlags.HasBucketIndex) > 0;
    public static bool HasOnlySubBucketsFlag(this BucketFlags flags) => (flags & BucketFlags.HasOnlySubBuckets) > 0;
    public static bool HasBufferZipCompressedFlag(this BucketFlags flags) => (flags & BucketFlags.BufferZipCompressed) > 0;

    public static BucketFlags Unset(this BucketFlags flags, BucketFlags toUnset) => flags & ~toUnset;
}
