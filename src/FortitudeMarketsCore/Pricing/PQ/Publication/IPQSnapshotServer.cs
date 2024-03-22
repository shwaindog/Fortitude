#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQSnapshotServer : IConversationResponder
{
    event Action<IConversationRequester, uint[]>? OnSnapshotRequest;
    void Send(IConversationRequester client, IVersionedMessage message);
}
