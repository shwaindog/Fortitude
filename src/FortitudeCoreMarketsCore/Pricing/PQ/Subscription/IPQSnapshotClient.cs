using System.Collections.Generic;
using FortitudeIO.Transports.Sockets.Subscription;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public interface IPQSnapshotClient : ISocketSubscriber
    {
        void RequestSnapshots(IList<IUniqueSourceTickerIdentifier> sourceTickerIds);
    }
}