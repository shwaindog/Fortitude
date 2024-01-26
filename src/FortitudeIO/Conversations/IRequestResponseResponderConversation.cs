#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
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
    void RegisterSerializer(uint messageId, IMessageSerializer serializer);
    void Broadcast(IVersionedMessage message);
}
