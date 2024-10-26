﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages;

public interface IProcessorRegistry : IAsyncResponseSource
{
    IRule? RulePayload { get; set; }

    bool ShouldCollectMetrics { get; set; }

    void ProcessingComplete();
    void RegisterStart(IRule processor);
    void RegisterAwaiting(IRule processor);
    void RegisterFinish(IRule processor);
}

public interface IProcessorRegistry<TResult, TInterface> : IProcessorRegistry, IReusableAsyncResponseSource<TInterface>
    where TInterface : IDispatchResult where TResult : TInterface
{
    TResult? Result { get; set; }
}

internal enum TimeType
{
    Start
  , Awaiting
  , Completed
}

public class ProcessorRegistry<TResult, TInterface> : ReusableValueTaskSource<TInterface>, IProcessorRegistry<TResult, TInterface>
    where TInterface : IDispatchResult where TResult : TInterface
{
    private readonly ReusableList<RuleTime> ruleTimes = new();

    private bool     havePublishedResults;
    private TResult? result;

    public TResult? Result
    {
        get => result;
        set => result = value;
    }

    public IRule? RulePayload          { get; set; }
    public bool   ShouldCollectMetrics { get; set; }

    public override IRecycler? Recycler
    {
        get => base.Recycler;
        set
        {
            base.Recycler      = value;
            ruleTimes.Recycler = value;
        }
    }

    public void ProcessingComplete()
    {
        CollectDispatchStats();
    }

    public void RegisterStart(IRule processor)
    {
        if (result != null)
        {
            IncrementRefCount();
            if (ShouldCollectMetrics) ruleTimes.Add(new RuleTime(processor, TimeType.Start, TimeContext.UtcNow));
        }
    }

    public void RegisterAwaiting(IRule processor)
    {
        if (ShouldCollectMetrics && result != null) ruleTimes.Add(new RuleTime(processor, TimeType.Awaiting, TimeContext.UtcNow));
    }

    public void RegisterFinish(IRule processor)
    {
        if (result != null)
        {
            if (ShouldCollectMetrics) ruleTimes.Add(new RuleTime(processor, TimeType.Awaiting, TimeContext.UtcNow));
            DecrementRefCount();
        }
    }

    public override int DecrementRefCount()
    {
        if (RefCount == 2) // last refCount is for the consumer to decrement or will be auto decremented on a timer
            ProcessingComplete();
        return base.DecrementRefCount();
    }

    public override void StateReset()
    {
        havePublishedResults = false;
        ShouldCollectMetrics = false;
        ruleTimes.Clear();
        result = default;

        base.StateReset();
    }

    private void CollectDispatchStats()
    {
        if (!havePublishedResults && result != null)
        {
            havePublishedResults = true;

            if (ShouldCollectMetrics)
            {
                var finishTime = DateTime.Now;

                long totalTaskTimeTicks         = 0;
                long totalAwaitTimeTicks        = 0;
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
                                awaitDateTime      =  awaitOrComplete.Instant;
                            }

                            if (ruleStart.Rule == awaitOrComplete.Rule && ruleStart.TimeType == TimeType.Completed)
                            {
                                totalExecuteAwaitTimeTicks += awaitOrComplete.Instant.Ticks - ruleStart.Instant.Ticks;
                                if (awaitDateTime != null) totalAwaitTimeTicks += awaitOrComplete.Instant.Ticks - awaitDateTime.Value.Ticks;
                                result.AddRuleTime
                                    (new RuleExecutionTime
                                        (ruleStart.Rule, ruleStart.Instant, awaitDateTime, awaitOrComplete.Instant));
                                break;
                            }
                        }
                    }
                }

                result.TotalExecutionAwaitingTimeTicks = totalExecuteAwaitTimeTicks;

                result.TotalAwaitingTimeTicks  = totalAwaitTimeTicks;
                result.TotalExecutionTimeTicks = totalTaskTimeTicks;
                result.FinishedDispatchTime    = finishTime;
            }
            TrySetResult(result);
        }
    }

    protected string MembersToString() =>
        $"{nameof(InstanceNumber)}: {InstanceNumber}, {nameof(Version)}: {Version}, {nameof(RefCount)}: {RefCount}, " +
        $"{nameof(IsInRecycler)}: {IsInRecycler}, {nameof(IsCompleted)}: {IsCompleted}, {nameof(result)}: {result}, " +
        $"{nameof(havePublishedResults)}: {havePublishedResults}, {nameof(RulePayload)}: {RulePayload}";

    public override string ToString() => $"{nameof(ProcessorRegistry<TResult, TInterface>)}({MembersToString()})";


    private struct RuleTime
    {
        public RuleTime(IRule rule, TimeType timeType, DateTime instant)
        {
            Rule     = rule;
            TimeType = timeType;
            Instant  = instant;
        }

        public readonly IRule    Rule;
        public readonly DateTime Instant;
        public readonly TimeType TimeType;
    }
}

public class DispatchProcessorRegistry : ProcessorRegistry<DispatchResult, IDispatchResult>
{
    public DispatchProcessorRegistry() { }

    public override string ToString() => $"{nameof(DispatchProcessorRegistry)}({MembersToString()})";
}

public class DeploymentLifeTimeProcessorRegistry : ProcessorRegistry<RuleDeploymentLifeTime, IRuleDeploymentLifeTime>
{
    public DeploymentLifeTimeProcessorRegistry() { }
    public override string ToString() => $"{nameof(DeploymentLifeTimeProcessorRegistry)}({MembersToString()})";
}
