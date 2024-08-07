﻿#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeIO.Conversations;

public interface IConversationResponder : IConversation
{
    IReadOnlyDictionary<int, IConversationRequester>? Clients { get; }
    IStreamListener? StreamListener { get; }
    event Action<IConversationRequester>? NewClient;
    event Action<IConversationRequester>? ClientRemoved;
    void RemoveClient(IConversationRequester clientSocketSessionContext, CloseReason closeReason, string? reason = null);
    void Broadcast(IVersionedMessage message);
}
