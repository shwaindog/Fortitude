// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;

public interface INetworkInboundMessageQueue : IMessageQueue
{
    ISocketDispatcherListener SocketDispatcherListener { get; }
}

public class NetworkInboundMessageQueue : MessageQueue, INetworkInboundMessageQueue
{
    public NetworkInboundMessageQueue
    (IConfigureMessageBus messageBus, MessageQueueType queueType, int id
      , ISocketListenerMessageQueueRingPoller ringPoller) : base(messageBus, queueType, id, ringPoller) =>
        SocketDispatcherListener = ringPoller;

    public ISocketDispatcherListener SocketDispatcherListener { get; }
}
