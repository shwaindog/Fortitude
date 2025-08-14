using System.Collections;
using FortitudeCommon.AsyncProcessing;

namespace FortitudeCommon.DataStructures.Lists;

public class LockReleasingEnumerator<T> : IEnumerator<T>
{
    private IEnumerator<T> protectedEnumerator;

    private ISyncLock lockToRelease;

    public LockReleasingEnumerator(ISyncLock lockToRelease, IEnumerator<T> protectedEnumerator)
    {
        this.lockToRelease       = lockToRelease;
        this.protectedEnumerator = protectedEnumerator;
    }

    object? IEnumerator.Current => protectedEnumerator.Current;

    public T            Current => protectedEnumerator.Current;

    public void Dispose()
    {
        lockToRelease.Release(true);
    }

    public bool MoveNext() => protectedEnumerator.MoveNext();

    public void Reset()
    {
        protectedEnumerator.Reset();
    }
}
