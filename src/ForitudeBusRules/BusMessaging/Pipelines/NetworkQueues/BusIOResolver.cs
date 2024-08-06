// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;

public interface IBusNetworkResolver
{
    ISocketDispatcherResolver GetNetworkDispatcherResolver
    (QueueSelectionStrategy queueSelectionStrategy
      , MessageQueueType resolveFor = MessageQueueType.AllNetwork, INetworkInboundMessageQueue? preferredInboundQueue = null,
        INetworkOutboundMessageQueue? preferredOutboundQueue = null);

    ISocketDispatcherResolver GetNetworkOutboundDispatcherResolver
    (INetworkOutboundMessageQueue? preferOutboundMessageQueue,
        QueueSelectionStrategy queueSelectionStrategy = QueueSelectionStrategy.LeastBusy);

    INetworkInboundMessageQueue?  GetNetworkInboundQueueOnSocketListener(ISocketDispatcherListener socketDispatcherListener);
    INetworkOutboundMessageQueue? GetNetworkOutboundQueueOnSocketListener(ISocketDispatcherSender socketDispatcherSender);
}

public class BusNetworkResolver : IBusNetworkResolver
{
    private IConfigureMessageBus configureMessageBus;

    public BusNetworkResolver(IConfigureMessageBus configureMessageBus) => this.configureMessageBus = configureMessageBus;

    public ISocketDispatcherResolver GetNetworkDispatcherResolver
    (QueueSelectionStrategy queueSelectionStrategy
      , MessageQueueType resolveFor = MessageQueueType.AllNetwork, INetworkInboundMessageQueue? preferredInboundQueue = null
      , INetworkOutboundMessageQueue? preferredOutboundQueue = null)
    {
        var busPooledSocketDispatcherResolver = new BusPooledSocketDispatcherResolver(configureMessageBus, queueSelectionStrategy, resolveFor
                                                                                    , preferredInboundQueue, preferredOutboundQueue);
        return busPooledSocketDispatcherResolver;
    }

    public INetworkInboundMessageQueue? GetNetworkInboundQueueOnSocketListener(ISocketDispatcherListener socketDispatcherListener)
    {
        return configureMessageBus.AllMessageQueues.NetworkInboundMessageQueueGroup.FirstOrDefault<INetworkInboundMessageQueue>(mq =>
                 mq.SocketDispatcherListener == socketDispatcherListener);
    }

    public INetworkOutboundMessageQueue? GetNetworkOutboundQueueOnSocketListener(ISocketDispatcherSender socketDispatcherSender)
    {
        return configureMessageBus.AllMessageQueues.NetworkOutboundMessageQueueGroup.FirstOrDefault<INetworkOutboundMessageQueue>(mq =>
                 mq.SocketDispatcherSender == socketDispatcherSender);
    }

    public ISocketDispatcherResolver GetNetworkOutboundDispatcherResolver
    (INetworkOutboundMessageQueue? preferOutboundMessageQueue,
        QueueSelectionStrategy queueSelectionStrategy = QueueSelectionStrategy.LeastBusy)
    {
        if (preferOutboundMessageQueue != null)
        {
            var socketDispatcher = new SocketDispatcher(null, preferOutboundMessageQueue.SocketDispatcherSender);
            return new SimpleSocketDispatcherResolver(socketDispatcher);
        }

        var socketDispatcherSender   = FindSocketDispatcherSender(queueSelectionStrategy);
        var fallbackSocketDispatcher = new SocketDispatcher(null, socketDispatcherSender);
        return new SimpleSocketDispatcherResolver(fallbackSocketDispatcher);
    }

    private ISocketDispatcherSender FindSocketDispatcherSender(QueueSelectionStrategy queueSelectionStrategy)
    {
        ISocketDispatcherSender socketDispatcherSender;
        var selectedInbound = configureMessageBus.AllMessageQueues.NetworkOutboundMessageQueueGroup.AsMessageQueueList()
                                                 .SelectEventQueue(queueSelectionStrategy);
        socketDispatcherSender = selectedInbound.SocketDispatcherSender;
        return socketDispatcherSender;
    }
}
