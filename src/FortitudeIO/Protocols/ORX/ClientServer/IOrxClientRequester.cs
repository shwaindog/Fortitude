﻿#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Conversations;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public interface IOrxClientRequester : IConversationRequester, ISocketConversation
{
    IConversationDeserializationRepository DeserializationRepository { get; }
    IMessageSerializationRepository SerializationRepository { get; }
}
