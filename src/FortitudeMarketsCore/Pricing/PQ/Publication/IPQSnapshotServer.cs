#region

using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.Publishing;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQSnapshotServer : ISocketPublisher
{
    event Action<ISocketSessionContext, uint[]>? OnSnapshotRequest;
}
