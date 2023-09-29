using System;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.SessionConnection;

namespace FortitudeIO.Transports.Sockets.Publishing
{
    public interface ISocketPublisher : ISocketConnector, IBinaryStreamPublisher
    {
        void Send(ISession client, IVersionedMessage message);
        void Send(ISocketSessionConnection client, IVersionedMessage message);
    }

    public interface ITcpSocketPublisher : ISocketPublisher
    {
        ISocketSessionConnection RegisterAcceptor(IOSSocket socket, Action<ISocketSessionConnection> acceptor);
        void RemoveClient(ISocketSessionConnection client);
        void Broadcast(IVersionedMessage message);
    }
}