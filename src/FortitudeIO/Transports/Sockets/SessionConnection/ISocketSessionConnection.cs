#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public interface ISocketSessionConnection : ISessionConnection, ISocketSession
    , IDoublyLinkedListNode<ISocketSessionConnection>
{
    new ISocketSessionConnection? Next { get; set; }
    new ISocketSessionConnection? Previous { get; set; }
    new ISocketSessionReceiver? SessionReceiver { get; }
    new ISocketSessionSender? SessionSender { get; }
    event Action<ISocketSessionConnection, string, int>? Error;
    void OnError(ISocketSessionConnection sessionConnection, string message, int id);
}
