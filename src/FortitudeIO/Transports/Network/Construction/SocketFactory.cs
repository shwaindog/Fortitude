#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Sockets;

#endregion

namespace FortitudeIO.Transports.Network.Construction;

public interface ISocketFactory
{
    IOSSocket Create(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , IEndpointConfig endpointConfig);
}

public class SocketFactory(IOSNetworkingController networkingController) : ISocketFactory
{
    private readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketFactory));

    public IOSSocket Create(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , IEndpointConfig endpointConfig)
    {
        return networkTopicConnectionConfig.ConversationProtocol switch
        {
            SocketConversationProtocol.UdpPublisher => CreateNewUdpPublisher(networkTopicConnectionConfig, endpointConfig)
            , SocketConversationProtocol.UdpSubscriber => CreateNewUdpSubscriber(networkTopicConnectionConfig, endpointConfig)
            , SocketConversationProtocol.TcpAcceptor => CreateNewTcpAcceptor(networkTopicConnectionConfig, endpointConfig)
            , _ => CreateNewTcpClient(networkTopicConnectionConfig, endpointConfig)
        };
    }

    private IOSSocket CreateNewTcpClient(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , IEndpointConfig endpointConfig)
    {
        var host = endpointConfig.Hostname;
        var port = endpointConfig.Port;
        var connectionAttrbs = networkTopicConnectionConfig.ConnectionAttributes;
        logger.Info("Attempting TCP connection on {0}:{1}", host, port);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.NoDelay = (connectionAttrbs & SocketConnectionAttributes.Fast) > 0;
        socket.LingerState = new LingerOption(true, 1);

        if ((networkTopicConnectionConfig.ConnectionAttributes & SocketConnectionAttributes.KeepAlive) > 0)
        {
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

            var k = new KeepAlive
            {
                OnOff = 1, KeepAliveTimeMillisecs = 14_000, KeepAliveIntervalMillisecs = 2_000
            };
            var inValue = k.ToByteArray();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
        }

        socket.ReceiveTimeout = (int)networkTopicConnectionConfig.ResponseTimeoutMs;
        socket.SendTimeout = (int)networkTopicConnectionConfig.ResponseTimeoutMs;
        socket.SendBufferSize = networkTopicConnectionConfig.SendBufferSize;
        socket.ReceiveBufferSize = networkTopicConnectionConfig.ReceiveBufferSize;
        socket.Connect(new IPEndPoint(networkingController.GetIpAddress(host), port));
        return socket;
    }

    private IOSSocket CreateNewTcpAcceptor(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , IEndpointConfig endpointConfig)
    {
        var port = endpointConfig.Port;
        var connectionAttrbs = networkTopicConnectionConfig.ConnectionAttributes;
        var listeningSocket = networkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listeningSocket.LingerState = new LingerOption(false, 0);
        listeningSocket.ExclusiveAddressUse = true;
        listeningSocket.SendBufferSize = networkTopicConnectionConfig.SendBufferSize;
        listeningSocket.ReceiveBufferSize = networkTopicConnectionConfig.ReceiveBufferSize;
        logger.Info("Acceptor attempting TCP bind on {0}:{1}", IPAddress.Any, port);
        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        listeningSocket.NoDelay = (connectionAttrbs & SocketConnectionAttributes.Fast) > 0;
        listeningSocket.Listen(10);
        return listeningSocket;
    }

    private IOSSocket CreateNewUdpSubscriber(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , IEndpointConfig endpointConfig)
    {
        var host = endpointConfig.Hostname;
        var ipAddress = networkingController.GetIpAddress(endpointConfig.Hostname);
        var port = endpointConfig.Port;
        var subnetMaskIpAddress = endpointConfig.SubnetMaskIpAddress!;
        logger.Info("Attempting UDP-sub connection for config (hostname:{0}, subnet:{1}, port:{2}) which resolved to ip-address {3}",
            host, subnetMaskIpAddress, port, ipAddress);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.ExclusiveAddressUse = false;
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        var nics = networkingController.GetAllNetworkInterfaces();
        IPv4InterfaceProperties? adapterIp4Properties = null;
        IPAddress? adapterAddress = null;
        NetworkInterface? selectedAdapter = null;
        foreach (var adapter in nics)
        {
            var ipProps = adapter.GetIPProperties();
            if (!adapter.SupportsMulticast)
                continue;
            if (!ipProps.MulticastAddresses.Any())
                continue;
            if (OperationalStatus.Up != adapter.OperationalStatus)
                continue;
            var adapterIpProps = adapter.GetIPProperties();
            if (!adapterIpProps.UnicastAddresses.Select(ua => ua.Address).Contains(ipAddress))
                continue;
            adapterIp4Properties = ipProps.GetIPv4Properties();
            adapterAddress = ipProps.UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
            selectedAdapter = adapter;
            if (adapterAddress != null) break;
        }

        if (selectedAdapter == null || adapterAddress == null)
            throw new MatchingNicNotFoundException($"No multicast enabled NIC Found with IP {ipAddress}");
        logger.Info("Subscribe will bind on network adapter {0} with description '{1}', which resolved to {2}:{3}",
            selectedAdapter.Name, selectedAdapter.Description, ipAddress, port);
        socket.Bind(new IPEndPoint(adapterAddress, port));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
            new MulticastOption(subnetMaskIpAddress, adapterIp4Properties!.Index));
        socket.ReceiveBufferSize = networkTopicConnectionConfig.ReceiveBufferSize;
        return socket;
    }

    private IOSSocket CreateNewUdpPublisher(INetworkTopicConnectionConfig networkTopicConnectionConfig
        , IEndpointConfig endpointConfig)
    {
        var ipAddress = networkingController.GetIpAddress(endpointConfig.Hostname);
        var port = endpointConfig.Port;
        logger.Info("Attempting UDP-pub connection on (hostname:{0}, subnet:{1}, port:{2}) which resolved to ip-address:{3}",
            endpointConfig.Hostname, endpointConfig.SubnetMask, port, ipAddress);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.ExclusiveAddressUse = false;
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        var nics = networkingController.GetAllNetworkInterfaces();
        IPv4InterfaceProperties? adapterIp4Properties = null;
        NetworkInterface? selectedAdapter = null;
        foreach (var adapter in nics)
        {
            var ipProperties = adapter.GetIPProperties();
            if (!adapter.SupportsMulticast)
                continue;
            if (!ipProperties.MulticastAddresses.Any())
                continue;
            if (OperationalStatus.Up != adapter.OperationalStatus)
                continue;
            var adapterIpProps = adapter.GetIPProperties();
            if (!adapterIpProps.UnicastAddresses.Select(ua => ua.Address).Contains(ipAddress))
                continue;
            selectedAdapter = adapter;
            adapterIp4Properties = ipProperties.GetIPv4Properties();
            break;
        }

        if (selectedAdapter == null || adapterIp4Properties == null)
            throw new MatchingNicNotFoundException($"No multicast enabled NIC Found with IP {ipAddress}");
        logger.Info("Publish will occur from network adapter {0} with description '{1}' on {2}:{3}",
            selectedAdapter.Name, selectedAdapter.Description, ipAddress, port);
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface,
            IPAddress.HostToNetworkOrder(adapterIp4Properties.Index));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, socket.Ttl = 200);
        socket.EnableBroadcast = true;
        socket.MulticastLoopback = false;
        socket.Connect(new IPEndPoint(endpointConfig.SubnetMaskIpAddress!, endpointConfig.Port));
        socket.SendBufferSize = networkTopicConnectionConfig.SendBufferSize;
        return socket;
    }
}

public class MatchingNicNotFoundException : Exception
{
    public MatchingNicNotFoundException(string? message) : base(message) { }
}
