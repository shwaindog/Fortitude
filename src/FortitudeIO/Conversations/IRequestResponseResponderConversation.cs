using System;
using System.Collections.Generic;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets;

namespace FortitudeIO.Transports.NewSocketAPI.Conversations
{
    public interface IRequestResponseResponderConversation : IConversation
    {
        event Action<ISocketConversation> OnNewClient;
        event Action<ISocketConversation> OnClientRemoved;
        IReadOnlyDictionary<int, ISocketConversation> Clients { get; }
        IConversationListener ConversationListener { get; }
        void RemoveClient(ISocketConversation clientSocketSessionContext);
        void RegisterSerializer(uint messageId, IBinarySerializer serializer);
        void Broadcast(IVersionedMessage message);
    }
}