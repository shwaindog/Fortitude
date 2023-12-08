#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsApi.Trading.Orders.Server;

#endregion

namespace FortitudeMarketsApi.Trading.Replay;

public interface IReplayMessage : ITradingMessage, IStoreState<IReplayMessage>
{
    ReplayMessageType ReplayMessageType { get; set; }
    IOrderUpdate? PastOrder { get; set; }
    IExecutionUpdate? PastExecutionUpdate { get; set; }
    new IReplayMessage Clone();
}
