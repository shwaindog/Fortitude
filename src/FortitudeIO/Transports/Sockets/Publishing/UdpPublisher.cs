#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets.Dispatcher;
using FortitudeIO.Transports.Sockets.SessionConnection;
using FortitudeIO.Transports.Sockets.Subscription;

#endregion

namespace FortitudeIO.Transports.Sockets.Publishing;

public abstract class UdpPublisher : SocketPublisherBase
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(UdpPublisher));

    protected readonly IConnectionConfig ConnectionConfig;
    private readonly IPAddress mcastIntf;

    private UdpSubscriber? matchSubscriber;

    protected UdpPublisher(IFLogger logger, ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController,
        IConnectionConfig connectionConfig, string sessionDescription, string? multicastInterface)
        : base(logger, dispatcher, sessionDescription, networkingController, connectionConfig.Port)
    {
        ConnectionConfig = connectionConfig;
        try
        {
            mcastIntf = string.IsNullOrEmpty(multicastInterface) ?
                networkingController.GetIpAddress(Dns.GetHostName()) :
                networkingController.GetIpAddress(multicastInterface);
        }
        catch
        {
            mcastIntf = networkingController.GetIpAddress(Dns.GetHostName());
        }

        // logger.Debug("Will publish on IPAddress: {0}", mcastIntf);
    }

    public override IBinaryStreamSubscriber StreamFromSubscriber => matchSubscriber ??= BuildSubscriber(this);

    public ISocketSessionConnection? PublisherConnection => matchSubscriber!.PublisherConnection;

    public bool IsConnected => matchSubscriber!.IsConnected;

    public ISocketSubscriber SocketStreamFromSubscriber => (ISocketSubscriber)StreamFromSubscriber;

    public void Connect()
    {
        throw new NotImplementedException();
    }

    public void Send(ISocketSessionContext client, IVersionedMessage message)
    {
        throw new NotImplementedException();
    }

    protected abstract UdpSubscriber BuildSubscriber(UdpPublisher publisher);

    public abstract class UdpSubscriber : SocketSubscriber
    {
        protected readonly UdpPublisher UdpPublisher;

        protected UdpSubscriber(UdpPublisher udpPublisher, IFLogger logger, ISocketDispatcher dispatcher,
            IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
            string sessionDescription, int wholeMessagesPerReceive,
            IMap<uint, IMessageDeserializer>? serializerCache = null)
            : base(logger, dispatcher, networkingController, connectionConfig,
                sessionDescription, wholeMessagesPerReceive, serializerCache) =>
            UdpPublisher = udpPublisher;

        public override IBinaryStreamPublisher StreamToPublisher => UdpPublisher;

        public ISocketSessionConnection? PublisherConnection => Connector;

        protected override ISocketSessionConnection? Connector { get; set; }

        protected override IOSSocket CreateAndConnect(string host, int port)
        {
            IPAddress mcastGroup;
            Logger.Info("Attempting UDPpub connection on {0} {1}:{2}={3}:{4}",
                SessionDescription, UdpPublisher.mcastIntf, host,
                mcastGroup = NetworkingController.GetIpAddress(host), port);
            var socket = NetworkingController.CreateOSSocket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);
            socket.ExclusiveAddressUse = false;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            var nics = NetworkingController.GetAllNetworkInterfaces();
            IPv4InterfaceProperties? adapterIp4Properties = null;
            foreach (var adapter in nics)
            {
                if (!adapter.SupportsMulticast)
                    continue;
                var ipProperties = adapter.GetIPProperties();
                if (!ipProperties.MulticastAddresses.Any())
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
            socket.Connect(new IPEndPoint(mcastGroup, port));
            return socket;
        }
    }
}
