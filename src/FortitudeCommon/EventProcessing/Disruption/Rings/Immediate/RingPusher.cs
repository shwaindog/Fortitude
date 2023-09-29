using FortitudeCommon.EventProcessing.Disruption.Consuming;
using FortitudeCommon.EventProcessing.Disruption.Sequences;
using FortitudeCommon.EventProcessing.Disruption.Waiting;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Immediate
{
    internal class RingPusher<T> where T : class
    {
        private readonly IRingBarrier barrier;
        private readonly IRingConsumer<T>[] consumers;
        private readonly PushingRing<T> ringBuffer;
        public readonly Sequence Sequence;
        private PaddedVolatileFlag running = new PaddedVolatileFlag(false);

        public RingPusher(PushingRing<T> ringBuffer, IRingBarrier barrier, IRingConsumer<T>[] consumers)
        {
            this.ringBuffer = ringBuffer;
            this.consumers = consumers;
            this.barrier = barrier;
            Sequence = new Sequence(ringBuffer.Cursor.Value);
        }

        public bool Running
        {
            get { return running.IsSet(); }
        }

        public void Start()
        {
            running.Set();
            var nextSequence = Sequence.Value + 1;
            while (running.IsSet())
            {
                var availableSequence = barrier.WaitFor(nextSequence);
                var batchSize = availableSequence - nextSequence + 1;

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
}