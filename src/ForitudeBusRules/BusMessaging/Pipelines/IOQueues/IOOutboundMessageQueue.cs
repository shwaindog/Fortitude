#region

using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.IOQueues;

public interface IIOOutboundMessageQueue : IMessageQueue
{
    ISocketDispatcherSender SocketDispatcherSender { get; }
}

public class IOOutboundMessageQueue : MessageQueue, IIOOutboundMessageQueue
{
    public IOOutboundMessageQueue(IConfigureMessageBus messageBus, MessageQueueType queueType, int id, ISocketSenderMessageQueueRingPoller ringPoller)
        : base(messageBus, queueType, id, ringPoller) =>
        SocketDispatcherSender = ringPoller;

    public ISocketDispatcherSender SocketDispatcherSender { get; }
}
