namespace FortitudeCommon.Types;

public interface ITracksChanges<in T> : IStoreState<T> where T : class
{
    bool HasUpdates { get; set; }
}
