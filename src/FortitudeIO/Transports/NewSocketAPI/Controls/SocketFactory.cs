#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Client;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

public interface ISocketFactory
{
    IOSSocket Create(ISocketTopicConnectionConfig topicConnectionConfig
        , ISocketConnectionConfig socketConnectionConfig);
}

public class SocketFactory : ISocketFactory
{
    private static readonly IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketFactory));
    private readonly IOSNetworkingController networkingController;

    public SocketFactory(IOSNetworkingController networkingController) =>
        this.networkingController = networkingController;

    public IOSSocket Create(ISocketTopicConnectionConfig topicConnectionConfig
        , ISocketConnectionConfig socketConnectionConfig)
    {
        var port = socketConnectionConfig.Port;
        switch (topicConnectionConfig.ConversationProtocol)
        {
            case SocketConversationProtocol.UdpPublisher:
                return CreateNewUdpPublisher(topicConnectionConfig, socketConnectionConfig);
            case SocketConversationProtocol.UdpSubscriber:
                return CreateNewUdpSubscriber(topicConnectionConfig, socketConnectionConfig);
            case SocketConversationProtocol.TcpAcceptor:
                return CreateNewTcpPublisher(topicConnectionConfig, socketConnectionConfig);
            default: return CreateNewTcpClient(topicConnectionConfig, socketConnectionConfig);
        }
    }

    private IOSSocket CreateNewTcpClient(ISocketTopicConnectionConfig topicConnectionConfig
        , ISocketConnectionConfig socketConnectionConfig)
    {
        var host = socketConnectionConfig.Hostname!;
        var port = socketConnectionConfig.Port;
        logger.Info("Attempting TCP connection on {0}:{1}", host, port);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        socket.NoDelay = true;

        if ((topicConnectionConfig.ConnectionAttributes & SocketConnectionAttributes.KeepAlive) > 0)
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

        socket.Connect(new IPEndPoint(networkingController.GetIpAddress(host), port));
        socket.SendBufferSize = topicConnectionConfig.SendBufferSize;
        socket.ReceiveBufferSize = topicConnectionConfig.ReceiveBufferSize;
        return socket;
    }

    private IOSSocket CreateNewTcpPublisher(ISocketTopicConnectionConfig topicConnectionConfig
        , ISocketConnectionConfig socketConnectionConfig)
    {
        var listeningSocket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
        listeningSocket.NoDelay = true;
        listeningSocket.Bind(new IPEndPoint(IPAddress.Any, socketConnectionConfig.Port));
        listeningSocket.Listen(10);
        listeningSocket.SendBufferSize = topicConnectionConfig.SendBufferSize;
        return listeningSocket;
    }

    private IOSSocket CreateNewUdpSubscriber(ISocketTopicConnectionConfig topicConnectionConfig
        , ISocketConnectionConfig socketConnectionConfig)
    {
        var host = socketConnectionConfig.Hostname!;
        var mcastIntf = socketConnectionConfig.SubnetMaskIpAddress!;
        logger.Info("Attempting UDPsub connection on {0}:{1}={2}:{3}",
            mcastIntf, host, networkingController.GetIpAddress(host), socketConnectionConfig.Port);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
            SocketType.Dgram, ProtocolType.Udp);
        socket.ExclusiveAddressUse = false;
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        var nics = networkingController.GetAllNetworkInterfaces();
        IPv4InterfaceProperties? adapterIp4Properties = null;
        IPAddress? adapterAddress = null;
        foreach (var adapter in nics)
        {
            var ipProps = adapter.GetIPProperties();
            if (!ipProps.MulticastAddresses.Any())
                continue;
            if (!adapter.SupportsMulticast)
                continue;
            if (OperationalStatus.Up != adapter.OperationalStatus)
                continue;
            adapterIp4Properties = ipProps.GetIPv4Properties();
            adapterAddress = ipProps.UnicastAddresses
                .FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
            if (adapterAddress != null) break;
        }

        if (adapterAddress == null)
            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("adapterAddress != null");
        socket.Bind(new IPEndPoint(adapterAddress, socketConnectionConfig.Port));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
            new MulticastOption(mcastIntf, adapterIp4Properties!.Index));
        socket.ReceiveBufferSize = topicConnectionConfig.ReceiveBufferSize;
        return socket;
    }

    private IOSSocket CreateNewUdpPublisher(ISocketTopicConnectionConfig topicConnectionConfig
        , ISocketConnectionConfig socketConnectionConfig)
    {
        IPAddress mcastGroup;
        logger.Info("Attempting UDPpub connection on {0} {1}:{2}={3}:{4}",
            socketConnectionConfig.InstanceName,
            socketConnectionConfig.SubnetMask, socketConnectionConfig.Hostname, mcastGroup =
                networkingController.GetIpAddress(socketConnectionConfig.Hostname), socketConnectionConfig.Port);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Dgram,
            ProtocolType.Udp);
        socket.ExclusiveAddressUse = false;
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        var nics = networkingController.GetAllNetworkInterfaces();
        IPv4InterfaceProperties? adapterIp4Properties = null;
        foreach (var adapter in nics)
        {
            var ipProperties = adapter.GetIPProperties();
            if (!ipProperties.MulticastAddresses.Any())
                continue;
            if (!adapter.SupportsMulticast)
                continue;
            if (OperationalStatus.Up != adapter.OperationalStatus)
                continue;
            adapterIp4Properties = ipProperties.GetIPv4Properties();
            if (null == adapterIp4Properties)
                continue;

            break;
        }

        if (adapterIp4Properties == null) throw new NullReferenceException("adapterIp4Properties != null");
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface,
            IPAddress.HostToNetworkOrder(adapterIp4Properties.Index));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, socket.Ttl = 200);
        socket.EnableBroadcast = true;
        socket.MulticastLoopback = false;
        socket.Connect(new IPEndPoint(socketConnectionConfig.SubnetMaskIpAddress!, socketConnectionConfig.Port));
        socket.SendBufferSize = topicConnectionConfig.SendBufferSize;
        return socket;
    }
}
