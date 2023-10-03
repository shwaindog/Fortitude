using System.Runtime.InteropServices;
using System.Threading;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.EventProcessing.Disruption.Sequences
{
    [StructLayout(LayoutKind.Explicit, Size = 2*MemoryUtils.CacheLineSize)]
    public struct PaddedAtomicLong
    {
        [FieldOffset(MemoryUtils.CacheLineSize)] private long lvalue;

        public PaddedAtomicLong(long value)
        {
            lvalue = value;
        }

        public long IncrementAndGet()
        {
            return Interlocked.Increment(ref lvalue);
        }
    }
}