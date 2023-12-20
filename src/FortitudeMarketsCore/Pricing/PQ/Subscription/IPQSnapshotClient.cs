#region

using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription;

public interface IPQSnapshotClient : ISocketSubscriber
{
    void RequestSnapshots(IList<IUniqueSourceTickerIdentifier> sourceTickerIds);
}
