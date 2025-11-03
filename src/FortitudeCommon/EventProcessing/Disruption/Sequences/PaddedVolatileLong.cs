// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedVolatileLong(long value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    internal long lvalue = value;

    public long Value
    {
        get => Thread.VolatileRead(ref lvalue);
        set => Thread.VolatileWrite(ref lvalue, value);
    }

    public static class Extensions
    {
        public static long IncrementAndGet(ref PaddedVolatileLong atomicLong) => IncrementAndGet(ref atomicLong.lvalue);
        
        private static long IncrementAndGet(ref long value) => Interlocked.Increment(ref value);
        
        public static long DecrementAndGet(ref PaddedVolatileLong atomicLong) => DecrementAndGet(ref atomicLong.lvalue);
        
        private static long DecrementAndGet(ref long value) => Interlocked.Decrement(ref value);
    }
}