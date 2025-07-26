// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedLong(long value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    public long Value = value;

    public static class Extensions
    {
        public static long IncrementAndGet(ref PaddedLong paddedLong) => IncrementAndGet(ref paddedLong.Value);
        
        private static long IncrementAndGet(ref long value) => ++value;
    }
}