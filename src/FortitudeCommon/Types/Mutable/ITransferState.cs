// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types.Mutable;

public interface ITransferState
{
    ITransferState CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags);
}

public interface ITransferState<T> : ITransferState where T : class
{
    T CopyFrom(T source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);
}

[Flags]
public enum CopyMergeFlags : byte
{
    Default              = 0x00
  , FullReplace          = 0x01
  , JustDifferences      = 0x02
  , AppendMissing        = 0x04
  , RemoveUnmatched      = 0x08
  , SkipReferenceLookups = 0x10
  , UpdateFlagsNone      = 0x20
  , UpdateFlagsAll       = 0x40
  , AsNew                = 0x80
}

public static class CopyMergeFlagExtensions
{
    public static bool IsDefault(this CopyMergeFlags check)               => check == CopyMergeFlags.Default;
    public static bool HasFullReplace(this CopyMergeFlags check)          => (check & CopyMergeFlags.FullReplace) > 0;
    public static bool HasJustDifferences(this CopyMergeFlags check)      => (check & CopyMergeFlags.JustDifferences) > 0;
    public static bool HasAppendMissing(this CopyMergeFlags check)        => (check & CopyMergeFlags.AppendMissing) > 0;
    public static bool HasRemoveUnmatched(this CopyMergeFlags check)      => (check & CopyMergeFlags.RemoveUnmatched) > 0;
    public static bool HasSkipReferenceLookups(this CopyMergeFlags check) => (check & CopyMergeFlags.SkipReferenceLookups) > 0;
    public static bool HasUpdateFlagsNone(this CopyMergeFlags check)      => (check & CopyMergeFlags.UpdateFlagsNone) > 0;
    public static bool HasUpdateFlagsAll(this CopyMergeFlags check)       => (check & CopyMergeFlags.UpdateFlagsAll) > 0;
    public static bool HasAsNew(this CopyMergeFlags check)                => (check & CopyMergeFlags.AsNew) > 0;

    public static CopyMergeFlags AddSkipReferenceLookups(this CopyMergeFlags original) => original | CopyMergeFlags.RemoveUnmatched;
}
