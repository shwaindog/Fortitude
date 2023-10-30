namespace FortitudeCommon.AsyncProcessing
{
    public interface ISequencer
    {
        long Claim();
        void Serialize(long waitFor);
        void Release(long completedAt);
    }
}