#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Sequences;

#endregion

namespace FortitudeCommon.AsyncProcessing;

public class SpinLockLight(int spinIterations = 200) : RecyclableObject, ISyncLock
{
    private bool wasAcquired;

    private PaddedLong flag = new(0);

    public SpinLockLight() : this(200) { }

    public int ReleaseCount { get; private set; }

    public bool Acquire(int timeoutMs = int.MaxValue)
    {
        DateTime timeoutTime = timeoutMs == int.MaxValue ? DateTime.MaxValue : DateTime.UtcNow.AddMilliseconds(timeoutMs);

        long acquiredValue;
        while ((acquiredValue = Interlocked.CompareExchange(ref flag.Value, 1, 0)) != 0)
        {
            if (TimeContext.UtcNow > timeoutTime)
            {
                break;
            }
            Thread.SpinWait(spinIterations);
        }
        return wasAcquired = acquiredValue == 0;
    }

    public void Reset()
    {
        ReleaseCount = 0;
        Thread.VolatileWrite(ref flag.Value, 0);
        base.StateReset();
    }

    public void Release(bool? forceRelease = null)
    {
        if (!wasAcquired && forceRelease != true) return;
        ReleaseCount++;
        wasAcquired = false;
        Thread.VolatileWrite(ref flag.Value, 0);
    }

    public void Dispose()
    {
        Release();
    }
    
    public override void StateReset()
    {
        Reset();
        base.StateReset();
    }
}
