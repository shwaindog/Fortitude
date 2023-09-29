using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;
using FortitudeMarketsApi.Trading.Replay;
using FortitudeMarketsCore.Trading.ORX.Session;

namespace FortitudeMarketsCore.Trading.Replay
{
    public class ReplayMessage : TradingMessage, IReplayMessage
    {
        public override uint MessageId => (uint) TradingMessageIds.Replay;
        public ReplayMessageType ReplayMessageType { get; set; }
        public IOrderUpdate PastOrder { get; set; }
        public IExecutionUpdate PastExecutionUpdate { get; set; }
    }
}