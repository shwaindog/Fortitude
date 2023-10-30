#region

using System.Net;

#endregion

namespace FortitudeCommon.OSWrapper.NetworkingWrappers;

public static class OSSocketHelpers
{
    public static IPEndPoint? RemoteOrLocalIPEndPoint(this IOSSocket socket) =>
        socket.Connected ? socket.RemoteEndPoint as IPEndPoint : socket.LocalEndPoint as IPEndPoint;
}
