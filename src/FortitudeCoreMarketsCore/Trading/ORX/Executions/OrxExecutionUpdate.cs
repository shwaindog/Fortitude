#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Executions;

public sealed class OrxExecutionUpdate : OrxTradingMessage, IExecutionUpdate
{
    public OrxExecutionUpdate() { }

    public OrxExecutionUpdate(IExecutionUpdate toClone) : base(toClone)
    {
        Execution = new OrxExecution(toClone.Execution!);
        ExecutionUpdateType = toClone.ExecutionUpdateType;
        SocketReceivedTime = toClone.SocketReceivedTime;
        AdapterProcessedTime = toClone.AdapterProcessedTime;
        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public OrxExecutionUpdate(OrxExecution execution, ExecutionUpdateType executionUpdateType
        , DateTime socketReceivedTime,
        DateTime adapterProcessedTime, DateTime clientReceivedTime)
    {
        Execution = execution;
        ExecutionUpdateType = executionUpdateType;
        SocketReceivedTime = socketReceivedTime;
        AdapterProcessedTime = adapterProcessedTime;
        ClientReceivedTime = clientReceivedTime;
    }

    [OrxMandatoryField(10)] public OrxExecution? Execution { get; set; }

    public override uint MessageId => (uint)TradingMessageIds.ExecutionUpdate;

    IExecution? IExecutionUpdate.Execution
    {
        get => Execution;
        set => Execution = (OrxExecution?)value;
    }

    [OrxMandatoryField(11)] public ExecutionUpdateType ExecutionUpdateType { get; set; }

    [OrxMandatoryField(12)] public DateTime SocketReceivedTime { get; set; }

    [OrxMandatoryField(13)] public DateTime AdapterProcessedTime { get; set; }

    public DateTime ClientReceivedTime { get; set; }

    public IExecutionUpdate Clone() => new OrxExecutionUpdate(this);

    public void CopyFrom(IExecutionUpdate executionUpdate, IRecycler orxRecyclingFactory)
    {
        base.CopyFrom(executionUpdate, orxRecyclingFactory);
        if (executionUpdate.Execution != null)
        {
            var orxExecution = orxRecyclingFactory.Borrow<OrxExecution>();
            orxExecution.CopyFrom(executionUpdate.Execution, orxRecyclingFactory);
            Execution = orxExecution;
        }

        ExecutionUpdateType = executionUpdate.ExecutionUpdateType;
        SocketReceivedTime = executionUpdate.SocketReceivedTime;
        AdapterProcessedTime = executionUpdate.AdapterProcessedTime;
        ClientReceivedTime = executionUpdate.ClientReceivedTime;
    }
}
