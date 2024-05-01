#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines.IOQueues;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeBusRules.Connectivity.Network;

public static class BusNetworkingExtensions
{
    public static IIOInboundMessageQueue? IOInboundMessageQueue(this ISocketSessionContext socketSessionContext, IMessageBus messageBus) =>
        messageBus.BusIOResolver.GetInboundQueueOnSocketListener(socketSessionContext.SocketDispatcher.Listener!);

    public static IIOOutboundMessageQueue? IOOutboundMessageQueue(this ISocketSessionContext socketSessionContext, IMessageBus messageBus) =>
        messageBus.BusIOResolver.GetOutboundQueueOnSocketListener(socketSessionContext.SocketDispatcher.Sender!);
}
