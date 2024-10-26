#region

using FortitudeCommon.Types;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.Orders.Server;

#endregion

namespace FortitudeMarkets.Trading.Replay;

public interface IReplayMessage : ITradingMessage, IStoreState<IReplayMessage>
{
    ReplayMessageType ReplayMessageType { get; set; }
    IOrderUpdate? PastOrder { get; set; }
    IExecutionUpdate? PastExecutionUpdate { get; set; }
    new IReplayMessage Clone();
}
