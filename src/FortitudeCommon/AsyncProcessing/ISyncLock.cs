namespace FortitudeCommon.AsyncProcessing
{
    public interface ISyncLock
    {
        void Acquire();
        void Release();
    }
}