using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;

namespace FortitudeMarketsApi.Trading.Replay
{
    public interface IReplayMessage : ITradingMessage
    {
        ReplayMessageType ReplayMessageType { get; set;  }
        IOrderUpdate PastOrder { get; set; }
        IExecutionUpdate PastExecutionUpdate { get; set; }
    }
}