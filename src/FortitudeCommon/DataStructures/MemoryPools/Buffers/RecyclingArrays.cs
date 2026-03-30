// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.DataStructures.MemoryPools.Buffers;

public interface IRecyclingArrayWrapper : IRecyclableObject, IDisposable
{
    Type ArrayElementType { get; }
    
    int Count { get; }
    
    int Length { get; }
}

public class RecyclingArrayWrapper<TElem> : RecyclableObject, IReadOnlyList<TElem>, IRecyclingArrayWrapper
{
    public RecyclingArrayWrapper() => AsArray = new TElem[16];

    public RecyclingArrayWrapper(TElem[] asArray) => AsArray = asArray;
    
    public RecyclingArrayWrapper(int size) => AsArray = new TElem[size];

    public TElem[] AsArray { get; }

    IEnumerator IEnumerable.  GetEnumerator() => GetEnumerator();

    IEnumerator<TElem> IEnumerable<TElem>.GetEnumerator() => GetEnumerator();
    public StructArrayEnumerator<TElem>   GetEnumerator() => AsArray.GetStructEnumerator();

    public bool Contains(TElem item) => IndexOf(item) >= 0;

    public void CopyTo(TElem[] array, int arrayIndex)
    {
        for (var i = 0; i < AsArray.Length && (i + arrayIndex) < array.Length ; i++)
        {
            array[i + arrayIndex] = AsArray[i];
        }
    }

    public Type ArrayElementType => typeof(TElem);

    public int Count => AsArray.Length;
    public int Length => AsArray.Length;
    
    public bool IsReadOnly => false;

    public int  IndexOf(TElem item)
    {
        for (var i = 0; i < AsArray.Length; i++)
        {
            var checkElem = AsArray[i];
            if (Equals(checkElem, item)) return i;
        }
        return -1;
    }

    public TElem this[int index]
    {
        get => AsArray[index];
        set => AsArray[index] =  value;
    }
    
    public void Dispose()
    {
        if (!IsInRecycler)
        {
            Recycle();
        }
    }

    public override void StateReset()
    {
        for (int i = 0; i < AsArray.Length; i++)
        {
            AsArray[i] = default!;
        }
        base.StateReset();
    }
}

public static class RecyclingArrays
{
    private static readonly Recycler?[]            SmallArrayRanges = new Recycler?[16];
    private static readonly ConcurrentDictionary<int, Recycler> LargeArrayRanges = new ();

    public static RecyclingArrayWrapper<TElem> GetReusableArrayOf<TElem>(int size)
    {
        var       elemType      = typeof(TElem);
        Recycler? foundRecycler = null;
        if (size < SmallArrayRanges.Length)
        {
            foundRecycler = SmallArrayRanges[size];
            if (foundRecycler == null)
            {
                SmallArrayRanges[size] = foundRecycler = new Recycler();
            }
        }
        else { foundRecycler = LargeArrayRanges.GetOrAdd(size, _ => new Recycler()); }
        if (!foundRecycler.HasFactory(elemType))
        {
            foundRecycler.RegisterFactory(() => new RecyclingArrayWrapper<TElem>(size));
        }
        var borrowedArray = foundRecycler.Borrow<RecyclingArrayWrapper<TElem>>();
        return borrowedArray;
    }
}
