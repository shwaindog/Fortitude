#region

using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Publication;

public class OrxHeartbeatManagerSrv
{
    private readonly int frequencyMs;
    private readonly OrxServerMessaging orxServerMessaging;
    private readonly AutoResetEvent are = new(false);

    public OrxHeartbeatManagerSrv(OrxServerMessaging orxServerMessaging, int frequencyMs)
    {
        this.orxServerMessaging = orxServerMessaging;
        this.frequencyMs = frequencyMs;

        orxServerMessaging.OnConnected += () => SendHeartbeat(new object(), true);

        orxServerMessaging.RegisterSerializer<OrxHeartbeatMessage>();
    }

    private void SendHeartbeat(object state, bool timedOut)
    {
        if (orxServerMessaging.IsConnected)
        {
            orxServerMessaging.Broadcast(new OrxHeartbeatMessage());
            ThreadPool.RegisterWaitForSingleObject(are, SendHeartbeat!, null, frequencyMs, true);
        }
    }
}
