using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public class SingleDestDirectFormatWriterRequestCache : RecyclableObject, IFormatWriterRequestCache
{
    public const string SIngleDestinationTarget = "SingleDestTarget";
    
    private readonly DoublyLinkedList<BlockingFormatWriterResolverHandle> queuedRequests = new();

    private readonly ISyncLock queueProtectLock = new SpinLockLight();

    protected IMutableFLogFormattingAppender OwningAppender = null!;

    [ThreadStatic] private static IRecycler? requesterThreadRecycler;

    protected IFormatWriter? DirectFormatWriter;

    protected Action<IBlockingFormatWriterResolverHandle> RequestHandleDisposed;

    protected FormatWriterReceivedHandler<IFormatWriter> OnReturningFormatWriter;

    public SingleDestDirectFormatWriterRequestCache()
    {
        RequestHandleDisposed   = WriterHandleDispose;
        OnReturningFormatWriter = WriterFinishedWithBuffer;
    }
    
    public SingleDestDirectFormatWriterRequestCache Initialize(IMutableFLogFormattingAppender owningAppender, IFLogContext context, string targetName = SIngleDestinationTarget)
    {
        OwningAppender = owningAppender;

        if (GetType() == typeof(SingleDestDirectFormatWriterRequestCache))
        {
            CreateFormatWriters(targetName, context);
        }

        return this;
    }

    protected virtual void CreateFormatWriters(string targetName, IFLogContext context) =>
        DirectFormatWriter = OwningAppender.CreatedDirectFormatWriter(context, targetName, OnReturningFormatWriter);

    protected bool HasRequests => queuedRequests.Head != null;

    public bool IsOpen => OwningAppender.IsOpen;

    public virtual int FormatWriterRequestQueue => queuedRequests.Count;
    protected IRecycler RequesterRecycler => requesterThreadRecycler ??= new Recycler();

    protected virtual IFormatWriter? TrgGetFormatWriter(IFLogEntry fLogEntry) =>
        Interlocked.CompareExchange(ref DirectFormatWriter, null, DirectFormatWriter);

    public virtual void TryToReturnUsedFormatWriter(IFormatWriter toReturn)
    {
        DirectFormatWriter = toReturn;
    }

    public virtual IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry)
    {
        IFormatWriter? useFormatWriter = TrgGetFormatWriter(logEntry);

        var requesterSync = GetFormatWriterRequesterWaitStrategy();

        var requestHandle =
            RequesterRecycler
                .Borrow<BlockingFormatWriterResolverHandle>()
                .Initialize(logEntry, OwningAppender, RequestHandleDisposed, requesterSync, useFormatWriter);
        if (useFormatWriter == null)
        {
            requesterSync.Acquire(0);

            // Console.Out.WriteLine("Adding logEntry " + logEntry.InstanceNumber + " to queue.");
            return AddToQueue(requestHandle);
        }
        return requestHandle;
    }

    protected virtual ISyncLock GetFormatWriterRequesterWaitStrategy()
    {
        return RequesterRecycler.Borrow<ManualResetEventLock>();
    }

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

            if (queuedRequests.Head != null)
            {
                removedFirst = queuedRequests.Remove(queuedRequests.Head);
            }
        }
        if (removedFirst == null)
        {
            TryToReturnUsedFormatWriter(toSend);
            return null;
        }
        // Console.Out.WriteLine("Sending format writer to wait handle.");
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
                if (toSend != null)
                {
                    removedFirst = queuedRequests.Remove(queuedRequests.Head);
                }
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

    public override void StateReset()
    {
        Close();
        OwningAppender = null!;
        base.StateReset();
    }
}
