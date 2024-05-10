#region

using System.Runtime.InteropServices;
using FortitudeCommon.OSWrapper.Memory;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedVolatileFlag
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    private volatile byte bvalue;

    public PaddedVolatileFlag(bool value) => bvalue = (byte)(value ? 1 : 0);

    public bool IsSet() => bvalue == 1;

    public void Set()
    {
        bvalue = 1;
    }

    public void Clear()
    {
        bvalue = 0;
    }
}
