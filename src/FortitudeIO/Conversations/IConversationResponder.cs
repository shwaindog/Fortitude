#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationResponder : IConversation
{
    IReadOnlyDictionary<int, ISocketConversation>? Clients { get; }
    IConversationListener? ConversationListener { get; }
    event Action<ISocketSessionContext>? OnNewClient;
    event Action<ISocketSessionContext>? OnClientRemoved;
    void RemoveClient(ISocketConversation clientSocketSessionContext);
    void RegisterSerializer(uint messageId, IMessageSerializer serializer);
    void Broadcast(IVersionedMessage message);
}
