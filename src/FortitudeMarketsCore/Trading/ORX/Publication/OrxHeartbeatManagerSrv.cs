using System.Threading;
using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.ORX.Publication
{
    public class OrxHeartbeatManagerSrv
    {
        private readonly OrxServerMessaging orxServerMessaging;
        private readonly int frequencyMs;
        private AutoResetEvent are = new AutoResetEvent(false);

        public OrxHeartbeatManagerSrv(OrxServerMessaging orxServerMessaging, int frequencyMs)
        {
            this.orxServerMessaging = orxServerMessaging;
            this.frequencyMs = frequencyMs;

            orxServerMessaging.OnConnected += () => SendHeartbeat(null, true);

            orxServerMessaging.RegisterSerializer<OrxHeartbeatMessage>();
        }

        private void SendHeartbeat(object state, bool timedOut)
        {
            if (orxServerMessaging.IsConnected)
            {
                orxServerMessaging.Broadcast(new OrxHeartbeatMessage());
                ThreadPool.RegisterWaitForSingleObject(are, SendHeartbeat, null, frequencyMs, true);
            }
        }
    }
}