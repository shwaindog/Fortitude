#region

using FortitudeIO.Sockets;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public interface ISocketSessionSender : ISessionSender, ISocketSession
{
    ISocketSessionConnection? Parent { get; set; }
}
