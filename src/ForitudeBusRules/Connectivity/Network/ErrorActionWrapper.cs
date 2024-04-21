#region

using FortitudeBusRules.BusMessaging.Pipelines.Execution;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.Connectivity.Network;

public class ErrorActionWrapper : RecyclableObject
{
    private static readonly IRecycler FactoryRecycler = new Recycler();
    private static readonly ISyncLock SyncLock = new SpinLockLight();

    public IQueueContext? CapturedQueueContext { get; private set; }

    public Action<string, int> OriginalRegisteredAction { get; private set; } = null!;

    public ErrorActionWrapper? Next { get; set; }

    private void WrappedAction(string message, int errorCode)
    {
        var checkSameContext = QueueContext.CurrentThreadQueueContext;
        if (CapturedQueueContext == null || checkSameContext == CapturedQueueContext)
        {
            OriginalRegisteredAction.Invoke(message, errorCode);
            return;
        }

        var oneParamActionPayload = CapturedQueueContext.PooledRecycler.Borrow<TwoParamSyncActionPayload<string, int>>();
        oneParamActionPayload.Configure(OriginalRegisteredAction, message, errorCode);
        CapturedQueueContext.RegisteredOn.EnqueuePayload(OriginalRegisteredAction, Rule.NoKnownSender, null, MessageType.QueueParamsExecutionPayload);
    }

    public void Invoke(string message, int errorCode)
    {
        WrappedAction(message, errorCode);
        Next?.Invoke(message, errorCode);
    }

    public static ErrorActionWrapper? operator +(ErrorActionWrapper? lhs, Action<string, int>? rhs)
    {
        if (rhs == null) return lhs;
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        if (callbackQueueContext == null)
        {
            var noContextWrapper = FactoryRecycler.Borrow<ErrorActionWrapper>();
            noContextWrapper.CapturedQueueContext = null;
            noContextWrapper.OriginalRegisteredAction = rhs;
            return noContextWrapper;
        }

        var wrappedContextWrapper = callbackQueueContext.PooledRecycler.Borrow<ErrorActionWrapper>();
        wrappedContextWrapper.OriginalRegisteredAction = rhs;
        wrappedContextWrapper.CapturedQueueContext = callbackQueueContext;

        if (lhs == null) return wrappedContextWrapper;
        SyncLock.Acquire();
        try
        {
            var endActionWrapper = LastActionWrapperInChain(lhs);
            endActionWrapper!.Next = wrappedContextWrapper;
            return lhs;
        }
        finally
        {
            SyncLock.Release();
        }
    }

    public static ErrorActionWrapper? operator -(ErrorActionWrapper? lhs, Action<string, int>? rhs)
    {
        if (lhs == null) return null;
        if (rhs == null) return lhs;
        SyncLock.Acquire();
        try
        {
            var currentActionWrapper = lhs;
            ErrorActionWrapper? previousActionWrapper = null;
            while (currentActionWrapper != null)
            {
                if (currentActionWrapper.OriginalRegisteredAction == rhs)
                {
                    if (previousActionWrapper == null) return currentActionWrapper.Next;
                    previousActionWrapper.Next = currentActionWrapper.Next;
                    currentActionWrapper.DecrementRefCount();
                    return lhs;
                }

                currentActionWrapper = currentActionWrapper.Next;
                previousActionWrapper = currentActionWrapper;
            }

            return previousActionWrapper;
        }
        finally
        {
            SyncLock.Release();
        }
    }


    public static ErrorActionWrapper? LastActionWrapperInChain(ErrorActionWrapper? rootActionWrapper)
    {
        var currentActionWrapper = rootActionWrapper;
        var lastNonNullActionWrapper = rootActionWrapper;
        while (currentActionWrapper != null)
        {
            lastNonNullActionWrapper = currentActionWrapper;
            currentActionWrapper = currentActionWrapper.Next;
        }

        return lastNonNullActionWrapper;
    }
}
