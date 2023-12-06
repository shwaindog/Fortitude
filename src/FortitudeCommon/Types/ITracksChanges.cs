namespace FortitudeCommon.Types;

public interface ITracksChanges<T> : IStoreState<T> where T : class
{
    bool HasUpdates { get; set; }
}
