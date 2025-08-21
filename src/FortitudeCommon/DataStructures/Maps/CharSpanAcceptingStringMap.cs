using FortitudeCommon.Extensions;

namespace FortitudeCommon.DataStructures.Maps;

public class CharSpanAcceptingStringMap<TV> : ConcurrentMap<string, TV>
{
    public CharSpanAcceptingStringMap() { }

    public CharSpanAcceptingStringMap(IMap<string, TV> toClone) : base(toClone)
    {
        
    }
    
    public TV this[ReadOnlySpan<char> key]
    {
        get => FindExistingKey(key)!.Value.Value;
        set
        {
            var hasExisting =  FindExistingKey(key);
            lock (SyncLock)
            {
                if (hasExisting != null)
                {
                    if (Equals(value, null))
                    {
                        ConcurrentDictionary.TryRemove(hasExisting.Value.Key, out _);
                        OnUpdated();
                    }
                    else
                    {
                        ConcurrentDictionary.AddOrUpdate(hasExisting.Value.Key, value, (_, newVal) => newVal);
                    }
                    return;
                }
                if (Equals(value, null)) return;
                var newKey = new String(key);
                base[newKey] = value;
            }
        }
    }

    private KeyValuePair<string, TV>? FindExistingKey(ReadOnlySpan<char> check)
    {
        foreach (var kvp in ConcurrentDictionary)
        {
            if (check.SequenceMatches(kvp.Key)) return kvp;
        }
        return null;
    }

    public TV? GetValue(ReadOnlySpan<char> key)
    {
        return FindExistingKey(key)!.Value.Value;;
    }

    public bool TryGetValue(ReadOnlySpan<char> key, out TV? value)
    {
        value = default;
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            value = existing.Value.Value;
            return true;
        }
        return false;
    }

    public TV GetOrPut(ReadOnlySpan<char> key, Func<string, TV> createFunc)
    {
        if (!TryGetValue(key, out var value))
        {
            lock (SyncLock)
            {
                var newKey = new String(key);
                value = createFunc(newKey);
                if (ConcurrentDictionary.TryAdd(newKey, value))
                {
                    OnUpdated();
                }
            }
        }

        return value!;
    }

    public TV AddOrUpdate(ReadOnlySpan<char> key, TV value)
    {
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            lock (SyncLock)
            {
                ConcurrentDictionary.AddOrUpdate(existing.Value.Key, value!, (_, _) => value!);
                return value;
            }
        }
        var newKey = new String(key);
        lock (SyncLock)
        {
            ConcurrentDictionary.AddOrUpdate(newKey, value!, (_, _) => value!);
            OnUpdated();
        }
        return value;
    }

    public bool Add(ReadOnlySpan<char> key, TV value)
    {
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            return false;
        }
        lock (SyncLock)
        {   
            var newKey = new String(key);
            if (ConcurrentDictionary.TryAdd(newKey, value))
            {
                OnUpdated();
                return true;
            }

            return false;
        }
    }

    public bool Remove(ReadOnlySpan<char> key)
    {
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            lock (SyncLock)
            {
                if (ConcurrentDictionary.TryRemove(existing.Value.Key, out _))
                {
                    OnUpdated();
                    return true;
                }
            }
        }

        return false;
    }
}
