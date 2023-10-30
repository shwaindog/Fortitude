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

        orxClient.OnConnected += () => SendHeartbeat(new object(), true);

        orxClient.StreamToPublisher.RegisterSerializer<OrxHeartbeatMessage>((uint)TradingMessageIds.Heartbeat);
    }

    private void SendHeartbeat(object state, bool timedOut)
    {
        if (orxClient.IsConnected)
        {
            orxClient.Send(new OrxHeartbeatMessage());
            ThreadPool.RegisterWaitForSingleObject(are, SendHeartbeat!, null, frequencyMs, true);
        }
    }
}
