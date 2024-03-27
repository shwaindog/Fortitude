#region

using System.Net;
using FortitudeCommon.Types;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Config;

public interface IEndpointConfig : ICloneable<IEndpointConfig>
{
    string Hostname { get; set; }
    ushort Port { get; set; }
    string InstanceName { get; set; }
    string? SubnetMask { get; set; }
    IPAddress? SubnetMaskIpAddress { get; }
}

public class EndpointConfig : IEndpointConfig
{
    public EndpointConfig(string hostname, ushort port, string? instanceName = null, string? subnetMask = null)
    {
        Hostname = hostname;
        Port = port;
        SubnetMask = subnetMask;
        InstanceName = instanceName ?? $"{hostname}:{port}";
    }

    public EndpointConfig(IEndpointConfig toClone)
    {
        Hostname = toClone.Hostname;
        Port = toClone.Port;
        SubnetMask = toClone.SubnetMask;
        InstanceName = toClone.InstanceName;
    }

    public string InstanceName { get; set; }
    public string Hostname { get; set; }
    public string? SubnetMask { get; set; }
    public IPAddress? SubnetMaskIpAddress => SubnetMask != null ? IPAddress.Parse(SubnetMask) : null;
    public ushort Port { get; set; }

    object ICloneable.Clone() => Clone();

    public IEndpointConfig Clone() => new EndpointConfig(this);

    protected bool Equals(IEndpointConfig other)
    {
        var hostNameSame = Equals(Hostname, other.Hostname);
        var portSame = Port == other.Port;
        var subNetSame = Equals(SubnetMask, other.SubnetMask);
        var instanceNameSame = Equals(InstanceName, other.InstanceName);

        return hostNameSame && portSame && instanceNameSame && subNetSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((EndpointConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(Hostname);
        hashCode.Add(Port);
        hashCode.Add(InstanceName);
        hashCode.Add(SubnetMask);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"SocketConnectionConfig ({nameof(Hostname)}: {Hostname}, {nameof(Port)}: {Port}, {nameof(InstanceName)}: {InstanceName}, " +
        $"{nameof(SubnetMask)}: {SubnetMask})";
}
