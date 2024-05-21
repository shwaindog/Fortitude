#region

using System.Runtime.InteropServices;
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
        BucketId = bucketId;
        BucketPeriodStart = (uint)(bucketPeriodStart.Ticks / LowestBucketGranularityTickDivisor);
        BucketFlags = bucketFlags;
        BucketPeriod = bucketPeriod;
        ParentOrFileOffset = parentOrFileOffset;
    }

    public uint BucketId;
    public uint NumEntries;
    private uint BucketPeriodStart;
    public TimeSeriesPeriod BucketPeriod;
    public BucketFlags BucketFlags;
    public ulong DataSizeBytes;
    public long ParentOrFileOffset;
    public ushort NumIndexEntries;

    public DateTime BucketStartTime
    {
        get => DateTime.FromBinary(BucketPeriodStart * LowestBucketGranularityTickDivisor);
        set => BucketPeriodStart = (uint)(value.Ticks / LowestBucketGranularityTickDivisor);
    }

    public override string ToString() =>
        $"{nameof(BucketIndexInfo)}({nameof(BucketId)}: {BucketId}, {nameof(NumEntries)}: {NumEntries}, {nameof(BucketPeriod)}: {BucketPeriod}, " +
        $"{nameof(BucketFlags)}: {BucketFlags}, {nameof(DataSizeBytes)}: {DataSizeBytes}, " +
        $"{nameof(ParentOrFileOffset)}: {ParentOrFileOffset}, {nameof(NumIndexEntries)}: {NumIndexEntries}, " +
        $"{nameof(BucketStartTime)}: {BucketStartTime})";
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
}
