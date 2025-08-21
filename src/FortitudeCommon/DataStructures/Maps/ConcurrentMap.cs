#region

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.DataStructures.Maps;

public class ConcurrentMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    protected readonly ConcurrentDictionary<TK, TV> ConcurrentDictionary = new();
    private readonly IRecycler personalRecycler = new Recycler();
    protected readonly object SyncLock = new();

    public ConcurrentMap() { }

    public ConcurrentMap(IMap<TK, TV> toClone) => ConcurrentDictionary = new ConcurrentDictionary<TK, TV>(toClone);

    public TV this[TK key]
    {
        get => ConcurrentDictionary[key];
        set
        {
            lock (SyncLock)
            {
                ConcurrentDictionary.AddOrUpdate(key, value!, (_, _) => value!);
                OnUpdated();
            }
        }
    }

    public ICollection<TK> Keys => ConcurrentDictionary.Keys;
    public ICollection<TV> Values => ConcurrentDictionary.Values;

    public TV? GetValue(TK key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public bool TryGetValue(TK key, out TV? value) => ConcurrentDictionary.TryGetValue(key, out value);

    public TV GetOrPut(TK key, Func<TK, TV> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            value = createFunc(key);
            ConcurrentDictionary.TryAdd(key, value);
        }

        return value!;
    }

    public TV AddOrUpdate(TK key, TV value)
    {
        var newValue = ConcurrentDictionary.AddOrUpdate(key, value!, (_, _) => value!);
        OnUpdated();
        return newValue;
    }

    public bool Add(TK key, TV value)
    {
        lock (SyncLock)
        {
            if (ConcurrentDictionary.TryAdd(key, value))
            {
                OnUpdated();
                return true;
            }

            return false;
        }
    }

    public bool Remove(TK key)
    {
        lock (SyncLock)
        {
            if (ConcurrentDictionary.TryRemove(key, out _))
            {
                OnUpdated();
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        lock (SyncLock)
        {
            ConcurrentDictionary.Clear();
            OnUpdated();
        }
    }

    public int Count => ConcurrentDictionary.Count;

    public bool ContainsKey(TK key) => ConcurrentDictionary.ContainsKey(key);

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        var existingOrNew = personalRecycler
            .Borrow<ReusableWrappingEnumerator<KeyValuePair<TK, TV>>>();
        existingOrNew.ProxiedEnumerator ??= ConcurrentDictionary.GetEnumerator();
        return existingOrNew;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    object ICloneable.Clone() => Clone();

    public IMap<TK, TV> Clone() => new ConcurrentMap<TK, TV>(this);

    public event Action<IEnumerable<KeyValuePair<TK, TV>>>? Updated;

    protected void OnUpdated()
    {
        Updated?.Invoke(ConcurrentDictionary);
    }
}
