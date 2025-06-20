﻿#region

using System.Net;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

#endregion

namespace FortitudeIO.Transports.Network.Config;




public interface IEndpointConfig : ICloneable<IEndpointConfig>
{
    string  Hostname     { get; set; }
    ushort  Port         { get; set; }
    string  InstanceName { get; set; }
    string? SubnetMask   { get; set; }

    CountryCityCodes HostLocationCode { get; set; }

    IPAddress? SubnetMaskIpAddress { get; }
}

public class EndpointConfig : ConfigSection, IEndpointConfig
{
    public EndpointConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public EndpointConfig(string hostname, ushort port, CountryCityCodes hostCountryCityCode, string? instanceName = null, string? subnetMask = null)
    {
        Hostname         = hostname;
        Port             = port;
        HostLocationCode = hostCountryCityCode;
        SubnetMask       = subnetMask;
        InstanceName     = instanceName ?? $"{hostname}:{port}";
    }

    public EndpointConfig(IEndpointConfig toClone) : this(toClone, new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build(), "") { }

    public EndpointConfig(IEndpointConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Hostname         = toClone.Hostname;
        Port             = toClone.Port;
        SubnetMask       = toClone.SubnetMask;
        InstanceName     = toClone.InstanceName;
        HostLocationCode = toClone.HostLocationCode;
    }

    public string InstanceName
    {
        get => this[nameof(InstanceName)]!;
        set => this[nameof(InstanceName)] = value;
    }

    public string Hostname
    {
        get => this[nameof(Hostname)]!;
        set => this[nameof(Hostname)] = value;
    }

    public string? SubnetMask
    {
        get => this[nameof(SubnetMask)];
        set => this[nameof(SubnetMask)] = value;
    }

    public IPAddress? SubnetMaskIpAddress => SubnetMask != null ? IPAddress.Parse(SubnetMask) : null;

    public ushort Port
    {
        get => ushort.Parse(this[nameof(Port)]!);
        set => this[nameof(Port)] = value.ToString();
    }

    public CountryCityCodes HostLocationCode
    {
        get => Enum.Parse<CountryCityCodes>(this[nameof(HostLocationCode)]!);
        set => this[nameof(HostLocationCode)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    public IEndpointConfig Clone() => new EndpointConfig(this);

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(InstanceName)}"]     = null;
        root[$"{path}{Split}{nameof(Hostname)}"]         = null;
        root[$"{path}{Split}{nameof(SubnetMask)}"]       = null;
        root[$"{path}{Split}{nameof(SubnetMask)}"]       = null;
        root[$"{path}{Split}{nameof(Port)}"]             = null;
        root[$"{path}{Split}{nameof(HostLocationCode)}"] = CountryCityCodes.Unknown.ToString();
    }

    protected bool Equals(IEndpointConfig other)
    {
        var hostNameSame     = Equals(Hostname, other.Hostname);
        var portSame         = Port == other.Port;
        var subNetSame       = Equals(SubnetMask, other.SubnetMask);
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
        $"{nameof(EndpointConfig)} ({nameof(Hostname)}: {Hostname}, {nameof(Port)}: {Port}, {nameof(InstanceName)}: {InstanceName}, " +
        $"{nameof(SubnetMask)}: {SubnetMask})";
}
