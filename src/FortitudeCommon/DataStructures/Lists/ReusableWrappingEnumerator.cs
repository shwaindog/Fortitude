#region

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public class ReusableWrappingEnumerator<T> : AutoRecycledObject, IEnumerator<T>
{
    public IEnumerator<T>? ProxiedEnumerator { get; set; }

    public bool DisposeRecycles { get; set; } = true;

    public bool MoveNext()
    {
        var canMoveNext = ProxiedEnumerator!.MoveNext();
        if (!canMoveNext)
            if (AutoRecycleAtRefCountZero)
                DecrementRefCount();
            else
                Reset();

        return canMoveNext;
    }

    public void Reset()
    {
        ProxiedEnumerator?.Reset();
    }

    object? IEnumerator.Current => Current;

    public void Dispose()
    {
        if(DisposeRecycles)
        {
            StateReset();
            Recycle();
        }
    }

    public T Current => ProxiedEnumerator!.Current;

    public override void StateReset()
    {
        Reset();
        base.StateReset();
    }
}
