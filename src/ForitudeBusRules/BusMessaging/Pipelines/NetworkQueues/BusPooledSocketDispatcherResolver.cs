// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeCommon.Chronometry.Timers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;

internal class BusPooledSocketDispatcherResolver : ISocketDispatcherResolver
{
    private readonly IConfigureMessageBus   messageBus;
    private readonly QueueSelectionStrategy queueSelectionStrategy;
    private readonly MessageQueueType       resolveFor;

    private INetworkInboundMessageQueue?  preferredFirstSelectedInboundQueue;
    private INetworkOutboundMessageQueue? preferredFirstSelectedOutboundQueue;

    public BusPooledSocketDispatcherResolver
    (IConfigureMessageBus messageBus, QueueSelectionStrategy queueSelectionStrategy
      , MessageQueueType resolveFor = MessageQueueType.AllNetwork, INetworkInboundMessageQueue? preferredFirstSelectedInboundQueue = null
      , INetworkOutboundMessageQueue? preferredFirstSelectedOutboundQueue = null)
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
        if ((socketDispatcherListener == null) & ((resolveFor & MessageQueueType.NetworkInbound) > 0))
        {
            var selectedInbound = messageBus.AllMessageQueues.NetworkInboundMessageQueueGroup.AsMessageQueueList()
                                            .SelectEventQueue(queueSelectionStrategy);
            socketDispatcherListener = selectedInbound.SocketDispatcherListener;
        }
        else
        {
            preferredFirstSelectedInboundQueue = null;
        }

        if (socketDispatcherSender == null && (resolveFor & MessageQueueType.NetworkOutbound) > 0)
            socketDispatcherSender = FindSocketDispatcherSender(queueSelectionStrategy);
        else
            preferredFirstSelectedOutboundQueue = null;

        return new SocketDispatcher(socketDispatcherListener, socketDispatcherSender);
    }

    private ISocketDispatcherSender FindSocketDispatcherSender(QueueSelectionStrategy queueSelectionStrategy)
    {
        var selectedInbound = messageBus.AllMessageQueues.NetworkOutboundMessageQueueGroup.AsMessageQueueList()
                                        .SelectEventQueue(queueSelectionStrategy);
        var socketDispatcherSender = selectedInbound.SocketDispatcherSender;
        return socketDispatcherSender;
    }
}
