#region

using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQSnapshotStreamSubscriber : ISocketSubscriber
{
    event Action<ISocketSessionConnection, uint[]>? OnSnapshotRequest;
}
