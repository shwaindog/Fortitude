#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serialization;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Conversations;

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
