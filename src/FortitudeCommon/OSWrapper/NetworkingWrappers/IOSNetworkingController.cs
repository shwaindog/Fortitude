using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public interface IOSNetworkingController
    {
        IOSSocket CreateOSSocket(SocketType socketType, ProtocolType protocolType);
        IOSSocket CreateOSSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType);
        IOSSocket CreateOSSocket(SocketInformation socketInformation);
        IPAddress GetIpAddress(string dnsHostName);
        NetworkInterface[] GetAllNetworkInterfaces();
        IDirectOSNetworkingApi DirectOSNetworkingApi { get; }
    }
}
