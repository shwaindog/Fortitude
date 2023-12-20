#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public interface IAutoRecycleEnumerator : IAutoRecycledObject, IEnumerator, IDisposable
{
    IAutoRecycleEnumerator Add(object? append);
    IAutoRecycleEnumerator AddRange(IEnumerable appendAll);
    IAutoRecycleEnumerator Remove(object? remove);
    IAutoRecycleEnumerator Clear();
}

public class AutoRecycleEnumerator : AutoRecycledObject, IAutoRecycleEnumerator
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(AutoRecycleEnumerator));

    private readonly List<object?> backingList = new();
    private int currentIndex = -1;

    public IAutoRecycleEnumerator Add(object? append)
    {
        backingList.Add(append);
        return this;
    }

    public IAutoRecycleEnumerator AddRange(IEnumerable appendAll)
    {
        foreach (var item in appendAll) backingList.Add(item);
        return this;
    }

    public IAutoRecycleEnumerator Remove(object? remove)
    {
        backingList.Remove(remove);
        return this;
    }

    public IAutoRecycleEnumerator Clear()
    {
        backingList.Clear();
        return this;
    }

    public bool MoveNext()
    {
        var canMoveNext = ++currentIndex < backingList.Count;
        try
        {
            if (canMoveNext) Current = backingList[currentIndex];
        }
        catch (Exception ex)
        {
            logger.Warn("Caught index no longer accessible at {0} on list [{1}]. {2}", currentIndex
                , string.Join(", ", backingList), ex);
            canMoveNext = false;
        }

        if (!canMoveNext)
        {
            if (AutoRecycleAtRefCountZero)
                DecrementRefCount();
            else
                Reset();
        }

        return canMoveNext;
    }

    public void Reset()
    {
        currentIndex = -1;
        Current = null;
    }

    public object? Current { get; private set; }

    public override void StateReset()
    {
        currentIndex = -1;
        backingList.Clear();
        base.StateReset();
    }

    public void Dispose()
    {
        StateReset();
        Recycle();
    }
}

public interface IAutoRecycleEnumerator<T> : IAutoRecycledObject, IEnumerator<T>
{
    IAutoRecycleEnumerator<T> Add(T append);
    IAutoRecycleEnumerator<T> AddRange(IEnumerable<T> appendAll);
    IAutoRecycleEnumerator<T> Remove(T remove);
    IAutoRecycleEnumerator<T> Clear();
}

public class AutoRecycleEnumerator<T> : AutoRecycledObject, IAutoRecycleEnumerator<T>
{
    private static readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(AutoRecycleEnumerator<T>));

    private readonly List<T> backingList = new();
    private int currentIndex = -1;

    public IAutoRecycleEnumerator<T> Add(T append)
    {
        backingList.Add(append);
        return this;
    }

    public IAutoRecycleEnumerator<T> AddRange(IEnumerable<T> appendAll)
    {
        backingList.AddRange(appendAll);
        return this;
    }

    public IAutoRecycleEnumerator<T> Remove(T remove)
    {
        backingList.Remove(remove);
        return this;
    }

    public IAutoRecycleEnumerator<T> Clear()
    {
        backingList.Clear();
        return this;
    }

    object? IEnumerator.Current => Current;

    public T Current { get; private set; } = default!;

    public bool MoveNext()
    {
        var canMoveNext = ++currentIndex < backingList.Count;
        try
        {
            if (canMoveNext) Current = backingList[currentIndex];
        }
        catch (Exception ex)
        {
            logger.Warn("Caught index no longer accessible at {0} on list [{1}]. {2}", currentIndex
                , string.Join(", ", backingList), ex);
            canMoveNext = false;
        }

        if (!canMoveNext)
        {
            if (AutoRecycleAtRefCountZero)
                DecrementRefCount();
            else
                Reset();
        }

        return canMoveNext;
    }

    public void Reset()
    {
        currentIndex = -1;
        Current = default!;
    }

    public void Dispose()
    {
        StateReset();
        Recycle();
    }

    public override void StateReset()
    {
        currentIndex = -1;
        backingList.Clear();
        base.StateReset();
    }
}
