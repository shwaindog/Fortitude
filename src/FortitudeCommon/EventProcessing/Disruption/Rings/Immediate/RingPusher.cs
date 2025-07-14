using FortitudeCommon.EventProcessing.Disruption.Consuming;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Immediate;

internal class RingPusher<T> where T : class
{
    private readonly IRingBarrier       barrier;
    private readonly IRingConsumer<T>[] consumers;
    private readonly PushingRing<T>     ringBuffer;
    public readonly  Sequence           Sequence;

    private PaddedVolatileFlag running = new(false);

    public RingPusher(PushingRing<T> ringBuffer, IRingBarrier barrier, IRingConsumer<T>[] consumers)
    {
        this.ringBuffer = ringBuffer;
        this.consumers  = consumers;
        this.barrier    = barrier;
        var ringMask = ringBuffer.Length - 1;

        Sequence = new Sequence(ringBuffer.Cursor.Value, ringMask);
    }

    public bool Running => running.IsSet();

    public void Start()
    {
        running.Set();
        var nextSequence = Sequence.Value + 1;
        while (running.IsSet())
        {
            var availableSequence = barrier.WaitFor(nextSequence);
            var batchSize         = availableSequence - nextSequence + 1;

            for (var i = 0; i < consumers.Length; i++)
            {
                var consumer = consumers[i];
                for (var cursor = nextSequence; cursor <= availableSequence; cursor++)
                {
                    consumer.OnNext(cursor, batchSize, ringBuffer[cursor],
                                    cursor == nextSequence, cursor == availableSequence);
                }
            }

            nextSequence = (Sequence.Value = availableSequence) + 1;
        }
        for (var i = 0; i < consumers.Length; i++)
        {
            consumers[i].OnCompleted();
        }
    }

    public void Stop()
    {
        running.Clear();
    }
}

internal class RingPusherLong<T>(PushingRingLong<T> ringBuffer, IRingBarrierLong barrier, IRingConsumer<T>[] consumers)
    where T : class
{
    public readonly SequenceLong Sequence = new(ringBuffer.Cursor.Value);

    private PaddedVolatileFlag running = new(false);

    public bool Running => running.IsSet();

    public void Start()
    {
        running.Set();
        var nextSequence = Sequence.Value + 1;
        while (running.IsSet())
        {
            var availableSequence = barrier.WaitFor(nextSequence);
            var batchSize         = (int)(availableSequence - nextSequence + 1);

            for (var i = 0; i < consumers.Length; i++)
            {
                var consumer = consumers[i];
                for (var cursor = nextSequence; cursor <= availableSequence; cursor++)
                {
                    consumer.OnNext(cursor, batchSize, ringBuffer[cursor],
                                    cursor == nextSequence, cursor == availableSequence);
                }
            }

            nextSequence = (Sequence.Value = availableSequence) + 1;
        }
        for (var i = 0; i < consumers.Length; i++)
        {
            consumers[i].OnCompleted();
        }
    }

    public void Stop()
    {
        running.Clear();
    }
}
