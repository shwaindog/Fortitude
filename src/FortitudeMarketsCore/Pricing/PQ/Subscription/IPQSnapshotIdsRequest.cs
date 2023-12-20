#region

using FortitudeIO.Protocols;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotIdsRequest : IVersionedMessage
{
    uint[] RequestSourceTickerIds { get; set; }
}
