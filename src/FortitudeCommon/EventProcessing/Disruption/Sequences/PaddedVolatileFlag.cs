#region

using System.Runtime.InteropServices;
using FortitudeCommon.DataStructures.Memory;


#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

[StructLayout(LayoutKind.Explicit, Size = 2 * MemoryUtils.CacheLineSize)]
public struct PaddedVolatileFlag(bool value)
{
    [FieldOffset(MemoryUtils.CacheLineSize)]
    private volatile byte bvalue = (byte)(value ? 1 : 0);

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
