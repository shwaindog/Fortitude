// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public interface IReusableList<T> : IReusableObject<IReusableList<T>>, IList<T>, IReadOnlyCollection<T>
{
    new int Count { get; }

    IReusableList<T> AddRange(IEnumerable<T> addAll);

    void Sort();
    void Sort(Comparison<T> comparison);
    void ShiftToEnd(int indexToBeAtEnd);
}

public class ReusableList<T> : ReusableObject<IReusableList<T>>, IReusableList<T>, IReadOnlyList<T>
{
    private readonly List<T> backingList;

    public ReusableList() => backingList = new List<T>();

    public ReusableList(IRecycler recycler, int size = 16)
    {
        Recycler    = recycler;
        backingList = new List<T>(size);
    }

    private ReusableList(ReusableList<T> toClone)
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

    public void Add(T item)
    {
        backingList.Add(item);
    }

    public IReusableList<T> AddRange(IEnumerable<T> addAll)
    {
        backingList.AddRange(addAll);
        return this;
    }

    public void Clear()
    {
        backingList.Clear();
    }

    public bool Contains(T item) => backingList.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
    {
        backingList.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item) => backingList.Remove(item);

    public int  Count      => backingList.Count;
    public bool IsReadOnly => false;

    public int IndexOf(T item) => backingList.IndexOf(item);

    public void Insert(int index, T item)
    {
        backingList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        backingList.RemoveAt(index);
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
        set => backingList[index] = value;
    }

    public override IReusableList<T> CopyFrom
    (IReusableList<T> source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        backingList.Clear();
        backingList.AddRange(source);
        return this;
    }

    public override void StateReset()
    {
        backingList.Clear();
        base.StateReset();
    }

    public override IReusableList<T> Clone() => Recycler?.Borrow<ReusableList<T>>().CopyFrom(this) ?? new ReusableList<T>(this);

    public IAutoRecycleEnumerator<T> GetEnumerator()
    {
        var reusableEnumerator = Recycler?.Borrow<AutoRecycleEnumerator<T>>() ?? new AutoRecycleEnumerator<T>();
        reusableEnumerator.AddRange(backingList);
        return reusableEnumerator;
    }

    public override string ToString() => $"ReusableList<{typeof(T).Name}>({backingList.JoinToString()})";
}
