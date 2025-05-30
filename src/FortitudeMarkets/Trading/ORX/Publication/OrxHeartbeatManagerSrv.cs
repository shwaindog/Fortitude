#region

using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Publication;

public class OrxHeartbeatManagerSrv
{
    private readonly AutoResetEvent are = new(false);
    private readonly int frequencyMs;
    private readonly OrxServerMessaging orxServerMessaging;

    public OrxHeartbeatManagerSrv(OrxServerMessaging orxServerMessaging, int frequencyMs)
    {
        this.orxServerMessaging = orxServerMessaging;
        this.frequencyMs = frequencyMs;

        orxServerMessaging.Connected += () => SendHeartbeat(new object(), true);

        orxServerMessaging.SerializationRepository.RegisterSerializer<OrxHeartbeatMessage>();
    }

    private void SendHeartbeat(object state, bool timedOut)
    {
        if (orxServerMessaging.IsStarted)
        {
            orxServerMessaging.Broadcast(new OrxHeartbeatMessage());
            ThreadPool.RegisterWaitForSingleObject(are, SendHeartbeat!, null, frequencyMs, true);
        }
    }
}
