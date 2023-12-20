#region

using FortitudeIO.Transports.Sockets.Publishing;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQSnapshotServer : ISocketPublisher
{
    IPQSnapshotStreamSubscriber SnapshotClientStreamFromSubscriber { get; }
}
