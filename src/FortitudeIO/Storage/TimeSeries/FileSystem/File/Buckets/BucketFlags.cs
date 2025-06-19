// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeIO.Storage.TimeSeries.FileSystem.File.Buckets;

[Flags]
public enum BucketFlags : ushort
{
    None                   = 0
  , IsHighestSibling       = 1
  , IsLastPossibleSibling  = 2
  , BucketClosedGracefully = 4
  , BucketCurrentAppending = 8
  , BucketClosedForReading = 16
  , HasBucketIndex         = 32
  , HasOnlySubBuckets      = 64 // else is DataBucket
  , CompressedData         = 128
  , CompressedBucketIndex  = 256
}

public static class BucketFlagsExtensions
{
    public static bool HasIsHighestFileOffsetBucketFlag(this BucketFlags flags) => (flags & BucketFlags.IsHighestSibling) > 0;
    public static bool HasIsLastPossibleSiblingFlag(this BucketFlags flags)     => (flags & BucketFlags.IsLastPossibleSibling) > 0;
    public static bool HasBucketClosedGracefullyFlag(this BucketFlags flags)    => (flags & BucketFlags.BucketClosedGracefully) > 0;
    public static bool HasBucketCurrentAppendingFlag(this BucketFlags flags)    => (flags & BucketFlags.BucketCurrentAppending) > 0;
    public static bool HasBucketClosedForReadingFlag(this BucketFlags flags)    => (flags & BucketFlags.BucketClosedForReading) > 0;
    public static bool HasFixedEntrySizeBucketFlag(this BucketFlags flags)      => (flags & BucketFlags.HasBucketIndex) > 0;
    public static bool HasOnlySubBucketsFlag(this BucketFlags flags)            => (flags & BucketFlags.HasOnlySubBuckets) > 0;
    public static bool HasCompressedDataFlag(this BucketFlags flags)            => (flags & BucketFlags.CompressedData) > 0;
    public static bool HasCompressedBucketIndexFlag(this BucketFlags flags)     => (flags & BucketFlags.CompressedBucketIndex) > 0;

    public static BucketFlags Unset(this BucketFlags flags, BucketFlags toUnset) => flags & ~toUnset;
}
