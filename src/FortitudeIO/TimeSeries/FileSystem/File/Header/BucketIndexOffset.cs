#region

using System.Runtime.InteropServices;

#endregion

namespace FortitudeIO.TimeSeries.FileSystem.File.Header;

[StructLayout(LayoutKind.Sequential)]
public struct BucketIndexOffset
{
    public BucketIndexOffset(uint bucketId, DateTime earliestEntryDateTime, BucketIndexFlags bucketIndexFlags, ulong fileOffset, uint bucketSize)
    {
        BucketId = bucketId;
        earliestEntryDateTimeTicks = earliestEntryDateTime.Ticks;
        BucketIndexFlags = bucketIndexFlags;
        BucketSize = bucketSize;
        FileOffset = fileOffset;
    }

    public uint BucketId;
    public uint BucketSize;
    private long earliestEntryDateTimeTicks;
    public ulong FileOffset;
    public BucketIndexFlags BucketIndexFlags;

    public DateTime EarliestEntryDateTime
    {
        get => DateTime.FromBinary(earliestEntryDateTimeTicks);
        set => earliestEntryDateTimeTicks = value.Ticks;
    }
}

public class BucketIndexEarliestEntryComparer : IComparer<BucketIndexOffset>
{
    public int Compare(BucketIndexOffset lhs, BucketIndexOffset rhs)
    {
        if (lhs.EarliestEntryDateTime > rhs.EarliestEntryDateTime) return 1;
        if (lhs.EarliestEntryDateTime < rhs.EarliestEntryDateTime) return -1;
        return 0;
    }
}

[Flags]
public enum BucketIndexFlags : ushort
{
    EmptyEntry = 0
    , Deleted = 0x00_01
    , Corrupt = 0x00_02
    , HadUnexpectedClose = 0x00_04
    , HasData = 0x00_08
    , HasBucketStartDelimiter = 0x00_10
    , HasBucketEndDelimiter = 0x00_20
    , WriterCurrentlyOpened = 0x00_40
    , WriterAppending = 0x00_80
    , ClosedForReading = 0x01_00
    , HasSubBuckets = 0x02_00
    , HasSubBucketIndex = 0x04_00
    , HasSummary = 0x08_00
}
