using System;
using System.Collections.Generic;

namespace FortitudeCommon.EventProcessing.Disruption.Consuming
{
    public interface IRingConsumer<T> : IObserver<T> where T : class
    {
        void OnNext(long sequence, long batchSize, T data, bool startOfBatch, bool endOfBatch);
        void OnNext(IEnumerable<T> batch);
    }
}