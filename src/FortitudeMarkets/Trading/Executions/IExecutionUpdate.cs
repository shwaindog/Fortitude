#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public interface IExecutionUpdate : ITradingMessage, IStoreState<IExecutionUpdate>
{
    IExecution? Execution { get; set; }
    ExecutionUpdateType ExecutionUpdateType { get; set; }
    DateTime SocketReceivedTime { get; set; }
    DateTime AdapterProcessedTime { get; set; }
    DateTime ClientReceivedTime { get; set; }
}
