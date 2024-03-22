#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationResponder : IConversation
{
    IReadOnlyDictionary<int, IConversationRequester>? Clients { get; }
    IListener? ConversationListener { get; }
    event Action<IConversationRequester>? NewClient;
    event Action<IConversationRequester>? ClientRemoved;
    void RemoveClient(IConversationRequester clientSocketSessionContext);
    void Broadcast(IVersionedMessage message);
}
