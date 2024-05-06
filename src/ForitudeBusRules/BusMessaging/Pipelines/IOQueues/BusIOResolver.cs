#region

using FortitudeBusRules.BusMessaging.Pipelines.Groups;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.IOQueues;

public interface IBusIOResolver
{
    ISocketDispatcherResolver GetDispatcherResolver(QueueSelectionStrategy queueSelectionStrategy
        , MessageQueueType resolveFor = MessageQueueType.AllIO, IIOInboundMessageQueue? preferredInboundQueue = null,
        IIOOutboundMessageQueue? preferredOutboundQueue = null);

    ISocketDispatcherResolver GetOutboundDispatcherResolver(IIOOutboundMessageQueue? preferOutboundMessageQueue,
        QueueSelectionStrategy queueSelectionStrategy = QueueSelectionStrategy.LeastBusy);

    IIOInboundMessageQueue? GetInboundQueueOnSocketListener(ISocketDispatcherListener socketDispatcherListener);
    IIOOutboundMessageQueue? GetOutboundQueueOnSocketListener(ISocketDispatcherSender socketDispatcherSender);
}

public class BusIOResolver : IBusIOResolver
{
    private IConfigureMessageBus configureMessageBus;

    public BusIOResolver(IConfigureMessageBus configureMessageBus) => this.configureMessageBus = configureMessageBus;

    public ISocketDispatcherResolver GetDispatcherResolver(QueueSelectionStrategy queueSelectionStrategy
        , MessageQueueType resolveFor = MessageQueueType.AllIO, IIOInboundMessageQueue? preferredInboundQueue = null
        , IIOOutboundMessageQueue? preferredOutboundQueue = null)
    {
        var busPooledSocketDispatcherResolver = new BusPooledSocketDispatcherResolver(configureMessageBus, queueSelectionStrategy, resolveFor
            , preferredInboundQueue, preferredOutboundQueue);
        return busPooledSocketDispatcherResolver;
    }

    public IIOInboundMessageQueue? GetInboundQueueOnSocketListener(ISocketDispatcherListener socketDispatcherListener)
    {
        return configureMessageBus.AllMessageQueues.IOInboundMessageQueueGroup.FirstOrDefault<IIOInboundMessageQueue>(mq =>
            mq.SocketDispatcherListener == socketDispatcherListener);
    }

    public IIOOutboundMessageQueue? GetOutboundQueueOnSocketListener(ISocketDispatcherSender socketDispatcherSender)
    {
        return configureMessageBus.AllMessageQueues.IOOutboundMessageQueueGroup.FirstOrDefault<IIOOutboundMessageQueue>(mq =>
            mq.SocketDispatcherSender == socketDispatcherSender);
    }

    public ISocketDispatcherResolver GetOutboundDispatcherResolver(IIOOutboundMessageQueue? preferOutboundMessageQueue,
        QueueSelectionStrategy queueSelectionStrategy = QueueSelectionStrategy.LeastBusy)
    {
        if (preferOutboundMessageQueue != null)
        {
            var socketDispatcher = new SocketDispatcher(null, preferOutboundMessageQueue.SocketDispatcherSender);
            return new SimpleSocketDispatcherResolver(socketDispatcher);
        }

        var socketDispatcherSender = FindSocketDispatcherSender(queueSelectionStrategy);
        var fallbackSocketDispatcher = new SocketDispatcher(null, socketDispatcherSender);
        return new SimpleSocketDispatcherResolver(fallbackSocketDispatcher);
    }

    private ISocketDispatcherSender FindSocketDispatcherSender(QueueSelectionStrategy queueSelectionStrategy)
    {
        ISocketDispatcherSender socketDispatcherSender;
        var selectedInbound = configureMessageBus.AllMessageQueues.IOOutboundMessageQueueGroup.AsMessageQueueList()
            .SelectEventQueue(queueSelectionStrategy);
        socketDispatcherSender = selectedInbound.SocketDispatcherSender;
        return socketDispatcherSender;
    }
}
