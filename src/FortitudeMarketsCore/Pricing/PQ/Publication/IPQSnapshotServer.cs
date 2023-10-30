using FortitudeIO.Transports.Sockets.Publishing;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public interface IPQSnapshotServer : ISocketPublisher
    {
        IPQSnapshotStreamSubscriber SnapshotClientStreamFromSubscriber { get; }
    }
}