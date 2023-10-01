#region

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

#endregion

namespace FortitudeCommon.DataStructures.Maps;

[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
public class ConcurrentMap<TK, TV> : IMap<TK, TV> where TK : notnull
{
    private readonly ConcurrentDictionary<TK, TV> concurrentDictionary = new();
    private readonly object sync = new();

    public TV this[TK key]
    {
        get => concurrentDictionary[key];
        set
        {
            lock (sync)
            {
                concurrentDictionary.AddOrUpdate(key, value, (oldKey, oldValue) => value);
                OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<TK, TV>(kvp)));
            }
        }
    }

    public bool TryGetValue(TK key, out TV? value) => concurrentDictionary.TryGetValue(key, out value);

    public void Add(TK key, TV value)
    {
        lock (sync)
        {
            if (concurrentDictionary.TryAdd(key, value))
                OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<TK, TV>(kvp)));
        }
    }

    public bool Remove(TK key)
    {
        lock (sync)
        {
            if (concurrentDictionary.TryRemove(key, out var removedValue))
            {
                OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<TK, TV>(kvp)));
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
            OnUpdate?.Invoke(concurrentDictionary.Select(kvp => new KeyValuePair<TK, TV>(kvp)));
        }
    }

    public int Count => concurrentDictionary.Count;

    public bool ContainsKey(TK key) => concurrentDictionary.ContainsKey(key);

    public event Action<IEnumerable<KeyValuePair<TK, TV>>>? OnUpdate;

    public virtual IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
    {
        return concurrentDictionary.Select(kvp => new KeyValuePair<TK, TV>(kvp.Key, kvp.Value, true))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
