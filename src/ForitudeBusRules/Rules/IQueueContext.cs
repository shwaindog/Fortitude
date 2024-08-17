﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines;
using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeBusRules.BusMessaging.Pipelines.Timers;
using FortitudeCommon.Chronometry.Timers;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.Rules;

public interface IQueueContext
{
    IMessageQueue RegisteredOn   { get; }
    IMessageBus   MessageBus     { get; }
    IRecycler     PooledRecycler { get; }
    IQueueTimer   QueueTimer     { get; }

    IMessageQueueList<IMessageQueue> GetEventQueues(MessageQueueType selector);
}

public class QueueContext : IQueueContext
{
    [ThreadStatic] public static QueueContext? CurrentThreadQueueContext;

    private readonly IConfigureMessageBus configureMessageBus;

    public QueueContext(MessageQueue registeredOn, IConfigureMessageBus configureMessageBus, IRecycler? pooledRecycler = null)
    {
        RegisteredOn             = registeredOn;
        this.configureMessageBus = configureMessageBus;

        PooledRecycler = pooledRecycler ?? new Recycler();
        QueueTimer     = new QueueTimer(TimerContext.CreateUpdateableTimer($"QueueTimer_{RegisteredOn.Name}"), this);
    }

    public IMessageQueue RegisteredOn   { get; }
    public IMessageBus   MessageBus     => configureMessageBus;
    public IRecycler     PooledRecycler { get; }
    public IQueueTimer   QueueTimer     { get; }

    public IMessageQueueList<IMessageQueue> GetEventQueues(MessageQueueType selector) =>
        configureMessageBus.AllMessageQueues.SelectEventQueues(selector);

    public override string ToString() => $"{nameof(QueueContext)}({nameof(RegisteredOn)}: {RegisteredOn})";
}
