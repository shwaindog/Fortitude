#region

using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;

#endregion

namespace FortitudeMarketsApi.Trading.Replay;

public interface IReplayMessage : ITradingMessage
{
    ReplayMessageType ReplayMessageType { get; set; }
    IOrderUpdate? PastOrder { get; set; }
    IExecutionUpdate? PastExecutionUpdate { get; set; }
    new IReplayMessage Clone();
}
