using System.Collections;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.DataStructures.Maps;

public interface IRecyclableReadOnlyDictionary<TK, TV> : IRecyclableObject, IReadOnlyDictionary<TK, TV>
{
}

public interface IReusableDictionary<TK, TV> : IReusableObject<IReusableDictionary<TK, TV>>, IDictionary<TK, TV>, IRecyclableReadOnlyDictionary<TK, TV>
{
    new TV this[TK key] { get; set; }

    new int Count { get; }

    new ReusableList<TK> Keys { get; }

    new ReusableList<TV> Values { get; }

    new bool ContainsKey(TK key);

    new bool TryGetValue(TK key, out TV? value);
}

public class ReusableDictionary<TK, TV> : ReusableObject<IReusableDictionary<TK, TV>>, IReusableDictionary<TK, TV> where TK : notnull
{
    private readonly GarbageAndLockFreeMap<TK, TV> backingDictionary = new();

    public ReusableDictionary() { }

    public ReusableDictionary(IEnumerable<KeyValuePair<TK, TV>> toPopulate)
    {
        foreach (var kvp in toPopulate)
        {
            Add(kvp);
        }
    }

    public void Add(KeyValuePair<TK, TV> item)
    {
        if (item.Key is IRecyclableObject keyRecyclable)
        {
            keyRecyclable.IncrementRefCount();
        }
        if (item.Value is IRecyclableObject valueRecyclable)
        {
            valueRecyclable.IncrementRefCount();
        }
        backingDictionary.Add(item.Key, item.Value);
    }

    public void Clear()
    {
        foreach (var kvp in backingDictionary)
        {
            if (kvp.Key is IRecyclableObject keyRecyclable)
            {
                keyRecyclable.DecrementRefCount();
            }
            if (kvp.Value is IRecyclableObject valueRecyclable)
            {
                valueRecyclable.DecrementRefCount();
            }
        }
        backingDictionary.Clear();
    }

    public bool Contains(KeyValuePair<TK, TV> item)
    {
        if (backingDictionary.TryGetValue(item.Key, out var value))
        {
            return Equals(value, item.Value);
        }
        return false;
    }

    public void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
    {
        int i = arrayIndex;
        foreach (var keyValuePair in backingDictionary)
        {
            if (i < array.Length)
            {
                array[i++] = keyValuePair;
            }
            else
            {
                break;
            }
        }
    }

    public bool Remove(KeyValuePair<TK, TV> item)
    {
        TV? checkKeyForRecycle = default;
        if (backingDictionary.ContainsKey(item.Key))
        {
            checkKeyForRecycle = backingDictionary[item.Key];
        }
        var result = backingDictionary.Remove(item.Key);
        if (item.Key is IRecyclableObject keyRecyclable)
        {
            keyRecyclable.DecrementRefCount();
        }
        if (checkKeyForRecycle is IRecyclableObject recyclable)
        {
            recyclable.DecrementRefCount();
        }
        return result;
    }

    public int Count => backingDictionary.Count;

    public bool IsReadOnly => false;

    public void Add(TK key, TV value)
    {
        if (key is IRecyclableObject keyRecyclable)
        {
            keyRecyclable.IncrementRefCount();
        }
        if (value is IRecyclableObject valueRecyclable)
        {
            valueRecyclable.IncrementRefCount();
        }
        backingDictionary.TryAdd(key, value);
    }

    public bool ContainsKey(TK key) => backingDictionary.ContainsKey(key);

    public bool Remove(TK key)
    {
        TV? checkKeyForRecycle = default;
        if (backingDictionary.ContainsKey(key))
        {
            checkKeyForRecycle = backingDictionary[key];
        }
        var result = backingDictionary.Remove(key);
        if (key is IRecyclableObject keyRecyclable)
        {
            keyRecyclable.DecrementRefCount();
        }
        if (checkKeyForRecycle is IRecyclableObject recyclable)
        {
            recyclable.DecrementRefCount();
        }
        return result;
    }


    bool IReadOnlyDictionary<TK, TV>.TryGetValue(TK key, out TV value) => TryGetValue(key, out value!);

    #pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public bool TryGetValue(TK key, out TV? value)
        
    {
        return backingDictionary.TryGetValue(key, out value);
    }
    #pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    public TV this[TK key]
    {
        get => backingDictionary[key];
        set
        {
            var hasOldValue = backingDictionary.ContainsKey(key);
            var oldValue    = hasOldValue ? backingDictionary[key] : default;
            if (ReferenceEquals(oldValue, value)) return;
            if (value is IRecyclableObject valueRecyclable)
            {
                valueRecyclable.IncrementRefCount();
            }
            backingDictionary[key] = value;
            if (oldValue is IRecyclableObject oldRecyclable)
            {
                oldRecyclable.DecrementRefCount();
            }
            if (!hasOldValue && key is IRecyclableObject keyRecyclable)
            {
                keyRecyclable.IncrementRefCount();
            }
        }
    }

    public override IRecycler? Recycler
    {
        get => base.Recycler;
        set
        {
            base.Recycler = value;
            if (value != null)
            {
                backingDictionary.KeysListFactory = value.Borrow<ReusableList<TK>>;
                backingDictionary.ValuesListFactory = value.Borrow<ReusableList<TV>>;
            }
            else
            {
                backingDictionary.KeysListFactory   = () => new ReusableList<TK>();
                backingDictionary.ValuesListFactory = () => new ReusableList<TV>();
            }
        }
    }

    public override void StateReset()
    {
        Clear();
        base.StateReset();
    }

    IEnumerable<TK> IReadOnlyDictionary<TK, TV>.Keys   => Keys;

    IEnumerable<TV> IReadOnlyDictionary<TK, TV>.Values => Values;

    ICollection<TK> IDictionary<TK, TV>.Keys   => Keys;

    ICollection<TV> IDictionary<TK, TV>.Values => Values;

    public ReusableList<TK> Keys => backingDictionary.Keys;

    public ReusableList<TV> Values => backingDictionary.Values;

    public override ReusableDictionary<TK, TV> Clone() =>
        Recycler?.Borrow<ReusableDictionary<TK, TV>>().CopyFrom(this, CopyMergeFlags.FullReplace) ?? new ReusableDictionary<TK, TV>(this);

    public override ReusableDictionary<TK, TV> CopyFrom
        (IReusableDictionary<TK, TV> source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Clear();
        foreach (var kvp in source)
        {
            Add(kvp);
        }
        return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<TK, TV>> GetEnumerator() => backingDictionary.GetEnumerator();
}
