// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;

namespace FortitudeCommon.Extensions;

public struct StructArrayEnumerator<TElem>(TElem[]? owner) : IEnumerator<TElem>, IEnumerator
{
    private TElem[]? owner        = owner;
    private int      currentIndex = -1;

    public bool MoveNext() => ++currentIndex < (owner?.Length ?? 0);

    public void Reset() => currentIndex = -1;

    object? IEnumerator.Current => owner != null ? owner[currentIndex] : null;

    public void Dispose()
    {
        owner        = null;
        currentIndex = -1;
    }

    public TElem Current => owner != null ? owner[currentIndex] : throw new InvalidOperationException("No item exists");
}

public static class ArrayExtensions
{
    
    
    
    public static StructArrayEnumerator<TElem> GetStructEnumerator<TElem>(this TElem[] arrayToEnumerateo) => new (arrayToEnumerateo);
}
