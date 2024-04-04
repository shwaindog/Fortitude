#region

using FortitudeCommon.Configuration;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IClusterConfig
{
    IClusterInstance ClusterConnectivityEndpoint { get; set; }
    List<IRemoteServiceConfig> RemoteServiceConfigs { get; set; }
    List<ILocalServiceConfig> LocalServiceConfigs { get; set; }
}

public class ClusterConfig : ConfigSection, IClusterConfig
{
    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { "ClusterConnectivityEndpoint:Name", "ClusterConnectionManager" }, { "ClusterConnectivityEndpoint:StreamType", "InterClusterCommsManager" }
        , { "ClusterConnectivityEndpoint:ClusterAccessibleHostname", "localhost" }, { "ClusterConnectivityEndpoint:ClusterAccessiblePort", "9090" }
        , { "ClusterConnectivityEndpoint:LocalConnectionConfig:ConversationProtocol", "TcpAcceptor" }
        , { "ClusterConnectivityEndpoint:LocalConnectionConfig:AvailableConnections:0:Hostname", "0.0.0.0" }
        , { "ClusterConnectivityEndpoint:LocalConnectionConfig:AvailableConnections:0:Port", "9090" }
    };

    private IClusterInstance? clusterConnectivityEndpoint;
    private List<IRemoteServiceConfig> lastestRemoteServiceConfigs = new();
    private List<ILocalServiceConfig> latestLocalServiceConfigs = new();

    public ClusterConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path)
    {
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public ClusterConfig() : this(InMemoryConfigRoot, InMemoryPath) { }


    public ClusterConfig(IClusterConfig toClone, IConfigurationRoot configRoot, string path) : this(configRoot, path)
    {
        ClusterConnectivityEndpoint = toClone.ClusterConnectivityEndpoint;
        RemoteServiceConfigs = toClone.RemoteServiceConfigs;
        LocalServiceConfigs = toClone.LocalServiceConfigs;
    }

    public ClusterConfig(IClusterConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public IClusterInstance ClusterConnectivityEndpoint
    {
        get => clusterConnectivityEndpoint ??= new ClusterInstance(ConfigRoot, Path + ":" + nameof(ClusterConnectivityEndpoint));
        set =>
            clusterConnectivityEndpoint
                = value != null ? new ClusterInstance(value, ConfigRoot, Path + ":" + nameof(ClusterConnectivityEndpoint)) : null;
    }

    public List<IRemoteServiceConfig> RemoteServiceConfigs
    {
        get
        {
            lastestRemoteServiceConfigs.Clear();
            foreach (var serviceName in GetSection(nameof(RemoteServiceConfigs)).GetChildren())
                lastestRemoteServiceConfigs.Add(new RemoteServiceConfig(ConfigRoot, serviceName.Path));
            return lastestRemoteServiceConfigs;
        }
        set
        {
            lastestRemoteServiceConfigs.Clear();
            for (var i = 0; i < value.Count; i++)
                lastestRemoteServiceConfigs.Add(new RemoteServiceConfig(value[i], ConfigRoot, Path + ":" + nameof(RemoteServiceConfigs) + $":{i}"));
        }
    }

    public List<ILocalServiceConfig> LocalServiceConfigs
    {
        get
        {
            latestLocalServiceConfigs.Clear();
            foreach (var serviceName in GetSection(nameof(LocalServiceConfigs)).GetChildren())
                latestLocalServiceConfigs.Add(new LocalServiceConfig(ConfigRoot, serviceName.Path));
            return latestLocalServiceConfigs;
        }
        set
        {
            latestLocalServiceConfigs.Clear();
            for (var i = 0; i < value.Count; i++)
                latestLocalServiceConfigs.Add(new LocalServiceConfig(value[i], ConfigRoot, Path + ":" + nameof(LocalServiceConfigs) + $":{i}"));
        }
    }
}
