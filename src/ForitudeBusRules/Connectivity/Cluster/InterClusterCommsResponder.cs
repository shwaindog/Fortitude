#region

using FortitudeIO.Protocols.ORX.ClientServer;

#endregion

namespace FortitudeBusRules.Connectivity.Cluster;

public class InterClusterCommsServer : OrxAuthenticationServer
{
    public InterClusterCommsServer(IOrxMessageResponder messageMessageResponder, byte currentVersion) :
        base(messageMessageResponder, currentVersion) { }
}
