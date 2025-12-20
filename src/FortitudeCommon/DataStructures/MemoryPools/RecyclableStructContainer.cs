// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;

namespace FortitudeCommon.DataStructures.MemoryPools;

public class RecyclableContainer<T> : RecyclableObject, IEquatable<T>, IEquatable<RecyclableContainer<T>>
{
    public T StoredValue { get; set; } = default!;

    public RecyclableContainer<T> Initialize(T storedValue)
    {
        StoredValue = storedValue;

        return this;
    }

    public bool Equals(T other) => EqualityComparer<T>.Default.Equals(StoredValue, other);
    
    public bool Equals(RecyclableContainer<T>? other) => 
        other != null && EqualityComparer<T>.Default.Equals(StoredValue, other.StoredValue);
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((T)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(StoredValue);

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({StoredValue})";
}
