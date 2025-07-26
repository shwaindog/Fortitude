using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.AsyncProcessing;

public class PassThroughSyncLock : RecyclableObject,  ISyncLock
{
    public int ReleaseCount { get; private set; }

    public bool Acquire(int timeoutMs = int.MaxValue)
    {
        return true;
    }

    public void Release(bool? forceRelease = null)
    {
        ReleaseCount++;
    }

    public void Reset()
    {
        ReleaseCount = 0;
        base.StateReset();
    }

    public void Dispose() { }
    
    public override void StateReset()
    {
        Reset();
        base.StateReset();
    }
}