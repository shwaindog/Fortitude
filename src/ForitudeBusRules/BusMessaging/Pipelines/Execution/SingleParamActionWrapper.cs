// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Execution;

public class SingleParamActionWrapper<T> : RecyclableObject
{
    private static readonly IRecycler FactoryRecycler = new Recycler();
    private static readonly ISyncLock SyncLock        = new SpinLockLight();

    public IQueueContext? CapturedQueueContext { get; private set; }

    public Action<T> OriginalRegisteredAction { get; private set; } = null!;

    public SingleParamActionWrapper<T>? Next { get; set; }

    private void WrappedAction(T firstParam)
    {
        var checkSameContext = QueueContext.CurrentThreadQueueContext;
        if (CapturedQueueContext == null || checkSameContext == CapturedQueueContext)
        {
            OriginalRegisteredAction.Invoke(firstParam);
            return;
        }

        var singleParamPayload = FactoryRecycler.Borrow<OneParamSyncActionPayload<T>>();
        singleParamPayload.Configure(OriginalRegisteredAction, firstParam);
        CapturedQueueContext.RegisteredOn.EnqueuePayloadBody(singleParamPayload, Rule.NoKnownSender, MessageType.InvokeablePayload, null);
    }

    public SingleParamActionWrapper<T>? InvokeReturnNext(T firstParam)
    {
        WrappedAction(firstParam);
        return Next;
    }


    public static Action<T> WrapAndAttach(Action<T> calledAction)
    {
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        var toAttach             = FactoryRecycler.Borrow<SingleParamActionWrapper<T>>();
        toAttach.CapturedQueueContext     = callbackQueueContext;
        toAttach.OriginalRegisteredAction = calledAction;
        return toAttach.Invoke;
    }

    public void Invoke(T firstParam)
    {
        var currentNext                         = this;
        while (currentNext != null) currentNext = currentNext.InvokeReturnNext(firstParam);
    }

    public static SingleParamActionWrapper<T>? operator +(SingleParamActionWrapper<T>? lhs, Action<T>? rhs)
    {
        if (rhs == null) return lhs;
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        var toAttach             = FactoryRecycler.Borrow<SingleParamActionWrapper<T>>();
        toAttach.CapturedQueueContext     = callbackQueueContext;
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

    public static SingleParamActionWrapper<T>? operator -(SingleParamActionWrapper<T>? lhs, Action<T>? rhs)
    {
        if (lhs == null) return null;
        if (rhs == null) return lhs;
        SyncLock.Acquire();
        try
        {
            var                          currentActionWrapper  = lhs;
            SingleParamActionWrapper<T>? previousActionWrapper = null;
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
                currentActionWrapper  = currentActionWrapper.Next;
            }

            return lhs;
        }
        finally
        {
            SyncLock.Release();
        }
    }

    public static SingleParamActionWrapper<T> LastActionWrapperInChain(SingleParamActionWrapper<T> rootActionWrapper)
    {
        var currentActionWrapper     = rootActionWrapper;
        var lastNonNullActionWrapper = rootActionWrapper;
        while (currentActionWrapper != null)
        {
            lastNonNullActionWrapper = currentActionWrapper;
            currentActionWrapper     = currentActionWrapper.Next;
        }

        return lastNonNullActionWrapper;
    }

    public override void StateReset()
    {
        OriginalRegisteredAction = null!;
        CapturedQueueContext     = null!;
        Next                     = null!;
        base.StateReset();
    }
}
