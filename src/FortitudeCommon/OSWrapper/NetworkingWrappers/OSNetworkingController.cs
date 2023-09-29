using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public class OSNetworkingController : IOSNetworkingController
    {
        private DirectOSNetworkingApi directOSNetworkingApi;

        public IOSSocket CreateOSSocket(SocketType socketType, ProtocolType protocolType)
        {
            return new OSSocket(new Socket(socketType, protocolType));
        }

        public IOSSocket CreateOSSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType)
        {
            return new OSSocket(new Socket(addressFamily, socketType, protocolType));
        }

        public IOSSocket CreateOSSocket(SocketInformation socketInformation)
        {
            return new OSSocket(new Socket(socketInformation));
        }

        public IPAddress GetIpAddress(string dnsHostName)
        {
            foreach (var a in Dns.GetHostAddresses(dnsHostName))
                if (a.AddressFamily == AddressFamily.InterNetwork) return a;
            throw new Exception("Could not resolve ip address for " + dnsHostName);
        }

        public NetworkInterface[] GetAllNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }

        public IDirectOSNetworkingApi DirectOSNetworkingApi => directOSNetworkingApi 
            ?? (directOSNetworkingApi = new DirectOSNetworkingApi());
    }
}