using System.Threading;
using FortitudeCommon.EventProcessing.Disruption.Sequences;

namespace FortitudeCommon.AsyncProcessing
{
    public class SpinLockLight : ISyncLock
    {
        private PaddedLong flag = new PaddedLong(0);

        public void Acquire()
        {
            while (Interlocked.CompareExchange(ref flag.Value, 1, 0) != 0)
            {
                Thread.Yield();
            }
        }

        public void Release()
        {
            Thread.VolatileWrite(ref flag.Value, 0);
        }
    }
}