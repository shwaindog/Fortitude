// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

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

    int Size { get; }

    bool Poll();

    event Action<QueueEventTime> QueueEntryStart;
    event Action<QueueEventTime> QueueEntryComplete;
}

public class AsyncValueTaskPollingRing<T> : IAsyncValueTaskPollingRing<T> where T : class, ICanCarryTaskCallbackPayload
{
    private static IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(AsyncValueTaskPollingRing<>));

    private readonly T[] cells;

    private readonly IClaimStrategy claimStrategy;

    private readonly Sequence   conCursor = new(Sequence.InitialValue);
    private readonly Sequence[] conCursors;

    private readonly AwaitingCompleteContainer[] incompleteTasks;

    private readonly Sequence pubCursor = new(Sequence.InitialValue);

    private readonly int readAheadRingMask;
    private readonly int ringMask;

    private long halfSize;

    private long incompleteTasksConsumerCursor;
    private long incompleteTasksPublishCursor;

    public AsyncValueTaskPollingRing(string name, int size, Func<T> dataFactory, ClaimStrategyType claimStrategyType, bool logErrors = true)
    {
        InterceptHandler = _ => false;

        Name = name;

        var ringSize          = MemoryUtils.CeilingNextPowerOfTwo(size);
        var readAheadRingSize = ringSize;
        Size     = ringSize;
        halfSize = ringSize / 2;

        ringMask          = ringSize - 1;
        readAheadRingMask = readAheadRingSize - 1;
        cells             = new T[ringSize];
        incompleteTasks   = new AwaitingCompleteContainer[readAheadRingSize];
        conCursors        = new[] { conCursor };

        claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);

        for (var i = 0; i < cells.Length; i++) cells[i]                     = dataFactory();
        for (var i = 0; i < incompleteTasks.Length; i++) incompleteTasks[i] = new AwaitingCompleteContainer();
    }

    public T this[long sequence] => cells[(int)sequence & ringMask];

    public int Size { get; }

    public string Name { get; }

    public Func<T, bool> InterceptHandler { get; set; }

    public event Action<QueueEventTime>? QueueEntryStart;
    public event Action<QueueEventTime>? QueueEntryComplete;

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

    public bool Poll()
    {
        var maxPublishedSequence = pubCursor.Value;
        var currentSequence      = conCursor.Value;
        if (currentSequence <= maxPublishedSequence)
        {
            var datum = cells[(int)currentSequence & ringMask];
            try
            {
                QueueEntryStart?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
                if (datum.IsTaskCallbackItem)
                {
                    datum.InvokeTaskCallback();
                    datum.TaskPostProcessingCleanup();
                    return true;
                }

                if (InterceptHandler(datum)) return true;

                var messageTask = ProcessEntry(datum);
                if (!messageTask.IsCompleted)
                    EnqueueIncompleteValueTask(currentSequence, messageTask);
                else
                    try
                    {
                        messageTask.GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        Logger.Warn("Async message number {0}. Caught {1}", currentSequence, ex);
                    }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Unhandled exception will attempt to recover. Got {0}", ex);
            }
            finally
            {
                QueueEntryComplete?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
                conCursor.Value = currentSequence + 1;
                CheckAwaitingTasksComplete();
            }
        }
        return false;
    }

    public void EnqueueCallback(SendOrPostCallback d, object? state)
    {
        var seqId = Claim();
        var entry = this[seqId & ringMask];
        entry.SetAsTaskCallbackItem(d, state);
        Publish(seqId);
    }

    public virtual ValueTask ProcessEntry(T entry)
    {
        Logger.Warn("You should override this to process events");
        return ValueTask.CompletedTask;
    }

    private void EnqueueIncompleteValueTask(long messageId, ValueTask incompleteTask)
    {
        var seqId = incompleteTasksPublishCursor++;
        var evt   = incompleteTasks[seqId & readAheadRingMask];
        evt.Value            = messageId;
        evt.AwaitingComplete = incompleteTask;
    }

    public void CheckAwaitingTasksComplete()
    {
        var gapCount = 0;
        for (var currLookAheadConCur = incompleteTasksConsumerCursor
             ; currLookAheadConCur < incompleteTasksPublishCursor
             ; currLookAheadConCur++)
        {
            var checkIncompleteTask = incompleteTasks[currLookAheadConCur & readAheadRingMask];
            if (checkIncompleteTask is { Value: >= 0 } and ({ AwaitingComplete.IsCompleted: true } or { AwaitingComplete.IsFaulted: true } or
                                                            { AwaitingComplete.IsCanceled: true }))
            {
                try
                {
                    checkIncompleteTask.AwaitingComplete.GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Logger.Warn("Async message number {0}. Caught {1}", checkIncompleteTask.Value, ex);
                }
                checkIncompleteTask.Value            = -1;
                checkIncompleteTask.AwaitingComplete = ValueTask.CompletedTask;
                if (currLookAheadConCur == incompleteTasksConsumerCursor) incompleteTasksConsumerCursor += 1;
            }
            else
            {
                if (checkIncompleteTask.Value < 0) gapCount++;
            }
        }
        if (gapCount > Math.Min(1000, halfSize)) CompactIncompleteRemaining();
    }

    public void CompactIncompleteRemaining()
    {
        var incrementConsumer = 0;
        for (var slot = incompleteTasksPublishCursor - 1; slot >= incompleteTasksConsumerCursor; slot--)
        {
            var slotTask = incompleteTasks[slot & readAheadRingMask];
            if (slotTask is { Value: >= 0, AwaitingComplete.IsCompleted: false }) continue;
            for (var checkToMove = slot - 1; checkToMove >= incompleteTasksConsumerCursor; checkToMove--)
            {
                var checkTask = incompleteTasks[checkToMove & readAheadRingMask];
                if (checkTask is { Value: >= 0, AwaitingComplete.IsCompleted: false })
                {
                    slotTask.Value            = checkTask.Value;
                    slotTask.AwaitingComplete = checkTask.AwaitingComplete;
                    incrementConsumer++;
                    checkTask.Value            = -1;
                    checkTask.AwaitingComplete = ValueTask.CompletedTask;
                    break;
                }
                if (checkToMove == incompleteTasksConsumerCursor)
                {
                    incompleteTasksConsumerCursor += incrementConsumer;
                    return;
                }
            }
        }
        incompleteTasksConsumerCursor += incrementConsumer;
    }

    private class AwaitingCompleteContainer
    {
        public ValueTask AwaitingComplete;
        public long      Value;
    }
}
