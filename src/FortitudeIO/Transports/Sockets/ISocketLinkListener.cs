#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets;

public interface ISocketLinkListener : ISocketLink, IBinaryStreamPublisher
{
    IBinaryStreamSubscriber StreamFromSubscriber { get; }
    void Enqueue(IDoublyLinkedList<ISocketSessionConnection> cxs, IVersionedMessage message);
}
