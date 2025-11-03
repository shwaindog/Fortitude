// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Execution;

public class TwoParamActionWrapper<T, TU> : RecyclableObject
{
    private static readonly IRecycler FactoryRecycler = new Recycler();
    private static readonly ISyncLock SyncLock        = new SpinLockLight();

    public IQueueContext? CapturedQueueContext     { get; private set; }
    public Action<T, TU>  OriginalRegisteredAction { get; private set; } = null!;

    public TwoParamActionWrapper<T, TU>? Next { get; set; }

    private void WrappedAction(T firstParam, TU secondParam)
    {
        var checkSameContext = QueueContext.CurrentThreadQueueContext;
        if (CapturedQueueContext == null || checkSameContext == CapturedQueueContext)
        {
            OriginalRegisteredAction.Invoke(firstParam, secondParam);
            return;
        }

        var singleParamPayload = FactoryRecycler.Borrow<TwoParamSyncActionPayload<T, TU>>();
        singleParamPayload.Configure(OriginalRegisteredAction, firstParam, secondParam);
        CapturedQueueContext.RegisteredOn.EnqueuePayloadBody(singleParamPayload, Rule.NoKnownSender, MessageType.InvokeablePayload, null);
    }

    public TwoParamActionWrapper<T, TU>? InvokeReturnNext(T firstParam, TU secondParam)
    {
        WrappedAction(firstParam, secondParam);
        return Next;
    }

    public void Invoke(T firstParam, TU secondParam)
    {
        var currentNext                         = this;
        while (currentNext != null) currentNext = currentNext.InvokeReturnNext(firstParam, secondParam);
    }

    public static TwoParamActionWrapper<T, TU>? operator +(TwoParamActionWrapper<T, TU>? lhs, Action<T, TU>? rhs)
    {
        if (rhs == null) return lhs;
        var callbackQueueContext = QueueContext.CurrentThreadQueueContext;
        var toAttach             = FactoryRecycler.Borrow<TwoParamActionWrapper<T, TU>>();
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

    public static TwoParamActionWrapper<T, TU>? operator -(TwoParamActionWrapper<T, TU>? lhs, Action<T, TU>? rhs)
    {
        if (lhs == null) return null;
        if (rhs == null) return lhs;
        SyncLock.Acquire();
        try
        {
            var                           currentActionWrapper  = lhs;
            TwoParamActionWrapper<T, TU>? previousActionWrapper = null;
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

    public static TwoParamActionWrapper<T, TU> LastActionWrapperInChain(TwoParamActionWrapper<T, TU> rootActionWrapper)
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
