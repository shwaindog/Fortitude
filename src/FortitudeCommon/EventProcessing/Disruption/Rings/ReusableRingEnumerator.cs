#region

using System.Collections;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public class ReusableRingEnumerator<T> : IEnumerator<T?>
{
    private readonly LoopingArray<T> theRing;
    private long enumeratorCurrentPosition;

    public ReusableRingEnumerator(LoopingArray<T> theRing)
    {
        this.theRing = theRing;
        enumeratorCurrentPosition = theRing.ConsumerCursor;
        Current = default;
    }

    public void Dispose() { }

    public bool MoveNext()
    {
        if (enumeratorCurrentPosition < theRing.PublisherCursor)
        {
            Current = theRing.Cells[(int)enumeratorCurrentPosition & theRing.RingMask];
            enumeratorCurrentPosition++;
            return true;
        }

        return false;
    }

    public void Reset()
    {
        enumeratorCurrentPosition = theRing.ConsumerCursor;
    }

    object? IEnumerator.Current => Current;

    public T? Current { get; private set; }
}
