// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedVolatileLong
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    private long lvalue;

    public PaddedVolatileLong(long value) => lvalue = value;

    public long Value
    {
        get => Thread.VolatileRead(ref lvalue);
        set => Thread.VolatileWrite(ref lvalue, value);
    }
}
