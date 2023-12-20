#region

using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.Publishing;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ;

public interface IPQUpdateServer : IPQPublisher
{
    void Send(IVersionedMessage message);
}
