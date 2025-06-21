using System.Net;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

namespace FortitudeIO.Config;

public interface ILocalEndpointConfig : ICloneable<ILocalEndpointConfig>
{
    const string DefaultInterfaceIp = "0.0.0.0"; // all network interface cards  

    string  InterfaceIp  { get; set; }
    ushort  Port         { get; set; }
    string? InstanceName { get; set; }
    string? SubnetMask   { get; set; }

    IPAddress? SubnetMaskIpAddress { get; }
    IPAddress InterfaceIpAddress { get; }
}

public class LocalEndpointConfig : ConfigSection, ILocalEndpointConfig
{
    public LocalEndpointConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public LocalEndpointConfig(ushort port, string interfaceIp = ILocalEndpointConfig.DefaultInterfaceIp, string? instanceName = null, string? subnetMask = null)
    {
        InterfaceIp         = interfaceIp;
        Port             = port;
        SubnetMask       = subnetMask;
        InstanceName     = instanceName ?? $"{interfaceIp}:{port}";
    }

    public LocalEndpointConfig(ILocalEndpointConfig toClone) : this(toClone, new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build(), "") { }

    public LocalEndpointConfig(ILocalEndpointConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        InterfaceIp      = toClone.InterfaceIp;
        Port             = toClone.Port;
        SubnetMask       = toClone.SubnetMask;
        InstanceName     = toClone.InstanceName;
    }

    public string? InstanceName
    {
        get => this[nameof(InstanceName)];
        set => this[nameof(InstanceName)] = value;
    }

    public string InterfaceIp
    {
        get => this[nameof(InterfaceIp)] ?? ILocalEndpointConfig.DefaultInterfaceIp;
        set => this[nameof(InterfaceIp)] = value;
    }

    public string? SubnetMask
    {
        get => this[nameof(SubnetMask)];
        set => this[nameof(SubnetMask)] = value;
    }

    public IPAddress? SubnetMaskIpAddress => SubnetMask != null ? IPAddress.Parse(SubnetMask) : null;
    public IPAddress InterfaceIpAddress  => IPAddress.Parse(InterfaceIp);

    public ushort Port
    {
        get => ushort.Parse(this[nameof(Port)]!);
        set => this[nameof(Port)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    public ILocalEndpointConfig Clone() => new LocalEndpointConfig(this);

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(InstanceName)}"]     = null;
        root[$"{path}{Split}{nameof(InterfaceIp)}"]         = null;
        root[$"{path}{Split}{nameof(SubnetMask)}"]       = null;
        root[$"{path}{Split}{nameof(SubnetMask)}"]       = null;
        root[$"{path}{Split}{nameof(Port)}"]             = null;
    }

    protected bool Equals(ILocalEndpointConfig other)
    {
        var adapterIpSame    = InterfaceIp == other.InterfaceIp;
        var portSame         = Port == other.Port;
        var subNetSame       = SubnetMask == other.SubnetMask;
        var instanceNameSame = InstanceName == other.InstanceName;

        return adapterIpSame && portSame && instanceNameSame && subNetSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Transports.Network.Config.EndpointConfig)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(InterfaceIp);
        hashCode.Add(Port);
        hashCode.Add(InstanceName);
        hashCode.Add(SubnetMask);
        return hashCode.ToHashCode();
    }

    public override string ToString() =>
        $"{nameof(LocalEndpointConfig)} ({nameof(InterfaceIp)}: {InterfaceIp}, {nameof(Port)}: {Port}, {nameof(InstanceName)}: {InstanceName}, " +
        $"{nameof(SubnetMask)}: {SubnetMask})";
}
