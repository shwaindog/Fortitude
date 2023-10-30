#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

#endregion

namespace FortitudeCommon.OSWrapper.NetworkingWrappers;

public interface IOSNetworkingController
{
    IDirectOSNetworkingApi DirectOSNetworkingApi { get; }
    IOSSocket CreateOSSocket(SocketType socketType, ProtocolType protocolType);
    IOSSocket CreateOSSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType);
    IPAddress GetIpAddress(string dnsHostName);
    NetworkInterface[] GetAllNetworkInterfaces();
}
