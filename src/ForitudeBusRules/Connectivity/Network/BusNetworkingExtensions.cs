// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeBusRules.Connectivity.Network;

public static class BusNetworkingExtensions
{
    public static INetworkInboundMessageQueue? NetworkInboundMessageQueue(this ISocketSessionContext socketSessionContext, IMessageBus messageBus) =>
        messageBus.BusNetworkResolver.GetNetworkInboundQueueOnSocketListener(socketSessionContext.SocketDispatcher.Listener!);

    public static INetworkOutboundMessageQueue? NetworkOutboundMessageQueue
        (this ISocketSessionContext socketSessionContext, IMessageBus messageBus) =>
        messageBus.BusNetworkResolver.GetNetworkOutboundQueueOnSocketListener(socketSessionContext.SocketDispatcher.Sender!);
}
