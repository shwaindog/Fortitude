// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory.Buffers;

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedAtomicInt(int value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    internal int iValue = value;

    public static class Extensions
    {
        public static int IncrementAndGet(ref PaddedAtomicInt atomicInt) => IncrementAndGet(ref atomicInt.iValue);
        
        private static int IncrementAndGet(ref int value)                   => Interlocked.Increment(ref value);

        public static  int DecrementAndGet(ref PaddedAtomicInt atomicInt) => DecrementAndGet(ref atomicInt.iValue);
        
        private static int DecrementAndGet(ref int value) => Interlocked.Decrement(ref value);
    }
}
