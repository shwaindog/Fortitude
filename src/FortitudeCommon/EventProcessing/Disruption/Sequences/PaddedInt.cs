// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedInt(int value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    public int Value = value;

    public static class Extensions
    {
        public static int IncrementAndGet(ref PaddedInt paddedInt) => IncrementAndGet(ref paddedInt.Value);
        
        private static int IncrementAndGet(ref int value) => ++value;
    }
}
