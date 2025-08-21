using FortitudeCommon.Extensions;

namespace FortitudeCommon.DataStructures.Maps;

public class CharSpanAcceptingStringMap<TV> : ConcurrentMap<string, TV>
{
    private List<string> keys = new();
    public CharSpanAcceptingStringMap() { }

    public CharSpanAcceptingStringMap(IMap<string, TV> toClone) : base(toClone)
    {
        foreach (var kvp in toClone)
        {
            if (TryAdd(kvp.Key, kvp.Value))
            {
                keys.Add(kvp.Key);                
            }
        }
    }
    
    public TV this[ReadOnlySpan<char> key]
    {
        get => FindExistingKey(key)!.Value.Value;
        set
        {
            var hasExisting =  FindExistingKey(key);
            if (hasExisting != null)
            {
                if (Equals(value, null))
                {
                    if(ConcurrentDictionary.TryRemove(hasExisting.Value.Key, out var removedValued))
                    {
                        OnUpdated(hasExisting.Value.Key, removedValued, MapUpdateType.Remove);
                    }
                }
                else
                {
                    base.AddOrUpdate(hasExisting.Value.Key, value);
                }
                return;
            }
            if (Equals(value, null)) return;
            var newKey = new String(key);
            base[newKey] = value;
        }
    }

    protected override void OnUpdated(string key, TV value, MapUpdateType updateType)
    {
        if (updateType == MapUpdateType.Create)
        {
            keys.Add(key);
        }
        if (updateType == MapUpdateType.Remove)
        {
            keys.Remove(key);
        }
        base.OnUpdated(key, value, updateType);
    }

    private KeyValuePair<string, TV>? FindExistingKey(ReadOnlySpan<char> check)
    {
        foreach (var key in keys)
        {
            if (check.SequenceMatches(key)) return new KeyValuePair<string, TV>(key, base[key]);
        }
        return null;
    }

    public TV? GetValue(ReadOnlySpan<char> key)
    {
        return FindExistingKey(key)!.Value.Value;
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
            var newKey = new String(key);
            value = createFunc(newKey);
            if (ConcurrentDictionary.TryAdd(newKey, value))
            {
                OnUpdated(newKey, value, MapUpdateType.Create);
            }
        }

        return value!;
    }

    public TV AddOrUpdate(ReadOnlySpan<char> key, TV value)
    {
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            return base.AddOrUpdate(existing.Value.Key, value);
        }
        var newKey = new String(key);
        return base.AddOrUpdate(newKey, value);
    }

    public bool TryAdd(ReadOnlySpan<char> key, TV value)
    {
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            return false;
        }   
        var newKey = new String(key);
        return base.TryAdd(newKey, value);
    }

    public bool Remove(ReadOnlySpan<char> key)
    {
        var existing = FindExistingKey(key);
        if (existing != null)
        {
            return base.Remove(existing.Value.Key);
        }
        return false;
    }
}
