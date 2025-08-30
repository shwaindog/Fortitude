// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public class SingleDestDirectFormatWriterRequestCache : RecyclableObject, IFormatWriterRequestCache
{
    public const string SIngleDestinationTarget = "SingleDestTarget";

    [ThreadStatic] private static IRecycler? requesterThreadRecycler;

    private readonly DoublyLinkedList<BlockingFormatWriterResolverHandle> queuedRequests = new();

    private readonly ISyncLock queueProtectLock = new SpinLockLight();

    protected IFormatWriter? DirectFormatWriter;

    protected FormatWriterReceivedHandler<IFormatWriter> OnReturningFormatWriter;

    protected IMutableFLogFormattingAppender OwningAppender = null!;

    protected Action<IBlockingFormatWriterResolverHandle> RequestHandleDisposed;

    public SingleDestDirectFormatWriterRequestCache()
    {
        RequestHandleDisposed   = WriterHandleDispose;
        OnReturningFormatWriter = WriterFinishedWithBuffer;
    }

    public SingleDestDirectFormatWriterRequestCache Initialize(IMutableFLogFormattingAppender owningAppender, IFLogContext context
      , string targetName = SIngleDestinationTarget)
    {
        OwningAppender = owningAppender;

        if (GetType() == typeof(SingleDestDirectFormatWriterRequestCache)) CreateFormatWriters(targetName, context);

        return this;
    }

    protected bool HasRequests => queuedRequests.Head != null;
    protected IRecycler RequesterRecycler => requesterThreadRecycler ??= new Recycler();

    public bool IsOpen => OwningAppender.IsOpen;

    public virtual int FormatWriterRequestQueue => queuedRequests.Count;

    public virtual void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        DirectFormatWriter = toReturn;
    }

    public virtual IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry)
    {
        var useFormatWriter = TrgGetFormatWriter(logEntry);

        var requesterSync = GetFormatWriterRequesterWaitStrategy();

        var requestHandle =
            RequesterRecycler
                .Borrow<BlockingFormatWriterResolverHandle>()
                .Initialize(logEntry, OwningAppender, RequestHandleDisposed, requesterSync, useFormatWriter);
        if (useFormatWriter == null)
        {
            requesterSync.Acquire(0);

            return AddToQueue(requestHandle);
        }
        return requestHandle;
    }

    public virtual void Close()
    {
        using (queueProtectLock)
        {
            queueProtectLock.Acquire();
            var current = queuedRequests.Head;
            while (current != null)
            {
                var removedFirst = queuedRequests.Remove(current);
                removedFirst.IssueRequestAborted();
                removedFirst.DecrementRefCount();
                current = queuedRequests.Head;
            }
        }
    }

    protected virtual void CreateFormatWriters(string targetName, IFLogContext context) =>
        DirectFormatWriter = OwningAppender.CreatedDirectFormatWriter(context, targetName, OnReturningFormatWriter);

    protected virtual IFormatWriter? TrgGetFormatWriter(IFLogEntry fLogEntry) =>
        Interlocked.CompareExchange(ref DirectFormatWriter, null, DirectFormatWriter);

    protected virtual ISyncLock GetFormatWriterRequesterWaitStrategy() => RequesterRecycler.Borrow<ManualResetEventLock>();

    protected virtual void WriterFinishedWithBuffer(IFormatWriter setReadyOrFlush)
    {
        TrySendFormatWriteToNextIfAny(setReadyOrFlush);
    }

    protected virtual void WriterHandleDispose(IBlockingFormatWriterResolverHandle actualHandle)
    {
        // to do schedule this check
        if (actualHandle is { IsAvailable: true, WasTaken: false }) { }

        actualHandle.DecrementRefCount();
    }

    protected virtual BlockingFormatWriterResolverHandle AddToQueue(BlockingFormatWriterResolverHandle newRequest)
    {
        using var queueLock = queueProtectLock;
        queueProtectLock.Acquire();

        queuedRequests.AddLast(newRequest);

        return newRequest;
    }

    protected BlockingFormatWriterResolverHandle? TrySendFormatWriteToNextIfAny(IFormatWriter toSend)
    {
        BlockingFormatWriterResolverHandle? removedFirst = null;
        using (queueProtectLock)
        {
            queueProtectLock.Acquire();

            if (queuedRequests.Head != null) removedFirst = queuedRequests.Remove(queuedRequests.Head);
        }
        if (removedFirst == null)
        {
            TryToReturnUsedFormatWriter(toSend);
            return null;
        }
        removedFirst.ReceiveFormatWriterHandler(toSend);
        return removedFirst;
    }

    protected BlockingFormatWriterResolverHandle? CheckFormatWriteAvailableForNextQueued()
    {
        BlockingFormatWriterResolverHandle? removedFirst = null;
        IFormatWriter?                      toSend       = null;
        using (queueProtectLock)
        {
            queueProtectLock.Acquire();

            if (queuedRequests.Head != null)
            {
                toSend = TrgGetFormatWriter(queuedRequests.Head.RequestingLogEntry!);
                if (toSend != null) removedFirst = queuedRequests.Remove(queuedRequests.Head);
            }
        }
        if (removedFirst == null)
        {
            if (toSend != null) TryToReturnUsedFormatWriter(toSend);
            return null;
        }
        removedFirst.ReceiveFormatWriterHandler(toSend!);
        return removedFirst;
    }

    public override void StateReset()
    {
        Close();
        OwningAppender = null!;
        base.StateReset();
    }
}
