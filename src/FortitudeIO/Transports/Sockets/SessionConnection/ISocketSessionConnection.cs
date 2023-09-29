using System;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.SessionConnection
{
    public interface ISocketSessionConnection : ISessionConnection, ISocketSession, IDoublyLinkedListNode<ISocketSessionConnection>
    {
        event Action<ISocketSessionConnection, string, int> Error;
        void OnError(ISocketSessionConnection sessionConnection, string message, int id);
        new ISocketSessionConnection Next { get; set; }
        new ISocketSessionConnection Previous { get; set; }
        new ISocketSessionReceiver SessionReceiver { get; }
        new ISocketSessionSender SessionSender { get; }
    }
}