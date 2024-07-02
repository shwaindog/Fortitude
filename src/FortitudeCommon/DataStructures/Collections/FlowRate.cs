// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.DataStructures.Collections;

public enum FlowRateType
{
    MaxNumberPerPeriod
  , BatchRunsWithDelay
  , EntryDelays
}

public interface IFlowRate
{
    FlowRateType       FlowRateType     { get; }
    public PeriodRate? PeriodRate       { get; }
    public BatchRate?  BatchRate        { get; }
    public TimeSpan?   BetweenEachEntry { get; }
}

public struct PeriodRate
{
    public PeriodRate(int numberPerPeriod, TimeSpan period)
    {
        NumberPerPeriod = numberPerPeriod;
        Period          = period;
    }

    public int      NumberPerPeriod { get; }
    public TimeSpan Period          { get; }
}

public struct BatchRate
{
    public BatchRate(TimeSpan batchLength, int numberInBatch)
    {
        BatchLength   = batchLength;
        NumberInBatch = numberInBatch;
    }

    public int      NumberInBatch { get; }
    public TimeSpan BatchLength   { get; }
}

public struct FlowRate // not inheriting from IFlowRate to prevent accidental boxing unboxing
{
    public FlowRate(IFlowRate flowRate)
    {
        FlowRateType     = flowRate.FlowRateType;
        PeriodRate       = flowRate.PeriodRate;
        BatchRate        = flowRate.BatchRate;
        BetweenEachEntry = flowRate.BetweenEachEntry;
    }

    public FlowRate(PeriodRate periodRate)
    {
        FlowRateType = FlowRateType.MaxNumberPerPeriod;
        PeriodRate   = periodRate;
    }

    public FlowRate(BatchRate batchRate)
    {
        FlowRateType = FlowRateType.BatchRunsWithDelay;
        BatchRate    = batchRate;
    }

    public FlowRate(TimeSpan betweenEachEntry)
    {
        FlowRateType     = FlowRateType.EntryDelays;
        BetweenEachEntry = betweenEachEntry;
    }

    public FlowRateType FlowRateType     { get; }
    public PeriodRate?  PeriodRate       { get; }
    public BatchRate?   BatchRate        { get; }
    public TimeSpan?    BetweenEachEntry { get; }
}

public class FlowRegulator
{
    private readonly FlowRate flowRate;

    private readonly decimal maxEntriesPerMs;

    private DateTime nextBatchStartTime = DateTime.UtcNow;
    private DateTime startTime;
    private long     totalEntries;

    public FlowRegulator(FlowRate flowRate)
    {
        this.flowRate = flowRate;
        if (flowRate.FlowRateType == FlowRateType.MaxNumberPerPeriod)
            maxEntriesPerMs = flowRate.PeriodRate!.Value.NumberPerPeriod / (decimal)flowRate.PeriodRate!.Value.Period.TotalMilliseconds;
    }

    public FlowRegulator(IFlowRate flowRate) : this(new FlowRate(flowRate)) { }

    public async ValueTask IncrementEntry(DateTime? currentTime = null)
    {
        var now                     = currentTime ?? DateTime.UtcNow;
        var current                 = Interlocked.Increment(ref totalEntries);
        if (current == 1) startTime = now;

        if (flowRate.FlowRateType == FlowRateType.EntryDelays) await Task.Delay(flowRate.BetweenEachEntry!.Value);
        if (flowRate.FlowRateType == FlowRateType.BatchRunsWithDelay)
        {
            var batchComplete = current % flowRate.BatchRate!.Value.NumberInBatch == 0;
            if (batchComplete && now < nextBatchStartTime)
            {
                await Task.Delay(nextBatchStartTime - now);
                nextBatchStartTime += flowRate.BatchRate!.Value.BatchLength;
            }
            else if (batchComplete && now > nextBatchStartTime)
            {
                nextBatchStartTime = now + flowRate.BatchRate!.Value.BatchLength;
            }
        }
        if (flowRate.FlowRateType == FlowRateType.MaxNumberPerPeriod && current > 1)
        {
            var totalMs        = (decimal)(now - startTime).TotalMilliseconds;
            var currentRate    = current / totalMs;
            var extendPeriodMs = currentRate / maxEntriesPerMs * totalMs;

            if (extendPeriodMs > 20)
            {
                var extendPeriodTimeSpan = TimeSpan.FromMilliseconds((double)extendPeriodMs);
                await Task.Delay(extendPeriodTimeSpan);
            }
        }
    }
}
