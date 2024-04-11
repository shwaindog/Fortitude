#region

using FortitudeCommon.Configuration;
using FortitudeCommon.DataStructures.Lists;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public interface IClusterConfig
{
    IClusterInstance ClusterConnectivityEndpoint { get; set; }
    IEnumerable<IRemoteServiceConfig> RemoteServiceConfigs { get; set; }
    IEnumerable<ILocalServiceConfig> LocalServiceConfigs { get; set; }
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
    private object? ignoreSuppressWarnings;

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

    public IEnumerable<IRemoteServiceConfig> RemoteServiceConfigs
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<IRemoteServiceConfig>>();
            foreach (var serviceName in GetSection(nameof(RemoteServiceConfigs)).GetChildren())
                if (serviceName["Name"] != null)
                    autoRecycleList.Add(new RemoteServiceConfig(ConfigRoot, serviceName.Path));
            return autoRecycleList;
        }
        set
        {
            var oldCount = RemoteServiceConfigs.Count();
            var i = 0;
            foreach (var remoteServiceConfig in value)
            {
                ignoreSuppressWarnings
                    = new RemoteServiceConfig(remoteServiceConfig, ConfigRoot, Path + ":" + nameof(RemoteServiceConfigs) + $":{i}");
                i++;
            }

            for (var j = i; j < oldCount; j++) RemoteServiceConfig.ClearValues(ConfigRoot, Path + ":" + nameof(RemoteServiceConfigs) + $":{i}");
        }
    }

    public IEnumerable<ILocalServiceConfig> LocalServiceConfigs
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<AutoRecycledEnumerable<ILocalServiceConfig>>();
            foreach (var serviceName in GetSection(nameof(LocalServiceConfigs)).GetChildren())
                autoRecycleList.Add(new LocalServiceConfig(ConfigRoot, serviceName.Path));
            return autoRecycleList;
        }
        set
        {
            var i = 0;
            foreach (var remoteServiceConfig in value)
            {
                ignoreSuppressWarnings = new LocalServiceConfig(remoteServiceConfig, ConfigRoot, Path + ":" + nameof(LocalServiceConfigs) + $":{i}");
                i++;
            }
        }
    }
}
