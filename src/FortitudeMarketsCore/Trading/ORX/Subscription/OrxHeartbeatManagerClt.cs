#region

using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Subscription;

public class OrxHeartbeatManagerClt
{
    private readonly AutoResetEvent are = new(false);
    private readonly int frequencyMs;
    private readonly OrxTradingClientMessaging orxClient;

    public OrxHeartbeatManagerClt(OrxTradingClientMessaging orxClient, int frequencyMs)
    {
        this.orxClient = orxClient;
        this.frequencyMs = frequencyMs;

        orxClient.Connected += () => SendHeartbeat(new object(), true);

        orxClient.SerializationRepository.RegisterSerializer<OrxHeartbeatMessage>();
    }

    private void SendHeartbeat(object state, bool timedOut)
    {
        if (orxClient.IsStarted)
        {
            orxClient.Send(new OrxHeartbeatMessage());
            ThreadPool.RegisterWaitForSingleObject(are, SendHeartbeat!, null, frequencyMs, true);
        }
    }
}
