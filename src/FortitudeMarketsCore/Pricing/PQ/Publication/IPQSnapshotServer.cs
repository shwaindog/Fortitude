#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQSnapshotServer : IConversationRequester
{
    event Action<ISocketSessionContext, uint[]>? OnSnapshotRequest;
    void Send(ISocketSessionContext client, IVersionedMessage message);
}
