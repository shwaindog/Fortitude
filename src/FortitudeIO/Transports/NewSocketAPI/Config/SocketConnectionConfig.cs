#region

using System.Net;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Topics.Config.ConnectionConfig;
using TransportType = FortitudeIO.Topics.Config.ConnectionConfig.TransportType;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.SocketFactory;

public interface ISocketConnectionConfig : ITopicConnectionConfig
{
    SocketConnectionAttributes ConnectionAttributes { get; set; }
    MutableString SocketDescription { get; set; }
    MutableString? Hostname { get; set; }
    IPAddress? SubnetMask { get; set; }
    bool PortIsDynamic { get; set; }
    ushort PortStartRange { get; set; }
    ushort PortEndRange { get; set; }
}

public interface ISocketReceiverConfig : ISocketConnectionConfig
{
    int NumberOfReceivesPerPoll { get; }
}

public class SocketConnectionConfig : ISocketReceiverConfig
{
    public SocketConnectionConfig(string instanceName,
        MutableString socketDescription,
        SocketConnectionAttributes connectionAttributes = SocketConnectionAttributes.None,
        int sendBufferSize = 0, int receiveBufferSize = 0,
        MutableString? hostname = null, IPAddress? subnetMask = null,
        bool portIsDynamic = false,
        ushort portStartRange = 0, ushort portEndRange = 0, int numberOfReceivesPerPoll = 50)
    {
        InstanceName = instanceName;
        ConnectionAttributes = connectionAttributes;
        SocketDescription = socketDescription;
        SendBufferSize = sendBufferSize;
        ReceiveBufferSize = receiveBufferSize;
        Hostname = hostname;
        SubnetMask = subnetMask;
        PortIsDynamic = portIsDynamic;
        PortStartRange = portStartRange;
        PortEndRange = portEndRange;
        NumberOfReceivesPerPoll = numberOfReceivesPerPoll;
    }

    public string InstanceName { get; set; }
    public TransportType TransportType => TransportType.Sockets;
    public int SendBufferSize { get; set; }
    public int ReceiveBufferSize { get; set; }
    public SocketConnectionAttributes ConnectionAttributes { get; set; }
    public MutableString SocketDescription { get; set; }
    public MutableString? Hostname { get; set; }
    public IPAddress? SubnetMask { get; set; }
    public bool PortIsDynamic { get; set; }
    public ushort PortStartRange { get; set; }
    public ushort PortEndRange { get; set; }

    public int NumberOfReceivesPerPoll { get; }

    public override string ToString() =>
        $"SocketConnectionConfig ({nameof(SocketDescription)}: " +
        $"{SocketDescription}{nameof(Hostname)}: {Hostname}, {nameof(SubnetMask)}: {SubnetMask}, " +
        $"{nameof(PortIsDynamic)}: {PortIsDynamic}, {nameof(PortStartRange)}: {PortStartRange}, " +
        $"{nameof(PortEndRange)}: {PortEndRange}, {nameof(SendBufferSize)}: {SendBufferSize}, " +
        $"{nameof(ReceiveBufferSize)}: {ReceiveBufferSize}, " +
        $"{nameof(NumberOfReceivesPerPoll)}: {NumberOfReceivesPerPoll})" +
        $"{nameof(ConnectionAttributes)}: {ConnectionAttributes})";
}
