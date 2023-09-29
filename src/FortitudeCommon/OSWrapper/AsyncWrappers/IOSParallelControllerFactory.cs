namespace FortitudeCommon.OSWrapper.AsyncWrappers
{
    public interface IOSParallelControllerFactory
    {
        IOSParallelController GetOSParallelController { get; }
    }
}