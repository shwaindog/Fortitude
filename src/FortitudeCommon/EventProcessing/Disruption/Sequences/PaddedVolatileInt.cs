// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedVolatileInt(int value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    internal int lvalue = value;

    public int Value
    {
        get => Thread.VolatileRead(ref lvalue);
        set => Thread.VolatileWrite(ref lvalue, value);
    }
}
