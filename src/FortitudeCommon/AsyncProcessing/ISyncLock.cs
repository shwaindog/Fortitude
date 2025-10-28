using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.AsyncProcessing;

public interface ISyncLock : IRecyclableObject, IDisposable
{
    int ReleaseCount { get; }

    bool Acquire(int timeoutMs = int.MaxValue);

    void Reset();

    void Release(bool? forceRelease = null);
}