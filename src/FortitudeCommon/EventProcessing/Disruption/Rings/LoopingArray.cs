#region

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public class LoopingArray<T> : IEnumerable<T?> 
{
    protected readonly bool AllowOverwrite;
    internal readonly T[] Cells;
    internal readonly int RingMask;
    internal readonly int RingSize;
    internal long ConsumerCursor;
    internal long PublisherCursor;

    public LoopingArray(int size, bool allowOverwrite = true)
    {
        RingSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        RingMask = RingSize - 1;
        Cells = new T[RingSize];
        AllowOverwrite = allowOverwrite;
    }

    public T this[int index]
    {
        get => Cells[index & RingMask];
        set => Cells[index & RingMask] = value;
    }

    public long Count => PublisherCursor - ConsumerCursor;

    public long Capacity => Cells.Length;

    public IEnumerator<T?> GetEnumerator() => new ReusableRingEnumerator<T>(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int NextPublisherIndex()
    {
        if (PublisherCursor - ConsumerCursor == RingSize)
        {
            if (AllowOverwrite)
                ConsumerCursor++;
            else
                throw new Exception("Capacity reached");
        }

        return (int)(PublisherCursor++ & RingMask);
    }

    public void Clear(long range)
    {
        if (range < 0) throw new ArgumentOutOfRangeException();
        ConsumerCursor += Math.Min(PublisherCursor - ConsumerCursor, range);
    }

    public T? Peek()
    {
        if (PublisherCursor == ConsumerCursor) return default;
        return Cells[(int)ConsumerCursor & RingMask];
    }
}
