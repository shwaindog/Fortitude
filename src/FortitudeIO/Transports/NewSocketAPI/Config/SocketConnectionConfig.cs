#region

using System.Net;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Topics.Config.ConnectionConfig;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets;
using TransportType = FortitudeIO.Topics.Config.ConnectionConfig.TransportType;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Config;

public interface ISocketConnectionConfig : ITopicConnectionConfig, ICloneable<ISocketConnectionConfig>
{
    SocketConnectionAttributes ConnectionAttributes { get; set; }
    MutableString SocketDescription { get; set; }
    MutableString? Hostname { get; set; }
    IPAddress? SubnetMask { get; set; }
    bool PortIsDynamic { get; set; }
    ushort PortStartRange { get; set; }
    ushort PortEndRange { get; set; }
    uint ConnectionTimeoutMs { get; set; }
    uint ResponseTimeoutMs { get; set; }

    IConnectionConfig
        ToConnectionConfig(ConnectionDirectionType connectionDirectionType = ConnectionDirectionType.Both);
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
        ushort portStartRange = 0, ushort portEndRange = 0, int numberOfReceivesPerPoll = 50
        , uint connectionTimeoutMs = 10_000, uint responseTimeoutMs = 60_000)
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
        PortEndRange = portEndRange < portStartRange ? portStartRange : portEndRange;
        NumberOfReceivesPerPoll = numberOfReceivesPerPoll;
        ConnectionTimeoutMs = connectionTimeoutMs;
        ResponseTimeoutMs = responseTimeoutMs;
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
    public uint ConnectionTimeoutMs { get; set; }
    public uint ResponseTimeoutMs { get; set; }

    object ICloneable.Clone() => Clone();

    public ISocketConnectionConfig Clone() =>
        new SocketConnectionConfig(InstanceName, SocketDescription,
            ConnectionAttributes, SendBufferSize, ReceiveBufferSize, Hostname, SubnetMask,
            PortIsDynamic, PortStartRange, PortEndRange);

    public IConnectionConfig
        ToConnectionConfig(ConnectionDirectionType connectionDirectionType = ConnectionDirectionType.Both) =>
        new ConnectionConfig(InstanceName, Hostname?.ToString() ?? "", PortStartRange, connectionDirectionType
            , SubnetMask?.ToString(), 1_000);

    protected bool Equals(ISocketReceiverConfig other)
    {
        var instanceNameSame = InstanceName == other.InstanceName;
        var sendBufferSizeSame = SendBufferSize == other.SendBufferSize;
        var receiveBufferSizeSame = ReceiveBufferSize == other.ReceiveBufferSize;
        var connectionAttsSame = ConnectionAttributes == other.ConnectionAttributes;
        var descriptionSame = SocketDescription.Equals(other.SocketDescription);
        var hostNameSame = Equals(Hostname, other.Hostname);
        var subNetSame = Equals(SubnetMask, other.SubnetMask);
        var portDynamicSame = PortIsDynamic == other.PortIsDynamic;
        var portStartRangeSame = PortStartRange == other.PortStartRange;
        var portEndRangeSame = PortEndRange == other.PortEndRange;
        var numIntervalsPerPollSame = NumberOfReceivesPerPoll == other.NumberOfReceivesPerPoll;
        var connectionTimeoutSame = ConnectionTimeoutMs == other.ConnectionTimeoutMs;

        return instanceNameSame && sendBufferSizeSame && receiveBufferSizeSame &&
               connectionAttsSame && descriptionSame && hostNameSame && subNetSame && portDynamicSame &&
               portStartRangeSame && portEndRangeSame && numIntervalsPerPollSame && connectionTimeoutSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SocketConnectionConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(InstanceName);
        hashCode.Add(SendBufferSize);
        hashCode.Add(ReceiveBufferSize);
        hashCode.Add((int)ConnectionAttributes);
        hashCode.Add(SocketDescription);
        hashCode.Add(Hostname);
        hashCode.Add(SubnetMask);
        hashCode.Add(PortIsDynamic);
        hashCode.Add(PortStartRange);
        hashCode.Add(PortEndRange);
        hashCode.Add(NumberOfReceivesPerPoll);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"SocketConnectionConfig ({nameof(InstanceName)}: {InstanceName}, {nameof(SocketDescription)}: " +
        $"{SocketDescription}, {nameof(Hostname)}: {Hostname}, {nameof(SubnetMask)}: {SubnetMask}, " +
        $"{nameof(PortIsDynamic)}: {PortIsDynamic}, {nameof(PortStartRange)}: {PortStartRange}, " +
        $"{nameof(PortEndRange)}: {PortEndRange}, {nameof(SendBufferSize)}: {SendBufferSize}, " +
        $"{nameof(ReceiveBufferSize)}: {ReceiveBufferSize}, " +
        $"{nameof(NumberOfReceivesPerPoll)}: {NumberOfReceivesPerPoll}, " +
        $"{nameof(ConnectionAttributes)}: {ConnectionAttributes})";
}
