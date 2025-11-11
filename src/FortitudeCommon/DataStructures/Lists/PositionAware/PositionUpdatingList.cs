// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.DataStructures.Lists.PositionAware;

public class PositionUpdatingList<T> : RecyclableObject, IList<T>
    where T : IOffsetAwareListItem
{
    private readonly Type?    ownerType;
    private readonly string?  memberName  = "Not-Set";
    private readonly List<T> backingList = new();

    public PositionUpdatingList() { }

    public PositionUpdatingList(Type ownerType,  [System.Runtime.CompilerServices.CallerMemberName] string memberName = "Not-Set")
    {
        this.ownerType  = ownerType;
        this.memberName = memberName;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<T> GetEnumerator() => backingList.GetEnumerator();

    public void Add(T item)
    {
        int atIndex = backingList.Count;
        backingList.Add(item);
        item.AtIndex = atIndex;
        if (item is ICodeLocationAwareListItem codeLocationAwareItem)
        {
            codeLocationAwareItem.ListOwningType = ownerType;
            codeLocationAwareItem.ListMemberName = memberName;
        }
    }

    public void Clear()
    {
        for (int i = 0; i < backingList.Count; i++)
        {
            var item = backingList[i];

            item.AtIndex = -1;
        }
        backingList.Clear();
    }

    public bool Contains(T item) => backingList.Contains(item);

    public void CopyTo(T[] array, int arrayIndex)
    {
        var myIndex = 0;
        for (int i = arrayIndex; i < array.Length; i++) { array[i] = backingList[myIndex++]; }
    }

    public bool Remove(T item)
    {
        var atIndex = backingList.IndexOf(item);
        if (atIndex >= 0)
        {
            for (int i = atIndex + 1; i < backingList.Count; i++)
            {
                var trailingItem = backingList[i];

                trailingItem.AtIndex = i - 1;
            }
            backingList.RemoveAt(atIndex);
            item.AtIndex = -1;
            if (item is ICodeLocationAwareListItem codeLocationAwareItem)
            {
                codeLocationAwareItem.ListOwningType = null;
                codeLocationAwareItem.ListMemberName = "Not-Set";
            }
            return true;
        }
        return false;
    }

    public int Count => backingList.Count;

    public bool IsReadOnly => false;

    public int IndexOf(T item) => backingList.IndexOf(item);

    public void Insert(int index, T item)
    {
        index = Math.Clamp(index, 0, backingList.Count);
        if (index >= backingList.Count)
        {
            Add(item);
            return;
        }
        var updateFrom = index + 1;
        backingList.Insert(index, item);
        item.AtIndex = index;
        if (item is ICodeLocationAwareListItem codeLocationAwareItem)
        {
            codeLocationAwareItem.ListOwningType = ownerType;
            codeLocationAwareItem.ListMemberName = memberName;
        }
        for (int i = updateFrom; i < backingList.Count; i++)
        {
            var trailingItem = backingList[i];

            trailingItem.AtIndex = i;
        }
    }

    public void RemoveAt(int index)
    {
        if (index >= Count || index < 0) return;
        for (int i = index + 1; i < backingList.Count; i++)
        {
            var trailingItem = backingList[i];

            trailingItem.AtIndex = i - 1;
        }
        var removed =  backingList[index];
        removed.AtIndex = -1;
        if (removed is ICodeLocationAwareListItem codeLocationAwareItem)
        {
            codeLocationAwareItem.ListOwningType = null;
            codeLocationAwareItem.ListMemberName = "Not-Set";
        }
        backingList.RemoveAt(index);
    }

    public T this[int index]
    {
        get => backingList[index];
        set
        {
            var oldValue = backingList[index];
            oldValue.AtIndex = -1;
            if (oldValue is ICodeLocationAwareListItem oldCodeLocationAwareItem)
            {
                oldCodeLocationAwareItem.AtIndex = -1;
                oldCodeLocationAwareItem.ListOwningType = null;
                oldCodeLocationAwareItem.ListMemberName = "Not-Set";
            }
            backingList[index] = value;
            value.AtIndex = index;
            if (value is ICodeLocationAwareListItem codeLocationAwareItem)
            {
                codeLocationAwareItem.ListOwningType = ownerType;
                codeLocationAwareItem.ListMemberName = memberName;
            }
        }
    }
}
