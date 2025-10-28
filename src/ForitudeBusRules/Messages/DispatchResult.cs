// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.SelectionStrategies;
using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeBusRules.Messages;

public interface IDispatchResult : IReusableObject<IDispatchResult>
{
    long TotalExecutionTimeTicks         { get; set; }
    long TotalExecutionAwaitingTimeTicks { get; set; }
    long TotalAwaitingTimeTicks          { get; set; }

    RouteSelectionResult?        DeploymentSelectionResult  { get; set; }
    IDispatchSelectionResultSet? DispatchSelectionResultSet { get; set; }

    DateTime SentTime             { get; set; }
    DateTime FinishedDispatchTime { get; set; }

    IReadOnlyList<RuleExecutionTime> RulesReceived { get; }

    void AddRuleTime(RuleExecutionTime ruleExecutionTime);
}

public struct RuleExecutionTime
{
    public RuleExecutionTime(IRule rule, DateTime startTime, DateTime? awaitTime, DateTime completeTime)
    {
        Rule         = rule;
        StartTime    = startTime;
        AwaitTime    = awaitTime;
        CompleteTime = completeTime;
    }

    public IRule     Rule         { get; set; }
    public DateTime  StartTime    { get; set; }
    public DateTime? AwaitTime    { get; set; }
    public DateTime  CompleteTime { get; set; }
}

public class DispatchResult : ReusableObject<IDispatchResult>, IDispatchResult
{
    private readonly ReusableList<RuleExecutionTime> rulesTimes = new();

    public DispatchResult() { }

    public DispatchResult(DispatchResult toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public IReadOnlyList<RuleExecutionTime> RulesReceived => rulesTimes;

    public long TotalExecutionTimeTicks         { get; set; }
    public long TotalExecutionAwaitingTimeTicks { get; set; }
    public long TotalAwaitingTimeTicks          { get; set; }

    public RouteSelectionResult?        DeploymentSelectionResult  { get; set; }
    public IDispatchSelectionResultSet? DispatchSelectionResultSet { get; set; }

    public DateTime SentTime             { get; set; }
    public DateTime FinishedDispatchTime { get; set; }

    public override IRecycler? Recycler
    {
        get => base.Recycler;
        set
        {
            base.Recycler       = value;
            rulesTimes.Recycler = value;
        }
    }

    public override void StateReset()
    {
        SentTime             = DateTime.MinValue;
        FinishedDispatchTime = DateTime.MaxValue;
        DispatchSelectionResultSet?.DecrementRefCount();
        DispatchSelectionResultSet      = null;
        DeploymentSelectionResult       = null;
        TotalExecutionTimeTicks         = 0;
        TotalExecutionAwaitingTimeTicks = 0;
        rulesTimes.Clear();
        base.StateReset();
    }

    public void AddRuleTime(RuleExecutionTime ruleExecutionTime)
    {
        rulesTimes.Add(ruleExecutionTime);
    }

    public override IDispatchResult Clone() => Recycler?.Borrow<DispatchResult>().CopyFrom(this) ?? new DispatchResult(this);

    public override IDispatchResult CopyFrom
    (IDispatchResult source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SentTime                        = source.SentTime;
        FinishedDispatchTime            = source.FinishedDispatchTime;
        TotalExecutionTimeTicks         = source.TotalExecutionTimeTicks;
        TotalExecutionAwaitingTimeTicks = source.TotalExecutionAwaitingTimeTicks;
        rulesTimes.Clear();
        rulesTimes.AddRange(source.RulesReceived);
        return this;
    }

    public override string ToString() =>
        $"{nameof(DispatchResult)}({nameof(rulesTimes)}: [{string.Join(", ", rulesTimes)}], {nameof(refCount)}: {refCount}, " +
        $"{nameof(TotalExecutionTimeTicks)}: {TotalExecutionTimeTicks}, {nameof(TotalExecutionAwaitingTimeTicks)}: {TotalExecutionAwaitingTimeTicks}, " +
        $"{nameof(SentTime)}: {SentTime}, {nameof(FinishedDispatchTime)}: {FinishedDispatchTime}, {nameof(IsInRecycler)}: {IsInRecycler})";
}

public class DispatchExecutionSummary
{
    public DispatchExecutionSummary(string name) => Name = name;
    public string Name                            { get; set; }
    public int    NumberOfInvocations             { get; set; }
    public long   TotalRulesExecuted              { get; set; }
    public long   TotalExecutionTimeTicks         { get; set; }
    public long   TotalExecutionAwaitingTimeTicks { get; set; }

    public void AddDispatchResult(IDispatchResult dispatchResult)
    {
        NumberOfInvocations++;
        TotalExecutionAwaitingTimeTicks += dispatchResult.TotalExecutionAwaitingTimeTicks;
        TotalExecutionTimeTicks         += dispatchResult.TotalExecutionTimeTicks;
        TotalRulesExecuted              += dispatchResult.RulesReceived.Count;
    }
}
