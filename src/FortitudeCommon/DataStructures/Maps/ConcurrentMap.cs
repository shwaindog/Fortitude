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
    private readonly ConcurrentDictionary<TK, TV> concurrentDictionary = new();
    private readonly IRecycler personalRecycler = new Recycler();
    private readonly object sync = new();

    public ConcurrentMap() { }

    public ConcurrentMap(IMap<TK, TV> toClone) => concurrentDictionary = new ConcurrentDictionary<TK, TV>(toClone);

    public TV this[TK key]
    {
        get => concurrentDictionary[key];
        set
        {
            lock (sync)
            {
                concurrentDictionary.AddOrUpdate(key, value!, (_, _) => value!);
                OnUpdate?.Invoke(concurrentDictionary);
            }
        }
    }

    public ICollection<TK> Keys => concurrentDictionary.Keys;
    public ICollection<TV> Values => concurrentDictionary.Values;

    public TV? GetValue(TK key)
    {
        TryGetValue(key, out var value);
        return value;
    }

    public bool TryGetValue(TK key, out TV? value) => concurrentDictionary.TryGetValue(key, out value);

    public TV GetOrPut(TK key, Func<TK, TV> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            value = createFunc(key);
            concurrentDictionary.TryAdd(key, value);
        }

        return value!;
    }

    public TV AddOrUpdate(TK key, TV value)
    {
        var newValue = concurrentDictionary.AddOrUpdate(key, value!, (_, _) => value!);
        OnUpdate?.Invoke(concurrentDictionary);
        return newValue;
    }

    public bool Add(TK key, TV value)
    {
        lock (sync)
        {
            if (concurrentDictionary.TryAdd(key, value))
            {
                OnUpdate?.Invoke(concurrentDictionary);
                return true;
            }

            return false;
        }
    }

    public bool Remove(TK key)
    {
        lock (sync)
        {
            if (concurrentDictionary.TryRemove(key, out _))
            {
                OnUpdate?.Invoke(concurrentDictionary);
                return true;
            }
        }

        return false;
    }

    public void Clear()
    {
        lock (sync)
        {
            concurrentDictionary.Clear();
            OnUpdate?.Invoke(concurrentDictionary);
        }
    }

    public int Count => concurrentDictionary.Count;

    public bool ContainsKey(TK key) => concurrentDictionary.ContainsKey(key);

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        var existingOrNew = personalRecycler
            .Borrow<ReusableWrappingEnumerator<KeyValuePair<TK, TV>>>();
        existingOrNew.ProxiedEnumerator ??= concurrentDictionary.GetEnumerator();
        return existingOrNew;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    object ICloneable.Clone() => Clone();

    public IMap<TK, TV> Clone() => new ConcurrentMap<TK, TV>(this);

    public event Action<IEnumerable<KeyValuePair<TK, TV>>>? OnUpdate;
}
