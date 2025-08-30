using System.Collections;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.DataStructures.Lists;

public class CappedSizeDroppingAppendList<T> : ReusableObject<CappedSizeDroppingAppendList<T>>, IList<T>
{
    private readonly LinkedList<T> backingList;

    private readonly ISyncLock updateLock = new YieldLockLight();

    public CappedSizeDroppingAppendList() : this(64) { }

    public CappedSizeDroppingAppendList(IRecycler recycler, int maxSize = 16)
    {
        MaxSize     = maxSize;
        Recycler    = recycler;
        backingList = new LinkedList<T>();
    }

    public CappedSizeDroppingAppendList(int maxSize = 64)
    {
        MaxSize     = maxSize;
        backingList = new LinkedList<T>();
    }

    public CappedSizeDroppingAppendList(CappedSizeDroppingAppendList<T> toClone)
    {
        MaxSize     = toClone.MaxSize;
        backingList = new LinkedList<T>();
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public int MaxSize { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    public virtual void Add(T item)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        if (backingList.Count >= MaxSize++)
        {
            if (backingList.First!.Value is IRecyclableObject recyclableFirst)
            {
                recyclableFirst.DecrementRefCount();
            } 
            backingList.RemoveFirst();
        }
        if (item is IRecyclableObject recyclableItem)
        {
            recyclableItem.IncrementRefCount();
        } 
        backingList.AddLast(item);
        syncLock.Release(true);
    }

    public CappedSizeDroppingAppendList<T> AddRange(IEnumerable<T> addAll)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        foreach (var toAdd in addAll)
        {
            if (backingList.Count >= MaxSize++)
            {
                if (backingList.First!.Value is IRecyclableObject recyclableFirst)
                {
                    recyclableFirst.DecrementRefCount();
                } 
                backingList.RemoveFirst();
            }
            if (toAdd is IRecyclableObject recyclableItem)
            {
                recyclableItem.IncrementRefCount();
            } 
            backingList.AddLast(toAdd);
        }
        syncLock.Release(true);
        return this;
    }

    public void Clear()
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        foreach (var item in backingList)
        {
            if (item is IRecyclableObject recyclableItem)
            {
                recyclableItem.DecrementRefCount();
            } 
        }
        backingList.Clear();
        syncLock.Release(true);
    }

    public bool Contains(T item) => backingList.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        backingList.CopyTo(array, arrayIndex);
        syncLock.Release(true);
    }

    public bool Remove(T item)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        var result = backingList.Remove(item);
        if (result && item is IRecyclableObject recyclableItem)
        {
            recyclableItem.DecrementRefCount();
        }
        syncLock.Release(true);
        return result;
    }

    public int Count
    {
        get => backingList.Count;
        set
        { /* no op */
        }
    }

    public bool IsReadOnly => false;

    public int IndexOf(T item)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        int count = 0;
        foreach (var check in backingList)
        {
            if (Equals(item, check))
            {
                syncLock.Release(true);
                return count;
            }
            count++;
        }
        syncLock.Release(true);
        return -1;
    }

    public void Insert(int index, T item)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        int count       = 0;
        var currentNode = backingList.First;
        while (currentNode != null)
        {
            if (count++ == index)
            {
                if (backingList.Count >= MaxSize++)
                {
                    if (currentNode.Value is IRecyclableObject recyclableFirst)
                    {
                        recyclableFirst.DecrementRefCount();
                    } 
                    backingList.RemoveFirst();
                }
                if (item is IRecyclableObject recyclableItem)
                {
                    recyclableItem.IncrementRefCount();
                } 
                backingList.AddBefore(currentNode, new LinkedListNode<T>(item));
                syncLock.Release(true);
                return;
            }

            currentNode = currentNode.Next;
        }
        if (item is IRecyclableObject recyclableLast)
        {
            recyclableLast.IncrementRefCount();
        } 
        backingList.AddLast(item);
        syncLock.Release(true);
    }

    public void RemoveAt(int index)
    {
        using var syncLock = updateLock;
        syncLock.Acquire();
        int count       = 0;
        var currentNode = backingList.First;
        while (currentNode != null)
        {
            if (count++ == index)
            {
                if (currentNode.Value is IRecyclableObject recyclableAt)
                {
                    recyclableAt.DecrementRefCount();
                } 
                backingList.Remove(currentNode);
            }

            currentNode = currentNode.Next;
        }
        syncLock.Release(true);
    }

    public T this[int index]
    {
        get
        {
            using var syncLock = updateLock;
            syncLock.Acquire();
            int count = 0;
            foreach (var check in backingList)
            {
                if (count++ == index) return check;
                count++;
            }
            syncLock.Release(true);
            throw new ArgumentOutOfRangeException($"No entry exists at {index}");
        }
        set => Insert(index, value);
    }

    public override CappedSizeDroppingAppendList<T> CopyFrom(CappedSizeDroppingAppendList<T> source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (ReferenceEquals(this, source)) return this;
        Clear();
        
        using var syncLock = updateLock;
        syncLock.Acquire();
        MaxSize = source.MaxSize;
        foreach (var item in source)
        {
            if (item is IRecyclableObject recyclableLast)
            {
                recyclableLast.IncrementRefCount();
            } 
            backingList.AddLast(item);
        }
        syncLock.Release(true);
        return this;
    }

    public override void StateReset()
    {
        updateLock.Release(true);
        Clear();
        base.StateReset();
    }

    public override CappedSizeDroppingAppendList<T> Clone() =>
        Recycler?.Borrow<CappedSizeDroppingAppendList<T>>().CopyFrom(this)
     ?? new CappedSizeDroppingAppendList<T>(this);

    public IEnumerator<T> GetEnumerator()
    {
        updateLock.Acquire();

        var lockReleasingEnum = new LockReleasingEnumerator<T>(updateLock, backingList.GetEnumerator());
        return lockReleasingEnum;
    }

    protected string CappedSizeDroppingAppendListItemsToString          => $"[{backingList.JoinToString()}]";
    protected string CappedSizeDroppingAppendListItemsOnNewLineToString => $"[\n\t{backingList.JoinToString(",\n\t")}]";

    public override string ToString() => $"CappedSizeDroppingAppendList<{typeof(T).Name}>[{CappedSizeDroppingAppendListItemsToString}]";
}
