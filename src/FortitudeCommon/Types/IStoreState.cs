namespace FortitudeCommon.Types;

public interface IStoreState
{
    IStoreState CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags);
}

public interface IStoreState<T> : IStoreState where T : class
{
    T CopyFrom(T source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

[Flags]
public enum CopyMergeFlags : byte
{
    Default = 0x00
    , FullReplace = 0x01
    , JustDifferences = 0x02
    , AppendMissing = 0x04
    , RemoveUnmatched = 0x08
}
