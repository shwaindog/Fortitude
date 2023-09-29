using System.Threading;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Subscription
{
    public class OrxHeartbeatManagerClt
    {
        private readonly OrxTradingClientMessaging orxClient;
        private readonly int frequencyMs;
        private readonly AutoResetEvent are = new AutoResetEvent(false);

        public OrxHeartbeatManagerClt(OrxTradingClientMessaging orxClient, int frequencyMs)
        {
            this.orxClient = orxClient;
            this.frequencyMs = frequencyMs;

            orxClient.OnConnected += () => SendHeartbeat(null, true);

            orxClient.StreamToPublisher.RegisterSerializer<OrxHeartbeatMessage>((uint)TradingMessageIds.Heartbeat);
        }

        private void SendHeartbeat(object state, bool timedOut)
        {
            if (orxClient.IsConnected)
            {
                orxClient.Send(new OrxHeartbeatMessage());
                ThreadPool.RegisterWaitForSingleObject(are, SendHeartbeat, null, frequencyMs, true);
            }
        }
    }
}
