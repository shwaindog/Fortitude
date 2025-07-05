// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public interface IReusableList<T> : IReusableObject<IReusableList<T>>, IMutableCapacityList<T>
{
    new T this[int index] { get; set; }

    new int Count { get; }

    IReusableList<T> AddRange(IEnumerable<T> addAll);

    void Sort();
    void Sort(Comparison<T> comparison);
    void ShiftToEnd(int indexToBeAtEnd);
}

public class ReusableList<T> : ReusableObject<IReusableList<T>>, IReusableList<T>
{
    private readonly List<T> backingList;

    public ReusableList() => backingList = new List<T>();

    public ReusableList(IRecycler recycler, int size = 16)
    {
        Recycler    = recycler;
        backingList = new List<T>(size);
    }

    public ReusableList(ReusableList<T> toClone)
    {
        backingList = new List<T>(toClone.Count);
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    public void Sort()
    {
        backingList.Sort();
    }

    public void Sort(Comparison<T> comparison)
    {
        backingList.Sort(comparison);
    }

    public virtual void Add(T item)
    {
        if (item is IRecyclableObject recyclableObject)
        {
            recyclableObject.IncrementRefCount();
        }
        backingList.Add(item);
    }

    public IReusableList<T> AddRange(IEnumerable<T> addAll)
    {
        foreach (var toAdd in addAll)
        {
            Add(toAdd);
        }
        return this;
    }

    public void Clear()
    {
        foreach (var item in backingList)
        {
            if (item is IRecyclableObject recyclableObj)
            {
                recyclableObj.DecrementRefCount();
            }
        }
        backingList.Clear();
    }

    public bool Contains(T item) => backingList.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
    {
        backingList.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        var result = backingList.Remove(item);
        if (item is IRecyclableObject recyclableObject)
        {
            recyclableObject.DecrementRefCount();
        }
        return result;
    }

    public int  Count
    {
        get => backingList.Count;
        set {  /* no op */ }
    }

    public int  Capacity
    {
        get => backingList.Capacity;
        set => backingList.Capacity = value;
    }

    public bool IsReadOnly => false;

    public int IndexOf(T item) => backingList.IndexOf(item);

    public void Insert(int index, T item)
    {
        if (item is IRecyclableObject recyclableObject)
        {
            recyclableObject.IncrementRefCount();
        }
        backingList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        T? checkForRecycle = default;
        if (index < backingList.Count)
        {
            checkForRecycle = backingList[index];
        }
        backingList.RemoveAt(index);
        if (checkForRecycle is IRecyclableObject recyclable)
        {
            recyclable.DecrementRefCount();
        }
    }

    public void ShiftToEnd(int indexToBeAtEnd)
    {
        var toBeAtEnd = backingList[indexToBeAtEnd];
        for (var i = indexToBeAtEnd; i < backingList.Count; i++)
        {
            var j = i + 1;
            if (j < backingList.Count)
                backingList[i] = backingList[j];
            else
                backingList[i] = toBeAtEnd;
        }
    }

    public T this[int index]
    {
        get => backingList[index];
        set
        {
            T? oldValue = index < backingList.Count ? backingList[index] : default;
            if (ReferenceEquals(oldValue, value)) return;
            if (value is IRecyclableObject valueRecyclable)
            {
                valueRecyclable.IncrementRefCount();
            }
            backingList[index] = value;
            if (oldValue is IRecyclableObject oldRecyclable)
            {
                oldRecyclable.DecrementRefCount();
            }
        }
    }

    public override IReusableList<T> CopyFrom
    (IReusableList<T> source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Clear();
        
        foreach (var item in source)
        {
            if (item is IRecyclableObject recyclableObj)
            {
                recyclableObj.IncrementRefCount();
            }
        }
        backingList.AddRange(source);
        return this;
    }

    public override void StateReset()
    {
        Clear();
        base.StateReset();
    }

    public override IReusableList<T> Clone() => Recycler?.Borrow<ReusableList<T>>().CopyFrom(this) ?? new ReusableList<T>(this);

    public IAutoRecycleEnumerator<T> GetEnumerator()
    {
        var reusableEnumerator = Recycler?.Borrow<AutoRecycleEnumerator<T>>() ?? new AutoRecycleEnumerator<T>();
        reusableEnumerator.AddRange(backingList);
        return reusableEnumerator;
    }

    protected string ReusableListItemsToString => $"[{backingList.JoinToString()}]";
    protected string ReusableListItemsOnNewLineToString => $"[\n\t{backingList.JoinToString(",\n\t")}]";

    public override string ToString() => $"ReusableList<{typeof(T).Name}>[{ReusableListItemsToString}]";
}


public static class ReusableListExtensions
{
    public static ReusableList<T> ToReusableList<T>(this IEnumerable<T> toConvert, IRecycler? optionalRecycler)
    {
        if (toConvert is ReusableList<T> reusableList)
        {
            return reusableList;
        }
        reusableList = optionalRecycler?.Borrow<ReusableList<T>>() ?? new ReusableList<T>();
        reusableList.AddRange(toConvert);
        return reusableList;
    }
}