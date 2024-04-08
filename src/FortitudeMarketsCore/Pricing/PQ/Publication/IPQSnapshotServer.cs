#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeMarketsCore.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQSnapshotServer : IConversationResponder
{
    event Action<IConversationRequester, PQSnapshotIdsRequest>? OnSnapshotRequest;
    void Send(IConversationRequester client, IVersionedMessage message);
}
