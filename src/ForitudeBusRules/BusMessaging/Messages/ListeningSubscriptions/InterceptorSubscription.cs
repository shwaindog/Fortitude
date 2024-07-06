// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public abstract class InterceptorSubscription : RecyclableObject, ISubscription
{
    protected bool         HasUnsubscribed;
    public    IInterceptor RegisteredInterceptor { get; set; } = null!;
    public    IRule        RegistrationRule      { get; set; } = null!;

    public IConfigureMessageBus ConfigureMessageBus { get; set; } = null!;

    public abstract void      Unsubscribe();
    public abstract ValueTask UnsubscribeAsync();

    public       ValueTask DisposeAwaitValueTask { get; set; }
    public async ValueTask Dispose()             => await UnsubscribeAsync();

    public ValueTask DisposeAsync() => UnsubscribeAsync();

    public override void StateReset()
    {
        RegistrationRule      = null!;
        RegisteredInterceptor = null!;
        DisposeAwaitValueTask = ValueTask.CompletedTask;
        base.StateReset();
    }
}

public class QueueTypeInterceptorSubscription : InterceptorSubscription, ISubscription
{
    public MessageQueueType RegisteredQueueTypes { get; set; }

    public override void Unsubscribe()
    {
        if (HasUnsubscribed) return;
        HasUnsubscribed = true;
        ConfigureMessageBus.AllMessageQueues.RemoveListenSubscribeInterceptor
            (RegistrationRule, (IListenSubscribeInterceptor)RegisteredInterceptor, RegisteredQueueTypes);
    }

    public override async ValueTask UnsubscribeAsync()
    {
        if (HasUnsubscribed) return;
        HasUnsubscribed = true;
        await ConfigureMessageBus.AllMessageQueues.RemoveListenSubscribeInterceptor
            (RegistrationRule, (IListenSubscribeInterceptor)RegisteredInterceptor, RegisteredQueueTypes);
    }

    public override void StateReset()
    {
        RegisteredQueueTypes = MessageQueueType.None;
        base.StateReset();
    }

    public override string ToString() =>
        $"{nameof(QueueTypeInterceptorSubscription)}({nameof(RegisteredQueueTypes)}: {RegisteredQueueTypes}, " +
        $"{nameof(RegisteredInterceptor)}: {RegisteredInterceptor}, {nameof(RegistrationRule)}: {RegistrationRule}, " +
        $"{nameof(HasUnsubscribed)}: {HasUnsubscribed})";
}
