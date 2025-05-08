// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarkets.Trading.Executions;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Executions;

public sealed class OrxExecutionUpdate : OrxTradingMessage, IExecutionUpdate, ITransferState<OrxExecutionUpdate>
{
    public OrxExecutionUpdate() { }

    public OrxExecutionUpdate(IExecutionUpdate toClone) : base(toClone)
    {
        Execution           = new OrxExecution(toClone.Execution!);
        ExecutionUpdateType = toClone.ExecutionUpdateType;

        SocketReceivedTime = toClone.SocketReceivedTime;

        AdapterProcessedTime = toClone.AdapterProcessedTime;

        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public OrxExecutionUpdate
    (OrxExecution execution, ExecutionUpdateType executionUpdateType
      , DateTime socketReceivedTime,
        DateTime adapterProcessedTime, DateTime clientReceivedTime)
    {
        Execution = execution;

        ExecutionUpdateType = executionUpdateType;
        SocketReceivedTime  = socketReceivedTime;

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

    public override IVersionedMessage CopyFrom
    (IVersionedMessage source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        CopyFrom((IExecutionUpdate)source, copyMergeFlags);

    public override void StateReset()
    {
        Execution?.DecrementRefCount();
        Execution = null;

        ExecutionUpdateType = ExecutionUpdateType.Unknown;
        SocketReceivedTime  = DateTimeConstants.UnixEpoch;

        AdapterProcessedTime = DateTimeConstants.UnixEpoch;

        ClientReceivedTime = DateTimeConstants.UnixEpoch;
        base.StateReset();
    }

    public IExecutionUpdate CopyFrom
    (IExecutionUpdate executionUpdate
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(executionUpdate, copyMergeFlags);
        if (executionUpdate.Execution != null)
        {
            var orxExecution = Recycler!.Borrow<OrxExecution>();
            orxExecution.CopyFrom(executionUpdate.Execution, copyMergeFlags);
            Execution = orxExecution;
        }

        ExecutionUpdateType = executionUpdate.ExecutionUpdateType;
        SocketReceivedTime  = executionUpdate.SocketReceivedTime;

        AdapterProcessedTime = executionUpdate.AdapterProcessedTime;

        ClientReceivedTime = executionUpdate.ClientReceivedTime;
        return this;
    }

    public OrxExecutionUpdate CopyFrom
    (OrxExecutionUpdate source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) =>
        (OrxExecutionUpdate)CopyFrom((IVersionedMessage)source, copyMergeFlags);


    public override IExecutionUpdate Clone() => Recycler?.Borrow<OrxExecutionUpdate>().CopyFrom(this) ?? new OrxExecutionUpdate(this);
}
