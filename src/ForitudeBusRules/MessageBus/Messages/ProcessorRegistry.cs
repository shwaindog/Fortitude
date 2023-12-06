#region

using Fortitude.EventProcessing.BusRules.Messaging;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Messages;

public interface IProcessorRegistry : IReusableAsyncResponseSource<IDispatchResult>
{
    DispatchResult? DispatchResult { get; set; }

    IRule? RulePayLoad { get; set; }

    void ProcessingComplete();
    void RegisterStart(IRule processor);
    void RegisterAwaiting(IRule processor);
    void RegisterFinish(IRule processor);
}

internal enum TimeType
{
    Start
    , Awaiting
    , Completed
}

public class ProcessorRegistry : ReusableValueTaskSource<IDispatchResult>, IProcessorRegistry
{
    private DispatchResult? dispatchResult;
    private bool havePublishedResults;
    private List<RuleTime> ruleTimes = new();

    public DispatchResult? DispatchResult
    {
        get => dispatchResult;
        set
        {
            if (value == dispatchResult && dispatchResult != null) dispatchResult.OwningProcessorRegistry = null;
            dispatchResult = value;
            if (dispatchResult != null) dispatchResult.OwningProcessorRegistry = this;
        }
    }

    public IRule? RulePayLoad { get; set; }

    public void ProcessingComplete()
    {
        CollectDispatchStats();
    }

    public void RegisterStart(IRule processor)
    {
        if (dispatchResult != null)
        {
            IncrementRefCount();
            ruleTimes.Add(new RuleTime(processor, TimeType.Start, DateTime.Now));
        }
    }

    public void RegisterAwaiting(IRule processor)
    {
        if (dispatchResult != null) ruleTimes.Add(new RuleTime(processor, TimeType.Awaiting, DateTime.Now));
    }

    public void RegisterFinish(IRule processor)
    {
        if (dispatchResult != null)
        {
            DecrementRefCount();
            ruleTimes.Add(new RuleTime(processor, TimeType.Awaiting, DateTime.Now));
        }
    }

    public override int DecrementRefCount()
    {
        if (RefCount == 2) // last refCount is for the consumer to decrement or will be auto decremented on a timer
            ProcessingComplete();
        return base.DecrementRefCount();
    }

    public override void Reset()
    {
        havePublishedResults = false;
        ruleTimes.Clear();
        if (dispatchResult != null)
        {
            dispatchResult.Reset();
            dispatchResult.SentTime = DateTime.Now;
        }

        base.Reset();
    }

    private void CollectDispatchStats()
    {
        if (!havePublishedResults && dispatchResult != null)
        {
            havePublishedResults = true;
            var finishTime = DateTime.Now;
            long totalTaskTimeTicks = 0;
            long totalAwaitTimeTicks = 0;
            long totalExecuteAwaitTimeTicks = 0;
            for (var i = 0; i < ruleTimes.Count; i++)
            {
                var ruleStart = ruleTimes[i];
                if (ruleStart.TimeType == TimeType.Start)
                {
                    DateTime? awaitDateTime = null;
                    for (var j = i + 1; j < ruleTimes.Count; j++)
                    {
                        var awaitOrComplete = ruleTimes[j];

                        if (ruleStart.Rule == awaitOrComplete.Rule && ruleStart.TimeType is TimeType.Awaiting)
                        {
                            totalTaskTimeTicks += awaitOrComplete.Instant.Ticks - ruleStart.Instant.Ticks;
                            awaitDateTime = awaitOrComplete.Instant;
                        }

                        if (ruleStart.Rule == awaitOrComplete.Rule && ruleStart.TimeType == TimeType.Completed)
                        {
                            totalExecuteAwaitTimeTicks += awaitOrComplete.Instant.Ticks - ruleStart.Instant.Ticks;
                            if (awaitDateTime != null)
                                totalAwaitTimeTicks += awaitOrComplete.Instant.Ticks - awaitDateTime.Value.Ticks;
                            dispatchResult.AddRuleTime(new RuleExecutionTime(ruleStart.Rule, ruleStart.Instant
                                , awaitDateTime
                                , awaitOrComplete.Instant));
                            break;
                        }
                    }
                }
            }

            dispatchResult.TotalExecutionAwaitingTimeTicks = totalExecuteAwaitTimeTicks;
            dispatchResult.TotalAwaitingTimeTicks = totalAwaitTimeTicks;
            dispatchResult.TotalExecutionTimeTicks = totalTaskTimeTicks;
            dispatchResult.FinishedDispatchTime = finishTime;
            TrySetResult(dispatchResult);
        }
    }

    public override string ToString() =>
        $"{GetType().Name}({nameof(InstanceNumber)}: {InstanceNumber}, {nameof(Version)}: {Version}, {nameof(RefCount)}: {RefCount}, " +
        $"{nameof(IsInRecycler)}: {IsInRecycler}, {nameof(IsCompleted)}: {IsCompleted}, {nameof(dispatchResult)}: {dispatchResult}, " +
        $"{nameof(havePublishedResults)}: {havePublishedResults}, {nameof(RulePayLoad)}: {RulePayLoad})";


    private struct RuleTime
    {
        public RuleTime(IRule rule, TimeType timeType, DateTime instant)
        {
            Rule = rule;
            TimeType = timeType;
            Instant = instant;
        }

        public readonly IRule Rule;
        public readonly DateTime Instant;
        public readonly TimeType TimeType;
    }
}
