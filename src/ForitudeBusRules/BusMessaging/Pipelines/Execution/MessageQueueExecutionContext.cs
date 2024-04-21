#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Pipelines.Execution;

public abstract class MessageQueueExecutionContext : RecyclableObject
{
    protected IRecycler EventQueueRecycler = null!;
    protected IMessageQueue MessageQueue = null!;
    protected IRule SenderRule = null!;

    public void Configure(IMessageQueue executionQueue, IRule senderRule)
    {
        MessageQueue = executionQueue;
        EventQueueRecycler = executionQueue.Context.PooledRecycler;
        SenderRule = senderRule;
    }

    public override void StateReset()
    {
        SenderRule = null!;
    }
}

public class MessageQueueExecutionContextAction<TP> : MessageQueueExecutionContext, IAlternativeExecutionContextAction<TP>
{
    public ValueTask Execute(Action<TP> methodToExecute, TP firstParam)
    {
        var noParamsSyncPayload = EventQueueRecycler.Borrow<OneParamSyncActionPayload<TP>>();
        noParamsSyncPayload.Configure(methodToExecute, firstParam);
        noParamsSyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(noParamsSyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextAction<TP>)}.{nameof(OneParamSyncActionPayload<TP>)}", MessageType.QueueParamsExecutionPayload);
        return new ValueTask(noParamsSyncPayload, noParamsSyncPayload.Version);
    }

    public ValueTask Execute(Action<TP, BasicCancellationToken?> methodToExecute, TP firstParam, BasicCancellationToken? secondParam)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<TwoParamSyncActionPayload<TP, BasicCancellationToken?>>();
        noParamsAsyncPayload.Configure(methodToExecute, firstParam, secondParam);
        noParamsAsyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextAction<TP>)}.{nameof(TwoParamSyncActionPayload<TP, BasicCancellationToken?>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }
}

public class MessageQueueExecutionContextResult<TR> : MessageQueueExecutionContext, IAlternativeExecutionContextResult<TR>
{
    public ValueTask<TR> Execute(Func<TR> methodToExecute)
    {
        var noParamsSyncPayload = EventQueueRecycler.Borrow<NoParamsSyncResultPayload<TR>>();
        noParamsSyncPayload.Configure(methodToExecute);
        noParamsSyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(noParamsSyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextResult<TR>)}.{nameof(NoParamsSyncResultPayload<TR>)}", MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsSyncPayload, noParamsSyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<ValueTask<TR>> methodToExecute)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<NoParamsAsyncResultPayload<TR>>();
        noParamsAsyncPayload.Configure(methodToExecute);
        noParamsAsyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextResult<TR>)}.{nameof(NoParamsAsyncResultPayload<TR>)}", MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<BasicCancellationToken?, TR> methodToExecute, BasicCancellationToken? firstParam)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<OneParamSyncResultPayload<TR, BasicCancellationToken?>>();
        noParamsAsyncPayload.Configure(methodToExecute, firstParam);
        noParamsAsyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextResult<TR>)}.{nameof(OneParamSyncResultPayload<TR, BasicCancellationToken?>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }
}

public class MessageQueueExecutionContextResult<TR, TP> : MessageQueueExecutionContext, IAlternativeExecutionContextResult<TR, TP>
{
    public ValueTask<TR> Execute(Func<TP, TR> methodToExecute, TP firstParam)
    {
        var oneParamSyncPayload = EventQueueRecycler.Borrow<OneParamSyncResultPayload<TR, TP>>();
        oneParamSyncPayload.Configure(methodToExecute, firstParam);
        oneParamSyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(oneParamSyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextResult<TR, TP>)}.{nameof(OneParamSyncResultPayload<TR, TP>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(oneParamSyncPayload, oneParamSyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<TP, ValueTask<TR>> methodToExecute, TP firstParam)
    {
        var oneParamAsyncPayload = EventQueueRecycler.Borrow<OneParamAsyncResultPayload<TR, TP>>();
        oneParamAsyncPayload.Configure(methodToExecute, firstParam);
        oneParamAsyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(oneParamAsyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextResult<TR, TP>)}.{nameof(OneParamAsyncResultPayload<TR, TP>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(oneParamAsyncPayload, oneParamAsyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<TP, BasicCancellationToken?, TR> methodToExecute, TP firstParam, BasicCancellationToken? secondParam)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<TwoParamsSyncResultPayload<TR, TP, BasicCancellationToken?>>();
        noParamsAsyncPayload.Configure(methodToExecute, firstParam, secondParam);
        noParamsAsyncPayload.ResponseTimeoutAndRecycleTimer = MessageQueue.Context.Timer;
        MessageQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(MessageQueueExecutionContextResult<TR, TP>)}.{nameof(TwoParamsSyncResultPayload<TR, TP, BasicCancellationToken?>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }
}
