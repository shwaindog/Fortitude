#region

using FortitudeCommon.Configuration;
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

public class ServiceEndpoint : ConfigSection, IServiceEndpoint
{
    private INetworkTopicConnectionConfig? clusterAccessibleClientConnectionConfig;
    private INetworkTopicConnectionConfig? serviceStartConnectionConfig;

    public ServiceEndpoint(IConfigurationRoot configRoot, string path) : base(configRoot, path) { }

    public ServiceEndpoint(IServiceEndpoint toClone, IConfigurationRoot root, string path) : this(root, path)
    {
        ClusterAccessibleClientConnectionConfig = toClone.ClusterAccessibleClientConnectionConfig;
        ServiceStartConnectionConfig = toClone.ServiceStartConnectionConfig;
    }

    public ServiceEndpoint(IServiceEndpoint toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public INetworkTopicConnectionConfig ClusterAccessibleClientConnectionConfig
    {
        get =>
            clusterAccessibleClientConnectionConfig
                = new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(ClusterAccessibleClientConnectionConfig));
        set =>
            clusterAccessibleClientConnectionConfig
                = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
    }

    public INetworkTopicConnectionConfig ServiceStartConnectionConfig
    {
        get => serviceStartConnectionConfig = new NetworkTopicConnectionConfig(ConfigRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
        set => serviceStartConnectionConfig = new NetworkTopicConnectionConfig(value, ConfigRoot, Path + ":" + nameof(ServiceStartConnectionConfig));
    }
}
