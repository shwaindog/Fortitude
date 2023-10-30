using System.Threading;

namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public interface IOSParallelController
    {
        IOSThread CreateNewOSThread(ThreadStart threadStart);
        IOSThread CreateNewOSThread(ParameterizedThreadStart threadStart);
        IOSThread CreateNewOSThread(ThreadStart threadStart, int maxThreadSize);
        IOSThread CreateNewOSThread(ParameterizedThreadStart threadStart, int maxThreadSize);
        void Yield();
        IOSThread CurrentOSThread { get; }
        IIntraOSThreadSignal SingleOSThreadActivateSignal(bool initialeState);
        IIntraOSThreadSignal AllWaitingOSThreadActivateSignal(bool initialeState);
        void Schedule(WaitOrTimerCallback callback, object state, uint callInMs, bool callOnce = true);
        void Schedule(WaitOrTimerCallback callback, uint callInMs, bool callOnce = true);
        IIntraOSThreadSignal ScheduleWithEarlyTrigger(WaitOrTimerCallback callback, object state, uint callInMs, 
            bool callOnce = true);
        IIntraOSThreadSignal ScheduleWithEarlyTrigger(WaitOrTimerCallback callback, uint callInMs, 
            bool callOnce = true);

        ITimerCallbackSubscription ScheduleWithEarlyTrigger(IIntraOSThreadSignal earlyTrigger, 
            WaitOrTimerCallback callback, uint callInMs, bool callOnce = true);
        void Sleep(int millseconds);
        void CallFromThreadPool(WaitCallback workItem);
    }
}