﻿#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Publishing;

public interface ISocketSenderFactory
{
    bool HasConversationPublisher(ConversationType conversationType);
    SocketSender GetConversationPublisher(ISocketSessionContext socketSocketSessionContext);
}

public class SocketSenderFactory : ISocketSenderFactory
{
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;
#pragma warning disable CS0414 // Field is assigned but its value is never used
    private ConversationType conversationType = ConversationType.Publisher;
#pragma warning restore CS0414 // Field is assigned but its value is never used

    public SocketSenderFactory(IDirectOSNetworkingApi directOSNetworkingApi) => this.directOSNetworkingApi = directOSNetworkingApi;

    public bool HasConversationPublisher(ConversationType conversationType) =>
        conversationType is ConversationType.Publisher or ConversationType.Requester
            or ConversationType.Responder;

    public SocketSender GetConversationPublisher(ISocketSessionContext socketSocketSessionContext)
    {
        var newSocketSender = new SocketSender(socketSocketSessionContext, socketSocketSessionContext.SerdesFactory.MessageSerializationRepository);

        return newSocketSender;
    }
}
