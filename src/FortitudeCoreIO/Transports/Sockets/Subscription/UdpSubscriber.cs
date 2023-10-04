#region

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Dispatcher;

#endregion

namespace FortitudeIO.Transports.Sockets.Subscription;

public abstract class UdpSubscriber : SocketSubscriber
{
    private readonly IPAddress mcastIntf;
    private readonly IOSNetworkingController networkingController;

    protected UdpSubscriber(IFLogger logger, ISocketDispatcher dispatcher,
        IOSNetworkingController networkingController, IConnectionConfig connectionConfig,
        string sessionDescription, string? multicastInterface, int wholeMessagesPerReceive,
        IMap<uint, IBinaryDeserializer> serializerCache)
        : base(logger, dispatcher, networkingController, connectionConfig,
            sessionDescription, wholeMessagesPerReceive, serializerCache)
    {
        this.networkingController = networkingController;
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
    }

    protected override IOSSocket CreateAndConnect(string host, int port)
    {
        Logger.Info("Attempting UDPsub connection on {0} {1}:{2}={3}:{4}",
            SessionDescription, mcastIntf, host, networkingController.GetIpAddress(host), port);
        var socket = networkingController.CreateOSSocket(AddressFamily.InterNetwork,
            SocketType.Dgram, ProtocolType.Udp);
        socket.ExclusiveAddressUse = false;
        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

        var nics = networkingController.GetAllNetworkInterfaces();
        IPv4InterfaceProperties? adapterIp4Properties = null;
        IPAddress? adapterAddress = null;
        foreach (var adapter in nics)
        {
            if (!adapter.SupportsMulticast)
                continue;
            var ipProps = adapter.GetIPProperties();
            if (!ipProps.MulticastAddresses.Any())
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
        socket.Bind(new IPEndPoint(adapterAddress, port));
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership,
            new MulticastOption(mcastIntf, adapterIp4Properties!.Index));
        return socket;
    }
}
