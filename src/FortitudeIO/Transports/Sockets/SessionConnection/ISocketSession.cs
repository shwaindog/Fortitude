#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public interface ISocketSession : ISession
{
    IOSSocket Socket { get; }
    long Id { get; }
    IntPtr Handle { get; }
}
