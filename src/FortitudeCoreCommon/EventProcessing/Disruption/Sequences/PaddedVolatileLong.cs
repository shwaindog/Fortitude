using System.Runtime.InteropServices;
using System.Threading;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.EventProcessing.Disruption.Sequences
{
    [StructLayout(LayoutKind.Explicit, Size = 2*MemoryUtils.CacheLineSize)]
    public struct PaddedVolatileLong
    {
        [FieldOffset(MemoryUtils.CacheLineSize)] private long lvalue;

        public PaddedVolatileLong(long value)
        {
            lvalue = value;
        }

        public long Value
        {
            get { return Thread.VolatileRead(ref lvalue); }
            set { Thread.VolatileWrite(ref lvalue, value); }
        }
    }
}