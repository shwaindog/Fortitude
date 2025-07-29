// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory.Buffers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedAtomicLong(long value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    internal long lValue = value;

    public static class Extensions
    {
        public static long IncrementAndGet(ref PaddedAtomicLong atomicLong) => IncrementAndGet(ref atomicLong.lValue);
        
        private static long IncrementAndGet(ref long value) => Interlocked.Increment(ref value);
        
        public static long DecrementAndGet(ref PaddedAtomicLong atomicLong) => DecrementAndGet(ref atomicLong.lValue);
        
        private static long DecrementAndGet(ref long value) => Interlocked.Decrement(ref value);
    }
}
