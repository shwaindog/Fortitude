#region

using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public enum ActivationState
{
    Disabled
    , OnFirstRequest
    , OnEachRequest
    , Manual
    , OnStartup
}

public interface IServiceEndpoint
{
    INetworkTopicConnectionConfig ServiceStartConnectionConfig { get; set; }
    INetworkTopicConnectionConfig ClusterAccessibleClientConnectionConfig { get; set; }
}

public class ServiceEndpoint : ConfigurationSection, IServiceEndpoint
{
    public const string DefaultLocalServiceEndpointsPath = ClusterConfig.DefaultClusterConfigPath + ":" + "ServiceEndpoint";
    private readonly IConfigurationRoot configRoot;
    private INetworkTopicConnectionConfig? clusterAccessibleClientConnectionConfig;
    private INetworkTopicConnectionConfig? serviceStartConnectionConfig;

    public ServiceEndpoint(IConfigurationRoot configRoot, string path) : base(configRoot, path) => this.configRoot = configRoot;

    public ServiceEndpoint(IServiceEndpoint toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        ClusterAccessibleClientConnectionConfig = toClone.ClusterAccessibleClientConnectionConfig;
        ServiceStartConnectionConfig = toClone.ServiceStartConnectionConfig;
    }

    public INetworkTopicConnectionConfig ClusterAccessibleClientConnectionConfig
    {
        get =>
            clusterAccessibleClientConnectionConfig
                = new NetworkTopicConnectionConfig(configRoot, Path + ":" + nameof(ClusterAccessibleClientConnectionConfig));
        set =>
            clusterAccessibleClientConnectionConfig
                = new NetworkTopicConnectionConfig(value, configRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
    }

    public INetworkTopicConnectionConfig ServiceStartConnectionConfig
    {
        get => serviceStartConnectionConfig = new NetworkTopicConnectionConfig(configRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
        set => serviceStartConnectionConfig = new NetworkTopicConnectionConfig(value, configRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
    }
}
