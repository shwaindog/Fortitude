namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public interface ITimerCallbackSubscription
    {
        bool Unregister(IIntraOSThreadSignal registeredSignalWhenSubscribed);
    }
}
