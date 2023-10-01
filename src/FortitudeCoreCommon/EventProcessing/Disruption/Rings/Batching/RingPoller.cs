using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace FortitudeCommon.EventProcessing.Disruption.Rings.Batching
{
    public abstract class RingPoller<T> : IDisposable where T : class
    {
        private readonly AutoResetEvent are = new AutoResetEvent(true);
        protected readonly PollingRing<T> Ring;
        private readonly Thread thread;
        private readonly int timeoutMs;
        private volatile bool running = true;
        private ISubject<T> eventSubject = new Subject<T>();

        protected RingPoller(PollingRing<T> ring, uint timeoutMs)
        {
            Ring = ring;
            this.timeoutMs = (int) timeoutMs;

            thread = new Thread(delegate()
            {
                while (running)
                {
                    are.WaitOne(this.timeoutMs);
                    foreach (var data in Ring)
                    {
                        eventSubject.OnNext(data);
                        Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
                    }
                }
            });
            thread.IsBackground = true;
            thread.Name = ring.Name + "Poller";
            thread.Start();
        }

        public void Dispose()
        {
            running = false;
            are.Set();
            thread.Join();
            foreach (var data in Ring)
            {
                eventSubject.OnNext(data);
                Processor(Ring.CurrentSequence, Ring.CurrentBatchSize, data, Ring.StartOfBatch, Ring.EndOfBatch);
            }
        }

        private IObservable<T> Events => eventSubject.AsObservable();

        protected virtual void Processor(long sequence, long batchSize, T data, bool startOfBatch, bool endOfBatch){}
    }
}