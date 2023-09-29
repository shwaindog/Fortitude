using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeIO.Protocols;
using FortitudeIO.Transports.Sockets.Publishing;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

namespace FortitudeIO.Transports.Sockets
{
    public interface ISocketLinkListener : ISocketLink, IBinaryStreamPublisher
    {
        IBinaryStreamSubscriber StreamFromSubscriber{ get; }
        void Enqueue(IDoublyLinkedList<ISocketSessionConnection> cxs, IVersionedMessage message);
    }
}