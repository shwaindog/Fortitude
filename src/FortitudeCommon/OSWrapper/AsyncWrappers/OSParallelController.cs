namespace FortitudeCommon.OSWrapper.AsyncWrappers;

public class OSParallelController : IOSParallelController
{
    private readonly AutoResetEvent NeverTriggered = new(false);

    public IOSThread CreateNewOSThread(ThreadStart threadStart) => new OSThread(new Thread(threadStart));

    public IOSThread CreateNewOSThread(ParameterizedThreadStart threadStart) => new OSThread(new Thread(threadStart));

    public IOSThread CreateNewOSThread(ThreadStart threadStart, int maxThreadSize) =>
        new OSThread(new Thread(threadStart, maxThreadSize));

    public IOSThread CreateNewOSThread(ParameterizedThreadStart threadStart, int maxThreadSize) =>
        new OSThread(new Thread(threadStart, maxThreadSize));

    public void Yield()
    {
        Thread.Yield();
    }

    public IOSThread CurrentOSThread => new OSThread(Thread.CurrentThread);

    public IIntraOSThreadSignal SingleOSThreadActivateSignal(bool initialeState) =>
        new IntraOSThreadSignal(new AutoResetEvent(initialeState));

    public IIntraOSThreadSignal AllWaitingOSThreadActivateSignal(bool initialeState) =>
        new IntraOSThreadSignal(new ManualResetEvent(initialeState));

    public void Sleep(int millsecondsTimeout)
    {
        Thread.Sleep(millsecondsTimeout);
    }

    public void Schedule(WaitOrTimerCallback callback, object state, uint callInMs, bool callOnce = true)
    {
        ThreadPool.RegisterWaitForSingleObject(NeverTriggered, callback, state, callInMs, callOnce);
    }

    public void Schedule(WaitOrTimerCallback callback, uint callInMs, bool callOnce = true)
    {
        ThreadPool.RegisterWaitForSingleObject(NeverTriggered, callback, null, callInMs, callOnce);
    }

    public IIntraOSThreadSignal ScheduleWithEarlyTrigger(WaitOrTimerCallback callback, object state, uint callInMs,
        bool callOnce = true)
    {
        var autoResetEvent = new AutoResetEvent(false);
        var trigger = new IntraOSThreadSignal(autoResetEvent);
        ThreadPool.RegisterWaitForSingleObject(autoResetEvent, callback, state, callInMs, callOnce);
        return trigger;
    }

    public IIntraOSThreadSignal ScheduleWithEarlyTrigger(WaitOrTimerCallback callback, uint callInMs,
        bool callOnce = true)
    {
        var autoResetEvent = new AutoResetEvent(false);
        var trigger = new IntraOSThreadSignal(autoResetEvent);
        ThreadPool.RegisterWaitForSingleObject(autoResetEvent, callback, null, callInMs, callOnce);
        return trigger;
    }

    public ITimerCallbackSubscription ScheduleWithEarlyTrigger(IIntraOSThreadSignal earlyTrigger,
        WaitOrTimerCallback callback, uint callInMs, bool callOnce = true)
    {
        if (!(earlyTrigger is IntraOSThreadSignal extractWaitHandle))
            throw new ArgumentException("Expected earlyTrigger to be an InterOSThreadSignal");
        var registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(extractWaitHandle.EventWaitHandler,
            callback, null, callInMs, callOnce);

        return new TimerCallbackSubscription(registeredWaitHandle);
    }

    public void CallFromThreadPool(WaitCallback workItem)
    {
        ThreadPool.QueueUserWorkItem(workItem);
    }
}
