namespace FortitudeCommon.Types
{
    public interface ITracksChanges<in T> : IStoreState<T>
    {
        bool HasUpdates { get; set; }
    }
}
