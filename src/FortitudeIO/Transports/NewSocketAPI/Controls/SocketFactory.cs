using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.SocketFactory;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.Client;

namespace FortitudeIO.Transports.NewSocketAPI.Controls
{
    public interface ISocketFactory
    {
        IOSSocket Create(SocketConversationProtocol protocolType, ISocketConnectionConfig socketConnectionConfig,
            ushort attemptNum);
        ushort GetPort(ISocketConnectionConfig socketConnectionConfig, ushort attemptNum);
    }

    public class SocketFactory : ISocketFactory
    {
        private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(SocketFactory));
        private readonly IOSNetworkingController networkingController;

        public SocketFactory(IOSNetworkingController networkingController)
        {
            this.networkingController = networkingController;
        }

        public IOSSocket Create(SocketConversationProtocol protocolType, ISocketConnectionConfig socketConnectionConfig,
            ushort attemptNum)
        {
            ushort port = GetPort(socketConnectionConfig, attemptNum);
            switch (protocolType)
            {
                case SocketConversationProtocol.UDPPublisher :
                    return CreateNewUdpPublisher(socketConnectionConfig, port);
                case SocketConversationProtocol.UDPSubscriber :
                    return CreateNewUdpSubscriber(socketConnectionConfig, port);
                case SocketConversationProtocol.TCPAcceptor :
                    return CreateNewTcpPublisher(socketConnectionConfig, port);
                default : return CreateNewTcpClient(socketConnectionConfig, port);
            }
        }

        private IOSSocket CreateNewTcpClient(ISocketConnectionConfig socketConnectionConfig, ushort port)
        {
            var host = socketConnectionConfig.Hostname.ToString();
            logger.Info("Attempting TCP connection on {1}:{2}", host, port);
            var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;

            if ((socketConnectionConfig.ConnectionAttributes & SocketConnectionAttributes.KeepAlive) > 0)
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);

                KeepAlive k = new KeepAlive
                {
                    OnOff = 1,
                    KeepAliveTimeMillisecs = 14_000,
                    KeepAliveIntervalMillisecs = 2_000
                };
                var inValue = k.ToByteArray();

                socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
            }
            socket.Connect(new IPEndPoint(networkingController.GetIpAddress(host), port));
            socket.SendBufferSize = socketConnectionConfig.SendBufferSize;
            socket.ReceiveBufferSize = socketConnectionConfig.ReceiveBufferSize;
            return socket;
        }

        private IOSSocket CreateNewTcpPublisher(ISocketConnectionConfig socketConnectionConfig, ushort port)
        {
            IOSSocket listeningSocket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.NoDelay = true;
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            listeningSocket.Listen(10);
            listeningSocket.SendBufferSize = socketConnectionConfig.SendBufferSize;
            return listeningSocket;
        }

        private IOSSocket CreateNewUdpSubscriber(ISocketConnectionConfig socketConnectionConfig, ushort port)
        {
            var host = socketConnectionConfig.Hostname.ToString();
            var mcastIntf = socketConnectionConfig.SubnetMask;
            logger.Info("Attempting UDPsub connection on {0}:{1}={2}:{3}",
                mcastIntf, host, networkingController.GetIpAddress(host), port);
            var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
                SocketType.Dgram, ProtocolType.Udp);
            socket.ExclusiveAddressUse = false;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            var nics = networkingController.GetAllNetworkInterfaces();
            IPv4InterfaceProperties adapterIp4Properties = null;
            IPAddress adapterAddress = null;
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
                if (null == adapterIp4Properties)
                    continue;
                adapterAddress = ipProps.UnicastAddresses
                    .FirstOrDefault(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
                if (adapterAddress != null) break;
            }
            if (adapterAddress == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("adapterAddress != null");
            }
            socket.Bind(new IPEndPoint(adapterAddress, port));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
                new MulticastOption(mcastIntf, adapterIp4Properties.Index));
            socket.ReceiveBufferSize = socketConnectionConfig.ReceiveBufferSize;
            return socket;
        }

        private IOSSocket CreateNewUdpPublisher(ISocketConnectionConfig socketConnectionConfig, ushort port)
        {
            IPAddress mcastGroup;
            logger.Info("Attempting UDPpub connection on {0} {1}:{2}={3}:{4}",
                socketConnectionConfig.SocketDescription, 
                socketConnectionConfig.SubnetMask, socketConnectionConfig.Hostname, mcastGroup = 
                    networkingController.GetIpAddress(socketConnectionConfig.Hostname.ToString()), port);
            var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);
            socket.ExclusiveAddressUse = false;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            var nics = networkingController.GetAllNetworkInterfaces();
            IPv4InterfaceProperties adapterIp4Properties = null;
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
            if (adapterIp4Properties == null)
            {
                throw new NullReferenceException("adapterIp4Properties != null");
            }
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface,
                IPAddress.HostToNetworkOrder(adapterIp4Properties.Index));
            socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, socket.Ttl = 200);
            socket.EnableBroadcast = true;
            socket.MulticastLoopback = false;
            socket.Connect(new IPEndPoint(socketConnectionConfig.SubnetMask, port));
            socket.SendBufferSize = socketConnectionConfig.SendBufferSize;
            return socket;
        }

        public ushort GetPort(ISocketConnectionConfig socketConnectionConfig, ushort attemptNum)
        {
            if (socketConnectionConfig.PortIsDynamic)
            {
                ushort portModulus =
                    (ushort) (socketConnectionConfig.PortEndRange - socketConnectionConfig.PortStartRange);
                ushort port = (ushort) (socketConnectionConfig.PortStartRange + attemptNum % portModulus);
                return port;
            }
            return socketConnectionConfig.PortStartRange;
        }
    }
}