// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Connectivity.Network.Dispatcher;
using FortitudeIO.Transports.Network.Dispatcher;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.NetworkQueues;

public interface INetworkOutboundMessageQueue : IMessageQueue
{
    ISocketDispatcherSender SocketDispatcherSender { get; }
}

public class NetworkOutboundMessageQueue : MessageQueue, INetworkOutboundMessageQueue
{
    public NetworkOutboundMessageQueue
        (IConfigureMessageBus messageBus, MessageQueueType queueType, int id, ISocketSenderMessageQueueRingPoller messagePump)
        : base(messageBus, queueType, id, messagePump) =>
        SocketDispatcherSender = messagePump;

    public ISocketDispatcherSender SocketDispatcherSender { get; }
}
