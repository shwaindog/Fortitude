#region

using System.Collections;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public interface IAutoRecycleEnumerable<T> : IAutoRecycledObject, IEnumerable<T>
{
    IList<T> BackingList { get; }
    bool HasItems { get; }
    IAutoRecycleEnumerable<T> Add(T append);
    IAutoRecycleEnumerable<T> AddRange(IEnumerable<T> appendAll);
    IAutoRecycleEnumerable<T> Remove(T remove);
    IAutoRecycleEnumerable<T> Clear();
}

public class AutoRecycledEnumerable<T> : AutoRecycledObject, IAutoRecycleEnumerable<T>
{
    public static readonly IAutoRecycleEnumerable<T> Empty = new EmptyAutoRecycledEnumerable<T>();

    private bool disableEnumeratorRecycle;
    private ReusableList<T>? recycledBackingList;

    public ReusableList<T> ReusableBackingList
    {
        get { return recycledBackingList ??= Recycler?.Borrow<ReusableList<T>>() ?? new ReusableList<T>(); }
    }

    public IList<T> BackingList => ReusableBackingList;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator()
    {
        var autoRecycleEnumerator = ReusableBackingList.GetEnumerator();
        if (disableEnumeratorRecycle) autoRecycleEnumerator.DisableAutoRecycle();
        if (AutoRecycleAtRefCountZero) // be able to switch off auto recycle
            DecrementRefCount();

        return autoRecycleEnumerator;
    }

    public IAutoRecycleEnumerable<T> Add(T item)
    {
        ReusableBackingList.Add(item);
        return this;
    }

    public IAutoRecycleEnumerable<T> AddRange(IEnumerable<T> items)
    {
        ReusableBackingList.AddRange(items);
        return this;
    }

    public IAutoRecycleEnumerable<T> Remove(T remove)
    {
        ReusableBackingList.Remove(remove);
        return this;
    }

    public IAutoRecycleEnumerable<T> Clear()
    {
        ReusableBackingList.Clear();
        return this;
    }

    public bool HasItems => ReusableBackingList.Count > 0;

    public override void StateReset()
    {
        disableEnumeratorRecycle = false;
        recycledBackingList?.DecrementRefCount();
        recycledBackingList = null;
        base.StateReset();
    }

    public void DisableEnumeratorAutoRecycle()
    {
        disableEnumeratorRecycle = true;
    }

    public void EnableEnumeratorAutoRecycle()
    {
        disableEnumeratorRecycle = false;
    }
}

public class EmptyAutoRecycledEnumerable<T> : RecyclableObject, IAutoRecycleEnumerable<T>
{
    // ReSharper disable once CollectionNeverUpdated.Local
    private static readonly IList<T> AlwaysEmptyList = new List<T>().AsReadOnly();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => Enumerable.Empty<T>().GetEnumerator();
    public IList<T> BackingList => AlwaysEmptyList;

    public bool HasItems => false;

    public void DisableAutoRecycle() { }

    public void EnableAutoRecycle() { }

    public IAutoRecycleEnumerable<T> Add(T append) => this;

    public IAutoRecycleEnumerable<T> AddRange(IEnumerable<T> appendAll) => this;

    public IAutoRecycleEnumerable<T> Remove(T remove) => this;

    public IAutoRecycleEnumerable<T> Clear() => this;

    public override int DecrementRefCount() => RefCount;

    public override int IncrementRefCount() => RefCount;

    public override bool Recycle() => false;

    public void DisableEnumeratorAutoRecycle() { }

    public void EnableEnumeratorAutoRecycle() { }
}
