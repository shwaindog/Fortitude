using System;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.SessionConnection
{
    public interface ISocketSession : ISession
    {
        IOSSocket Socket { get; }
        long Id { get; }
        IntPtr Handle { get; }
    }
}