#region

using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

#endregion

namespace FortitudeBusRules.Config;

public class ClusterConfig : ConfigurationSection
{
    public const string DefaultClusterConfigPath = BusRulesConfig.DefaultBusRulesConfigPath + ":" + "ClusterConfig";

    private static readonly Dictionary<string, string?> Defaults = new()
    {
        { "ClusterConnectivityEndpoint:Name", "ClusterConnectionManager" }, { "ClusterConnectivityEndpoint:StreamType", "InterClusterCommsManager" }
        , { "ClusterConnectivityEndpoint:ClusterAccessibleHostname", "localhost" }, { "ClusterConnectivityEndpoint:ClusterAccessiblePort", "9090" }
        , { "ClusterConnectivityEndpoint:LocalConnectionConfig:ConversationProtocol", "TcpAcceptor" }
        , { "ClusterConnectivityEndpoint:LocalConnectionConfig:AvailableConnections:0:Hostname", "0.0.0.0" }
        , { "ClusterConnectivityEndpoint:LocalConnectionConfig:AvailableConnections:0:Port", "9090" }
    };

    private readonly IConfigurationRoot configRoot;

    private LocalServiceEndpoints? clusterConnectivityEndpoint;
    private List<LocalServiceEndpoints>? localServiceConfigs;
    private List<RemoteServiceConfig>? remoteServiceConfigs;

    public ClusterConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path)
    {
        this.configRoot = configRoot;
        foreach (var checkDefault in Defaults) this[checkDefault.Key] ??= checkDefault.Value;
    }

    public ClusterConfig() : this(new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build(), DefaultClusterConfigPath) { }


    public ClusterConfig(ClusterConfig toClone, IConfigurationRoot configRoot, string path) : this(configRoot, path)
    {
        ClusterConnectivityEndpoint = toClone.ClusterConnectivityEndpoint;
        RemoteServiceConfigs = toClone.RemoteServiceConfigs;
        AdditionalLocalServices = toClone.AdditionalLocalServices;
    }

    public LocalServiceEndpoints ClusterConnectivityEndpoint
    {
        get => clusterConnectivityEndpoint ??= new LocalServiceEndpoints(configRoot, Path + ":" + nameof(ClusterConnectivityEndpoint));
        set => clusterConnectivityEndpoint = new LocalServiceEndpoints(value, configRoot, Path + ":" + nameof(ClusterConnectivityEndpoint));
    }

    public List<RemoteServiceConfig> RemoteServiceConfigs
    {
        get
        {
            if (remoteServiceConfigs != null) return remoteServiceConfigs;
            remoteServiceConfigs = new List<RemoteServiceConfig>();
            foreach (var serviceName in GetSection(nameof(RemoteServiceConfigs)).GetChildren())
                remoteServiceConfigs.Add(new RemoteServiceConfig(configRoot, serviceName.Path));
            return remoteServiceConfigs;
        }
        set
        {
            remoteServiceConfigs = new List<RemoteServiceConfig>();
            for (var i = 0; i < value.Count; i++)
                remoteServiceConfigs.Add(new RemoteServiceConfig(value[i], configRoot, Path + ":" + nameof(RemoteServiceConfigs) + $":{i}"));
        }
    }

    public List<LocalServiceEndpoints> AdditionalLocalServices
    {
        get
        {
            if (localServiceConfigs != null) return localServiceConfigs;
            localServiceConfigs = new List<LocalServiceEndpoints>();
            foreach (var serviceName in GetSection(nameof(AdditionalLocalServices)).GetChildren())
                localServiceConfigs.Add(new LocalServiceEndpoints(configRoot, serviceName.Path));
            return localServiceConfigs;
        }
        set
        {
            localServiceConfigs = new List<LocalServiceEndpoints>();
            for (var i = 0; i < value.Count; i++)
                localServiceConfigs.Add(new LocalServiceEndpoints(value[i], configRoot, Path + ":" + nameof(AdditionalLocalServices) + $":{i}"));
        }
    }
}

public class LocalServiceEndpoints : ConfigurationSection
{
    public const string DefaultLocalServiceEndpointsPath = ClusterConfig.DefaultClusterConfigPath + ":" + "LocalServiceEndpoints";
    private readonly IConfigurationRoot configRoot;

    private INetworkTopicConnectionConfig? serviceStartConnectionConfig;

    public LocalServiceEndpoints(IConfigurationRoot configRoot, string path) : base(configRoot, path) => this.configRoot = configRoot;

    public LocalServiceEndpoints(LocalServiceEndpoints toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        Name = toClone.Name;
        StreamType = toClone.StreamType;
        ClusterAccessibleHostname = toClone.ClusterAccessibleHostname;
        ClusterAccessiblePort = toClone.ClusterAccessiblePort;
        ServiceStartConnectionConfig = toClone.ServiceStartConnectionConfig.Clone();
    }

    public string Name
    {
        get => this[nameof(Name)]!;
        set => this[nameof(Name)] = value;
    }

    public string StreamType
    {
        get => this[nameof(StreamType)]!;
        set => this[nameof(StreamType)] = value;
    }

    public string ClusterAccessibleHostname
    {
        get => this[nameof(ClusterAccessibleHostname)]!;
        set => this[nameof(ClusterAccessibleHostname)] = value;
    }

    public ushort ClusterAccessiblePort
    {
        get => ushort.Parse(this[nameof(ClusterAccessiblePort)]!);
        set => this[nameof(ClusterAccessiblePort)] = value.ToString();
    }

    public INetworkTopicConnectionConfig ServiceStartConnectionConfig
    {
        get => serviceStartConnectionConfig ??= new NetworkTopicConnectionConfig(configRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
        set => serviceStartConnectionConfig = new NetworkTopicConnectionConfig(value, configRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
    }
}

public class RemoteServiceConfig : ConfigurationSection
{
    public const string DefaultRemoteServiceConfigPath = ClusterConfig.DefaultClusterConfigPath + ":" + "RemoteServiceConfig";
    private readonly IConfigurationRoot configRoot;
    private List<string>? connectRemoteServiceNames;
    private INetworkTopicConnectionConfig? remoteServiceConnectionConfig;

    public RemoteServiceConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) => this.configRoot = configRoot;

    public RemoteServiceConfig(RemoteServiceConfig toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        Name = toClone.Name;
        StreamType = toClone.StreamType;
        Description = toClone.Description;
        RemoteServiceConnectionConfig = toClone.RemoteServiceConnectionConfig.Clone();
        ConnectToRemoteServiceNames = toClone.ConnectToRemoteServiceNames.ToList();
    }

    public string Name
    {
        get => this[nameof(Name)]!;
        set => this[nameof(Name)] = value;
    }

    public string StreamType
    {
        get => this[nameof(StreamType)]!;
        set => this[nameof(StreamType)] = value;
    }

    public string Description
    {
        get => this[nameof(Description)]!;
        set => this[nameof(Description)] = value;
    }

    public INetworkTopicConnectionConfig RemoteServiceConnectionConfig
    {
        get => remoteServiceConnectionConfig ??= new NetworkTopicConnectionConfig(configRoot, Path + ":" + nameof(RemoteServiceConnectionConfig));
        set =>
            remoteServiceConnectionConfig = new NetworkTopicConnectionConfig(value, configRoot, Path + ":" + nameof(RemoteServiceConnectionConfig));
    }

    public List<string> ConnectToRemoteServiceNames
    {
        get
        {
            if (connectRemoteServiceNames != null) return connectRemoteServiceNames;
            connectRemoteServiceNames = new List<string>();
            foreach (var serviceName in GetSection(nameof(ConnectToRemoteServiceNames)).GetChildren())
                connectRemoteServiceNames.Add(serviceName!.Value!);
            return connectRemoteServiceNames;
        }
        set
        {
            connectRemoteServiceNames = new List<string>();
            for (var i = 0; i < value.Count; i++)
            {
                this[nameof(ConnectToRemoteServiceNames) + $":{i}"] = value[i];
                connectRemoteServiceNames.Add(value[i]);
            }
        }
    }
}
