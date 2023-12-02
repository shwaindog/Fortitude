#region

using Fortitude.EventProcessing.BusRules.MessageBus.Messages;
using Fortitude.EventProcessing.BusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace Fortitude.EventProcessing.BusRules.Messaging;

public interface IDispatchResult : IRecyclableObject<IDispatchResult>
{
    long TotalExecutionTimeTicks { get; set; }
    long TotalExecutionAwaitingTimeTicks { get; set; }
    long TotalAwaitingTimeTicks { get; set; }
    DateTime SentTime { get; }
    DateTime FinishedDispatchTime { get; }
    IReadOnlyList<RuleExecutionTime> RulesReceived { get; }
    IProcessorRegistry? OwningProcessorRegistry { get; set; }
    void Reset();
    void AddRuleTime(RuleExecutionTime ruleExecutionTime);
}

public struct RuleExecutionTime
{
    public RuleExecutionTime(IRule rule, DateTime startTime, DateTime? awaitTime, DateTime completeTime)
    {
        Rule = rule;
        StartTime = startTime;
        AwaitTime = awaitTime;
        CompleteTime = completeTime;
    }

    public IRule Rule { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? AwaitTime { get; set; }
    public DateTime CompleteTime { get; set; }
}

public class DispatchResult : IDispatchResult
{
    private readonly List<RuleExecutionTime> rulesTimes = new();
    private int refCount = 0;
    public IReadOnlyList<RuleExecutionTime> RulesReceived => rulesTimes;
    public long TotalExecutionTimeTicks { get; set; }
    public long TotalExecutionAwaitingTimeTicks { get; set; }
    public long TotalAwaitingTimeTicks { get; set; }
    public DateTime SentTime { get; set; }
    public DateTime FinishedDispatchTime { get; set; }
    public IProcessorRegistry? OwningProcessorRegistry { get; set; }

    public void Reset()
    {
        SentTime = DateTime.MinValue;
        FinishedDispatchTime = DateTime.MaxValue;
        TotalExecutionTimeTicks = 0;
        TotalExecutionAwaitingTimeTicks = 0;
        rulesTimes.Clear();
    }

    public void AddRuleTime(RuleExecutionTime ruleExecutionTime)
    {
        rulesTimes.Add(ruleExecutionTime);
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((DispatchResult)source, copyMergeFlags);
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; } = false;
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }

    public int DecrementRefCount()
    {
        if (OwningProcessorRegistry != null)
        {
            OwningProcessorRegistry.DecrementRefCount();
            return OwningProcessorRegistry.RefCount;
        }
        else
        {
            if (Interlocked.Increment(ref refCount) <= 0 && RecycleOnRefCountZero) Recycle();
            return refCount;
        }
    }

    public int IncrementRefCount()
    {
        if (OwningProcessorRegistry != null)
        {
            OwningProcessorRegistry.IncrementRefCount();
            return OwningProcessorRegistry.RefCount;
        }
        else
        {
            return Interlocked.Increment(ref refCount);
        }
    }

    public bool Recycle()
    {
        if (RecycleOnRefCountZero)
        {
            if (refCount <= 0) Recycler!.Recycle(this);
        }
        else
        {
            Recycler!.Recycle(this);
        }

        return IsInRecycler;
    }

    public void CopyFrom(IDispatchResult source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SentTime = source.SentTime;
        FinishedDispatchTime = source.FinishedDispatchTime;
        TotalExecutionTimeTicks = source.TotalExecutionTimeTicks;
        TotalExecutionAwaitingTimeTicks = source.TotalExecutionAwaitingTimeTicks;
        rulesTimes.Clear();
        rulesTimes.AddRange(source.RulesReceived);
    }

    public override string ToString() =>
        $"{GetType().Name}({nameof(rulesTimes)}: [{string.Join(", ", rulesTimes)}], {nameof(refCount)}: {refCount}, {nameof(TotalExecutionTimeTicks)}: {TotalExecutionTimeTicks}, {nameof(TotalExecutionAwaitingTimeTicks)}: {TotalExecutionAwaitingTimeTicks}, {nameof(SentTime)}: {SentTime}, {nameof(FinishedDispatchTime)}: {FinishedDispatchTime}, {nameof(IsInRecycler)}: {IsInRecycler})";
}

public class DispatchExecutionSummary
{
    public DispatchExecutionSummary(string name) => Name = name;
    public string Name { get; set; }
    public int NumberOfInvocations { get; set; }
    public long TotalRulesExecuted { get; set; }
    public long TotalExecutionTimeTicks { get; set; }
    public long TotalExecutionAwaitingTimeTicks { get; set; }

    public void AddDispatchResult(IDispatchResult dispatchResult)
    {
        NumberOfInvocations++;
        TotalExecutionAwaitingTimeTicks += dispatchResult.TotalExecutionAwaitingTimeTicks;
        TotalExecutionTimeTicks += dispatchResult.TotalExecutionTimeTicks;
        TotalRulesExecuted += dispatchResult.RulesReceived.Count;
    }
}
