
namespace FortitudeMarketsCore.Trading.ORX.Session
{
    public sealed class OrxHeartbeatMessage : OrxTradingMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.Heartbeat;
    }
}
