// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeCommon.Chronometry.Timers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.IOQueues;

internal class BusPooledSocketDispatcherResolver : ISocketDispatcherResolver
{
    private readonly IConfigureMessageBus   messageBus;
    private readonly QueueSelectionStrategy queueSelectionStrategy;
    private readonly MessageQueueType       resolveFor;

    private IIOInboundMessageQueue?  preferredFirstSelectedInboundQueue;
    private IIOOutboundMessageQueue? preferredFirstSelectedOutboundQueue;

    public BusPooledSocketDispatcherResolver
    (IConfigureMessageBus messageBus, QueueSelectionStrategy queueSelectionStrategy
      , MessageQueueType resolveFor = MessageQueueType.AllIO, IIOInboundMessageQueue? preferredFirstSelectedInboundQueue = null
      , IIOOutboundMessageQueue? preferredFirstSelectedOutboundQueue = null)
    {
        this.messageBus = messageBus;
        this.resolveFor = resolveFor;

        this.queueSelectionStrategy = queueSelectionStrategy;

        this.preferredFirstSelectedInboundQueue  = preferredFirstSelectedInboundQueue;
        this.preferredFirstSelectedOutboundQueue = preferredFirstSelectedOutboundQueue;
    }

    public IUpdateableTimer Timer { get; set; } = null!;

    public ISocketDispatcher Resolve(INetworkTopicConnectionConfig networkSessionContext)
    {
        var socketDispatcherListener = preferredFirstSelectedInboundQueue?.SocketDispatcherListener;
        var socketDispatcherSender   = preferredFirstSelectedOutboundQueue?.SocketDispatcherSender;
        if ((socketDispatcherListener == null) & ((resolveFor & MessageQueueType.IOInbound) > 0))
        {
            var selectedInbound = messageBus.AllMessageQueues.IOInboundMessageQueueGroup.AsMessageQueueList()
                                            .SelectEventQueue(queueSelectionStrategy);
            socketDispatcherListener = selectedInbound.SocketDispatcherListener;
        }
        else
        {
            preferredFirstSelectedInboundQueue = null;
        }

        if (socketDispatcherSender == null && (resolveFor & MessageQueueType.IOOutbound) > 0)
            socketDispatcherSender = FindSocketDispatcherSender(queueSelectionStrategy);
        else
            preferredFirstSelectedOutboundQueue = null;

        return new SocketDispatcher(socketDispatcherListener, socketDispatcherSender);
    }

    private ISocketDispatcherSender FindSocketDispatcherSender(QueueSelectionStrategy queueSelectionStrategy)
    {
        var selectedInbound = messageBus.AllMessageQueues.IOOutboundMessageQueueGroup.AsMessageQueueList()
                                        .SelectEventQueue(queueSelectionStrategy);
        var socketDispatcherSender = selectedInbound.SocketDispatcherSender;
        return socketDispatcherSender;
    }
}
