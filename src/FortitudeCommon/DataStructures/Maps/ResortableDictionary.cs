using System.Collections;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.DataStructures.Maps;

public interface IResortableDictionary<TK, TV> : IReusableObject<IResortableDictionary<TK, TV>>, IDictionary<TK, TV>, IReadOnlyList<TV>
{
    ISortOrderComparer<TV> SortComparer { get; set; }

    IResortableDictionary<TK, TV> ForceSort();

    int IndexOf(TK key);

    new int Count { get; }

    new IEnumerator<KeyValuePair<TK, TV>> GetEnumerator();
}

public class ResortableDictionary<TK, TV> : ReusableObject<IResortableDictionary<TK, TV>>, IResortableDictionary<TK, TV>
  , IComparer<KeyValuePair<TK, TV>>
{
    private readonly List<KeyValuePair<TK, TV>> sortedKeyValuePairs = new();

    private ISortOrderComparer<TV> sortComparer;

    public ResortableDictionary()
    {
        sortComparer = null!;
    }

    public ResortableDictionary(IResortableDictionary<TK, TV> toClone)
    {
        sortComparer = toClone.SortComparer;
        sortedKeyValuePairs.AddRange(toClone);
    }

    public ResortableDictionary<TK, TV> WithSortComparer(ISortOrderComparer<TV> sorter)
    {
        SortComparer = sorter;

        return this;
    }

    public void Add(TK key, TV value)
    {
        var index = IndexOf(key);
        if (index >= 0)
        {
            sortedKeyValuePairs.RemoveAt(index);
        }
        index = InsertIndexOfValue(value);
        if (index < sortedKeyValuePairs.Count)
        {
            sortedKeyValuePairs.Insert(index, new KeyValuePair<TK, TV>(key, value));
        }
        else
        {
            sortedKeyValuePairs.Add(new KeyValuePair<TK, TV>(key, value));
        }
    }

    public void Add(KeyValuePair<TK, TV> item)
    {
        var index = IndexOf(item.Key);
        if (index >= 0)
        {
            sortedKeyValuePairs.RemoveAt(index);
        }
        index = InsertIndexOfValue(item.Value);
        if (index < sortedKeyValuePairs.Count)
        {
            sortedKeyValuePairs.Insert(index, item);
        }
        else
        {
            sortedKeyValuePairs.Add(item);
        }
    }

    public void Clear()
    {
        sortedKeyValuePairs.Clear();
    }

    public bool Contains(KeyValuePair<TK, TV> item) => sortedKeyValuePairs.Any(kvp => Equals(kvp.Key, item.Key) && Equals(kvp.Value, item.Value));

    public bool ContainsKey(TK key) => sortedKeyValuePairs.Any(kvp => Equals(kvp.Key, key));

    public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
    {
        var myIndex = 0;
        for (int i = arrayIndex; i < array.Length && myIndex < sortedKeyValuePairs.Count ; i++)
        {
            array[i] = sortedKeyValuePairs[myIndex++];
        }
    }

    public int Count => sortedKeyValuePairs.Count;

    public bool IsReadOnly => false;

    public TV this[int index] => sortedKeyValuePairs[index].Value;

    public TV this[TK key]
    {
        get
        {
            var index = IndexOf(key);
            return sortedKeyValuePairs[index].Value;
        }
        set
        {
            var index = IndexOf(key);
            if (index >= 0)
            {
                sortedKeyValuePairs.RemoveAt(index);
            }
            index = InsertIndexOfValue(value);
            if (index < sortedKeyValuePairs.Count)
            {
                sortedKeyValuePairs.Insert(index, new KeyValuePair<TK, TV>(key, value));
            }
            else
            {
                sortedKeyValuePairs.Add(new KeyValuePair<TK, TV>(key, value));
            }
        }
    }

    public ICollection<TK> Keys => sortedKeyValuePairs.Select(kvp => kvp.Key).ToReusableList(Recycler);

    public bool Remove(TK key)
    {
        var index = IndexOf(key);
        if (index < 0) return false;
        sortedKeyValuePairs.RemoveAt(index);
        return true;
    }

    public bool Remove(KeyValuePair<TK, TV> item)
    {
        for (int i = 0; i < sortedKeyValuePairs.Count; i++)
        {
            var checkEntry = sortedKeyValuePairs[i];
            if (Equals(item.Key, checkEntry.Key) && Equals(item.Value, checkEntry.Value))
            {
                sortedKeyValuePairs.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public ISortOrderComparer<TV> SortComparer
    {
        get => sortComparer;
        set
        {
            if (ReferenceEquals(sortComparer, value)) return;
            sortComparer = value;
            sortedKeyValuePairs.Sort(this);
        }
    }

    IResortableDictionary<TK, TV> IResortableDictionary<TK, TV>.ForceSort() => ForceSort();

    public ResortableDictionary<TK, TV> ForceSort()
    {
        sortedKeyValuePairs.Sort(this);
        return this;
    }

    public int Compare(KeyValuePair<TK, TV> lhs, KeyValuePair<TK, TV> rhs)
    {
        return sortComparer.Compare(lhs.Value, rhs.Value);
    }

    public bool TryGetValue(TK key, [MaybeNullWhen(false)] out TV value)
    {
        var index = IndexOf(key);
        if (index < 0)
        {
            value = default;
            return false;
        }
        value = sortedKeyValuePairs[index].Value;
        return true;
    }

    public ICollection<TV> Values => sortedKeyValuePairs.Select(kvp => kvp.Value).ToReusableList(Recycler);

    public int IndexOf(TK key)
    {
        for (int i = 0; i < sortedKeyValuePairs.Count; i++)
        {
            var checkEntry = sortedKeyValuePairs[i];
            if (Equals(key, checkEntry.Key))
            {
                return i;
            }
        }
        return -1;
    }

    private int InsertIndexOfValue(TV value)
    {
        for (int i = 0; i < sortedKeyValuePairs.Count; i++)
        {
            var checkEntry = sortedKeyValuePairs[i];
            if (sortComparer.Compare(value, checkEntry.Value) > 0)
            {
                return i;
            }
        }
        return sortedKeyValuePairs.Count;
    }

    public override void StateReset()
    {
        sortComparer = null!;
        sortedKeyValuePairs.Clear();
        base.StateReset();
    }

    public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => sortedKeyValuePairs.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    IEnumerator<TV> IEnumerable<TV>.GetEnumerator() => Values.GetEnumerator();

    public override IResortableDictionary<TK, TV> Clone() => 
        Recycler?.Borrow<ResortableDictionary<TK, TV>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new ResortableDictionary<TK, TV>(this);

    public override ResortableDictionary<TK, TV> CopyFrom(IResortableDictionary<TK, TV> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        sortedKeyValuePairs.Clear();
        sortedKeyValuePairs.AddRange(source);

        return this;
    }
}
