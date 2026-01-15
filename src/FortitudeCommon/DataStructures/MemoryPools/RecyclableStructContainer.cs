// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;

namespace FortitudeCommon.DataStructures.MemoryPools;

public interface IRecyclableStructContainer : IRecyclableObject
{
    Type StoredType { get; }
}

public class RecyclableContainer<T> : RecyclableObject, IRecyclableStructContainer, IEquatable<T>, IEquatable<RecyclableContainer<T>>
{
    public T StoredValue { get; set; } = default!;

    public Type StoredType => typeof(T);

    public RecyclableContainer<T> Initialize(T storedValue)
    {
        StoredValue = storedValue;

        return this;
    }

    public bool Equals(T? other) => EqualityComparer<T>.Default.Equals(StoredValue, other);
    
    public bool Equals(RecyclableContainer<T>? other) => 
        other != null && EqualityComparer<T>.Default.Equals(StoredValue, other.StoredValue);
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((T)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => !EqualityComparer<T>.Default.Equals(StoredValue, default) 
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        ? EqualityComparer<T>.Default.GetHashCode(StoredValue!) : 0;

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({StoredValue})";
}
