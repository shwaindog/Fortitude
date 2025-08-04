using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.AsyncProcessing;

public interface IReadWriterSyncLock : ISyncLock
{
    int TryAcquireLockTimeOutMs { get; set; }
    int AcquireLockTimeOutMs    { get; set; }

    bool HasOutstandingLocksHeld       { get; }
    bool HasOutstandingWriteLock       { get; }

    IDisposable AcquireReaderLock(int? timeoutMs = null);

    bool TryAcquireReaderLock(int? timeoutMs = null);

    void ReleaseReaderLock();

    IDisposable AcquireUpgradeableReaderLock(int? timeoutMs = null);

    bool TryAcquireUpgradeableReaderLock(int? timeoutMs = null);

    void ReleaseUpgradableReaderLock();

    IDisposable AcquireWriterLock(int? timeoutMs = null);

    bool TryAcquireWriterLock(int? timeoutMs = 0);

    void ReleaseWriterLock();
}

internal class ReaderWriterSyncLock : RecyclableObject, IReadWriterSyncLock
{
    ReaderWriterLockSlim  rwl = new();

    protected Action<IReadWriterSyncLock>? WriteLockReleasedCallback;
    protected Action<IReadWriterSyncLock>? NoReaderLocksRemainingCallback;

    public ReaderWriterSyncLock()
    {
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.None, false));
    }

    public ReaderWriterSyncLock(Action<IReadWriterSyncLock> writeLockReleasedCallback, Action<IReadWriterSyncLock> noReaderLocksRemainingCallback) : this()
    {
        WriteLockReleasedCallback      = writeLockReleasedCallback;
        NoReaderLocksRemainingCallback = noReaderLocksRemainingCallback;
    }

    public ReaderWriterSyncLock Initialize(
        Action<IReadWriterSyncLock>? writeLockReleaseCallback = null
       ,Action<IReadWriterSyncLock>? noReaderLocksRemainingCallback = null
        )
    {
        WriteLockReleasedCallback      = writeLockReleaseCallback;
        NoReaderLocksRemainingCallback = noReaderLocksRemainingCallback;
        if (rwl != null!)
        {
            rwl =   new();
        }
        if(lastRequest.Count != 1) lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.None, false));

        return this;
    }

    private enum LastLockAcquired
    {
        None
      , Reader
      , UpgradeableReader
      , Writer
    }

    private record struct LastLockAcquiredState(LastLockAcquired Requested, bool WasAcquired);

    private readonly Stack<LastLockAcquiredState> lastRequest = new();

    public int ReleaseCount { get; private set; }

    public int TryAcquireLockTimeOutMs { get; set; } = 10_000;

    public int AcquireLockTimeOutMs { get; set; } = 2_000;

    
    public IDisposable AcquireReaderLock(int? timeoutMs = null)
    {
        var wasAcquired = rwl.TryEnterReadLock(timeoutMs ?? AcquireLockTimeOutMs);
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.Reader, wasAcquired));
        return this;
    }

    public bool TryAcquireReaderLock(int? timeoutMs = null)
    {
        var wasAcquired = rwl.TryEnterReadLock(timeoutMs ?? TryAcquireLockTimeOutMs);
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.Reader, wasAcquired));
        return wasAcquired;
    }

    public void ReleaseReaderLock()
    {
        if (!rwl.IsReadLockHeld) return;
        var readLocksRemaining = rwl.WaitingReadCount + rwl.WaitingUpgradeCount;
        rwl.ExitReadLock();
        if (readLocksRemaining == 1)
        {
            NoReaderLocksRemainingCallback?.Invoke(this);
        }
    }

    public IDisposable AcquireUpgradeableReaderLock(int? timeoutMs = null)
    {
        var wasAcquired = rwl.TryEnterUpgradeableReadLock(timeoutMs ?? AcquireLockTimeOutMs);
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.UpgradeableReader, wasAcquired));
        return this;
    }

    public bool TryAcquireUpgradeableReaderLock(int? timeoutMs = 0)
    {
        var wasAcquired = rwl.TryEnterUpgradeableReadLock(timeoutMs ?? TryAcquireLockTimeOutMs);
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.UpgradeableReader, wasAcquired));
        return wasAcquired;
    }

    public void ReleaseUpgradableReaderLock()
    {
        if (!rwl.IsUpgradeableReadLockHeld) return;
        rwl.ExitUpgradeableReadLock();
        var readLocksRemaining = rwl.WaitingReadCount + rwl.WaitingUpgradeCount;
        if (readLocksRemaining == 1)
        {
            NoReaderLocksRemainingCallback?.Invoke(this);
        }
    }

    public IDisposable AcquireWriterLock(int? timeoutMs = null)
    {
        var wasAcquired = rwl.TryEnterWriteLock(timeoutMs ?? AcquireLockTimeOutMs);
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.Writer, wasAcquired));
        return this;
    }

    public bool TryAcquireWriterLock(int? timeoutMs = 0)
    {
        var wasAcquired = rwl.TryEnterWriteLock(timeoutMs ?? TryAcquireLockTimeOutMs);
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.Writer, wasAcquired));
        return wasAcquired;
    }

    public void ReleaseWriterLock()
    {
        if (!rwl.IsWriteLockHeld) return;
        rwl.ExitWriteLock();
        WriteLockReleasedCallback?.Invoke(this);
    }

    public bool Acquire(int timeoutMs = int.MaxValue)
    {
        return TryAcquireWriterLock(timeoutMs);
    }

    public void Release(bool? forceRelease = null)
    {
        var lastLock = lastRequest.Pop();
        switch (lastLock.Requested)
        {
            case LastLockAcquired.Reader: if(lastLock.WasAcquired) ReleaseReaderLock(); break;
            case LastLockAcquired.Writer: if(lastLock.WasAcquired) ReleaseWriterLock(); break;
            case LastLockAcquired.UpgradeableReader: if(lastLock.WasAcquired) ReleaseUpgradableReaderLock(); break;
        }
        if (lastRequest.Count == 0)
        {
            while(rwl.IsReadLockHeld)
            {
                rwl.ExitReadLock();
            }
            while(rwl.IsWriteLockHeld)
            {
                rwl.ExitWriteLock();
            }
            while(rwl.IsUpgradeableReadLockHeld)
            {
                rwl.ExitUpgradeableReadLock();
            }
            lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.None, false));
        }
    }

    public bool HasOutstandingLocksHeld => rwl.WaitingReadCount != 0 || rwl.WaitingUpgradeCount != 0 || rwl.WaitingWriteCount != 0;

    public bool HasOutstandingWriteLock       => rwl.WaitingWriteCount != 0 || rwl.IsWriteLockHeld || rwl.WaitingUpgradeCount != 0;

    public void Reset()
    {
        ReleaseCount = 0;
        if (HasOutstandingLocksHeld)
        {
            rwl.Dispose();
            rwl = null!;
            rwl = new ReaderWriterLockSlim();
        }
        lastRequest.Clear();
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.None, false));
    }

    public void Dispose()
    {
        Release();
    }

    public override void StateReset()
    {
        Reset();
        WriteLockReleasedCallback = null;
        base.StateReset();
    }
}
