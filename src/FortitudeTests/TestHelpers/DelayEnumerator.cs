#region

using System.Collections;

#endregion

namespace FortitudeTests.TestHelpers;

public class DelayEnumerator<T> : IEnumerator<T>
{
    private readonly IEnumerable<T> delayActualEnumerator;
    private IEnumerator<T>? realizedEnumerator;

    public DelayEnumerator(IEnumerable<T> delayActualEnumerator) => this.delayActualEnumerator = delayActualEnumerator;

    private IEnumerator<T> Enum => realizedEnumerator ??= delayActualEnumerator.GetEnumerator();

    public void Dispose()
    {
        Enum.Dispose();
    }

    public bool MoveNext() => Enum.MoveNext();

    public void Reset()
    {
        Enum.Reset();
    }

    object IEnumerator.Current => Current!;

    public T Current => Enum.Current;
}
