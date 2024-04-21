#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.Connectivity.Network;

public class ActionWrapper : RecyclableObject
{
    private static readonly IRecycler FactoryRecycler = new Recycler();
    private static readonly ISyncLock SyncLock = new SpinLockLight();

    public IQueueContext? CapturedQueueContext { get; private set; }
    public Action OriginalRegisteredAction { get; private set; } = null!;
    public ActionWrapper? Next { get; set; }

    private void WrappedAction()
    {
        var checkSameContext = QueueContext.CurrentThreadQueueContext;
        if (CapturedQueueContext == null || checkSameContext == CapturedQueueContext)
        {
            OriginalRegisteredAction.Invoke();
            return;
        }

        CapturedQueueContext.RegisteredOn.EnqueuePayload(OriginalRegisteredAction, Rule.NoKnownSender, null, MessageType.RunActionPayload);
    }

    public void Invoke()
    {
        WrappedAction();
        Next?.Invoke();
    }

    public static ActionWrapper? operator +(ActionWrapper? lhs, Action? rhs)
    {
        if (rhs == null) return lhs;
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        if (callbackQueueContext == null)
        {
            var noContextWrapper = FactoryRecycler.Borrow<ActionWrapper>();
            noContextWrapper.CapturedQueueContext = null;
            noContextWrapper.OriginalRegisteredAction = rhs;
            return noContextWrapper;
        }

        var wrappedContextWrapper = callbackQueueContext.PooledRecycler.Borrow<ActionWrapper>();
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

    public static ActionWrapper? operator -(ActionWrapper? lhs, Action? rhs)
    {
        if (lhs == null) return null;
        SyncLock.Acquire();
        try
        {
            var currentActionWrapper = lhs;
            ActionWrapper? previousActionWrapper = null;
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

    public static ActionWrapper? LastActionWrapperInChain(ActionWrapper? rootActionWrapper)
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
