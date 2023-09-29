using System;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public interface IPQSnapshotStreamSubscriber : ISocketSubscriber
    {
        Action<ISocketSessionConnection, uint[]> OnSnapshotRequest { get; set; }
    }
}