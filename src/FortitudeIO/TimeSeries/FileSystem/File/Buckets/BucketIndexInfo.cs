// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Extensions;
using static FortitudeIO.TimeSeries.FileSystem.File.Buckets.BucketHeader;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketIndexInfo
{
    public BucketIndexInfo(uint bucketId, DateTime bucketPeriodStart, BucketFlags bucketFlags, TimeSeriesPeriod bucketPeriod, long parentOrFileOffset)
    {
        if (bucketPeriodStart.Ticks % LowestBucketGranularityTickDivisor > 0)
            throw new ArgumentException(
                                        $"To keep timeseries files efficient bucket start times be whole divisible by " +
                                        $"{TimeSpan.FromTicks(LowestBucketGranularityTickDivisor).TotalMinutes} mins.");
        BucketId           = bucketId;
        BucketPeriodStart  = (uint)(bucketPeriodStart.Ticks / LowestBucketGranularityTickDivisor);
        BucketFlags        = bucketFlags;
        BucketPeriod       = bucketPeriod;
        ParentOrFileOffset = parentOrFileOffset;
    }

    public  uint BucketId;
    public  uint NumEntries;
    private uint BucketPeriodStart;

    public TimeSeriesPeriod BucketPeriod;
    public BucketFlags      BucketFlags;

    public long ParentOrFileOffset;

    public DateTime BucketStartTime
    {
        get => (BucketPeriodStart * LowestBucketGranularityTickDivisor).CappedTicksToDateTime();
        set => BucketPeriodStart = (uint)(value.Ticks / LowestBucketGranularityTickDivisor);
    }

    public TimeSeriesPeriodRange TimeSeriesPeriodRange => new(BucketStartTime, BucketPeriod);

    public override string ToString() =>
        $"{nameof(BucketIndexInfo)}({nameof(BucketId)}: {BucketId}, {nameof(NumEntries)}: {NumEntries}, " +
        $"{nameof(BucketPeriod)}: {BucketPeriod}, {nameof(BucketFlags)}: {BucketFlags}, " +
        $"{nameof(ParentOrFileOffset)}: {ParentOrFileOffset}, {nameof(BucketStartTime)}: {BucketStartTime})";
}

public class BucketIndexEarliestEntryComparer : IComparer<BucketIndexInfo>
{
    public int Compare(BucketIndexInfo lhs, BucketIndexInfo rhs)
    {
        if (lhs.BucketStartTime > rhs.BucketStartTime) return 1;
        if (lhs.BucketStartTime < rhs.BucketStartTime) return -1;
        return 0;
    }
}

public static class BucketIndexExtensions
{
    public static bool Intersects(this BucketIndexInfo bucketIndexInfo, DateTime? fromTime = null, DateTime? toTime = null) =>
        (bucketIndexInfo.BucketStartTime < toTime || (toTime == null && fromTime != null))
     && (bucketIndexInfo.BucketPeriod.PeriodEnd(bucketIndexInfo.BucketStartTime) > fromTime || (fromTime == null && toTime != null));

    public static bool Intersects(this BucketIndexInfo bucketIndexInfo, UnboundedTimeRange? periodRange)
    {
        if (periodRange == null) return true;
        var range = periodRange.Value;
        return (bucketIndexInfo.BucketStartTime < range.ToTime || (range.ToTime == null && range.FromTime != null))
            && (bucketIndexInfo.BucketPeriod.PeriodEnd(bucketIndexInfo.BucketStartTime) > range.FromTime
             || (range.FromTime == null && range.ToTime != null));
    }
}
