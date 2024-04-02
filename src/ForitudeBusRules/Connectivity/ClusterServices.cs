#region

using FortitudeBusRules.Config;
using FortitudeBusRules.MessageBus;

#endregion

namespace FortitudeBusRules.Connectivity;

public class ClusterServices
{
    private readonly IClusterConfig clusterConfig;
    private readonly IEventBus eventBus;

    public ClusterServices(IClusterConfig clusterConfig, IEventBus eventBus)
    {
        this.clusterConfig = clusterConfig;
        this.eventBus = eventBus;
    }

    public void Start()
    {
        if (clusterConfig.ClusterConnectivityEndpoint != null) StartInterClusterCommunications();
    }

    private void StartInterClusterCommunications()
    {
        throw new NotImplementedException();
    }
}
