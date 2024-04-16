#region

using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public class ConnectionTimeoutException : Exception
{
    public ConnectionTimeoutException(string? message, ISocketSessionContext socketSessionContext) : base(message) =>
        SocketSessionContext = socketSessionContext;

    public ISocketSessionContext SocketSessionContext { get; set; }
}
