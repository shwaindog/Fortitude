using System;
using System.Collections.Generic;

namespace FortitudeCommon.EventProcessing.Disruption.Consuming
{
    public class NoopRingConsumer<T> : IRingConsumer<T> where T : class
    {
        public void OnNext(long sequence, int batchSize, T data, bool startOfBatch, bool endOfBatch)
        {
        }

        public void OnNext(IEnumerable<T> batch)
        {
        }

        public void OnNext(T value)
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }
    }
}