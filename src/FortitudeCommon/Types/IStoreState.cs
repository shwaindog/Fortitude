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

public static class CopyMergeFlagExtensions
{
    public static bool IsDefault(this CopyMergeFlags check) => check == CopyMergeFlags.Default;
    public static bool HasFullReplace(this CopyMergeFlags check) => (check & CopyMergeFlags.FullReplace) > 0;
    public static bool HasJustDifferences(this CopyMergeFlags check) => (check & CopyMergeFlags.JustDifferences) > 0;
    public static bool HasAppendMissing(this CopyMergeFlags check) => (check & CopyMergeFlags.AppendMissing) > 0;
    public static bool HasRemoveUnmatched(this CopyMergeFlags check) => (check & CopyMergeFlags.RemoveUnmatched) > 0;
}
