﻿#region

using FortitudeCommon.AsyncProcessing.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeCommon.EventProcessing.Disruption.Rings.PollingRings;

public interface IAsyncValueTaskPollingRing<T> : ITaskCallbackPollingRing<T> where T : class, ICanCarryTaskCallbackPayload
{
    Func<T, bool> InterceptHandler { get; set; }
    ValueTaskProcessEvent<T> ProcessEvent { get; set; }
    ValueTask<long> Poll();
}

public class AsyncValueValueTaskPollingRing<T> : IAsyncValueTaskPollingRing<T> where T : class, ICanCarryTaskCallbackPayload
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(AsyncValueValueTaskPollingRing<>));
    private readonly T[] cells;

    private readonly IClaimStrategy claimStrategy;
    private readonly TaskCompleteSequenceContainer[] completeAhead;
    private readonly Sequence conCursor = new(Sequence.InitialValue);
    private readonly Sequence[] conCursors;

    private readonly Sequence pubCursor = new(Sequence.InitialValue);
    private readonly int ringMask;
    private long completedReadAheadConsumerCursor;
    private long completedReadAheadPublishCursor;
    private long readAheadCursor = -1;

    public AsyncValueValueTaskPollingRing(string name, int size, Func<T> dataFactory
        , ClaimStrategyType claimStrategyType, ValueTaskProcessEvent<T>? processAsyncTask = null, int? completeAheadQueueSize = null
        , bool logErrors = true)
    {
        ProcessEvent = processAsyncTask ?? ((currentSequence, _) =>
        {
            Logger.Warn("Poll Sink not set on AsyncTaskPollingRing.  Will do nothing");
            return new ValueTask<long>(currentSequence);
        });
        InterceptHandler = _ => false;
        Name = name;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask = ringSize - 1;
        cells = new T[ringSize];
        completeAhead = new TaskCompleteSequenceContainer[completeAheadQueueSize ?? size];
        conCursors = new[] { conCursor };

        claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
        for (var i = 0; i < completeAhead.Length; i++) completeAhead[i] = new TaskCompleteSequenceContainer();
    }

    public T this[long sequence] => cells[(int)sequence & ringMask];

    public string Name { get; }

    public ValueTaskProcessEvent<T> ProcessEvent { get; set; }

    public Func<T, bool> InterceptHandler { get; set; }

    public long Claim()
    {
        var sequence = claimStrategy.Claim();
        claimStrategy.WaitFor(sequence, conCursors);
        return sequence;
    }

    public void Publish(long sequence)
    {
        claimStrategy.Serialize(pubCursor, sequence);
        pubCursor.Value = sequence;
    }

    public async ValueTask<long> Poll()
    {
        var maxPublishedSequence = pubCursor.Value;
        var currentSequence = readAheadCursor + 1;
        if (currentSequence <= maxPublishedSequence)
        {
            var datum = cells[(int)currentSequence & ringMask];
            readAheadCursor = currentSequence;
            try
            {
                if (datum.IsTaskCallbackItem)
                {
                    datum.InvokeTaskCallback();
                    CheckConsumerShouldMoveForward(currentSequence);
                    return currentSequence;
                }

                if (InterceptHandler(datum))
                {
                    CheckConsumerShouldMoveForward(currentSequence);
                    return currentSequence;
                }

                var completedSequenceId = await ProcessEvent(currentSequence, datum);
                CheckConsumerShouldMoveForward(completedSequenceId);
                return completedSequenceId;
            }
            catch (Exception ex)
            {
                Logger.Error("Unhandled exception will attempt to recover. Got {0}", ex);
                CheckConsumerShouldMoveForward(currentSequence);
            }

            return currentSequence;
        }

        return -1;
    }

    public void EnqueueCallback(SendOrPostCallback d, object? state)
    {
        var seqId = Claim();
        var entry = this[seqId];
        entry.SetAsTaskCallbackItem(d, state);
        Publish(seqId);
    }

    private void EnqueueCompleteAhead(long completed)
    {
        var seqId = completedReadAheadPublishCursor++;
        var evt = completeAhead[seqId];
        evt.Value = completed;
    }

    private void CheckConsumerShouldMoveForward(long completedSequenceId)
    {
        if (conCursor.Value == completedSequenceId - 1)
        {
            conCursor.Value = completedSequenceId;
            UpdateConCursorWithCompleteReadAheadItems(completedReadAheadConsumerCursor, completedReadAheadPublishCursor);
        }
        else
        {
            EnqueueCompleteAhead(completedSequenceId);
        }
    }

    public void UpdateConCursorWithCompleteReadAheadItems(long lookAheadStart, long lookAheadMax)
    {
        for (var currLookAheadConCur = lookAheadStart; currLookAheadConCur < lookAheadMax; currLookAheadConCur++)
        {
            var checkLookAhead = completeAhead[currLookAheadConCur].Value;
            if (conCursor.Value == checkLookAhead - 1)
            {
                conCursor.Value = checkLookAhead;
                completedReadAheadConsumerCursor = currLookAheadConCur;
            }
            else if (checkLookAhead > conCursor.Value)
            {
                if (!CheckFurtherBackCompleteReadAheadCompleteFound(currLookAheadConCur + 1, lookAheadMax)) break;
                currLookAheadConCur--;
            }
        }
    }

    public bool CheckFurtherBackCompleteReadAheadCompleteFound(long lookAheadStart, long lookAheadMax)
    {
        var updatedConCursor = false;
        for (var currLookAheadConCur = lookAheadStart; currLookAheadConCur < lookAheadMax; currLookAheadConCur++)
        {
            var checkLookAhead = completeAhead[currLookAheadConCur].Value;
            if (conCursor.Value == checkLookAhead - 1)
            {
                updatedConCursor = true;
                conCursor.Value = checkLookAhead;
                break;
            }
        }

        return updatedConCursor;
    }

    private class TaskCompleteSequenceContainer
    {
        public long Value;
    }
}