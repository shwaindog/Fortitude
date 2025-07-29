using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.AsyncProcessing;

public class AutoResetEventLock(bool defaultAcquired = false) : RecyclableObject, ISyncLock
{
    private readonly AutoResetEvent resetEvent = new(!defaultAcquired);

    private bool wasAcquired;

    public AutoResetEventLock() : this(false) { }

    public int ReleaseCount { get; private set; }

    public bool Acquire(int timeoutMs = int.MaxValue)
    {
        return wasAcquired = resetEvent.WaitOne(timeoutMs);
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
        resetEvent.Set();
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
