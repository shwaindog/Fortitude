#region

using Fortitude.EventProcessing.BusRules.EventBus.Tasks;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace Fortitude.EventProcessing.BusRules.Messaging;

public interface IProcessorRegistry : IRecyclableObject
{
    void SetResponse(DispatchResult dispatchResult);
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
    private DispatchResult? response;
    private List<RuleTime> ruleTimes = new();

    public void SetResponse(DispatchResult? dispatchResult = null)
    {
        response = dispatchResult;
        if (dispatchResult != null)
        {
            dispatchResult.Reset();
            dispatchResult.SentTime = DateTime.Now;
        }
    }

    public void RegisterStart(IRule processor)
    {
        if (response != null)
        {
            IncrementRefCount();
            ruleTimes.Add(new RuleTime(processor, TimeType.Start, DateTime.Now));
        }
    }

    public void RegisterAwaiting(IRule processor)
    {
        if (response != null) ruleTimes.Add(new RuleTime(processor, TimeType.Awaiting, DateTime.Now));
    }

    public void RegisterFinish(IRule processor)
    {
        if (response != null)
        {
            DecrementRefCount();
            ruleTimes.Add(new RuleTime(processor, TimeType.Awaiting, DateTime.Now));
        }
    }

    public override void Reset()
    {
        CollectDispatchStats();

        ruleTimes.Clear();
        base.Reset();
    }

    private void CollectDispatchStats()
    {
        if (response != null)
        {
            var finishTime = DateTime.Now;
            long totalTaskTimeTicks = 0;
            long totalAwaitTimeTicks = 0;
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
                            totalAwaitTimeTicks += awaitOrComplete.Instant.Ticks - ruleStart.Instant.Ticks;
                            response.AddRuleTime(new RuleExecutionTime(ruleStart.Rule, ruleStart.Instant, awaitDateTime
                                , awaitOrComplete.Instant));
                            break;
                        }
                    }
                }
            }

            response.TotalExecutionAwaitingTimeTicks = totalAwaitTimeTicks;
            response.TotalExecutionTimeTicks = totalTaskTimeTicks;
            response.FinishedDispatchTime = finishTime;
            TrySetResult(response);
        }
    }


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
