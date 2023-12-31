﻿#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.OSWrapper.NetworkingWrappers;

public class OSNetworkingController : IOSNetworkingController
{
    public IDirectOSNetworkingApi directOSNetworkingApi { get; set; } =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            new DirectWindowsNetworkingApi() :
            new DirectLinuxNetworkingApi();

    public IOSSocket CreateOSSocket(SocketType socketType, ProtocolType protocolType) =>
        new OSSocket(new Socket(socketType, protocolType));

    public IOSSocket CreateOSSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) =>
        new OSSocket(new Socket(addressFamily, socketType, protocolType));

    public IPAddress GetIpAddress(string dnsHostName)
    {
        foreach (var a in Dns.GetHostAddresses(dnsHostName))
            if (a.AddressFamily == AddressFamily.InterNetwork)
                return a;
        throw new Exception("Could not resolve ip address for " + dnsHostName);
    }

    public NetworkInterface[] GetAllNetworkInterfaces() => NetworkInterface.GetAllNetworkInterfaces();

    public IDirectOSNetworkingApi DirectOSNetworkingApi => directOSNetworkingApi;
}
