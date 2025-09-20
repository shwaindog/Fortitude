// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Chronometry.Timers;
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
    protected IFormatWriter? RecoverLostThreadWriter;
    
    protected Thread? currentFormatWriterOwnerThread;
    protected DateTime? timeFormatWriterOwnerWasTaken;

    private ITimerUpdate? checkLostThreadTimer;
    private object        lostThreadSyncLock = new();

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
        
        checkLostThreadTimer
            = FLogContext.NullOnUnstartedContext?.AsyncRegistry.LoggerTimers.RunEvery(TimeSpan.FromSeconds(2), CheckLostFormatWriterThreadHandler);

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
                .Initialize(logEntry, Thread.CurrentThread,  OwningAppender, RequestHandleDisposed, requesterSync, useFormatWriter);
        
        
        if (useFormatWriter == null)
        {
            requesterSync.Acquire(0);

            return AddToQueue(requestHandle);
        }
        currentFormatWriterOwnerThread = requestHandle.RequesterTHread;
        timeFormatWriterOwnerWasTaken = DateTime.UtcNow;
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
        DirectFormatWriter = RecoverLostThreadWriter = OwningAppender.CreatedDirectFormatWriter(context, targetName, OnReturningFormatWriter);

    protected virtual IFormatWriter? TrgGetFormatWriter(IFLogEntry fLogEntry) =>
        Interlocked.CompareExchange(ref DirectFormatWriter, null, DirectFormatWriter);

    protected virtual ISyncLock GetFormatWriterRequesterWaitStrategy() => RequesterRecycler.Borrow<ManualResetEventLock>();

    protected virtual void WriterFinishedWithBuffer(IFormatWriter setReadyOrFlush)
    {
        timeFormatWriterOwnerWasTaken  = DateTime.UtcNow.AddYears(10);
        currentFormatWriterOwnerThread = null;
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

            while (queuedRequests.Head != null && removedFirst?.IsDisposed == false)
            {
                removedFirst = queuedRequests.Remove(queuedRequests.Head);
            }
        }
        if (removedFirst == null || removedFirst.IsDisposed)
        {
            TryToReturnUsedFormatWriter(toSend);
            return null;
        }
        currentFormatWriterOwnerThread = removedFirst.RequesterTHread;
        timeFormatWriterOwnerWasTaken  = DateTime.UtcNow;
        if(removedFirst.ReceiveFormatWriterHandler(toSend))
            return removedFirst;
        return TrySendFormatWriteToNextIfAny(toSend);
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
        if (toSend == null) return null;
        if (removedFirst == null || removedFirst.IsDisposed)
        {
            TryToReturnUsedFormatWriter(toSend);
            return null;
        }
        currentFormatWriterOwnerThread = removedFirst.RequesterTHread;
        timeFormatWriterOwnerWasTaken  = DateTime.UtcNow;
        if(removedFirst.ReceiveFormatWriterHandler(toSend))
            return removedFirst;
        return TrySendFormatWriteToNextIfAny(toSend);
    }

    public override void StateReset()
    {
        Close();
        OwningAppender = null!;
        base.StateReset();
    }

    private void CheckLostFormatWriterThreadHandler(IScheduleActualTime? scheduleActualTime)
    {
        lock (lostThreadSyncLock)
        {
            var checkCurrentOwner = currentFormatWriterOwnerThread;
            if (checkCurrentOwner != null)
            {
                var wasTerminated = checkCurrentOwner.Join(500);
                if (wasTerminated && checkCurrentOwner == currentFormatWriterOwnerThread)
                {
                    Console.Out.WriteLine("Recovering lost format writer from thread {0} Id: {1}"
                                        , checkCurrentOwner.Name
                                        , checkCurrentOwner.ManagedThreadId);
                    if (RecoverLostThreadWriter != null)
                    {
                        TrySendFormatWriteToNextIfAny(RecoverLostThreadWriter);
                    }
                    timeFormatWriterOwnerWasTaken = DateTime.UtcNow.AddYears(10);
                    currentFormatWriterOwnerThread = null;
                }
                var timeElapsed = DateTime.Now - timeFormatWriterOwnerWasTaken!.Value;
                if(timeElapsed > TimeSpan.FromSeconds(2))
                {
                    Console.Out.WriteLine("Thread {{Name: {0} Id: {1} has had the format writer for {2} ms"
                                        , currentFormatWriterOwnerThread!.Name, currentFormatWriterOwnerThread.ManagedThreadId, timeElapsed.TotalMilliseconds);
                }
            }
        }
    }
}
