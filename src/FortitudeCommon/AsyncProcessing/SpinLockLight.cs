#region

using FortitudeCommon.EventProcessing.Disruption.Sequences;

#endregion

namespace FortitudeCommon.AsyncProcessing;

public class SpinLockLight : ISyncLock
{
    private PaddedLong flag = new(0);

    public void Acquire()
    {
        while (Interlocked.CompareExchange(ref flag.Value, 1, 0) != 0) Thread.SpinWait(10_000);
    }

    public void Release()
    {
        Thread.VolatileWrite(ref flag.Value, 0);
    }
}
