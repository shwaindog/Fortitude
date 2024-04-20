#region

using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.IOQueues;

public interface IIOInboundMessageQueue : IMessageQueue
{
    ISocketDispatcherListener SocketDispatcherListener { get; }
}

public class IOInboundMessageQueue : MessageQueue, IIOInboundMessageQueue
{
    public IOInboundMessageQueue(IConfigureMessageBus messageBus, MessageQueueType queueType, int id
        , ISocketListenerMessageQueueRingPoller ringPoller) : base(messageBus, queueType, id, ringPoller) =>
        SocketDispatcherListener = ringPoller;

    public ISocketDispatcherListener SocketDispatcherListener { get; }
}
