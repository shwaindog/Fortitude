#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public class ReusableListEnumerator : ReusableObject, IEnumerator
{
    private IList? backingList;
    private int currentIndex = -1;

    public IList? BackingList
    {
        get => backingList;
        set
        {
            if (value == backingList)
            {
                currentIndex = -1;
                return;
            }

            backingList = value;
        }
    }

    public bool MoveNext()
    {
        var canMoveNext = ++currentIndex < (backingList?.Count ?? 0);
        if (!canMoveNext) Recycle();
        return canMoveNext;
    }

    public object? Current => backingList?[currentIndex];

    public override void Reset()
    {
        currentIndex = -1;
        backingList = null;
        base.Reset();
    }
}

public interface IReusableEnumerator<T> : IReusableObject<IReusableEnumerator<T>>, IEnumerator<T>
    where T : class
{
    new void Reset();
}

public class ReusableEnumerator<T> : ReusableObject<IReusableEnumerator<T>>, IReusableEnumerator<T>
    where T : class
{
    private List<T>? backingList;
    private int currentIndex = -1;

    public ReusableEnumerator() { }

    private ReusableEnumerator(ReusableEnumerator<T> toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public List<T>? BackingList
    {
        get => backingList;
        set
        {
            if (value == backingList)
            {
                currentIndex = -1;
                return;
            }

            backingList = value;
        }
    }

    public override IReusableEnumerator<T> Clone() =>
        Recycler?.Borrow<ReusableEnumerator<T>>().CopyFrom(this) ?? new ReusableEnumerator<T>(this);

    public bool MoveNext()
    {
        var canMoveNext = ++currentIndex < (backingList?.Count ?? 0);
        if (!canMoveNext) Recycle();
        return canMoveNext;
    }


    object IEnumerator.Current => Current;

    public void Dispose()
    {
        AutoRecycleAtRefCountZero = false;
        Recycle();
    }

    public T Current => backingList![currentIndex];

    public override void Reset()
    {
        currentIndex = -1;
        backingList = null;
        base.Reset();
    }


    public override ReusableEnumerator<T> CopyFrom(IReusableEnumerator<T> source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ReusableEnumerator<T> reusableEnumerator)
        {
            reusableEnumerator.backingList = reusableEnumerator.BackingList;
            currentIndex = reusableEnumerator.currentIndex;
        }

        return this;
    }
}
