#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface ISocketLinkInitiator : ISocketLink, IBinaryStreamSubscriber
{
    IBinaryStreamPublisher? StreamToPublisher { get; }
    IFLogger Logger { get; }
}
