#region

using FortitudeCommon.Chronometry;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.Executions;

public class ExecutionUpdate : TradingMessage, IExecutionUpdate
{
    public ExecutionUpdate(IExecutionUpdate toClone) : base(toClone)
    {
        Execution = toClone.Execution;
        ExecutionUpdateType = toClone.ExecutionUpdateType;
        SocketReceivedTime = toClone.SocketReceivedTime;
        AdapterProcessedTime = toClone.AdapterProcessedTime;
        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public ExecutionUpdate(IExecution execution, ExecutionUpdateType executionUpdateType
        , DateTime? socketReceivedTime = null,
        DateTime? adapterProcessedTime = null, DateTime? clientReceivedTime = null)
    {
        Execution = execution;
        ExecutionUpdateType = executionUpdateType;
        SocketReceivedTime = socketReceivedTime ?? DateTimeConstants.UnixEpoch;
        AdapterProcessedTime = adapterProcessedTime ?? DateTimeConstants.UnixEpoch;
        ClientReceivedTime = clientReceivedTime ?? DateTimeConstants.UnixEpoch;
    }

    public override uint MessageId => (uint)TradingMessageIds.ExecutionUpdate;

    public IExecution? Execution { get; set; }
    public ExecutionUpdateType ExecutionUpdateType { get; set; }
    public DateTime SocketReceivedTime { get; set; }
    public DateTime AdapterProcessedTime { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public IExecutionUpdate Clone() => new ExecutionUpdate(this);
}
