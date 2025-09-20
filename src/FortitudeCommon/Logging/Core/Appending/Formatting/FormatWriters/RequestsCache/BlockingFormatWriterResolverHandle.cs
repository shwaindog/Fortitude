// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public interface IBlockingFormatWriterResolverHandle : IDisposable, IRecyclableObject
{
    bool IsDisposed { get; }

    bool IsAvailable { get; }

    bool WasTaken { get; }

    IFLogEntry? RequestingLogEntry { get; }

    bool TryGetFormatWriter(out IFormatWriter? formatWriter);

    IFormatWriter? GetOrWaitForFormatWriter(int timeout = 2_000);

    bool ReceiveFormatWriterHandler(IFormatWriter writer);
    void IssueRequestAborted();
}

public class BlockingFormatWriterResolverHandle : RecyclableObject, IBlockingFormatWriterResolverHandle
  , IDoublyLinkedListNode<BlockingFormatWriterResolverHandle>
{
    private IFormatWriter? currentFormatWriter;

    private Action<IBlockingFormatWriterResolverHandle>? onDisposeCallback;

    private ISyncLock? requesterLockStrategy;

    public BlockingFormatWriterResolverHandle Initialize
    (
        IFLogEntry requester
      , Thread requesterThread
      , IFLogFormattingAppender owner
      , Action<IBlockingFormatWriterResolverHandle> onCompleteHandBackAction
      , ISyncLock syncStrategy
      , IFormatWriter? requestedFormatWriter = null
    )
    {
        IsDisposed            = false;
        WasTaken              = false;
        RequestingLogEntry    = requester;
        RequesterTHread = requesterThread;
        onDisposeCallback     = onCompleteHandBackAction;
        requesterLockStrategy = syncStrategy;
        if (requestedFormatWriter != null)
        {
            currentFormatWriter       = requestedFormatWriter;
            currentFormatWriter.InUse = true;
            IsAvailable               = true;
        }
        else
        {
            currentFormatWriter = null;
            IsAvailable         = false;
        }

        return this;
    }

    public IFLogEntry? RequestingLogEntry { get; private set; }

    public Thread RequesterTHread { get; private set; } = null!;

    public void Dispose()
    {
        if (!IsDisposed)
        {
            lock (this)
            {
                if (IsDisposed) return;
                IsDisposed = true;
                onDisposeCallback?.Invoke(this);
            }
        }
        // do not decrement ref count onDisposeCallback should do that
    }

    public IFormatWriter? GetOrWaitForFormatWriter(int timeout = 2000)
    {
        if (IsAvailable && !IsDisposed)
        {
            WasTaken = true;
            return currentFormatWriter;
        }
        if (!IsDisposed && requesterLockStrategy!.Acquire(timeout))
            if (!IsDisposed)
            {
                WasTaken = true;
                return currentFormatWriter;
            }
        return null;
    }

    public bool IsAvailable { get; private set; }

    public bool IsDisposed { get; set; }

    public bool WasTaken { get; private set; }

    public void IssueRequestAborted()
    {
        IsDisposed = true;
        requesterLockStrategy?.Release();
    }

    public bool ReceiveFormatWriterHandler(IFormatWriter writer)
    {
        if (IsDisposed) return false;
        lock (this)
        {
            if (IsDisposed) return false;
            currentFormatWriter = writer;
            writer.InUse        = true;
            IsAvailable         = true;

            requesterLockStrategy!.Release(true);
            return true;
        }
    }

    public bool TryGetFormatWriter(out IFormatWriter? formatWriter)
    {
        formatWriter = null;
        if (!IsDisposed && IsAvailable)
        {
            formatWriter = currentFormatWriter!;
            return WasTaken = true;
        }
        return false;
    }

    public BlockingFormatWriterResolverHandle? Next { get; set; }
    public BlockingFormatWriterResolverHandle? Previous { get; set; }

    public override void StateReset()
    {
        onDisposeCallback   = null;
        currentFormatWriter = null;

        IsAvailable = false;
        WasTaken    = false;

        requesterLockStrategy?.DecrementRefCount();
        requesterLockStrategy = null;

        base.StateReset();
    }
}
