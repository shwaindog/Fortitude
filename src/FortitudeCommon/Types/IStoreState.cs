namespace FortitudeCommon.Types;

public interface IStoreState
{
    void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

public interface IStoreState<in T> : IStoreState where T : class
{
    void CopyFrom(T source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

[Flags]
public enum CopyMergeFlags : byte
{
    Default = 0x00
    , CopyNonUpdated = 0x01
    , CopyUpdated = 0x02
    , NoAppendLookupValues = 0x04
}
