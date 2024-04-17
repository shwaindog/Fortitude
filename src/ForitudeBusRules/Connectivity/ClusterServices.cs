#region

using FortitudeBusRules.BusMessaging;
using FortitudeBusRules.Config;

#endregion

namespace FortitudeBusRules.Connectivity;

public class ClusterServices
{
    private readonly IClusterConfig clusterConfig;
    private readonly IMessageBus messageBus;

    public ClusterServices(IClusterConfig clusterConfig, IMessageBus messageBus)
    {
        this.clusterConfig = clusterConfig;
        this.messageBus = messageBus;
    }

    public void Start()
    {
        if (clusterConfig.ClusterConnectivityEndpoint != null) StartInterClusterCommunications();
    }

    private void StartInterClusterCommunications() { }
}
