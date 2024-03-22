#region

using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationResponder : IConversation
{
    IReadOnlyDictionary<int, IConversationRequester>? Clients { get; }
    IConversationListener? ConversationListener { get; }
    event Action<IConversationRequester>? NewClient;
    event Action<IConversationRequester>? ClientRemoved;
    void RemoveClient(IConversationRequester clientSocketSessionContext);
    void RegisterSerializer(uint messageId, IMessageSerializer serializer);
    void Broadcast(IVersionedMessage message);
}
