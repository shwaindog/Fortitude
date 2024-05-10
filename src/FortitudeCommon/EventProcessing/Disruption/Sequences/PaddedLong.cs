#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedLong
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    public long Value;

    public PaddedLong(long value) => Value = value;
}
