namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public interface IIntraOSThreadSignal
    {
        void Dispose();
        bool Reset();
        bool Set();
        bool WaitOne();
        bool WaitOne(int timeoutMs);
    }
}