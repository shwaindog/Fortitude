#region

using FortitudeBusRules.Messaging;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Pipelines.Execution;

public abstract class EventQueueExecutionContext : RecyclableObject
{
    protected IEventQueue EventQueue = null!;
    protected IRecycler EventQueueRecycler = null!;
    protected IRule SenderRule = null!;

    public void Configure(IEventQueue executionQueue, IRule senderRule)
    {
        EventQueue = executionQueue;
        EventQueueRecycler = executionQueue.Context.PooledRecycler;
        SenderRule = senderRule;
    }

    public override void StateReset()
    {
        SenderRule = null!;
    }
}

public class EventQueueExecutionContextAction<TP> : EventQueueExecutionContext, IAlternativeExecutionContextAction<TP>
{
    public ValueTask Execute(Action<TP> methodToExecute, TP firstParam)
    {
        var noParamsSyncPayload = EventQueueRecycler.Borrow<OneParamSyncActionPayload<TP>>();
        noParamsSyncPayload.Configure(methodToExecute, firstParam);
        EventQueue.EnqueuePayload(noParamsSyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextAction<TP>)}.{nameof(OneParamSyncActionPayload<TP>)}", MessageType.QueueParamsExecutionPayload);
        return new ValueTask(noParamsSyncPayload, noParamsSyncPayload.Version);
    }

    public ValueTask Execute(Action<TP, BasicCancellationToken?> methodToExecute, TP firstParam, BasicCancellationToken? secondParam)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<TwoParamSyncActionPayload<TP, BasicCancellationToken?>>();
        noParamsAsyncPayload.Configure(methodToExecute, firstParam, secondParam);
        EventQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextAction<TP>)}.{nameof(TwoParamSyncActionPayload<TP, BasicCancellationToken?>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }
}

public class EventQueueExecutionContextResult<TR> : EventQueueExecutionContext, IAlternativeExecutionContextResult<TR>
{
    public ValueTask<TR> Execute(Func<TR> methodToExecute)
    {
        var noParamsSyncPayload = EventQueueRecycler.Borrow<NoParamsSyncResultPayload<TR>>();
        noParamsSyncPayload.Configure(methodToExecute);
        EventQueue.EnqueuePayload(noParamsSyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextResult<TR>)}.{nameof(NoParamsSyncResultPayload<TR>)}", MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsSyncPayload, noParamsSyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<ValueTask<TR>> methodToExecute)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<NoParamsAsyncResultPayload<TR>>();
        noParamsAsyncPayload.Configure(methodToExecute);
        EventQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextResult<TR>)}.{nameof(NoParamsAsyncResultPayload<TR>)}", MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<BasicCancellationToken?, TR> methodToExecute, BasicCancellationToken? firstParam)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<OneParamSyncResultPayload<TR, BasicCancellationToken?>>();
        noParamsAsyncPayload.Configure(methodToExecute, firstParam);
        EventQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextResult<TR>)}.{nameof(OneParamSyncResultPayload<TR, BasicCancellationToken?>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }
}

public class EventQueueExecutionContextResult<TR, TP> : EventQueueExecutionContext, IAlternativeExecutionContextResult<TR, TP>
{
    public ValueTask<TR> Execute(Func<TP, TR> methodToExecute, TP firstParam)
    {
        var oneParamSyncPayload = EventQueueRecycler.Borrow<OneParamSyncResultPayload<TR, TP>>();
        oneParamSyncPayload.Configure(methodToExecute, firstParam);
        EventQueue.EnqueuePayload(oneParamSyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextResult<TR, TP>)}.{nameof(OneParamSyncResultPayload<TR, TP>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(oneParamSyncPayload, oneParamSyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<TP, ValueTask<TR>> methodToExecute, TP firstParam)
    {
        var oneParamAsyncPayload = EventQueueRecycler.Borrow<OneParamAsyncResultPayload<TR, TP>>();
        oneParamAsyncPayload.Configure(methodToExecute, firstParam);
        EventQueue.EnqueuePayload(oneParamAsyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextResult<TR, TP>)}.{nameof(OneParamAsyncResultPayload<TR, TP>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(oneParamAsyncPayload, oneParamAsyncPayload.Version);
    }

    public ValueTask<TR> Execute(Func<TP, BasicCancellationToken?, TR> methodToExecute, TP firstParam, BasicCancellationToken? secondParam)
    {
        var noParamsAsyncPayload = EventQueueRecycler.Borrow<TwoParamsSyncResultPayload<TR, TP, BasicCancellationToken?>>();
        noParamsAsyncPayload.Configure(methodToExecute, firstParam, secondParam);
        EventQueue.EnqueuePayload(noParamsAsyncPayload, SenderRule,
            $"{nameof(EventQueueExecutionContextResult<TR, TP>)}.{nameof(TwoParamsSyncResultPayload<TR, TP, BasicCancellationToken?>)}"
            , MessageType.QueueParamsExecutionPayload);
        return new ValueTask<TR>(noParamsAsyncPayload, noParamsAsyncPayload.Version);
    }
}
