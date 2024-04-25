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
    ValueTaskProcessEvent<T> ProcessEvent { get; set; }

    int Size { get; }
    ValueTask<long> Poll();

    event Action<QueueEventTime> QueueEntryStart;
    event Action<QueueEventTime> QueueEntryComplete;
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
    private readonly int readAheadRingMask;
    private readonly int ringMask;
    private long completedReadAheadConsumerCursor;
    private long completedReadAheadPublishCursor;
    private long readAheadCursor = -1;

    public AsyncValueValueTaskPollingRing(string name, int size, Func<T> dataFactory
        , ClaimStrategyType claimStrategyType, ValueTaskProcessEvent<T>? processAsyncTask = null, bool logErrors = true)
    {
        ProcessEvent = processAsyncTask ?? ((currentSequence, _) =>
        {
            Logger.Warn("Poll Sink not set on AsyncTaskPollingRing.  Will do nothing");
            return new ValueTask<long>(currentSequence);
        });
        InterceptHandler = _ => false;
        Name = name;
        Size = size;
        var ringSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        var readAheadRingSize = MemoryUtils.CeilingNextPowerOfTwo(size);
        ringMask = ringSize - 1;
        readAheadRingMask = readAheadRingSize - 1;
        cells = new T[ringSize];
        completeAhead = new TaskCompleteSequenceContainer[readAheadRingSize];
        conCursors = new[] { conCursor };

        claimStrategy = claimStrategyType.GetInstance(name, ringSize, logErrors);

        for (var i = 0; i < cells.Length; i++) cells[i] = dataFactory();
        for (var i = 0; i < completeAhead.Length; i++) completeAhead[i] = new TaskCompleteSequenceContainer();
    }

    public T this[long sequence] => cells[(int)sequence & ringMask];

    public int Size { get; }

    public string Name { get; }

    public ValueTaskProcessEvent<T> ProcessEvent { get; set; }

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
                QueueEntryStart?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
                if (datum.IsTaskCallbackItem)
                {
                    datum.InvokeTaskCallback();
                    QueueEntryComplete?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
                    CheckConsumerShouldMoveForward(currentSequence);
                    return currentSequence;
                }

                if (InterceptHandler(datum))
                {
                    QueueEntryComplete?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
                    CheckConsumerShouldMoveForward(currentSequence);
                    return currentSequence;
                }

                var completedSequenceId = await ProcessEvent(currentSequence, datum);
                QueueEntryComplete?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
                CheckConsumerShouldMoveForward(completedSequenceId);
                return completedSequenceId;
            }
            catch (Exception ex)
            {
                QueueEntryComplete?.Invoke(new QueueEventTime(currentSequence, DateTime.UtcNow));
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
        var entry = this[seqId & ringMask];
        entry.SetAsTaskCallbackItem(d, state);
        Publish(seqId);
    }

    private void EnqueueCompleteAhead(long completed)
    {
        var seqId = completedReadAheadPublishCursor++;
        var evt = completeAhead[seqId & readAheadRingMask];
        evt.Value = completed;
    }

    private void CheckConsumerShouldMoveForward(long completedSequenceId)
    {
        if (conCursor.Value == completedSequenceId - 1)
        {
            // Logger.Debug("NAME: {0} In order - ring[{1}]={2}", Name, completedSequenceId, this[completedSequenceId & ringMask]);
            conCursor.Value = completedSequenceId;
            UpdateConCursorWithCompleteReadAheadItems(completedReadAheadConsumerCursor, completedReadAheadPublishCursor);
        }
        else
        {
            // Logger.Debug("NAME: {0} out of order - ring[{1}]={2}", Name, completedSequenceId, this[completedSequenceId & ringMask]);
            EnqueueCompleteAhead(completedSequenceId);
        }
    }

    public void UpdateConCursorWithCompleteReadAheadItems(long lookAheadStart, long lookAheadMax)
    {
        for (var currLookAheadConCur = lookAheadStart; currLookAheadConCur < lookAheadMax; currLookAheadConCur++)
        {
            var checkLookAhead = completeAhead[currLookAheadConCur & readAheadRingMask].Value;
            if (conCursor.Value == checkLookAhead - 1)
            {
                // Logger.Debug("NAME: {0} play forward - ring[{1}]={2}", Name, checkLookAhead, this[checkLookAhead & ringMask]);
                conCursor.Value = checkLookAhead;
                completedReadAheadConsumerCursor = currLookAheadConCur;
            }
            else if (checkLookAhead > conCursor.Value)
            {
                // Logger.Debug("NAME: {0} not next completed - ring[{1}]={2}", Name, checkLookAhead, this[checkLookAhead & ringMask]);
                if (!CheckFurtherBackCompleteReadAheadCompleteFound(currLookAheadConCur + 1, lookAheadMax)) break;
                currLookAheadConCur--;
            }
        }
    }

    public bool CheckFurtherBackCompleteReadAheadCompleteFound(long lookAheadStart, long lookAheadMax)
    {
        for (var currLookAheadConCur = lookAheadStart; currLookAheadConCur < lookAheadMax; currLookAheadConCur++)
        {
            var checkLookAhead = completeAhead[currLookAheadConCur & readAheadRingMask].Value;
            if (conCursor.Value == checkLookAhead - 1)
            {
                // Logger.Debug("NAME: {0} found further done completed - ring[{1}]={2}", Name, checkLookAhead, this[checkLookAhead & ringMask]);
                conCursor.Value = checkLookAhead;
                return true;
            }
        }

        return false;
    }

    private class TaskCompleteSequenceContainer
    {
        public long Value;
    }
}
