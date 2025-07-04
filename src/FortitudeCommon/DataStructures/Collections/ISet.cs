#region

using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeCommon.DataStructures.Collections;

public interface IRecyclableSet<T> : ISet<T>, IRecyclableObject, IList<T>
{
}


public class InsertionOrderSet<T> : ReusableObject<InsertionOrderSet<T>>, IRecyclableSet<T>, ICloneable<InsertionOrderSet<T>>
{
    private readonly ReusableList<T> backingList = new();

    public InsertionOrderSet() { }

    public InsertionOrderSet(ISet<T> toClone)
    {
        foreach (var item in toClone)
        {
            Add(item);
        }
    }

    public InsertionOrderSet(InsertionOrderSet<T> toClone)
    {
        backingList = new ReusableList<T>(toClone.backingList);
    }

    public T this[int index]
    {
        get => backingList[index];
        set
        {
            if (backingList.Contains(value)) return;
            backingList[index] = value;
        }
    }

    public int Count => backingList.Count;

    public bool Remove(T item) => backingList.Remove(item);

    public bool Contains(T item) => backingList.Contains(item);

    public bool Add(T item)
    {
        if (backingList.Contains(item)) return false;
        backingList.Add(item);
        return true;
    }

    void ICollection<T>.Add(T item)
    {
        Add(item);
    }

    public void Insert(int index, T item)
    {
        if (backingList.Contains(item)) return;
        backingList.Insert(index, item);
    }

    public void Clear()
    {
        backingList.Clear();
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        backingList.CopyTo(array, arrayIndex);
    }

    public bool IsReadOnly => false;

    public void ExceptWith(IEnumerable<T> other)
    {
        foreach (var removeItem in other) Remove(removeItem);
    }

    public void IntersectWith(IEnumerable<T> other)
    {
        var materialisedOther = Recycler?.Borrow<ReusableList<T>>() ?? new ReusableList<T>();
        materialisedOther.AddRange(other);
        foreach (var checkItem in backingList)
            if (!materialisedOther.Contains(checkItem))
                backingList.Remove(checkItem);

        materialisedOther.DecrementRefCount();
    }

    public bool IsProperSubsetOf(IEnumerable<T> other) => other.Count() != Count && backingList.All(other.Contains);

    public bool IsProperSupersetOf(IEnumerable<T> other) => other.Count() != Count && other.All(Contains);

    public bool IsSubsetOf(IEnumerable<T> other) => backingList.All(other.Contains);

    public bool IsSupersetOf(IEnumerable<T> other) => other.All(Contains);

    public bool Overlaps(IEnumerable<T> other) => other.Any(Contains);

    public bool SetEquals(IEnumerable<T> other) => other.Count() == Count && backingList.All(other.Contains);

    public void SymmetricExceptWith(IEnumerable<T> other)
    {
        var materialisedOther = Recycler?.Borrow<ReusableList<T>>() ?? new ReusableList<T>();
        materialisedOther.AddRange(other);
        foreach (var checkItem in backingList)
            if (materialisedOther.Contains(checkItem))
                backingList.Remove(checkItem);
        foreach (var otherItem in materialisedOther)
            if (!backingList.Contains(otherItem))
                backingList.Add(otherItem);

        materialisedOther.DecrementRefCount();
    }

    public void UnionWith(IEnumerable<T> other)
    {
        foreach (var otherItem in other)
          Add(otherItem);
    }

    public int IndexOf(T item) => backingList.IndexOf(item);

    public bool RemoveAny(IEnumerable<T> items) => items.Any(Remove);

    public void RemoveAt(int index)
    {
        backingList.RemoveAt(index);
    }

    object ICloneable.          Clone() => Clone();

    public override InsertionOrderSet<T> Clone() => 
        Recycler?.Borrow<InsertionOrderSet<T>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new InsertionOrderSet<T>(this);


    public override void StateReset()
    {
        backingList.Clear();
        base.StateReset();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T>   GetEnumerator() => backingList.GetEnumerator();

    public override InsertionOrderSet<T> CopyFrom(InsertionOrderSet<T> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        backingList.Clear();
        backingList.AddRange(source.backingList);

        return this;
    }

    public static InsertionOrderSet<T> operator +(IEnumerable<T>? lhs, InsertionOrderSet<T> rhs)
    {
        if (lhs == null) return rhs;
        rhs.UnionWith(lhs);
        return rhs;
    }

    public static InsertionOrderSet<T> operator +(InsertionOrderSet<T> lhs, IEnumerable<T>? rhs)
    {
        if (rhs == null) return lhs;
        lhs.UnionWith(rhs);
        return lhs;
    }

    public static InsertionOrderSet<T> operator -(InsertionOrderSet<T> lhs, IEnumerable<T>? rhs)
    {
        if (rhs == null) return lhs;
        lhs.RemoveAny(rhs);
        return lhs;
    }
}
