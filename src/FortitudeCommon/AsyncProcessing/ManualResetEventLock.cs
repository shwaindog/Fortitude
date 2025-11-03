using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.AsyncProcessing;

public class ManualResetEventLock : RecyclableObject, ISyncLock
{
    private readonly ManualResetEvent resetEvent;

    private bool wasAcquired;
    private int  instanceCount;
    private int  instanceNumber;

    public ManualResetEventLock() : this(false) { }
    
    public ManualResetEventLock(bool defaultAcquired = false)
    {
        resetEvent = new ManualResetEvent(!defaultAcquired);
        instanceNumber = Interlocked.Increment(ref instanceCount);
    }

    public int ReleaseCount { get; private set; }

    public bool Acquire(int timeoutMs = int.MaxValue)
    {
        wasAcquired = resetEvent.WaitOne(timeoutMs);
        return wasAcquired;
    }

    public void Release(bool? forceRelease = null)
    {
        if (!wasAcquired && forceRelease != true) return;
        ReleaseCount++;
        wasAcquired = false;
        resetEvent.Set();
    }

    public void Reset()
    {
        ReleaseCount = 0;
        resetEvent.Reset();
    }

    public void Dispose()
    {
        Release(true);
    }

    public override void StateReset()
    {
        resetEvent.Set();
        base.StateReset();
    }
}
