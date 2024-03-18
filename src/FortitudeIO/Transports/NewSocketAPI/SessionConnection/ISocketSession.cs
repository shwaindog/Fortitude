#region

using FortitudeCommon.OSWrapper.NetworkingWrappers;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.SessionConnection;

public interface ISocketSession : ISession
{
    IOSSocket Socket { get; }
    long Id { get; }
    IntPtr Handle { get; }
}
