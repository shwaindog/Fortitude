// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Collections;

#endregion

namespace FortitudeCommon.DataStructures.Memory.UnmanagedMemory;

public unsafe class VirtualMemoryByteArrayEnumerator : IEnumerator<byte>
{
    private readonly long                        arrayOffset;
    private readonly int                         length;
    private readonly IVirtualMemoryAddressRange? mappedViewRegion;

    private long currPos = -1;

    public VirtualMemoryByteArrayEnumerator(IVirtualMemoryAddressRange mappedViewRegion, long arrayOffset, int length)
    {
        this.mappedViewRegion = mappedViewRegion;
        this.arrayOffset      = arrayOffset;
        this.length           = length;
    }

    public bool MoveNext() => ++currPos < length;

    public void Reset()
    {
        currPos = -1;
    }

    object IEnumerator.Current => Current;

    public byte Current => *(mappedViewRegion!.StartAddress + arrayOffset + currPos);

    public void Dispose()
    {
        Reset();
    }
}
