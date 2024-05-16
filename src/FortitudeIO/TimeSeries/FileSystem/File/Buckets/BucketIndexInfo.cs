#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct BucketIndexInfo
{
    public BucketIndexInfo(uint bucketId, DateTime bucketPeriodStart, BucketFlags bucketFlags, TimeSeriesPeriod bucketPeriod, long parentOrFileOffset)
    {
        BucketId = bucketId;
        BucketPeriodStart = bucketPeriodStart.Ticks;
        BucketFlags = bucketFlags;
        BucketPeriod = bucketPeriod;
        ParentOrFileOffset = parentOrFileOffset;
    }

    public uint BucketId;
    public TimeSeriesPeriod BucketPeriod;
    public BucketFlags BucketFlags;
    private long BucketPeriodStart;
    public long ParentOrFileOffset;

    public DateTime BucketStartTime
    {
        get => DateTime.FromBinary(BucketPeriodStart);
        set => BucketPeriodStart = value.Ticks;
    }
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
