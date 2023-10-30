using System;
using System.Threading;

namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public class TimerCallbackSubscription : ITimerCallbackSubscription
    {
        private readonly RegisteredWaitHandle registeredWaitHandle;

        public TimerCallbackSubscription(RegisteredWaitHandle registeredWaitHandle)
        {
            this.registeredWaitHandle = registeredWaitHandle;
        }

        public bool Unregister(IIntraOSThreadSignal registeredSignalWhenSubscribed)
        {
            if(!(registeredSignalWhenSubscribed is IntraOSThreadSignal interOsSignal)) 
                throw new ArgumentException("Expected registeredSignalWhenSubscribed to be InterOSThreadSignal");
            return registeredWaitHandle.Unregister(interOsSignal.EventWaitHandler);
        }
    }
}