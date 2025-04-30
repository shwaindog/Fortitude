// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Maps.IdMap;

/// <summary>
///     A readonly concrete implementation of INameIdLookup.  To populate override and access the cache member directly.
///     see NameIdLookupGenerator.
/// </summary>
public class IdLookup<T> : IIdLookup<T> where T : notnull
{
    private static int lastInstanceNum;

    protected readonly IDictionary<int, T> Cache         = new Dictionary<int, T>();
    protected readonly IDictionary<T, int> ReverseLookup = new Dictionary<T, int>();

    public IdLookup() { }

    public IdLookup(IDictionary<int, T>? copyDict)
    {
        if (copyDict == null) return;
        Cache         = new Dictionary<int, T>(copyDict);
        ReverseLookup = Cache.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
    }

    public IdLookup(IIdLookup<T> toClone) : this((toClone as IdLookup<T>)?.Cache) { }

    public int InstanceNum { get; } = Interlocked.Increment(ref lastInstanceNum);

    public virtual T? this[int id]
    {
        get
        {
            var result = Cache.TryGetValue(id, out var name) ? name : default;
            Console.Out.WriteLine("");
            return result;
        }
    }

    public virtual int this[T? name]
    {
        get
        {
            if (name == null) return 0;
            return ReverseLookup.TryGetValue(name, out var value) ? value : -1;
        }
    }

    public int Count => Cache.Count;

    public T? GetValue(int id) => this[id];

    public int GetId(T? name) => name == null ? 0 : this[name];

    public virtual object Clone() => new IdLookup<T>(this);

    IIdLookup<T> IIdLookup<T>.Clone() => (IIdLookup<T>)Clone();

    public virtual bool AreEquivalent(IIdLookup<T>? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var containsAtLeast = !other.Except(this).Any();
        var countSame       = true;
        if (exactTypes)
        {
            var exactNameIdLookup = other as IdLookup<T>;
            countSame = Cache.Count == (exactNameIdLookup?.Cache?.Count ?? 0);
        }

        return containsAtLeast && countSame;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public virtual IEnumerator<KeyValuePair<int, T>> GetEnumerator() => Cache.GetEnumerator();

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IIdLookup<T>, true);

    public override int GetHashCode()
    {
        var hashCode = 0;
        foreach (var kvp in Cache)
        {
            hashCode = (hashCode * 397) ^ kvp.Key.GetHashCode();
            hashCode = (hashCode * 397) ^ kvp.Value.GetHashCode();
        }

        return hashCode;
    }
}
