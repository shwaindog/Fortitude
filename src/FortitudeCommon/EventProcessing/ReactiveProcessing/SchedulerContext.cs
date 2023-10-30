using System.Reactive.Concurrency;

namespace FortitudeCommon.EventProcessing.ReactiveProcessing
{
    public class SchedulerContext
    {
        public SchedulerContext()
        {
            Dispatcher = CurrentThreadScheduler.Instance;
            CurrentThread = Scheduler.CurrentThread;
            NewThread = NewThreadScheduler.Default;
            ThreadPool = ThreadPoolScheduler.Instance;
        }

        public SchedulerContext(IScheduler dispatcher, IScheduler currentThread, IScheduler newThread, IScheduler threadPool)
        {
            Dispatcher = dispatcher;
            CurrentThread = currentThread;
            NewThread = newThread;
            ThreadPool = threadPool;
        }

        public IScheduler Dispatcher { get; private set; }
        public IScheduler CurrentThread { get; private set; }
        public IScheduler NewThread { get; private set; }
        public IScheduler ThreadPool { get; private set; }
    }
}
