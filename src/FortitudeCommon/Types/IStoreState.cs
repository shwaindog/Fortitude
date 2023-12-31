﻿namespace FortitudeCommon.Types;

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
    , CopyNonUpdated = 0x01
    , CopyUpdated = 0x02
    , NoAppendLookupValues = 0x04
}
