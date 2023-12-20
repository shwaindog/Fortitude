#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Conversations;

public interface IRequestResponseResponderConversation : IConversation
{
    IReadOnlyDictionary<int, ISocketConversation>? Clients { get; }
    IConversationListener? ConversationListener { get; }
    event Action<ISocketConversation>? OnNewClient;
    event Action<ISocketConversation>? OnClientRemoved;
    void RemoveClient(ISocketConversation clientSocketSessionContext);
    void RegisterSerializer(uint messageId, IBinarySerializer serializer);
    void Broadcast(IVersionedMessage message);
}
