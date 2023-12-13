#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public interface IAutoRecycleUnfoldingEnumerator : IAutoRecycledObject, IEnumerator
{
    IAutoRecycleUnfoldingEnumerator Add(IEnumerator append);
    IAutoRecycleUnfoldingEnumerator AddRange(IEnumerable<IEnumerator> appendAll);
    IAutoRecycleUnfoldingEnumerator Remove(IEnumerator remove);
    IAutoRecycleUnfoldingEnumerator Clear();
}

public class AutoRecycleUnfoldingEnumerator : AutoRecycledObject, IAutoRecycleUnfoldingEnumerator
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(AutoRecycleUnfoldingEnumerator));

    private readonly List<IEnumerator> backingList = new();
    private int currentEnumeratorIndex;

    public IAutoRecycleUnfoldingEnumerator Add(IEnumerator append)
    {
        backingList.Add(append);
        return this;
    }

    public IAutoRecycleUnfoldingEnumerator AddRange(IEnumerable<IEnumerator> appendAll)
    {
        backingList.AddRange(appendAll);
        return this;
    }

    public IAutoRecycleUnfoldingEnumerator Remove(IEnumerator remove)
    {
        backingList.Remove(remove);
        return this;
    }

    public IAutoRecycleUnfoldingEnumerator Clear()
    {
        backingList.Clear();
        return this;
    }

    public bool MoveNext()
    {
        var canMoveNext = false;
        try
        {
            if (currentEnumeratorIndex < backingList.Count)
            {
                var currentEnum = backingList[currentEnumeratorIndex];
                canMoveNext = currentEnum.MoveNext();
                if (canMoveNext)
                {
                    Current = currentEnum.Current;
                }
                else
                {
                    if (++currentEnumeratorIndex < backingList.Count)
                    {
                        currentEnum = backingList[currentEnumeratorIndex];
                        canMoveNext = currentEnum.MoveNext();
                        if (canMoveNext) Current = currentEnum.Current;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            logger.Warn("Caught index no longer accessible at {0} on list [{1}]. {2}", currentEnumeratorIndex
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
        Current = null;
        currentEnumeratorIndex = 0;
    }

    public object? Current { get; private set; }

    public override void DisableAutoRecycle()
    {
        for (var i = 0; i < backingList.Count; i++)
        {
            var item = backingList[i];
            if (item is IAutoRecycleEnumerator autoRecycleEnumerator) autoRecycleEnumerator.DisableAutoRecycle();
        }

        AutoRecycleAtRefCountZero = false;
    }

    public override void EnableAutoRecycle()
    {
        for (var i = 0; i < backingList.Count; i++)
        {
            var item = backingList[i];
            if (item is IAutoRecycleEnumerator autoRecycleEnumerator) autoRecycleEnumerator.EnableAutoRecycle();
        }

        AutoRecycleAtRefCountZero = true;
    }

    public override void StateReset()
    {
        currentEnumeratorIndex = 0;
        backingList.Clear();
        base.StateReset();
    }

    public void Dispose()
    {
        StateReset();
        Recycle();
    }
}

public interface IAutoRecycleUnfoldingEnumerator<T> : IAutoRecycledObject, IEnumerator<T>
{
    IAutoRecycleUnfoldingEnumerator<T> Add(IEnumerator<T> append);
    IAutoRecycleUnfoldingEnumerator<T> AddRange(IEnumerable<IEnumerator<T>> appendAll);
    IAutoRecycleUnfoldingEnumerator<T> Remove(IEnumerator<T> remove);
    IAutoRecycleUnfoldingEnumerator<T> Clear();
}

public class AutoRecycleUnfoldingEnumerator<T> : AutoRecycledObject, IAutoRecycleUnfoldingEnumerator<T>
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(AutoRecycleUnfoldingEnumerator<T>));

    private readonly List<IEnumerator<T>> backingList = new();
    private int currentEnumeratorIndex;

    public IAutoRecycleUnfoldingEnumerator<T> Add(IEnumerator<T> append)
    {
        backingList.Add(append);
        return this;
    }

    public IAutoRecycleUnfoldingEnumerator<T> AddRange(IEnumerable<IEnumerator<T>> appendAll)
    {
        backingList.AddRange(appendAll);
        return this;
    }

    public IAutoRecycleUnfoldingEnumerator<T> Remove(IEnumerator<T> remove)
    {
        backingList.Remove(remove);
        return this;
    }

    public IAutoRecycleUnfoldingEnumerator<T> Clear()
    {
        backingList.Clear();
        return this;
    }

    object? IEnumerator.Current => Current;

    public T Current { get; private set; } = default!;

    public bool MoveNext()
    {
        var canMoveNext = false;
        try
        {
            while (!canMoveNext && currentEnumeratorIndex < backingList.Count)
            {
                var currentEnum = backingList[currentEnumeratorIndex];
                canMoveNext = currentEnum.MoveNext();
                if (canMoveNext)
                    Current = currentEnum.Current;
                else
                    ++currentEnumeratorIndex;
            }
        }
        catch (Exception ex)
        {
            logger.Warn("Caught index no longer accessible at {0} on list [{1}]. {2}", currentEnumeratorIndex
                , string.Join(", ", backingList), ex);
            canMoveNext = false;
        }

        if (!canMoveNext)
        {
            if (AutoRecycleAtRefCountZero)
            {
                DecrementRefCount();
            }
            else
            {
                Current = default!;
                currentEnumeratorIndex = 0;
            }
        }

        return canMoveNext;
    }

    public void Reset()
    {
        Current = default!;
        currentEnumeratorIndex = 0;
    }


    public void Dispose()
    {
        StateReset();
        Recycle();
    }

    public override void DisableAutoRecycle()
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < backingList.Count; i++)
            if (backingList[i] is IAutoRecycleEnumerator<T> autoRecycleEnumerator)
                autoRecycleEnumerator.DisableAutoRecycle();

        AutoRecycleAtRefCountZero = false;
    }

    public override void EnableAutoRecycle()
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < backingList.Count; i++)
            if (backingList[i] is IAutoRecycleEnumerator<T> autoRecycleEnumerator)
                autoRecycleEnumerator.EnableAutoRecycle();

        AutoRecycleAtRefCountZero = true;
    }


    public override void StateReset()
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        for (var i = 0; i < backingList.Count; i++)
            if (backingList[i] is IAutoRecycleEnumerator<T> { IsInRecycler: false } autoRecycleEnumerator)
            {
                autoRecycleEnumerator.EnableAutoRecycle();
                autoRecycleEnumerator.DecrementRefCount();
            }

        currentEnumeratorIndex = 0;
        backingList.Clear();
        base.StateReset();
    }
}
