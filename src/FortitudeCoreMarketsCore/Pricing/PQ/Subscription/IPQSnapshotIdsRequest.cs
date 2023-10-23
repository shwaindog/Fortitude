using FortitudeIO.Protocols;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public interface IPQSnapshotIdsRequest : IVersionedMessage
    {
        uint[] RequestSourceTickerIds { get; set; }
    }
}