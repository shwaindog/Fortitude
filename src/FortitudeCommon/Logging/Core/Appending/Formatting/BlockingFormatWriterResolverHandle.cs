// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IBlockingFormatWriterResolverHandle : IDisposable
{
    bool IsDisposed { get; }

    bool IsAvailable { get; }

    bool WasTaken { get; }

    bool TryGetFormatWriter(out IFormatWriter? formatWriter);

    IFormatWriter? GetOrWaitForFormatWriter(int timeout = 2_000);

    void ReceiveFormatWriterHandler(IFormatWriter writer);
}

public class BlockingFormatWriterResolverHandle : RecyclableObject, IBlockingFormatWriterResolverHandle
  , IDoublyLinkedListNode<BlockingFormatWriterResolverHandle>
{
    private IFormatWriter? currentFormatWriter;

    private ISyncLock? requesterLockStrategy;

    private IFLogFormattingAppender? owningFormattingAppender;

    private Action<IBlockingFormatWriterResolverHandle>? onDisposeCallback;

    public BlockingFormatWriterResolverHandle Initialize
    (
        IFLogFormattingAppender owner
      , Action<IBlockingFormatWriterResolverHandle> onCompleteHandBackAction
      , ISyncLock syncStrategy
      , IFormatWriter? requestedFormatWriter = null
    )
    {
        onDisposeCallback        = onCompleteHandBackAction;
        owningFormattingAppender = owner;
        requesterLockStrategy    = syncStrategy;
        currentFormatWriter      = requestedFormatWriter;
        IsAvailable              = requestedFormatWriter != null;

        return this;
    }

    public void Dispose()
    {
        IsDisposed = true;
        onDisposeCallback?.Invoke(this);

        // do not decrement ref count onDisposeCallback should do that
    }

    public IFormatWriter? GetOrWaitForFormatWriter(int timeout = 2000)
    {
        if (IsAvailable)
        {
            WasTaken = true;
            return currentFormatWriter;
        }
        if (requesterLockStrategy!.Acquire(timeout))
        {
            WasTaken = true;
            return currentFormatWriter;
        }
        return null;
    }

    public BlockingFormatWriterResolverHandle? Next     { get; set; }
    public BlockingFormatWriterResolverHandle? Previous { get; set; }

    public bool IsAvailable { get; private set; }

    public bool IsDisposed { get; private set; }

    public bool WasTaken { get; private set; }

    public void ReceiveFormatWriterHandler(IFormatWriter writer)
    {
        currentFormatWriter = writer;
        IsAvailable         = true;

        requesterLockStrategy!.Release(true);
    }

    public bool TryGetFormatWriter(out IFormatWriter? formatWriter)
    {
        formatWriter = null;
        if (IsAvailable)
        {
            formatWriter = currentFormatWriter!;
            return WasTaken = true;
        }
        return false;
    }

    public override void StateReset()
    {
        onDisposeCallback   = null;
        currentFormatWriter = null;

        IsAvailable = false;
        WasTaken    = false;

        owningFormattingAppender = null;
        requesterLockStrategy?.DecrementRefCount();
        requesterLockStrategy = null;

        base.StateReset();
    }
}
