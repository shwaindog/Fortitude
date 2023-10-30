using System.Threading;

namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public class IntraOSThreadSignal : IIntraOSThreadSignal
    {
        internal readonly EventWaitHandle EventWaitHandler;

        public IntraOSThreadSignal(EventWaitHandle eventWaitHandler)
        {
            EventWaitHandler = eventWaitHandler;
        }

        public void Dispose()
        {
            EventWaitHandler.Dispose();
        }

        public bool Reset()
        {
            return EventWaitHandler.Reset();
        }
        
        public bool Set()
        {
            return EventWaitHandler.Set();
        }

        public bool WaitOne()
        {
            return EventWaitHandler.WaitOne();
        }

        public bool WaitOne(int timeout)
        {
            return EventWaitHandler.WaitOne(timeout);
        }
    }
}
