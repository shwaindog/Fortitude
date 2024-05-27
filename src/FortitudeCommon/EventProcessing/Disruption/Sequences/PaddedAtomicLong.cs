// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedAtomicLong
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    private long lvalue;

    public PaddedAtomicLong(long value) => lvalue = value;

    public long IncrementAndGet() => Interlocked.Increment(ref lvalue);
}
