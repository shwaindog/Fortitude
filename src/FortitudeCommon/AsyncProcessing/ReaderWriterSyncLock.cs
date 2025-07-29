using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.AsyncProcessing;

public interface IReadWriterSyncLock : ISyncLock
{
    int TryAcquireLockTimeOutMs { get; set; }
    int AcquireLockTimeOutMs    { get; set; }

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

    protected Action? WriteLockReleasedCallback;

    public ReaderWriterSyncLock()
    {
        lastRequest.Push(new LastLockAcquiredState(LastLockAcquired.None, false));
    }

    public ReaderWriterSyncLock(Action writeLockReleasedCallback) : this()
    {
        WriteLockReleasedCallback = writeLockReleasedCallback;
    }

    public ReaderWriterSyncLock Initialize(Action? writeLockReleaseCallback = null)
    {
        WriteLockReleasedCallback = writeLockReleaseCallback;
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
        rwl.ExitReadLock();
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
        WriteLockReleasedCallback?.Invoke();
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

    public void Reset()
    {
        ReleaseCount = 0;
        if (rwl.WaitingReadCount != 0 || rwl.WaitingUpgradeCount != 0 || rwl.WaitingWriteCount != 0)
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
