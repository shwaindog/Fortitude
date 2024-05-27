// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Memory.UnmanagedMemory;

public unsafe class UnmanagedByteArray : IByteArray
{
    private readonly long                        arrayOffset;
    private          IVirtualMemoryAddressRange? mappedViewRegion;

    private VirtualMemoryByteArrayEnumerator? reusableEnumerator;

    public UnmanagedByteArray(IVirtualMemoryAddressRange mappedViewRegion, long arrayOffset, int length)
    {
        if (mappedViewRegion.SizeBytes < arrayOffset + length)
            throw new Exception("Memory mapped file view size does not match expected file position and/or size");
        this.mappedViewRegion = mappedViewRegion;
        this.arrayOffset      = arrayOffset;
        Length                = length;
    }

    public bool IsReadOnly => false;

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<byte> GetEnumerator() => reusableEnumerator ??= new VirtualMemoryByteArrayEnumerator(mappedViewRegion!, arrayOffset, Length);

    public void CopyTo(byte[] array, int arrayIndex)
    {
        for (var i = 0; i < Length; i++) array[arrayIndex + i] = *(mappedViewRegion!.StartAddress + arrayOffset + i);
    }

    int IReadOnlyCollection<byte>.Count => Length;

    byte IReadOnlyList<byte>.this[int index] => this[index];

    public byte this[long index]
    {
        get
        {
            if (index > Length || index < 0)
                throw new IndexOutOfRangeException($"Attempting to read beyond the bounds of VirtualMemoryMappedByteArray." +
                                                   $" index: {index}, length {Length} ");
            return *(mappedViewRegion!.StartAddress + arrayOffset + index);
        }
        set
        {
            if (index > Length || index < 0)
                throw new IndexOutOfRangeException($"Attempting to write beyond the bounds of VirtualMemoryMappedByteArray." +
                                                   $" index: {index}, length {Length} ");
            *(mappedViewRegion!.StartAddress + arrayOffset + index) = value;
        }
    }

    public void SetLength(long newSize)
    {
        if (mappedViewRegion!.SizeBytes < arrayOffset + newSize)
            throw new Exception("Memory mapped file view size does not match expected file position and/or size");
        Length = (int)newSize;
    }

    public void Flush()
    {
        mappedViewRegion?.Flush();
    }

    public int Length { get; private set; }

    int IByteArray.Count => Length;

    public void Dispose()
    {
        mappedViewRegion?.Dispose();
        mappedViewRegion = null;
    }

    public void Clear()
    {
        for (var i = arrayOffset; i < arrayOffset + Length; i++) *(mappedViewRegion!.StartAddress + i) = 0;
    }

    public bool Contains(byte item)
    {
        for (var i = arrayOffset; i < arrayOffset + Length; i++)
            if (item == *(mappedViewRegion!.StartAddress + i))
                return true;

        return false;
    }
}
