#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Execution;

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

        CapturedQueueContext.RegisteredOn.EnqueuePayloadBody(OriginalRegisteredAction, Rule.NoKnownSender, MessageType.RunActionPayload, null);
    }

    public ActionWrapper? InvokeReturnNext()
    {
        WrappedAction();
        return Next;
    }

    public static Action WrapAndAttach(Action calledAction)
    {
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        var toAttach = FactoryRecycler.Borrow<ActionWrapper>();
        toAttach.CapturedQueueContext = callbackQueueContext;
        toAttach.OriginalRegisteredAction = calledAction;
        return toAttach.Invoke;
    }

    public void Invoke()
    {
        var currentNext = this;
        while (currentNext != null) currentNext = currentNext.InvokeReturnNext();
    }

    public static ActionWrapper? operator +(ActionWrapper? lhs, Action? rhs)
    {
        if (rhs == null) return lhs;
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        var toAttach = FactoryRecycler.Borrow<ActionWrapper>();
        toAttach.CapturedQueueContext = callbackQueueContext;
        toAttach.OriginalRegisteredAction = rhs;
        if (lhs == null) return toAttach;
        SyncLock.Acquire();
        try
        {
            var endActionWrapper = LastActionWrapperInChain(lhs);
            endActionWrapper.Next = toAttach;
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
        if (rhs == null) return lhs;
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

                previousActionWrapper = currentActionWrapper;
                currentActionWrapper = currentActionWrapper.Next;
            }

            return lhs;
        }
        finally
        {
            SyncLock.Release();
        }
    }

    public static ActionWrapper LastActionWrapperInChain(ActionWrapper rootActionWrapper)
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

    public override void StateReset()
    {
        OriginalRegisteredAction = null!;
        CapturedQueueContext = null!;
        Next = null!;
        base.StateReset();
    }
}
