﻿#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.IOQueues;

public interface IBusIOResolver
{
    ISocketDispatcherResolver GetDispatcherResolver(QueueSelectionStrategy queueSelectionStrategy
        , MessageQueueType resolveFor = MessageQueueType.AllIO);

    IIOInboundMessageQueue? GetInboundQueueOnSocketListener(ISocketDispatcherListener socketDispatcherListener);
    IIOOutboundMessageQueue? GetInboundQueueOnSocketListener(ISocketDispatcherSender socketDispatcherSender);
}

public class BusIOResolver : IBusIOResolver
{
    private IConfigureMessageBus configureMessageBus;

    public BusIOResolver(IConfigureMessageBus configureMessageBus) => this.configureMessageBus = configureMessageBus;

    public ISocketDispatcherResolver GetDispatcherResolver(QueueSelectionStrategy queueSelectionStrategy
        , MessageQueueType resolveFor = MessageQueueType.AllIO)
    {
        ISocketDispatcherListener? socketDispatcherListener = null;
        ISocketDispatcherSender? socketDispatcherSender = null;
        if ((resolveFor & MessageQueueType.IOInbound) > 0)
        {
            var selectedInbound = configureMessageBus.AllMessageQueues.IOInboundMessageQueueGroup.AsMessageQueueList()
                .SelectEventQueue(queueSelectionStrategy);
            socketDispatcherListener = selectedInbound.SocketDispatcherListener;
        }

        if ((resolveFor & MessageQueueType.IOOutbound) > 0)
        {
            var selectedInbound = configureMessageBus.AllMessageQueues.IOOutboundMessageQueueGroup.AsMessageQueueList()
                .SelectEventQueue(queueSelectionStrategy);
            socketDispatcherSender = selectedInbound.SocketDispatcherSender;
        }

        var socketDispatcher = new SocketDispatcher(socketDispatcherListener, socketDispatcherSender);
        return new SimpleSocketDispatcherResolver(socketDispatcher);
    }

    public IIOInboundMessageQueue? GetInboundQueueOnSocketListener(ISocketDispatcherListener socketDispatcherListener)
    {
        return configureMessageBus.AllMessageQueues.IOInboundMessageQueueGroup.FirstOrDefault<IIOInboundMessageQueue>(mq =>
            mq.SocketDispatcherListener == socketDispatcherListener);
    }

    public IIOOutboundMessageQueue? GetInboundQueueOnSocketListener(ISocketDispatcherSender socketDispatcherSender)
    {
        return configureMessageBus.AllMessageQueues.IOOutboundMessageQueueGroup.FirstOrDefault<IIOOutboundMessageQueue>(mq =>
            mq.SocketDispatcherSender == socketDispatcherSender);
    }
}