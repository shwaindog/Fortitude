#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public class ExecutionUpdate : TradingMessage, IExecutionUpdate
{
    public ExecutionUpdate() { }

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
        Execution            = execution;
        ExecutionUpdateType  = executionUpdateType;
        SocketReceivedTime   = socketReceivedTime ?? DateTime.MinValue;
        AdapterProcessedTime = adapterProcessedTime ?? DateTime.MinValue;
        ClientReceivedTime   = clientReceivedTime ?? DateTime.MinValue;
    }

    public override uint MessageId => (uint)TradingMessageIds.ExecutionUpdate;

    public IExecution? Execution { get; set; }
    public ExecutionUpdateType ExecutionUpdateType { get; set; }
    public DateTime SocketReceivedTime { get; set; }
    public DateTime AdapterProcessedTime { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IExecutionUpdate executionUpdate)
        {
            Execution = executionUpdate.Execution?.SyncOrRecycle(Execution as Execution);
            ExecutionUpdateType = executionUpdate.ExecutionUpdateType;
            SocketReceivedTime = executionUpdate.SocketReceivedTime;
            AdapterProcessedTime = executionUpdate.AdapterProcessedTime;
            ClientReceivedTime = executionUpdate.ClientReceivedTime;
        }

        return this;
    }

    public IExecutionUpdate CopyFrom(IExecutionUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (IExecutionUpdate)CopyFrom((IVersionedMessage)source, copyMergeFlags);

    public override IExecutionUpdate Clone() =>
        (IExecutionUpdate?)Recycler?.Borrow<ExecutionUpdate>().CopyFrom(this) ?? new ExecutionUpdate(this);
}
