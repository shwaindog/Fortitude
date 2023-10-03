#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Lists;

public class ReuseIteratorList<T> : IList<T>
{
    private readonly IList<T> backingList;
    private readonly ReusedEnumerator reusedEnumerator;

    public ReuseIteratorList()
    {
        backingList = new List<T>();
        reusedEnumerator = new ReusedEnumerator(backingList);
    }

    public ReuseIteratorList(int capacity)
    {
        backingList = new List<T>(capacity);
        reusedEnumerator = new ReusedEnumerator(backingList);
    }

    public ReuseIteratorList(IEnumerable<T> collection)
    {
        backingList = new List<T>(collection);
        reusedEnumerator = new ReusedEnumerator(backingList);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<T> GetEnumerator()
    {
        reusedEnumerator.Reset();
        return reusedEnumerator;
    }

    public void Add(T item)
    {
        backingList.Add(item);
    }

    public void Clear()
    {
        backingList.Clear();
    }

    public bool Contains(T item)
    {
        return backingList.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        backingList.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return backingList.Remove(item);
    }

    public int Count => backingList.Count;
    public bool IsReadOnly => false;

    public int IndexOf(T item)
    {
        return backingList.IndexOf(item);
    }

    public void Insert(int index, T item)
    {
        backingList.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        backingList.RemoveAt(index);
    }

    public T this[int index]
    {
        get => backingList[index];
        set => backingList[index] = value;
    }

    private class ReusedEnumerator : IEnumerator<T>
    {
        private readonly IList<T> backingList;
        private int currPos;

        public ReusedEnumerator(IList<T> backingList)
        {
            this.backingList = backingList;
        }

        public void Dispose()
        {
            Reset();
        }

        public bool MoveNext()
        {
            return ++currPos < backingList.Count;
        }

        public void Reset()
        {
            currPos = 0;
        }

        object IEnumerator.Current => Current!;

        public T Current => backingList[currPos];
    }
}
